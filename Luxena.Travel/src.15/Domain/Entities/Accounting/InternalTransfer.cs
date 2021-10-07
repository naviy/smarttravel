using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Внутренний перевод", "Внутренние переводы")]
	[SupervisorPrivileges]
	public partial class InternalTransfer : Entity2
	{

		[Patterns.Number, EntityName]
		public string Number { get; set; }

		[Patterns.Date, EntityDate]
		public DateTimeOffset Date { get; set; }

		[RU("Из заказа")]
		protected Order _FromOrder;

		[RU("От контрагента"), Customer, Required]
		protected Party _FromParty;

		[RU("В заказ")]
		protected Order _ToOrder;

		[RU("К контрагенту"), Customer, Required]
		protected Party _ToParty;

		[RU("Сумма")]
		public decimal Amount { get; set; }


		static partial void Config_(Domain.EntityConfiguration<InternalTransfer> entity)
		{
			entity.Association(a => a.FromOrder, a => a.OutgoingTransfers);
			entity.Association(a => a.FromParty);//, a => a.OutgoingTransfers);
			entity.Association(a => a.ToOrder, a => a.IncomingTransfers);
			entity.Association(a => a.ToParty);//, a => a.IncomingTransfers);
		}


		protected override IList<Domain.Entity> GetDependents()
		{
			return new[] { FromOrder, ToOrder };
		}

		protected override void Bind()
		{
			base.Bind();

			ModifyMaster<Order, InternalTransfer>(a => a.FromOrder, a => a.OutgoingTransfers);
			ModifyMaster<Order, InternalTransfer>(a => a.ToOrder, a => a.IncomingTransfers);
		}

		public override void Calculate()
		{
			base.Calculate();

			if (FromOrder != null)
				FromParty = FromOrder.Customer;

			if (ToOrder != null)
				ToParty = ToOrder.Customer;
		}

	}


	partial class Domain
	{
		public DbSet<InternalTransfer> InternalTransfers { get; set; }
	}

}