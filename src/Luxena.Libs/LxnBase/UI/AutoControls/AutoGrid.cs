using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Serialization;

using Ext;
using Ext.data;
using Ext.form;
using Ext.grid;
using Ext.menu;
using Ext.util;

using jQueryApi;

using LxnBase.Data;
using LxnBase.Net;
using LxnBase.Services;

using Action = Ext.Action;
using ActionConfig = Ext.ActionConfig;
using ColumnConfig = LxnBase.Services.ColumnConfig;
using Element = System.Html.Element;
using Field = Ext.form.Field;
using ItemConfig = Ext.menu.ItemConfig;
using Record = Ext.data.Record;
using WindowClass = Ext.Window;


namespace LxnBase.UI.AutoControls
{
	public class AutoGrid : EditorGridPanel
	{
		public AutoGrid(AutoGridArgs args, EditorGridPanelConfig config)
			: base(config
				.cls("autoList")
				.border(false)
				.bodyBorder(false)
				.frame(true)
				.stripeRows(true)
				.header(false)
				.loadMask(true)
				.autoScroll(true)
				.autoHeight(false)
				.clicksToEdit(2)
				.deferRowRender(false)
				.region("center")
				.custom("_args", args)
				.ToDictionary())
		{
		}

		public event GenericOneArgDelegate OnSelect;

		public event AnonymousDelegate OnCancelSelect;

		public Action CreateAction { get { return _createAction; } }

		public Action EditAction { get { return _editAction; } }

		public Action CopyAction { get { return _copyAction; } }

		public Action DeleteAction { get { return _deleteAction; } }

		public Action ExportAction { get { return _exportAction; } }


		protected override void initComponent()
		{
			_baseRequest = _args.BaseRequest ?? new RangeRequest();

			_baseRequest.ClassName = _args.Type;

			if (!_args.NonPaged && (!Script.IsNullOrUndefined(_baseRequest.Limit) || _baseRequest.Limit == 0))
				_baseRequest.Limit = _args.Mode == ListMode.List ? ListRowNumberLimit : SelectRowNumberLimit;

			tbar = CreateActionToolbar();

			if (!Script.IsValue(store))
				store = CreateStore();

			if (!Script.IsValue(selModel))
				selModel = CreateSelectionModel();

			if (!Script.IsValue(_args.ColumnsConfig))
				_args.ColumnsConfig = GetColumnsConfig();

			ArrayList cols = new ArrayList();
			cols.AddRange((object[])_args.ColumnsConfig);

			colModel = CreateColumnModel(_args.ColumnsConfig);

			if (!Script.IsValue(view))
				view = new AutoGridView(new GridViewConfig().forceFit(true).ToDictionary());

			((AutoGridView)view).SetColumns((Column[])cols);

			InitStore(store);
			InitSelectionModel((AbstractSelectionModel)selModel);

			if (!_args.NonPaged)
			{
				_filterPlugin = new GridFilterPlugin(this);
				plugins = new object[] { _filterPlugin };

				bbar = CreatePagingToolbar();
			}

			on("render", new AnonymousDelegate(OnRender));

			base.initComponent();
		}

		public RangeRequest BaseRequest
		{
			get { return _baseRequest; }
		}

		public AutoGridArgs Args
		{
			get { return _args; }
		}

		public ListConfig ListConfig
		{
			get { return _args.ListConfig; }
		}

		public GridView GridView
		{
			get { return getView(); }
		}

		public ColumnModel ColumnModel
		{
			get { return getColumnModel(); }
		}

		public RowSelectionModel SelectionModel
		{
			get { return (RowSelectionModel)getSelectionModel(); }
		}

		public void Reload(bool reinitPaging)
		{
			stopEditing();

			_baseRequest.VisibleProperties = (string[])GetVisibleProperties();

			if (reinitPaging)
				store.reload(new Dictionary("params", new Dictionary("start", 0)));
			else
				store.reload();
		}

		public void Refresh()
		{
			Record[] records = store.getRange();

			if (records.Length == 0)
				return;

			object[] ids = new object[records.Length];

			for (int i = 0; i < ids.Length; ++i)
			{
				if (records[i].id == null)
					continue;

				ids[i] = records[i].id;
			}

			GenericService.Refresh(_args.Type, ids, (string[])GetVisibleProperties(), _args.ForcedProperties,
				delegate (object result)
				{
					if (result == null)
						return;

					object[] objs = (object[])result;

					for (int i = 0; i < objs.Length; ++i)
					{
						object[] obj = (object[])objs[i];

						if (obj == null)
						{
							((RecordMeta)records[i].data).__deleted = true;
						}
						else
						{
							records[i].data = ((RangeReader)store.reader).ReadData(obj);
						}

						records[i].commit();
					}
				},
				null);
		}

		public void ReloadGrid(RangeRequest request)
		{
			request.VisibleProperties = (string[])GetVisibleProperties();
			request.HiddenProperties = _args.ForcedProperties;

			request.Limit = _baseRequest.Limit;

			store.reload(new Dictionary("params", request));
		}

		public void UpdateRecord(object values, Record record)
		{
			Dictionary obj = Dictionary.GetDictionary(values);

			Ext.data.Field[] recordFields = (Ext.data.Field[])record.fields.getRange();

			foreach (Ext.data.Field t in recordFields)
			{
				string name = t.name;

				if (obj.ContainsKey(name))
					Type.SetField(record.data, name, obj[name]);
			}

			record.commit();
		}

		public ColumnConfig GetColumnConfig(int columnIndex)
		{
			return TryGetColumnConfigByName(((ColumnModel)colModel).getColumnId(columnIndex));
		}

		public ColumnConfig GetColumnConfigByName(string name)
		{
			ColumnConfig config = TryGetColumnConfigByName(name);

			if (config == null)
				throw new Exception("Unknown column name: " + name);

			return config;
		}

		public ColumnConfig TryGetColumnConfigByName(string name)
		{
			ColumnConfig[] columnConfigs = ListConfig.Columns;

			foreach (ColumnConfig config in columnConfigs)
				if (config.Name == name)
					return config;

			return null;
		}

		public bool HandleKeyEvent(jQueryEvent keyEvent)
		{
			Dictionary eventDictionary = Dictionary.GetDictionary(typeof(EventObject));

			int key = keyEvent.Which;
			bool isAdditionalKey = keyEvent.CtrlKey || keyEvent.ShiftKey || keyEvent.AltKey;
			bool isCtrlKey = keyEvent.CtrlKey && !keyEvent.ShiftKey && !keyEvent.AltKey;

			if (key == (int)eventDictionary["UP"])
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				RestoreFocus();

				return true;
			}

			if (key == (int)eventDictionary["DOWN"] && !isAdditionalKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				RestoreFocus();

				return true;
			}

			if (key == (int)eventDictionary["F2"] && !isAdditionalKey)
			{
				if (SelectionModel.getSelections().Length != 1)
					return true;

				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				startEditing(store.indexOf(SelectionModel.getSelected()), 1);

				return true;
			}

			if (key == (int)eventDictionary["ESC"])
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				RestoreFocus();

				return true;
			}

			if (key == (int)eventDictionary["F"] && isCtrlKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				_generalFilterField.focus();

				return true;
			}

			if (_args.Mode == ListMode.List)
			{
				if (key == (int)eventDictionary["INSERT"] && !isAdditionalKey)
				{
					keyEvent.PreventDefault();
					keyEvent.StopPropagation();

					if (!ListConfig.IsCreationAllowed.IsDisabled && ListConfig.IsCreationAllowed.Visible)
						Create(_args.Type);

					return true;
				}

				if (key == (int)eventDictionary["DELETE"] && isCtrlKey)
				{
					keyEvent.PreventDefault();
					keyEvent.StopPropagation();

					if (!ListConfig.IsRemovingAllowed.IsDisabled && ListConfig.IsRemovingAllowed.Visible)
						Remove();

					return true;
				}

				if (key == (int)eventDictionary["ENTER"] && !isAdditionalKey)
				{
					keyEvent.PreventDefault();
					keyEvent.StopPropagation();

					View();

					return true;
				}

				if (key == (int)eventDictionary["ENTER"] && !keyEvent.CtrlKey && keyEvent.ShiftKey && !keyEvent.AltKey)
				{
					keyEvent.PreventDefault();
					keyEvent.StopPropagation();

					if (SelectionModel.getSelections().Length == 0)
						return true;

					if (!ListConfig.IsEditAllowed.IsDisabled && ListConfig.IsEditAllowed.Visible)
						Edit();

					return true;
				}

				if (key == (int)eventDictionary["D"] && isCtrlKey)
				{
					keyEvent.PreventDefault();
					keyEvent.StopPropagation();

					if (!ListConfig.IsCreationAllowed.IsDisabled && ListConfig.IsCreationAllowed.Visible)
						Copy();

					return true;
				}
			}
			else
			{
				if (key == (int)eventDictionary["ENTER"] && !isAdditionalKey)
				{
					keyEvent.PreventDefault();
					keyEvent.StopPropagation();

					Select();

					return true;
				}
			}

			if (key == (int)eventDictionary["PAGE_UP"] && !isAdditionalKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				if (_currentPage > 1)
					_pagingToolbar.changePage(_currentPage - 1);

				return true;
			}

			if (key == (int)eventDictionary["PAGE_DOWN"] && !isAdditionalKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				if (_currentPage < _totalPages)
					_pagingToolbar.changePage(_currentPage + 1);

				return true;
			}

			if (key == (int)eventDictionary["HOME"] && isCtrlKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				if (_currentPage != 1)
					_pagingToolbar.changePage(1);

				SelectionModel.selectRow(0);

				return true;
			}

			if (key == (int)eventDictionary["END"] && isCtrlKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				if (_currentPage != _totalPages)
					_pagingToolbar.changePage(_totalPages);

				SelectionModel.selectRow(store.getCount() - 1);

				return true;
			}

			if (key == (int)eventDictionary["HOME"] && !isAdditionalKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				SelectionModel.selectRow(0);

				return true;
			}

			if (key == (int)eventDictionary["END"] && !isAdditionalKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				SelectionModel.selectRow(store.getCount() - 1);

				return true;
			}

			if (key == (int)eventDictionary["R"] && isCtrlKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				Reload(false);

				return true;
			}

			if (key == (int)eventDictionary["G"] && isCtrlKey)
			{
				keyEvent.PreventDefault();
				keyEvent.StopPropagation();

				Element field = (Element)Dictionary.GetDictionary(_pagingToolbar)["field"];

				field.Focus();

				return true;
			}

			return false;
		}

		public void ReloadWithData(RangeResponse values)
		{
			((WebServiceProxy)store.proxy).SetResponse(values);

			Reload(false);

			SelectionModel.selectRow(values.SelectedRow);
		}

		public object[] GetSelectedIds()
		{
			Record[] records = (Record[])SelectionModel.getSelections();

			ArrayList ids = new ArrayList();

			foreach (Record t in records)
				ids.Add(t.id);

			return (object[])ids;
		}

		public Reference[] GetSelectedItems()
		{
			Record[] records = (Record[])SelectionModel.getSelections();

			Reference[] list = new Reference[records.Length];

			for (int i = 0; i < records.Length; i++)
				list[i] = GetInfo(records[i]);

			return list;
		}

		private Reference GetInfo(Record record)
		{
			Reference info = new Reference();

			info.Id = record.id;
			info.Name = ResolveReferenceText(record);
			info.Type = ResolveType(record);

			return info;
		}

		private void OnRender()
		{
			GridView.on("refresh", new GridViewRefreshDelegate(
				delegate
				{
					if (store.getCount() == 0)
						return;

					if (_selectedRowNumber != -1)
					{
						SelectionModel.selectRow(_selectedRowNumber);
						return;
					}

					int index = (int)(Dictionary.GetDictionary(SelectionModel)["lastActive"] ?? 0);

					if (index != 0)
					{
						if (index >= store.getCount())
							index = store.getCount() - 1;

						SelectionModel.selectRow(index);

						GridView.focusRow(index);
					}
				}));


			Load();
		}

		private void Load()
		{
			if (_args.NonPaged)
				GenericService.GetRange(_args.Type, null, delegate (object result) { store.loadData(((RangeResponse)result).List); }, null);
			else
			{
				_baseRequest.VisibleProperties = (string[])GetVisibleProperties();
				_baseRequest.HiddenProperties = _args.ForcedProperties;

				store.load();
			}
		}

		private Store CreateStore()
		{
			Store gridStore;

			ColumnConfig[] columnConfigs = ListConfig.Columns;

			ArrayList dataFields = new ArrayList();

			dataFields.Add(ObjectPropertyNames.Id);
			dataFields.Add(ObjectPropertyNames.ObjectClass);
			dataFields.Add(ObjectPropertyNames.Version);

			foreach (ColumnConfig t in columnConfigs)
				dataFields.Add(t.Name);

			if (_args.NonPaged)
			{
				gridStore = new ArrayStore(new ArrayStoreConfig()
					.id(0)
					.fields(dataFields)
					.ToDictionary());
			}
			else
			{
				WebServiceProxy proxy = GenericService.GetRangeProxy(_args.Type);

				proxy.on("load", new DataProxyLoadDelegate(OnDataProxyLoaded));

				gridStore = new GenericStore(new StoreConfig()
					.baseParams(_baseRequest)
					.proxy(proxy)
					.reader(new RangeReader(
						new Dictionary(
							"id", 0,
							"root", "List",
							"totalProperty", "TotalCount"
							),
						dataFields
						))
					.remoteSort(true)
					.ToDictionary());
			}

			//Script.Eval("console.log(dataFields)");

			return gridStore;
		}

		private void InitStore(Store gridStore)
		{
			gridStore.on("update", new StoreUpdateDelegate(
				delegate (Store s, Record record, string operation)
				{
					if (operation != Record.EDIT)
						return;

					Update(record);
				}));
		}

		private AbstractSelectionModel CreateSelectionModel()
		{
			CheckboxSelectionModel model = new CheckboxSelectionModel();
			model.singleSelect = ListConfig.SingleSelect;

			return model;
		}

		private void InitSelectionModel(AbstractSelectionModel model)
		{
			model.on("selectionchange", new SelectionChangedDelegate(OnSelectonChanged));
		}

		private ColumnModel CreateColumnModel(ArrayList columnsConfig)
		{
			if (!((CheckboxSelectionModel)selModel).singleSelect)
				columnsConfig.Insert(0, selModel);

			return new ColumnModel(new ColumnModelConfig()
				.columns((Array)columnsConfig)
				.listeners(new Dictionary(
					"hiddenchange", new ColumnModelHiddenchangeDelegate(
						delegate (ColumnModel columnModel, double columnIndex, bool isHidden)
						{
							if (_generalFilterField.getValue() != null && ApplyGeneralFilter(false))
								Reload(true);
							else if (!isHidden)
								Reload(false);
						})))
				.ToDictionary());
		}

		private ArrayList GetColumnsConfig()
		{
			ColumnConfig[] columnConfigs = ListConfig.Columns;

			ArrayList cols = new ArrayList();

			bool isVisiblePropertiesPassed = !Script.IsNullOrUndefined(_baseRequest.VisibleProperties);

			foreach (ColumnConfig config in columnConfigs)
			{
				if (_args.Mode == ListMode.Select && config.Type == TypeEnum.Object)
					((ClassColumnConfig)config).RenderAsString = true;

				Delegate renderer = ControlFactory.CreateRenderer(config);

				if (_args.Mode == ListMode.List && renderer == null && config.IsReference && FormsRegistry.HasViewForm(_args.Type))
					renderer = ControlFactory.CreateRefrenceRenderer(_args.Type);

				Field editor = null;

				if (_args.Mode == ListMode.List && !config.IsReadOnly && ListConfig.IsQuickEditAllowed)
					editor = ControlFactory.CreateEditor(config, true);

				Ext.grid.ColumnConfig cfg = new Ext.grid.ColumnConfig()
					.id(config.Name)
					.header(config.Caption ?? config.Name)
					.sortable(config.IsPersistent)
					.dataIndex(config.Name)
					.hidden(isVisiblePropertiesPassed ? !_baseRequest.VisibleProperties.Contains(config.Name) : config.Hidden);

				if (renderer != null)
					cfg.renderer(renderer);

				if (editor != null)
					cfg.editor(editor);

				if (config.ListWidth != 0)
					cfg.width((double)config.ListWidth);

				cols.Add(cfg.ToDictionary());
			}

			return cols;
		}

		private object CreateActionToolbar()
		{
			ArrayList list = new ArrayList();

			if (_args.Mode == ListMode.List)
			{
				if (ListConfig.IsCreationAllowed.Visible && !ListConfig.UseCustomCreation)
				{
					_createAction = new Action(new ActionConfig()
						.text(BaseRes.CreateItem_Lower)
						.handler(new AnonymousDelegate(delegate { Create(_args.Type); }))
						.disabled(ListConfig.IsCreationAllowed.IsDisabled)
						// !!! Button must be created not Action !!!
						.custom("tooltip", ListConfig.IsCreationAllowed.DisableInfo)
						.ToDictionary());

					list.Add(_createAction);
				}

				if (ListConfig.IsEditAllowed.Visible)
				{
					_editAction = new Action(new ActionConfig()
						.text(BaseRes.Edit_Lower)
						.disabled(true)
						.handler(new AnonymousDelegate(Edit))
						// !!! Button must be created not Action !!!
						.custom("tooltip", ListConfig.IsEditAllowed.DisableInfo)
						.ToDictionary());

					list.Add(_editAction);
				}

				if (ListConfig.IsCopyingAllowed.Visible)
				{
					_copyAction = new Action(new ActionConfig()
						.text(BaseRes.Copy_Lower)
						.disabled(true)
						.handler(new AnonymousDelegate(Copy))
						// !!! Button must be created not Action !!!
						.custom("tooltip", ListConfig.IsCreationAllowed.DisableInfo)
						.ToDictionary());

					list.Add(_copyAction);
				}

				if (ListConfig.IsRemovingAllowed.Visible)
				{
					_deleteAction = new Action(new ActionConfig()
						.text(BaseRes.Remove_Lower)
						.disabled(true)
						.handler(new AnonymousDelegate(Remove))
						// !!! Button must be created not Action !!!
						.custom("tooltip", ListConfig.IsRemovingAllowed.DisableInfo)
						.ToDictionary());

					list.Add(_deleteAction);
				}


				_args.OnCreateActionToolbar(list, this);

				if (!_args.NonPaged)
				{
					_resetFilterButton = new Button(new ButtonConfig()
						.text(BaseRes.AutoGrid_ResetFilter_Title)
						.disabled(true)
						.handler(new AnonymousDelegate(ResetFilter))
						.ToDictionary());

					if (list.Count > 0)
						list.Add("-");

					list.Add(_resetFilterButton);
				}

				_exportSelectionItem = new Item(new ItemConfig()
					.text(BaseRes.Export_Selection)
					.handler(new AnonymousDelegate(delegate { GridExport(DocumentExportMode.Selected); }))
					.disabled(true)
					.ToDictionary());

				_exportAction = new Action(new ActionConfig()
					.iconCls("export")
					.disabled(false)
					// !!! Button must be created not Action !!!
					.custom("menu", new Menu(new MenuConfig()
						.items(new object[]
						{
							new Item(new ItemConfig()
								.text(BaseRes.Export_All)
								.handler(new AnonymousDelegate(delegate { GridExport(DocumentExportMode.All); }))
								.ToDictionary()),
							_exportSelectionItem,
							new Item(new ItemConfig()
								.text(BaseRes.Export_ExceptSelection)
								.handler(new AnonymousDelegate(delegate { GridExport(DocumentExportMode.ExceptSelected); }))
								.ToDictionary())
						})
						.listeners(new Dictionary("beforeshow",
							new MenuBeforeshowDelegate(delegate (Component component)
							{
								Menu menu = (Menu)component;

								int totalCount = store.getTotalCount();

								int selectedCount = SelectionModel.getSelections().Length;

								Item item = (Item)((MixedCollection)menu.items).itemAt(0);

								item.setText(string.Format(BaseRes.Export_All, totalCount));

								item = (Item)((MixedCollection)menu.items).itemAt(1);

								item.setText(string.Format(BaseRes.Export_Selection, selectedCount));

								item = (Item)((MixedCollection)menu.items).itemAt(2);

								item.setText(string.Format(BaseRes.Export_ExceptSelection, totalCount - selectedCount));
							})))
						.ToDictionary())
					)
					.ToDictionary()
				);

				list.Add(_exportAction);
			}

			int spacePos = list.Count == 0 ? 0 : list.Count - 1;

			if (ListConfig.Filterable)
			{
				_generalFilterField = new TextField(new TextFieldConfig()
					.value(_baseRequest.GeneralFilter)
					.enableKeyEvents(true)
					.listeners(new Dictionary(
						"keydown", new TextFieldKeypressDelegate(
							delegate (TextField objthis, EventObject e)
							{
								double key = e.getKey();

								if (key == EventObject.ENTER)
								{
									e.stopEvent();

									ApplyGeneralFilter(true);
								}
								else if (key == EventObject.ESC)
									objthis.setValue(_baseRequest.GeneralFilter);
							}),
						"change", new AnonymousDelegate(delegate { ApplyGeneralFilter(true); })
						))
					.ToDictionary());

				BoxComponent component = new BoxComponent(new BoxComponentConfig()
					.autoEl(new Dictionary("tag", "div"))
					.cls("filter")
					.ToDictionary());

				list.AddRange(new object[] { component, _generalFilterField });
				//list.AddRange(new object[] { BaseRes.AutoGrid_SimpleFilter_Title, _generalFilterField });
			}

			if (_args.Mode == ListMode.Select)
			{
				list.Add("->");

				_selectAction = new Action(new ActionConfig()
					.text(BaseRes.Select_Lower)
					.handler(new AnonymousDelegate(Select))
					.disabled(true)
					.ToDictionary());

				Action cancelAction = new Action(new ActionConfig()
					.text(BaseRes.Cancel_Lower)
					.handler(new AnonymousDelegate(
						delegate
						{
							if (OnCancelSelect != null)
								OnCancelSelect();
						}))
					.ToDictionary());

				list.Add(_selectAction);
				list.Add(cancelAction);
			}

			if (list.Count == 0)
				return null;

			if (_args.Mode == ListMode.List && list.Count != spacePos)
				list.Insert(spacePos, "->");

			Toolbar toolbar = new Toolbar(list);
			toolbar.cls = "autoGridToolbar";
			return toolbar;
		}

		private PagingToolbar CreatePagingToolbar()
		{
			PagingToolbarConfig pagingToolbarConfig = new PagingToolbarConfig()
				.pageSize(_baseRequest.Limit)
				.store(store)
				.displayInfo(true)
				.displayMsg(BaseRes.AutoGrid_DispayMsg)
				.emptyMsg(BaseRes.AutoGrid_EmptyMsg);

			_pagingToolbar = new PagingToolbar(pagingToolbarConfig.ToDictionary());
			_pagingToolbar.on("change", new PagingToolbarChangeDelegate(
				delegate (PagingToolbar obj, object changeEvent)
				{
					_currentPage = (int)Dictionary.GetDictionary(changeEvent)["activePage"];
					_totalPages = (int)Dictionary.GetDictionary(changeEvent)["pages"];
				}));

			return _pagingToolbar;
		}


		public void Create(string type)
		{
			if (Script.IsNullOrUndefined(type))
				type = _args.Type;

			FormsRegistry.EditObject(type, null, null,
				delegate (object result)
				{
					ItemResponse response = (ItemResponse)result;

					Dictionary obj = (Dictionary)response.Item;
					RangeResponse rangeResponse = response.RangeResponse;

					object reference = obj[ObjectPropertyNames.Reference];

					if (reference is Date)
						reference = ((Date)reference).Format("dd.MM.yyyy");

					string message = BaseRes.Created + " " + reference;

					if (Script.IsNullOrUndefined(rangeResponse.SelectedRow))
					{
						MessageRegister.Info(ListConfig.Caption, message, BaseRes.AutoGrid_NotDisplay_Msg);

						return;
					}

					MessageRegister.Info(ListConfig.Caption, message);

					ReloadWithData(rangeResponse);
				},
				null,
				_baseRequest
			);
		}



		public void Edit()
		{
			Record record = SelectionModel.getSelected();

			string type = ResolveType(record);

			GenericService.CanUpdate(type, record.id,
				delegate (object res)
				{
					OperationStatus status = (OperationStatus)res;

					if (!status.IsEnabled)
					{
						string msg = Script.IsNullOrUndefined(status.DisableInfo) ? BaseRes.AutoGrid_ActionNotPermitted_Msg : status.DisableInfo;

						MessageBoxWrap.Show(new Dictionary(
							"title", BaseRes.Error,
							"msg", msg,
							"icon", MessageBox.ERROR,
							"buttons", MessageBox.OK
							));
						return;
					}

					FormsRegistry.EditObject(type, record.id, null,
						delegate (object result)
						{
							ItemResponse response = (ItemResponse)result;

							Dictionary obj = (Dictionary)response.Item;
							RangeResponse rangeResponse = response.RangeResponse;

							object reference = obj[ObjectPropertyNames.Reference];

							if (reference is Date)
								reference = ((Date)reference).Format("d.m.Y");

							string message = BaseRes.Updated + " " + reference;

							if (Script.IsNullOrUndefined(rangeResponse.SelectedRow))
								MessageRegister.Info(ListConfig.Caption, message, BaseRes.AutoGrid_NotDisplay_Msg);
							else
								MessageRegister.Info(ListConfig.Caption, message);

							ReloadWithData(rangeResponse);
						}, null, _baseRequest);
				}, null);
		}

		private void Copy()
		{
			Record selected = SelectionModel.getSelected();

			string type = ResolveType(selected);

			FormsRegistry.EditObject(type, selected.id, null,
				delegate (object result)
				{
					ItemResponse response = (ItemResponse)result;

					Dictionary obj = (Dictionary)response.Item;
					RangeResponse rangeResponse = response.RangeResponse;

					object reference = obj[ObjectPropertyNames.Reference];

					if (reference is Date)
						reference = ((Date)reference).Format("d.m.Y");

					string message = BaseRes.Created + " " + reference;

					if (Script.IsNullOrUndefined(rangeResponse.SelectedRow))
					{
						MessageRegister.Info(ListConfig.Caption, message, BaseRes.AutoGrid_NotDisplay_Msg);
						return;
					}

					MessageRegister.Info(ListConfig.Caption, message);

					ReloadWithData(rangeResponse);
				},
				null, _baseRequest, LoadMode.Remote, true);
		}

		private void View()
		{
			Record record = SelectionModel.getSelected();

			if (record == null)
				return;

			string type = ResolveType(record);

			if (FormsRegistry.HasViewForm(type))
				FormsRegistry.ViewObject(type, record.id);
		}

		private void Remove()
		{
			Array selected = SelectionModel.getSelections();

			if (selected == null || selected.Length == 0)
				return;

			object[] ids = new string[selected.Length];

			for (int i = 0; i < selected.Length; i++)
				ids[i] = ((Record)selected[i]).id;

			GenericService.CanDelete(_args.Type, ids,
				delegate (object res)
				{
					OperationStatus status = (OperationStatus)res;

					if (!status.IsEnabled)
					{
						string msg = Script.IsNullOrUndefined(status.DisableInfo) ? BaseRes.AutoGrid_ActionNotPermitted_Msg : status.DisableInfo;

						MessageBoxWrap.Show(new Dictionary(
							"title", BaseRes.Error,
							"msg", msg,
							"icon", MessageBox.ERROR,
							"buttons", MessageBox.OK
						));
						return;
					}

					MessageBoxWrap.Confirm(BaseRes.Confirmation, StringUtility.GetNumberText(selected.Length, BaseRes.AutoGrid_DeleteMsg1, BaseRes.AutoGrid_DeleteMsg2, BaseRes.AutoGrid_DeleteMsg3),
						delegate (string button, string text)
						{
							if (button == "yes")
								RemoveRows();
						});
				}, null);
		}

		private void GridExport(DocumentExportMode mode)
		{
			DocumentExportArgs args = new DocumentExportArgs();
			args.Mode = mode;
			args.Request = (RangeRequest)store.baseParams;

			if (mode == DocumentExportMode.ExceptSelected || mode == DocumentExportMode.Selected)
			{
				Record[] records = (Record[])SelectionModel.getSelections();

				ArrayList ids = new ArrayList();

				foreach (Record t in records)
					ids.Add(t.id);

				args.SelectedDocuments = (object[])ids;
			}

			ReportLoader.Load(string.Format("export/{0}/Export_{1}.xls", _args.Type, Date.Now.Format("Y-m-d_H-i")), new Dictionary("exportParams", Json.Stringify(args)));
		}

		private void Select()
		{
			Record[] selections = (Record[])SelectionModel.getSelections();
			ArrayList ids = new ArrayList();

			foreach (Record t in selections)
				ids.Add(t.id);

			if (OnSelect != null)
				OnSelect(ids);
		}

		private void RemoveRows()
		{
			Record[] selectedRecords = (Record[])SelectionModel.getSelections();

			if (selectedRecords == null)
				return;

			object[] selectedIds = new string[selectedRecords.Length];

			for (int i = 0; i < selectedRecords.Length; i++)
				selectedIds[i] = selectedRecords[i].id;

			GenericService.Delete(_args.Type, selectedIds, _baseRequest,
				delegate (object result)
				{
					DeleteOperationResponse response = (DeleteOperationResponse)result;

					if (response.Success)
						OnDeleteSuccess(response.RangeResponse, selectedRecords);
					else if (selectedRecords.Length == 1)
						OnSingleDeleteFailed(selectedRecords[0]);
					else
						OnMultipleDeleteFailed(response.UndeletableObjects, selectedRecords);
				},
				null);
		}

		private void OnDeleteSuccess(RangeResponse result, Record[] deleted)
		{
			string message = null;

			if (deleted.Length <= MaxMessageDetailItems)
				message = RecordListToString(deleted, ", ");

			if (string.IsNullOrEmpty(message))
				MessageRegister.Info(ListConfig.Caption + ": " + StringUtility.GetNumberText(deleted.Length, BaseRes.AutoGrid_DeleteCompletedMsg1, BaseRes.AutoGrid_DeleteCompletedMsg2, BaseRes.AutoGrid_DeleteCompletedMsg3));
			else
				MessageRegister.Info(ListConfig.Caption, BaseRes.Deleted + " " + message);

			ReloadWithData(result);
		}

		private void OnSingleDeleteFailed(Record record)
		{
			string type = ResolveType(record);

			GenericService.CanReplace(type, record.id,
				delegate (object result)
				{
					if ((bool)result)
					{
						ReplaceForm form = new ReplaceForm(type, record.id);
						form.Saved += delegate { Reload(false); };

						form.Open();
					}
					else
					{
						MessageBoxWrap.Show(BaseRes.Warning,
							BaseRes.AutoGrid_DeleteConstrainedFailed_Msg + "<br/>" + BaseRes.AutoGrid_ReplaceToAdmin_Msg,
							MessageBox.WARNING, MessageBox.OK);
					}
				}, null);
		}

		private void OnMultipleDeleteFailed(object[] objects, Record[] records)
		{
			const string newLine = "<br>";

			StringBuilder msg = new StringBuilder(BaseRes.AutoGrid_DeleteFailed_Msg + newLine + newLine);

			msg.Append("<div class='undeletable'>");

			for (int i = 0; i < objects.Length; i++)
			{
				if (i == 10)
				{
					msg.Append("<div>...</div>");
					break;
				}

				Array val = (Array)objects[i];
				msg.Append(string.Format("<div>{0}</div>", val[Reference.NamePos]));
			}

			msg.Append("</div>");

			if (objects.Length == records.Length)
				MessageBoxWrap.Show(BaseRes.Warning, msg.ToString(), MessageBox.WARNING, MessageBox.OK);
			else
			{
				msg.Append(newLine);
				msg.Append(BaseRes.AutoGrid_ContinueDelete_Msg);

				MessageBoxWrap.Show(BaseRes.Warning, msg.ToString(), MessageBox.WARNING, MessageBox.YESNO,
					delegate (string button, string text1)
					{
						if (button != "yes")
							return;

						foreach (Array val in objects)
							foreach (Record record in records)
								if (val[Reference.IdPos] == record.id)
								{
									SelectionModel.deselectRow(store.indexOf(record));
									break;
								}

						RemoveRows();
					});
			}
		}

		private void Update(Record record)
		{
			Dictionary newData = Dictionary.GetDictionary(record.getChanges());
			Dictionary oldData = Dictionary.GetDictionary(record.modified);

			foreach (DictionaryEntry old in oldData)
			{
				if (newData[old.Key] is string && string.IsNullOrEmpty((string)newData[old.Key]))
					newData[old.Key] = null;

				if (old.Value == newData[old.Key])
					newData.Remove(old.Key);
			}

			if (newData.Count == 0)
			{
				record.reject(true);
				return;
			}

			string type = ResolveType(record);

			object version = record.get(ObjectPropertyNames.Version);

			GenericService.Update(type, record.id, version, newData, _baseRequest,
				delegate (object result)
				{
					ItemResponse response = (ItemResponse)result;
					Dictionary obj = (Dictionary)response.Item;

					UpdateRecord(obj, record);
				},
				delegate (WebServiceFailureArgs args) { OnAutoCommitFailed(args, record); });
		}

		private bool ApplyGeneralFilter(bool reload)
		{
			RangeRequest rangeRequest = (RangeRequest)store.baseParams;

			string filter = (string)_generalFilterField.getValue();

			ArrayList properties = GetVisibleProperties();

			if (rangeRequest.GeneralFilter == filter && rangeRequest.VisibleProperties.Length == properties.Count)
			{
				bool isEquals = true;

				foreach (string t in rangeRequest.VisibleProperties)
				{
					if (!properties.Contains(t))
						isEquals = false;
				}

				if (isEquals)
					return false;
			}

			rangeRequest.GeneralFilter = filter;
			rangeRequest.VisibleProperties = (string[])properties;

			if (reload)
				Reload(true);

			return true;
		}

		private ArrayList GetVisibleProperties()
		{
			ArrayList properties = new ArrayList();

			foreach (ColumnConfig t in _args.ListConfig.Columns)
			{
				if (Script.IsNullOrUndefined(ColumnModel.getColumnById(t.Name)))
					continue;

				bool isHidden = (bool)Type.GetField(ColumnModel.getColumnById(t.Name), "hidden");

				if (Script.IsUndefined(isHidden) || !isHidden)
					properties.Add(t.Name);
			}

			return properties;
		}

		private void ResetFilter()
		{
			_baseRequest.NamedFilters = null;
			_baseRequest.GeneralFilter = null;
			_baseRequest.Filters = null;

			_generalFilterField.setValue();

			_filterPlugin.ResetFilter();

			Reload(true);
		}

		private void OnAutoCommitFailed(WebServiceFailureArgs args, Record record)
		{
			args.Handled = true;

			Button button = new Button(new ButtonConfig()
				.text("OK")
				.ToDictionary());

			Label label = new Label(new LabelConfig()
				.text(args.Error.Message)
				.style("font-weight: bold")
				.ToDictionary());

			Radio repeatRadio = new Radio(new RadioConfig()
				.boxLabel(BaseRes.AutoGrid_RepeatUpdate)
				.style("padding-top: 10px")
				.checked_(true)
				.name("updateOption")
				.ToDictionary());

			Radio rejectRadio = new Radio(new RadioConfig()
				.boxLabel(BaseRes.AutoGrid_RejectChanges)
				.name("updateOption")
				.ToDictionary());

			WindowClass window = new WindowClass(new WindowConfig()
				.width(350)
				.modal(true)
				.plain(true)
				.baseCls("x-panel")
				.bodyStyle("padding: 10px")
				.resizable(false)
				.title(BaseRes.Error)
				.items(new object[]
				{
					label,
					new Dictionary(
						"items", new object[] { repeatRadio, rejectRadio },
						"bodyStyle", "padding: 10px"
						)
				})
				.buttons(new object[] { button })
				.listeners(new Dictionary("close", new AnonymousDelegate(EventsManager.UnregisterKeyDownHandler)))
				.buttonAlign("center")
				.ToDictionary());

			button.on("click", new AnonymousDelegate(
				delegate
				{
					if ((bool)Type.GetField(repeatRadio, "checked"))
						Update(record);
					else
						store.rejectChanges();

					window.close();
				}));

			window.show();

			EventsManager.RegisterKeyDownHandler(window, false);
		}

		private void OnSelectonChanged(AbstractSelectionModel selectionModel)
		{
			int length = SelectionModel.getSelections().Length;

			if (_deleteAction != null && !ListConfig.IsRemovingAllowed.IsDisabled)
			{
				if (length == 0)
					_deleteAction.disable();
				else
					_deleteAction.enable();
			}

			if (_editAction != null && !ListConfig.IsEditAllowed.IsDisabled)
			{
				if (length != 1)
					_editAction.disable();
				else
					_editAction.enable();
			}

			if (_copyAction != null && !ListConfig.IsCreationAllowed.IsDisabled)
			{
				if (length != 1)
					_copyAction.disable();
				else
					_copyAction.enable();
			}

			if (_selectAction != null)
			{
				if (length == 0)
					_selectAction.disable();
				else
					_selectAction.enable();
			}

			if (_exportSelectionItem != null)
			{
				if (length == 0)
					_exportSelectionItem.disable();
				else
					_exportSelectionItem.enable();
			}
		}

		private void OnDataProxyLoaded(object responseProxy, object responseObject, object arg)
		{
			if (_resetFilterButton != null)
			{
				if (!string.IsNullOrEmpty(_baseRequest.GeneralFilter) ||
					(_baseRequest.Filters != null && _baseRequest.Filters.Length > 0))
				{
					_resetFilterButton.enable();

					Element button = ((Ext.Element)_resetFilterButton.getEl().child(_resetFilterButton.buttonSelector)).dom;
					button.SetAttribute(_resetFilterButton.tooltipType, GetCurrentFilterDescription());
				}
				else
				{
					_resetFilterButton.disable();

					Element button = ((Ext.Element)_resetFilterButton.getEl().child(_resetFilterButton.buttonSelector)).dom;
					button.SetAttribute(_resetFilterButton.tooltipType, string.Empty);
				}
			}

			store.setDefaultSort(_baseRequest.Sort, _baseRequest.Dir);

			RangeResponse response = (RangeResponse)responseObject;

			if (!Script.IsNullOrUndefined(response.SelectedRow))
				_selectedRowNumber = response.SelectedRow;
			else
				_selectedRowNumber = -1;

			_baseRequest.PositionableObjectId = null;
		}

		private string GetCurrentFilterDescription()
		{
			StringBuilder builder = new StringBuilder();

			string separator = string.Empty;

			if (!string.IsNullOrEmpty(_baseRequest.GeneralFilter))
			{
				builder.Append(string.Format(BaseRes.Filter_GeneralFilterMsg, _baseRequest.GeneralFilter));
				separator = " " + BaseRes.Filter_Conjunction;
			}

			if (_baseRequest.Filters != null && _baseRequest.Filters.Length > 0)
			{
				foreach (PropertyFilter filter in _baseRequest.Filters)
				{
					ColumnConfig columnConfig = TryGetColumnConfigByName(filter.Property);

					if (columnConfig == null)
						continue;

					foreach (PropertyFilterCondition condition in filter.Conditions)
					{
						string notStr = condition.Not ? " " + BaseRes.Filter_Not : "";

						switch (condition.Operator)
						{
							case FilterOperator.IsNull:
								builder.Append(string.Format("{0} '{1}'{2} {3}",
									separator,
									columnConfig.Caption,
									notStr,
									EnumUtility.Localize(typeof(FilterOperator), condition.Operator, typeof(BaseRes)).ToLowerCase()));
								break;
							case FilterOperator.Equals:
							case FilterOperator.StartsWith:
							case FilterOperator.Contains:
							case FilterOperator.EndsWith:
							case FilterOperator.Less:
							case FilterOperator.LessOrEquals:
							case FilterOperator.Greater:
							case FilterOperator.GreaterOrEquals:
								builder.Append(string.Format("{0} '{1}'{2} {3} {4}",
									separator,
									columnConfig.Caption,
									notStr,
									EnumUtility.Localize(typeof(FilterOperator), condition.Operator, typeof(BaseRes)).ToLowerCase(),
									ConvertParameter(condition.Value, columnConfig)));
								break;
							case FilterOperator.IsIn:
								{
									string operatorStr = ((Array)condition.Value).Length == 1
										? EnumUtility.Localize(typeof(FilterOperator), FilterOperator.Equals, typeof(BaseRes)).
											ToLowerCase()
										: EnumUtility.Localize(typeof(FilterOperator), condition.Operator, typeof(BaseRes)).
											ToLowerCase();

									builder.Append(string.Format("{0} '{1}'{2} {3} {4}",
										separator,
										columnConfig.Caption,
										notStr,
										operatorStr,
										ConvertParameter(condition.Value, columnConfig)));
									break;
								}
						}
					}

					separator = " " + BaseRes.Filter_Conjunction;
				}
			}

			if (builder.IsEmpty)
				return string.Empty;

			return builder.ToString();
		}

		private static string ConvertParameter(object value, ColumnConfig columnConfig)
		{
			switch (columnConfig.Type)
			{
				case TypeEnum.String:
				case TypeEnum.Object:
					return string.Format("'{0}'", value);

				case TypeEnum.Bool:
					if ((bool)value)
						return BaseRes.Filter_True;
					return BaseRes.Filter_False;

				case TypeEnum.Date:
					RenderDelegate dateRenderer = (RenderDelegate)ControlFactory.CreateRenderer(columnConfig);
					return string.Format("'{0}'", (string)dateRenderer.Invoke(value));

				case TypeEnum.List:
					Array values = ((Array)value);

					RenderDelegate listRenderer = (RenderDelegate)ControlFactory.CreateRenderer(columnConfig);

					string result = "";

					if (values.Length == 1)
						result = string.Format("'{0}'", listRenderer.Invoke(values[0]));
					else
					{
						string separator = "";

						foreach (object t in values)
						{
							result += string.Format("{0}'{1}'", separator, listRenderer.Invoke(t.GetType()));
							separator = ", ";
						}

						result = string.Format("({0})", result);
					}

					return result;
			}

			return (string)value;
		}

		private string RecordListToString(Record[] selected, string separator)
		{
			string refColumn = GetReferenceColumn();

			if (refColumn == null)
				return null;

			StringBuilder builder = new StringBuilder();
			string sep = "";

			foreach (Record t in selected)
			{
				object value = t.get(refColumn);

				if (value is Array)
					value = ((Array)value)[Reference.NamePos];

				if (value is Date)
					value = ((Date)value).Format("d.m.Y");

				builder.Append(sep + (string)value);
				sep = separator;
			}

			if (builder.IsEmpty)
				return null;

			return builder.ToString();
		}

		private string GetReferenceColumn()
		{
			foreach (ColumnConfig config in ListConfig.Columns)
			{
				if (config.IsReference)
					return config.Name;
			}

			return null;
		}

		private void RestoreFocus()
		{
			Record selectedRecord = SelectionModel.getSelected();

			if (Script.IsNullOrUndefined(selectedRecord))
			{
				if (store.getCount() > 0)
				{
					SelectionModel.selectRow(0);

					GridView.focusRow(0);
				}
			}
			else
				GridView.focusRow(store.indexOf(selectedRecord));
		}

		private string ResolveType(Record record)
		{
			string type = (string)Type.GetField(record.data, ObjectPropertyNames.ObjectClass);

			if (Script.IsNullOrUndefined(type))
				type = _args.Type;
			return type;
		}

		private string ResolveReferenceText(Record record)
		{
			string refColumn = GetReferenceColumn();

			if (refColumn == null)
				return null;

			return (string)record.get(refColumn);
		}

		private const int MaxMessageDetailItems = 5;
		private const int ListRowNumberLimit = 25;
		private const int SelectRowNumberLimit = 10;

		[PreserveName]
#pragma warning disable 649
		private readonly AutoGridArgs _args;
#pragma warning restore 649

		private RangeRequest _baseRequest;

		private GridFilterPlugin _filterPlugin;

		private Action _createAction;
		private Action _editAction;
		private Action _copyAction;
		private Action _deleteAction;
		private Action _exportAction;
		private Item _exportSelectionItem;
		private Action _selectAction;

		private Button _resetFilterButton;

		private TextField _generalFilterField;

		private PagingToolbar _pagingToolbar;

		private int _currentPage;
		private int _totalPages;
		private int _selectedRowNumber = -1;
	}
}