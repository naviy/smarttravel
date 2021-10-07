using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public interface ICashOrderForm
	{
		byte[] Print(CashInOrderPayment payment);
	}
}