using System;
using System.Collections;
using System.Serialization;

using Ext;
using Ext.data;

using LxnBase;
using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Luxena.Travel.Services;

using Action = Ext.Action;
using Record = Ext.data.Record;


namespace Luxena.Travel
{

	public delegate void AviaProcessSaveAndContinueAction(AviaDocumentProcessForm form, int selectedIndex, ArrayList processedDocumentIds);

	
	public class AviaDocumentListTab : ProductListTab
	{
		public AviaDocumentListTab(string tabId, ListArgs args) : base(tabId, args) { }

		public static void ListObject(ListArgs args, bool newTab)
		{
			ListObjectsOfType(typeof(AviaDocumentListTab), args, newTab);
		}

		protected override void HandleAltEnterPress()
		{
			if (AutoGrid.SelectionModel.getSelections().Length != 0)
				TryProcess();
		}

		protected override void HandleInsertPress()
		{
			Create(ClassNames.AviaTicket);
		}
		

		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			if (ListConfig.IsEditAllowed.Visible)
			{
				_handleButton = Action(Res.AviaDocument_Handle_Action.ToLowerCase(), TryProcess, true, ListConfig.IsEditAllowed.DisableInfo);

				CreateVoidAction(VoidDocuments);
			}

			_printButton = Action(Res.AviaDocument_Print_Action.ToLowerCase(), PrintDocuments, true);

			if (ListConfig.IsEditAllowed.Visible)
			{
				MenuAction(BaseRes.CreateItem_Lower, new object[]
				{
					MenuItem(DomainRes.AviaTicket, delegate { Create(ClassNames.AviaTicket); }),
					MenuItem(DomainRes.AviaMco, delegate { Create(ClassNames.AviaMco); }),
					MenuItem(DomainRes.AviaRefund, delegate { Create(ClassNames.AviaRefund); }),
					AppActions.AviaConsoleParser
				});
			}

			CreateStdToolbarActions();

			if (autoGrid != null)
			{
				if (toolbarItems.Contains(autoGrid.CreateAction))
					toolbarItems.Remove(autoGrid.CreateAction);
				if (toolbarItems.Contains(autoGrid.ExportAction))
					toolbarItems.Remove(autoGrid.ExportAction);
			}

			toolbarItems.InsertRange(0, (object[])toolbarActions);
		}

		protected override void OnRowChange(Record[] selections)
		{
			base.OnRowChange(selections);

			if (_handleButton != null && !ListConfig.IsEditAllowed.IsDisabled)
			{
				_handleButton.setDisabled(selections.Length != 1);
			}

			_printButton.setDisabled(selections.Length == 0 || !IsTickets);
		}


		protected override void CreateColumnConfigs()
		{
			AviaDocumentSemantic v = new SemanticDomain(this).AviaDocument;

			AddColumns(new object[]
			{
				v.IssueDate,
				ColumnCfg("Type"),
				v.Producer.ToColumn(true),

				v.AirlinePrefixCode.ToColumn(false, 50, CellRenderers.Right),

				v.Number.ToColumn(false, 70, v.GetNumberRenderer()),
				v.ReissueFor.ToColumn(true),
				v.PassengerName.ToColumn(false, 165, ProductSemantic.OnePassengerNameRenderer),
				v.Customer,
				v.PaymentType,
				v.EqualFare,
				v.Commission,
				v.Total,
				v.ServiceFee,
				v.FeesTotal.ToColumn(true),
				v.Vat.ToColumn(true),
				v.GrandTotal,
				v.Order,
				v.Seller,
				v.IsPaid,

				v.PnrCode.ToColumn(true, 70),
				ColumnCfg("ConjunctionNumbers", true),

				v.GdsPassportStatus.ToColumn(true, 100),
				ColumnCfg("GdsPassport", true, 135),
				ColumnCfg("Itinerary", true, 80),
				v.Intermediary.ToColumn(true),

			});

			if (UseHandling)
				AddColumns(new object[]
				{
					v.Handling.ToColumn(true),
					v.CommissionDiscount.ToColumn(true),
				});

			AddColumns(new object[]
			{
				v.Discount.ToColumn(true),
				v.BonusDiscount.ToColumn(true),
				v.BonusAccumulation.ToColumn(true),
				v.BookerOffice.ToColumn(true),
				v.BookerCode.ToColumn(true),
				v.Booker.ToColumn(true),
				v.TicketerOffice.ToColumn(true),
				v.TicketerCode.ToColumn(true),
				v.Ticketer.ToColumn(true),
				v.TicketingIataOffice.ToColumn(true),
				v.Owner.ToColumn(true),
				v.Originator.ToColumn(true),
				ColumnCfg("Origin", true),
				ColumnCfg("AirlinePnrCode", true),
				v.TourCode.ToColumn(true),
				v.IsProcessed.ToColumn(true),
				v.IsVoid.ToColumn(true),
				v.RequiresProcessing.ToColumn(true),
				v.Note.ToColumn(true),

				v.CreatedOn.ToColumn(true),
				v.CreatedBy.ToColumn(true),
				v.ModifiedOn.ToColumn(true),
				v.ModifiedBy.ToColumn(true),
			});
			
		}


		private void TryProcess()
		{
			BaseTryProcess(AutoGrid, _baseParams, delegate(AviaDocumentProcessForm form, int index, ArrayList ids)
			{
				_form = form;
				_selectedIndex = index;
				_processedDocumentIds = ids;
			});
		}


		public static void BaseTryProcess(AutoGrid autoGrid, RangeRequest baseParams, AviaProcessSaveAndContinueAction saveAndContinueAction)
		{
			Record record = autoGrid.SelectionModel.getSelected();
			string type = (string)Type.GetField(record.data, ObjectPropertyNames.ObjectClass);

			AviaService.CanProcess(record.id, type,
				delegate(object result)
				{
					ProcessOperationPermissionsResponse response = (ProcessOperationPermissionsResponse)result;
					
					OperationStatus canProcess = new OperationStatus();

					if (response.CustomActionPermissions.ContainsKey("CanProcess"))
						canProcess = (OperationStatus)(response.CustomActionPermissions["CanProcess"]);

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
								delegate(string button, string text)
								{
									if (button != "yes") return;

									FormsRegistry.EditObject(type, record.id, null, 
										delegate(object arg1)
										{
											ItemResponse itemResponse = ((ItemResponse)arg1);

											Dictionary obj = (Dictionary)itemResponse.Item;
											RangeResponse rangeResponse = itemResponse.RangeResponse;

											string message = BaseRes.Updated + " " + obj[ObjectPropertyNames.Reference];

											if (Script.IsNullOrUndefined(rangeResponse.SelectedRow))
											{
												MessageRegister.Info(DomainRes.AviaDocument_Caption_List, message, BaseRes.AutoGrid_NotDisplay_Msg);

												return;
											}

											MessageRegister.Info(DomainRes.AviaDocument_Caption_List, message);

											((WebServiceProxy)autoGrid.store.proxy).SetResponse(rangeResponse);

											autoGrid.Reload(false);

											autoGrid.SelectionModel.selectRow(rangeResponse.SelectedRow);

										}, null, baseParams
									);
								})
							));
						return;
					}

					Process(autoGrid, (string)record.id, type, response.Args, saveAndContinueAction);

				}, null);
		}


		private static void Process(
			AutoGrid AutoGrid, string docId, string type, AviaDocumentProcessArgs args, 
			AviaProcessSaveAndContinueAction saveAndContinueAction
		)
		{
			Store store = AutoGrid.store;

			args.AllowSaveAndContinue = saveAndContinueAction != null;
			AviaDocumentProcessForm form = new AviaDocumentProcessForm(args);

			form.Saved += delegate(object response)
			{
				AviaDocumentProcessDto[] dtos = (AviaDocumentProcessDto[])response;
				ArrayList ids = new ArrayList();

				foreach (AviaDocumentProcessDto dto in dtos)
					ids.Add(dto.Id);

				if (saveAndContinueAction != null && form.SaveAndContinue)
					saveAndContinueAction(form, store.indexOf(AutoGrid.SelectionModel.getSelected()), ids);

				AutoGrid.Reload(false);
			};

			form.Open(docId, type);
		}

		private void VoidDocuments()
		{
			VoidObjects(AviaService.CanUpdate, AviaService.ChangeVoidStatus);
		}

		private void PrintDocuments()
		{
			string value = Json.Stringify(GetSelectedIds());

			ReportLoader.Load(string.Format("print/ticket/Tickets_{0}.pdf", Date.Now.Format("Y-m-d_H-i-s")), new Dictionary("tickets", value));
		}

		protected override void OnStoreLoad(Store sender, Record[] records, object options)
		{
			if (Script.IsNullOrUndefined(_form))
				return;

			FindUnprocessedDocument(records);
		}

		private void FindUnprocessedDocument(Record[] records)
		{
			while (_selectedIndex < records.Length)
			{
				Record record = records[_selectedIndex];

				if (((bool)record.get("RequiresProcessing")) && !_processedDocumentIds.Contains(record.id))
				{
					GenericService.CanUpdate("AviaDocument", record.id,
						delegate(object res)
						{
							if (!(bool)res)
							{
								++_selectedIndex;

								FindUnprocessedDocument(records);

								return;
							}

							_form.SaveAndContinue = true;

							AutoGrid.SelectionModel.selectRow(_selectedIndex);

							_form.Open(record.id, (string)Type.GetField(record.data, ObjectPropertyNames.ObjectClass));

							_form = null;

						}, null);

					return;
				}
				++_selectedIndex;
			}

			_form.CloseWindow();

			_form = null;
		}


		private Action _handleButton;
		private Action _printButton;

		private AviaDocumentProcessForm _form;
		private int _selectedIndex;
		private ArrayList _processedDocumentIds;
	}
}