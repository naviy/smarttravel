using System.Collections.Generic;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Тип проживания", "Типы проживания"), Small]
	[SupervisorPrivileges]
	public partial class AccommodationType : Entity3D 
	{
		//public virtual ICollection<Accommodation> Accommodations { get; set; }
		//public virtual ICollection<Tour> Tours { get; set; }
	}


	[RU("Тип питания", "Типы питания"), Small]
	[SupervisorPrivileges]
	public partial class CateringType : Entity3D 
	{
		//public virtual ICollection<Accommodation> Accommodations { get; set; }
		//public virtual ICollection<Tour> Tours { get; set; }
	}


	[RU("Вид дополнительной услуги", "Виды дополнительных услуг"), Small]
	[SupervisorPrivileges]
	public partial class GenericProductType : Entity3 
	{
		//public virtual ICollection<GenericProduct> GenericProducts { get; set; }
	}


	[RU("Платёжная система", "Платёжные системы"), Small]
	[SupervisorPrivileges]
	public partial class PaymentSystem : Entity3 
	{
		//public virtual ICollection<Payment> Payments { get; set; }
	}


	partial class Domain
	{
		public DbSet<AccommodationType> AccommodationTypes { get; set; }
		public DbSet<CateringType> CateringTypes { get; set; }
		public DbSet<GenericProductType> GenericProductTypes { get; set; }
		public DbSet<PaymentSystem> PaymentSystems { get; set; }
	}

}
