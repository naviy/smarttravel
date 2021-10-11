using System;

using Luxena.Domain.Contracts;




namespace Luxena.Travel.Domain
{



	//===g






	public partial class InvoiceDto : EntityContract
	{

		public string Number { get; set; }

		public DateTime IssueDate { get; set; }

		public DateTime TimeStamp { get; set; }

		public Order.Reference Order { get; set; }

		public Person.Reference IssuedBy { get; set; }

		public MoneyDto Total { get; set; }

		public MoneyDto Vat { get; set; }

		public InvoiceType Type { get; set; }

		public string FileExtension { get; set; }

	}






	//===g






	public partial class InvoiceContractService : EntityContractService<Invoice, Invoice.Service, InvoiceDto>
	{

		//---g



		public InvoiceContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;
				c.IssueDate = r.IssueDate;
				c.TimeStamp = r.TimeStamp;
				c.Order = r.Order;
				c.IssuedBy = r.IssuedBy;
				c.Total = r.Total;
				c.Vat = r.Vat;
				c.Type = r.Type;
				c.FileExtension = r.FileExtension;
			};

			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;
				r.IssueDate = c.IssueDate + db;
				r.TimeStamp = c.TimeStamp + db;
				r.IssuedBy = c.IssuedBy + db;
				r.Total = c.Total + db;
				r.Vat = c.Vat + db;
				r.Type = c.Type + db;
				r.FileExtension = c.FileExtension + db;
			};

		}



		public InvoiceDto Issue(object orderId, string number, DateTime issueDate, int? formNumber, bool showPaid)
		{

			var order = db.Order.By(orderId);

			var r = db.Invoice.Issue(order, number, issueDate, formNumber, showPaid);


			return New(r);

		}



		public InvoiceDto IssueReceipt(object orderId)
		{
			var order = db.Order.By(orderId);

			var r = db.Invoice.IssueReceipt(order);

			return New(r);
		}



		public InvoiceDto IssueCompletionCertificate(object orderId, string number, DateTime issueDate, bool showPaid)
		{
			var order = db.Order.By(orderId);

			var r = db.Invoice.IssueCompletionCertificate(order, number, issueDate, showPaid);

			return New(r);
		}



		//---g

	}






	//===g



}