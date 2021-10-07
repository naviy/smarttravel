using System.Collections.Generic;
using System.Linq;
using System.Web.OData;

using Newtonsoft.Json;


namespace Luxena.Domain.Web
{

	public abstract class DomainODataController<TDomain> : ODataController
		where TDomain : Domain<TDomain>, new()
	{

		protected readonly TDomain db = new TDomain();

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}

			base.Dispose(disposing);
		}


		public static string Serialize(IDictionary<string, object> values)
		{
			if (values.No()) return null;

			foreach (var pair in values.ToArray())
			{
				var value = pair.Value;
				if (value != null && value.GetType().IsEnum)
					values[pair.Key] = value.ToString();
			}

			var json = JsonConvert.SerializeObject(values);

			return json;
		}

		public static IDictionary<string, object> Deserialize(string json)
		{
			if (json.No()) return null;

			return JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
		}

	}

}
