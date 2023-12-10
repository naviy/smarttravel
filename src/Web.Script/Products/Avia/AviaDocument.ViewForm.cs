using System;
using System.Collections;
using System.Html;
using System.Serialization;

using Ext;
using Ext.util;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Luxena.Travel.Services;

using Action = Ext.Action;
using Element = System.Html.Element;
using MenuItemConfig = Ext.menu.ItemConfig;


namespace Luxena.Travel
{


	public delegate string StringDelegate();


	public sealed class AviaDocumentViewForm : BasicViewForm
	{
		public AviaDocumentViewForm(string tabId, object id, string type)
			: base(tabId, id, type, "aviaDocumentView")
		{
		}

		protected override void LoadInstance()
		{
			AviaService.GetAviaDocument(_id, _type, false, Load, delegate { Tabs.Close(this); });
		}

		public static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string)id, delegate (string tabId) { return new AviaDocumentViewForm(tabId, id, type); });
		}


		protected override object[] CreateToolbarItems()
		{
			return new object[]
			{
				_handleButton = Action(Res.AviaDocument_Handle_Action.ToLowerCase(), TryProcess),

				_printButton = Action(Res.AviaDocument_Print_Action.ToLowerCase(), Print, true),

				_blankButton = Action(Res.AviaDocument_Blank_Action, Blank, true),

				_voidButton = Action(Res.Void_Action.ToLowerCase(), VoidDocument),

				_addRefundButton = Action(Res.AviaDocument_AddRefund_Action.ToLowerCase(), AddRefund),

				new ToolbarSeparator(),

				_createOrderButton = MenuAction(Res.AviaDocument_AddToOrder.ToLowerCase(),
					new object[]
					{
						MenuItem(Res.AviaDocument_AddToOrderNew.ToLowerCase(), delegate { CreateNewOrder(); }),
						MenuItem(Res.AviaDocument_AddToOrderExist.ToLowerCase(), delegate { TryAddToExistingOrder(); })
					}
				),

				new ToolbarSeparator(),

				_copyButton = Action(BaseRes.Copy.ToLowerCase(), CopyEntity),

				_editButton = Action(BaseRes.Edit.ToLowerCase(), EditEntity),

				_deleteButton = Action(BaseRes.Remove.ToLowerCase(), RemoveEntity)
			};
		}

		protected override void Load(object response)
		{
			_document = (AviaDocumentDto)response;

			Render();
		}


		private void PrepareButtons()
		{
			if (_type == "AviaTicket")
			{
				_ticket = (AviaTicketDto)_document;

				_printButton.setDisabled(false);
				_blankButton.setDisabled(false);

				if (_ticket.Refund != null)
					_addRefundButton.hide();
			}
			else if (_type == "AviaMco")
			{
				_mco = (AviaMcoDto)_document;

				if (_mco.Refund != null)
					_addRefundButton.hide();
			}
			else
			{
				_refund = (AviaRefundDto)_document;

				_addRefundButton.hide();
				_printButton.hide();
				_blankButton.hide();
			}

			if (!Script.IsNullOrUndefined(_document.CanDelete) && (!_document.CanDelete.Visible || _document.CanDelete.IsDisabled))
				_deleteButton.hide();

			if (!Script.IsNullOrUndefined(_document.CanUpdate) && (!_document.CanUpdate.Visible || _document.CanUpdate.IsDisabled))
			{
				_handleButton.setDisabled(true);
				_voidButton.setDisabled(true);
				//_addRefundButton.setDisabled(true);
				_editButton.setDisabled(true);
				//_copyButton.setDisabled(true);
				_createOrderButton.setDisabled(true);
			}
			else
				_createOrderButton.setDisabled(_document.IsVoid);
		}


		private void Render()
		{
			SetTabCaption();

			PrepareButtons();

			_titleLabel.setText(GetTitle());

			_titleLabel.removeClass("processed");
			_titleLabel.removeClass("unprocessed");
			_titleLabel.removeClass("voided");

			_voidButton.setText(Res.Void_Action.ToLowerCase());

			if (_document.RequiresProcessing)
				_titleLabel.addClass("unprocessed");
			else if (_document.IsVoid)
			{
				_titleLabel.addClass("voided");

				_voidButton.setText(Res.Restore_Action.ToLowerCase());
			}
			else
				_titleLabel.addClass("processed");

			_contentPanel.body.dom.InnerHTML = GetFullHtml();
		}

		private void SetTabCaption()
		{
			string str = _document.Name;

			string prefix = "T";

			if (_refund != null)
				prefix = "R";
			else if (_mco != null)
				prefix = "M";
			else if (_ticket != null && _ticket.ReissueFor != null)
				prefix = "RI";

			setTitle(string.Format("{0} {1}", prefix, str));
		}

		protected override string GetTitle()
		{
			string caption = DomainRes.AviaTicket;

			if (_refund != null)
				caption = DomainRes.AviaRefund;
			else if (_mco != null)
				caption = DomainRes.AviaMco;

			string status = _document.RequiresProcessing ? Res.AviaDocument_Unprocessed : (_document.IsVoid ? Res.AviaDocument_Voided : Res.AviaDocument_Processed);

			caption = string.Format("{0} ({1})", caption, status.ToLowerCase());

			if (Script.IsNullOrUndefined(_document.Number))
				caption = string.Format("{0} - {1}", caption, Res.AviaDocument_NotIssued);

			return caption;
		}

		private string GetFullHtml()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(GetCommonDataHtml());
			sb.Append(GetFinanceDataHtml());

			if (_ticket != null)
			{
				if (_ticket.Segments != null && _ticket.Segments.Length > 0)
					sb.Append(
						string.Format(@"<div class='segments'><h2>{0}</h2><div style='padding-top: 5px'>{1}</div></div>",
							DomainRes.AviaTicket_FlightSegment,
							RenderSegments()));

				if (_ticket.PenalizeOperations != null && _ticket.PenalizeOperations.Length > 0)
					sb.Append(
						string.Format(@"<div class='penalizeOperations'><h2>{0}</h2><div style='padding-top: 5px'>{1}</div></div>",
							DomainRes.AviaTicket_PenalizeOperations,
							RenderPenalizeOperations()));
			}

			sb.Append(HasValue(_document.Note, delegate
			{
				return
					string.Format(@"<div class='note'><h2>{0}</h2><pre>{1}</pre></div>",
						DomainRes.Common_Note, _document.Note);
			}));
			return sb.ToString();
		}

		private string GetCommonDataHtml()
		{
			AviaDocumentSemantic v = new SemanticDomain(this).AviaDocument;
			AviaDocumentDto r = _document;


			string commonDataHtml =
				@"<div class='commonData'><table><col style='width: 135px' />" +

				v.IssueDate.ToHtmlTr2(r, true) +

				@"<tr><td class='fieldLabel'>" + DomainRes.Common_Number + " / " + DomainRes.Airline + @":</td>" +
				@"<td class='fieldValue'>" + GetNumber() + " / " + HasValue(_document.Producer, delegate { return Link(_document.Producer); }, _document.AirlinePrefixCode) + @"</td></tr>" +

				HasValue(new object[] { r.PnrCode, r.AirlinePnrCode, r.TourCode }, delegate
				{
					return @"
					<tr><td class='fieldLabel'>" + v.PnrCode._title + " / " + v.TourCode._title + @":</td>
					<td class='fieldValue'>" + NotEmpty(r.PnrCode) +
						HasValue(r.AirlinePnrCode, delegate { return string.Format(" ({0})", r.AirlinePnrCode); }) +
						" / " + NotEmpty(r.TourCode) + "</td></tr>";
				}) +

				v.ReissueFor.ToHtmlTr2(r) +
				v.ReissuedBy.ToHtmlTr2(r) +
				//
				//				v.InConnectionWith.ToHtmlTr2(r) +

				v.Refund.ToHtmlTr2(r) +
				v.RefundedProduct.ToHtmlTr2(r) +
				//
				//				v.Description.ToHtmlTr2(r) +

				GetPassengerHtml() +

				GetGdsPassportStatus() +

				v.Provider.ToHtmlTr2(r) +

				"<tr name='Customer'><td class='fieldLabel " + (_document.Customer == null ? "error" : string.Empty) + @"'>" + DomainRes.Common_Customer + " / " + DomainRes.Common_Intermediary + @":</td>" +
				HasValue(new object[] { _document.Customer, _document.Intermediary }, delegate
				{
					return @"
					<td class='fieldValue'>" + Link(_document.Customer) + " / " + Link(_document.Intermediary) + @"</td></tr>";
				}, "<td/></tr>") +

				ProductViewForm.GetOriginHtml(r) +

				ProductViewForm.GetBookerAndTicketerHtml(v, r) +

				v.TicketingIataOffice.ToHtmlTr2(r, true) +

				v.Seller.ToHtmlTr2(r, true) +
				v.Owner.ToHtmlTr2(r, true) +
				v.LegalEntity.ToHtmlTr2(r) +
				v.Order.ToHtmlTr2(r, true) +

				HasValue(_document.Voidings, delegate
				{
					return @"
					<tr><td class='fieldLabel'>" + Res.AviaDocument_Voidings + @":</td>
					<td class='fieldValue'>" + Voidings(_document.Voidings) + @"</td></tr>";
				}) +

				@"</table></div>";

			return commonDataHtml;
		}

		private string GetNumber()
		{
			if (Script.IsNullOrUndefined(_document.Number))
				return string.Empty;

			return string.Format("{0}-{1}{2}", _document.AirlinePrefixCode, AviaDocumentDto.NumberToString(_document.Number),
				!string.IsNullOrEmpty(_document.Conjunction) ? "-" + _document.Conjunction : string.Empty);
		}



		private string GetPassengerHtml()
		{
			string passenger = Script.IsValue(_document.Passenger) ?
				string.Format("{0} ({1})", Link(_document.Passenger), _document.PassengerName) :
				_document.PassengerName;

			if (_ticket != null && _ticket.Passenger != null)
			{
				string warn = null;
				string cssStyle = "passengerPassport-warn";


				switch (_ticket.PassportValidationResult)
				{
					case PassportValidationResult.NoPassport:
						string text;

						if (string.IsNullOrEmpty(_ticket.GdsPassport))
							text = string.Format(Res.Passenger_NoPassport_Warning, _document.Originator);
						else
							text = string.Format(Res.Passenger_NoGdsPassport_Warning, (GdsOriginator)_document.Originator == GdsOriginator.Unknown ? "GDS" : EnumUtility.ToString(typeof(GdsOriginator), (GdsOriginator)_document.Originator));

						warn = text;

						break;

					case PassportValidationResult.ExpirationDateNotValid:
						warn = Res.Passenger_PassportExpired_Warning;

						break;

					case PassportValidationResult.NotValid:

						string originator = (GdsOriginator)_document.Originator == GdsOriginator.Unknown ? "GDS" : EnumUtility.ToString(typeof(GdsOriginator), (GdsOriginator)_document.Originator);

						warn = string.Format(Res.Passenger_PassportDataDoesntMatch_Warning, originator);

						cssStyle = "passengerPassport-error";

						break;
				}

				if (warn != null)
					passenger += "<div class='" + cssStyle + "'>" + warn + "</div>";
			}

			return "<tr><td class='fieldLabel'>" + DomainRes.AviaDocument_Passenger + ":</td><td class='fieldValue'>" + passenger + @"</td></tr>";
		}

		private string GetGdsPassportStatus()
		{
			if (_ticket == null)
				return string.Empty;

			string statusText = "<div>" + EnumUtility.Localize(typeof(GdsPassportStatus), _ticket.GdsPassportStatus, typeof(DomainRes)) + "</div>";

			if (!string.IsNullOrEmpty(_document.GdsPassport))
				statusText += "<div>" + _ticket.GdsPassport + "</div>";

			if (_ticket.GdsPassportStatus != GdsPassportStatus.Exist && _ticket.PassportRequired)
				statusText += "<div class='gdsPassportStatus-error'>" + Res.AviaTicket_PassengerPassportRequired + "</div>";

			string gdsPassport = @"
					<tr>
						<td class='fieldLabel'>" + DomainRes.AviaDocument_GdsPassportStatus + @":</td>
						<td class='fieldValue' style='width: 390px;'>" + statusText + @"</td>
					</tr>";

			return gdsPassport;
		}

		private string GetFinanceDataHtml()
		{
			AviaDocumentSemantic v = new SemanticDomain(this).AviaDocument;
			AviaDocumentDto r = _document;

			return
				@"<div class='financeData'><table>" +
				v.Fare.ToHtmlTr4(r, true) +
				v.EqualFare.ToHtmlTr4(r, false, true) +
				v.FeesTotal.ToHtmlTr4(r, true, false, false,
					Script.IsNullOrUndefined(r.Fees) ? null : typeof(AviaDocumentViewForm).FullName + @".showFees()"
				) +
				Fees() +

				v.ConsolidatorCommission.ToHtmlTr4(r, false) +

				v.CancelFee.ToHtmlTr4(r, false)+

				v.Total.ToHtmlTr4(r, true, false, true) +

				v.Vat.ToHtmlTr4(r, false) +

				v.Commission.ToHtmlTr4(r, false, false, false,
					HasValue(r.CommissionPercent, delegate { return "<td class='fieldValue gray'>(" + r.CommissionPercent.Format("N2") + "%)</td>"; })
				) +

				v.ServiceFee.ToHtmlTr4(r, true, false, true) +

				v.CommissionDiscount.ToHtmlTr4(r) +

				v.Handling.ToHtmlTr4(r) +
				v.HandlingN.ToHtmlTr4(r) +

				v.Discount.ToHtmlTr4(r) +

				v.BonusDiscount.ToHtmlTr4(r) +

				v.BonusAccumulation.ToHtmlTr4(r) +

				v.RefundServiceFee.ToHtmlTr4(r, Script.IsValue(_refund)) +
				v.ServiceFeePenalty.ToHtmlTr4(r, Script.IsValue(_refund)) +

				v.CancelCommission.ToHtmlTr4(r) +
			//HasValue(_refund, delegate
			//{
			//	return
			//		v.RefundServiceFee.ToHtmlTr4(r, true) +
			//		v.ServiceFeePenalty.ToHtmlTr4(r, true);
			//}) +


			v.PaymentType.ToHtmlTr4(r) +
				(string.IsNullOrEmpty(r.PaymentDetails) ? "" : @"<tr><td class='fieldValue gray rightAlign' colspan='2'>" + r.PaymentDetails + @"</td><td/><td/></tr>") +

				v.GrandTotal.ToHtmlTr4(r, true) +

				@"</table></div>";
		}

		private string RenderSegments()
		{
			if (_ticket.Segments == null || _ticket.Segments.Length == 0)
				return string.Empty;

			const string newLine = "<br/>";

			string segments = @"
				<table>
					<tr class='alternate'>
						<th>" + DomainRes.FlightSegment_FromAirportName + @"</th>
						<th>" + DomainRes.FlightSegment_ToAirportName + @"</th>
						<th>" + DomainRes.FlightSegment_Carrier + @"</th>
						<th style='width: 40px'>" + DomainRes.FlightSegment_FlightNumber + @"</th>
						<th style='width: 60px'>" + DomainRes.FlightSegment_ServiceClass + newLine + DomainRes.FlightSegment_Seat + @"</th>
						<th style='width: 130px'>" + Res.FlightSegment_Departure + "," + newLine + Res.FlightSegment_Registration + @"</th>
						<th style='width: 120px'>" + Res.FlightSegment_Arrival + @"</th>
						<th style='width: 60px'>" + Res.FlightSegment_Duration + newLine + Res.FlightSegment_Stops + @"</th>
						<th style='width: 60px'>" + DomainRes.FlightSegment_FareBasis + @"</th>
						<th style='width: 80px'>" + DomainRes.FlightSegment_Luggage + newLine + DomainRes.FlightSegment_Meal + @"</th>
						<th style='width: 20px'></th>
						<th style='width: 60px'>Coupon Amount</th>
					<tr>";

			for (int i = 0; i < _ticket.Segments.Length; i++)
			{
				FlightSegmentDto seg = _ticket.Segments[i];
				string cssClass = (i % 2) == 0 ? string.Empty : "class='alternate'";

				segments += @"
					<tr valign='top' " + cssClass + @">
						<td>" + NotEmpty(seg.FromAirport != null ? seg.FromAirport.Name : null) + newLine + Link(seg.FromAirport, seg.FromAirportName) + @"</td>
						<td>" + NotEmpty(seg.ToAirport != null ? seg.ToAirport.Name : null) + newLine + Link(seg.ToAirport, seg.ToAirportName) + @"</td>
						<td>" + NotEmpty(seg.CarrierCode) + newLine + Link(seg.Carrier) + @"</td>
						<td align='center'>" + NotEmpty(seg.FlightNumber) + @"</td>
						<td align='center'>" + HasValue(seg.ServiceClassName, delegate { return seg.ServiceClassName; }, seg.ServiceClassCode) + newLine + NotEmpty(seg.Seat) + @"</td>
						<td align='center'>" + HasValue(seg.DepartureTime, delegate { return Format.date(seg.DepartureTime, "d.m.y H:i"); }) + newLine +
					HasValue(seg.CheckInTerminal, delegate { return Res.Terminal_Text + " " + seg.CheckInTerminal + (!string.IsNullOrEmpty(seg.CheckInTime) ? ", " : string.Empty); }) +
						NotEmpty(seg.CheckInTime) +
							@"</td>
						<td align='center'>" + HasValue(seg.ArrivalTime, delegate { return Format.date(seg.ArrivalTime, "d.m.y H:i"); }) + newLine +
								HasValue(seg.ArrivalTerminal, delegate { return Res.Terminal_Text + " " + seg.ArrivalTerminal; }) +
									@"</td>
						<td align='center'>" + NotEmpty(seg.Duration) + newLine + NotEmpty(seg.NumberOfStops) + @"</td>
						<td align='center'>" + NotEmpty(seg.FareBasis) + @"</td>
						<td align='center'>" + NotEmpty(seg.Luggage) + newLine + NotEmpty(seg.MealString) + @"</td>
						<td align='center'><div class='" + (seg.Type == FlightSegmentType.Ticketed ? "ticketed-segment" : "unticketed-segment") + @"'/></td>
						<td align='right'>" + NotEmpty(MoneyDto.ToMoneyFullString(seg.CouponAmount)) + @"</td>
					</tr>";
			}

			segments += "</table>";

			return segments;
		}

		private string RenderPenalizeOperations()
		{
			if (_ticket.PenalizeOperations == null || _ticket.PenalizeOperations.Length == 0)
				return string.Empty;

			StringBuilder builder = new StringBuilder();

			builder.Append(@"
				<table>
					<tr class='header'>
						<td/>
						<th>" + Res.AviaTicket_ChangesPenalty + @"</th>
						<th>" + Res.AviaTicket_RefundPenalty + @"</th>
						<th>" + Res.AviaTicket_NoShowPenalty + @"</th>
					</tr>
			");

			builder.Append(@"
				<tr>
					<td class='caption'>" + Res.AviaTicket_BeforeDeparture + @"</td>
					<td>" + TryGetPenalizeOperationString(PenalizeOperationType.ChangesBeforeDeparture) + @"</td>
					<td>" + TryGetPenalizeOperationString(PenalizeOperationType.RefundBeforeDeparture) + @"</td>
					<td>" + TryGetPenalizeOperationString(PenalizeOperationType.NoShowBeforeDeparture) + @"</td>
				</tr>");

			if (_ticket.Segments != null && _ticket.Segments.Length > 1)
				builder.Append(@"
					<tr>
						<td class='caption'>" + Res.AviaTicket_AfterDeparture + @"</td>
						<td>" + TryGetPenalizeOperationString(PenalizeOperationType.ChangesAfterDeparture) + @"</td>
						<td>" + TryGetPenalizeOperationString(PenalizeOperationType.RefundAfterDeparture) + @"</td>
						<td>" + TryGetPenalizeOperationString(PenalizeOperationType.NoShowAfterDeparture) + @"</td>
					</tr>
				");

			builder.Append("</table>");

			return builder.ToString();
		}

		private string TryGetPenalizeOperationString(PenalizeOperationType operationType)
		{
			PenalizeOperationDto operation = null;

			foreach (PenalizeOperationDto dto in _ticket.PenalizeOperations)
				if (dto.Type == operationType)
				{
					operation = dto;
					break;
				}

			if (operation == null)
				return string.Empty;

			string res = EnumUtility.Localize(typeof(PenalizeOperationStatus), operation.Status, typeof(DomainRes));

			if (operation.Status == PenalizeOperationStatus.Chargeable && !string.IsNullOrEmpty(operation.Description))
				res = string.Format("{0} {1}", res, operation.Description);

			return res;
		}

		public static void ShowFees()
		{
			DivElement button = (DivElement)Document.GetElementById("showFeesButton");

			if (button.ClassName.IndexOf("feesClosed") >= 0)
				button.ClassName = button.ClassName.Replace("feesClosed", "feesOpened");
			else if (button.ClassName.IndexOf("feesOpened") >= 0)
				button.ClassName = button.ClassName.Replace("feesOpened", "feesClosed");

			ElementCollection collection = Document.GetElementsByTagName("tr");

			for (int i = 0; i < collection.Length; i++)
			{
				Element element = collection[i];
				if (collection[i].ClassName == "feeRow")
					element.Style.Display = element.Style.Display != "none" ? "none" : "";
			}
		}

		private string Fees()
		{
			if (_document.Fees == null || _document.Fees.Length == 0)
				return string.Empty;

			StringBuilder fees = new StringBuilder();

			for (int i = 0; i < _document.Fees.Length; i++)
			{
				AviaDocumentFeeDto fee = _document.Fees[i];

				fees.Append(string.Format(@"
					<tr name='{0}' id='{1}' style='display: none' class='feeRow'><td/>
						<td class='fieldValue gray rightAlign'>{2}</td>
						<td class='fieldValue gray'>{3}</td>
						<td class='fieldValue gray'>{4}</td>
					</tr>",
					FeeRowName,
					FeeRowName + i,
					fee.Amount != null ? fee.Amount.Amount.Format("N2") : "0.00",
					fee.Amount != null ? fee.Amount.Currency.Name : "",
					fee.Code
				));
			}

			return fees.ToString();
		}

		private static string Voidings(string[] voidings)
		{
			if (voidings == null || voidings.Length == 0)
				return string.Empty;

			string html = string.Empty;

			foreach (string voiding in voidings)
				html += string.Format("<div>{0}</div>", voiding);

			return html;
		}

		private void TryProcess()
		{
			AviaService.CanProcess(_id, _type,
				delegate (object result)
				{
					ProcessOperationPermissionsResponse response = (ProcessOperationPermissionsResponse)result;
					OperationStatus canProcess = new OperationStatus();
					if ((response.CustomActionPermissions).ContainsKey("CanProcess"))
						canProcess = (OperationStatus)((response.CustomActionPermissions)["CanProcess"]);

					if (!response.CanUpdate.IsEnabled)
					{
						MessageBoxWrap.Show(new Dictionary(
							"title", BaseRes.Error,
							"msg", response.CanUpdate.DisableInfo ?? BaseRes.AutoGrid_ActionNotPermitted_Msg,
							"icon", MessageBox.ERROR,
							"buttons", MessageBox.OK
							));

						return;
					}

					if (!canProcess.IsEnabled)
					{
						MessageBoxWrap.Show(new Dictionary(
							"title", BaseRes.Warning,
							"msg", canProcess.DisableInfo,
							"icon", MessageBox.WARNING,
							"buttons", MessageBox.YESNO,
							"fn", new MessageBoxResponseDelegate(
								delegate (string button, string text)
								{
									if (button == "yes")
										EditEntity();
								})
							));
					}
					else
					{
						AviaDocumentProcessForm form = new AviaDocumentProcessForm(response.Args);
						form.Saved +=
							delegate (object res)
							{
								AviaDocumentProcessDto processDto = ((AviaDocumentProcessDto[])res)[0];

								_document.Customer = processDto.Customer;
								_document.Intermediary = processDto.Intermediary;
								_document.ServiceFee = processDto.ServiceFee;
								_document.Handling = processDto.Handling;
								_document.CommissionDiscount = processDto.CommissionDiscount;
								_document.Discount = processDto.Discount;
								_document.GrandTotal = processDto.GrandTotal;
								_document.PaymentType = processDto.PaymentType;
								_document.RequiresProcessing = processDto.RequiresProcessing;
								_document.Note = processDto.Note;
								_document.Order = processDto.Order;

								if (_ticket != null)
								{
									AviaTicketProcessDto dto = (AviaTicketProcessDto)processDto;

									_ticket.PenalizeOperations = dto.PenalizeOperations;

									_ticket.Passenger = dto.Passenger;
									_ticket.GdsPassport = dto.GdsPassport;
									_ticket.GdsPassportStatus = dto.GdsPassportStatus;
								}

								if (_refund != null)
								{
									AviaRefundProcessDto dto = (AviaRefundProcessDto)processDto;

									_refund.RefundServiceFee = dto.RefundServiceFee;
									_refund.ServiceFeePenalty = dto.ServiceFeePenalty;
								}

								Render();
							};

						form.Open(_id, _type);
					}
				}, null);
		}


		private void CreateNewOrder()
		{
			Dictionary values = new Dictionary("AviaDocuments", new object[] { _document.Id });
			values["SeparateServiceFee"] = true;

			FormsRegistry.EditObject(ClassNames.Order, null, values, delegate (object arg)
			{
				LoadInstance();
				ItemResponse response = (ItemResponse)arg;
				FormsRegistry.ViewObject(ClassNames.Order, ((OrderDto)response.Item).Id);
			}, null);
		}


		private void TryAddToExistingOrder()
		{
			Dictionary documents = new Dictionary();
			documents[_document.Id] = _document.Name;

			ProductDto.TryAddToExistingOrder(
				documents,
				true,
				_document.Name,
				LoadInstance
			);
		}




		private void AddRefund()
		{
			Dictionary dictionary = new Dictionary();

			dictionary["IssueDate"] = Date.Now;

			AddValue(dictionary, _document, "AirlinePrefixCode");
			AddValue(dictionary, _document, "Number");
			AddValue(dictionary, _document, "Producer");
			AddValue(dictionary, _document, "Provider");

			Reference obj = Reference.Create(_type, AviaDocumentDto.NumberToString(_document.Number), _document.Id);

			dictionary["RefundedDocument"] = obj;

			AddValue(dictionary, _document, "PassengerName");
			AddValue(dictionary, _document, "Customer");
			AddValue(dictionary, _document, "Intermediary");

			AddValue(dictionary, _document, "Originator");

			AddValue(dictionary, _document, "Order");


			dictionary["Seller"] = AppManager.CurrentPerson;

			AddValue(dictionary, _document, "Fare");
			AddValue(dictionary, _document, "EqualFare");
			AddValue(dictionary, _document, "FeesTotal");
			AddValue(dictionary, _document, "Total");
			AddValue(dictionary, _document, "Vat");
			AddValue(dictionary, _document, "Commission");
			AddValue(dictionary, _document, "ServiceFee");
			AddValue(dictionary, _document, "Discount");
			AddValue(dictionary, _document, "GrandTotal");

			AddValue(dictionary, _document, "Discount");

			AddValue(dictionary, _document, "TaxRateOfProduct");
			AddValue(dictionary, _document, "TaxRateOfServiceFee");


			FormsRegistry.EditObject("AviaRefund", null, dictionary,
				delegate (object arg)
				{
					if (arg == null)
						return;

					AviaRefundDto refund = (AviaRefundDto)((ItemResponse)arg).Item;

					Reference info = Reference.Create("AviaRefund", refund.Number.ToString(), refund.Id);

					if (_ticket != null)
						_ticket.Refund = info;
					else if (_mco != null)
						_mco.Refund = info;

					Load(_document);

					FormsRegistry.ViewObject("AviaRefund", refund.Id);
				}, null);
		}

		private void VoidDocument()
		{
			MessageBoxWrap.Confirm(BaseRes.Confirmation, _document.IsVoid ? Res.Document_Restore_Confirmation : Res.Document_Void_Confirmation,
				delegate (string button, string text)
				{
					if (button != "yes")
						return;

					AviaService.ChangeVoidStatus(new object[] { _document.Id }, null,
						delegate (object result)
						{
							AviaDocumentDto dto = (AviaDocumentDto)((object[])result)[0];

							_document.IsVoid = dto.IsVoid;
							_document.RequiresProcessing = dto.RequiresProcessing;
							_document.Voidings = dto.Voidings;
							_document.Order = dto.Order;

							_createOrderButton.setDisabled(_document.IsVoid);

							Render();
						},
						null);
				});
		}

		private void Print()
		{

			if (AppManager.SystemConfiguration.Ticket_NoPrintReservations)
			{
				PrintTicket();
				return;
			}


			AviaService.GetReservationDocumentCount(_document.Id,
				delegate (object result)
				{
					if ((int)result == 1)
					{
						PrintTicket();
						return;
					}

					MessageBoxWrap.Show(new Dictionary(
						"msg", string.Format(Res.PrintReservation_Msg, result),
						"title", Res.PrintReservation_Msg_Title,
						"buttons", MessageBox.YESNOCANCEL,
						"icon", MessageBox.QUESTION,
						"cls", "print-ticket-message-box",
						"fn", new MessageBoxResponseDelegate(
							delegate (string button, string text)
							{
								if (button == "yes")
									PrintReservation();
								else if (button == "no")
									PrintTicket();
							})
						));
				}, null);
		}

		private void Blank()
		{
			ReportLoader.Load(string.Format("print/blank/Blank_{0}-{1}.pdf", _ticket.AirlinePrefixCode, _ticket.Number), new Dictionary("ticketId", _id));
		}

		private void PrintTicket()
		{
			ReportLoader.Load(string.Format("print/ticket/Ticket_{0}-{1}.pdf", _ticket.AirlinePrefixCode, _ticket.Number), new Dictionary("tickets", Json.Stringify(new object[] { _id })));
		}

		private void PrintReservation()
		{
			ReportLoader.Load(string.Format("print/reservation/Tickets_{0}.pdf", Date.Now.Format("Y-m-d_H-i")), new Dictionary("ticket", _id));
		}


		private const string FeeRowName = "aviaDocumentFee";


		private AviaDocumentDto _document;

		private AviaTicketDto _ticket;
		private AviaMcoDto _mco;
		private AviaRefundDto _refund;

		private Action _handleButton;
		private Action _printButton;
		private Action _blankButton;

		private Action _voidButton;
		private Action _addRefundButton;
		private Action _createOrderButton;
		private Action _editButton;
		private Action _copyButton;
		private Action _deleteButton;
	}
}