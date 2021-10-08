using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[Extends(typeof(Organization))]
	public class AccommodationProvider
	{
		public class Service : Entity3Service<Organization>
		{
			protected override IQueryable<Organization> NewQuery()
			{
				return base.NewQuery().Where(a => a.IsAccommodationProvider);
			}
		}

	}


	[Extends(typeof(Organization))]
	public class BusTicketProvider
	{
		public class Service : Entity3Service<Organization>
		{
			protected override IQueryable<Organization> NewQuery()
			{
				return base.NewQuery().Where(a => a.IsBusTicketProvider);
			}
		}
	}


	[Extends(typeof(Organization))]
	public class CarRentalProvider
	{
		public class Service : Entity3Service<Organization>
		{
			protected override IQueryable<Organization> NewQuery()
			{
				return base.NewQuery().Where(a => a.IsCarRentalProvider);
			}
		}
	}


	[Extends(typeof(Organization))]
	public class GenericProductProvider
	{
		public class Service : Entity3Service<Organization>
		{
			protected override IQueryable<Organization> NewQuery()
			{
				return base.NewQuery().Where(a => a.IsGenericProductProvider);
			}
		}
	}


//	[Extends(typeof(Organization))]
//	public class InsuranceProvider
//	{
//		public class Service : Entity3Service<Organization>
//		{
//			protected override IQueryable<Organization> NewQuery()
//			{
//				return base.NewQuery().Where(a => a.IsInsuranceProvider);
//			}
//		}
//	}


	[Extends(typeof(Organization))]
	public class PasteboardProvider
	{
		public class Service : Entity3Service<Organization>
		{
			protected override IQueryable<Organization> NewQuery()
			{
				return base.NewQuery().Where(a => a.IsPasteboardProvider);
			}
		}
	}


	[Extends(typeof(Organization))]
	public class TourProvider
	{
		public class Service : Entity3Service<Organization>
		{
			protected override IQueryable<Organization> NewQuery()
			{
				return base.NewQuery().Where(a => a.IsTourProvider);
			}
		}
	}


	[Extends(typeof(Organization))]
	public class TransferProvider
	{
		public class Service : Entity3Service<Organization>
		{
			protected override IQueryable<Organization> NewQuery()
			{
				return base.NewQuery().Where(a => a.IsTransferProvider);
			}
		}
	}

}