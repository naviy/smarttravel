using System;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Аэропорт", "Аэропорты")]
	[SupervisorPrivileges]
	public partial class Airport : Entity3
	{

		[Patterns.Code, EntityName2]
		public virtual string Code { get; set; }

		public virtual Country Country { get; set; }

		[RU("Населенный пункт (англ.)")]
		public virtual string Settlement { get; set; }

		[RU("Населенный пункт")]
		public virtual string LocalizedSettlement { get; set; }

		[RU("Широта")]
		public virtual double? Latitude { get; set; }

		[RU("Долгота")]
		public virtual double? Longitude { get; set; }


		public override string ToString()
		{
			return Name.No() ? Code : string.Format(DomainRes.Airport_Format_ToString, Code, Name);
		}

		/// <summary>
		/// Расстояние в километрах
		/// </summary>
		public static double GetDistance(Airport a, Airport b)
		{
			if (a?.Longitude == null || a.Latitude == null || b?.Longitude == null || b.Latitude == null)
				return 0;

			return Math.Round(0.001 * GeoUtility.GetDistance(
				a.Latitude.Value, a.Longitude.Value,
				b.Latitude.Value, b.Longitude.Value
			), 1);
		}


		public override Entity Resolve(Domain db)
		{
			return db.Airport.ByCode(Code) ?? db.Airport.Save(this);
		}


		public class Service : Entity3Service<Airport>
		{

			public Airport ByCode(string code)
			{
				return code.No() ? null : By(a => a.Code == code);
			}

			public override RangeResponse Suggest(RangeRequest prms)
			{
				var list = (
					from a in Query
					where
						a.Code.StartsWith(prms.Query) ||
						a.Name.StartsWith(prms.Query) ||
						(a.Code + " - " + a.Name).StartsWith(prms.Query)
					orderby a.Name
					select EntityReference.FromArray(a.Id, a.Code + " - " + a.Name, a.GetType().Name)
				)
				.ToArray();

				return new RangeResponse(list);
			}

		}

	}

}