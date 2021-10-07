using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Закрытый период", "Закрытые периоды")]
	[SupervisorPrivileges]
	public partial class ClosedPeriod : Entity2
	{

		[RU("Дата с"), EntityDate, Required]
		public virtual DateTime DateFrom { get; set; }

		[RU("Дата по"), Required]
		public virtual DateTime DateTo { get; set; }

		[RU("Состояние"), DefaultValue(PeriodState.Closed)]
		public virtual PeriodState PeriodState { get; set; }

	}

}