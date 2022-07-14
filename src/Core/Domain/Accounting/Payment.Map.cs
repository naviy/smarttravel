using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;

using NHibernate;
using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Accounting
{
	public sealed class PaymentMap : Entity2Mapping<Payment>
	{
		public PaymentMap()
		{
			Discriminator(x =>
			{
				x.Length(20);
				x.Type(NHibernateUtil.String);
				x.Column("class");
			});

			Property(x => x.Number, m => { m.NotNullable(true); m.Length(20); m.Index("payment_number_idx"); });

			Property(x => x.DocumentNumber, m => { m.Length(20); m.Index("payment_documentnumber_idx"); });
			Property(x => x.DocumentUniqueCode, m => { m.Length(30); m.Unique(true); m.Access(Accessor.ReadOnly); });

			ManyToOne(x => x.Payer, m => m.NotNullable(true));

			ManyToOne(x => x.Order);

			ManyToOne(x => x.Invoice);

			Property(x => x.Sign, m => { m.NotNullable(true); m.Access(Accessor.ReadOnly); });

			Component(x => x.Amount);

			Component(x => x.Vat);

			Property(x => x.PaymentForm, m => m.NotNullable(true));

			ManyToOne(x => x.PaymentSystem);

			Property(x => x.ReceivedFrom, m => { m.Length(200); m.Index("payment_receivedfrom_idx"); });

			Property(x => x.Note, m => m.Type(NHibernateUtil.StringClob));
			Property(x => x.IsVoid, m => m.NotNullable(true));

			ManyToOne(x => x.AssignedTo, m => m.NotNullable(true));
			ManyToOne(x => x.RegisteredBy, m => m.NotNullable(true));
			Property(x => x.Date, m => { m.Type<UtcKindDateType>(); m.NotNullable(true); m.Index("payment_date_idx"); });
			Property(x => x.PostedOn);

			ManyToOne(x => x.Owner);
			ManyToOne(x => x.BankAccount);

			Property(x => x.PrintedDocument, m => m.Lazy(true));
		}
	}

}