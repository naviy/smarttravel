using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Domain;


namespace Luxena.Domain.Entities
{

	public class EntityEvent
	{
		public readonly object Entity;
		public readonly Queue<Handler> Handlers;

		public EntityEvent(object entity)
		{
			Entity = entity;
			Handlers = new Queue<Handler>();
		}

		public bool Equals(IEntity entity)
		{
			return Equals(Entity, entity);
		}

		public void Add<TEntity>(Action<TEntity> keyAction, Action<TEntity> action)
			where TEntity : class
		{
			object actionKey = keyAction;
			if (!Handlers.Any(a => Equals(a.ActionKey, actionKey)))
				Handlers.Enqueue(new Handler(actionKey, r => (action ?? keyAction)((TEntity)r)));
		}

		public bool Exec()
		{
			var action = Handlers.Dequeue().Action;
			action(Entity);
			return Handlers.Count == 0;
		}


		public class Handler
		{
			public object ActionKey;
			public Action<object> Action;

			public Handler(object actionKey, Action<object> action)
			{
				ActionKey = actionKey;
				Action = action;
			}
		}

	}


	public class EntityEvents : LinkedList<EntityEvent>
	{

		public void Add<TEntity>(DomainBase db, TEntity r, Action<TEntity> keyAction, Action<TEntity> action = null)
			where TEntity : class, IEntity
		{
			if (r == null) throw new ArgumentNullException("r");
			if (keyAction == null) throw new ArgumentNullException("keyAction");

			var r1 = r;
			var @event = this.By(a => a.Equals(r1));

			if (@event == null)
			{
				r = db.Unproxy(r);
				AddLast(@event = new EntityEvent(r));
			}

			@event.Add(keyAction, action);
		}


		public void AddBefore<TEntity>(DomainBase db, IEntity mr, TEntity r, Action<TEntity> keyAction, Action<TEntity> action = null)
			where TEntity : class, IEntity
		{
			if (r == null) throw new ArgumentNullException("r");
			if (keyAction == null) throw new ArgumentNullException("keyAction");

			var masterNode = First;
			while (masterNode != null && !masterNode.Value.Equals(mr))
				masterNode = masterNode.Next;

			var r1 = r;
			var @event = this.By(a => a.Equals(r1));

			if (@event == null)
			{
				r = db.Unproxy(r);
				@event = new EntityEvent(r);

				if (masterNode == null)
					AddLast(@event);
				else
					AddBefore(masterNode, @event);
			}

			@event.Add(keyAction, action);
		}


		public void Exec()
		{
			while (Count > 0)
			{
				var @event = First.Value;
				if (@event.Exec())
					Remove(@event);
			}
		}

	}


}
