using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class InsuranceDto : ProductDto
	{

		public ProductPassengerDto[] Passengers { get; set; }

		public string Number { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

	}


	[DataContract]
	public partial class InsuranceRefundDto : InsuranceDto { }


	public abstract partial class InsuranceContractService<TInsurance, TInsuranceService, TInsuranceDto>
		: ProductContractService<TInsurance, TInsuranceService, TInsuranceDto>
		where TInsurance : Insurance, new()
		where TInsuranceService : Insurance.Service<TInsurance>
		where TInsuranceDto : InsuranceDto, new()
	{
		protected InsuranceContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Passengers = NewPassengers(r.Passengers);
				c.Number = r.Number;

				c.StartDate = r.StartDate;
				c.FinishDate = r.FinishDate;
			};

			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;

				r.StartDate = c.StartDate + db;
				r.FinishDate = c.FinishDate + db;

				dc.ProductPassenger.Update(
					r, r.Passengers, c.Passengers,
					r.AddPassenger, r.RemovePassenger
				);
			};
		}
	}


	public partial class InsuranceContractService : InsuranceContractService<Insurance, Insurance.Service, InsuranceDto> { }

	public partial class InsuranceRefundContractService : InsuranceContractService<InsuranceRefund, InsuranceRefund.Service, InsuranceRefundDto> { }

}