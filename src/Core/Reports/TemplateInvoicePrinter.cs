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


		public int FormNumber { get; set; }

		public string ServiceFeeTitle { get; set; }


		public string TemplateFileName1
		{
			get => _templateFileName1 ?? TemplateFileName ?? "~/static/reports/InvoiceTemplate1.xlsx";
			set => _templateFileName1 = value;
		}
		private string _templateFileName1;


		public string TemplateFileName11
		{
			get => _templateFileName11 ?? TemplateFileName1;
			set => _templateFileName11 = value;
		}
		private string _templateFileName11;


		public string TemplateFileName2
		{
			get => _templateFileName2 ?? TemplateFileName ?? "~/static/reports/InvoiceTemplate2.xlsx";
			set => _templateFileName2 = value;
		}
		private string _templateFileName2;


		public string TemplateFileName21
		{
			get => _templateFileName21 ?? TemplateFileName2;
			set => _templateFileName21 = value;
		}
		private string _templateFileName21;



		//---g



		public byte[] Build(
			Order order,
			string number,
			DateTime issueDate,
			Person issuedBy,
			Party owner,
			BankAccount bankAccount,
			int? formNumber,
			bool showPaid,
			out string fileExtension
		)
		{

			var e = new StreamParams
			{
				Order = order,
				Number = number,
				IssueDate = issueDate,
				IssuedBy = issuedBy,
				Owner = owner,
				BankAccount = bankAccount,
				ShowPaid = showPaid,
			};


			var fn = formNumber ?? FormNumber;


			if (fn == 11)
			{
				return GetStream2(e, TemplateFileName11, out fileExtension);
			}

			if (fn == 2)
			{
				return GetStream2(e, TemplateFileName2, out fileExtension);
			}

			if (fn == 21)
			{
				return GetStream2(e, TemplateFileName21, out fileExtension);
			}

			return GetStream1(e, TemplateFileName1, out fileExtension);

		}



		//---g




		class StreamParams
		{
			public Order Order;
			public string Number;
			public DateTime IssueDate;
			public Person IssuedBy;
			public Party Owner;
			public BankAccount BankAccount;
			public bool ShowPaid;
		}



		private byte[] GetStream1(
			StreamParams e,
			string templateFileName,
			out string fileExtension
		)
		{


			var configuration = db.Configuration;


			var pos = 1;

			var shipTo = (e.Order.ShipTo ?? e.Order.Customer).As(a => a.NameForDocuments);

			var billTo = e.Order.BillTo == null && e.Order.BillToName.Yes()
				? e.Order.BillToName
				: (e.Order.BillTo ?? e.Order.Customer).As(a => a.NameForDocuments)
			;


			if (shipTo == billTo)
			{
				billTo = ReportRes.InvoicePrinter_Same;
			}


			var supplierDetails = configuration.GetSupplierDetails(db, e.Order, owner: e.Owner, bankAccount: e.BankAccount, multiline: true);

			var customerDetails = configuration.GetCustomerDetails(db, e.Order.ShipTo ?? e.Order.Customer, multiline: true);

			var totalSuffix =
				e.Order.BankAccount?.TotalSuffix.Clip() ??
				(e.Order.Vat.No() ? $", {DomainRes.Common_WithoutVat}" : null)
			;


			var data = new
			{

				Number = e.Number,
				IssueDate = e.IssueDate,
				OrderNo = e.Order.Number,

				Supplier = supplierDetails,
				ShipTo = shipTo,
				BillTo = billTo,

				SupplierDetails = supplierDetails,
				CustomerDetails = customerDetails,


				ItemCount = e.Order.Items.Count,


				Items = e.Order.Items
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

					totals.Add(e.Order.Discount, ReportRes.InvoicePrinter_Discount, skipIfEmpty: true);

					totals.Add(e.Order.Total, ReportRes.InvoicePrinter_InvoiceTotalWithVat);


					if (configuration.InvoicePrinter_ShowVat)
					{
						totals.Add(e.Order.Vat, ReportRes.InvoicePrinter_Vat);
					}


					if (e.ShowPaid && !Equals(e.Order.Total, e.Order.TotalDue))
					{

						totals.Add(e.Order.Paid, ReportRes.InvoicePrinter_Paid);

						totals.Add(e.Order.TotalDue, ReportRes.InvoicePrinter_TotalDue);


						if (configuration.InvoicePrinter_ShowVat)
						{
							totals.Add(e.Order.VatDue, ReportRes.InvoicePrinter_Vat);
						}

					}

				}),


				TotalWords = (e.ShowPaid ? e.Order.TotalDue : e.Order.Total).ToWords() + totalSuffix,

				Warning1 = configuration.VatRate > 0 ? ReportRes.InvoicePrinter_VatLawChanged : null,

				IssuedBy = e.IssuedBy.NameForDocuments,

				FooterDetails = configuration.InvoicePrinter_FooterDetails,

			};



			return Build(
				templateFileName,
				data,
				out fileExtension
			).ToBytes();

		}



		private byte[] GetStream2(
			StreamParams e,
			string templateFileName,
			out string fileExtension
		)
		{

			var pos = 1;

			var shipTo = (e.Order.ShipTo ?? e.Order.Customer).As(a => a.NameForDocuments);


			var billTo = e.Order.BillTo == null && e.Order.BillToName.Yes()
				? e.Order.BillToName
				: (e.Order.BillTo ?? e.Order.Customer).As(a => a.NameForDocuments);


			if (shipTo == billTo)
				billTo = ReportRes.InvoicePrinter_Same;


			var vatRate = db.Configuration.VatRate / 100;

			var orderHasVat = e.Order.Vat.Yes();



			var data = new
			{

				Number = e.Number,
				IssueDate = e.IssueDate,
				OrderNo = e.Order.Number,

				Supplier = db.Configuration.GetSupplierDetails(db, e.Order, owner: e.Owner, bankAccount: e.BankAccount),
				ShipTo = shipTo,
				BillTo = billTo,

				ItemCount = e.Order.Items.Count,


				Items = e.Order.Items
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

					totals.Add(e.Order.Discount, ReportRes.InvoicePrinter_Discount, skipIfEmpty: true);

					totals.Add(e.Order.Total, ReportRes.InvoicePrinter_InvoiceTotalWithVat);


					if (db.Configuration.InvoicePrinter_ShowVat)
					{
						totals.Add(e.Order.Vat, ReportRes.InvoicePrinter_Vat);
					}


					if (e.ShowPaid && !Equals(e.Order.Total, e.Order.TotalDue))
					{

						totals.Add(e.Order.Paid, ReportRes.InvoicePrinter_Paid);

						totals.Add(e.Order.TotalDue, ReportRes.InvoicePrinter_TotalDue);


						if (db.Configuration.InvoicePrinter_ShowVat)
						{
							totals.Add(e.Order.VatDue, ReportRes.InvoicePrinter_Vat);
						}

					}

				}),


				TotalWords = (e.ShowPaid ? e.Order.TotalDue : e.Order.Total).ToWords(),

				Warning1 = db.Configuration.VatRate > 0 ? ReportRes.InvoicePrinter_VatLawChanged : null,

				IssuedBy = e.IssuedBy.NameForDocuments,

				FooterDetails = db.Configuration.InvoicePrinter_FooterDetails,

			};



			return Build(
				templateFileName,
				data,
				out fileExtension
			).ToBytes();

		}



		//---g

	}






	//===g



}
