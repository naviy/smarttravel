using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Авиакомпания", "Авиакомпании"), Icon("plane")]
	public class Airline : Domain.EntityQuery<Organization>
	{

		protected override IQueryable<Organization> GetQuery()
		{
			return db.Organizations.Where(a => a.IsAirline);
		}

		public override void CalculateDefaults(Organization r)
		{
			r.IsAirline = true;
		}


		public static IEnumerable<TLookup> Lookup<TEntity, TLookup>(LookupParams<TEntity, TLookup> p)
			where TEntity : Organization
		{
			if (p.Filter.No())
				return p.GetList();

			IEnumerable<TLookup> data = null;

			var filter = p.Filter;

			var b = filter.Length <= 2 && Entity.LookupStep(p, ref data, () => p.Where(a =>
				a.AirlineIataCode.StartsWith(filter)
			));

			b = b || filter.Length <= 3 && Entity.LookupStep(p, ref data, () => p.Where(a =>
				a.AirlinePrefixCode.StartsWith(filter)
			));

			// ReSharper disable once RedundantAssignment
			b = b || Entity3.Lookup3(p, ref data);

			p.Count = null;

			return data;
		}

	}


	public class AirlineLookup : Entity3Lookup
	{
		public string IataCode { get; set; }

		public static IQueryable<AirlineLookup> SelectAndOrderByName(IQueryable<Organization> query)
		{
			return query
				.Select(a => new AirlineLookup
				{
					Id = a.Id,
					Name = a.Name,
					IataCode = a.AirlineIataCode,
				})
				.OrderBy(a => a.Name);
		}
	}


	partial class Domain
	{
		public Airline Airlines { get; set; }
	}


	[Localization(typeof(Airline)), Lookup(typeof(Airline))]
	public class AirlineAttribute : Attribute { }

}