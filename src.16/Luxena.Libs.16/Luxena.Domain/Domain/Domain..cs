using System;
using System.Data.Entity;

using NLog;


namespace Luxena.Domain
{

	public abstract partial class Domain<TDomain> : DbContext
		where TDomain : Domain<TDomain>, new()
	{

		public virtual void InitSecurity() { }


		#region Logging

		//public string LogString { get { return _logger.; } }

		public bool SqlLogging { get { return Database.Log == null; } set { Database.Log = s => Logger.Info(s); } }


		public void Log(string msg)
		{
			Logger.Info(msg);
		}

		public void Log(object value)
		{
			Logger.Info(value);
		}

		public void Log(Exception ex)
		{
			Logger.Error(ex);
		}


		public void Debug(string msg) { Logger.Debug(msg); }
		public void Debug(object value) { Logger.Debug(value); }

		public void Warn(string msg) { Logger.Warn(msg); }
		public void Warn(object value) { Logger.Warn(value); }

		public void Trace(string msg) { Logger.Trace(msg); }
		public void Trace(object value) { Logger.Trace(value); }

		public static readonly Logger Logger = LogManager.GetLogger("Domain");

		#endregion


		#region Utilites


		public class Lazy<TValue> : Lazy<TDomain, TValue>
		{
			public Lazy(Func<TDomain, TValue> defaultGetter) : base(defaultGetter) { }
		}

		public static Lazy<TValue> NewLazy<TValue>(Func<TDomain, TValue> defaultGetter)
		{
			return new Lazy<TValue>(defaultGetter);
		}


		//public static T Using<T>(Func<TDomain, T> action)
		//{
		//	using (var db = new TDomain())
		//	{
		//		return action(db);
		//	}
		//}

		//public static void Using(Action<TDomain> action)
		//{
		//	using (var db = new TDomain())
		//	{
		//		action(db);
		//	}
		//}

		#endregion


	}

}
