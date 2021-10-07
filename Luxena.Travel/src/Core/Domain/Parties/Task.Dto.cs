using System;

using Luxena.Base.Metamodel;
using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class TaskDto : EntityContract
	{

		public string Type { get; set; }

		public string Text { get { return Number; } }

		public string Number { get; set; }

		public string Subject { get; set; }

		public string Description { get; set; }

		public Party.Reference RelatedTo { get; set; }

		public Order.Reference Order { get; set; }

		public Party.Reference AssignedTo { get; set; }

		public TaskStatus Status { get; set; }

		public DateTime? DueDate { get; set; }

		public bool Overdue { get; set; }

		public bool CanModify { get; set; }

	}


	public partial class TaskContractService : EntityContractService<Task, Task.Service, TaskDto>
	{

		public TaskContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Type = Class.Of(r).Id;

				c.Number = r.Number;
				c.Subject = r.Subject;
				c.Description = r.Description;
				c.RelatedTo = r.RelatedTo;
				c.Order = r.Order;
				c.AssignedTo = r.AssignedTo;
				c.Status = r.Status;
				c.DueDate = r.DueDate;

				c.Overdue = r.Overdue;
				c.CanModify = db.CanUpdate(r);
			};

			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.Subject = c.Subject + db;
				r.Description = c.Description + db;
				r.RelatedTo = c.RelatedTo + db;
				r.SetOrder(c.Order + db);
				r.AssignedTo = c.AssignedTo + db;
				r.Status = c.Status + db;
				r.DueDate = c.DueDate + db;
			};
		}

	}

}