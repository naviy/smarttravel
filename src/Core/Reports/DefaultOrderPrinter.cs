using System;
using System.Collections.Generic;

using iTextSharp.text;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public class DefaultOrderPrinter : ReportPrinterBase<Order>, IOrderPrinter
	{
		protected override void AddPage(Order order)
		{
			r = order;

			AddHeader();

			AddFooter();
		}


		protected override void AddHeader()
		{
			AddTitle(DomainRes.Order + " " + r.Number);

			var table = NewRowTable(20f, 50f, 15f, 15f);

			Write(table, DomainRes.Order_IssueDate, r.IssueDate);
			Write(table, DomainRes.Common_Discount, r.Discount);

			Write(table, DomainRes.Common_Customer, r.Customer.As(a => a.NameForDocuments));
			Write(table, DomainRes.Common_Total, r.Total);

			Write(table, DomainRes.Common_BillTo, r.BillToName ?? r.BillTo.As(a => a.NameForDocuments));
			Write(table, DomainRes.Order_Vat, r.Vat);

			Write(table, DomainRes.Common_ShipTo, r.ShipTo.As(a => a.NameForDocuments));
			Write(table, DomainRes.Order_Paid, r.Paid);

			Write(table, DomainRes.Common_AssignedTo, r.AssignedTo.As(a => a.NameForDocuments));
			Write(table, DomainRes.Order_TotalDue, r.TotalDue);

			Write(table, DomainRes.Common_Owner, r.Owner.As(a => a.NameForDocuments));
			table.AddCells(2);

			if (WriteIf(table, DomainRes.Order_IsPublic, r.IsPublic))
				table.AddCells(2);

			Write(table, DomainRes.Order_IsSubjectOfPaymentsControl, r.IsSubjectOfPaymentsControl);
			table.AddCells(2);

			if (WriteIf(table, DomainRes.Common_Note, r.Note))
				table.AddCells(2);

			Document.Add(table);

			if (r.Items.Yes())
			{
				WriteBR();
				Write(DomainRes.OrderItems, FontBold);
				WriteBR();
				table = NewDetailTable(3, 45, 11, 15, 15, 11);

				table.AddCellR(FontBold, CommonRes.Number_Short);
				table.AddCell(FontBold, DomainRes.OrderItem_Text);
				table.AddCellR(FontBold, DomainRes.OrderItem_Quantity);
				table.AddCellR(FontBold, DomainRes.OrderItem_Price);
				table.AddCellR(FontBold, DomainRes.Common_Amount);
				table.AddCell(FontBold, DomainRes.OrderItem_Consignment);

				foreach (var item in r.Items)
				{
					table.AddCell(Font, item.Position + 1);
					table.AddCell(Font, item.Text);
					table.AddCell(Font, item.Quantity);
					table.AddCell(Font, item.Price);
					table.AddCell(Font, item.Total);
					table.AddCell(Font, item.Consignment.As(a => a.Number));
				}

				Document.Add(table);
			}

			if (r.Payments.Yes())
			{
				WriteBR();
				Write(DomainRes.Payment_Caption_List, FontBold);
				WriteBR();
				table = NewDetailTable(3, 15, 15, 15, 15, 15, 15);

				table.AddCellR(FontBold, CommonRes.Number_Short);
				table.AddCell(FontBold, DomainRes.Payment);
				table.AddCell(FontBold, DomainRes.Payment_PostedOn);
				table.AddCell(FontBold, DomainRes.Payment_DocumentNumber);
				table.AddCell(FontBold, DomainRes.Payment_Payer);
				table.AddCell(FontBold, DomainRes.Payment_RegisteredBy);
				table.AddCellR(FontBold, DomainRes.Payment_Amount);

				var i = 1;

				foreach (var payment in r.Payments)
				{
					table.AddCell(Font, i++);
					table.AddCell(Font, payment.Number);
					table.AddCell(Font, payment.PostedOn);
					table.AddCell(Font, payment.DocumentNumber);
					table.AddCell(Font, payment.Payer.As(a => a.NameForDocuments));
					table.AddCell(Font, payment.RegisteredBy.As(a => a.NameForDocuments));
					table.AddCell(Font, payment.Sign * payment.Amount);
				}

				Document.Add(table);
			}

			WriteBR();
			WriteBR();
			Write(ReportRes.InvoicePrinter_SignedBy.AsFormat(r.AssignedTo.As(a => a.NameForDocuments)), p => p.Right());
		}



		protected override void AddFooter()
		{
			ContentByte.SetFontAndSize(BaseFont, 8);

			ContentByte.BeginText();
			ContentByte.ShowTextAligned(Element.ALIGN_CENTER, string.Format(ReportRes.DefaultTicketPrinter_LuxenaCopyright, DateTime.Today.Year), Document.Right / 2, Document.Bottom, 0);
			ContentByte.EndText();
		}


		protected IList<Order> Orders;
		private Order r;

	}

}