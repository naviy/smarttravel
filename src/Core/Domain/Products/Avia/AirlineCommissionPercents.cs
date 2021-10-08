namespace Luxena.Travel.Domain
{

	[RU("Комиссия от авиакомпании", "Комиссии от авиакомпаний")]
	[SupervisorPrivileges]
	public partial class AirlineCommissionPercents : Entity
	{

		[EntityName]
		public virtual Organization Airline { get; set; }

		[RU("Перелет внутри страны, %")]
		public virtual decimal Domestic { get; set; }

		[RU("Международный перелет, %")]
		public virtual decimal International { get; set; }

		[RU("Перелет внутри страны (interline), %")]
		public virtual decimal InterlineDomestic { get; set; }

		[RU("Международный перелет (interline), %")]
		public virtual decimal InterlineInternational { get; set; }


		public class Service : EntityService<AirlineCommissionPercents>
		{

			public AirlineCommissionPercents By(Organization airline)
			{
				return airline == null ? null : By(a => a.Airline == airline);
			}

		}

	}

}