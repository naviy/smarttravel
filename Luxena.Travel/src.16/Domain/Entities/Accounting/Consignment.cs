using System;
using System.Collections.Generic;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Накладная", "Накладные")]
	[SupervisorPrivileges]
	public partial class Consignment : Entity2
	{
		[EntityName, Patterns.Number, Length(12)]
		public string Number { get; set; }

		[EntityDate, Patterns.IssueDate]
		public DateTimeOffset IssueDate { get; set; }

		[RU("Отпущено")]
		protected Party _Supplier;

		[RU("Получено")]
		protected Party _Acquirer;

		[ReadOnly]
		public virtual Order Order
		{
			get { return OrderItems.OneAs(a => a.Order); }
		}

		
		[RU("Сумма с НДС")]
		public Money GrandTotal { get; set; }

		[Patterns.Vat]
		public Money Vat { get; set; }

		[RU("Сумма без НДС")]
		public Money Total
		{
			get { return GrandTotal - Vat; }
		}

		[Patterns.Discount]
		public Money Discount { get; set; }

		[RU("Всего отпущено")]
		public string TotalSupplied { get; set; }


		public virtual ICollection<OrderItem> OrderItems { get; set; }

		public virtual ICollection<IssuedConsignment> IssuedConsignments { get; set; }


		static partial void Config_(Domain.EntityConfiguration<Consignment> entity)
		{
			entity.Association(a => a.Supplier);//, a => a.Consignments_Supplier);
			entity.Association(a => a.Acquirer);//, a => a.Consignments_Acquirer);
		}

	}


	partial class Domain
	{
		public DbSet<Consignment> Consignments { get; set; }
	}

}