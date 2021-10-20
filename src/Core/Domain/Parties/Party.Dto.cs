using System;
using System.Linq;
using Luxena.Base.Data;
using Luxena.Base.Metamodel;
using Luxena.Domain.Contracts;




namespace Luxena.Travel.Domain
{



	//===g






	public abstract class PartyDto : EntityContract
	{

		//---g



		public string Type { get; set; }


		public string Text => Name;

		public string Name { get; set; }

		public string LegalName { get; set; }

		public string Signature { get; set; }


		public string BonusCardNumber { get; set; }

		public decimal? BonusAmount { get; set; }


		public string Phone1 { get; set; }

		public string Phone2 { get; set; }

		public string Fax { get; set; }

		public string Email1 { get; set; }

		public string Email2 { get; set; }

		public string WebAddress { get; set; }

		public bool IsCustomer { get; set; }

		public bool CanNotBeCustomer { get; set; }

		public bool IsSupplier { get; set; }

		public Party.Reference ReportsTo { get; set; }
		public BankAccount.Reference DefaultBankAccount { get; set; }

		public string Details { get; set; }

		public string InvoiceSuffix { get; set; }

		public string LegalAddress { get; set; }

		public string ActualAddress { get; set; }

		public string Note { get; set; }

		public FileDto[] Files { get; set; }

		public PartyProductDto[] Products { get; set; }
		public int ProductCount { get; set; }

		public PartyOrderDto[] Orders { get; set; }
		public int OrderCount { get; set; }

		public PartyInvoiceDto[] Invoices { get; set; }
		public int InvoiceCount { get; set; }

		//public OperationPermissions Permissions { get; set; }

		public PartyBalance Balance { get; set; }



		//---g

	}






	public partial class PartyContractService<TParty, TPartyService, TPartyContract>
			: EntityContractService<TParty, TPartyService, TPartyContract>
		where TParty : Party
		where TPartyService : Party.Service<TParty>
		where TPartyContract : PartyDto, new()
	{

		//---g



		public PartyContractService()
		{

			ContractFromEntity += (r, c) =>
			{
				c.Type = Class.Of(r).Id;

				c.Name = r.Name;
				c.LegalName = r.LegalName;
				c.Signature = r.Signature;

				c.BonusCardNumber = r.BonusCardNumber;
				c.BonusAmount = r.BonusAmount;

				c.Phone1 = r.Phone1;
				c.Phone2 = r.Phone2;
				c.Fax = r.Fax;

				c.Email1 = r.Email1;
				c.Email2 = r.Email2;
				c.WebAddress = r.WebAddress;

				c.Details = r.Details;
				c.InvoiceSuffix = r.InvoiceSuffix;

				c.LegalAddress = r.LegalAddress;
				c.ActualAddress = r.ActualAddress;

				c.IsCustomer = r.IsCustomer;
				c.CanNotBeCustomer = r.CanNotBeCustomer;
				c.IsSupplier = r.IsSupplier;

				c.Note = r.Note;

				c.Files = dc.File.New(r.Files);

				c.ProductCount = db.Product.Query.Count(a => a.Customer.Id == r.Id && !a.IsVoid);
				c.Products = dc.PartyProduct.New(db.Product.Query
					.Where(a => a.Customer.Id == r.Id && !a.IsVoid)
					.OrderByDescending(a => a.IssueDate)
					.Take(20).ToList()
				);

				c.OrderCount = db.Order.Query.Count(a => a.Customer.Id == r.Id && !a.IsVoid);
				c.Orders = dc.PartyOrder.New(db.Order.Query
					.Where(a => a.Customer.Id == r.Id && !a.IsVoid)
					.OrderByDescending(a => a.IssueDate)
					.Take(20).ToList()
				);

				c.InvoiceCount = db.Invoice.Query.Count(a => a.Order.Customer.Id == r.Id && !a.Order.IsVoid);
				c.Invoices = dc.PartyInvoice.New(db.Invoice.Query
					.Where(a => a.Order.Customer.Id == r.Id && !a.Order.IsVoid)
					.OrderByDescending(a => a.IssueDate)
					.Take(20).ToList()
				);

				c.ReportsTo = r.ReportsTo;
				c.DefaultBankAccount = r.DefaultBankAccount;

				c.Balance = PartyBalance.By(db, dc, r);

			};



			EntityFromContract += (r, c) =>
			{

				r.Name = c.Name + db;
				r.LegalName = c.LegalName + db;
				r.Signature = c.Signature + db;

				r.BonusCardNumber = c.BonusCardNumber + db;
				r.BonusAmount = c.BonusAmount + db;

				r.Phone1 = c.Phone1 + db;
				r.Phone2 = c.Phone2 + db;
				r.Fax = c.Fax + db;

				r.Email1 = c.Email1 + db;
				r.Email2 = c.Email2 + db;
				r.WebAddress = c.WebAddress + db;

				r.Details = c.Details + db;
				r.InvoiceSuffix = c.InvoiceSuffix + db;

				r.LegalAddress = c.LegalAddress + db;
				r.ActualAddress = c.ActualAddress + db;

				r.IsCustomer = c.IsCustomer + db;
				r.CanNotBeCustomer = c.CanNotBeCustomer + db;
				r.IsSupplier = c.IsSupplier + db;

				r.Note = c.Note + db;

				r.ReportsTo = c.ReportsTo + db;
				r.DefaultBankAccount = c.DefaultBankAccount + db;

			};

		}



		//---g

	}






	//===g






	public abstract class PartyListDetailDto : EntityContract
	{

		public string Type { get; set; }

		public string Name { get; set; }

		public string Phone1 { get; set; }

		public string Email1 { get; set; }

	}






	public partial class PartyListDetailContractService<TParty, TPartyService, TPartyContract>
			: EntityContractService<TParty, TPartyService, TPartyContract>
		where TParty : Party
		where TPartyService : Party.Service<TParty>
		where TPartyContract : PartyListDetailDto, new()
	{
		public PartyListDetailContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Type = _partyTypeNames[(int)r.Type];
				c.Name = r.Name;
				c.Phone1 = r.Phone1;
				c.Email1 = r.Email1;
			};
		}

		static readonly string[] _partyTypeNames = Enum.GetNames(typeof (PartyType));
	}






	//===g






	public partial class PartyOrderDto : EntityContract
	{

		public string Type => nameof(Order);

		public string Text => Number;

		public string Number { get; set; }

		public DateTime IssueDate { get; set; }

		public MoneyDto Total { get; set; }

		public MoneyDto Paid { get; set; }

		public MoneyDto TotalDue { get; set; }

	}






	public partial class PartyOrderContractService : EntityContractService<Order, Order.Service, PartyOrderDto>
	{

		public PartyOrderContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;

				c.Total = r.Total;
				c.Paid = r.Paid;
				c.TotalDue = r.TotalDue;
			};
		}

	}






	//===g






	public partial class PartyInvoiceDto : EntityContract
	{

		public string Number { get; set; }

		public DateTime IssueDate { get; set; }

		public InvoiceType Type { get; set; }

		public DateTime TimeStamp { get; set; }

		public Order.Reference Order { get; set; }

		public MoneyDto Total { get; set; }

		public MoneyDto Vat { get; set; }

	}






	public partial class PartyInvoiceContractService : EntityContractService<Invoice, Invoice.Service, PartyInvoiceDto>
	{

		public PartyInvoiceContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;
				c.TimeStamp = r.TimeStamp;
				c.Order = r.Order;
				c.Total = r.Total;
				c.Vat = r.Vat;
				c.Type = r.Type;
			};
		}

	}






	//===g






	public partial class PartyProductDto : EntityContract
	{

		public string Type { get; set; }

		public string Name { get; set; }

		public bool IsRefund { get; set; }

		public DateTime IssueDate { get; set; }

		public Organization.Reference Provider { get; set; }

		public Order.Reference Order { get; set; }

		public Person.Reference Seller { get; set; }

		public Party.Reference Owner { get; set; }

		public MoneyDto Fare { get; set; }

		public MoneyDto Total { get; set; }

		public MoneyDto ServiceFee { get; set; }

		public MoneyDto GrandTotal { get; set; }

		public string Itinerary { get; set; }

	}






	public partial class PartyProductContractService : EntityContractService<Product, Product.Service, PartyProductDto>
	{
		
		public PartyProductContractService()
		{

			ContractFromEntity += (r, c) =>
			{
				c.Type = Enum.GetName(typeof (ProductType), r.Type);
				c.Name = r.Name;
				c.IsRefund = r.IsRefund;

				c.IssueDate = r.IssueDate;

				c.Provider = r.Provider ?? r.Producer;
				c.Seller = r.Seller;
				c.Order = r.Order;
				c.Owner = r.Owner;

				if (r is AviaDocument ad)
					c.Itinerary = ad.Itinerary;

				c.Fare = r.Fare;
				c.Total = r.Total.Else(r.GetTotal);

				c.ServiceFee = r.ServiceFee;

				c.GrandTotal = r.GrandTotal.Else(r.GetGrandTotal);
			};

		}

	}






	//===g



}