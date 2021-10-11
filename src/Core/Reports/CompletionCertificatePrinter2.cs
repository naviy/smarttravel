using System;
using System.IO;
using System.Linq;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Reports
{



	//===g





	public class CompletionCertificatePrinter : TemplatePrinter, ICompletionCertificatePrinter
	{

		//---g



		public Domain.Domain db { get; set; }



		//---g



		public byte[] Build(Order order, string number, DateTime issueDate, Person issuedBy, bool showPaid)
		{

			var stream = GetStream2(order, number, issueDate, issuedBy, showPaid);

			return stream.ToBytes();

		}



		private Stream GetStream2(Order order, string number, DateTime issueDate, Person issuedBy, bool showPaid)
		{

			var pos = 1;


			var shipTo = (order.ShipTo ?? order.Customer).As(a => a.NameForDocuments);

			var billTo = order.BillTo == null && order.BillToName.Yes()
				? order.BillToName
				: (order.BillTo ?? order.Customer).As(a => a.NameForDocuments)
			;

			if (shipTo == billTo)
				billTo = ReportRes.InvoicePrinter_Same;


			var vatRate = db.Configuration.VatRate / 100;

			var orderHasVat = order.Vat.Yes();


			var data = new
			{

				Number = number,
				IssueDate = issueDate,
				OrderNo = order.Number,

				Supplier = db.Configuration.Company,
				Customer = order.ShipTo ?? order.Customer,
				SupplierDetails = db.Configuration.GetSupplierDetails(db, order),
				CustomerDetails = db.Configuration.GetCustomerDetails(db, order.ShipTo ?? order.Customer),

				ShipTo = shipTo,
				BillTo = billTo,


				ItemCount = order.Items.Count,

				Items = order.Items

					.Where(a => a.LinkType != OrderItemLinkType.ServiceFee)

					.OrderBy(a => a.Position)

					.Select(a =>
					{

						var serviceFee = a.ServiceFee.AsAmount();
						var price = a.Product?.Total.AsAmount() ?? a.Total.AsAmount();

						var vat = orderHasVat ? Math.Round(serviceFee * vatRate / (1 + vatRate), 2) : 0;


						return new
						{
							Position = pos++,
							a.Text,
							a.Quantity,
							Price = price,
							Total = a.Quantity * price,
							ServiceFee0 = (serviceFee - vat).If(b => b != 0),
							Vat = vat.If(b => b != 0),
							ServiceFee = serviceFee.If(b => b != 0),
							GrandTotal = a.Product?.GrandTotal.AsAmount() ?? a.GrandTotal.AsAmount(),
						};

					})

					.ToArray()
				,


				Totals = new TotalSums().Do(totals =>
				{

					totals
						.Add(order.Discount, ReportRes.InvoicePrinter_Discount, skipIfEmpty: true)
						.Add(order.Total, ReportRes.InvoicePrinter_InvoiceTotalWithVat)
						.Add(order.Vat, ReportRes.InvoicePrinter_Vat)
					;


					if (showPaid && !Equals(order.Total, order.TotalDue))
					{
						totals
							.Add(order.Paid, ReportRes.InvoicePrinter_Paid)
							.Add(order.TotalDue, ReportRes.InvoicePrinter_TotalDue)
							.Add(order.VatDue, ReportRes.InvoicePrinter_Vat)
						;
					}

				}),


				TotalWords = (showPaid ? order.TotalDue : order.Total).ToWords(),

				Warning1 = db.Configuration.VatRate > 0 ? ReportRes.InvoicePrinter_VatLawChanged : null,

				IssuedBy = issuedBy.NameForDocuments,

				FooterDetails = db.Configuration.InvoicePrinter_FooterDetails,

			};


			return Build(
				TemplateFileName ?? "~/static/reports/CompletionCertificateTemplate2.xlsx", 
				data, 
				out _
			);

		}



		//---g

	}






	//===g



}
