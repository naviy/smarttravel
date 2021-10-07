using System.Collections.Generic;
using System.Text;

using Luxena.Domain;
using Luxena.Travel.Reports;


namespace Luxena.Travel.Domain
{

	[RU("ПКО")]
	public partial class CashInOrderPayment : Payment
	{

		[SemanticSetup]
		public static void AnnotationSetup(SemanticSetup<CashInOrderPayment> se)
		{
			se.For(a => a.DocumentNumber)
				.RU("№ ПКО");
		}


		public override PaymentForm PaymentForm => PaymentForm.CashInOrder;

		public override string DocumentUniqueCode => 
			IsVoid || DocumentNumber.No() ? null : $"{DocumentNumber}_{Date:yyyy}";

		public override void SetPostedOn()
		{
			if (SavePosted)
				Post();
		}


		public virtual string GetReason()
		{
			if (Order.Items.Count == 0)
				return Order.Note ?? DomainRes.CashInOrderPayment_DefaultReason;

			var list = new List<Product>();
			var itemText = new List<string>();

			foreach (var item in Order.Items)
			{
				if (item.Product == null)
					itemText.Add(item.Text.Length > 0 ? item.Text.ToLowerFirstLetter() : string.Empty);
				else if (!list.Contains(item.Product))
					list.Add(item.Product);
			}

			var builder = new StringBuilder();

			if (list.Count > 0)
			{
				builder.Append(list.Count == 1 ? DomainRes.AviaTicket : DomainRes.AviaTickets);

				var sep = list.Count > 0 ? " № " : string.Empty;

				foreach (var doc in list)
				{
					builder.Append(sep).Append(doc.ToString());
					sep = ", ";
				}

				foreach (var text in itemText)
				{
					builder.Append(sep).Append(text);
					sep = ", ";
				}
			}
			else
				foreach (var text in itemText)
					builder.Append(text);

			return builder.ToString();
		}


		public new class Service : Service<CashInOrderPayment>
		{

			#region Read

			public byte[] GetFile(object paymentId)
			{
				var r = By(paymentId);
				if (r.PrintedDocument == null)
					Issue(r);
				return r.PrintedDocument;
			}

			#endregion


			public Service()
			{

				Calculating += r =>
				{
					//if (r.Number.No())
					//	r.Number = NewSequence();
				};

			}

			
			public void Issue(CashInOrderPayment r)
			{
				AssertUpdate(r);

				r.SetDate(db, r.Date);

				r.PrintedDocument = db.Resolve<ICashOrderForm>().As(a => a.Print(r));

				Export(r);
			}

		}
	}

}