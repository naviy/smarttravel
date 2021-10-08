using System.Data.Entity;
using System.Diagnostics;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Пассажир", "Пассажиры")]
	[AgentPrivileges]
	[DebuggerDisplay(@"{Product} - {PassengerName}, {Passenger}")]
	public partial class ProductPassenger : Entity2
	{

		protected Product _Product;

		[RU("Имя пассажира", ruShort: "имя")]
		public virtual string PassengerName { get; set; }

		[RU("Пассажир", ruShort: "персона")]
		protected Person _Passenger;


		static partial void Config_(Domain.EntityConfiguration<ProductPassenger> entity)
		{
			entity.Association(a => a.Product, a => a.Passengers);
			entity.Association(a => a.Passenger);//, a => a.ProductPassengers);
		}

		public override string ToString()
		{
			return Passenger.AsString() ?? PassengerName;
		}

		//protected override Domain.Entity Clone()
		//{
		//	var c = (ProductPassenger)base.Clone();

		//	c.PassengerName = PassengerName;
		//	c.Passenger = Passenger;

		//	return c;
		//}


		protected override void Bind()
		{
			base.Bind();

//			if (PassengerId.No() && PassengerName.No())
//				this.Delete(db);

			ModifyMaster<Product, ProductPassenger>(a => a.Product, a => a.Passengers);
		}

	}


	partial class Domain
	{
		public DbSet<ProductPassenger> ProductPassengers { get; set; }
	}

}