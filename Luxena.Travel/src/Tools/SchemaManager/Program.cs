using System;
using System.Configuration;
using System.Globalization;
using System.Threading;

using Common.Logging;

using NHibernate.Tool.hbm2ddl;

using NHibernateConfiguration = NHibernate.Cfg.Configuration;


namespace Luxena.Travel
{
	internal class SchemaManager
	{
		private static void Main(params string[] args)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

			_log.Info("Schema Manager started");

			try
			{
				var cfg = Config.ConfigurationBuilder.Build(null);

				bool script;

				if (!bool.TryParse(ConfigurationManager.AppSettings["script"], out script))
					script = true;

				bool execute;

				if (!bool.TryParse(ConfigurationManager.AppSettings["execute"], out execute))
					execute = true;

				Run(cfg, script, execute, args);

				_log.Info("Schema Manager finished");
			}
			catch (Exception exception)
			{
				_log.Fatal("Schema Manager failed", exception);
			}
		}

		public static void Run(NHibernateConfiguration cfg, bool script, bool execute, params string[] args)
		{
			if (args.Length != 0 && args[0].ToLower() == "update")
			{
				_log.Info("Update mode");

				new SchemaUpdate(cfg).Execute(script, execute);
			}
			else
			{
				_log.Info("Create mode");

				new SchemaExport(cfg).Create(line=> Console.WriteLine(line), execute);
			}
		}

		private static readonly ILog _log = LogManager.GetLogger(typeof (SchemaManager));
	}
}