namespace Luxena.Base.Data.NHibernate
{
	public interface ITransactionManagerAware
	{
		void SetTransactionManager(TransactionManager transManager);
	}
}