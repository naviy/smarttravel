using System;
using System.Web.Http;
using System.Web.OData;


namespace Luxena.Domain.Web
{

	public abstract class DomainActionODataController<TDomain, TAction> : DomainODataController<TDomain>
		where TDomain : Domain<TDomain>, new()
		where TAction : Domain<TDomain>.DomainAction, new()
	{

		public IHttpActionResult Get()
		{
			db.InitSecurity();

			var action = new TAction { db = db, Id = "" };

			action.Calculate();

			return Ok(action);
		}

		public IHttpActionResult Get(string key)
		{
			return Get();
		}


		public IHttpActionResult Put([FromODataUri] string key, TAction action)
		{
			db.InitSecurity();

			action = action ?? new TAction();

			try
			{

				action.db = db;
				if (action.Id == null)
					action.Id = "";

				action.CalculateDefaults();
				action.Calculate();
			}
			catch (Exception ex)
			{
				var ex2 = new Exception("CALCULATE ACTION Error", ex);
				db.Log(ex);
				throw ex2;
			}

			return Ok(action);
		}

		public IHttpActionResult Patch([FromODataUri] string key, Delta<TAction> delta)
		{
			db.InitSecurity();

			var action = delta.GetEntity();

			try
			{

				action.db = db;
				if (action.Id == null)
					action.Id = "id";

				action.CalculateDefaults();
				action.Calculate();
			}
			catch (Exception ex)
			{
				var ex2 = new Exception("PATCH ACTION Error", ex);
				db.Log(ex);
				throw ex2;
			}

			return Ok(action);
		}


		public IHttpActionResult Post(TAction action)
		{
			db.InitSecurity();

			try
			{
				action.db = db;

				if (action.Id == null)
					action.Id = "";

				action.Calculate();
				action.Execute();

				return Ok(action);
			}

			catch (Exception ex)
			{
				var ex2 = new Exception("ACTION ERROR", ex);
				db.Log(ex);
				throw ex2;
			}
		}

	}

}