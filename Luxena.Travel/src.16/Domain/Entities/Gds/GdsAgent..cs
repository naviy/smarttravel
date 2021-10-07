using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Gds-агент", "Gds-агенты")]
	[SupervisorPrivileges]
	public partial class GdsAgent : Entity2
	{

		[Patterns.Name, EntityName]
		public string Name => Person?.Name + " - " + Origin + " - " + OfficeCode + " - " + Code;

		public string Codes => Origin + " - " + OfficeCode + " - " + Code;

		protected Person _Person;

		[RU("Источник документов")]
		public ProductOrigin Origin { get; set; }

		[RU("Код агента")]
		public string Code { get; set; }

		[RU("Код офиса")]
		public string OfficeCode { get; set; }

		[ActiveOwner]
		protected Party _Office;


		//public override string ToString()
		//{
		//	return Name;
		//}


		static partial void Config_(Domain.EntityConfiguration<GdsAgent> entity)
		{
			entity.Association(a => a.Person, a => a.GdsAgents);
			entity.Association(a => a.Office);//, a => a.GdsAgents_Office);
		}

	}


	partial class GdsAgentLookup
	{

		public string Person { get; set; }

		public string Codes { get; set; }

		static partial void SelectAndOrderByName(IQueryable<GdsAgent> query, ref IEnumerable<GdsAgentLookup> lookupList)
		{
			lookupList = query
				.OrderBy(a => new { a.Person.Name, a.Origin, a.OfficeCode, a.Code })
				.Select(a => new GdsAgentLookup
				{
					Id = a.Id,
					Name = a.Person.Name + " - " + a.Origin + " - " + a.OfficeCode + " - " + a.Code,
					Person = a.Person.Name,
					Codes = a.Origin + " - " + a.OfficeCode + " - " + a.Code,
				})
			;
		}
	}

	partial class Domain
	{
		public DbSet<GdsAgent> GdsAgents { get; set; }
	}

}