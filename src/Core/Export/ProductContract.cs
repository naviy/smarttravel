using System;
using System.Linq;
using System.Runtime.Serialization;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Export
{



	//===g






	[DataContract]
	public class ProductContract : EntityContract
	{

		//---g



		[DataMember]
		public string Type { get; set; }
		[DataMember]
		public DateTime IssueDate { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public bool IsVoid { get; set; }
		[DataMember]
		public PartyReference Producer { get; set; }
		[DataMember]
		public PartyReference Provider { get; set; }
		[DataMember]
		public string TicketingIataOffice { get; set; }
		[DataMember]
		public string PassengerName { get; set; }
		[DataMember]
		public PartyReference[] Passengers { get; set; }
		[DataMember]
		public MoneyContract Fare { get; set; }
		[DataMember]
		public MoneyContract EqualFare { get; set; }
		[DataMember]
		public decimal? CommissionPercent { get; set; }
		[DataMember]
		public MoneyContract Commission { get; set; }
		[DataMember]
		public MoneyContract CommissionDiscount { get; set; }
		[DataMember]
		public MoneyContract FeesTotal { get; set; }
		[DataMember]
		public MoneyContract Vat { get; set; }
		[DataMember]
		public MoneyContract Total { get; set; }
		[DataMember]
		public MoneyContract TotalToTransfer { get; set; }
		[DataMember]
		public MoneyContract ServiceFee { get; set; }
		[DataMember]
		public MoneyContract Handling { get; set; }
		[DataMember]
		public MoneyContract HandlingN { get; set; }
		[DataMember]
		public MoneyContract Discount { get; set; }
		[DataMember]
		public MoneyContract GrandTotal { get; set; }
		[DataMember]
		public MoneyContract Profit { get; set; }
		[DataMember]
		public MoneyContract ExtraCharge { get; set; }
		[DataMember]
		public PaymentType PaymentType { get; set; }
		[DataMember]
		public PartyReference Seller { get; set; }
		[DataMember]
		public PartyReference Owner { get; set; }
		[DataMember]
		public PartyReference Intermediary { get; set; }
		[DataMember]
		public PartyReference Customer { get; set; }
		[DataMember]
		public EntityReference Order { get; set; }
		[DataMember]
		public string PnrCode { get; set; }
		[DataMember]
		public string TourCode { get; set; }
		[DataMember]
		public string Note { get; set; }
		[DataMember]
		public bool IsPaid { get; set; }



		//---g

	

		public ProductContract() { }



		public ProductContract(Product r) : base(r)
		{

			Type = r.Type.ToString();
			IssueDate = r.IssueDate;
			Name = r.Name;
			IsVoid = r.IsVoid;
			Producer = r.Producer;
			Provider = r.Provider;
			TicketingIataOffice = r.TicketingIataOffice;
			PassengerName = r.PassengerName;
			Passengers = r.Passengers.Select(a => (PartyReference)a.Passenger).ToArray();
			Fare = r.Fare;
			EqualFare = r.EqualFare;
			CommissionPercent = r.CommissionPercent;
			Commission = r.Commission;
			CommissionDiscount = r.CommissionDiscount;
			FeesTotal = r.FeesTotal;
			Vat = r.Vat;
			Total = r.Total;
			TotalToTransfer = r.TotalToTransfer;
			ServiceFee = r.ServiceFee;
			Handling = r.Handling;
			HandlingN = r.HandlingN;
			Discount = r.Discount;
			GrandTotal = r.GrandTotal;
			Profit = r.Profit;
			ExtraCharge = r.ExtraCharge;
			PaymentType = r.PaymentType;
			Seller = r.Seller;
			Owner = r.Owner;
			Intermediary = r.Intermediary ?? r.Order?.Intermediary;
			Customer = r.Customer;
			Order = r.Order;
			PnrCode = r.PnrCode;
			TourCode = r.TourCode;
			Note = r.Note;
			IsPaid = r.IsPaid;

		}



		//---g
		


		public virtual void AssignTo(Domain.Domain db, Product r)
		{

			r.IssueDate = IssueDate;
			r.Name = Name;
			r.IsVoid = IsVoid;

			r.Passengers = Passengers?.Where(a => a != null).Select(a => new ProductPassenger
			{
				Product = r,
				Passenger = db.Person.ByName(a.Text),
				PassengerName = a.Text
			}).ToList();

			r.PassengerName = PassengerName;

			r.Fare = Fare;
			r.EqualFare = EqualFare;
			r.CommissionPercent = CommissionPercent;
			r.Commission = Commission;
			r.CommissionDiscount = CommissionDiscount;
			r.FeesTotal = FeesTotal;
			r.Vat = Vat;
			r.Total = Total;
			r.ServiceFee = ServiceFee;
			r.Handling = Handling;
			r.HandlingN = HandlingN;
			r.Discount = Discount;
			r.GrandTotal = GrandTotal;
			r.PaymentType = PaymentType;
			r.Seller = db.Person.ByName(Seller?.Text);
			r.Owner = db.Party.ByName(Owner?.Text);
			r.Intermediary = db.Party.ByName(Intermediary?.Text);
			r.SetCustomer2(db.Party.ByName(Customer?.Text));
			r.PnrCode = PnrCode;
			r.TourCode = TourCode;
			r.Note = Note;

		}



		//---g

	}






	//===g



}