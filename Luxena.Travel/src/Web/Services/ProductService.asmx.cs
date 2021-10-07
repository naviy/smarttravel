using System;
using System.Linq;
using System.Web.Services;

using Luxena.Base.Data;
using Luxena.Travel.Domain;


namespace Luxena.Travel.Web.Services
{

	public class ProductService : DomainWebService
	{

//		[WebMethod]
//		public EntityReference[] GetProducers(string className)
//		{
//			var productType = (ProductType)Enum.Parse(typeof(ProductType), className);
//			return db.Commit(() => db.Organization.ProducerListBy(productType).ToReferences());
//		}
//
//		[WebMethod]
//		public EntityReference[] GetProviders(string className)
//		{
//			var productType = (ProductType)Enum.Parse(typeof(ProductType), className);
//			return db.Commit(() => db.Organization.ProviderListBy(productType).ToReferences());
//		}
//

		[WebMethod]
		public OperationStatus CanUpdate(object[] ids)
		{
			return db.Commit(() => db.Product.CanUpdate(ids));
		}

		[WebMethod]
		public object ChangeVoidStatus(object[] ids, RangeRequest @params)
		{
			return db.Commit(() => dc.Product.ChangeVoidStatus(ids, @params));
		}


		[WebMethod]
		public PasteboardDto GetPasteboard(object id)
		{
			return db.Commit(() => dc.Pasteboard.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePasteboard(PasteboardDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Pasteboard.Update(dto, @params));
		}

		[WebMethod]
		public PasteboardRefundDto GetPasteboardRefund(object id)
		{
			return db.Commit(() => dc.PasteboardRefund.By(id));
		}

		[WebMethod]
		public ItemResponse UpdatePasteboardRefund(PasteboardRefundDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.PasteboardRefund.Update(dto, @params));
		}


		[WebMethod]
		public SimCardDto GetSimCard(object id)
		{
			return db.Commit(() => dc.SimCard.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateSimCard(SimCardDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.SimCard.Update(dto, @params));
		}


		[WebMethod]
		public IsicDto GetIsic(object id)
		{
			return db.Commit(() => dc.Isic.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateIsic(IsicDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Isic.Update(dto, @params));
		}


		[WebMethod]
		public BusTicketDto GetBusTicket(object id)
		{
			return db.Commit(() => dc.BusTicket.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateBusTicket(BusTicketDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.BusTicket.Update(dto, @params));
		}

		[WebMethod]
		public BusTicketRefundDto GetBusTicketRefund(object id)
		{
			return db.Commit(() => dc.BusTicketRefund.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateBusTicketRefund(BusTicketRefundDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.BusTicketRefund.Update(dto, @params));
		}



		[WebMethod]
		public ExcursionDto GetExcursion(object id)
		{
			return db.Commit(() => dc.Excursion.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateExcursion(ExcursionDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Excursion.Update(dto, @params));
		}


		[WebMethod]
		public TourDto GetTour(object id)
		{
			return db.Commit(() => dc.Tour.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateTour(TourDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Tour.Update(dto, @params));
		}


		[WebMethod]
		public AccommodationDto GetAccommodation(object id)
		{
			return db.Commit(() => dc.Accommodation.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateAccommodation(AccommodationDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Accommodation.Update(dto, @params));
		}


		[WebMethod]
		public TransferDto GetTransfer(object id)
		{
			return db.Commit(() => dc.Transfer.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateTransfer(TransferDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Transfer.Update(dto, @params));
		}


		[WebMethod]
		public InsuranceDto GetInsurance(object id)
		{
			return db.Commit(() => dc.Insurance.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateInsurance(InsuranceDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.Insurance.Update(dto, @params));
		}

		[WebMethod]
		public InsuranceRefundDto GetInsuranceRefund(object id)
		{
			return db.Commit(() => dc.InsuranceRefund.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateInsuranceRefund(InsuranceRefundDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.InsuranceRefund.Update(dto, @params));
		}


		[WebMethod]
		public CarRentalDto GetCarRental(object id)
		{
			return db.Commit(() => dc.CarRental.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateCarRental(CarRentalDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.CarRental.Update(dto, @params));
		}


		[WebMethod]
		public GenericProductTypeDto GetGenericProductType(object id)
		{
			return db.Commit(() => dc.GenericProductType.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateGenericProductType(GenericProductTypeDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.GenericProductType.Update(dto, @params));
		}


		[WebMethod]
		public GenericProductDto GetGenericProduct(object id)
		{
			return db.Commit(() => dc.GenericProduct.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateGenericProduct(GenericProductDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.GenericProduct.Update(dto, @params));
		}

		[WebMethod]
		public EntityReference[] GetGenericProductTypes()
		{
			return db.Commit(() => db.GenericProductType.Query.OrderBy(a => a.Name).ToReferences());
		}


		[WebMethod]
		public CurrencyDailyRateDto GetCurrencyDailyRate(object id)
		{
			return db.Commit(() => dc.CurrencyDailyRate.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateCurrencyDailyRate(CurrencyDailyRateDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.CurrencyDailyRate.Update(dto, @params));
		}


		[WebMethod]
		public BankAccountDto GetBankAccount(object id)
		{
			return db.Commit(() => dc.BankAccount.By(id));
		}

		[WebMethod]
		public ItemResponse UpdateBankAccount(BankAccountDto dto, RangeRequest @params)
		{
			return db.Commit(() => dc.BankAccount.Update(dto, @params));
		}


		/// <summary>
		/// Не удалять!
		/// </summary>
		[WebMethod]
		public EntityReference XXX()
		{
			return null;
		}

	}

}