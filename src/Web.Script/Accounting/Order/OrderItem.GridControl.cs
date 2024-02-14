using System;
using System.Collections;

using Ext;
using Ext.data;
using Ext.grid;
using Ext.menu;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using ItemConfig = Ext.menu.ItemConfig;
using Menu = Ext.menu.Menu;
using Record = Ext.data.Record;


namespace Luxena.Travel
{
	public class OrderItemGridControl : EditorGridPanel
	{
		public event AnonymousDelegate FinanceDataChanged;

		public OrderItemGridControl(double height)
			: base(new EditorGridPanelConfig()
				.cls("invoce-records")
				.height(height)
				.stripeRows(true)
				.enableHdMenu(false)
				.enableColumnResize(true)
				.enableColumnMove(false)
				.autoScroll(true)
				.collapsible(false)
				.frame(false)
				.ToDictionary())
		{
		}

		protected override void initComponent()
		{
			store = InitStore();
			sm = InitSelectionModel();
			cm = InitColumnModel();
			tbar = InitToolbar();

			base.initComponent();
		}

		public MoneyDto Discount
		{
			get
			{
				MoneyDto result = new MoneyDto();
				result.Currency = GrandTotal.Currency;
				result.Amount = 0;

				for (int i = 0; i < getStore().data.Length; i++)
				{
					OrderItemDto dto = (OrderItemDto)getStore().getAt(i).data;

					if (!Script.IsNullOrUndefined(dto.Discount) && Reference.Equals(result.Currency, dto.Discount.Currency))
						result.Amount += dto.Discount.Amount;
				}
				return result;
			}
		}

		public MoneyDto GrandTotal
		{
			get
			{
				MoneyDto result = new MoneyDto();
				result.Currency = AppManager.SystemConfiguration.DefaultCurrency;

				for (int i = 0; i < getStore().data.Length; i++)
				{
					OrderItemDto dto = (OrderItemDto)getStore().getAt(i).data;

					if (!Script.IsNullOrUndefined(dto.GrandTotal) && Reference.Equals(result.Currency, dto.GrandTotal.Currency))
						result.Amount += dto.GrandTotal.Amount;
				}
				return result;
			}
		}

		public MoneyDto Vat
		{
			get
			{
				MoneyDto givenVat = MoneyDto.GetZeroMoney(GrandTotal.Currency);
				MoneyDto taxedTotal = MoneyDto.GetZeroMoney(GrandTotal.Currency);

				decimal serviceFee = 0;

				for (int i = 0; i < getStore().data.Length; i++)
				{
					OrderItemDto dto = (OrderItemDto)getStore().getAt(i).data;

					if (Script.IsNullOrUndefined(dto.HasVat) || !dto.HasVat) continue;

					if (dto.GivenVat != null && Reference.Equals(givenVat.Currency, dto.GivenVat.Currency))
						givenVat.Amount += dto.GivenVat.Amount;

					if (dto.TaxedTotal != null && Reference.Equals(taxedTotal.Currency, dto.TaxedTotal.Currency))
						taxedTotal.Amount += dto.TaxedTotal.Amount;

					if (!UseServiceFeeOnlyInVat) continue;

					if (dto.LinkType == OrderItemLinkType.ServiceFee && !Script.IsNullOrUndefined(dto.GrandTotal))
						serviceFee += dto.GrandTotal.Amount;
				}

				MoneyDto vat = MoneyDto.GetZeroMoney(GrandTotal.Currency);

				MoneyDto discount = Discount ?? MoneyDto.GetZeroMoney(GrandTotal.Currency);

				if (givenVat.Amount != 0)
					vat.Amount += givenVat.Amount;

				if (taxedTotal.Amount != 0)
					vat.Amount += (taxedTotal.Amount - discount.Amount) * AppManager.SystemConfiguration.VatRate / (100 + AppManager.SystemConfiguration.VatRate);

				if (vat != null && vat.Amount < 0)
					vat.Amount = 0;

				if (UseServiceFeeOnlyInVat && vat != null)
					vat.Amount = serviceFee * AppManager.SystemConfiguration.VatRate / 100;

				return vat;
			}
		}

		public bool UseServiceFeeOnlyInVat
		{
			get { return _useServiceFeeOnlyInVat; }
			set
			{
				if (_useServiceFeeOnlyInVat == value) return;

				_useServiceFeeOnlyInVat = value;

				if (FinanceDataChanged != null)
					FinanceDataChanged();
			}
		}

		public bool IsModified
		{
			get { return _isModified; }
		}

		public OrderItemDto[] Items
		{
			get
			{
				OrderItemDto[] result = new OrderItemDto[0];

				for (int i = 0; i < store.data.Length; i++)
					result[i] = (OrderItemDto)store.getAt(i).data;

				return result;
			}
			set
			{
				foreach (OrderItemDto dto in value)
				{
					Record record = new Record(dto);
					_store.add(new Record[] { record });
				}
				if (FinanceDataChanged != null)
					FinanceDataChanged();
			}
		}

		public void SetParentForm(OrderEditForm form)
		{
			_parent = form;
		}

		public void SetInitialData(string orderId, OrderItemDto[] orderItems)
		{
			_orderId = orderId;

			if (orderItems != null && orderItems.Length > 0)
				_store.loadData(orderItems);
		}

		public void TryAddDocuments(object[] aviaDocumentIds)
		{
			OrderService.GenerateOrderItems(
				aviaDocumentIds, 
				_parent.SeparateServiceFee, 
				_orderId,
				_parent.BankAccountId,
				delegate(object result)
				{
					GenerateOrderItemsResponse response = (GenerateOrderItemsResponse)result;

					if (response.OrderItems == null || response.OrderItems.Length == 0)
						UpdateOrder(response.Items, response.Customer);
					else
						ConfirmAddingDocuments(response);
				}, null
			);
		}

		private Toolbar InitToolbar()
		{
			_editButton = new LinkButton(new ButtonConfig()
				.text(BaseRes.Edit_Lower)
				.handler(new AnonymousDelegate(EditItem))
				.disabled(true));

			_removeButton = new LinkButton(new ButtonConfig()
				.text(BaseRes.Remove_Lower)
				.handler(new AnonymousDelegate(RemoveItem))
				.disabled(true));

			Item addWithSamecustomer = new Item(new ItemConfig()
				.handler(new AnonymousDelegate(AddDocumentsWithSameCustomer))
				.text(Res.Menu_WithSameAcquirer)
				.ToDictionary());

			Menu menu = new Menu(new MenuConfig()
				.items(new object[] {
					new Item(new ItemConfig()
						.text(Res.Menu_AddNew)
						.handler(new AnonymousDelegate(CreateItem))
						.ToDictionary()),
					"-",
					new Item(new ItemConfig()
						.text(Res.Menu_GetByNumber)
						.handler(new AnonymousDelegate(
							delegate
							{
								MessageBoxWrap.Prompt(Res.AddDocument_Title, Res.EnterDocumentNumber_Text,
									delegate(string button, string text)
									{
										if (button == "ok" && !string.IsNullOrEmpty(text))
											AddDocumentByNumber(text);
									});
							}))
						.ToDictionary()),
					new Item(new ItemConfig()
						.text(Res.Menu_MyDocuments)
						.handler(new AnonymousDelegate(AddUserDocuments))
						.ToDictionary()),
					new Item(new ItemConfig()
						.handler(new AnonymousDelegate(AddDocumentsFromFullList))
						.text(Res.Menu_FromList)
						.ToDictionary()),
					addWithSamecustomer
				})
				.listeners(new Dictionary(
					"beforeshow", new MenuBeforeshowDelegate(
						delegate
						{
							if (_parent.Customer == null)
								addWithSamecustomer.setDisabled(true);
							else
								addWithSamecustomer.setDisabled(false);

						})))
				.cls("simple-menu")
				.ToDictionary());

			LinkButton addButton = new LinkButton(new ButtonConfig()
				.text(Res.Add.ToLowerCase())
				.menu(menu));

			_changeVatButton = new LinkButton(new ButtonConfig()
				.text(Res.OrderItem_WithVat)
				.handler(new AnonymousDelegate(ChangeVat))
				.disabled(true));

			return new Toolbar(new object[] { "->", addButton, _editButton, _removeButton, "|", _changeVatButton });
		}

		private Store InitStore()
		{

			_store = new JsonStore(new JsonStoreConfig()
				.fields(new string[]
				{
					"Id", "Version", "LinkType", "Product", "Text", "ProductText", "Price", "Quantity", "Total",
					"Discount", "GrandTotal", "GivenVat", "TaxedTotal", "HasVat", "IsForceDelivered", "Consignment"
				})
				.ToDictionary()
			);

			return _store;

		}

		private AbstractSelectionModel InitSelectionModel()
		{
			_selectionModel = new CheckboxSelectionModel();
			_selectionModel.singleSelect = false;
			_selectionModel.on("selectionchange", new SelectionChangedDelegate(delegate
			{
				if (_selectionModel.getCount() == 0)
				{
					_removeButton.disable();
					_changeVatButton.disable();
				}
				else
				{
					_removeButton.enable();

					Record[] records = (Record[])_selectionModel.getSelections();

					bool hasVat = (bool)records[0].get("HasVat");
					bool canChangeVat = _parent.CanChangeVat;

					for (int i = 0; i < records.Length; i++)
						if (hasVat ^ (bool)records[i].get("HasVat"))
						{
							canChangeVat = false;
							break;
						}

					if (canChangeVat)
					{
						_changeVatButton.setText(hasVat ? Res.OrderItem_WithoutVat : Res.OrderItem_WithVat);
						_changeVatButton.enable();
					}
					else
					{
						_changeVatButton.disable();
						_changeVatButton.setText(Res.OrderItem_WithVat);
					}
				}

				if (_selectionModel.getCount() != 1)
					_editButton.disable();
				else
					_editButton.enable();
			}));

			return _selectionModel;
		}

		private ColumnModel InitColumnModel()
		{
			ArrayList columnModelConfig = new ArrayList();

			if (!_selectionModel.singleSelect)
				columnModelConfig.Insert(0, _selectionModel);

			columnModelConfig.Add(new RowNumberer(new Dictionary(
				"header", Res.Common_ListNumber)));

			columnModelConfig.Add(new Dictionary(
				"id", "Text",
				"header", Res.OrderItem_Text,
				"sortable", false,
				"dataIndex", "Text",
				"width", 290,
				"renderer", new RenderDelegate(delegate(object value) { return string.Format("<pre>{0}</pre>", value); })
			));

			columnModelConfig.Add(new Dictionary(
				"id", "Quantity",
				"header", Res.Common_QuantityShort,
				"sortable", false,
				"dataIndex", "Quantity",
				"width", 35
			));

			columnModelConfig.Add(new Dictionary(
				"id", "Price",
				"header", Res.Common_Price,
				"sortable", false,
				"dataIndex", "Price",
				"width", 75,
				"renderer", ControlFactoryExt.GetMoneyRenderer()
			));

			columnModelConfig.Add(new Dictionary(
				"id", "Total",
				"header", DomainRes.Common_Amount,
				"sortable", false,
				"dataIndex", "Total",
				"width", 85,
				"renderer", ControlFactoryExt.GetMoneyRenderer()
			));

			columnModelConfig.Add(new Dictionary(
				"id", "HasVat",
				"header", DomainRes.Common_Vat,
				"sortable", false,
				"dataIndex", "HasVat",
				"width", 85,
				"renderer", ControlFactory.CreateBooleanRenderer()
			));

			return new ColumnModel(columnModelConfig);
		}

		private void CreateItem()
		{
			FormsRegistry.EditObject(ClassNames.OrderItem, null, null,
				delegate(object result)
				{
//					Log.Add(result);
					AddOrderItems(new OrderItemDto[] { (OrderItemDto)((ItemResponse)result).Item });
				},
				null, null, LoadMode.Local
			);
		}

		private void EditItem()
		{
			if (_selectionModel.getCount() == 0)
				return;

			Record record = _selectionModel.getSelected();

			OrderItemDto data = (OrderItemDto)record.data;

			FormsRegistry.EditObject(ClassNames.OrderItem, data.Id, Dictionary.GetDictionary(data),
				delegate(object result)
				{
					UpdateRecord(((ItemResponse)result).Item, record);

					if (FinanceDataChanged != null)
						FinanceDataChanged();

				}, null, null, LoadMode.Local
			);
		}

		private void UpdateRecord(object obj, Record record)
		{
			record.data = obj;

			record.commit();

			_isModified = true;
		}

		private void AddDocumentByNumber(string number)
		{
			OrderService.FindAviaDocumentByNumer(number,
				delegate(object result)
				{
					Reference info = (Reference)result;

					if (result != null)
						TryAddDocuments(new object[] { (string)info.Id });
					else
						MessageBoxWrap.Show(string.Empty, string.Format(Res.NoDocumentFound_Text, number), MessageBox.WARNING,
							MessageBox.OK);
				}, null
			);
		}

		private void AddDocumentsFromFullList()
		{
			RangeRequest request = new RangeRequest();
			request.Filters = new PropertyFilter[]
			{
				PropertyFilterExtention.CreateFilter("IsVoid", FilterOperator.Equals, false, false),
				PropertyFilterExtention.CreateFilter("Order", FilterOperator.IsNull, true, false)
			};

			request.Sort = "IssueDate";
			request.Dir = "DESC";

			FormsRegistry.SelectObjects(ClassNames.AviaDocument, request, false,
				delegate(object result)
				{
					TryAddDocuments((object[])result);
				}, null
			);
		}

		private void AddUserDocuments()
		{
			RangeRequest request = new RangeRequest();
			request.Sort = "IssueDate";
			request.Dir = "DESC";
			request.Filters = new PropertyFilter[]
			{
				PropertyFilterExtention.CreateFilter("Seller", FilterOperator.Equals, AppManager.CurrentPerson.Name, false),
				PropertyFilterExtention.CreateFilter("IsVoid", FilterOperator.Equals, false, false),
				PropertyFilterExtention.CreateFilter("Order", FilterOperator.IsNull, true, false)
			};

			FormsRegistry.SelectObjects(ClassNames.AviaDocument, request, false,
				delegate(object result)
				{
					TryAddDocuments((object[])result);
				}, null
			);
		}

		private void AddDocumentsWithSameCustomer()
		{
			Reference customer = _parent.Customer;

			if (customer == null)
				return;

			RangeRequest request = new RangeRequest();
			request.Sort = "IssueDate";
			request.Dir = "DESC";

			request.Filters = new PropertyFilter[]
			{
				PropertyFilterExtention.CreateFilter("Customer", FilterOperator.Equals, customer.Name, false),
				PropertyFilterExtention.CreateFilter("IsVoid", FilterOperator.Equals, false, false),
				PropertyFilterExtention.CreateFilter("Order", FilterOperator.IsNull, true, false)
			};

			FormsRegistry.SelectObjects(ClassNames.AviaDocument, request, false,
				delegate(object result)
				{
					TryAddDocuments((object[])result);
				}, null);
		}

		private void RemoveItem()
		{
			if (_selectionModel.getCount() == 0)
				return;

			Record[] selected = (Record[])_selectionModel.getSelections();

			if (selected == null)
				return;

			for (int i = 0; i < selected.Length; i++)
			{
				_store.remove(selected[i]);
			}

			_isModified = true;

			if (FinanceDataChanged != null)
				FinanceDataChanged();
		}

		private void ChangeVat()
		{
			Record[] selected = (Record[])_selectionModel.getSelections();

			if (selected == null || selected.Length == 0)
				return;

			bool hasVat = (bool)selected[0].get("HasVat");

			for (int i = 0; i < selected.Length; i++)
			{
				selected[i].set("HasVat", !hasVat);
				selected[i].commit();
			}

			_changeVatButton.setText(hasVat ? Res.OrderItem_WithVat : Res.OrderItem_WithoutVat);
			_isModified = true;

			if (FinanceDataChanged != null)
				FinanceDataChanged();
		}

		private void UpdateOrder(OrderItemDto[] orderItems, Reference customer)
		{
			if (_parent.Customer == null && customer != null)
				_parent.Customer = customer;

			AddOrderItems(orderItems);
		}

		private void AddOrderItems(OrderItemDto[] orderItems)
		{
			if (orderItems == null || orderItems.Length == 0)
				return;

			bool add = false;

			for (int i = 0; i < orderItems.Length; i++)
			{
				int pos = i;

				if (_store.findBy(new StoreSearchDelegate(
					delegate(Record rec, object recordId) { return StoreSearchFunc((OrderItemDto)rec.data, orderItems[pos]); })) != -1)
					continue;

				Record record = new Record(orderItems[i]);
				_store.add(new Record[] { record });

				add = true;
			}

			if (add)
			{
				_isModified = true;

				if (FinanceDataChanged != null)
					FinanceDataChanged();
			}
		}

		private static bool StoreSearchFunc(OrderItemDto oldItem, OrderItemDto newItem)
		{
			return
				!Script.IsNullOrUndefined(oldItem) && !Script.IsNullOrUndefined(newItem) &&
				!Script.IsNullOrUndefined(oldItem.Product) && !Script.IsNullOrUndefined(newItem.Product) &&
				oldItem.Product.Id == newItem.Product.Id &&
				oldItem.LinkType == newItem.LinkType;
		}

		private void ConfirmAddingDocuments(GenerateOrderItemsResponse response)
		{
			if (response.Items.Length > 0)
				MessageFactory.DocumentsAlreadyAddedToOrder(Res.AviaDocument_AddToOrder_Title,
					Res.AviaDocument_CannotAddDocumentsToOrder_Msg,
					response.OrderItems,
					Res.AviaDocument_ContinueAddToOrder_Msg,
					delegate
					{
						UpdateOrder(response.Items, response.Customer);
					});
			else
				MessageFactory.DocumentsAlreadyAddedToOrder(Res.AviaDocument_AddToOrder_Title,
					Res.AviaDocument_CannotAddDocumentsToOrder_Msg, response.OrderItems);
		}

		private OrderEditForm _parent;

		private bool _isModified;
		private JsonStore _store;
		private CheckboxSelectionModel _selectionModel;
		private LinkButton _editButton;
		private LinkButton _removeButton;
		private LinkButton _changeVatButton;
		private string _orderId;
		private bool _useServiceFeeOnlyInVat;
	}
}