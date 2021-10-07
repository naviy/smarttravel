using Luxena.Base.Data;


namespace Luxena.Base.Managers
{
	public abstract class BaseManager
	{
		//public ISecurityContext SecurityContext { get; set; }
		//public IPreferences Preferences { get; set; }
		public ITransactionManager TransactionManager { get; set; }
	}
}