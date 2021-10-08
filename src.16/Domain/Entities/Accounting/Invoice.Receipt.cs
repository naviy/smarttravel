using System;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Квитанция", "Квитанции")]
	public class Receipt : Domain.EntityQuery<Invoice>
	{
		protected override IQueryable<Invoice> GetQuery()
		{
			return db.Invoices.Where(a => a.Type == InvoiceType.Receipt);
		}

		public override void CalculateDefaults(Invoice r)
		{
			r.Type = InvoiceType.Receipt;
		}
	}


	partial class Domain
	{
		public Receipt Receipts { get; set; }
	}


	[Localization(typeof(Receipt)), Lookup(typeof(Receipt))]
	public class ReceiptAttribute : Attribute { }

}