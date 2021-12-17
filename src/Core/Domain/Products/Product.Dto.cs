using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Base.Data;
using Luxena.Domain.Contracts;
using Luxena.Travel.Services;




namespace Luxena.Travel.Domain
{



	//===g






	public abstract partial class ProductDto : EntityContract
	{

		//---g



		public string Name { get; set; }

		public bool IsRefund { get; set; }

		public Product.Reference ReissueFor { get; set; }

		public Product.Reference ReissuedBy { get; set; }

		public Product.Reference RefundedProduct { get; set; }

		public Product.Reference Refund { get; set; }

		public DateTime IssueDate { get; set; }

		public bool RequiresProcessing { get; set; }

		public bool IsVoid { get; set; }

		public Organization.Reference Producer { get; set; }

		public Organization.Reference Provider { get; set; }

		public Party.Reference Customer { get; set; }

		public Party.Reference Intermediary { get; set; }

		public string Note { get; set; }


		public int Originator { get; set; }

		public string OriginString { get; set; }

		public string BookerOffice { get; set; }

		public string BookerCode { get; set; }

		public Person.Reference Booker { get; set; }

		public string TicketerOffice { get; set; }

		public string TicketerCode { get; set; }

		public Person.Reference Ticketer { get; set; }

		public GdsFile.Reference OriginalDocument { get; set; }


		public Person.Reference Seller { get; set; }

		public Party.Reference Owner { get; set; }

		public Organization.Reference LegalEntity { get; set; }

		public Country.Reference Country { get; set; }

		public string PnrCode { get; set; }

		public string TourCode { get; set; }

		public MoneyDto Fare { get; set; }

		public MoneyDto EqualFare { get; set; }

		public MoneyDto FeesTotal { get; set; }

		public MoneyDto ConsolidatorCommission { get; set; }

		public MoneyDto Total { get; set; }

		public MoneyDto Vat { get; set; }

		public MoneyDto Commission { get; set; }

		public MoneyDto CommissionDiscount { get; set; }

		public decimal CommissionPercent { get; set; }

		public MoneyDto CancelCommission { get; set; }

		public MoneyDto ServiceFee { get; set; }

		public MoneyDto Handling { get; set; }

		public MoneyDto HandlingN { get; set; }

		public MoneyDto Discount { get; set; }

		public MoneyDto BonusDiscount { get; set; }

		public MoneyDto BonusAccumulation { get; set; }

		public MoneyDto GrandTotal { get; set; }

		public int PaymentType { get; set; }

		public int TaxRateOfProduct { get; set; }

		public int TaxRateOfServiceFee { get; set; }

		public Order.Reference Order { get; set; }

		public OperationStatus CanUpdate { get; set; }

		public OperationStatus CanDelete { get; set; }


		public MoneyDto CancelFee { get; set; }

		public MoneyDto ServiceFeePenalty { get; set; }

		public MoneyDto RefundServiceFee { get; set; }



		//---g

	}






	public class ProductContractService<TProduct, TProductService, TProductDto>
		: EntityContractService<TProduct, TProductService, TProductDto>
		where TProduct : Product, new()
		where TProductService : Product.Service<TProduct>
		where TProductDto : ProductDto,  new()
	{

		//---g



		public ProductContractService()
		{

			ContractFromEntity += (r, c) =>
			{

				c.Name = r.Name;
				c.IsRefund = r.IsRefund;

				c.IssueDate = r.IssueDate;

				c.ReissueFor = r.ReissueFor;
				c.ReissuedBy = db.Product.By(a => a.ReissueFor == r);
				c.RefundedProduct = r.RefundedProduct;
				c.Refund = db.Product.By(a => a.RefundedProduct == r);

				c.RequiresProcessing = r.RequiresProcessing;
				c.IsVoid = r.IsVoid;

				c.Producer = r.Producer;
				c.Provider = r.Provider;

				c.Customer = r.Customer;
				c.Intermediary = r.Intermediary;

				c.Originator = (int)r.Originator;
				c.OriginString = $"{r.Originator} ({r.Origin.ToDisplayString()})";
				c.OriginalDocument = r.OriginalDocument;

				c.BookerOffice = r.BookerOffice;
				c.BookerCode = r.BookerCode;
				c.Booker = r.Booker;

				c.TicketerOffice = r.TicketerOffice;
				c.TicketerCode = r.TicketerCode;
				c.Ticketer = r.Ticketer;

				c.Seller = r.Seller;
				c.Order = r.Order;
				c.Owner = r.Owner;
				c.LegalEntity = r.LegalEntity;

				c.Country = r.Country;
				c.PnrCode = r.PnrCode;
				c.TourCode = r.TourCode;

				c.Note = r.Note;

				c.Fare = r.Fare;
				c.EqualFare = r.EqualFare;
				c.FeesTotal = r.FeesTotal;
				c.ConsolidatorCommission = r.ConsolidatorCommission;
				c.Total = r.Total.Else(r.GetTotal);

				c.Vat = r.Vat;
				c.Commission = r.Commission;
				c.CommissionDiscount = r.CommissionDiscount;
				c.CancelCommission = r.CancelCommission;

				if (r.CommissionPercent.Yes())
					c.CommissionPercent = r.CommissionPercent ?? 0;

				c.Discount = r.Discount;

				//			if (!db.IsNew(r))
				//			{
				c.BonusDiscount = r.BonusDiscount;
				//			}
				//			else if (r.Customer != null)
				//			{
				//				var amount = (
				//					from a in db.Product.Query
				//					where a.Customer.Id == r.Customer.Id // && a.Id != r.Id
				//					select (a.BonusAccumulation != null ? a.BonusAccumulation.Amount : 0m) - (a.BonusDiscount != null ? a.BonusDiscount.Amount : 0m)
				//				).Sum();
				//				BonusDiscount = new MoneyDto(amount, db.Configuration.DefaultCurrency);
				//			}

				c.BonusAccumulation = r.BonusAccumulation;
				c.ServiceFee = r.ServiceFee;
				c.Handling = r.Handling;
				c.HandlingN = r.HandlingN;

				c.CancelFee = r.CancelFee;
				c.ServiceFeePenalty = r.ServiceFeePenalty;
				c.RefundServiceFee = r.RefundServiceFee;

				c.GrandTotal = r.GrandTotal.Else(r.GetGrandTotal);

				c.PaymentType = (int)r.PaymentType;
				c.TaxRateOfProduct = (int)r.TaxRateOfProduct;
				c.TaxRateOfServiceFee = (int)r.TaxRateOfServiceFee;

				c.CanUpdate = db.CanUpdate(r);
				c.CanDelete = db.CanDelete(r);

			};


			EntityFromContract += (r, c) =>
			{

				r.IssueDate = c.IssueDate + db;

				r.ReissueFor = c.ReissueFor + db;
				r.RefundedProduct = c.RefundedProduct + db;

				r.Producer = c.Producer + db;
				r.Provider = c.Provider + db;

				r.SetCustomer(db, c.Customer + db);
				r.Intermediary = c.Intermediary + db;

				r.Originator = (GdsOriginator)c.Originator;

				r.Booker = c.Booker + db;
				r.BookerOffice = c.BookerOffice + db;
				r.BookerCode = c.BookerCode + db;

				r.Ticketer = c.Ticketer + db;
				r.TicketerOffice = c.TicketerOffice + db;
				r.TicketerCode = c.TicketerCode + db;

				r.Seller = c.Seller + db;
				r.Owner = c.Owner + db;
				r.LegalEntity = c.LegalEntity + db;

				r.Country = c.Country + db;
				r.PnrCode = c.PnrCode + db;
				r.TourCode = c.TourCode + db;

				r.Fare = c.Fare + db;
				r.EqualFare = c.EqualFare + db;
				r.FeesTotal = c.FeesTotal + db;
				r.ConsolidatorCommission = c.ConsolidatorCommission + db;
				r.Total = c.Total + db;
				r.Vat = c.Vat + db;
				r.Commission = c.Commission + db;
				r.CommissionDiscount = c.CommissionDiscount + db;
				r.CancelCommission = c.CancelCommission + db;
				r.ServiceFee = c.ServiceFee + db;
				r.Handling = c.Handling + db;
				r.HandlingN = c.HandlingN + db;
				r.Discount = c.Discount + db;
				r.BonusDiscount = c.BonusDiscount + db;
				r.BonusAccumulation = c.BonusAccumulation + db;

				r.CancelFee = c.CancelFee + db;
				r.RefundServiceFee = c.RefundServiceFee + db;
				r.ServiceFeePenalty = c.ServiceFeePenalty + db;

				r.GrandTotal = c.GrandTotal + db;

				r.Note = c.Note + db;

				r.PaymentType = (PaymentType)c.PaymentType + db;
				r.TaxRateOfProduct = (TaxRate)c.TaxRateOfProduct;
				r.TaxRateOfServiceFee = (TaxRate)c.TaxRateOfServiceFee;

				r.SetOrder2(db, c.Order + db);

			};

		}



		//---g



		protected override void AssertCreate(TProductDto c)
		{
			base.AssertCreate(c);

			if (!db.ClosedPeriod.IsOpened(c.IssueDate))
				throw new DocumentClosedException(Exceptions.Document_Closed);
		}



		protected ProductPassengerDto[] NewPassengers(IList<ProductPassenger> passengers)
		{
			return dc.ProductPassenger.New(passengers, list =>
				list.OrderBy(a => a.PassengerName ?? a.Passenger.As(b => b.Name))
			);
		}



		//---g

	}






	public partial class ProductContractService : EntityContractService
	{

		//---g



		public virtual ProductDto By(object id)
		{
			return New(db.Product.By(id));
		}



		public ProductDto New(Product r)
		{
			return r == null ? null : _newByProductType[(int)r.Type](dc, r);
		}



		public object ChangeVoidStatus(object[] ids, RangeRequest prms)
		{

			var products = db.Product.ListByIds(ids);

			if (products.Count != ids.Length)
				throw new ObjectsNotFoundException(ids.Length == 1 ? Exceptions.NoRowById_Translation : Exceptions.ObjectsNotFound_Error);


			db.Product.AssertUpdate(products);

			object result = products
				.Select(a =>
				{
					a.AddVoidStatus(db, !a.IsVoid);
					return New(a);
				})
				.ToArray()
			;


			if (prms != null)
			{
				prms.PositionableObjectId = products[0].Id;

				result = new[] { result, db.GetRange<Product>(prms) };
			}


			return result;

		}



		// ReSharper disable once RedundantExplicitArraySize
		private static readonly Func<Contracts, Product, ProductDto>[] _newByProductType = new Func<Contracts, Product, ProductDto>[(int)ProductTypes.MaxValue + 1]
		{

			(dc, r) => dc.AviaTicket.New((AviaTicket)r),
			(dc, r) => dc.AviaRefund.New((AviaRefund)r),
			(dc, r) => dc.AviaMco.New((AviaMco)r),
			(dc, r) => dc.Pasteboard.New((Pasteboard)r),
			(dc, r) => dc.SimCard.New((SimCard)r),
			(dc, r) => dc.Isic.New((Isic)r),
			(dc, r) => dc.Excursion.New((Excursion)r),
			(dc, r) => dc.Tour.New((Tour)r),
			(dc, r) => dc.Accommodation.New((Accommodation)r),
			(dc, r) => dc.Transfer.New((Transfer)r),
			(dc, r) => dc.Insurance.New((Insurance)r),
			(dc, r) => dc.CarRental.New((CarRental)r),
			(dc, r) => dc.GenericProduct.New((GenericProduct)r),
			(dc, r) => dc.BusTicket.New((BusTicket)r),
			(dc, r) => dc.PasteboardRefund.New((PasteboardRefund)r),
			(dc, r) => dc.InsuranceRefund.New((InsuranceRefund)r),
			(dc, r) => dc.BusTicketRefund.New((BusTicketRefund)r),

		};



		//---g

	}






	//===g



}