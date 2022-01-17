using System;
using System.IO;
using System.Linq;

using Luxena.Travel.Domain;




namespace Luxena.Travel.Reports
{



	//===g






	public class TemplateInvoicePrinter : TemplatePrinter, IInvoicePrinter
	{

		//---g



		public Domain.Domain db { get; set; }



		public string TemplateFileName1 { get; set; }

		public string TemplateFileName2 { get; set; }


		public int FormNumber { get; set; }

		public string ServiceFeeTitle { get; set; }



		//---g



		public byte[] Build(
			Order order,
			string number,
			DateTime issueDate,
			Person issuedBy,
			int? formNumber,
			bool showPaid,
			out string fileExtension
		)
		{

			var stream = (formNumber ?? FormNumber) == 2
				? GetStream2(order, number, issueDate, issuedBy, showPaid, out fileExtension)
				: GetStream1(order, number, issueDate, issuedBy, showPaid, out fileExtension)
			;

			return stream.ToBytes();

		}



		private Stream GetStream1(Order order, string number, DateTime issueDate, Person issuedBy, bool showPaid, out string fileExtension)
		{

			var pos = 1;

			var shipTo = (order.ShipTo ?? order.Customer).As(a => a.NameForDocuments);

			var billTo = order.BillTo == null && order.BillToName.Yes()
				? order.BillToName
				: (order.BillTo ?? order.Customer).As(a => a.NameForDocuments);


			if (shipTo == billTo)
				billTo = ReportRes.InvoicePrinter_Same;



			var data = new
			{

				Number = number,
				IssueDate = issueDate,
				OrderNo = order.Number,

				Supplier = db.Configuration.GetSupplierDetails(db, order),
				ShipTo = shipTo,
				BillTo = billTo,

				ItemCount = order.Items.Count,


				Items = order.Items
					.OrderBy(a => a.Position)
					.Select(a => new
					{
						Position = pos++,
						Text = a.IsServiceFee && ServiceFeeTitle.Yes() ? ServiceFeeTitle : a.Text,
						a.Quantity,
						Price = a.Price.AsAmount(),
						Total = a.Total.AsAmount(),
					})
					.ToArray()
				,


				Totals = new TotalSums().Do(totals =>
				{

					totals.Add(order.Discount, ReportRes.InvoicePrinter_Discount, skipIfEmpty: true);

					totals.Add(order.Total, ReportRes.InvoicePrinter_InvoiceTotalWithVat);

					
					if (db.Configuration.InvoicePrinter_ShowVat)
					{
						totals.Add(order.Vat, ReportRes.InvoicePrinter_Vat);
					}


					if (showPaid && !Equals(order.Total, order.TotalDue))
					{

						totals.Add(order.Paid, ReportRes.InvoicePrinter_Paid);

						totals.Add(order.TotalDue, ReportRes.InvoicePrinter_TotalDue);


						if (db.Configuration.InvoicePrinter_ShowVat)
						{
							totals.Add(order.VatDue, ReportRes.InvoicePrinter_Vat);
						}

					}

				}),


				TotalWords = (showPaid ? order.TotalDue : order.Total).ToWords(),

				Warning1 = db.Configuration.VatRate > 0 ? ReportRes.InvoicePrinter_VatLawChanged : null,

				IssuedBy = issuedBy.NameForDocuments,

				FooterDetails = db.Configuration.InvoicePrinter_FooterDetails,

			};



			return Build(
				TemplateFileName1 ?? TemplateFileName ?? "~/static/reports/InvoiceTemplate1.xlsx",
				data,
				out fileExtension
			);

		}



		private Stream GetStream2(Order order, string number, DateTime issueDate, Person issuedBy, bool showPaid, out string fileExtension)
		{

			var pos = 1;

			var shipTo = (order.ShipTo ?? order.Customer).As(a => a.NameForDocuments);


			var billTo = order.BillTo == null && order.BillToName.Yes()
				? order.BillToName
				: (order.BillTo ?? order.Customer).As(a => a.NameForDocuments);


			if (shipTo == billTo)
				billTo = ReportRes.InvoicePrinter_Same;


			var vatRate = db.Configuration.VatRate / 100;

			var orderHasVat = order.Vat.Yes();



			var data = new
			{

				Number = number,
				IssueDate = issueDate,
				OrderNo = order.Number,

				Supplier = db.Configuration.GetSupplierDetails(db, order),
				ShipTo = shipTo,
				BillTo = billTo,

				ItemCount = order.Items.Count,


				Items = order.Items
					.Where(a => !a.IsServiceFee)
					.OrderBy(a => a.Position)
					.Select(a =>
					{
						var serviceFee = a.ServiceFee.AsAmount();
						var price = a.Product?.Total.AsAmount() ?? a.Total.AsAmount();

						var vat = orderHasVat ? Math.Round(serviceFee * vatRate / (1 + vatRate), 2) : 0;


						var grandTotal = a.Product?.GrandTotal.AsAmount() ?? a.GrandTotal.AsAmount();


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
							GrandTotal = grandTotal,
							GrandTotal2 = grandTotal, // у шаблонизатора проблемы с использованием одиного поля несколько раз
						};
					})
					.ToArray()
				,


				Totals = new TotalSums().Do(totals =>
				{

					totals.Add(order.Discount, ReportRes.InvoicePrinter_Discount, skipIfEmpty: true);

					totals.Add(order.Total, ReportRes.InvoicePrinter_InvoiceTotalWithVat);


					if (db.Configuration.InvoicePrinter_ShowVat)
					{
						totals.Add(order.Vat, ReportRes.InvoicePrinter_Vat);
					}


					if (showPaid && !Equals(order.Total, order.TotalDue))
					{

						totals.Add(order.Paid, ReportRes.InvoicePrinter_Paid);

						totals.Add(order.TotalDue, ReportRes.InvoicePrinter_TotalDue);


						if (db.Configuration.InvoicePrinter_ShowVat)
						{
							totals.Add(order.VatDue, ReportRes.InvoicePrinter_Vat);
						}

					}

				}),


				TotalWords = (showPaid ? order.TotalDue : order.Total).ToWords(),

				Warning1 = db.Configuration.VatRate > 0 ? ReportRes.InvoicePrinter_VatLawChanged : null,

				IssuedBy = issuedBy.NameForDocuments,

				FooterDetails = db.Configuration.InvoicePrinter_FooterDetails,

			};



			return Build(
				TemplateFileName2 ?? TemplateFileName ?? "~/static/reports/InvoiceTemplate2.xlsx",
				data,
				out fileExtension
			);

		}



		//---g

	}






	//===g



}
