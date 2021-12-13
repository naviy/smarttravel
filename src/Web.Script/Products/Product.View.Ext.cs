using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.data;
using Ext.form;
using Ext.menu;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using ColumnConfig = Ext.grid.ColumnConfig;
using ComboBox = LxnBase.UI.Controls.ComboBox;
using Field = Ext.form.Field;
using Record = Ext.data.Record;
using MenuItemConfig = Ext.menu.ItemConfig;




namespace Luxena.Travel
{



	//===g






	partial class ProductSemantic
	{

		//---g



		[PreserveCase] public SemanticMember ReissuedBy = Member
			.Reference("Product")
			.Title(DomainRes.Common_ReissuedBy);

		[PreserveCase] public SemanticMember Refund = Member
			.Reference("Product")
			.Title(DomainRes.Product_Refund);




		[PreserveCase] public SemanticMember PassengerRow = Member;

		[PreserveCase] public SemanticMember OnePassengerRow = Member;

		[PreserveCase] public SemanticMember CustomerAndOrder = Member;

		[PreserveCase] public SemanticMember BookerRow = Member;

		[PreserveCase] public SemanticMember TicketerRow = Member;

		[PreserveCase] public SemanticMember SellerAndOwnerRow = Member;

		[PreserveCase] public SemanticMember PnrAndTourCodes = Member;

		[PreserveCase] public SemanticMember Finance = Member;



		//---g



		public override void Initialize()
		{

			CustomerAndOrder.ToEditor = new Product_CustomerAndOrderEditor(this).ToEditor;

			Finance.ToEditor = new Product_FinanceEditor(this).ToEditor;


			SellerAndOwnerRow.ToEditor = delegate
			{

				// ReSharper disable once SuspiciousTypeConversion.Global
				ProductEditForm pform = (ProductEditForm)EditForm;

				Field sellerField = Seller.ToField(-1, BoldLabel);

				Field ownerField;


				if (pform.DisplayOwnerAsLabel)
				{

					ownerField = new DisplayField(new DisplayFieldConfig()
						.hideLabel(true)
						.labelSeparator(string.Empty)
						.value(pform.ProductOwners[0].Name)
						.ToDictionary()
					);


					EditForm.Members.Add3(EditForm,
						delegate { },
						delegate
						{
							object value = EditForm.GetValue(Owner._name);
							if (!Script.IsValue(value))
								value = pform.ProductOwners[0];
							EditForm.SetValue(Owner._name, value);
						}
					);

				}

				else
				{

					ComboBoxConfig config = new ReferenceSelectorConfig(pform.ProductOwners)
						.hideLabel(true)
						.labelSeparator(string.Empty)
						.width(EditForm.FieldMaxWidth);

					FormMember member = EditForm.Members.Add2(EditForm, Owner, config, null);

					ownerField = new ComboBox(config.ToDictionary());
					member.SetField(ownerField);

					member.OnSaveValue += delegate
					{
						object value = ((ComboBox)ownerField).GetObjectInfo();
						EditForm.SetValue("Owner", value);
					};

				}


				return RowPanel(new Component[]
				{
					sellerField,
					TextComponent("/"),
					ownerField,
				});

			};



			IssueDate.SetColumn(false, null, GetDateRenderer());

			ReissueFor.SetColumn(true, 100);

			Name.SetColumn(false, 100, GetNumberRenderer());

			Intermediary.SetColumn(true);
			Customer.SetColumn(false, 135);
			PaymentType.SetColumn(true);


			BookerRow.ToEditor = delegate
			{
				return RowPanel(new Component[]
				{
					Booker.ToField(),
					TextComponent("/"),
					BookerOffice.ToField(80, HideLabel),
					TextComponent("-"),
					BookerCode.ToField(65, HideLabel),
				});
			};


			TicketerRow.ToEditor = delegate
			{
				return RowPanel(new Component[]
				{
					Ticketer.ToField(),
					TextComponent("/"),
					TicketerOffice.ToField(80, HideLabel),
					TextComponent("-"),
					TicketerCode.ToField(65, HideLabel),
				});
			};


			PnrCode.SetEditor(-2);
			TourCode.SetEditor(-2);


			PnrAndTourCodes.ToEditor = delegate
			{
				return RowPanel(new Component[]
				{
					PnrCode.ToField(71, delegate(FormMember m)
					{
						m.Label(string.Format("{0} / {1}", DomainRes.AviaDocument_PnrCode, DomainRes.AviaDocument_TourCode));
					}),

					TextComponent("/"),

					TourCode.ToField(72, HideLabel)
				});
			};


			EqualFare.SetColumn(false, 85, null, delegate(ColumnConfig cfg) { cfg.header(DomainRes.Common_Fare); });

			FeesTotal.SetColumn(false, 85);
			Vat.SetColumn(false, 85);
			Total.SetColumn(false, 85);
			ServiceFee.SetColumn(true);

			Handling.SetColumn(true);
			CommissionDiscount.SetColumn(true);

			Discount.SetColumn(true);
			GrandTotal.SetColumn(false, 95);
			Order.SetColumn(false, 80);
			Commission.SetColumn(true);
			Seller.SetColumn(true, 110);
			Owner.SetColumn(true, 110);
			LegalEntity.SetEditor(-3);
			IsProcessed.SetColumn(true, 70);
			IsVoid.SetColumn(true, 70);
			RequiresProcessing.SetColumn(true, 70);
			IsPaid.SetColumn(true, 70);


			Note
				.SetColumn(true, 100)
				.SetEditor(645, delegate(FormMember m)
				{
					m.Height(48);
					m.ItemCls("commentField");
				})
				;

			CreatedOn.SetColumn(true);
			CreatedBy.SetColumn(true);
			ModifiedOn.SetColumn(true);
			ModifiedBy.SetColumn(true);

		}



		//---g




		#region Passengers

		public static void SetOnePassengerEditorsAndColumns(SemanticMember passengerName, SemanticMember passenger, SemanticMember passengerRow)
		{
			passengerName.SetColumn(false, 165, OnePassengerNameRenderer);

			passengerName.SetEditor(-1, delegate(FormMember m)
			{
				m.Label(DomainRes.AviaDocument_Passenger);
				m.BoldLabel();
			});

			passenger.SetEditor(-1, delegate(FormMember m)
			{
				m.DataProxy(GenericService.SuggestProxy("Person"));
				m.HideLabel();
			});

			passengerRow.ToEditor = delegate
			{
				return RowPanel_(new Component[]
				{
					passengerName.ToEditor(),
					TextComponent_("/"),
					passenger.ToEditor(),
				});
			};
		}

		public static void SetManyPassengerEditorsAndColumns(SemanticMember passengerName, SemanticMember passengerRow)
		{
			passengerName.SetColumn(false, 165, ManyPassengerNameRenderer);

			IEditForm form = passengerRow.EditForm;

			passengerRow.ToEditor = delegate
			{
				ProductPassengerGridControl grid = new ProductPassengerGridControl(16 + 2 * form.FieldMaxWidth, 160);
				grid.fieldLabel = "<br><br><br><b>" + DomainRes.Common_Passenger + "</b>";

				FormMember member = new FormMember(form, passengerRow, null);
				member.OnLoadValue += delegate
				{
					ProductPassengerDto[] passengers = (ProductPassengerDto[])form.GetValue("Passengers");
					grid.SetInitialData(form, passengers);
				};

				member.OnSaveValue += delegate
				{
					ProductPassengerDto[] passengers = grid.Items;
					form.SetValue("Passengers", passengers);
				};

				member.OnIsModified += delegate { return grid.IsModified; };

				form.Members.Add(member);

				return grid;
			};
		}

		public static string GetPassengerValueHtml(Reference passenger, string passengerName)
		{
			return
				!Script.IsValue(passenger) ? passengerName :
				!Script.IsValue(passengerName) ? ObjectLink.RenderInfo(passenger) :
				string.Format("{0} / {1}", passengerName, ObjectLink.RenderInfo(passenger));
		}

		public static object OnePassengerNameRenderer(object passengerName, object metadata, Record record, int rowIndex, int colIndex, Store store)
		{
			object passenger = record.get("Passenger");

			if (passenger == null)
				return passengerName;

			object[] arr = passenger as object[];

			if (arr != null)
				return ObjectLink.Render(arr[Reference.IdPos], passengerName, (string)arr[Reference.TypePos]);

			return ObjectLink.RenderInfo((Reference)passenger);
		}

		public static object OnePassengerNameRenderer2(object passengerName, object metadata, Record record, int rowIndex, int colIndex, Store store)
		{
			Reference passenger = (Reference)record.get("Passenger");

			return GetPassengerValueHtml(passenger, (string)passengerName);
		}

		public static object ManyPassengerNameRenderer(object passengerName, object metadata, Record record, int rowIndex, int colIndex, Store store)
		{
			ProductPassengerDto[] passengers = record.get("PassengerDtos") as ProductPassengerDto[];

			if (passengers == null)
				return passengerName;

			string s = "";

			foreach (ProductPassengerDto passenger in passengers)
			{
				if (s != "")
					s += ", ";
				s += GetPassengerValueHtml(passenger.Passenger, passenger.PassengerName);
			}

			return s;
		}

		#endregion



		//---g



		public GridRenderDelegate GetNumberRenderer()
		{

			return delegate(object value, object metadata, Record record, int index, int colIndex, Store store)
			{
				if (Script.IsNullOrUndefined(value))
					return string.Format("<div style='text-align: center'>{0}</div>",
						ControlFactory
							.CreateRefrenceRenderer(_className)
							.Invoke(Res.AviaDocument_NoNumber, metadata, record, index, colIndex, store)
					);

				return ControlFactory
					.CreateRefrenceRenderer(_className)
					.Invoke(value, metadata, record, index, colIndex, store);
			};

		}



//		public static void SetProducerEditorsAndColumns(ViewMember provider, string className)
//		{
//			provider.LocalReference(delegate(AjaxCallback success)
//			{
//				ProductService.GetProducers(className, success, null);
//			});
//		}
//
//		public static void SetProviderEditorsAndColumns(ViewMember provider, string className)
//		{
//			provider.LocalReference(delegate(AjaxCallback success)
//			{
//				ProductService.GetProviders(className, success, null);
//			});
//		}



		//---g

	}






	//===g






	public class Product_CustomerAndOrderEditor
	{

		//---g



		public Product_CustomerAndOrderEditor(ProductSemantic v)
		{
			this.v = v;
		}



		//---g



		protected ProductSemantic v;
		private Field _customerField;
		private Field _orderField;



		//---g



		public Component ToEditor()
		{

			return v.RowPanel(new Component[]
			{
				_customerField = v.Customer.ToField(-1, delegate(FormMember m)
				{
					m.DataProxy(PartyService.SuggestCustomersProxy());
					m.Label(string.Format("{0} / {1}", v.Customer._title, v.Order._title));
					m.OnChangeValue(OnCustomerChanged);
				}),

				v.TextComponent("/"),

				_orderField = v.Order.ToField(-1, delegate(FormMember m)
				{
					m.ValueProperties(new string[] { "Customer" });
					m.HideLabel();
					m.OnChangeValue(OnOrderChanged);
				})
			});

		}



		private void OnOrderChanged(Field objthis, object newvalue, object oldvalue)
		{

			if (v.EditForm.Updating || newvalue == null)
				return;


			Array val = (Array)newvalue;

			Record record = ((Store)((ComboBox)objthis).store).getById((string)val[Reference.IdPos]);

			object[] customer = (object[])record.get("Customer");

			v.EditForm.Updating = true;
			_customerField.setValue(customer);
			v.EditForm.Updating = false;

		}



		private void OnCustomerChanged(Field objthis, object newvalue, object oldvalue)
		{

			if (v.EditForm.Updating || newvalue == oldvalue || _orderField.getValue() == null)
				return;


			Array oldArray = (Array)oldvalue;
			Array newArray = (Array)newvalue;

			if (oldArray != null && newArray != null && oldArray[0] == newArray[0])
				return;


			MessageBoxWrap.Confirm(Res.Confirmation, Res.AviaDocumentProcessForm_CustomerChanged, delegate(string button, string text)
			{
				v.EditForm.Updating = true;

				if (button == "yes")
					_orderField.setValue(null);
				else
					_customerField.setValue(oldvalue);

				v.EditForm.Updating = false;
			});

		}



		//---g
		
	}






	//===g






	public class Product_FinanceEditor
	{

		//---g



		public Product_FinanceEditor(ProductSemantic v)
		{
			this.v = v;
		}



		//---g



		protected ProductSemantic v;

		private Field _equalFareField;
		private Field _feesTotalField;
		private Field _totalField;
		private Field _vatField;
		private Field _commissionDiscountField;
		private Field _serviceFeeField;
		private Field _handlingField;
		private Field _handlingNField;
		private Field _discountField;
		private Field _bonusDiscountField;
		private Field _grandTotalField;
		private Field _cancelFeeField;
		private Field _refundServiceFeeField;
		private Field _serviceFeePenaltyField;
		private Field _cancelCommissionField;

		private MoneyControlCalculator _totalCalculator;
		private MoneyControlCalculator _grandTotalCalculator;


		
		//---g



		public Component ToEditor()
		{

			// ReSharper disable once SuspiciousTypeConversion.Global
			ProductEditForm f = (ProductEditForm)v.EditForm;

			InitFormMemberAction recalculate = delegate(FormMember m) { m.OnChangeValue(RecalculateTotals); };
			InitFormMemberAction recalculateBold = delegate(FormMember m) { m.OnChangeValue(RecalculateTotals); m.BoldLabel(); };


			ArrayList totalItems = new ArrayList();

			totalItems.Add(new Item(new MenuItemConfig()
				.text(Res.AviaDocument_CalculateFare)
				.handler(new AnonymousDelegate(delegate
				{
					Recalculate(_equalFareField);
					_vatField.focus();
				}))
				.ToDictionary()
			));


			totalItems.Add(new Item(new MenuItemConfig()
				.text(Res.AviaDocument_CalculateFeesTotal)
				.handler(new AnonymousDelegate(delegate
				{
					Recalculate(_feesTotalField);
					_vatField.focus();
				}))
				.ToDictionary()
			));


			if (f.IsRefund)
			{
				totalItems.Add(new Item(new MenuItemConfig()
					.text(Res.AviaDocument_CalculateCancelFee)
					.handler(new AnonymousDelegate(delegate
					{
						Recalculate(_cancelFeeField);
						_vatField.focus();
					}))
					.ToDictionary()
				));
			}


			ArrayList grandTotalItems = new ArrayList();

			grandTotalItems.Add(new Item(new MenuItemConfig()
				.text(Res.CalculateServiceFee_Text)
				.handler(GetRecalculateGrandTotalHandler(delegate { return _serviceFeeField; }))
				.ToDictionary()
			));


			if (f.UseHandling)
			{
				grandTotalItems.Add(new Item(new MenuItemConfig()
					.text(Res.CalculateHandling_Text)
					.handler(GetRecalculateGrandTotalHandler(delegate { return _handlingField; }))
					.ToDictionary()
				));

				grandTotalItems.Add(new Item(new MenuItemConfig()
					.text(Res.CalculateCommissionDiscount_Text)
					.handler(GetRecalculateGrandTotalHandler(delegate { return _commissionDiscountField; }))
					.ToDictionary()
				));
			}


			grandTotalItems.Add(new Item(new MenuItemConfig()
				.text(Res.CalculateDiscount_Text)
				.handler(GetRecalculateGrandTotalHandler(delegate { return _discountField; }))
				.ToDictionary()
			));


			if (f.IsRefund)
			{
				grandTotalItems.Add(new Item(new MenuItemConfig()
					.text(Res.CalculateRefundServiceFee_Text)
					.handler(GetRecalculateGrandTotalHandler(delegate { return _refundServiceFeeField; }))
					.ToDictionary()
				));

				grandTotalItems.Add(new Item(new MenuItemConfig()
					.text(Res.CalculateServiceFeePenalty_Text)
					.handler(GetRecalculateGrandTotalHandler(delegate { return _serviceFeePenaltyField; }))
					.ToDictionary()
				));
			}
			

			ArrayList list = new ArrayList();

			list.AddRange(new Component[]
			{
				v.Fare.ToEditor(),
				_equalFareField = v.EqualFare.ToField(0, recalculateBold),
				_feesTotalField = v.FeesTotal.ToField(0, recalculate)
			});


			if (f.IsRefund)
				list.Add(_cancelFeeField = v.CancelFee.ToField(0, recalculate));


			list.AddRange(new Component[]
			{
				_totalField = v.Total.ToField(0, delegate(FormMember m)
				{
					m.MenuOnChange(totalItems);
					m.BoldLabel();
				}),
				_vatField = v.Vat.ToField(),
				v.Commission.ToField(0, delegate(FormMember m) { m.BoldLabel(); })
			});


			if (f.UseHandling)
				list.Add(_commissionDiscountField = v.CommissionDiscount.ToField(0, recalculate));


			list.Add(_serviceFeeField = v.ServiceFee.ToField(0, recalculateBold));


			if (f.UseHandling)
			{
				list.Add(_handlingField = v.Handling.ToField(0, recalculate));
				list.Add(_handlingNField = v.HandlingN.ToField(0, recalculate));
			}


			list.Add(_discountField = v.Discount.ToField(0, recalculate));


			if (f.IsRefund)
			{
				list.AddRange(new Component[]
				{
					_refundServiceFeeField = v.RefundServiceFee.ToField(0, recalculate),
					_serviceFeePenaltyField = v.ServiceFeePenalty.ToField(0, recalculate)
				});
			}

			else
			{
				list.AddRange(new Component[]
				{
					_bonusDiscountField = v.BonusDiscount.ToField(0, recalculate),
					v.BonusAccumulation.ToField(0),
				});
			}


			list.Add(_cancelCommissionField = v.CancelCommission.ToField(0, recalculate));
			
			list.Add(_grandTotalField = v.GrandTotal.ToField(0, delegate(FormMember m) { m.MenuOnChange(grandTotalItems); m.BoldLabel(); }));

			list.Add(v.PaymentType.ToField(145));

			list.Add(v.TaxRateOfProduct.ToField(145));
			list.Add(v.TaxRateOfServiceFee.ToField(145));


			CreateCalculators();


			return new Panel(new FormPanelConfig()
				.items(list)
				.layout("form")
				.cls("finance-data")
				.labelWidth(160)
				.ToDictionary()
			);
			
		}



		private void RecalculateTotals(Field decimalField, object newValue, object oldValue)
		{
			_totalCalculator.Recalculate(_totalField);
			_grandTotalCalculator.Recalculate(_grandTotalField);
		}



		private AnonymousDelegate GetRecalculateGrandTotalHandler(Func<Field> control)
		{
			return delegate
			{
				Recalculate(control());
				_grandTotalField.focus();
			};
		}



		private void Recalculate(Field field)
		{

			MoneyControl control = (MoneyControl)field;

			if (_totalCalculator.Has(control))
			{
				_totalCalculator.Recalculate(control);
				_grandTotalCalculator.Recalculate(_grandTotalField);
			}
			else if (_grandTotalCalculator.Has(control))
			{
				_grandTotalCalculator.Recalculate(control);
			}

		}

		private void CreateCalculators()
		{
			_totalCalculator = new MoneyControlCalculator()
				.Add(_equalFareField, 1)
				.Add(_feesTotalField, 1)
				.Add(_cancelFeeField, -1)
				.Add(_totalField, -1);

			_grandTotalCalculator = new MoneyControlCalculator()
				.Add(_totalField, 1)
				.Add(_serviceFeeField, 1)
				.Add(_handlingField, 1)
				.Add(_handlingNField, -1)
				.Add(_commissionDiscountField, -1)
				.Add(_discountField, -1)
				.Add(_bonusDiscountField, -1)
				.Add(_refundServiceFeeField, -1)
				.Add(_serviceFeePenaltyField, -1)
				.Add(_grandTotalField, -1);
		}



		//---g

	}






	//===g



}
