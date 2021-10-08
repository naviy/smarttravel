using System.IO;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Reports
{
	public class BlankPrinter
	{
		public void Build(Stream stream, AviaTicket ticket)
		{
			_ticket = ticket;

			_document = new Document(_pageRect, 0, 0, 0, 0);
			_writer = PdfWriter.GetInstance(_document, stream);

			_writer.SetFullCompression();

			_document.Open();

			_contentByte = _writer.DirectContent;

			GenerateContent();

			_document.Close();
		}

		private void GenerateContent()
		{
			_contentByte.SetFontAndSize(_baseFont, 8);

			_contentByte.BeginText();
			_contentByte.ShowTextAligned(Element.ALIGN_LEFT, _ticket.Endorsement, 30, 180, 0);
			_contentByte.ShowTextAligned(Element.ALIGN_CENTER, _ticket.IssueDate.ToString("ddMMMyy").ToUpper(), 275, 168, 0);
			_contentByte.ShowTextAligned(Element.ALIGN_CENTER, _ticket.PnrCode, 360, 170, 0);
			_contentByte.ShowTextAligned(Element.ALIGN_LEFT, _ticket.PassengerName, 30, 155, 0);

			var y = 130;
			FlightSegment last = null;

			foreach (var segment in _ticket.Segments)
			{
				if (segment.Type != FlightSegmentType.Ticketed)
					continue;
				
				_contentByte.ShowTextAligned(Element.ALIGN_LEFT, segment.FromAirportName + " " + segment.FromAirportCode, 40, y, 0);
				_contentByte.ShowTextAligned(Element.ALIGN_CENTER, segment.CarrierIataCode, 175, y, 0);
				_contentByte.ShowTextAligned(Element.ALIGN_CENTER, segment.FlightNumber, 208, y, 0);
				_contentByte.ShowTextAligned(Element.ALIGN_CENTER, segment.ServiceClassCode, 234, y, 0);

				if (segment.DepartureTime.HasValue)
				{
					_contentByte.ShowTextAligned(Element.ALIGN_CENTER, segment.DepartureTime.Value.ToString("ddMMMyy").ToUpper(), 262, y, 0);
					_contentByte.ShowTextAligned(Element.ALIGN_CENTER, segment.DepartureTime.Value.ToString("HH:mm"), 305, y, 0);
				}

				_contentByte.ShowTextAligned(Element.ALIGN_CENTER, "OK", 338, y, 0);
				_contentByte.ShowTextAligned(Element.ALIGN_LEFT, segment.FareBasis, 350, y, 0);
				_contentByte.ShowTextAligned(Element.ALIGN_CENTER, segment.Luggage, 550, y, 0);

				y -= 11;

				last = segment;
			}

			if (last != null)
				_contentByte.ShowTextAligned(Element.ALIGN_LEFT, last.ToAirportName + " " + last.ToAirportCode, 40, y, 0);


			_contentByte.ShowTextAligned(Element.ALIGN_RIGHT, GetMoneyString(_ticket.Fare), 81, 68, 0);
			_contentByte.ShowTextAligned(Element.ALIGN_RIGHT, GetMoneyString(_ticket.EqualFare), 81, 54, 0);

			if (_ticket.Fees.Count > 0)
			{
				_contentByte.ShowTextAligned(Element.ALIGN_RIGHT, GetMoneyString(_ticket.Fees[0].Amount), 81, 42, 0);
				_contentByte.ShowTextAligned(Element.ALIGN_LEFT, _ticket.Fees[0].Code, 84, 42, 0);

				if (_ticket.Fees.Count > 1)
				{
					_contentByte.ShowTextAligned(Element.ALIGN_RIGHT, GetMoneyString(_ticket.Fees[1].Amount), 81, 30, 0);
					_contentByte.ShowTextAligned(Element.ALIGN_LEFT, _ticket.Fees[1].Code, 84, 30, 0);

					if (_ticket.Fees.Count == 3)
					{
						_contentByte.ShowTextAligned(Element.ALIGN_RIGHT, GetMoneyString(_ticket.Fees[2].Amount), 81, 18, 0);
						_contentByte.ShowTextAligned(Element.ALIGN_LEFT, _ticket.Fees[2].Code, 84, 18, 0);
					}
					else if (_ticket.Fees.Count > 3)
					{
						var sb = new StringBuilder("XT: ");

						var xt = _ticket.Fees[2].Amount;
						sb.AppendFormat("{0} {1}", GetMoneyString(_ticket.Fees[2].Amount), _ticket.Fees[2].Code);

						for (var i = 3; i < _ticket.Fees.Count; ++i)
						{
							xt += _ticket.Fees[i].Amount;
							sb.AppendFormat(", {0} {1}", GetMoneyString(_ticket.Fees[i].Amount), _ticket.Fees[i].Code);
						}

						_contentByte.ShowTextAligned(Element.ALIGN_LEFT, sb.ToString(), 102, 54, 0);

						_contentByte.ShowTextAligned(Element.ALIGN_RIGHT, GetMoneyString(xt), 81, 18, 0);
						_contentByte.ShowTextAligned(Element.ALIGN_LEFT, "XT", 84, 18, 0);
					}
				}
			}

			_contentByte.ShowTextAligned(Element.ALIGN_RIGHT, GetMoneyString(_ticket.Total), 81, 6, 0);

			_contentByte.ShowTextAligned(Element.ALIGN_LEFT, _ticket.PaymentForm, 102, 18, 0);
			_contentByte.ShowTextAligned(Element.ALIGN_LEFT, _ticket.Name, 330, 6, 0);
			_contentByte.EndText();
		}

		private static string GetMoneyString(Money money)
		{
			return string.Format("{0} {1:F}", money.Currency, money.Amount);
		}

		private static readonly Rectangle _pageRect = new RectangleReadOnly(595f, 200f);

		private AviaTicket _ticket;
		private Document _document;
		private PdfWriter _writer;

		private readonly BaseFont _baseFont = PdfUtility.GetBaseFont(PdfUtility.Arial, true, false, true);
		private PdfContentByte _contentByte;
	}
}