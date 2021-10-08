using System;
using System.Collections.Generic;
using System.Linq;

using Luxena.Domain.Contracts;




namespace Luxena.Travel.Domain
{



	public partial class AviaDocumentProcessDto : EntityContract
	{

		public ProductType Type { get; set; }

		public string AirlinePrefixCode { get; set; }

		public string Number { get; set; }

		public GdsOriginator Originator { get; set; }

		public string PassengerName { get; set; }

		public Party.Reference Customer { get; set; }

		public Party.Reference Intermediary { get; set; }

		public Person.Reference Seller { get; set; }

		public MoneyDto Commission { get; set; }

		public MoneyDto CommissionDiscount { get; set; }

		public decimal? CommissionPercent { get; set; }

		public MoneyDto Fare { get; set; }

		public MoneyDto EqualFare { get; set; }

		public MoneyDto Total { get; set; }

		public MoneyDto ServiceFee { get; set; }

		public MoneyDto Handling { get; set; }

		public MoneyDto Discount { get; set; }

		public MoneyDto GrandTotal { get; set; }

		public int PaymentType { get; set; }

		public bool? RequiresProcessing { get; set; }

		public bool? IsVoid { get; set; }

		public string Note { get; set; }

		public Order.Reference Order { get; set; }

		public string Name { get; set; }

	}






	public class AviaDocumentProcessContractService<TAviaDocument, TAviaDocumentService, TAviaDocumentProcessDto> : EntityContractService<TAviaDocument, TAviaDocumentService, TAviaDocumentProcessDto>
		where TAviaDocument : AviaDocument
		where TAviaDocumentService : EntityService<TAviaDocument>
		where TAviaDocumentProcessDto : AviaDocumentProcessDto, new()
	{

		public AviaDocumentProcessContractService()
		{

			ContractFromEntity += (r, c) =>
			{
				c.Type = r.Type;

				c.AirlinePrefixCode = r.AirlinePrefixCode;
				c.Number = r.Number;

				c.Originator = r.Originator;

				c.PassengerName = r.PassengerName;

				c.Customer = r.Customer;
				c.Intermediary = r.Intermediary;

				c.Seller = r.Seller;

				c.Fare = r.Fare;
				c.EqualFare = r.EqualFare;
				c.Commission = r.Commission;
				c.CommissionDiscount = r.CommissionDiscount;
				c.CommissionPercent = r.CommissionPercent;
				c.Total = r.Total ?? new Money(db.Configuration.DefaultCurrency);

				c.ServiceFee = r.ServiceFee;
				c.Handling = r.Handling;
				c.Discount = r.Discount;
				c.GrandTotal = r.GrandTotal;

				c.PaymentType = (int)r.PaymentType;

				c.RequiresProcessing = r.RequiresProcessing;
				c.IsVoid = r.IsVoid;

				c.Note = r.Note;

				c.Order = r.Order;

				c.Name = r.Name;
			};


			EntityFromContract += (r, c) =>
			{
				r.SetCustomer(db, c.Customer + db);
				r.Intermediary = c.Intermediary + db;

				r.EqualFare = r.EqualFare ?? c.EqualFare + db;

				r.Commission = c.Commission + db;
				r.ServiceFee = c.ServiceFee + db;
				r.Handling = c.Handling + db;
				r.CommissionDiscount = c.CommissionDiscount + db;
				r.Discount = c.Discount + db;
				r.GrandTotal = c.GrandTotal + db;

				r.Note = c.Note + db;

				r.PaymentType = (PaymentType)Enum.ToObject(typeof(PaymentType), c.PaymentType);

				r.SetOrder2(db, c.Order + db);
			};

		}



		public TAviaDocumentProcessDto[] Process(IList<TAviaDocumentProcessDto> dtos)
		{

			var documents = Service.ListByIds(dtos);

			db.AssertUpdate(documents);

			var resultList = new List<TAviaDocumentProcessDto>();

			foreach (var document in documents)
			{
				var doc = document;
				var dto = dtos.By(d => Equals(d.Id, doc.Id));

				Update(document, dto);

				resultList.Add(New(document));
			}


			return resultList.ToArray();

		}

	}






	public partial class AviaDocumentProcessContractService : EntityContractService
	{

		public AviaDocumentProcessDto New(AviaDocument r)
		{
			if (r.Type == ProductType.AviaTicket)
				return dc.AviaTicketProcess.New((AviaTicket)r);

			if (r.Type == ProductType.AviaRefund)
				return dc.AviaRefundProcess.New((AviaRefund)r);

			if (r.Type == ProductType.AviaMco)
				return dc.AviaMcoProcess.New((AviaMco)r);

			throw new NotImplementedException();
		}



		public AviaDocumentProcessDto ForHandlingByNumber(string number)
		{
			return New(db.AviaDocument.ForHandlingByNumber(number));
		}


		public AviaDocumentProcessDto[] ListForProcess(object[] ids, string className)
		{
			var documents = db.AviaDocument.ListByIds(ids);
			db.AssertUpdate(documents);

			return documents.Select(New).ToArray();
		}


		public AviaDocumentProcessDto[] AviaReservationsForProcess(object documentId)
		{
			var document = db.AviaDocument.By(documentId);
			db.AssertUpdate(document);

			var dtos = db.AviaDocument.GetReservationList(document).Select(New).ToArray();

			if (document.Type != ProductType.AviaRefund || dtos[0].Customer != null)
				return dtos;

			var ticket = db.AviaTicket.ByNumber(dtos[0].Number);
			if (ticket == null) return dtos;

			if (dtos[0].Customer == null)
				dtos[0].Customer = ticket.Customer;

			if (dtos[0].PaymentType == (int)Travel.Domain.PaymentType.Unknown)
				dtos[0].PaymentType = (int)ticket.PaymentType;

			return dtos;
		}

	}

}