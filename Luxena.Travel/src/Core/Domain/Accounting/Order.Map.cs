using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;

using NHibernate;
using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Accounting
{
	public class OrderMap : Entity2Mapping<Order>
	{
		public OrderMap()
		{
			Property(x => x.Number, m =>
			{
				m.NotNullable(true);
				m.Unique(true);
				m.Length(20);
			});

			Property(x => x.IssueDate, m => { m.Type<UtcKindDateType>(); m.NotNullable(true); });
			Property(x => x.IsVoid, m => m.NotNullable(true));

			ManyToOne(x => x.Customer, m => m.NotNullable(true));
			ManyToOne(x => x.ShipTo);
			ManyToOne(x => x.BillTo);
			Property(x => x.BillToName);
			ManyToOne(x => x.Intermediary);

			Component(x => x.Discount);

			Component(x => x.Vat);
			Property(x => x.UseServiceFeeOnlyInVat, m => m.NotNullable(true));

			Component(x => x.Total);

			Component(x => x.Paid, m => m.Access(Accessor.NoSetter));
			Component(x => x.CheckPaid, m => m.Access(Accessor.NoSetter));
			Component(x => x.WirePaid, m => m.Access(Accessor.NoSetter));
			Component(x => x.CreditPaid, m => m.Access(Accessor.NoSetter));
			Component(x => x.RestPaid, m => m.Access(Accessor.NoSetter));

			Component(x => x.TotalDue, m => m.Access(Accessor.NoSetter));

			Property(x => x.IsPaid, m => { m.Access(Accessor.ReadOnly); m.NotNullable(true); });

			Component(x => x.VatDue, m => m.Access(Accessor.NoSetter));

			Property(x => x.DeliveryBalance, m => { m.Access(Accessor.NoSetter); m.NotNullable(true); });


			Property(x => x.BonusDate);
			Property(x => x.BonusSpentAmount);
			ManyToOne(x => x.BonusRecipient);

			ManyToOne(x => x.AssignedTo);
			ManyToOne(x => x.Owner);
			ManyToOne(x => x.BankAccount);

			Property(x => x.Note, m => m.Type(NHibernateUtil.StringClob));

			Property(x => x.IsPublic, m => m.NotNullable(true));
			Property(x => x.AllowAddProductsInClosedPeriod, m => m.NotNullable(true));

			Property(x => x.IsSubjectOfPaymentsControl, m => m.NotNullable(true));

			Property(x => x.ConsignmentNumbers, m => m.Access(Accessor.ReadOnly));

			BagAggregate(x => x.Items, i => i.Order, o => o.Position);

			BagAggregate(x => x.Invoices, i => i.Order, "TimeStamp desc");

			Property(x => x.InvoiceLastIndex);
			Property(x => x.ConsignmentLastIndex);

			Bag(x => x.Payments, i => i.Order, o => o.CreatedOn);

			Bag(x => x.Tasks, i => i.Order, o => o.CreatedOn);

			Bag(x => x.OutgoingTransfers, i => i.FromOrder, o => o.CreatedOn);

			Bag(x => x.IncomingTransfers, i => i.ToOrder, o => o.CreatedOn);

		}
	}
}