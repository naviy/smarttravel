using System;
using System.Linq;

using Common.Logging;

using Luxena.Base.Data.NHibernate;


namespace Luxena.Travel.Services
{

	public class ExecSqlTask : ITask
	{
		public TransactionManager TransactionManager { get; set; }

		// bool ITask.IsStarted { get; set; }

		public string Sql { get; set; }

		public string SuccessMessage { get; set; }

		public string FailMessage { get; set; }


		public void Execute()
		{
			if (Sql.No()) return;

			try
			{
				var data = TransactionManager.Session.CreateSQLQuery(Sql).List<object>();
				TransactionManager.Commit();

				if (data == null || data.Count == 0 || data.All(a => a == null))
					return;

				_log.Info(string.Format(SuccessMessage ?? "{0}", data.First()));
			}
			catch (Exception ex)
			{
				_log.Error(FailMessage, ex);
			}

		}


		private static readonly ILog _log = LogManager.GetLogger(typeof(ExecSqlTask));
	}

}