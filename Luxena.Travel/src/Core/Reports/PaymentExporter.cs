using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public class PaymentExporter : IPropertyExporter
	{
		public Domain.Domain db { get; set; }

		public string ToCsv(object value, string separator)
		{
			var payment = value as Payment;
			if (payment != null)
				return string.Format("\"{0}\"{1}\"{2}\"{3}\"{4}\"{5}\"{6}\"{7}\"{8}\"", (typeof (Payment)).Name,
					separator, payment.Id,
					separator, payment.Order == null ? null : payment.Order.Id,
					separator, payment.Amount == null ? 0 : payment.Sign * payment.Amount.Amount,
					separator, payment.Amount == null ? db.Configuration.DefaultCurrency.Code : payment.Amount.Currency.Code);

			return null;
		}
	}
}