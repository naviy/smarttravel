using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate;


namespace Luxena.Travel.Domain.Avia
{

	public class AviaDocumentMap : SubEntityMapping<AviaDocument>
	{

		public AviaDocumentMap()
		{
			// unique constraint is specified in AuxilaryObjects.hbm.xml since the columns order metters

			Property(x => x.AirlinePrefixCode, m => m.Length(3));
			Property(x => x.Number);

			Property(x => x.AirlineIataCode, m => m.Length(2));
			Property(x => x.AirlineName, m => m.Length(100));
			Property(x => x.ConjunctionNumbers, m => m.Length(2));

			Property(x => x.GdsPassportStatus, m => m.NotNullable(true));
			Property(x => x.GdsPassport, m => m.Length(150));
			Property(x => x.Itinerary, m => m.Length(100));

			Property(x => x.PaymentForm, m => m.Length(50));
			Property(x => x.PaymentDetails, m => m.Type(NHibernateUtil.StringClob));

			Property(x => x.AirlinePnrCode, m => m.Length(20));
			Property(x => x.Remarks, m => m.Type(NHibernateUtil.StringClob));

//			Property(x => x.PrintUnticketedFlightSegments);

			BagAggregate(x => x.Fees, i => i.Document, o => o.Code);

			BagAggregate(x => x.Voidings, i => i.Document, o => o.CreatedOn);
		}

	}

}