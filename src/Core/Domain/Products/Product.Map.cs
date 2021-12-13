using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;

using NHibernate;
using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain
{

	public class ProductMap : Entity2Mapping<Product>
	{

		public ProductMap()
		{
//			Discriminator(x =>
//			{
//				x.Type(NHibernateUtil.Int32);
//				x.Column("type");
//			});

			Discriminator(x =>
			{
				x.Length(20);
				x.Type(NHibernateUtil.String);
				x.Column("class");
			});

			Property(x => x.Type, m => { m.NotNullable(true); m.Access(Accessor.ReadOnly); });
			Property(x => x.IsRefund, m => { m.NotNullable(true); m.Access(Accessor.ReadOnly); });
			Property(x => x.Name, m => { m.Length(70); m.Access(Accessor.ReadOnly); });

			ManyToOne(x => x.Order);
			ManyToOne(x => x.ReissueFor);
			ManyToOne(x => x.RefundedProduct);

			Property(x => x.IssueDate, m => { m.Type<UtcKindDateType>(); m.NotNullable(true); });

			Property(x => x.PassengerName, m => m.Access(Accessor.ReadOnly));

			ManyToOne(x => x.Producer);
			ManyToOne(x => x.Provider);

			ManyToOne(x => x.Owner);
			ManyToOne(x => x.LegalEntity);
			ManyToOne(x => x.Customer);
			ManyToOne(x => x.Seller);
			ManyToOne(x => x.Intermediary);

			Property(x => x.IsReservation, m => { m.NotNullable(true); m.Access(Accessor.ReadOnly); });
			Property(x => x.IsProcessed, m => m.NotNullable(true));
			Property(x => x.IsVoid, m => m.NotNullable(true));
			Property(x => x.RequiresProcessing, m => { m.NotNullable(true); m.Access(Accessor.ReadOnly); });

			ManyToOne(x => x.Country);
			Property(x => x.PnrCode, m => m.Length(20));
			Property(x => x.TourCode, m => m.Length(20));

			Property(x => x.BookerOffice, m => m.Length(20));
			Property(x => x.BookerCode, m => m.Length(20));
			ManyToOne(x => x.Booker);

			Property(x => x.TicketerOffice, m => m.Length(20));
			Property(x => x.TicketerCode, m => m.Length(20));
			ManyToOne(x => x.Ticketer);
			Property(x => x.IsTicketerRobot);
			Property(x => x.TicketingIataOffice, m => m.Length(10));

			Property(x => x.Originator, m => m.NotNullable(true));
			Property(x => x.Origin, m => m.NotNullable(true));

			ManyToOne(x => x.OriginalDocument);

			Component(x => x.Fare);
			Component(x => x.EqualFare);
			Component(x => x.ConsolidatorCommission);
			Component(x => x.BookingFee);
			Component(x => x.FeesTotal);
			Component(x => x.CancelFee);
			Property(x => x.CancelCommissionPercent);
			Component(x => x.CancelCommission);
			Component(x => x.Total);
			Component(x => x.RefundServiceFee);
			Component(x => x.ServiceFeePenalty);
			Component(x => x.CancelFee);
			Component(x => x.CancelCommission);


			Component(x => x.Vat);
			Property(x => x.CommissionPercent);
			Component(x => x.Commission);
			Component(x => x.CommissionDiscount);
			Component(x => x.ServiceFee);
			Component(x => x.Handling);
			Component(x => x.HandlingN);
			Component(x => x.Discount);
			Component(x => x.BonusDiscount);
			Component(x => x.BonusAccumulation);
			Component(x => x.GrandTotal);

			Property(x => x.PaymentType, m => m.NotNullable(true));
			Property(x => x.TaxRateOfProduct);
			Property(x => x.TaxRateOfServiceFee);

			Property(x => x.Note, m => m.Type(NHibernateUtil.StringClob));

			BagAggregate(x => x.Passengers, y => y.Product, a => a.PassengerName);
		}

	}

}