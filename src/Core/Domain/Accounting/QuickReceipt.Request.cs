using System;

using Luxena.Base.Data;
using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{
	[DataContract]
	public class QuickReceiptRequest
	{
		public EntityReference Customer { get; set; }
		public MoneyDto NotaxedAmount { get; set; }
		public MoneyDto TaxedAmount { get; set; }
		public EntityReference Owner { get; set; }
		public string Note { get; set; }


		public QuickReceiptResponse GetResponse(Domain db)
		{
			var order = new Order();

			order.SetCustomer(db, db.Party.Load(Customer));

			var notaxed = NotaxedAmount + db;
			var taxed = TaxedAmount + db ?? new Money(notaxed.Currency);

			var vat = taxed * db.Configuration.VatRate / (100 + db.Configuration.VatRate);
			vat.Amount = Math.Round(vat.Amount, 2);

			order.SetTotal(notaxed + taxed);
			order.SetVat(vat);
			order.SetDiscount(new Money(order.Total.Currency));

			order.Note = Note;
			order.Owner = db.Party.Load(Owner);

			db.Save(order);

			if (notaxed.Amount > 0)
				order.AddOrderItem(db, new OrderItem
				{
					Text = CommonRes.OrderItem_AviaTicketsSource,
					Price = notaxed,
					Quantity = 1
				});

			if (taxed.Amount > 0)
				order.AddOrderItem(db, new OrderItem
				{
					Text = CommonRes.OrderItem_AviaServiceFeeSource,
					Price = taxed,
					Quantity = 1
				});

			var receipt = db.Invoice.IssueReceipt(order);

			order.Items.Clear();

			var result = new QuickReceiptResponse
			{
				Order = order,
				Receipt = new EntityReference { Id = receipt.Id, Name = receipt.Number }
			};

			return result;
		}
	}

	[DataContract]
	public class QuickReceiptResponse
	{
		public Order.Reference Order { get; set; }
		public EntityReference Receipt { get; set; }
	}
}