using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace Luxena.Domain
{

	partial class Domain<TDomain>
	{

		public readonly Stack<Transaction> Transactions = new Stack<Transaction>();

		public Transaction CurrentTransaction
		{
			[DebuggerStepThrough]
			get
			{
				if (Transactions.No())
					throw new Exception("Domain: нужно запустить транзанцию (выполнять операцию сохранения используя в db.Commit(...))");
				return Transactions.Peek();
			}
		}

		public List<Transaction.Entry> Entries {[DebuggerStepThrough] get { return CurrentTransaction.RootEntries; } }

		public event Action OnCalculated;

		//public Action Calculated
		//{
		//	[DebuggerStepThrough]
		//	get { return CurrentTransaction.Calculated; }

		//	[DebuggerStepThrough]
		//	set { CurrentTransaction.Calculated = value; }
		//}

		public string EntryMap
		{
			get
			{
				var tran = CurrentTransaction;

				return tran.Entries.No() ? null :
					string.Join("", tran.Entries.Where(a => a.Master == null).Select(a => a.Map));
			}
		}


		[DebuggerStepThrough]
		public Transaction BeginWork()
		{
			return new Transaction((TDomain)this);
		}

		[DebuggerStepThrough]
		public void Commit(Action action, bool useFlush = true)
		{
			using (var tran = BeginWork())
			{
				tran.FlushOnCommit = useFlush;
				action();
				tran.Commit();
			}
		}

		[DebuggerStepThrough]
		public Task<int> CommitAsync(Action action)
		{
			using (var tran = BeginWork())
			{
				action();
				return tran.CommitAsync();
			}
		}


		private readonly List<Transaction.Entry> _flushingEntries = new List<Transaction.Entry>();
		private int _maxLevel;

		private void Flush()
		{
			if (FlushEntries())
				SaveChanges();
		}

		private Task<int> FlushAsync()
		{
			return FlushEntries() ? SaveChangesAsync() : null;
		}

		private bool FlushEntries()
		{
			if (_flushingEntries.Any(a => !a.Entity.IsSaving()))
			{
				_flushingEntries.Clear();
				return false;
			}

			for (var level = 0; level <= _maxLevel; level++)
			{
				foreach (var entry in _flushingEntries)
				{
					if (entry.Level != level || !entry.Active) continue;

					entry.Entity.Flush(entry.IsDeleted);
				}
			}

			_flushingEntries.Clear();

			return true;
		}


		public class Transaction : IDisposable
		{

			public readonly TDomain db;

			public readonly List<Entry> Entries = new List<Entry>();

			public List<Entry> RootEntries { get; private set; }

			public bool FlushOnCommit = true;


			public Transaction(TDomain db)
			{
				this.db = db;
				db.Transactions.Push(this);
			}

			public void Dispose()
			{
				db.Transactions.Pop();
				Entries.Clear();
			}



			private bool _entityIsModified;
			private int _waveIndex = -1;
			private int _maxLevel = -1;


			public Entry Save(Entity entity, bool isDeleted = false, bool active = true)
			{
				if (entity == null) return null;

				if (entity._cloneSource != null)
					entity = entity._cloneSource;

				entity.Domain(db);
				entity._isSaving = FlushOnCommit && entity.GetIsSaving();

				#region Зарегистрировать Entry для текущего Entity

				bool isModified;

				var entry = Entries.By(a => Equals(a.Entity, entity));
				if (entry == null)
				{
					entry = new Entry(entity) { IsDeleted = isDeleted, Active = active };
					Entries.Add(entry);

					if (FlushOnCommit)
						db._flushingEntries.Add(entry);

					isModified = true;
				}
				else
					isModified =
						active && !entry.Active ||
						isDeleted && !entry.IsDeleted ||
						active && !entry.IsBinding && entry.WaveIndex < _waveIndex + 1;

				_entityIsModified |= isModified;

				if (isModified)
					entry.WaveIndex = _waveIndex + 1;
				else
					return entry;

				entry.IsDeleted |= isDeleted;
				entry.Active = true;

				if (entry.IsDeleted)
				{
					entity.SetOld(entity);
					//entity.SetOld(entity.GetOld() ?? entity);
					entity.SetNew(entity.EmptyClone());
				}
				else if (entity.GetOld() == null)
				{
					entity.SetOld(entity.OriginalClone());
					entity.SetNew(entity);
				}

				#endregion


				#region Сохранить все мастера (новые и старые), и прилепиться к последнему

				var newDependents = entity.GetNew().GetDependents();
				var oldDependents = entity.GetOld().GetDependents();

				if (newDependents.Yes() || oldDependents.Yes())
				{
					Entry masterEntry = null;

					foreach (var master in oldDependents.AsConcat(newDependents).Distinct())
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

				return entry;
			}

			[DebuggerStepThrough]
			public void Save(IEnumerable<Entity> entities, bool isDeleted = false)
			{
				if (entities == null) return;

				foreach (var entity in entities)
				{
					if (entity == null) continue;

					Save(entity, isDeleted);
				}
			}


			private void Commit(Action flush)
			{
				Bind();
				Calculate();

				if (db.Transactions.Count != 1) return;

				if (FlushOnCommit)
					flush();

				foreach (var entry in Entries)
				{
					var entity = entry.Entity;
					entity.SetNew(null);
					entity.SetOld(null);
					entity.LastChangedPropertyName = null;
				}
			}

			public void Commit()
			{
				Commit(() => db.Flush());
			}

			public Task<int> CommitAsync()
			{
				Task<int> result = null;
				Commit(() => result = db.FlushAsync());
				return result;
			}


			private void Bind()
			{
				ReorderEntries();

				_entityIsModified = true;

				while (_entityIsModified)
				{
					_entityIsModified = false;
					_waveIndex++;

					Binding(RootEntries);
					ReorderEntries();
				}
			}


			private void ReorderEntries()
			{
				foreach (var entry in Entries)
				{
					foreach (var master in entry.Entity.GetDependents().Sure().Reverse())
					{
						if (master == null) continue;

						// ReSharper disable once AccessToForEachVariableInClosure
						var masterEntry = Entries.By(a => a.Entity == master);
						if (masterEntry == null) continue;

						entry.Master = masterEntry;
						break;
					}
				}

				RootEntries = Entries.Where(a => a.Master == null).ToList();
			}


			public void Binding(List<Entry> entries)
			{
				foreach (var entry in entries.ToList())
				{
					if (entry.Active && entry.WaveIndex == _waveIndex)
					{
						entry.IsBinding = true;
						entry.Entity.Bind();
						entry.IsBinding = false;
					}


					Binding(entry.Items);
				}
			}


			private void Calculate()
			{
				db._maxLevel = 0;

				CalculateLevels(RootEntries);

				//Entries.ForEach(a => a.IsCalculated = false);

				var calculationCount = 0;
				do
				{
					var entries = Entries.ToList();

					for (var level = _maxLevel; level >= 0; level--)
					{
						foreach (var entry in entries)
						{
							if (entry.Level == level && !entry.IsCalculated)
							{
								if (entry.Active && !entry.IsDeleted)
									entry.Entity.Calculate();

								entry.IsCalculated = true;
							}
						}
					}

					calculationCount++;
					if (calculationCount >= 80)
						throw new Exception("Domain.Calculate: слишком большой цикл");
				}
				while (Entries.Any(a => !a.IsCalculated));

				db.OnCalculated?.Invoke();
			}

			public void CalculateLevels(List<Entry> entries, int level = 0)
			{
				if (_maxLevel < level)
					_maxLevel = level;
				if (db._maxLevel < level)
					db._maxLevel = level;

				foreach (var entry in entries)
				{
					entry.Level = level;

					if (entry.Items.Yes())
						CalculateLevels(entry.Items, level + 1);
				}
			}


			public class Entry
			{
				public Entry(Entity entity)
				{
					Entity = entity;
				}

				// ReSharper disable once MemberHidesStaticFromOuterClass
				public Entity Entity;
				public bool Active;
				public bool IsDeleted, IsBinding, IsCalculated;


				public Entry Master
				{
					get { return _master; }
					set
					{
						if (_master == value) return;

						_master?.Items.Remove(this);
						_master = value;
						_master?.Items.Add(this);
					}
				}

				public readonly List<Entry> Items = new List<Entry>();
				public int WaveIndex;
				public int Level;
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
