using System;
using System.Collections.Generic;


namespace Luxena.Travel.Domain
{

	[RU("Задача", "Задачи")]
	[AgentPrivileges]
	public partial class Task : Entity2, IDomainContainer
	{

		[Patterns.Number, EntityName]
		public virtual string Number { get; set; }

		[RU("Тема")]
		public virtual string Subject { get; set; }

		[Patterns.Description]
		public virtual string Description { get; set; }

		[RU("Относительно")]
		public virtual Party RelatedTo { get; set; }

		public virtual Order Order { get; protected set; }

		public virtual void SetOrder(Order value)
		{
			WrapSetter(() =>
			{
				if (Equals(Order, value))
					return;

				Order?.RemoveTask(this);

				Order = value;

				if (value == null)
					return;

				value.AddTask(this);
			});
		}

		[RU("Ответственный")]
		public virtual Party AssignedTo { get; set; }

		[Patterns.Status]
		public virtual TaskStatus Status { get; set; }

		[RU("Выполнить до")]
		public virtual DateTime? DueDate { get; set; }

		[RU("Просроченный")]
		public virtual bool Overdue => Status != TaskStatus.Closed && DueDate < DateTime.Today;

		public virtual void AddComment(string text)
		{
			if (text.No()) return;

			_comments.Add(new TaskComment
			{
				Task = this,
				Text = text
			});
		}

		public virtual IList<TaskComment> Comments => _comments;

		private void WrapSetter(Action action)
		{
			if (_updating)
				return;

			_updating = true;
			try
			{
				action();
			}
			finally
			{
				_updating = false;
			}
		}

		//public virtual bool CanModify(Domain db)
		//{
		//	return db.CanUpdate(this).Visible;
		//}

		public virtual bool CanModify => db != null && db.CanUpdate(this);

		private bool _updating;

		private readonly IList<TaskComment> _comments = new List<TaskComment>();

		Domain IDomainContainer.Domain { get => db;
			set => db = value;
		}
		protected Domain db;

	}

}