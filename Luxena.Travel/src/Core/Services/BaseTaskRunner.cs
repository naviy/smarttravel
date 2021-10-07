using System;
using System.Threading;
using System.Threading.Tasks;

using Common.Logging;


namespace Luxena.Travel.Services
{

	public abstract class BaseTaskRunner : IDisposable
	{
		public IServiceResolver ServiceResolver { get; set; }

		public string[] TaskNames { get; set; }

		protected Timer Timer => _timer;

		protected BaseTaskRunner()
		{
			_timer = new Timer(TimerCallback, null, Timeout.Infinite, Timeout.Infinite);
		}

		~BaseTaskRunner()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				_timer.Dispose();
			}

			_disposed = true;
		}

		public void Start()
		{
			if (TaskNames == null)
				return;

			_log.Info($"Start tasks: {string.Join(", ", TaskNames)}");

			_timer.Change(1, Timeout.Infinite);
		}

		protected virtual void TimerCallback(object state)
		{
			try
			{
				using (ServiceResolver.OpenScope())
				{
					foreach (var taskName in TaskNames)
					{
						if (_disposed)
							return;

						var task = ServiceResolver.Resolve<ITask>(taskName);

						if (_disposed)
							return;

						try
						{
							// task.IsStarted = true;
							task.Execute();
						}
						catch (Exception ex)
						{
							_log.Error(ex);
						}
						// finally
						// {
						// 	task.IsStarted = false;
						// }
					}
				}

				if (_disposed)
					return;

				SetTimer();
			}
			catch(Exception ex)
			{
				_log.Error(ex);
			}
		}

		protected abstract void SetTimer();

		private static readonly ILog _log = LogManager.GetLogger(typeof (PeriodicTaskRunner));
		private readonly Timer _timer;
		private bool _disposed;
	}
}