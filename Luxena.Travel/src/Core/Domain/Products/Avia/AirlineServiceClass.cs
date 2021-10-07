namespace Luxena.Travel.Domain
{

	[RU("Сервис-класс авиакомпании", "Сервис-классы авиакомпаний")]
	[SupervisorPrivileges]
	public partial class AirlineServiceClass : Entity2
	{

		[Localization(typeof(Airline))]
		public virtual Organization Airline { get; set; }

		[EntityName, Patterns.Code]
		public virtual string Code { get; set; }

		public virtual ServiceClass ServiceClass { get; set; }


		public override string ToString() 
			=> Airline + " " + Code;
	}

}
