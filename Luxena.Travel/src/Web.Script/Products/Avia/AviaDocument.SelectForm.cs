using System;
using System.Collections;
using System.Runtime.CompilerServices;

using Ext.grid;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.AutoForms;

using ColumnConfig=LxnBase.Services.ColumnConfig;


namespace Luxena.Travel
{

	public class AviaDocumentSelectForm : AutoSelectForm
	{
		static AviaDocumentSelectForm()
		{
			FormsRegistry.RegisterSelect(ClassNames.AviaDocument, SelectObject);
		}

		private static void SelectObject(SelectArgs args)
		{
			ConfigManager.GetListConfig(args.Type,
				delegate(ListConfig config)
				{
					new AviaDocumentSelectForm(args, config).Open();
				});
		}

		public AviaDocumentSelectForm(SelectArgs args, ListConfig config) : base(args, config)
		{
			Width = 850;
			Height = 450;
		}

		public override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			args.ForcedProperties = new string[] { "RequiresProcessing", "IsVoid" };
			args.ColumnsConfig = CreateColumnsConfig();

			config.view(new ProductGridView((Column[]) args.ColumnsConfig));
		}

		private ArrayList CreateColumnsConfig()
		{
			ArrayList columns = new ArrayList();

			columns.Add(GetColumnConfig("IssueDate", false, 70));
			columns.Add(GetColumnConfig("Type", false, 45));
			columns.Add(GetColumnConfig("Producer", true));
			columns.Add(GetColumnConfig("AirlinePrefixCode", false, 55));
			columns.Add(GetColumnConfig("Number", false, 70, new RenderDelegate(delegate(object value) { return value; })));
			columns.Add(GetColumnConfig("ConjunctionNumbers", true));
			columns.Add(GetColumnConfig("PassengerName", false, 145));
			columns.Add(GetColumnConfig("Intermediary", true));
			columns.Add(GetColumnConfig("Customer", false, 140));
			columns.Add(GetColumnConfig("PaymentForm", true));
			columns.Add(GetColumnConfig("EqualFare", false, 85));
			columns.Add(GetColumnConfig("FeesTotal", false, 85));
			columns.Add(GetColumnConfig("Total", false, 85));
			columns.Add(GetColumnConfig("ServiceFee", true));
			columns.Add(GetColumnConfig("Discount", true));
			columns.Add(GetColumnConfig("GrandTotal", true, 95));
			columns.Add(GetColumnConfig("Commission", true));
			columns.Add(GetColumnConfig("BookerOffice", true));
			columns.Add(GetColumnConfig("BookerCode", true));
			columns.Add(GetColumnConfig("Booker", true));
			columns.Add(GetColumnConfig("TicketerOffice", true));
			columns.Add(GetColumnConfig("TicketerCode", true));
			columns.Add(GetColumnConfig("Ticketer", true));
			columns.Add(GetColumnConfig("Seller", true, 110));
			columns.Add(GetColumnConfig("Originator", true));
			columns.Add(GetColumnConfig("Origin", true));
			columns.Add(GetColumnConfig("PnrCode", true));
			columns.Add(GetColumnConfig("AirlinePnrCode", true));
			columns.Add(GetColumnConfig("TourCode", true));
			columns.Add(GetColumnConfig("IsProcessed", true, 70));
			columns.Add(GetColumnConfig("IsVoid", true, 70));
			columns.Add(GetColumnConfig("RequiresProcessing", true, 70));

			columns.Add(new Dictionary(
				"id", ObjectPropertyNames.Id,
				"header", "Id",
				"sortable", false,
				"dataIndex", ObjectPropertyNames.Id,
				"hidden", true
			));

			columns.Add(GetColumnConfig("CreatedOn", true));
			columns.Add(GetColumnConfig("CreatedBy", true));
			columns.Add(GetColumnConfig("ModifiedOn", true));
			columns.Add(GetColumnConfig("ModifiedBy", true));

			return columns;
		}

		[AlternateSignature]
		private extern Dictionary GetColumnConfig(string name, bool hidden);

		[AlternateSignature]
		private extern Dictionary GetColumnConfig(string name, bool hidden, int width);

		private Dictionary GetColumnConfig(string name, bool hidden, int width, Delegate renderer)
		{
			ColumnConfig columnConfig = GetColumnConfigByName(name);

			Ext.grid.ColumnConfig config = new Ext.grid.ColumnConfig()
				.id(columnConfig.Name)
				.header(columnConfig.Caption)
				.sortable(columnConfig.IsPersistent)
				.dataIndex(columnConfig.Name)
				.hidden(hidden);

			if ((renderer = renderer ?? ControlFactory.CreateRenderer(columnConfig)) != null)
				config.renderer(renderer);

			if (!Script.IsNullOrUndefined(width))
				config.width(width);

			return config.ToDictionary();
		}

		private ColumnConfig GetColumnConfigByName(string name)
		{
			ColumnConfig[] columnConfigs = ListConfig.Columns;

			for (int i = 0; i < columnConfigs.Length; i++)
				if (columnConfigs[i].Name == name)
					return columnConfigs[i];

			throw new Exception("Unknown column " + name);
		}

	}

}
