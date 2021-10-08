using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;

using Luxena.Travel.Domain;
using Luxena.Travel.Reports;


namespace Luxena.Travel
{

	public class TicketPrinter : ITicketPrinter
	{
		public string LogoImagePath { get; set; }
		public string WebSite { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }

		public void Build(Stream stream, IList<AviaTicket> tickets)
		{
			if (tickets == null || tickets.Count == 0)
				return;

			_tickets = tickets;
			_currentTicket = _tickets[0];

			_document = new Document(PageSize.A4, 20, 20, 75, 40);

			_pdfWriter = PdfWriter.GetInstance(_document, stream);

			_pdfWriter.SetFullCompression();

			_pdfWriter.PageEvent = new PdfPageEventAction(OnNewPage);

			_newPageInsertionMode = NewPageInsertionMode.BeginOfDocument;

			_document.Open();

			foreach (var t in _tickets)
			{
				_currentTicket = t;

				_newPageInsertionMode = NewPageInsertionMode.Normal;

				if (_addPageForTicket)
					_document.NewPage();

				_addPageForTicket = true;

				GeneratePrintedTicket();
			}

			_newPageInsertionMode = NewPageInsertionMode.EndOfDocument;

			_document.Close();
		}

		private void OnNewPage()
		{
			switch (_newPageInsertionMode)
			{
				case NewPageInsertionMode.BeginOfDocument:

					_addPageForTicket = false;

					break;

				case NewPageInsertionMode.Normal:

					_addPageForTicket = true;

					break;

				case NewPageInsertionMode.EndOfCurrentTicket:

					_addPageForTicket = false;

					var index = _tickets.IndexOf(_currentTicket);
					_currentTicket = _tickets[++index];

					break;

				case NewPageInsertionMode.EndOfDocument:
					return;
			}

			AddPageHeader();
		}

		private void GeneratePrintedTicket()
		{
			AddItinerary();
			AddCalculation();
			AddPenalties();
			AddAgreement();
			AddNotices();
			AddOptions();
		}

		private void AddPageHeader()
		{
			AddFrame();

			var image = Image.GetInstance(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogoImagePath));

			image.ScaleToFit(_document.Right - _document.LeftMargin, image.Height);
			image.SetAbsolutePosition(_document.Left, _document.PageSize.Top - image.ScaledHeight - 20);

			_pdfWriter.DirectContent.AddImage(image);

			_document.Add(new Paragraph(Resources.SaleCertificate, new Font(_baseFont, 7)).Center());

			AddAgencyDataTable();

			AddTicketDataTable();
		}

		private void AddFrame()
		{
			const float delta = 4;

			var yPos = _document.Bottom - 15;

			var top = _document.PageSize.Top - 20 + delta;
			var left = _document.Left - delta;
			var right = _document.Right + delta;
			var bottom = yPos - delta;

			var contentByte = _pdfWriter.DirectContent;

			contentByte.SetLineWidth(1.5f);

			contentByte.MoveTo(left - 0.75f, top);

			contentByte.LineTo(right, top);
			contentByte.LineTo(right, bottom);
			contentByte.LineTo(left, bottom);
			contentByte.LineTo(left, top);

			contentByte.Stroke();

			contentByte.SetColorFill(_blueColor);
			contentByte.Rectangle(_document.Left, yPos, _document.Right - _document.Left, 12);
			contentByte.Fill();

			contentByte.SetColorFill(Color.BLACK);

			contentByte.BeginText();

			var ownerParty = _currentTicket.Owner;

			if (ownerParty != null)
			{
				contentByte.SetFontAndSize(_baseFont, 10);
				contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, ownerParty.Email1, 90, yPos + 3, 0);
				contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, ownerParty.Phone1, 184, yPos + 3, 0);
			}

			contentByte.SetFontAndSize(_baseFontBold, 10);
			contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, WebSite, 297, yPos + 3, 0);

			contentByte.SetFontAndSize(_baseFont, 10);
			contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Email, 418, yPos + 3, 0);
			contentByte.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Phone, 510, yPos + 3, 0);

			contentByte.EndText();
		}

		private void AddAgencyDataTable()
		{
			var ownerParty = _currentTicket.Owner;

			var font = new Font(_baseFont, 10);
			var fontBold = new Font(_baseFontBold, 10);

			var table = new PdfPTable(6)
			{
				WidthPercentage = 100,
				SpacingBefore = 5
			};
			table.SetTotalWidth(new[] { 14.5f, 43.4f, 9f, 12.2f, 7f, 13.9f });

			var phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.AgencyOffice_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.AgencyOffice_eng, font)
			});

			var cell = new PdfPCell(phrase)
			{
				PaddingLeft = 0,
				PaddingRight = 0,
				Border = Rectangle.NO_BORDER,
				VerticalAlignment = Element.ALIGN_MIDDLE
			};

			table.AddCell(cell);

			phrase = new Phrase();
			if (ownerParty != null)
				phrase.Add(new Chunk(ownerParty.Note, fontBold));

			cell = new PdfPCell(phrase)
			{
				BorderWidth = 1f,
				HorizontalAlignment = Element.ALIGN_LEFT,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			cell = new PdfPCell(new Phrase(Resources.IataNumber, fontBold))
			{
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				Border = Rectangle.NO_BORDER
			};
			table.AddCell(cell);

			var iataCode = _currentTicket.TicketingIataOffice ?? _currentTicket.TicketerOffice;

			phrase = new Phrase(new Chunk(iataCode, fontBold));

			cell = new PdfPCell(phrase)
			{
				BorderWidth = 1f,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.Date_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.Data_eng, font)
			});

			cell = new PdfPCell(phrase)
			{
				HorizontalAlignment = Element.ALIGN_RIGHT,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				Border = Rectangle.NO_BORDER
			};

			table.AddCell(cell);

			phrase = new Phrase(_currentTicket.IssueDate.ToString("dd.MM.yyyy"), fontBold);

			cell = new PdfPCell(phrase)
			{
				BorderWidth = 1f,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			_document.Add(table);
		}

		private void AddTicketDataTable()
		{
			_document.Add(CreateSectionHeader(Resources.ElectronicTicket_ua, Resources.ElectronicTicket_eng));

			var font = new Font(_baseFont, 10);
			var fontBold = new Font(_baseFontBold, 10);

			var table = new PdfPTable(4)
			{
				WidthPercentage = 100,
				SpacingBefore = 2
			};
			table.SetTotalWidth(new[] { 14.5f, 32, 28.5f, 25 });

			var phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.Passenger_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.Passenger_eng, font)
			});

			var cell = new PdfPCell(phrase)
			{
				PaddingLeft = 0,
				PaddingRight = 0,
				Border = Rectangle.NO_BORDER
			};

			table.AddCell(cell);

			cell = new PdfPCell(new Phrase(_currentTicket.PassengerName, fontBold))
			{
				HorizontalAlignment = Element.ALIGN_CENTER,
				BorderWidth = 1,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.Airline_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.Airline_eng, font)
			});

			cell = new PdfPCell(phrase)
			{
				HorizontalAlignment = Element.ALIGN_RIGHT,
				Border = Rectangle.NO_BORDER
			};

			table.AddCell(cell);

			cell = new PdfPCell(new Phrase(_currentTicket.Producer.Name, fontBold))
			{
				HorizontalAlignment = Element.ALIGN_CENTER,
				BorderWidth = 1,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.Number_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.Number_eng, font)
			});

			cell = new PdfPCell(phrase)
			{
				PaddingLeft = 0,
				Border = Rectangle.NO_BORDER
			};

			table.AddCell(cell);

			cell = new PdfPCell(new Phrase(_currentTicket.Number.Yes() ? $"{_currentTicket.AirlinePrefixCode} {_currentTicket.Number}" : string.Empty, fontBold))
			{
				HorizontalAlignment = Element.ALIGN_CENTER,
				BorderWidth = 1,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.BookingNumber_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.BookingNumber_eng, font)
			});

			cell = new PdfPCell(phrase)
			{
				HorizontalAlignment = Element.ALIGN_RIGHT,
				Border = Rectangle.NO_BORDER
			};

			table.AddCell(cell);

			cell = new PdfPCell(new Phrase(_currentTicket.PnrCode + _currentTicket.AirlinePnrCode.As(a => " / " + a), fontBold))
			{
				HorizontalAlignment = Element.ALIGN_CENTER,
				BorderWidth = 1,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.Passport_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.Passport_en, font)
			});

			cell = new PdfPCell(phrase)
			{
				PaddingLeft = 0,
				PaddingRight = 0,
				Border = Rectangle.NO_BORDER
			};

			table.AddCell(cell);

			var passportNumber = string.Empty;

			if (_currentTicket.GdsPassportStatus == GdsPassportStatus.Exist && _currentTicket.Passenger != null)
			{
				var passport = _currentTicket.ParseGdsPassport();

				if (passport != null && _currentTicket.Passenger.Passports.Any(t => t.Number == passport.Number))
					passportNumber = passport.Number;
			}

			cell = new PdfPCell(new Phrase(passportNumber, fontBold))
			{
				HorizontalAlignment = Element.ALIGN_CENTER,
				Border = Rectangle.NO_BORDER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.FQTV, font)
			});

			cell = new PdfPCell(phrase)
			{
				HorizontalAlignment = Element.ALIGN_RIGHT,
				Border = Rectangle.NO_BORDER
			};

			table.AddCell(cell);

			cell = new PdfPCell(new Phrase(string.Empty, font))
			{
				HorizontalAlignment = Element.ALIGN_CENTER,
				Border = Rectangle.NO_BORDER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			_document.Add(table);
		}

		private void AddItinerary()
		{
			//var segments = _currentTicket.GetTicketedSegments();
			var segments = _currentTicket.Segments.Where(a => a.Type != FlightSegmentType.Voided).ToList();

			_document.Add(CreateSectionHeader(Resources.Itinerary_ua, Resources.Itinerary_eng));

			var font = new Font(_baseFont, 9);
			var fontBold = new Font(_baseFontBold, 8);

			var table = new PdfPTable(8)
			{
				WidthPercentage = 100
			};
			table.SetTotalWidth(new[] { 9.3f, 18f, 18.3f, 18.3f, 10.8f, 7.7f, 8.5f, 9.1f });
			table.SpacingBefore = 2;
			table.DefaultCell.PaddingLeft = 0;
			table.DefaultCell.PaddingRight = 0;
			table.DefaultCell.PaddingTop = 2;
			table.DefaultCell.PaddingBottom = 2;

			table.DefaultCell.Border = Rectangle.BOTTOM_BORDER;

			AddCell(table, string.Concat(Resources.Segment_Date, Environment.NewLine, Resources.Segment_Flight, Environment.NewLine, Resources.ServiceClass), fontBold, Element.ALIGN_CENTER);
			AddCell(table, Resources.Segment_DeparturePlace, fontBold, Element.ALIGN_CENTER);
			AddCell(table, Resources.Segment_ArrivalPlace, fontBold, Element.ALIGN_CENTER);
			AddCell(table, string.Concat(Resources.Segment_DepartureTime, Environment.NewLine, Resources.Segment_CheckIn), fontBold, Element.ALIGN_CENTER);
			AddCell(table, Resources.Segment_ArrivelTime, fontBold, Element.ALIGN_CENTER);
			AddCell(table, Resources.Segment_FlightTime, fontBold, Element.ALIGN_CENTER);
			AddCell(table, Resources.Segment_Seat, fontBold, Element.ALIGN_CENTER);
			AddCell(table, Resources.Segment_Meal, fontBold, Element.ALIGN_CENTER);

			table.DefaultCell.Border = Rectangle.NO_BORDER;

			for (var pos = 0; pos < segments.Count; pos++)
			{
				var segment = segments[pos];

				if (pos == segments.Count - 1)
					table.DefaultCell.Border = Rectangle.BOTTOM_BORDER;

				AddCell(table, GetDepartureDateText(segment), font, Element.ALIGN_CENTER);
				AddCell(table, GetAirportString(segment.FromAirport), font, Element.ALIGN_LEFT);
				AddCell(table, GetAirportString(segment.ToAirport), font, Element.ALIGN_LEFT);

				table.DefaultCell.PaddingLeft = 10;

				AddCell(table, GetDepartureTimeString(segment), font, Element.ALIGN_LEFT);

				table.DefaultCell.PaddingLeft = 0;

				AddCell(table, GetArrivalTimeString(segment), font, Element.ALIGN_LEFT);
				AddCell(table, segment.Duration, font, Element.ALIGN_CENTER);
				AddCell(table, GetLuggageSeatString(segment.Luggage, segment.Seat), font, Element.ALIGN_CENTER);
				AddCell(table, segment.MealTypes.HasValue ? segment.MealTypes.Value.ToDisplayString() : string.Empty, font,
					Element.ALIGN_CENTER);
			}

			_document.Add(table);

			var grayFont = new Font(_baseFont, 8);
			grayFont.SetColor(_grayColor.R, _grayColor.G, _grayColor.B);

			var grayFontItalic = new Font(_baseFontItalic, 8);
			grayFontItalic.SetColor(_grayColor.R, _grayColor.G, _grayColor.B);

			table = new PdfPTable(6)
			{
				WidthPercentage = 100
			};
			table.SetTotalWidth(new[] { 14.5f, 48, 11.5f, 12, 7, 7 });
			table.SpacingBefore = 5;
			table.DefaultCell.PaddingTop = 0;

			table.DefaultCell.Border = Rectangle.NO_BORDER;
			AddCell(table, Resources.Endorsement, grayFontItalic, Element.ALIGN_CENTER);

			table.DefaultCell.Border = Rectangle.BOX;
			AddCell(table, _currentTicket.Endorsement, grayFontItalic, Element.ALIGN_CENTER);

			table.DefaultCell.Border = Rectangle.NO_BORDER;
			AddCell(table, Resources.TourCode, grayFontItalic, Element.ALIGN_CENTER);

			table.DefaultCell.Border = Rectangle.BOX;
			AddCell(table, _currentTicket.TourCode, grayFontItalic, Element.ALIGN_CENTER);

			table.DefaultCell.Border = Rectangle.NO_BORDER;
			AddCell(table, Resources.Payment, grayFontItalic, Element.ALIGN_CENTER);

			table.DefaultCell.Border = Rectangle.BOX;
			AddCell(table, _currentTicket.PaymentForm, grayFontItalic, Element.ALIGN_CENTER);

			_document.Add(table);
		}

		private void AddCalculation()
		{
			var font = new Font(_baseFont, 10);
			var fontBold = new Font(_baseFontBold, 10);

			var table = new PdfPTable(6)
			{
				WidthPercentage = 100
			};
			table.SetTotalWidth(new[] { 10f, 14f, 26f, 27f, 8f, 15f });
			table.SpacingBefore = 5;

			var phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.Fare_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.Fare_eng, font)
			});

			var cell = new PdfPCell(phrase)
			{
				PaddingLeft = 0,
				HorizontalAlignment = Element.ALIGN_RIGHT,
				Border = Rectangle.NO_BORDER
			};

			table.AddCell(cell);

			cell = new PdfPCell(new Phrase(_currentTicket.EqualFare?.MoneyString, fontBold))
			{
				BorderWidth = 1f,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.Taxes_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.Taxes_eng, font)
			});

			cell = new PdfPCell(phrase)
			{
				HorizontalAlignment = Element.ALIGN_RIGHT,
				Border = Rectangle.NO_BORDER,
				PaddingLeft = 0
			};
			table.AddCell(cell);

			var builder = new StringBuilder();
			var separator = string.Empty;

			var feesTotal = _currentTicket.FeesTotal + _currentTicket.Handling;
			var extraCharge = _currentTicket.ServiceFee - _currentTicket.Discount - _currentTicket.CommissionDiscount;

			if (feesTotal != null)
			{
				builder.Append(feesTotal.MoneyString);
				separator = " + ";
			}

			if (extraCharge != null)
			{
				builder.Append(separator).Append(extraCharge.MoneyString);
			}

			cell = new PdfPCell(new Phrase(builder.ToString(), fontBold))
			{
				BorderWidth = 1f,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.Total_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.Total_eng, font)
			});

			cell = new PdfPCell(phrase)
			{
				HorizontalAlignment = Element.ALIGN_RIGHT,
				Border = Rectangle.NO_BORDER
			};

			table.AddCell(cell);

			cell = new PdfPCell(new Phrase(_currentTicket.GrandTotal?.MoneyString, fontBold))
			{
				BorderWidth = 1f,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE,
				PaddingTop = 0.5f
			};

			table.AddCell(cell);

			AddSection(Resources.Calculation_ua, Resources.Calculation_eng, false, table);
		}

		private PdfPCell CreatePenaltyTableHeaderCell(Phrase phrase)
		{
			var cell = phrase == null ? new PdfPCell() : new PdfPCell(phrase);
			cell.HorizontalAlignment = Element.ALIGN_CENTER;
			cell.VerticalAlignment = Element.ALIGN_TOP;
			cell.Border = Rectangle.NO_BORDER;
			cell.SetLeading(0.0f, 0.2f);
			return cell;
		}

		private void AddPenalties()
		{
			var font = new Font(_baseFont, 9);
			var fontBold = new Font(_baseFontBold, 9);

			const string separator = " / ";

			var table = new PdfPTable(6)
			{
				WidthPercentage = 93,
				HorizontalAlignment = Element.ALIGN_LEFT
			};

			table.SetTotalWidth(new[] { 15.8f, 25.5f, 0.5f, 25.5f, 0.5f, 32.2f });

			table.AddCell(CreatePenaltyTableHeaderCell(null));

			var phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.DateChange_ua, fontBold),
				new Chunk(separator),
				new Chunk(Resources.DateChange_eng, font)
			});

			table.AddCell(CreatePenaltyTableHeaderCell(phrase));
			table.AddCell(CreatePenaltyTableHeaderCell(null));

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.Refund_ua, fontBold),
				new Chunk(separator),
				new Chunk(Resources.Refund_eng, font)
			});

			table.AddCell(CreatePenaltyTableHeaderCell(phrase));
			table.AddCell(CreatePenaltyTableHeaderCell(null));

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.NoShow_ua, fontBold),
				new Chunk(separator),
				new Chunk(Resources.NoShow_eng, font)
			});

			table.AddCell(CreatePenaltyTableHeaderCell(phrase));

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.BeforeDeparture_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.BeforeDeparture_eng, font)
			});

			table.AddCell(new PdfPCell(phrase) { HorizontalAlignment = Element.ALIGN_LEFT, Border = Rectangle.NO_BORDER, PaddingLeft = 0 });

			table.AddCell(new PdfPCell(GetPenaltyPhrase(PenalizeOperationType.ChangesBeforeDeparture, font))
			{
				BorderWidth = 1,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE
			});
			table.AddCell(new PdfPCell { Border = Rectangle.NO_BORDER });

			table.AddCell(new PdfPCell(GetPenaltyPhrase(PenalizeOperationType.RefundBeforeDeparture, font))
			{
				BorderWidth = 1,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE
			});
			table.AddCell(new PdfPCell { Border = Rectangle.NO_BORDER });

			table.AddCell(new PdfPCell(GetPenaltyPhrase(PenalizeOperationType.NoShowBeforeDeparture, font))
			{
				BorderWidth = 1,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE
			});

			phrase = new Phrase();
			phrase.AddRange(new object[]
			{
				new Chunk(Resources.AfterDepature_ua, fontBold),
				new Chunk(Environment.NewLine),
				new Chunk(Resources.AfterDepature_eng, font)
			});

			table.AddCell(new PdfPCell(phrase) { HorizontalAlignment = Element.ALIGN_LEFT, Border = Rectangle.NO_BORDER, PaddingLeft = 0 });

			table.AddCell(new PdfPCell(GetPenaltyPhrase(PenalizeOperationType.ChangesAfterDeparture, font))
			{
				BorderWidth = 1,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE
			});
			table.AddCell(new PdfPCell { Border = Rectangle.NO_BORDER });

			table.AddCell(new PdfPCell(GetPenaltyPhrase(PenalizeOperationType.RefundAfterDeparture, font))
			{
				BorderWidth = 1,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE
			});
			table.AddCell(new PdfPCell { Border = Rectangle.NO_BORDER });

			table.AddCell(new PdfPCell(GetPenaltyPhrase(PenalizeOperationType.NoShowAfterDeparture, font))
			{
				BorderWidth = 1,
				HorizontalAlignment = Element.ALIGN_CENTER,
				VerticalAlignment = Element.ALIGN_MIDDLE
			});

			var serviceFeeReturnPolicy = new Paragraph { Alignment = Element.ALIGN_CENTER };
			serviceFeeReturnPolicy.SetLeading(0.2f, 1.5f);

			serviceFeeReturnPolicy.AddRange(new object[]
			{
				new Chunk(Resources.ServiceFee_Msg_ua + separator, new Font(_baseFontBold, 8, Font.UNDERLINE, _darkBlueColor)),
				new Chunk(Resources.ServiceFee_Msg_eng, new Font(_baseFont, 8, Font.UNDERLINE, _orangeColor))
			});

			var fareReturnPolicy1 = new Paragraph { Alignment = Element.ALIGN_CENTER };
			fareReturnPolicy1.SetLeading(1, 1.5f);

			fareReturnPolicy1.AddRange(new object[]
			{
				new Chunk(Resources.FareReturnPolicy1_ua + separator + "\r\n", new Font(_baseFontBold, 6, Font.UNDERLINE, _darkBlueColor)),
				new Chunk(Resources.FareReturnPolicy1_en, new Font(_baseFont, 6, Font.UNDERLINE, _orangeColor))
			});

			var fareReturnPolicy2 = new Paragraph { Alignment = Element.ALIGN_CENTER };
			fareReturnPolicy2.SetLeading(1, 1.5f);

			fareReturnPolicy2.AddRange(new object[]
			{
				new Chunk(Resources.FareReturnPolicy2_ua + separator, new Font(_baseFontBold, 8, Font.UNDERLINE, _darkBlueColor)),
				new Chunk(Resources.FareReturnPolicy2_en, new Font(_baseFont, 8, Font.UNDERLINE, _orangeColor))
			});

			AddSection(Resources.Penalties_ua, Resources.Penalties_eng, false, table, serviceFeeReturnPolicy, fareReturnPolicy1, fareReturnPolicy2);
		}

		private Phrase GetPenaltyPhrase(PenalizeOperationType type, Font font)
		{
			if (_currentTicket.PenalizeOperations.IsNullOrEmpty())
				return new Phrase(Resources.EmptyPenalty, font);

			var operation = _currentTicket.PenalizeOperations.Find(op => op.Type == type);

			if (operation == null)
				return new Phrase();

			var text = string.Empty;

			switch (operation.Status)
			{
				case PenalizeOperationStatus.NotAllowed:

					if (type == PenalizeOperationType.ChangesBeforeDeparture || type == PenalizeOperationType.ChangesAfterDeparture)
						text = Resources.ChangesNotAllowed;
					else if (type == PenalizeOperationType.RefundBeforeDeparture || type == PenalizeOperationType.RefundAfterDeparture)
						text = Resources.RefundNotAllowed;
					else if (type == PenalizeOperationType.NoShowBeforeDeparture || type == PenalizeOperationType.NoShowAfterDeparture)
						text = Resources.NoShowNotAllowed;

					break;

				case PenalizeOperationStatus.NotChargeable:

					if (type == PenalizeOperationType.RefundBeforeDeparture || type == PenalizeOperationType.RefundAfterDeparture)
						text = Resources.RefundNoCharge;
					else
						text = Resources.NoCharge;

					break;

				case PenalizeOperationStatus.Chargeable:

					text = Resources.Charge;

					if (!string.IsNullOrEmpty(operation.Description))
						text = $"{text} {operation.Description}";

					break;
			}

			if (string.IsNullOrEmpty(text))
				text = Resources.EmptyPenalty;

			return new Phrase(text, font);

			//return new Phrase();
		}

		private void AddAgreement()
		{
			var font = new Font(_baseFont, 10);
			var fontBold = new Font(_baseFontBold, 10);

			var table = new PdfPTable(5)
			{
				SpacingBefore = 5,
				WidthPercentage = 100,
				SpacingAfter = 0
			};
			table.SetTotalWidth(new[] { 70, 10, 2f, 17f, 1 });

			table.DefaultCell.PaddingTop = 0;
			table.DefaultCell.BorderWidth = 1.5f;

			table.DefaultCell.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.RIGHT_BORDER;
			table.AddMergedCell(5, new Font(_baseFont, 7), Resources.PersonalDataConfirmation);

			table.DefaultCell.Border = Rectangle.LEFT_BORDER;
			AddCell(table, Resources.Familiarize_Msg1_ua, fontBold, Element.ALIGN_LEFT);

			table.DefaultCell.Border = Rectangle.NO_BORDER;
			table.AddCell(new PdfPCell(table.DefaultCell));
			table.AddCell(new PdfPCell(table.DefaultCell));
			table.AddCell(new PdfPCell(table.DefaultCell));

			table.DefaultCell.Border = Rectangle.RIGHT_BORDER;
			table.AddCell(new PdfPCell(table.DefaultCell));

			table.DefaultCell.Border = Rectangle.LEFT_BORDER;
			AddCell(table, Resources.Familiarize_Msg2_ua, fontBold, Element.ALIGN_LEFT);

			table.DefaultCell.Border = Rectangle.NO_BORDER;
			AddCell(table, Resources.Signature_ua, new Font(_baseFontBold, 10, Font.BOLD, _darkBlueColor), Element.ALIGN_LEFT);

			table.AddCell(new PdfPCell(table.DefaultCell));
			table.AddCell(new PdfPCell(table.DefaultCell));

			table.DefaultCell.Border = Rectangle.RIGHT_BORDER;
			table.AddCell(new PdfPCell(table.DefaultCell));

			table.DefaultCell.Border = Rectangle.LEFT_BORDER;
			AddCell(table, Resources.Familiarize_Msg1_eng, font, Element.ALIGN_LEFT);

			table.DefaultCell.Border = Rectangle.NO_BORDER;
			AddCell(table, Resources.Signature_eng, new Font(_baseFont, 10, Font.NORMAL, _orangeColor), Element.ALIGN_LEFT);

			table.AddCell(new PdfPCell(table.DefaultCell));

			table.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
			AddCell(table, string.Empty, new Font(_baseFont, 10, Font.UNDERLINE), Element.ALIGN_RIGHT);

			table.DefaultCell.Border = Rectangle.RIGHT_BORDER;
			table.AddCell(new PdfPCell(table.DefaultCell));

			table.DefaultCell.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;
			AddCell(table, Resources.Familiarize_Msg2_eng, font, Element.ALIGN_LEFT);

			table.DefaultCell.Border = Rectangle.BOTTOM_BORDER;
			table.AddCell(new PdfPCell(table.DefaultCell));
			table.AddCell(new PdfPCell(table.DefaultCell));
			table.AddCell(new PdfPCell(table.DefaultCell));

			table.DefaultCell.Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;
			table.AddCell(new PdfPCell(table.DefaultCell));

			font = new Font(_baseFont, 5);
			fontBold = new Font(_baseFontBold, 5);

			table.DefaultCell.Border = Rectangle.NO_BORDER;
			table.DefaultCell.PaddingTop = 2;
			table.AddMergedCell(5, fontBold, Resources.Ticket_Not_Valid_ua);
			table.DefaultCell.PaddingTop = 0;
			table.AddMergedCell(5, font, Resources.Ticket_Not_Valid_en);

			AddSection(Resources.Agreement_ua, Resources.Agreement_eng, false, table);
		}

		private void AddNotices()
		{
			var font = new Font(_baseFont, 5);
			var fontBold = new Font(_baseFontBold, 5);

			AddSection(Resources.Noties_ua, Resources.Noties_eng, false,
				new Paragraph(Resources.Noties_Msg_ua, fontBold) { SpacingBefore = 2 },
				new Paragraph(Resources.Noties_Msg_eng, font) { SpacingBefore = 2 });
		}

		private void AddOptions()
		{
			var table = new PdfPTable(2)
			{
				WidthPercentage = 90
			};

			table.DefaultCell.Border = Rectangle.NO_BORDER;
			table.DefaultCell.PaddingTop = 0;

			var orangeFont = new Font(_baseFontBold, 10, Font.BOLD, _orangeColor);
			var blueFont = new Font(_baseFontBold, 10, Font.BOLD, _darkBlueColor);

			var list = new List(false, 10)
			{
				ListSymbol = new Chunk("\u2022", orangeFont)
			};


			list.Add(new ListItem(5, Resources.HotelsReservation_ua, orangeFont));
			list.Add(new ListItem(10, Resources.CarRental_ua, orangeFont));
			list.Add(new ListItem(10, Resources.Insurance_ua, orangeFont));

			var cell = new PdfPCell(table.DefaultCell);
			cell.AddElement(list);
			table.AddCell(cell);

			list = new List(false, 10)
			{
				ListSymbol = new Chunk("\u2022", blueFont)
			};

			list.Add(new ListItem(5, Resources.HotelsReservation_eng, blueFont));
			list.Add(new ListItem(10, Resources.CarRental_eng, blueFont));
			list.Add(new ListItem(10, Resources.Insurance_eng, blueFont));

			cell = new PdfPCell(table.DefaultCell);
			cell.AddElement(list);
			table.AddCell(cell);

			AddSection(Resources.Options_ua, Resources.Options_eng, true, table);
		}

		private static void AddCell(PdfPTable table, string text, Font font, int alignment)
		{
			if (string.IsNullOrEmpty(text))
			{
				table.AddCell(string.Empty);
			}
			else
			{
				var cell = new PdfPCell(table.DefaultCell)
				{
					Phrase = new Phrase(text, font)
					{
						Leading = font.Size + 50
					},
					HorizontalAlignment = alignment
				};

				table.AddCell(cell);
			}
		}

		private static string GetDepartureDateText(FlightSegment segment)
		{
			var builder = new StringBuilder();

			if (segment.DepartureTime.HasValue)
				builder.AppendLine(segment.DepartureTime.Value.ToString("dd.MM.yyyy"));

			if (!string.IsNullOrEmpty(segment.FlightNumber))
			{
				if (!string.IsNullOrEmpty(segment.Carrier?.AirlineIataCode))
					builder.AppendFormat("{0} ", segment.Carrier.AirlineIataCode);

				builder.AppendLine(segment.FlightNumber);
			}

			if (segment.ServiceClass.HasValue && segment.ServiceClass.Value != ServiceClass.Unknown)
				builder.Append(segment.ServiceClass.Value.ToDisplayString());
			else
				builder.Append(segment.ServiceClassCode);

			return builder.ToString();
		}

		private static string GetDepartureTimeString(FlightSegment segment)
		{
			var builder = new StringBuilder();
			var separator = string.Empty;

			if (segment.DepartureTime.HasValue)
			{
				builder.Append(segment.DepartureTime.Value.ToString("HH:mm"));
				separator = " ";
			}

			if (!string.IsNullOrEmpty(segment.CheckInTerminal))
				builder.AppendFormat("{0}{1} {2}", separator, Resources.Segment_Terminal, segment.CheckInTerminal);

			if (!string.IsNullOrEmpty(segment.CheckInTime))
			{
				separator = builder.Length > 0 ? Environment.NewLine : string.Empty;
				builder.AppendFormat("{0}{1}", separator, segment.CheckInTime);
			}

			return builder.ToString();
		}

		private static string GetArrivalTimeString(FlightSegment segment)
		{
			var builder = new StringBuilder();
			var separator = string.Empty;

			if (segment.ArrivalTime.HasValue)
			{
				builder.Append(segment.ArrivalTime.Value.ToString("HH:mm"));
				separator = " ";
			}

			if (!string.IsNullOrEmpty(segment.ArrivalTerminal))
				builder.AppendFormat("{0}{1} {2}", separator, Resources.Segment_Terminal, segment.ArrivalTerminal);

			return builder.ToString();
		}

		private static string GetAirportString(Airport airport)
		{
			var builder = new StringBuilder();

			var settlement = string.Empty;
			var separator = string.Empty;

			if (!string.IsNullOrEmpty(airport.Settlement))
			{
				settlement += airport.Settlement;
				separator = ", ";
			}

			if (airport.Country != null)
				settlement += separator + airport.Country.Name;

			if (!string.IsNullOrEmpty(settlement) && !string.IsNullOrEmpty(airport.Name))
				settlement += ",";

			if (!string.IsNullOrEmpty(settlement))
				builder.AppendLine(settlement);

			if (!string.IsNullOrEmpty(airport.Name))
				builder.AppendLine(airport.Name);

			return builder.ToString();
		}

		private static string GetLuggageSeatString(string luggage, string seat)
		{
			var builder = new StringBuilder();

			var separator = string.Empty;

			if (!string.IsNullOrEmpty(luggage))
			{
				builder.Append(luggage);
				separator = Environment.NewLine;
			}

			if (!string.IsNullOrEmpty(seat))
				builder
					.Append(separator)
					.Append(seat);

			return builder.ToString();
		}

		private PdfPTable CreateSectionHeader(string nameUa, string nameEng)
		{
			var headerFont = new Font(_baseFontBold, 11);

			var headerFontOrange = new Font(_baseFontBold, 11);
			headerFontOrange.SetColor(_orangeColor.R, _orangeColor.G, _orangeColor.B);

			var headerFontBlue = new Font(_baseFontBold, 11);
			headerFontBlue.SetColor(_darkBlueColor.R, _darkBlueColor.G, _darkBlueColor.B);

			var phrase = new Phrase
			{
				new Chunk(nameUa, headerFontBlue),
				new Chunk(" / ", headerFont),
				new Chunk(nameEng, headerFontOrange)
			};

			var table = new PdfPTable(1)
			{
				WidthPercentage = 100,
				SplitRows = false,
				SpacingBefore = 2,
				KeepTogether = true
			};
			table.DefaultCell.BackgroundColor = _blueColor;
			table.DefaultCell.Border = Rectangle.NO_BORDER;
			table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
			table.DefaultCell.PaddingTop = 0;

			table.AddCell(phrase);

			table.DefaultCell.BackgroundColor = Color.WHITE;

			return table;
		}

		private void AddSection(string nameUa, string nameEng, bool isLastSection, params IElement[] items)
		{
			var table = CreateSectionHeader(nameUa, nameEng);

			var cell = new PdfPCell(table.DefaultCell)
			{
				FixedHeight = 5
			};

			table.AddCell(cell);

			cell = new PdfPCell(table.DefaultCell);

			foreach (var item in items)
				cell.AddElement(item);

			table.AddCell(cell);

			table.SetTotalWidth(new[] { _document.Right - _document.Left });
			table.CalculateHeightsFast();

			var position = _pdfWriter.GetVerticalPosition(true);

			if (position - _document.Bottom - table.TotalHeight - table.SpacingBefore >= 0 && isLastSection)
			{
				_newPageInsertionMode = Equals(_currentTicket, _tickets[_tickets.Count - 1]) ?
					NewPageInsertionMode.EndOfDocument : NewPageInsertionMode.EndOfCurrentTicket;
			}

			_document.Add(table);

			_newPageInsertionMode = NewPageInsertionMode.Normal;
		}

		private IList<AviaTicket> _tickets;
		private Document _document;
		private PdfWriter _pdfWriter;

		private readonly BaseFont _baseFont = PdfUtility.GetBaseFont(PdfUtility.Arial, false, false, true);
		private readonly BaseFont _baseFontItalic = PdfUtility.GetBaseFont(PdfUtility.Arial, false, true, true);
		private readonly BaseFont _baseFontBold = PdfUtility.GetBaseFont(PdfUtility.Arial, true, false, true);

		private readonly Color _blueColor = new Color(153, 204, 255);
		private readonly Color _darkBlueColor = new Color(0, 0, 128);
		private readonly Color _orangeColor = new Color(255, 102, 0);
		private readonly Color _grayColor = new Color(128, 128, 128);

		private AviaTicket _currentTicket;

		private NewPageInsertionMode _newPageInsertionMode;

		private bool _addPageForTicket;

		private enum NewPageInsertionMode
		{
			Normal,
			BeginOfDocument,
			EndOfCurrentTicket,
			EndOfDocument
		}
	}

}