using System;
using System.Collections.Generic;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{

	public class DefaultTicketPrinter : ReportPrinterBase<AviaTicket>, ITicketPrinter
	{

		public Domain.Domain db { get; set; }

		public bool DisplayAllSegments { get; set; }

		public DefaultTicketPrinter()
		{
			DisplayAllSegments = true;
		}


		protected override void AddPage(AviaTicket ticket)
		{
			AddHeader();

			AddTicketInfo(ticket);
			AddItinerary(DisplayAllSegments ? ticket.Segments : ticket.GetTicketedSegments());
			AddCalculation(ticket);

			AddFooter();
		}

		protected override void AddHeader()
		{
			var company = db.Configuration.Company;
			if (company == null) return;

			var sb = new StringWrapper();

			sb *= company.NameForDocuments?.ToUpper();
			sb *= company.LegalAddress;
			sb += company.Phone1.As(a => ReportRes.DefaultTicketPrinter_Phone.AsFormat(a));

			Document.Add(new Paragraph(14, sb, new Font(BaseFont, 11)).Right());
		}

		protected virtual void AddTicketInfo(AviaTicket ticket)
		{
			var paragraph = new Paragraph(ReportRes.DefaultTicketPrinter_ElectronicTicket, new Font(BaseFontBold, 16))
			{
				Alignment = Element.ALIGN_CENTER,
				SpacingBefore = 25,
				SpacingAfter = 30
			};

			Document.Add(paragraph);

			Document.Add(new Paragraph(25));

			var font = new Font(BaseFont, 12);
			var fontBold = new Font(BaseFontBold, 12);

			var table = new PdfPTable(2) { WidthPercentage = 100 };
			table.SetTotalWidth(new [] { 27.5f, 73.5f });
			table.DefaultCell.Border = Rectangle.NO_BORDER;
			table.DefaultCell.PaddingRight = 5;

			AddCell(table, ReportRes.DefaultTicketPrinter_Passenger, font, Element.ALIGN_RIGHT);
			AddCell(table, ticket.PassengerName, fontBold, Element.ALIGN_LEFT);

			AddCell(table, ReportRes.DefaultTicketPrinter_Carrier, font, Element.ALIGN_RIGHT);
			AddCell(table, ticket.Producer != null ? ticket.Producer.Name : ticket.AirlineName, fontBold, Element.ALIGN_LEFT);

			var text = ticket.Departure?.ToString("dd.MM.yyyy");

			AddCell(table, ReportRes.DefaultTicketPrinter_FlightDate, font, Element.ALIGN_RIGHT);
			AddCell(table, text, fontBold, Element.ALIGN_LEFT);

			AddCell(table, ReportRes.DefaultTicketPrinter_BookingCode, font, Element.ALIGN_RIGHT);
			AddCell(table, ticket.PnrCode, fontBold, Element.ALIGN_LEFT);

			AddCell(table, ReportRes.DefaultTicketPrinter_TicketNumber, font, Element.ALIGN_RIGHT);

			if (ticket.Number.Yes())
				AddCell(table, ticket.FullNumber, fontBold, Element.ALIGN_LEFT);

			Document.Add(table);
		}

		protected virtual void AddItinerary(IList<FlightSegment> segments)
		{
			var paragraph = new Paragraph(ReportRes.DefaultTicketPrinter_Itinerary, new Font(BaseFont, 12))
			{
				Alignment = Element.ALIGN_LEFT,
				SpacingBefore = 25,
				SpacingAfter = 15
			};

			Document.Add(paragraph);

			var font = new Font(BaseFont, 10);
			var fontBold = new Font(BaseFontBold, 10);

			var table = new PdfPTable(8) { WidthPercentage = 100 };
			table.SetTotalWidth(new [] { 8.5f, 18.5f, 18.5f, 18.3f, 10.6f, 7.7f, 6.5f, 11.4f });
			table.DefaultCell.PaddingLeft = 0;
			table.DefaultCell.PaddingRight = 0;
			table.DefaultCell.PaddingTop = 2;
			table.DefaultCell.PaddingBottom = 2;

			table.DefaultCell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;

			AddCell(table, $"{ReportRes.DefaultTicketPrinter_Date}{Environment.NewLine}{ReportRes.DefaultTicketPrinter_Flight}", fontBold, Element.ALIGN_CENTER);
			AddCell(table, ReportRes.DefaultTicketPrinter_DeparturePlace, fontBold, Element.ALIGN_CENTER);
			AddCell(table, ReportRes.DefaultTicketPrinter_ArrivalPlace, fontBold, Element.ALIGN_CENTER);
			AddCell(table, $"{ReportRes.DefaultTicketPrinter_DepartureTime}{Environment.NewLine}{ReportRes.DefaultTicketPrinter_CheckIn}", fontBold, Element.ALIGN_CENTER);
			AddCell(table, ReportRes.DefaultTicketPrinter_ArrivelTime, fontBold, Element.ALIGN_CENTER);
			AddCell(table, ReportRes.DefaultTicketPrinter_FlightTime, fontBold, Element.ALIGN_CENTER);
			AddCell(table, ReportRes.DefaultTicketPrinter_Seat, fontBold, Element.ALIGN_CENTER);
			AddCell(table, ReportRes.DefaultTicketPrinter_Meal, fontBold, Element.ALIGN_CENTER);

			table.DefaultCell.Border = Rectangle.NO_BORDER;

			foreach (var segment in segments)
			{
				var position = segments.IndexOf(segment);

				if (position == segments.Count - 1)
					table.DefaultCell.Border = Rectangle.BOTTOM_BORDER;

				table.DefaultCell.BackgroundColor = position % 2 == 0 ? new Color(0xF5, 0xF5, 0xF5) : Color.WHITE;

				AddCell(table, GetDepartureDateText(segment), font, Element.ALIGN_LEFT);
				AddCell(table, GetAirportString(segment.FromAirport), font, Element.ALIGN_LEFT);
				AddCell(table, GetAirportString(segment.ToAirport), font, Element.ALIGN_LEFT);

				table.DefaultCell.PaddingLeft = 20;

				AddCell(table, GetDepartureTimeString(segment), font, Element.ALIGN_LEFT);

				table.DefaultCell.PaddingLeft = 0;

				AddCell(table, GetArrivalTimeString(segment), font, Element.ALIGN_LEFT);
				AddCell(table, segment.Duration, font, Element.ALIGN_CENTER);
				AddCell(table, GetLuggageSeatString(segment.Luggage, segment.Seat), font, Element.ALIGN_CENTER);
				AddCell(table, segment.MealTypes.HasValue? segment.MealTypes.Value.ToDisplayString() : string.Empty, font, Element.ALIGN_CENTER);
			}

			Document.Add(table);
		}

		protected virtual void AddCalculation(AviaTicket ticket)
		{
			var font = new Font(BaseFont, 12);
			var fontBold = new Font(BaseFontBold, 14);

			var table = new PdfPTable(3)
			{
				HorizontalAlignment = Element.ALIGN_LEFT,
				SpacingBefore = 15,
				WidthPercentage = 90
			};
			table.SetTotalWidth(new[] { 55f, 30, 15 });
			table.DefaultCell.PaddingLeft = 0;
			table.DefaultCell.PaddingRight = 5;
			table.DefaultCell.PaddingBottom = 5;

			table.DefaultCell.Border = Rectangle.NO_BORDER;

			Money fare = null;

			var defaultCurrency = db.Configuration.DefaultCurrency.Code;

			if (ticket.Fare != null && ticket.Fare.Currency.Code == defaultCurrency)
				fare = ticket.Fare;
			else if (ticket.EqualFare != null && ticket.EqualFare.Currency.Code == defaultCurrency)
				fare = ticket.EqualFare;

			AddCell(table, ReportRes.DefaultTicketPrinter_Calculation, font, Element.ALIGN_LEFT);
			AddCell(table, ReportRes.DefaultTicketPrinter_Fare, font, Element.ALIGN_RIGHT);
			AddCell(table, GetMoneyString(fare), font, Element.ALIGN_RIGHT);

			AddCell(table, null, font, Element.ALIGN_LEFT);
			AddCell(table, ReportRes.DefaultTicketPrinter_Taxes, font, Element.ALIGN_RIGHT);
			AddCell(table, GetMoneyString(ticket.FeesTotal), font, Element.ALIGN_RIGHT);

			AddCell(table, null, font, Element.ALIGN_LEFT);
			AddCell(table, ReportRes.DefaultTicketPrinter_TicketPrice, font, Element.ALIGN_RIGHT);
			AddCell(table, GetMoneyString(ticket.Total), font, Element.ALIGN_RIGHT);

			var servFee = ticket.Discount != null ? ticket.ServiceFee - ticket.Discount : ticket.ServiceFee;

			AddCell(table, null, font, Element.ALIGN_LEFT);
			AddCell(table, ReportRes.DefaultTicketPrinter_ServiceFee, font, Element.ALIGN_RIGHT);
			AddCell(table, GetMoneyString(servFee), font, Element.ALIGN_RIGHT);

			AddCell(table, null, font, Element.ALIGN_LEFT);
			AddCell(table, ReportRes.DefaultTicketPrinter_Total, fontBold, Element.ALIGN_RIGHT);
			AddCell(table, GetMoneyString(ticket.GrandTotal), fontBold, Element.ALIGN_RIGHT);

			if (ticket.Vat != null && ticket.Vat.Amount > 0)
			{
				AddCell(table, null, font, Element.ALIGN_LEFT);
				AddCell(table, ReportRes.DefaultTicketPrinter_Vat, font, Element.ALIGN_RIGHT);
				AddCell(table, GetMoneyString(ticket.Vat), font, Element.ALIGN_RIGHT);
			}

			Document.Add(table);
		}

		protected override void AddFooter()
		{
			ContentByte.SetFontAndSize(BaseFont, 8);

			ContentByte.BeginText();
			ContentByte.ShowTextAligned(Element.ALIGN_CENTER, string.Format(ReportRes.DefaultTicketPrinter_LuxenaCopyright, DateTime.Today.Year), Document.Right/2, Document.Bottom, 0);
			ContentByte.EndText();
		}


		protected IList<AviaTicket> Tickets;

	}

}