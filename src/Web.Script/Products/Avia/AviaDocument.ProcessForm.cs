using System;
using System.Collections;

using Ext;
using Ext.data;
using Ext.form;
using Ext.menu;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.Controls;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using Action = Ext.Action;
using ActionConfig = Ext.ActionConfig;
using ComboBox = Ext.form.ComboBox;
using Element = Ext.Element;
using Field = Ext.form.Field;
using ItemConfig = Ext.menu.ItemConfig;
using HtmlWindow = System.Html.Window;
using Record = Ext.data.Record;




namespace Luxena.Travel
{
	public class AviaDocumentProcessForm : BaseEditForm
	{
		public AviaDocumentProcessForm(AviaDocumentProcessArgs args)
		{
			_args = args;

			Window.addClass(" process-document");

			Window.keys = new object[]
			{
				new Dictionary("key", 13, "fn", new Action<object, EventObject>(OnKeyEnterPress), "scope", this),
			};

			Form.labelWidth = 160;

			CreateFormItems();

			if (_args.AllowSaveAndContinue)
			{
				_saveAndContinueButton = Form.addButton(Res.SaveAndContinue, new AnonymousDelegate(delegate
				{
					_saveAndContinue = true;

					if ((bool)Type.InvokeMethod(_menu, "isVisible"))
						_invokeSave = true;
					else
						Save();
				}));
			}

			_saveButton = Form.addButton(BaseRes.Save, new AnonymousDelegate(delegate
			{
				_saveAndContinue = false;

				if ((bool)Type.InvokeMethod(_menu, "isVisible"))
					_invokeSave = true;
				else
					Save();
			}));

			_cancelButton = Form.addButton(BaseRes.Cancel, new AnonymousDelegate(Cancel));

			Fields = new Field[]
			{
				_passengerField.Widget,
				_gdsPassportStatus,
				_orderSelector.Widget,
				_customerSelector.Widget,
				_intermediarySelector.Widget,
				_equalFareField,
				_commissionField,
				_serviceFeeField,
				_handlingField,
				_commissionDiscountField,
				_discountField,
				_refundServiceFeeField,
				_serviceFeePenaltyField,
				_grandTotalField,
				_paymentType,
				_descriptionField,
				_noteField,
				_applyDataCheckbox,
			};

			ArrayList components = new ArrayList();
			components.AddRange(new object[]
			{
				_passengerField.Widget,
				_gdsPassportStatus,
				_orderSelector.Widget,
				_customerSelector.Widget,
				_intermediarySelector.Widget,
				_equalFareField,
				_commissionField,
				_serviceFeeField,
				_handlingField,
				_commissionDiscountField,
				_discountField,
				_refundServiceFeeField,
				_serviceFeePenaltyField,
				_grandTotalField,
				_paymentType,
				_editPenaltiesButton,
				_applyDataCheckbox,
			});

			if (_args.AllowSaveAndContinue)
				components.Add(_saveAndContinueButton);

			components.Add(_saveButton);
			components.Add(_cancelButton);

			ComponentSequence = (Component[])components;
		}

		public bool SaveAndContinue
		{
			get { return _saveAndContinue; }
			set { _saveAndContinue = value; }
		}

		private bool ShowEqualFare
		{
			get { return _document.EqualFare == null && AppManager.SystemConfiguration.UseDefaultCurrencyForInput; }
		}

		private bool UseHandling
		{
			get { return AppManager.SystemConfiguration.UseAviaHandling && _type != ClassNames.AviaMco; }
		}

		public void Open(object ticketId, string type)
		{
			_type = type;

			AviaService.GetAviaReservationForProcess(ticketId, _type, TicketsLoaded, null);
		}

		public void CloseWindow()
		{
			Window.close();
		}

		private void CreateFormItems()
		{
			_passengerNameField = new DisplayField(new DisplayFieldConfig()
				.fieldLabel(DomainRes.AviaDocument_Passenger)
				.cls("passenger-name")
				.ToDictionary());

			_passengerField = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Person)
				.allowEdit(_args.AllowEditParty)
				.customActionsDelegate(GetPassengerSelectorActions)
				.width(200)
				.selectOnFocus(true)
				.listeners(new Dictionary(
					"changeValue", new FieldChangeDelegate(
						delegate (Field objthis, object newValue, object oldValue)
						{
							if (oldValue == null || newValue == null)
								_customerSelector.UpdateTriggerStatus();
						})))
				.hideTrigger(false)
				.selectOnFocus(true));

			ApplyActions(_passengerField.Widget, GetPassengerActions);

			_passportWarnField = new DisplayField(new DisplayFieldConfig()
				.cls("passport-error-label")
				.ToDictionary());

			_copyPassportButton = new LinkButton(new ButtonConfig()
				.text(Res.AviaDocumentProcessForm_CopyPassportGdsString)
				.handler(new AnonymousDelegate(CopyPassportData)));

			Panel passportPanel = new Panel(new PanelConfig()
				.items(new object[] { _copyPassportButton })
				.cls("copy-passport-button")
				.ToDictionary());

			_gdsPassportStatus = new ComboBox(new ComboBoxConfig()
				.fieldLabel(DomainRes.AviaDocument_GdsPassportStatus)
				.store(new ArrayStore(new ArrayStoreConfig()
					.fields(new string[] { "Id", "Name" })
					.data(new object[]
					{
						new object[] { GdsPassportStatus.Unknown, EnumUtility.Localize(typeof (GdsPassportStatus), GdsPassportStatus.Unknown, typeof (DomainRes)) },
						new object[] { GdsPassportStatus.Exist, EnumUtility.Localize(typeof (GdsPassportStatus), GdsPassportStatus.Exist, typeof (DomainRes)) },
						new object[] { GdsPassportStatus.NotExist, EnumUtility.Localize(typeof (GdsPassportStatus), GdsPassportStatus.NotExist, typeof (DomainRes)) },
						new object[] { GdsPassportStatus.Incorrect, EnumUtility.Localize(typeof (GdsPassportStatus), GdsPassportStatus.Incorrect, typeof (DomainRes)) }
					})
					.ToDictionary())
				)
				.listeners(new Dictionary("select", new AnonymousDelegate(
					delegate
					{
						UpdateGdsPassportStatusWarn(((GdsPassportStatus)_gdsPassportStatus.getValue()));

						RefreshWindowShadow();
					})))
				.mode("local")
				.editable(false)
				.displayField("Name")
				.valueField("Id")
				.triggerAction("all")
				.selectOnFocus(true)
				.width(100)
				.ToDictionary());

			_gdsPassportStatusWarnField = new DisplayField(new DisplayFieldConfig()
				.cls("passport-error-label")
				.value(Res.AviaTicket_PassengerPassportRequired)
				.ToDictionary());


			string orderLabel = AppManager.SystemConfiguration.IsOrderRequiredForProcessedDocument
				? "<b>" + DomainRes.Order + "</b>" : DomainRes.Order;

			_orderSelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Order)
				.valueProperties(new string[] { "Customer" })
				.fieldLabel(orderLabel));

			_orderSelector.Widget.on("changeValue", new FieldChangeDelegate(OnOrderChanged));

			_customerSelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass(ClassNames.Party)
				.allowEdit(_args.AllowEditParty)
				.allowCreate(false)
				.setDataProxy(PartyService.SuggestCustomersProxy())
				.customActionsDelegate(GetCustomerSelectorActions)
				.fieldLabel("<b>" + DomainRes.Common_Customer + "</b>")
				.width(200)
				.selectOnFocus(true)
				.forceSelection(true));


			_customerSelector.Widget.on("changeValue", new FieldChangeDelegate(OnCustomerChanged));

			_intermediarySelector = new ObjectSelector((ObjectSelectorConfig)new ObjectSelectorConfig()
				.setClass("Party")
				.allowEdit(_args.AllowEditParty)
				.customActionsDelegate(GetIntermediarySelectorActions)
				.fieldLabel(DomainRes.Common_Intermediary)
				.width(200)
				.selectOnFocus(true)
				.forceSelection(true));

			_totalBox = new BoxComponent(new BoxComponentConfig()
				.cls("x-form-item total")
				.ToDictionary());

			_equalFareField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Common_EqualFare));
			if (AppManager.SystemConfiguration.UseDefaultCurrencyForInput)
				_equalFareField.labelStyle = "font-weight: bold;";
			_fareLabel = new Label(new LabelConfig().cls("fare").ToDictionary());

			_equalFarePanel = new Panel(new PanelConfig()
				.items(new object[] { _equalFareField, _fareLabel })
				.layout("form")
				.itemCls("float-left")
				.ToDictionary());

			_commissionField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Common_Commission));

			_serviceFeeField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Common_ServiceFee));
			_serviceFeeField.labelStyle = "font-weight: bold;";
			_serviceFeeField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));

			_handlingField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Common_Handling));
			_handlingField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));

			_commissionDiscountField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Common_CommissionDiscount));
			_commissionDiscountField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));

			_discountField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Common_Discount));
			_discountField.labelStyle = "font-weight: bold;";
			_discountField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));

			_refundServiceFeeField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Common_RefundServiceFee));
			_refundServiceFeeField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));

			_serviceFeePenaltyField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Common_ServiceFeePenalty));
			_serviceFeePenaltyField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));

			_grandTotalField = new MoneyControl(MoneyControlConfig.DefaultConfig(DomainRes.Product_GrandTotal));
			_grandTotalField.labelStyle = "font-weight: bold;";
			_grandTotalField.DecimalField.on("change", new FieldChangeDelegate(OnAmountChange));

			_paymentType = new ComboBox(new ComboBoxConfig()
				.fieldLabel(DomainRes.PaymentType)
				.store(new ArrayStore(new ArrayStoreConfig()
					.fields(new string[] { "Id", "Name" })
					.data(new object[]
					{
						new object[] { PaymentType.Unknown, EnumUtility.ToString(typeof (PaymentType), PaymentType.Unknown) },
						new object[] { PaymentType.Cash, EnumUtility.ToString(typeof (PaymentType), PaymentType.Cash) },
						new object[] { PaymentType.Invoice, EnumUtility.ToString(typeof (PaymentType), PaymentType.Invoice) },
						new object[] { PaymentType.Check, EnumUtility.ToString(typeof (PaymentType), PaymentType.Check) },
						new object[] { PaymentType.CreditCard, EnumUtility.ToString(typeof (PaymentType), PaymentType.CreditCard) },
						new object[] { PaymentType.Exchange, EnumUtility.ToString(typeof (PaymentType), PaymentType.Exchange) },
						new object[] { PaymentType.WithoutPayment, DomainRes.PaymentType_WithoutPayment },
					})
					.ToDictionary())
				)
				.mode("local")
				.editable(false)
				.displayField("Name")
				.valueField("Id")
				.triggerAction("all")
				.selectOnFocus(true)
				.ToDictionary());

			_descriptionField = new TextArea();
			_descriptionField.fieldLabel = DomainRes.Common_Description;
			_descriptionField.height = 48;
			_descriptionField.width = 260;

			_noteField = new TextArea();
			_noteField.fieldLabel = DomainRes.Common_Note;
			_noteField.height = 48;
			_noteField.width = 260;

			_editPenaltiesButton = new LinkButton(new ButtonConfig()
				.handler(new AnonymousDelegate(EditPenalizeOperations))
				.cls("edit-penalties")
				.text(Res.AviaDocumentProcessForm_EditPenalizeOperations));

			_reservationCount = new BoxComponent();

			_documentList = new Panel(new PanelConfig()
				.cls("document-list")
				.ToDictionary());

			ArrayList menuItems = new ArrayList();

			menuItems.Add(new Item(new ItemConfig()
				.text(Res.Menu_GetByNumber)
				.handler(new AnonymousDelegate(
					delegate
					{
						MessageBoxWrap.Prompt(Res.AddDocument_Title, Res.EnterDocumentNumber_Text,
							delegate (string button, string text)
							{
								if (button == "ok" && !string.IsNullOrEmpty(text))
									AddDocumentByNumber(text);
							});
					}))
				.ToDictionary()));

			menuItems.Add(new Item(new ItemConfig()
				.text(Res.Menu_MyUnprocessed)
				.handler(new AnonymousDelegate(AddUnprocessedDocuments))
				.ToDictionary()));

			menuItems.Add(new Item(new ItemConfig()
				.text(Res.Menu_AllMyDocuments)
				.handler(new AnonymousDelegate(AddAgentDocuments))
				.ToDictionary()));

			if (_args.HasAccessToDocumentList)
				menuItems.Add(new Item(new ItemConfig()
					.handler(new AnonymousDelegate(AddDocumentsFromFullList))
					.text(Res.Menu_FromList)
					.ToDictionary()));

			Menu menu = new Menu(new MenuConfig()
				.cls("simple-menu")
				.items(menuItems)
				.listeners(new Dictionary(
					"beforeshow", new MenuBeforeshowDelegate(
						delegate (Component sender) { ((Menu)sender).tryActivate(0); })))
				.ToDictionary());

			LinkButton addButton = new LinkButton(new ButtonConfig()
				.text(Res.Add)
				.menu(menu));

			Panel panel = new Panel(new PanelConfig()
				.items(new object[] { _documentList, addButton })
				.cls("documents-container")
				.ToDictionary());

			panel.setVisible(false);

			_applyDataCheckbox = new Checkbox(new CheckboxConfig()
				.boxLabel(Res.ProcessForm_ApplyToDocuments_Text)
				.handler(new CheckboxCheckDelegate(
					delegate (Checkbox checkbox, bool isChecked)
					{
						panel.setVisible(isChecked);

						if (isChecked)
							_documentList.doLayout();

						Type.InvokeMethod(Window.getEl(), "enableShadow", true);
					}))
				.listeners(new Dictionary("render", new AnonymousDelegate(
					delegate
					{
						Element el = (Element)Type.GetField(_applyDataCheckbox, "wrap");
						el.addClass("applyDataCheckbox");
					})))
				.ToDictionary());

			_documentsPanel = new Panel(new PanelConfig()
				.cls("documents")
				.items(new object[] { _reservationCount, _applyDataCheckbox, panel })
				.ToDictionary());

			Panel mainPanel = new Panel(new PanelConfig()
				.cls("main-panel")
				.layout("form")
				.items(new object[]
				{
					_passengerNameField,
					_passengerField.Widget,
					_passportWarnField,
					passportPanel,
					_gdsPassportStatus,
					_gdsPassportStatusWarnField,
					_orderSelector.Widget,
					_customerSelector.Widget,
					_intermediarySelector.Widget,
					_totalBox,
					_equalFarePanel,
					_commissionField,
					_serviceFeeField,
					_handlingField,
					_commissionDiscountField,
					_discountField,
					_refundServiceFeeField,
					_serviceFeePenaltyField,
					_grandTotalField,
					_paymentType,
					_descriptionField,
					_noteField,
					_editPenaltiesButton,
				})
				.ToDictionary());

			Form.add(mainPanel);
			Form.add(_documentsPanel);
		}

		protected override void OnSave()
		{
			if (_ticket != null)
			{
				_ticket.Passenger = _passengerField.GetObjectInfo();
				if (!Script.IsNullOrUndefined(_gdsPassportStatus.getValue()))
					_ticket.GdsPassportStatus = (GdsPassportStatus)((int)_gdsPassportStatus.value);
			}

			_document.Order = _orderSelector.GetObjectInfo();
			_document.Customer = _customerSelector.GetObjectInfo();
			_document.Intermediary = _intermediarySelector.GetObjectInfo();

			if (ShowEqualFare)
				_document.EqualFare = _equalFareField.getValue();

			_document.Commission = _commissionField.getValue();

			_document.ServiceFee = _serviceFeeField.getValue();
			_document.Discount = _discountField.getValue();
			_document.GrandTotal = _grandTotalField.getValue();

			if (UseHandling)
			{
				_document.Handling = _handlingField.getValue();
				_document.CommissionDiscount = _commissionDiscountField.getValue();
			}

			if (_refund != null)
			{
				_refund.RefundServiceFee = _refundServiceFeeField.getValue();
				_refund.ServiceFeePenalty = _serviceFeePenaltyField.getValue();
			}
			else if (_mco != null)
			{
				_mco.Description = (string)_descriptionField.getValue();
			}

			_document.PaymentType = (int)Type.InvokeMethod(_paymentType, "getValue");
			_document.Note = (string)_noteField.getValue();

			AviaDocumentProcessDto[] dtos = new AviaDocumentProcessDto[] { _document };

			if (_applyDataCheckbox.getValue())
			{
				foreach (object selectedDocument in _selectedDocuments)
				{
					AviaDocumentProcessDto dto = (AviaDocumentProcessDto)selectedDocument;

					dto.Order = _document.Order;
					dto.Customer = _document.Customer;
					dto.Intermediary = _document.Intermediary;
					dto.PaymentType = _document.PaymentType;

					if (_ticket != null)
					{
						AviaTicketProcessDto ticketDto = (AviaTicketProcessDto)dto;

						if (ticketDto.PassengerName == _document.PassengerName)
							ticketDto.Passenger = _ticket.Passenger;

						ArrayList list = new ArrayList();

						foreach (PenalizeOperationDto operation in _ticket.PenalizeOperations)
						{
							if (ticketDto.SegmentCount == 1 &&
								(operation.Type == PenalizeOperationType.ChangesAfterDeparture ||
								 operation.Type == PenalizeOperationType.RefundAfterDeparture ||
								 operation.Type == PenalizeOperationType.NoShowAfterDeparture)
								)
								continue;

							list.Add(operation);
						}

						ticketDto.PenalizeOperations = (PenalizeOperationDto[])list;
					}

					dtos[dtos.Length] = dto;
				}
			}

			if (_ticket != null)
				AviaService.ProcessAviaTickets((AviaTicketProcessDto[])dtos, CompleteSave, null);
			else if (_refund != null)
				AviaService.ProcessAviaRefunds((AviaRefundProcessDto[])dtos, CompleteSave, null);
			else
				AviaService.ProcessAviaMcos((AviaMcoProcessDto[])dtos, CompleteSave, null);
		}

		protected override void OnSaved(object result)
		{
			if (!_saveAndContinue)
				base.OnSaved(result);
		}

		private void TicketsLoaded(object result)
		{
			_documents = (AviaDocumentProcessDto[])result;

			_document = _documents[0];

			_selectedDocuments = new ArrayList();
			_addedDocuments = new AviaDocumentProcessDto[] { };

			_grandTotalComponentList = new BoxComponent[] { };
			_financeComponentList = new BoxComponent[] { };

			_refund = _type == ClassNames.AviaRefund ? (AviaRefundProcessDto)_document : null;
			_ticket = _type == ClassNames.AviaTicket ? (AviaTicketProcessDto)_document : null;
			_mco = _type == ClassNames.AviaMco ? (AviaMcoProcessDto)_document : null;

			Update();

			_recalculatedField = null;

			if (!Window.rendered)
				Window.show();
			else
			{
				RefreshWindowShadow();

				if (!_passengerField.Widget.hidden)
					_passengerField.Focus();
				else
					_orderSelector.Focus();
			}
		}

		private void Update()
		{
			_updating = true;

			try
			{
				UpdateTitle();

				UpdateFields();

				CreateGrandTotalMenu();

				UpdateFinanceData();

				UpdateDocumentList();
			}
			finally
			{
				_updating = false;
			}

			SetInitialValues();
		}

		private void UpdateTitle()
		{
			string text;

			switch (_type)
			{
				case ClassNames.AviaTicket:
					text = Res.AviaTicket_Handle_Title;
					break;

				case ClassNames.AviaRefund:
					text = Res.AviaRefund_Handle_Title;
					break;

				default:
					text = Res.AviaMco_Handle_Title;
					break;
			}

			Window.setTitle(string.Format(text, _document.Name));
		}

		private void UpdateFields()
		{
			_passengerNameField.setValue(_document.PassengerName);

			if (_ticket != null)
			{
				_passengerField.SetValue(_ticket.Passenger);
				_passengerField.UpdateTriggerStatus();

				_gdsPassportStatus.setValue(_ticket.GdsPassportStatus);

				Form.ShowItem(_passengerField.Widget);
				Form.ShowItem(_gdsPassportStatus);

				UpdateCopyPassportButton(_ticket.PassportValidationResult);
				UpdatePassportWarning(_ticket.PassportValidationResult);
				UpdateGdsPassportStatusWarn(_ticket.GdsPassportStatus);
			}
			else
			{
				Form.HideItem(_passengerField.Widget);
				Form.HideItem(_gdsPassportStatus);
				Form.HideItem(_passportWarnField);
				Form.HideItem(_gdsPassportStatusWarnField);

				_copyPassportButton.hide();
			}

			if (_mco != null)
			{
				Form.ShowItem(_descriptionField);
				_descriptionField.setValue(_mco.Description);
			}
			else
			{
				Form.HideItem(_descriptionField);
			}

			_orderSelector.SetValue(_document.Order);
			_customerSelector.SetValue(_document.Customer);
			_intermediarySelector.SetValue(_document.Intermediary);
			_paymentType.setValue(_document.PaymentType);
			_noteField.setValue(_document.Note);
		}

		private void UpdatePassportWarning(PassportValidationResult result)
		{
			if (_passengerField.GetObjectInfo() == null || result == PassportValidationResult.Valid)
			{
				Form.HideItem(_passportWarnField);

				return;
			}

			string text = null;
			bool error = false;

			switch (result)
			{
				case PassportValidationResult.NoPassport:
					{
						if (string.IsNullOrEmpty(_ticket.GdsPassport))
							text = string.Format(Res.Passenger_NoPassport_Warning, _document.Originator);
						else
							text = string.Format(Res.Passenger_NoGdsPassport_Warning, _document.Originator == GdsOriginator.Unknown ? "GDS" : EnumUtility.ToString(typeof(GdsOriginator), _document.Originator));

						break;
					}

				case PassportValidationResult.ExpirationDateNotValid:
					{
						text = Res.Passenger_PassportExpired_Warning;
						break;
					}

				case PassportValidationResult.NotValid:
					{
						string originator = _document.Originator == GdsOriginator.Unknown ? "GDS" : EnumUtility.ToString(typeof(GdsOriginator), _document.Originator);

						text = string.Format(Res.Passenger_PassportDataDoesntMatch_Warning, originator);
						error = true;

						break;
					}
			}

			string errorClass = "passport-error-label";
			string warnClass = "passport-warn-label";

			string removeCss = error ? warnClass : errorClass;
			string addCss = error ? errorClass : warnClass;

			if (!_passportWarnField.rendered)
				_passportWarnField.cls = addCss;
			else
			{
				_passportWarnField.removeClass(removeCss);
				_passportWarnField.addClass(addCss);
			}

			_passportWarnField.setValue(text);

			Form.ShowItem(_passportWarnField);
		}

		private void UpdateGdsPassportStatusWarn(GdsPassportStatus status)
		{
			if (_ticket.PassportRequired && status != GdsPassportStatus.Exist)
				Form.ShowItem(_gdsPassportStatusWarnField);
			else
				Form.HideItem(_gdsPassportStatusWarnField);
		}

		private void UpdateCopyPassportButton(PassportValidationResult validationResult)
		{
			if (_passengerField.GetObjectInfo() != null && validationResult == PassportValidationResult.Valid)
				_copyPassportButton.show();
			else
				_copyPassportButton.hide();
		}

		private void CreateGrandTotalMenu()
		{
			ArrayList items = new ArrayList();

			items.Add(new Item(new ItemConfig()
				.text(Res.CalculateServiceFee_Text)
				.handler(new AnonymousDelegate(delegate { OnGrandTotalFieldChanged(_serviceFeeField); }))
				.ToDictionary()));

			if (UseHandling)
			{
				items.Add(new Item(new ItemConfig()
					.text(Res.CalculateHandling_Text)
					.handler(new AnonymousDelegate(delegate { OnGrandTotalFieldChanged(_handlingField); }))
					.ToDictionary()));

				items.Add(new Item(new ItemConfig()
					.text(Res.CalculateCommissionDiscount_Text)
					.handler(new AnonymousDelegate(delegate { OnGrandTotalFieldChanged(_commissionDiscountField); }))
					.ToDictionary()));
			}

			items.Add(new Item(new ItemConfig()
				.text(Res.CalculateDiscount_Text)
				.handler(new AnonymousDelegate(delegate { OnGrandTotalFieldChanged(_discountField); }))
				.ToDictionary()));

			if (_refund != null)
			{
				items.Add(new Item(new ItemConfig()
					.text(Res.CalculateRefundServiceFee_Text)
					.handler(new AnonymousDelegate(delegate { OnGrandTotalFieldChanged(_refundServiceFeeField); }))
					.ToDictionary()));

				items.Add(new Item(new ItemConfig()
					.text(Res.CalculateServiceFeePenalty_Text)
					.handler(new AnonymousDelegate(delegate { OnGrandTotalFieldChanged(_serviceFeePenaltyField); }))
					.ToDictionary()));
			}

			_menu = new Menu(new MenuConfig()
				.cls("simple-menu")
				.listeners(new Dictionary(
					"beforeshow", new MenuBeforeshowDelegate(
						delegate { _menu.tryActivate(0); }),
					"beforehide", new AnonymousDelegate(
						delegate
						{
							if (!_updating)
								OnGrandTotalFieldChanged(_serviceFeeField);

							_updating = false;
						})))
				.items(items)
				.ToDictionary());
		}

		private void UpdateFinanceData()
		{
			if (_totalBox.rendered)
				_totalBox.getEl().update(RenderTotal());
			else
				_totalBox.autoEl = new Dictionary("html", RenderTotal());

			_commissionField.setValue(_document.Commission ?? MoneyDto.GetZeroMoney());
			_serviceFeeField.setValue(_document.ServiceFee ?? MoneyDto.GetZeroMoney());
			_discountField.setValue(_document.Discount ?? MoneyDto.GetZeroMoney());
			_grandTotalField.setValue(_document.GrandTotal ?? MoneyDto.GetZeroMoney());

			if (ShowEqualFare)
			{
				_equalFarePanel.setVisible(true);
				_equalFareField.setVisible(true);

				if (_document.Fare != null)
					_fareLabel.setText(string.Format(Res.AviaDocumentProcessForm_Fare_Text, MoneyDto.ToMoneyFullString(_document.Fare)));
			}
			else
			{
				_equalFarePanel.setVisible(false);
				_equalFareField.setVisible(false);
			}

			if (UseHandling)
			{
				Form.ShowItem(_handlingField);
				_handlingField.setValue(_document.Handling ?? MoneyDto.GetZeroMoney());

				Form.ShowItem(_commissionDiscountField);
				_commissionDiscountField.setValue(_document.CommissionDiscount ?? MoneyDto.GetZeroMoney());
			}
			else
			{
				Form.HideItem(_handlingField);
				((Field)_handlingField).setValue(null);

				Form.HideItem(_commissionDiscountField);
				((Field)_commissionDiscountField).setValue(null);
			}

			if (_refund != null)
			{
				Form.ShowItem(_refundServiceFeeField);
				Form.ShowItem(_serviceFeePenaltyField);

				_refundServiceFeeField.setValue(_refund.RefundServiceFee ?? MoneyDto.GetZeroMoney());
				_serviceFeePenaltyField.setValue(_refund.ServiceFeePenalty ?? MoneyDto.GetZeroMoney());
			}
			else
			{
				Form.HideItem(_refundServiceFeeField);
				Form.HideItem(_serviceFeePenaltyField);

				_refundServiceFeeField.setValue(null);
				_serviceFeePenaltyField.setValue(null);
			}

			_editPenaltiesButton.setVisible(_ticket != null);
		}

		private void UpdateDocumentList()
		{
			string text;
			if (!Script.IsNullOrUndefined(_documentList.items))
				_documentList.removeAll(true);

			_applyDataCheckbox.setValue(false);

			int unprocessedCount = 0;
			for (int i = 1; i < _documents.Length; i++)
			{
				AddDocument(_documents[i]);

				unprocessedCount += _documents[i].RequiresProcessing ? 1 : 0;
			}

			if (_ticket != null)
				text = Res.ReservationTicketCount_Text;
			else if (_refund != null)
				text = Res.ReservationRefundCount_Text;
			else
				text = Res.ReservationMcoCount_Text;

			text = "<div>" + string.Format(text, _documents.Length - 1) + "&nbsp;<span style='color: gray'>" + string.Format(Res.ReservationUnprocessedDocCount_Text, unprocessedCount) + "</span></div>";

			if (_reservationCount.rendered)
				_reservationCount.getEl().update(text);
			else
				_reservationCount.autoEl = new Dictionary("html", text);
		}



		private void SetInitialValues()
		{

			if (CanRecalculate())
			{

				if (_document.GrandTotal == null)
					RecalculateField(_grandTotalField);
				else if (_document.ServiceFee == null)
					RecalculateField(_serviceFeeField);
				else if (UseHandling && _document.Handling == null)
					RecalculateField(_handlingField);
				else if (UseHandling && _document.CommissionDiscount == null)
					RecalculateField(_commissionDiscountField);
				else if (_refund != null)
				{
					if (_refund.ServiceFeePenalty == null)
						RecalculateField(_serviceFeePenaltyField);
					else if (_refund.RefundServiceFee == null)
						RecalculateField(_refundServiceFeeField);
				}
				else if (_document.Discount == null)
					RecalculateField(_discountField);

			}
		}



		private string RenderTotal()
		{

			string commission = string.Empty;


			if (_document.Commission != null)
				commission = string.Format(" ({0} {1} {2})", DomainRes.Common_Commission.ToLowerCase(), _document.Commission.Amount.Format("N2"), _document.Commission.Currency.Name);


			return string.Format(
				@"<label class='x-form-item-label'>{0}:</label>" +
				"<div class='x-form-element'><div class='text'><div class='amount'>{1}</div><div class='currency'>{2}</div><div class='commission'>{3}</div></div></div>" +
				"<div class='x-form-clear-left'></div>",
				DomainRes.Product_Total,
				Script.IsValue(_document.Total) ? _document.Total.Amount.Format("N2") : "0.00",
				Script.IsValue(_document.Total) ? _document.Total.Currency.Name : "",
				commission
			);

		}



		private void GetPassengerActions(Field field, ActionCallbackDelegate actionCallbackDelegate)
		{

			Reference passenger = _passengerField.GetObjectInfo();


			if (passenger == null)
			{
				UpdateCopyPassportButton(PassportValidationResult.NoPassport);

				UpdatePassportWarning(PassportValidationResult.NoPassport);

				RefreshWindowShadow();

				return;
			}


			AviaService.ValidatePassengerPassport(

				(string)_document.Id,
				(string)passenger.Id,
				Script.IsNull(_ticket.GdsPassport),

				delegate (object result)
				{

					PassportValidationResponse response = (PassportValidationResponse)result;

					ArrayList actions = new ArrayList();


					switch (response.PassportValidationResult)
					{

						case PassportValidationResult.NoPassport:
							actions.Add(new Action(new ActionConfig()
								.text(Res.AddPassport_Action_Title)
								.handler(new AnonymousDelegate(AddPassport))
								.ToDictionary()));

							break;

						case PassportValidationResult.ExpirationDateNotValid:
							actions.Add(new Action(new ActionConfig()
								.text(Res.AddPassport_Action_Title)
								.handler(new AnonymousDelegate(AddPassport))
								.ToDictionary()));

							break;

						case PassportValidationResult.NotValid:

							/*actions.Add(new Action(new ActionConfig()
									.text(Res.Use_System_Passport_Action_Title)
									.handler(new AnonymousDelegate(SetIncorrectPassport))
									.ToDictionary()));

								actions.Add(new Action(new ActionConfig()
									.text(Res.Use_Gds_Passport_Action_Title)
									.handler(new AnonymousDelegate(delegate
									{
										EditPassport(response.Passport.Id, response.Passport.Version, (Dictionary)((object)_ticket.Passport));
									}))
									.ToDictionary()));
								*/

							actions.Add(new Action(new ActionConfig()

								.text(Res.ComparePassports)

								.handler(new AnonymousDelegate(delegate
								{

									PassportCompareForm form = new PassportCompareForm(response.Passport, _ticket.Passport);
									
									form.Saved += delegate (object arg1)
									{
										if ((bool)arg1)
											SetIncorrectPassport();
										else
											GenericService.Update(ClassNames.Passport, response.Passport.Id, response.Passport.Version, GetPassportData(_ticket.Passport), null,
												delegate { SetCorrectPassport(); }, null);
									};

									form.Open();

								}))

								.ToDictionary())

							);

							break;
					}


					UpdateCopyPassportButton(response.PassportValidationResult);

					UpdatePassportWarning(response.PassportValidationResult);

					RefreshWindowShadow();

					actionCallbackDelegate((Action[])actions);

				}, null

			);

		}



		private static object GetPassportData(PassportDto passport)
		{
			return new Dictionary(
				"LastName", passport.LastName,
				"FirstName", passport.FirstName,
				"MiddleName", passport.MiddleName,
				"Citizenship", passport.Citizenship != null ? new object[] { passport.Citizenship.Id, passport.Citizenship.Name, passport.Citizenship.Type } : null,
				"Birthday", passport.Birthday,
				"Gender", passport.Gender,
				"ExpiredOn", passport.ExpiredOn
				);
		}

		private Action[] GetPassengerSelectorActions(Store store, Record[] records, string query)
		{
			if (_ticket == null)
				return null;

			string passengerName = GetPersonName(_document.PassengerName);

			bool addNewPassenger = _ticket.SuggestPassenger == null;
			bool setPassenger = !addNewPassenger;
			bool addNewPerson = !string.IsNullOrEmpty(query) && !string.Equals(passengerName, query, true);

			if (records != null)
			{
				for (int i = 0; i < records.Length; i++)
				{
					string text = (string)records[i].get("Name");

					if (string.Equals(text, query, true))
						addNewPerson = false;

					if (string.Equals(text, passengerName, true))
					{
						addNewPassenger = false;
						setPassenger = false;
					}
				}
			}

			ArrayList actions = new ArrayList();

			if (addNewPassenger)
				actions.Add(new Action(new ActionConfig()
					.text(string.Format(Res.AddPassenger_Action_Title, passengerName))
					.handler(new AnonymousDelegate(
						delegate { AddPassenger(passengerName); }))
					.ToDictionary()));

			if (setPassenger)
				actions.Add(new Action(new ActionConfig()
					.text(string.Format(Res.SetPassenger_Action_Title, _ticket.SuggestPassenger.Name))
					.handler(new AnonymousDelegate(
						delegate { _passengerField.SetValue(_ticket.SuggestPassenger); }))
					.ToDictionary()));

			if (addNewPerson)
				actions.Add(new Action(new ActionConfig()
					.text(string.Format(Res.AddPassenger_Action_Title, query))
					.handler(new AnonymousDelegate(
						delegate { AddPassenger(query); }))
					.ToDictionary()));

			return actions.Count == 0 ? null : (Action[])actions;
		}

		private Action[] GetCustomerSelectorActions(Store store, Record[] records, string query)
		{
			ArrayList actions = new ArrayList();

			Reference passenger = _passengerField.GetObjectInfo();
			Reference customer = _customerSelector.GetObjectInfo();

			if (passenger != null && !string.Equals(passenger.Name, query, true))
				actions.Add(new Action(new ActionConfig()
					.text(Res.SetPassengerAsCustomer_Action_Title)
					.handler(new AnonymousDelegate(
						delegate { _customerSelector.SetValue(passenger); })).ToDictionary()));


			if (customer == null || !string.Equals(customer.Name, query, true))
			{
				Action[] items = GetIntermediarySelectorActions(store, records, query);

				if (items != null)
					actions.AddRange(items);
			}

			return (Action[])actions;
		}

		private static Action[] GetIntermediarySelectorActions(Store store, Record[] records, string query)
		{
			if (string.IsNullOrEmpty(query))
				return null;

			if (records != null)
				for (int i = 0; i < records.Length; i++)
				{
					string text = (string)records[i].get("Name");

					if (string.Equals(text, query, true))
						return null;
				}

			return new Action[]
			{
				new Action(new ActionConfig()
					.text(Res.AddPerson_Action_Title)
					.handler(new ComboBoxCustomActionDelegate(
						delegate(ObjectSelector selector1, string text1) { AddParty(selector1, text1, ClassNames.Person); }))
					.ToDictionary()),
				new Action(new ActionConfig()
					.text(Res.AddOrganization_Action_Title)
					.handler(new ComboBoxCustomActionDelegate(
						delegate(ObjectSelector selector2, string text2) { AddParty(selector2, text2, ClassNames.Organization); }))
					.ToDictionary())
			};
		}

		private static void AddParty(ObjectSelector selector, string text, string type)
		{
			if (string.IsNullOrEmpty(text))
				return;

			if (AppManager.SystemConfiguration.IsOrganizationCodeRequired && type == ClassNames.Organization)
			{
				Dictionary nameDictionary = new Dictionary("Name", text, "IsCustomer", true);

				FormsRegistry.EditObject(type, null, nameDictionary,
					delegate (object result)
					{
						OrganizationDto dto = (OrganizationDto)((ItemResponse)result).Item;

						Reference data = new Reference();
						data.Id = dto.Id;
						data.Name = dto.Name;
						data.Type = ClassNames.Organization;

						selector.SetValue(data);
					}, null, null);
			}
			else
				PartyService.CreateCustomer(type, text,
					delegate (object result)
					{
						Dictionary party = Dictionary.GetDictionary(result);

						Reference data = new Reference();
						data.Id = party[ObjectPropertyNames.Id];
						data.Name = (string)party[ObjectPropertyNames.Reference];
						data.Type = (string)party[ObjectPropertyNames.ObjectClass];

						selector.SetValue(data);

						string caption = null;

						switch (data.Type)
						{
							case "Person":
								caption = DomainRes.Person_Caption_List;
								break;

							case "Organization":
								caption = DomainRes.Organization_Caption_List;
								break;

							case "Department":
								caption = DomainRes.Department_Caption_List;
								break;
						}

						MessageRegister.Info(caption, BaseRes.Created + " " + data.Name);
					},
					null);
		}

		private void AddPassenger(string text)
		{
			if (string.IsNullOrEmpty(text))
				return;

			GenericService.Update("Person", null, null, new Dictionary("Name", text), null,
				delegate (object result)
				{
					ItemResponse response = (ItemResponse)result;
					Dictionary person = Dictionary.GetDictionary(response.Item);

					Reference obj = new Reference();
					obj.Id = person["Id"];
					obj.Name = (string)person["Name"];
					obj.Type = "Person";

					_passengerField.SetValue(obj);

					AddPassport();

					MessageRegister.Info(DomainRes.Person_Caption_List, BaseRes.Created + " " + person["Name"]);
				}, null);
		}

		private void AddPassport()
		{
			Dictionary passport = null;

			if (_ticket.Passport != null)
				passport = Dictionary.GetDictionary(_ticket.Passport);
			else if (!string.IsNullOrEmpty(_ticket.PassengerName))
			{
				string[] passengerName = GetPersonName(_document.PassengerName).Split(" ");

				passport = new Dictionary("LastName", passengerName[0]);

				if (passengerName.Length > 1)
					passport["FirstName"] = passengerName[1];
			}

			EditPassport(null, null, passport);
		}

		private void EditPassport(object id, object version, Dictionary value)
		{
			FormsRegistry.EditObject(ClassNames.Passport, id, value,
				delegate (object result)
				{
					PassportDto dto = (PassportDto)((ItemResponse)result).Item;
					dto.Owner = _passengerField.GetObjectInfo();


					GenericService.Update(ClassNames.Passport, id, version, dto, null,
						delegate
						{
							MessageRegister.Info(DomainRes.Passport_Caption_List, (dto.Id == null ? BaseRes.Created : BaseRes.Updated) + " " + dto.Number);

							_passengerField.Focus();

							RefreshActions();
						}, null);
				}
				, null, null, LoadMode.Local);
		}

		private void SetIncorrectPassport()
		{
			if (_ticket == null)
				return;

			_ticket.GdsPassport = null;
			_ticket.GdsPassportStatus = GdsPassportStatus.Incorrect;
			_gdsPassportStatus.setValue(GdsPassportStatus.Incorrect);
			_passportWarnField.setValue(string.Empty);

			Form.HideItem(_passportWarnField);

			UpdateGdsPassportStatusWarn(((GdsPassportStatus)_gdsPassportStatus.getValue()));

			RefreshActions();
		}

		private void SetCorrectPassport()
		{
			if (_ticket == null)
				return;

			_ticket.GdsPassportStatus = GdsPassportStatus.Exist;

			_gdsPassportStatus.setValue(GdsPassportStatus.Exist);

			_passportWarnField.setValue(string.Empty);
			_gdsPassportStatusWarnField.setValue(string.Empty);

			Form.HideItem(_passportWarnField);
			Form.HideItem(_gdsPassportStatusWarnField);

			RefreshActions();
		}

		private void CopyPassportData()
		{
			Reference passenger = _passengerField.GetObjectInfo();

			if (passenger == null)
				return;

			PassportListForm form = new PassportListForm(_ticket.Originator, (string)passenger.Id);
			form.Close +=
				delegate { _gdsPassportStatus.focus(); };

			form.Open();
		}

		private void OnAmountChange(Field decimalField, object newValue, object oldValue)
		{
			if (_updating)
				return;

			if (newValue == null)
				decimalField.value = 0;

			if (_updating || !CanRecalculate())
				return;

			for (int i = 0; i < _addedDocuments.Length; i++)
			{
				AviaDocumentProcessDto document = _addedDocuments[i];

				if (!CanRecalculateDocument(document))
					continue;

				if (decimalField == _serviceFeeField.DecimalField)
					document.ServiceFee.Amount = (decimal)newValue;
				else if (decimalField == _handlingField.DecimalField)
					document.Handling.Amount = (decimal)newValue;
				else if (decimalField == _commissionDiscountField.DecimalField)
					document.CommissionDiscount.Amount = (decimal)newValue;
				else if (decimalField == _discountField.DecimalField)
					document.Discount.Amount = (decimal)newValue;
				else if (decimalField == _refundServiceFeeField.DecimalField)
					((AviaRefundProcessDto)document).RefundServiceFee.Amount = (decimal)newValue;
				else if (decimalField == _serviceFeePenaltyField.DecimalField)
					((AviaRefundProcessDto)document).ServiceFeePenalty.Amount = (decimal)newValue;
				else if (decimalField == _grandTotalField.DecimalField)
					document.GrandTotal.Amount = (decimal)newValue;

				if (decimalField != _grandTotalField.DecimalField)
				{
					RecalculateDocument(document, _grandTotalField);
					RefreshFinanceDataText(i);
				}
			}

			if (decimalField != _grandTotalField.DecimalField)
				RecalculateField(_grandTotalField);
			else
				_menu.show(_grandTotalField.getEl());
		}

		private bool CanRecalculate()
		{
			bool res =
				_serviceFeeField.getValue() != null &&
				_discountField.getValue() != null &&
				_grandTotalField.getValue() != null;

			if (UseHandling)
				res = res && _handlingField.getValue() != null && _commissionDiscountField.getValue() != null;

			if (_refund != null)
				res = res &&
					_serviceFeePenaltyField.getValue() != null &&
						_refundServiceFeeField.getValue() != null;

			return res && AreCurrenciesEqual();
		}

		private bool AreCurrenciesEqual()
		{
			Reference currency = _document.Total.Currency;

			bool res = Reference.Equals(_serviceFeeField.Currency, currency) &&
				Reference.Equals(_discountField.Currency, currency) &&
				Reference.Equals(_grandTotalField.Currency, currency);

			if (UseHandling)
				res = res && Reference.Equals(_handlingField.Currency, currency) &&
					Reference.Equals(_commissionDiscountField.Currency, currency);

			if (_refund != null)
				res = res && Reference.Equals(_refundServiceFeeField.Currency, currency) &&
					Reference.Equals(_serviceFeePenaltyField.Currency, currency);

			return res;
		}

		private void OnGrandTotalFieldChanged(Field field)
		{
			RecalculateField(field);

			for (int i = 0; i < _addedDocuments.Length; i++)
			{
				AviaDocumentProcessDto document = _addedDocuments[i];

				if (CanRecalculateDocument(document))
				{
					RecalculateDocument(document, field);
					RefreshFinanceDataText(i);
				}
			}

			_paymentType.focus();

			_updating = true;

			if (_invokeSave)
			{
				_invokeSave = false;

				Save();
			}
		}

		private void RecalculateField(Field field)
		{
			_recalculatedField = field;

			if (field == _serviceFeeField)
				_serviceFeeField.Amount = _grandTotalField.Amount - _document.Total.Amount - _handlingField.Amount + _discountField.Amount + _serviceFeePenaltyField.Amount + _refundServiceFeeField.Amount + _commissionDiscountField.Amount;
			else if (field == _handlingField)
				_handlingField.Amount = _grandTotalField.Amount - _document.Total.Amount - _serviceFeeField.Amount + _discountField.Amount + _serviceFeePenaltyField.Amount + _refundServiceFeeField.Amount + _commissionDiscountField.Amount;
			else if (field == _commissionDiscountField)
				_commissionDiscountField.Amount = _document.Total.Amount + _serviceFeeField.Amount + _handlingField.Amount - _grandTotalField.Amount - _serviceFeePenaltyField.Amount - _refundServiceFeeField.Amount - _discountField.Amount;
			else if (field == _discountField)
				_discountField.Amount = _document.Total.Amount + _serviceFeeField.Amount + _handlingField.Amount - _grandTotalField.Amount - _serviceFeePenaltyField.Amount - _refundServiceFeeField.Amount - _commissionDiscountField.Amount;
			else if (field == _refundServiceFeeField)
				_refundServiceFeeField.Amount = _document.Total.Amount + _serviceFeeField.Amount + _handlingField.Amount - _grandTotalField.Amount - _discountField.Amount - _serviceFeePenaltyField.Amount - _commissionDiscountField.Amount;
			else if (field == _serviceFeePenaltyField)
				_serviceFeePenaltyField.Amount = _document.Total.Amount + _serviceFeeField.Amount + _handlingField.Amount - _grandTotalField.Amount - _discountField.Amount - _refundServiceFeeField.Amount - _commissionDiscountField.Amount;
			else if (field == _grandTotalField)
				_grandTotalField.Amount = _document.Total.Amount + _serviceFeeField.Amount + _handlingField.Amount - _discountField.Amount - _serviceFeePenaltyField.Amount - _refundServiceFeeField.Amount - _commissionDiscountField.Amount;
		}

		private bool CanRecalculateDocument(AviaDocumentProcessDto document)
		{
			Reference currency = _document.Total.Currency;

			bool res = Reference.Equals(document.ServiceFee.Currency, currency) &&
				Reference.Equals(document.Discount.Currency, currency) && Reference.Equals(document.GrandTotal.Currency, currency);

			if (_refund != null)
			{
				AviaRefundProcessDto refund = (AviaRefundProcessDto)document;

				res = res && Reference.Equals(refund.RefundServiceFee.Currency, currency) &&
					Reference.Equals(refund.ServiceFeePenalty.Currency, currency);
			}

			return res;
		}

		private void RecalculateDocument(AviaDocumentProcessDto document, Field field)
		{

			decimal value = 0;
			AviaRefundProcessDto refund = null;

			if (_refund != null)
			{
				refund = (AviaRefundProcessDto)document;
				value = refund.ServiceFeePenalty.Amount + refund.RefundServiceFee.Amount;
			}

			decimal handling = 0;
			decimal commissionDiscount = 0;

			if (UseHandling)
			{
				handling = document.Handling.Amount;
				commissionDiscount = document.CommissionDiscount.Amount;
			}

			if (field == _serviceFeeField)
				document.ServiceFee.Amount = document.GrandTotal.Amount - document.Total.Amount - handling + commissionDiscount + document.Discount.Amount + value;
			if (field == _handlingField)
				document.Handling.Amount = document.GrandTotal.Amount - document.Total.Amount - document.ServiceFee.Amount + commissionDiscount + document.Discount.Amount + value;
			if (field == _commissionDiscountField)
				document.CommissionDiscount.Amount = document.GrandTotal.Amount - document.Total.Amount - handling - document.ServiceFee.Amount + document.Discount.Amount + value;
			else if (field == _discountField)
				document.Discount.Amount = document.Total.Amount + document.ServiceFee.Amount + handling - commissionDiscount - document.GrandTotal.Amount - value;
			else if (refund != null && field == _refundServiceFeeField)
				refund.RefundServiceFee.Amount = refund.Total.Amount + refund.ServiceFee.Amount + handling - commissionDiscount - refund.GrandTotal.Amount - refund.Discount.Amount - refund.ServiceFeePenalty.Amount;
			else if (refund != null && field == _serviceFeePenaltyField)
				refund.ServiceFeePenalty.Amount = refund.Total.Amount + refund.ServiceFee.Amount + handling - commissionDiscount - refund.GrandTotal.Amount - refund.Discount.Amount - refund.RefundServiceFee.Amount;
			else if (field == _grandTotalField)
				document.GrandTotal.Amount = document.Total.Amount + document.ServiceFee.Amount + handling - commissionDiscount - document.Discount.Amount - value;

		}



		private bool AddDocument(AviaDocumentProcessDto dto)
		{

			if (_document.Id == dto.Id)
				return false;


			AviaRefundProcessDto refund = _refund != null ? (AviaRefundProcessDto)dto : null;

			for (int i = 0; i < _addedDocuments.Length; i++)
			{
				if (_addedDocuments[i].Id == dto.Id)
					return false;
			}


			_addedDocuments[_addedDocuments.Length] = dto;

			Reference currency = dto.Total.Currency;

			if (Script.IsNullOrUndefined(dto.ServiceFee))
				dto.ServiceFee = MoneyDto.GetZeroMoney(currency);

			if (UseHandling && Script.IsNullOrUndefined(dto.Handling))
				dto.Handling = MoneyDto.GetZeroMoney(currency);

			if (UseHandling && Script.IsNullOrUndefined(dto.CommissionDiscount))
				dto.CommissionDiscount = MoneyDto.GetZeroMoney(currency);

			if (Script.IsNullOrUndefined(dto.Discount))
				dto.Discount = MoneyDto.GetZeroMoney(currency);


			if (refund != null)
			{
				if (Script.IsNullOrUndefined(refund.RefundServiceFee))
					refund.RefundServiceFee = MoneyDto.GetZeroMoney(currency);

				if (Script.IsNullOrUndefined(refund.ServiceFeePenalty))
					refund.ServiceFeePenalty = MoneyDto.GetZeroMoney(currency);
			}


			if (Script.IsNullOrUndefined(dto.GrandTotal))
			{
				dto.GrandTotal = MoneyDto.GetZeroMoney(currency);
				dto.GrandTotal.Amount = dto.Total.Amount;

				if (CanRecalculateDocument(dto))
				{
					dto.GrandTotal.Amount += dto.ServiceFee.Amount - dto.Discount.Amount;

					if (UseHandling)
						dto.GrandTotal.Amount += dto.Handling.Amount;

					if (refund != null)
						dto.GrandTotal.Amount -= refund.RefundServiceFee.Amount - refund.ServiceFeePenalty.Amount;
				}
			}


			if (_recalculatedField != null && CanRecalculateDocument(dto))
			{

				if (_serviceFeeField != _recalculatedField)
					dto.ServiceFee = _serviceFeeField.getValue();

				if (_handlingField != _recalculatedField)
					dto.Handling = _handlingField.getValue();

				if (_commissionDiscountField != _recalculatedField)
					dto.CommissionDiscount = _commissionDiscountField.getValue();

				if (_discountField != _recalculatedField)
					dto.Discount = _discountField.getValue();

				if (_grandTotalField != _recalculatedField)
					dto.GrandTotal = _grandTotalField.getValue();

				if (refund != null)
				{
					if (_refundServiceFeeField != _recalculatedField)
						refund.RefundServiceFee = _refundServiceFeeField.getValue();

					if (_serviceFeePenaltyField != _recalculatedField)
						refund.ServiceFeePenalty = _serviceFeePenaltyField.getValue();
				}

				RecalculateDocument(dto, _recalculatedField);

			}


			string cssClass = dto.RequiresProcessing ? "textColor-red" : (dto.IsVoid ? "textColor-gray" : string.Empty);

			string text = Script.IsNullOrUndefined(dto.Number) ? dto.Name :
																					string.Format("{0}&nbsp;&nbsp;&nbsp;{1}", dto.Name, dto.PassengerName);

			Checkbox checkbox = new Checkbox(new CheckboxConfig()
				.boxLabel(text)
				.checked_(dto.RequiresProcessing)
				.handler(new CheckboxCheckDelegate(
					delegate (Checkbox sender, bool isChecked)
					{
						if (isChecked)
							_selectedDocuments.Add(dto);
						else
							_selectedDocuments.Remove(dto);
					}))
				.listeners(new Dictionary("render", new GenericOneArgDelegate(
					delegate (object sender) { ((Checkbox)sender).getEl().addClass(cssClass); })))
				.ToDictionary());

			BoxComponent grandTotal = new BoxComponent(new BoxComponentConfig()
				.cls("totals " + cssClass)
				.ToDictionary());

			BoxComponent financeData = new BoxComponent(new BoxComponentConfig()
				.cls("finance")
				.ToDictionary());

			_grandTotalComponentList[_grandTotalComponentList.Length] = grandTotal;
			_financeComponentList[_financeComponentList.Length] = financeData;

			RefreshFinanceDataText(_addedDocuments.Length - 1);

			_documentList.add(checkbox);
			_documentList.add(grandTotal);
			_documentList.add(financeData);

			if (dto.RequiresProcessing)
				_selectedDocuments.Add(dto);


			return true;

		}



		private void RefreshFinanceDataText(int index)
		{
			AviaDocumentProcessDto dto = _addedDocuments[index];

			BoxComponent component = _grandTotalComponentList[index];

			string html = string.Format("<div>{0}</div>", MoneyDto.ToMoneyFullString(dto.GrandTotal));

			if (component.rendered)
				component.getEl().update(html);
			else
				component.autoEl = new Dictionary("html", html);

			component = _financeComponentList[index];

			html = _refund == null ?
				string.Format(Res.AddedAviaDocumentFinanceData_Format,
					MoneyDto.ToMoneyFullString(dto.Total), MoneyDto.ToMoneyFullString(dto.ServiceFee), MoneyDto.ToMoneyFullString(dto.Discount)) :
				string.Format(Res.AddedAviaRefundFinanceData_Format,
					MoneyDto.ToMoneyFullString(dto.Total), MoneyDto.ToMoneyFullString(dto.ServiceFee), MoneyDto.ToMoneyFullString(dto.Discount),
					MoneyDto.ToMoneyFullString(((AviaRefundProcessDto)dto).RefundServiceFee), MoneyDto.ToMoneyFullString(((AviaRefundProcessDto)dto).ServiceFeePenalty));

			html = "<div>" + html + "</div>";

			if (component.rendered)
				component.getEl().update(html);
			else
				component.autoEl = new Dictionary("html", html);
		}

		private void AddDocumentByNumber(string number)
		{
			AviaService.GetAviaDocumentForHandlingByNumber(number, _type,
				delegate (object result)
				{
					if (result == null)
					{
						MessageBoxWrap.Show(new Dictionary(
							"msg", string.Format(Res.NoDocumentFound_Text, number),
							"buttons", MessageBox.OK,
							"icon", MessageBox.WARNING
							));

						return;
					}

					if (AddDocument((AviaDocumentProcessDto)result))
					{
						_documentList.doLayout();

						Type.InvokeMethod(Window.getEl(), "enableShadow", true);
					}
				}, null);
		}

		private void AddUnprocessedDocuments()
		{
			ArrayList list = new ArrayList();

			list.Add(PropertyFilterExtention.CreateFilter("Type", FilterOperator.IsIn, new object[] { _document.Type }, false));
			list.Add(PropertyFilterExtention.CreateFilter("RequiresProcessing", FilterOperator.Equals, true, false));

			if (AppManager.CurrentPerson.Name != null)
				list.Add(PropertyFilterExtention.CreateFilter("Seller", FilterOperator.Equals, AppManager.CurrentPerson.Name, false));

			RangeRequest request = new RangeRequest();
			request.Sort = "IssueDate";
			request.Dir = "DESC";
			request.Filters = (PropertyFilter[])list;

			FormsRegistry.SelectObjects(ClassNames.AviaDocument, request, false,
				delegate (object arg1)
				{
					object[] ids = (object[])arg1;

					AviaService.GetAviaDocumentsForProcess(ids, _type,
						delegate (object result)
						{
							AviaDocumentProcessDto[] dtos = (AviaDocumentProcessDto[])result;

							for (int i = 0; i < dtos.Length; i++)
								AddDocument(dtos[i]);

							_documentList.doLayout();

							Type.InvokeMethod(Window.getEl(), "enableShadow", true);
						}, null);
				}, null);
		}

		private void AddAgentDocuments()
		{
			ArrayList list = new ArrayList();

			list.Add(PropertyFilterExtention.CreateFilter("Type", FilterOperator.IsIn, new object[] { _document.Type }, false));

			if (AppManager.CurrentPerson.Name != null)
				list.Add(PropertyFilterExtention.CreateFilter("Seller", FilterOperator.Equals, AppManager.CurrentPerson.Name, false));

			RangeRequest request = new RangeRequest();
			request.Sort = "IssueDate";
			request.Dir = "DESC";
			request.Filters = (PropertyFilter[])list;

			FormsRegistry.SelectObjects(ClassNames.AviaDocument, request, false,
				delegate (object arg1)
				{
					object[] ids = (object[])arg1;

					AviaService.GetAviaDocumentsForProcess(ids, _type,
						delegate (object result)
						{
							AviaDocumentProcessDto[] dtos = (AviaDocumentProcessDto[])result;

							for (int i = 0; i < dtos.Length; i++)
								AddDocument(dtos[i]);

							_documentList.doLayout();

							Type.InvokeMethod(Window.getEl(), "enableShadow", true);
						}, null);
				}, null);
		}

		private void AddDocumentsFromFullList()
		{
			RangeRequest request = new RangeRequest();
			request.Sort = "IssueDate";
			request.Dir = "DESC";
			request.Filters = new PropertyFilter[]
			{
				PropertyFilterExtention.CreateFilter("Type", FilterOperator.IsIn, new object[] { _document.Type }, false)
			};

			FormsRegistry.SelectObjects(ClassNames.AviaDocument, request, false,
				delegate (object arg1)
				{
					object[] ids = (object[])arg1;

					AviaService.GetAviaDocumentsForProcess(ids, _type,
						delegate (object result)
						{
							AviaDocumentProcessDto[] dtos = (AviaDocumentProcessDto[])result;

							for (int i = 0; i < dtos.Length; i++)
								AddDocument(dtos[i]);

							_documentList.doLayout();

							Type.InvokeMethod(Window.getEl(), "enableShadow", true);
						}, null);
				}, null);
		}

		private void OnKeyEnterPress(object key, EventObject e)
		{
			if (!e.shiftKey && e.ctrlKey && !e.altKey)
			{
				e.stopEvent();

				_saveAndContinue = false;

				_saveButton.focus();

				if ((bool)Type.InvokeMethod(_menu, "isVisible"))
					_invokeSave = true;
				else
					Save();

				return;
			}

			if (!e.shiftKey && !e.ctrlKey && e.altKey)
			{
				e.stopEvent();

				_saveAndContinue = true;

				_saveAndContinueButton.focus();

				if ((bool)Type.InvokeMethod(_menu, "isVisible"))
					_invokeSave = true;
				else
					Save();
			}
		}

		private void EditPenalizeOperations()
		{
			PenalizeOperationEditForm form = new PenalizeOperationEditForm(_ticket.PenalizeOperations, _ticket.SegmentCount > 1);
			form.Saved +=
				delegate (object result)
				{
					PenalizeOperationDto[] dtos = (PenalizeOperationDto[])result;

					_ticket.PenalizeOperations = dtos;
				};

			form.Close +=
				delegate { _applyDataCheckbox.focus(); };

			form.Open();
		}

		private static string GetPersonName(string gdsPassengerName)
		{
			if (string.IsNullOrEmpty(gdsPassengerName))
				return null;

			string lastname = gdsPassengerName.Split('/')[0].ToLowerCase();
			string name = string.Empty;
			if (gdsPassengerName.Split('/').Length > 1)
				name = gdsPassengerName.Split('/')[1].ToLowerCase();

			if (!string.IsNullOrEmpty(name) && (name.Length > 1))
			{
				lastname = lastname.Substring(0, 1).ToUpperCase() + lastname.Substring(1, lastname.Length);
				name = name.Substring(0, 1).ToUpperCase() + name.Substring(1, name.Length);
				name = name.ReplaceRegex(new RegularExpression(@"(mr|mrs|mstr|miss)$"), string.Empty);
			}

			return (lastname + " " + name).Trim();
		}

		private void OnOrderChanged(Field objthis, object newvalue, object oldvalue)
		{
			if (_updating)
				return;

			Array val = (Array)newvalue;

			if (val != null)
			{
				Record record = ((Store)((ComboBox)objthis).store).getById((string)val[Reference.IdPos]);

				object[] customer = (object[])record.get("Customer");

				_updating = true;
				_customerSelector.SetValue(customer);
				_updating = false;
			}
		}

		private void OnCustomerChanged(Field objthis, object newvalue, object oldvalue)
		{
			if (_updating || newvalue == oldvalue || _orderSelector.GetValue() == null)
				return;

			Array oldArray = (Array)oldvalue;
			Array newArray = (Array)newvalue;

			if (oldArray != null && newArray != null && oldArray[0] == newArray[0])
				return;

			MessageBoxWrap.Confirm(Res.Confirmation, Res.AviaDocumentProcessForm_CustomerChanged, delegate (string button, string text)
			{
				_updating = true;

				if (button == "yes")
					_orderSelector.SetValue(null);
				else
					_customerSelector.SetValue(oldvalue);

				_updating = false;
			});
		}

		private readonly AviaDocumentProcessArgs _args;
		private AviaDocumentProcessDto _document;
		private AviaDocumentProcessDto[] _documents;
		private AviaDocumentProcessDto[] _addedDocuments;

		private BoxComponent[] _grandTotalComponentList;
		private BoxComponent[] _financeComponentList;

		private ArrayList _selectedDocuments;
		private AviaRefundProcessDto _refund;
		private AviaTicketProcessDto _ticket;
		private AviaMcoProcessDto _mco;
		private string _type;

		private DisplayField _passengerNameField;
		private ObjectSelector _passengerField;
		private DisplayField _passportWarnField;
		private LinkButton _copyPassportButton;


		private Field _gdsPassportStatus;
		private DisplayField _gdsPassportStatusWarnField;

		private ObjectSelector _customerSelector;
		private ObjectSelector _intermediarySelector;

		private BoxComponent _totalBox;

		private MoneyControl _commissionField;
		private MoneyControl _serviceFeeField;
		private MoneyControl _handlingField;
		private MoneyControl _commissionDiscountField;
		private MoneyControl _discountField;
		private MoneyControl _refundServiceFeeField;
		private MoneyControl _serviceFeePenaltyField;
		private MoneyControl _grandTotalField;

		private ComboBox _paymentType;

		private readonly Button _saveAndContinueButton;
		private readonly Button _saveButton;
		private readonly Button _cancelButton;

		private Menu _menu;
		private bool _saveAndContinue;
		private bool _updating;
		private bool _invokeSave;
		private BoxComponent _reservationCount;
		private Panel _documentList;
		private Panel _documentsPanel;
		private Checkbox _applyDataCheckbox;
		private Field _recalculatedField;
		private Field _descriptionField;
		private Field _noteField;
		private MoneyControl _equalFareField;
		private Panel _equalFarePanel;
		private Label _fareLabel;
		private LinkButton _editPenaltiesButton;
		private ObjectSelector _orderSelector;
	}
}