using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public interface IReceiptPrinter
	{
		byte[] Build(Order order, Invoice invoice);
	}
}