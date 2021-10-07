using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public partial class GenericProductDto : ProductDto
	{

		public ProductPassengerDto[] Passengers { get; set; }

		public GenericProductType.Reference GenericType { get; set; }

		public string Number { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

	}


	public partial class GenericProductContractService 
		: ProductContractService<GenericProduct, GenericProduct.Service, GenericProductDto>
	{
		public GenericProductContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Passengers = NewPassengers(r.Passengers);

				c.GenericType = r.GenericType;
				c.Number = r.Number;

				c.StartDate = r.StartDate;
				c.FinishDate = r.FinishDate;
			};

			EntityFromContract += (r, c) =>
			{
				r.GenericType = c.GenericType + db;
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

}