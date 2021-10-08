using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Luxena.Domain
{

	public partial class Domain<TDomain, TEntity, TKey> : IDisposable
		where TDomain : Domain<TDomain, TEntity, TKey>
		where TEntity : Domain<TDomain, TEntity, TKey>.Entity
	{


		public void Dispose()
		{
		}


		#region Transactions

		public readonly Stack<Transaction> Transactions = new Stack<Transaction>();

		public Transaction CurrentTransaction { get { return Transactions.Peek(); } }

		public Action Calculated { get { return CurrentTransaction.Calculated; } set { CurrentTransaction.Calculated = value; } }
		public List<Transaction.Entry> Entries { get { return CurrentTransaction.Entries; } }

		public string EntryMap { get { return string.Join("", Entries.Select(a => a.Map)); } }


		[DebuggerStepThrough]
		public Transaction BeginWork()
		{
			return new Transaction((TDomain)this);
		}

		[DebuggerStepThrough]
		public void Commit(Action action)
		{
			using (BeginWork())
			{
				action();
			}
		}

		#endregion


		readonly Dictionary<TEntity, TEntity> _oldEntitiesByEntity = new Dictionary<TEntity, TEntity>();

		public void SaveToList<TEntity2>(TEntity2 entity, List<TEntity2> list, bool isDeleted)
			where TEntity2: TEntity
		{
			if (isDeleted)
			{
				list.Remove(entity);
				_oldEntitiesByEntity.Remove(entity);
			}
			else if (!list.Contains(entity))
			{
				_oldEntitiesByEntity[entity] = entity.Clone((TDomain)this);
				list.Add(entity);
			}
		}


		public TEntity Old<TEntity2>(TEntity2 current)
			where TEntity2 : TEntity
		{
			return (TEntity2)_oldEntitiesByEntity.By(current) ?? (TEntity2)(_oldEntitiesByEntity[current] = current.EmptyClone());
		}


		


	}

}
