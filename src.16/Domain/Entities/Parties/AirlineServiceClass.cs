using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Сервис-класс авиакомпании", "Сервис-классы авиакомпаний")]
	[SupervisorPrivileges]
	public partial class AirlineServiceClass : Entity2
	{

		[Airline, Required]
		protected Organization _Airline;

		[EntityName, Patterns.Code, Required]
		public string Code { get; set; }

		public ServiceClass ServiceClass { get; set; }


		//public override string ToString()
		//{
		//	return Airline + " " + Code;
		//}


		static partial void Config_(Domain.EntityConfiguration<AirlineServiceClass> entity)
		{
			entity.Association(a => a.Airline, a => a.AirlineServiceClasses);
		}

	}


	partial class Domain
	{
		public DbSet<AirlineServiceClass> AirlineServiceClasses { get; set; }
	}

}
