using Luxena.Base.Data;


namespace Luxena.Base.Services
{
	public abstract class BaseService
	{
		public ITransactionManager TransactionManager { get; set; }
	}
}