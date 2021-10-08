using System.Diagnostics;


namespace Luxena.Travel.Domain
{

	[AgentPrivileges]
	[DebuggerDisplay(@"{Product} - {PassengerName}, {Passenger}")]
	public partial class ProductPassenger : Entity2
	{

		public virtual Product Product { get; set; }

		[RU("Имя пассажира", ruShort: "имя")]
		public virtual string PassengerName { get; set; }

		[RU("Пассажир", ruShort: "персона")]
		public virtual Person Passenger { get; set; }


		public override string ToString()
		{
			return Passenger.AsString() ?? PassengerName;
		}

		public override object Clone()
		{
			var clone = (ProductPassenger)base.Clone();

			clone.PassengerName = PassengerName;
			clone.Passenger = Passenger;

			return clone;
		}


		public class Service : Entity2Service<ProductPassenger>
		{

		}

	}

}