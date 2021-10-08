using System;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Выпущенная накладная", "Выпущенные накладные")]
	[SupervisorPrivileges(Create2 = 0)]
	public partial class IssuedConsignment : Entity
	{
		[EntityName, Patterns.Name]
		public string Number { get; set; }

		[EntityDate, DateTime2]
		public DateTimeOffset TimeStamp { get; set; }

		public virtual byte[] Content { get; set; }

		protected Consignment _Consignment;

		[RU("Выпустил"), Agent]
		protected Person _IssuedBy;

		public override string ToString()
		{
			return Number;
		}


		static partial void Config_(Domain.EntityConfiguration<IssuedConsignment> entity)
		{
			entity.Association(a => a.Consignment, a => a.IssuedConsignments);
			entity.Association(a => a.IssuedBy);//, a => a.IssuedConsignments_IssuedBy);
		}

	}


	partial class Domain
	{
		public DbSet<IssuedConsignment> IssuedConsignments { get; set; }
	}

}