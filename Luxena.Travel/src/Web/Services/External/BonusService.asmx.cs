using System.Web.Services;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{

	public class BonusService : DomainWebService
	{

		//http://travel/services/external/BonusService.asmx/SetBonuses
		//{ data: [{Number: '111', Amount: 222.5}, {Number: '222', Amount: 445}] }
		[WebMethod]
		public string SetBonuses(PartyBonusData[] data)
		{
			if (data == null) return "null";

			var sql = new StringWrapper();

			foreach (var b in data)
			{
				if (b.Number.No()) continue;
				var amount = b.Amount != null ? b.Amount.ToString().Replace(",", ".") : "null";
				sql *= $"update lt_party set bonusamount = {amount} where bonuscardnumber = '{b.Number}';";
			}

			db.Commit(() =>
				db.Session.CreateSQLQuery(sql).ExecuteUpdate()
			);

			return "OK";
		}

	}


	public class PartyBonusData
	{
		public string Number { get; set; }
		public decimal? Amount { get; set; }
	}

}
