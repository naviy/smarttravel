using System;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Контракт", "Контракты")]
	[AgentPrivileges]
	public partial class Contract : Entity2
	{

		[RU("Организация"), Required]
		public virtual Organization Customer { get; set; }

		[EntityName, Patterns.Number, Required]
		public virtual string Number { get; set; }

		[EntityDate, Patterns.Date]
		public virtual DateTime? IssueDate { get; set; }

		[RU("Дисконт, %")]
		public virtual decimal DiscountPc { get; set; }

		[Patterns.Note, Text]
		public virtual string Note { get; set; }


		public class Service : Entity2Service<Contract>
		{

		}

	}

}