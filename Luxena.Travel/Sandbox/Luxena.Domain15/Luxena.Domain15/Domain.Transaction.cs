using System;
using System.Collections.Generic;
using System.Linq;


namespace Luxena.Domain
{

	partial class Domain<TDomain, TEntity, TKey>
	{

		public class Transaction : IDisposable
		{

			public TDomain db;

			public readonly IDictionary<TEntity, Entry> EntriesByEntity = new Dictionary<TEntity, Entry>();

			public List<Entry> Entries { get; private set; }


			public Action Calculated;


			public Transaction(TDomain db)
			{
				this.db = db;
				db.Transactions.Push(this);

				foreach (var entity in db._oldEntitiesByEntity.Keys.ToList())
				{
					db._oldEntitiesByEntity[entity] = entity.Clone(db);
				}
			}

			public void Dispose()
			{
				Commit();
			}


			private bool _entityIsModified;
			private int _waveIndex = -1;


			public Entry Save(TEntity entity, bool active, bool isDeleted)
			{
				if (entity._original != null)
					entity = entity._original;


				#region Зарегистрировать Entry для текущего Entity

				bool isModified;

				var entry = EntriesByEntity.By(entity);
				if (entry == null)
				{
					entry = new Entry(this, entity) { IsDeleted = isDeleted, Active = active };
					EntriesByEntity.Add(entity, entry);

					isModified = true;
				}
				else
					isModified = active && !entry.Active || isDeleted && !entry.IsDeleted || active && entry.WaveIndex < _waveIndex + 1;

				_entityIsModified |= isModified;

				if (isModified)
					entry.WaveIndex = _waveIndex + 1;
				else
					return entry;

				entry.IsDeleted |= isDeleted;
				entry.Active = true;

				entity.db = db;

				if (entry.IsDeleted)
				{
					entity._new = entity.EmptyClone();
					entity._old = entity;
				}
				else
				{
					entity._new = entity;
					entity._old = entry.OldEntity;
				}

				#endregion


				#region Сохранить все мастера (новые и старые), и прилепиться к последнему

				var newMasterEntities = entity._new.GetDependents();
				var oldMasterEntities = entity._old.GetDependents();

				if (newMasterEntities.Yes() || oldMasterEntities.Yes())
				{
					Entry masterEntry = null;

					foreach (var master in oldMasterEntities.AsConcat(newMasterEntities).Distinct())
					{
						if (master != null)
							masterEntry = Save(master, false, false);
					}

					if (masterEntry != null && entry.Master != masterEntry)
					{
						entry.Master = masterEntry;
					}
				}

				#endregion


//				#region Проверить, не является ли текущий Entity для кого-то мастером
//
//				foreach (var childEntry in EntriesByEntity.Values.Where(a => a.Entity != entity && a.Master == null))
//				{
//					newMasterEntities = childEntry.Entity.GetDependents();
//					if (newMasterEntities.Yes() && newMasterEntities.Contains(entity))
//					{
//						childEntry.Master = entry;
//					}
//				}
//
//				#endregion

				return entry;
			}


			private void Commit()
			{

				//Entries = EntriesByEntity.Values.Where(a => a.Master == null).ToList();

				ReorderEntries();
				_entityIsModified = true;

				while (_entityIsModified)
				{
					_entityIsModified = false;
					_waveIndex++;

					Binding(Entries);
					ReorderEntries();
				} 

				Calculate(Entries);

				if (Calculated != null)
					Calculated();

				db.Transactions.Pop();

				if (db.Transactions.Count == 0)
					Flush(Entries);
			}

			private void ReorderEntries()
			{
				foreach (var entry in EntriesByEntity.Values)
				{
					//if (entry.Master != null) continue;

					foreach (var master in entry.Entity.GetDependents().Sure().Reverse())
					{
						if (master == null) continue;

						var masterEntry = EntriesByEntity.By(master);
						if (masterEntry == null) continue;

						entry.Master = masterEntry;
						//_entityIsModified = true;
						break;
					}
				}

				Entries = EntriesByEntity.Values.Where(a => a.Master == null).ToList();
			}


			public void Binding(List<Entry> entries)
			{
				foreach (var entry in entries.ToList())
				{
					if (entry.Active && entry.WaveIndex == _waveIndex)
					{
						var entity = entry.Entity;

						if (entity._OnPreCalculate != null)
							entity._OnPreCalculate(entity);

						entity.Bind();
					}


					Binding(entry.Items);
				}
			}

			public void Calculate(List<Entry> nodes)
			{
				foreach (var node in nodes)
				{
					Calculate(node.Items);

					if (node.Active && !node.IsDeleted)
						node.Entity.Calculate();
				}
			}


			public void Flush(List<Entry> entries)
			{
				foreach (var entry in entries)
				{
					if (entry.Active)
						entry.Entity.Flush(entry.IsDeleted);

					Flush(entry.Items);
				}
			}


			public class Entry
			{
				public Entry(Transaction tran, TEntity entity)
				{
					Entity = entity;
					OldEntity = tran.db.Old(entity);
				}

				public TEntity Entity;
				public TEntity OldEntity;
				public bool Active;
				public bool IsDeleted;


				public Entry Master
				{
					get { return _master; }
					set
					{
						if (_master == value) return;

						if (_master != null)
							_master.Items.Remove(this);

						_master = value;

						if (_master != null)
							_master.Items.Add(this);
					}
				}

				public readonly List<Entry> Items = new List<Entry>();
				public int WaveIndex;
				private Entry _master;


				public override string ToString()
				{
					return
						WaveIndex + " - " +
						"{" + Entity + "}" +
						(Items.Count > 0 ? " Items: " + Items.Count : null) +
						(IsDeleted ? " DELETE" : null) +
						(!Active ? " NOT ACTIVE" : null);
				}

				public string Map
				{
					get
					{
						var map = ToString() + "\r\n";

						if (Items.Yes())
							map += "\t" + string.Join("", Items.Select(a => a.Map)).Replace("\r\n", "\r\n\t").TrimEnd('\t');

						return map;
					}
				}

			}

		}

	}


}
