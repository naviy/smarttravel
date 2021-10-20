using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate;
using NHibernate.Mapping.ByCode;




namespace Luxena.Travel.Domain.Mapping
{



	//===g






	public class PartyMap : Entity2Mapping<Party>
	{

		public PartyMap()
		{

			Cache(x => x.Usage(CacheUsage.ReadWrite));

			Discriminator(x =>
			{
				x.Length(20);
				x.Type(NHibernateUtil.String);
				x.Column("class");
			});

			Property(x => x.Type, m => { m.NotNullable(true); m.Access(Accessor.ReadOnly); });

			Property(x => x.Name, x =>
			{
				x.Unique(true);
				x.NotNullable(true);
				x.Length(100);
			});

			Property(x => x.LegalName, x => x.Length(100));
			Property(x => x.Signature, x => x.Length(100));
			Property(x => x.Code, m => { m.Length(50); m.Unique(true); });

			Property(x => x.BonusCardNumber);
			Property(x => x.BonusAmount);

			Property(x => x.Phone1, x => x.Length(100));
			Property(x => x.Phone2, x => x.Length(100));
			Property(x => x.Fax, x => x.Length(100));
			Property(x => x.Email1, x => x.Length(100));
			Property(x => x.Email2, x => x.Length(100));
			Property(x => x.WebAddress, x => x.Length(100));

			Property(x => x.IsCustomer, m => m.NotNullable(true));
			Property(x => x.CanNotBeCustomer, m => m.NotNullable(true));
			Property(x => x.IsSupplier, m => m.NotNullable(true));

			ManyToOne(x => x.ReportsTo);
			ManyToOne(x => x.DefaultBankAccount);

			Property(x => x.Details, m => m.Type(NHibernateUtil.StringClob));
			Property(x => x.InvoiceSuffix, x => x.Length(100));

			Property(x => x.LegalAddress, m => m.Type(NHibernateUtil.StringClob));
			Property(x => x.ActualAddress, m => m.Type(NHibernateUtil.StringClob));

			Property(x => x.Note, m => m.Type(NHibernateUtil.StringClob));

			BagAggregate(x => x.Files, i => i.Party, "TimeStamp desc");

		}

	}
	





	//===g



}