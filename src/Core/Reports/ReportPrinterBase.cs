using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{

	public abstract class ReportPrinterBase<TEntity>
		where TEntity : class
	{

		protected ReportPrinterBase()
		{
			BaseFont = PdfUtility.GetBaseFont(PdfUtility.ArialNarrow, false, false, true);
			BaseFontBold = PdfUtility.GetBaseFont(PdfUtility.ArialNarrow, true, false, true);

			Font = new Font(BaseFont, 11f);
			FontBold = new Font(BaseFontBold, 11f);
		}


		public void Build(Stream stream, IList<TEntity> entities)
		{
			Document = new Document(PageSize.A4, 20, 20, 20, 20);

			if (entities.Count <= 0) return;

			InitDocument();

			Writer = PdfWriter.GetInstance(Document, stream);
			Writer.SetFullCompression();
			Writer.PageEvent = new PdfPageEventAction(OnStartPage, OnEndPage);

			Document.Open();
			ContentByte = Writer.DirectContent;

			for (var i = 0; i < entities.Count; i++)
			{
				if (i > 0)
					Document.NewPage();

				AddPage(entities[i]);
			}

			Writer.PageEvent = null;
			OnEndPage();

			Document.Close();
		}

		protected virtual void InitDocument()
		{
		}

		protected virtual void OnStartPage()
		{
			AddPageHeader();
		}

		protected virtual void OnEndPage()
		{
			AddPageFooter();
		}

		protected virtual void AddPage(TEntity r)
		{
		}

		protected virtual void AddPageHeader()
		{
		}

		protected virtual void AddPageFooter()
		{
		}


		protected virtual void AddHeader()
		{
		}

		protected virtual void AddFooter()
		{
		}

		protected virtual void AddTitle(string text)
		{
			Document.Add(new Paragraph(text, new Font(BaseFontBold, 16))
			{
				Alignment = Element.ALIGN_CENTER,
				SpacingBefore = 25,
				SpacingAfter = 30
			});
		}

		protected static void AddCell(PdfPTable table, string text, Font font, int alignment)
		{
			if (text.No())
			{
				table.AddCell(string.Empty);
			}
			else
			{
				table.AddCell(new PdfPCell(table.DefaultCell)
				{
					Phrase = new Phrase(text, font) { Leading = font.Size + 50 },
					HorizontalAlignment = alignment
				});
			}
		}

		protected PdfPTable NewRowTable(params float[] columnRelativeWidth)
		{
			return new PdfPTable(columnRelativeWidth)
			{
				WidthPercentage = 100,
				DefaultCell = { Border = Rectangle.NO_BORDER, },
			};
		}


		protected PdfPTable NewDetailTable(params float[] columnRelativeWidth)
		{
			return new PdfPTable(columnRelativeWidth)
			{
				WidthPercentage = 100,
				DefaultCell =
				{
					PaddingLeft = 4,
					PaddingRight = 4,
					PaddingTop = 2,
					PaddingBottom = 6,
					Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER,
				},
			};
		}

		protected void Write(PdfPTable table, string title, string value)
		{
			table.AddCell(Font, title + ": ", cell => cell.Right());
			table.AddCell(FontBold, value, cell => cell.Left());
		}

		protected void WriteR(PdfPTable table, string title, string value)
		{
			table.AddCell(Font, title + ": ", cell => cell.Right());
			table.AddCell(FontBold, value, cell => cell.Right());
		}

		protected bool WriteIf(PdfPTable table, string title, string value)
		{
			if (value.No()) return false;
			Write(table, title, value);
			return true;
		}

		protected void Write(PdfPTable table, string title, DateTime value)
		{
			Write(table, title, value.ToString("d.MM.yyyy"));
		}

		protected void Write(PdfPTable table, string title, bool value)
		{
			Write(table, title, value ? "Yes" : "No");
		}

		protected bool WriteIf(PdfPTable table, string title, bool value)
		{
			if (!value) return false;
			Write(table, title, true);
			return true;
		}

		protected void Write(PdfPTable table, string title, Money value)
		{
			WriteR(table, title, value.As(a => a.MoneyString));
		}

		protected bool WriteIf(PdfPTable table, string title, Money value)
		{
			if (value == null) return false;
			Write(table, title, value);
			return true;
		}

		protected void Write(string text, Action<Paragraph> action = null)
		{
			Write(text, Font, action);
		}

		protected void Write(string text, Font font, Action<Paragraph> action = null)
		{
			Document.Add(new Paragraph(text, font).Do(action));
		}

		protected void WriteBR()
		{
			Document.Add(new Paragraph(" ", Font));
		}


		protected static string GetDepartureDateText(FlightSegment segment)
		{
			var builder = new StringBuilder();
			var separator = string.Empty;

			if (segment.DepartureTime.HasValue)
			{
				builder.Append(segment.DepartureTime.Value.ToString("dd.MM.yyyy"));
				separator = Environment.NewLine;
			}

			if (segment.FlightNumber.Yes())
			{
				builder.Append(separator);

				if (segment.Carrier != null && segment.Carrier.AirlineIataCode.Yes())
				{
					builder.Append(segment.Carrier.AirlineIataCode);
					builder.Append(" ");
				}

				builder.Append(segment.FlightNumber);
			}

			return builder.ToString();
		}

		protected static string GetDepartureTimeString(FlightSegment segment)
		{
			var builder = new StringBuilder();
			var separator = string.Empty;

			if (segment.DepartureTime.HasValue)
			{
				builder.Append(segment.DepartureTime.Value.ToString("HH:mm"));
				separator = " ";
			}

			if (segment.CheckInTerminal.Yes())
				builder.AppendFormat("{0}{1} {2}", separator, ReportRes.DefaultTicketPrinter_Gate, segment.CheckInTerminal);

			if (segment.CheckInTime.Yes())
			{
				separator = builder.Length > 0 ? Environment.NewLine : string.Empty;
				builder.AppendFormat("{0}{1}", separator, segment.CheckInTime);
			}

			return builder.ToString();
		}

		protected static string GetArrivalTimeString(FlightSegment segment)
		{
			var builder = new StringBuilder();
			var separator = string.Empty;

			if (segment.ArrivalTime.HasValue)
			{
				builder.Append(segment.ArrivalTime.Value.ToString("HH:mm"));
				separator = " ";
			}

			if (segment.ArrivalTerminal.Yes())
				builder.AppendFormat("{0}{1} {2}", separator, ReportRes.DefaultTicketPrinter_Gate, segment.ArrivalTerminal);

			return builder.ToString();
		}

		protected static string GetAirportString(Airport airport)
		{
			var builder = new StringBuilder();

			var settlement = string.Empty;
			var separator = string.Empty;

			if (airport.Settlement.Yes())
			{
				settlement += airport.Settlement;
				separator = ", ";
			}

			if (airport.Country != null)
				settlement += separator + airport.Country.Name;

			if (settlement.Yes() && airport.Name.Yes())
				settlement += ",";

			if (settlement.Yes())
				builder.AppendLine(settlement);

			if (airport.Name.Yes())
				builder.AppendLine(airport.Name);

			return builder.ToString();
		}

		protected static string GetLuggageSeatString(string luggage, string seat)
		{
			var builder = new StringBuilder();

			var separator = string.Empty;

			if (luggage.Yes())
			{
				builder.Append(luggage);
				separator = Environment.NewLine;
			}

			if (seat.Yes())
				builder
					.Append(separator)
					.Append(seat);

			return builder.ToString();
		}

		protected static string GetMoneyString(Money money)
		{
			if (money == null)
				return string.Empty;

			return money.Amount.ToMoneyString();
		}


		protected Document Document;
		protected PdfWriter Writer;
		protected PdfContentByte ContentByte;

		protected readonly BaseFont BaseFont;
		protected readonly BaseFont BaseFontBold;

		protected readonly Font Font;
		protected readonly Font FontBold;
	}

}
