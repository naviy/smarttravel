using Luxena.Base.Data.NHibernate.Mapping;

using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Domain.Mapping
{

	public class OrganizationMap : SubEntityMapping<Organization>
	{
		public OrganizationMap()
		{
			DiscriminatorValue(PartyType.Organization);


			Property(x => x.IsProvider, m => m.Access(Accessor.ReadOnly));

			Property(x => x.IsAccommodationProvider, m => m.NotNullable(true));
			Property(x => x.IsBusTicketProvider, m => m.NotNullable(true));
			Property(x => x.IsCarRentalProvider, m => m.NotNullable(true));
//			Property(x => x.IsInsuranceProvider, m => m.NotNullable(true));
			Property(x => x.IsPasteboardProvider, m => m.NotNullable(true));
			Property(x => x.IsTourProvider, m => m.NotNullable(true));
			Property(x => x.IsTransferProvider, m => m.NotNullable(true));
			Property(x => x.IsGenericProductProvider, m => m.NotNullable(true));


			Property(x => x.IsAirline, m => m.NotNullable(true));

			Property(x => x.AirlineIataCode, m => { m.Unique(true); m.Length(2); });

			Property(x => x.AirlinePrefixCode, m => { m.Unique(true); m.Length(3); });

			Property(x => x.AirlinePassportRequirement, m => m.NotNullable(true));


			Property(x => x.IsInsuranceCompany, m => m.NotNullable(true));

			Property(x => x.IsRoamingOperator, m => m.NotNullable(true));


//			BagPersist(x => x.Departments, i => i.Organization);
//
//			BagPersist(x => x.Employees, i => i.Organization);
		}
	}

}