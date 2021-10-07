using System;
using System.Collections;

using Ext;
using Ext.grid;

using LxnBase;
using LxnBase.Data;
using LxnBase.UI;
using LxnBase.UI.AutoControls;

using Luxena.Travel.Controls;
using Luxena.Travel.Services;

using Button = Ext.Button;
using ButtonConfig = Ext.ButtonConfig;
using Record = Ext.data.Record;


namespace Luxena.Travel
{


	public class GdsFileListTab : EntityListTab
	{

		static GdsFileListTab()
		{
			RegisterList("GdsFile", typeof(GdsFileListTab));
		}

		public GdsFileListTab(string tabId, ListArgs args) : base(tabId, args) { }

		protected override void PreInitialize()
		{
			base.PreInitialize();
			Entity = se = dsm.GdsFile;
		}

		private GdsFileSemantic se;


		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);

			if (args.BaseRequest == null)
				args.BaseRequest = new RangeRequest();

			if (args.BaseRequest.Sort == null)
			{
				args.BaseRequest.Sort = "TimeStamp";
				args.BaseRequest.Dir = "DESC";
				args.BaseRequest.HiddenProperties = new string[] { "Content" };
			}
		}


		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.TimeStamp,
				se.Name,
				se.FileType,
				se.ImportResult,
				se.ImportOutput,
				se.Content.ToColumn(true),

				se.CreatedOn,
				se.CreatedBy,
				se.ModifiedOn,
				se.ModifiedBy,
			});
		}

		protected override void OnAddToolbarButtons(ArrayList toolbarItems, AutoGrid autoGrid)
		{
			OperationStatus applyActionStatus = GetOperationStatus(ReimportActionName);

			if (!applyActionStatus.Visible) return;

			_reimportButton = new Button(new ButtonConfig()
				.text(Res.GdsFile_Reimport_Text)
				.handler(new AnonymousDelegate(Reimport))
				.disabled(applyActionStatus.IsDisabled)
				.custom("tooltip", applyActionStatus.DisableInfo)
				.ToDictionary());

			toolbarItems.Add(new ToolbarSeparator());
			toolbarItems.Add(_reimportButton);
		}

		private void Reimport()
		{
			Record[] records = (Record[])AutoGrid.SelectionModel.getSelections();

			ArrayList ids = new ArrayList();

			for (int i = 0; i < records.Length; i++)
				ids.Add(records[i].id);

			GdsFileService.Reimport((object[]) ids,(RangeRequest) AutoGrid.store.baseParams,
				delegate(object result)
				{
					RangeResponse response = (RangeResponse)((object[])result)[1];

					((WebServiceProxy)AutoGrid.store.proxy).SetResponse(response);

					AutoGrid.Reload(false);

					Array data = (Array)((object[])result)[0];

					string message = StringUtility.GetNumberText(data.Length, Res.GdsFile_Imported1, Res.GdsFile_Imported2, Res.GdsFile_Imported3);

					if (Script.IsNullOrUndefined(response.SelectedRow))
						MessageRegister.Info(AutoGrid.ListConfig.Caption, message, BaseRes.AutoGrid_NotDisplay_Msg);
					else
					{
						MessageRegister.Info(AutoGrid.ListConfig.Caption, message);

						AutoGrid.SelectionModel.selectRow(response.SelectedRow);
					}

				}, null);
		}

		private Button _reimportButton;

		public const string ReimportActionName = "ReimportGdsDocuments";
	}
}