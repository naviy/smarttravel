using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public class InvoiceItemExporter : IPropertyExporter
	{
		public Domain.Domain db { get; set; }

		public string ToCsv(object value, string separator)
		{
			var invoiceItem = value as OrderItem;
			if (invoiceItem != null)
				return string.Format("\"{0}\"{1}\"{2}\"{3}\"{4}\"{5}\"{6}\"{7}\"{8}\"", (typeof(OrderItem)).Name, 
					separator, invoiceItem.Id,
					separator, invoiceItem.Text.No() ? string.Empty : invoiceItem.Text.Replace('\n', ' ').Replace('\r', ' '),
					separator, invoiceItem.GrandTotal == null ? 0 : invoiceItem.GrandTotal.Amount,
					separator, invoiceItem.GrandTotal == null ? db.Configuration.DefaultCurrency.Code : invoiceItem.GrandTotal.Currency.Code);

			return null;
		}
	}
}