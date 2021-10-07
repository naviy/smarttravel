using System;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Агент", "Агенты"), Icon("user-secret"), Small]
	public class Agent : Domain.EntityQuery<Person>
	{
		protected override IQueryable<Person> GetQuery()
		{
			return
				db.GdsAgents.Select(a => a.Person)
				.Union(db.Users.Where(a => a.Active && !a.IsAdministrator).Select(a => a.Person));
		}
	}


	partial class Domain
	{
		public Agent Agents { get; set; }
	}


	[Localization(typeof(Agent)), Lookup(typeof(Agent))]
	public class AgentAttribute : Attribute { }

}