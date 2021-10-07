using System;
using System.Collections;

using Ext;
using Ext.data;
using Ext.grid;

using LxnBase;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Luxena.Travel.Controls;

using LxnBase.Data;

using Record = Ext.data.Record;


namespace Luxena.Travel
{

	public class ProductPassengerGridControl : EditorGridPanel
	{

		public ProductPassengerGridControl(int width, int height)
			: base(new EditorGridPanelConfig()
				.cls("invoce-records")
				.width(width)
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
		
		public bool IsModified { get { return _isModified; } }

		public ProductPassengerDto[] Items
		{
			get
			{
				ProductPassengerDto[] result = new ProductPassengerDto[0];

				for (int i = 0; i < store.data.Length; i++)
					result[i] = (ProductPassengerDto)store.getAt(i).data;

				return result;
			}
			set
			{
				foreach (ProductPassengerDto dto in value)
				{
					Record record = new Record(dto);
					_store.add(new Record[] { record });
				}
			}
		}


		public void SetInitialData(IEditForm form, ProductPassengerDto[] passengers)
		{
			if (passengers == null || passengers.Length <= 0) return;

			if (form.Args.IsCopy || form.Args.IsNew)
			{
				foreach (ProductPassengerDto passenger in passengers)
				{
					passenger.Id = null;
					passenger.Version = 0;
				}
			}
				
			_store.loadData(passengers);
		}

		private Toolbar InitToolbar()
		{
			LinkButton addButton = new LinkButton(new ButtonConfig()
				.text(Res.Add.ToLowerCase())
				.handler(new AnonymousDelegate(CreateItem)));

			_editButton = new LinkButton(new ButtonConfig()
				.text(BaseRes.Edit_Lower)
				.handler(new AnonymousDelegate(EditItem))
				.disabled(true));

			_removeButton = new LinkButton(new ButtonConfig()
				.text(BaseRes.Remove_Lower)
				.handler(new AnonymousDelegate(RemoveItem))
				.disabled(true));

			return new Toolbar(new object[] { "->", addButton, _editButton, _removeButton, });
		}

		private Store InitStore()
		{
			_store = new JsonStore(new JsonStoreConfig()
				.fields(new string[] { "Id", "PassengerName", "Passenger" })
				.ToDictionary());

			return _store;
		}

		private AbstractSelectionModel InitSelectionModel()
		{
			_selectionModel = new CheckboxSelectionModel();
			_selectionModel.singleSelect = false;
			_selectionModel.on("selectionchange", new SelectionChangedDelegate(delegate
			{
				_removeButton.setDisabled(_selectionModel.getCount() == 0);
				_editButton.setDisabled(_selectionModel.getCount() != 1);
			}));

			return _selectionModel;
		}

		private ColumnModel InitColumnModel()
		{
			ArrayList columnModelConfig = new ArrayList();

			if (!_selectionModel.singleSelect)
				columnModelConfig.Insert(0, _selectionModel);

			columnModelConfig.Add(new RowNumberer(new Dictionary(
				"header", Res.Common_ListNumber
			)));

			columnModelConfig.Add(new Dictionary(
				"id", "PassengerName",
				"header", DomainRes.Common_PassengerName,
				"sortable", false,
				"dataIndex", "PassengerName",
				"width", 270,
				"renderer", new GridRenderDelegate(ProductSemantic.OnePassengerNameRenderer2)
			));

			return new ColumnModel(columnModelConfig);
		}

		private void CreateItem()
		{
			FormsRegistry.EditObject(
				ClassNames.ProductPassenger, 
				null, null,
				delegate(object result)
				{
					AddItem((ProductPassengerDto)((ItemResponse)result).Item);
				}, 
				null, null, LoadMode.Local
			);
		}

		private void EditItem()
		{
			if (_selectionModel.getCount() == 0) return;

			Record record = _selectionModel.getSelected();

			ProductPassengerDto data_ = (ProductPassengerDto)record.data;

			FormsRegistry.EditObject(
				ClassNames.ProductPassenger, 
				data_.Id, Dictionary.GetDictionary(data_),
				delegate(object result) { UpdateItem(((ItemResponse)result).Item, record); }, 
				null, null, LoadMode.Local
			);
		}

		private void UpdateItem(object obj, Record record)
		{
			record.data = obj;

			record.commit();

			_isModified = true;
		}

		private void RemoveItem()
		{
			if (_selectionModel.getCount() == 0)
				return;

			Record[] selected = (Record[])_selectionModel.getSelections();

			if (selected == null)
				return;

			foreach (Record r in selected) 
				_store.remove(r);

			_isModified = true;
		}

		private void AddItem(ProductPassengerDto passenger)
		{
			if (passenger == null) return;

			if (_store.findBy(new StoreSearchDelegate(delegate(Record rec, object recordId)
			{
				return StoreSearchFunc((ProductPassengerDto)rec.data, passenger);
			})) != -1)
				return;

			_store.add(new Record[] { new Record(passenger) });

			_isModified = true;
		}

		private static bool StoreSearchFunc(ProductPassengerDto oldItem, ProductPassengerDto newItem)
		{
			return
				!Script.IsNullOrUndefined(oldItem) && !Script.IsNullOrUndefined(newItem) &&
				oldItem.PassengerName == newItem.PassengerName &&
				(
					!Script.IsNullOrUndefined(oldItem.Passenger) && !Script.IsNullOrUndefined(newItem.Passenger) ||
					oldItem.Passenger.Id == newItem.Passenger.Id
				);
		}

		private bool _isModified;
		private JsonStore _store;
		private CheckboxSelectionModel _selectionModel;
		private LinkButton _editButton;
		private LinkButton _removeButton;
	}
}