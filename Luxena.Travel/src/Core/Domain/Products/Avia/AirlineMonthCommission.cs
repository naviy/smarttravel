using System;
using System.ComponentModel.DataAnnotations;


namespace Luxena.Travel.Domain
{

	[RU("Доп.комиссия от авиакомпании", "Доп.комиссии от авиакомпаний")]
	[AdminOnlyPrivileges]
	public partial class AirlineMonthCommission : Entity2
	{

		[Localization(typeof(Airline)), Required]
		public virtual Organization Airline { get; set; }

		[EntityName, Patterns.StartDate]
		public virtual DateTime DateFrom { get; set; }

		[Patterns.FinishDate]
		public virtual DateTime? DateTo { get; set; }

		[RU("Доп.комиссия, %")]
		public virtual decimal? CommissionPc { get; set; }


		public override string ToString() 
			=> $"{Airline} {DateFrom:yyyy MMMM}";
	}

}
