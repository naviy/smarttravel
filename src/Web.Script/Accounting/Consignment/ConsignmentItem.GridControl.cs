using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext;
using Ext.data;
using Ext.grid;
using Ext.menu;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using ItemConfig = Ext.menu.ItemConfig;
using Record = Ext.data.Record;


namespace Luxena.Travel
{
	public class ConsignmentItemGridControl : EditorGridPanel
	{
		public event AnonymousDelegate FinanceDataChanged;


		public ConsignmentItemGridControl(double height, bool isEditable)
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
				.custom("_isEditable", isEditable)
				.ToDictionary())

		{
		}

		protected override void initComponent()
		{
			store = InitStore();

			if (_isEditable)
			{
				sm = InitSelectionModel();
				tbar = InitToolbar();
			}

			cm = InitColumnModel();

			base.initComponent();
		}

		public bool IsModified
		{
			get { return _isModified; }
			set { _isModified = value; }
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
		}

		public void SetInitialData(object[] gridItems)
		{
			if (gridItems != null && gridItems.Length > 0)
				_store.loadData(gridItems);
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

					if (!Script.IsNullOrUndefined(dto.GrandTotal))
					{
						if (Reference.Equals(result.Currency, dto.GrandTotal.Currency))
							result.Amount += dto.GrandTotal.Amount;
					}
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

				for (int i = 0; i < getStore().data.Length; i++)
				{
					OrderItemDto dto = (OrderItemDto)getStore().getAt(i).data;

					if (!Script.IsNullOrUndefined(dto.HasVat) && dto.HasVat)
					{
						if (dto.GivenVat != null && Reference.Equals(givenVat.Currency, dto.GivenVat.Currency))
							givenVat.Amount += dto.GivenVat.Amount;

						if (dto.TaxedTotal != null && Reference.Equals(taxedTotal.Currency, dto.TaxedTotal.Currency))
							taxedTotal.Amount += dto.TaxedTotal.Amount;
					}
				}

				MoneyDto vat = MoneyDto.GetZeroMoney(GrandTotal.Currency);

				MoneyDto discount = Discount ?? MoneyDto.GetZeroMoney(GrandTotal.Currency);

				if (givenVat.Amount != 0)
					vat.Amount += givenVat.Amount;

				if (taxedTotal.Amount != 0)
					vat.Amount += (taxedTotal.Amount - discount.Amount) * AppManager.SystemConfiguration.VatRate / (100 + AppManager.SystemConfiguration.VatRate);

				return vat;
			}
		}

		private Toolbar InitToolbar()
		{
			_removeButton = new LinkButton(new ButtonConfig()
				.text(BaseRes.Remove_Lower)
				.handler(new AnonymousDelegate(RemoveItem))
				.disabled(true));

			Menu menu = new Menu(new MenuConfig()
				.cls("simple-menu")
				.items(new object[]
				{
					new Item(new ItemConfig()
						.text(Res.Menu_GetByOrderNumber)
						.handler(new AnonymousDelegate(
							delegate
							{
								MessageBoxWrap.Prompt(Res.AddDocument_Title, Res.EnterDocumentNumber_Text,
									delegate(string button, string text)
									{
										if (button == "ok" && !string.IsNullOrEmpty(text))
											AddByOrderNumber(text);
									});
							}))
						.ToDictionary())
				})
				.ToDictionary());

			LinkButton addButton = new LinkButton(new ButtonConfig()
				.text(Res.Add.ToLowerCase())
				.menu(menu));

			return new Toolbar(new object[] { "->", addButton, _removeButton });
		}



		private Store InitStore()
		{

			_store = new JsonStore(new JsonStoreConfig()
				.fields(new string[] { "Id", "Version", "Text", "Price", "Quantity", "Total", "Discount", "GrandTotal", "GivenVat", "TaxedTotal", "HasVat", "Source", "Order" })
				.ToDictionary());


			return _store;

		}



		private AbstractSelectionModel InitSelectionModel()
		{

			_selectionModel = new CheckboxSelectionModel();
			_selectionModel.singleSelect = false;
			_selectionModel.on("selectionchange", new SelectionChangedDelegate(
					delegate
					{
						if (_selectionModel.getCount() == 0)
							_removeButton.disable();
						else
							_removeButton.enable();
					}
				));
			return _selectionModel;
		}

		private ColumnModel InitColumnModel()
		{
			ArrayList columnModelConfig = new ArrayList();

			if (!Script.IsNullOrUndefined(_selectionModel) && !_selectionModel.singleSelect)
				columnModelConfig.Insert(0, _selectionModel);

			columnModelConfig.Add(new RowNumberer(new Dictionary(
				"header", Res.Common_ListNumber)));

			columnModelConfig.Add(new Dictionary(
				"id", "Text",
				"header", Res.OrderItem_Text,
				"sortable", false,
				"dataIndex", "Text",
				"width", 245,
				"renderer", new RenderDelegate(delegate(object value) { return string.Format("<pre>{0}</pre>", value); })
				));

			columnModelConfig.Add(new Dictionary(
				"id", "GrandTotal",
				"header", DomainRes.Common_Amount,
				"sortable", false,
				"dataIndex", "GrandTotal",
				"width", 85,
				"renderer", ControlFactoryExt.GetMoneyRenderer()
				));

			columnModelConfig.Add(new Dictionary(
				"id", "Order",
				"header", DomainRes.Order,
				"sortable", false,
				"dataIndex", "Order",
				"width", 85,
				"renderer", new RenderDelegate(RenderObjectInfoReference)
				));

			return new ColumnModel(columnModelConfig);
		}
		
		private static object RenderObjectInfoReference(object value)
		{
			if (Script.IsNullOrUndefined(value))
				return string.Empty;

			if (Script.IsNullOrUndefined(((Reference)value).Id))
				return ((Reference)value).Name;

			return ObjectLink.RenderInfo(((Reference)value));
		}

		private void AddByOrderNumber(string number)
		{
			OrderService.GetOrderItemsByNumber(number,
				delegate(object result)
				{
					OrderItemDto[] dtos = (OrderItemDto[])result;

					if (dtos.Length > 0)
					{
						ArrayList list = new ArrayList();
						foreach (OrderItemDto item in dtos)
						{
							if (Script.IsNullOrUndefined(item.Consignment) && !Script.IsNullOrUndefined(item.Product))
								list.Add(item);
						}
						
						if (list.Count == 0)
							MessageBoxWrap.Show(string.Empty, Res.InvoiceItemsInConsignments_Warn, MessageBox.WARNING, MessageBox.OK);
						else
						{
							dtos = new OrderItemDto[list.Count];
							for (int i = 0; i < list.Count; i++)
							{
								dtos[i] = (OrderItemDto)list[i];
							}
							AddOrderItems(dtos);
						}
					}
					else
						MessageBoxWrap.Show(string.Empty, string.Format(Res.NoInvoiceFound_Text, number), MessageBox.WARNING,
							MessageBox.OK);
				}, null);
		}

		private void RemoveItem()
		{
			if (_selectionModel.getCount() == 0)
				return;

			Record[] selected = (Record[])_selectionModel.getSelections();

			if (selected == null)
				return;

			int rowIndex = (int)Dictionary.GetDictionary(_selectionModel)["lastActive"];

			for (int i = 0; i < selected.Length; i++)
			{
				_store.remove(selected[i]);
			}

			if (FinanceDataChanged != null)
				FinanceDataChanged();
		
			SetGridFocus(rowIndex);

			_isModified = true;
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
					delegate(Record rec, object recordId) { return StoreSearchFunc((OrderItemDto) rec.data, orderItems[pos]); })) != -1)
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
			if (!Script.IsNullOrUndefined(oldItem) && !Script.IsNullOrUndefined(newItem) &&
				!Script.IsNullOrUndefined(oldItem.Product) && !Script.IsNullOrUndefined(newItem.Product) &&
				oldItem.Product.Id == newItem.Product.Id &&
				oldItem.LinkType == newItem.LinkType)
				return true;

			return false;
		}

		[AlternateSignature]
		private extern void SetGridFocus();

		private void SetGridFocus(int selectRowIndex)
		{
			if (_store.getCount() == 0)
				return;

			getView().focusRow(0);

			if (!Script.IsNullOrUndefined(selectRowIndex))
			{
				int count = _store.getCount();
				int index = selectRowIndex < count ? selectRowIndex : count - 1;

				_selectionModel.selectRow(index);
			}
		}

		[PreserveName]
		private bool _isEditable;

		private bool _isModified;
		private JsonStore _store;
		private CheckboxSelectionModel _selectionModel;
		private LinkButton _removeButton;
	}
}