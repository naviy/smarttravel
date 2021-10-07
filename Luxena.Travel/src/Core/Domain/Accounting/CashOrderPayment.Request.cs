using System;

using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public class CashInOrderPaymentRequest
	{

		public DateTime Date { get; set; }

		public Party.Reference Payer { get; set; }

		public string Number { get; set; }

		public MoneyDto Amount { get; set; }

		public MoneyDto OrderVat { get; set; }

		public MoneyDto PaymentVat { get; set; }

		public string ReceivedFrom { get; set; }

		public Party.Reference Owner { get; set; }

		public string Note { get; set; }

		public bool IsPosted { get; set; }

		public bool SeparateServiceFee { get; set; }

		public object[] DocumentIds { get; set; }

		public bool IsAdditionalPayment { get; set; }

		public bool CreateConsignment { get; set; }



		public Order CreateOrder(Domain db)
		{

			var order = new Order
			{
				Owner = Owner + db
			};
			order.SetCustomer(db, Payer + db);

			db.Save(order);


			var docs = db.AviaDocument.ListByIds(DocumentIds);
			order.Add(db, docs, SeparateServiceFee ? ServiceFeeMode.Separate : ServiceFeeMode.Join);


			return order;

		}


		public CashPaymentResponse GetResponse(Domain db, Contracts dc)
		{
			var r = new CashInOrderPayment();

			r.SetDate(db, Date + db);
			r.DocumentNumber = Number + db;
			r.SetPayer(Payer + db);
			r.SetAmount(Amount + db);
			r.SetVat(PaymentVat + db);
			r.ReceivedFrom = ReceivedFrom + db;
			r.Note = Note + db;

			if (IsPosted)
				r.SetPostedOn(Date + db);

			r.Owner = Owner + db;

			var order = CreateOrder(db);

			r.SetOrder(order);

			r.AssignedTo |= order.As(a => a.AssignedTo) ?? db.Security.Person;

			db.Save(r);

			Consignment consignment = null;

			if (CreateConsignment)
			{
				consignment = db.Consignment.Create(order);

				db.Issue(consignment);
			}

			var dto = new CashPaymentResponse(dc, r, consignment);

			return dto;
		}

	}

}