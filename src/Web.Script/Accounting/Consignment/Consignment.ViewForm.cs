using System;

using Ext;
using Ext.util;

using LxnBase.Data;
using LxnBase.Services;
using LxnBase.UI;
using LxnBase.UI.AutoControls;
using LxnBase.UI.AutoForms;

using Luxena.Travel.Services;


namespace Luxena.Travel
{
	public class ConsignmentViewForm : AutoViewForm
	{
		static ConsignmentViewForm()
		{
			FormsRegistry.RegisterView(ClassNames.Consignment, ViewObject);
		}

		public static void ViewObject(string type, object id, bool newTab)
		{
			Tabs.Open(newTab, (string)id, delegate(string tabId) { return new ConsignmentViewForm(tabId, id, type); });
		}

		public ConsignmentViewForm(string tabId, object id, string type) : base(tabId, id, type)
		{
		}

		protected override void initComponent()
		{
			base.initComponent();

			_fieldPanel = new Panel(new PanelConfig()
				.autoHeight(true)
				.border(false)
				.ToDictionary());

			_itemsGrid = new ConsignmentItemGridControl(200, false);

			_itemsPanel = new Panel(new PanelConfig()
				.autoHeight(true)
				.border(false)
				.cls("consignment-items")
				.items(new Component[] { _itemsGrid })
				.ToDictionary());

			add(_fieldPanel);
			add(_itemsPanel);

			doLayout();
		}

		private ConsignmentDto Consignment
		{
			get { return (ConsignmentDto )Instance; }
		}

		protected override void GetInstance()
		{
			ConsignmentService.GetConsignment(Id, Load, delegate { Tabs.Close(this); });
		}

		protected override void OnLoad()
		{
			ColumnConfig[] columns = ItemConfig.Columns;

			string caption = null;

			StringBuilder template = new StringBuilder();

			template.Append("<div class='consignment-title'>");
			template.Append(DomainRes.Consignment);
			template.Append("</div>");

			foreach (ColumnConfig column in columns)
			{
				object val = Type.GetField(Consignment, column.Name);

				if (column.IsReference)
				{
					if (val is string)
						caption = (string)val;
					else if (val is Array)
						caption = (string)((Array)val)[Reference.NamePos];
				}

				if (Script.IsNullOrUndefined(val) || column.Hidden ||
					column.Name == "CreatedOn" ||
					column.Name == "CreatedBy" ||
					column.Name == "ModifiedOn" ||
					column.Name == "ModifiedBy")
					continue;

				RenderDelegate renderer = (RenderDelegate)ControlFactory.CreateRenderer(column);
				string text = (string)(renderer == null ? val : renderer.Invoke(val));
				text = text.Split("\n").Join("<br/>");

				template.Append("<div class='viewItem'>");

				template.Append(string.Format("<div class='itemCaption'>{0}</div>", column.Caption + ":"));
				template.Append(string.Format("<div class='itemValue'>{0}</div>", text));

				template.Append("</div>");
			}

			template.Append(GetIssuedConsignments());

			if (caption == null)
				caption = ItemConfig.Caption;

			setTitle(caption);

			new Template(template.ToString()).overwrite(_fieldPanel.body);

			if (Consignment.Items.Length > 0)
				_itemsGrid.SetInitialData(Consignment.Items);

			doLayout();
		}
		
		private string GetIssuedConsignments()
		{
			StringBuilder sb = new StringBuilder();

			if (Consignment.IssuedConsignments.Length == 0)
			{
				return string.Empty;
			}

			sb.Append("<div class='viewItem'>");

			sb.Append(string.Format(@"<div class='itemCaption'>{0}:</div>", DomainRes.Consignment_IssuedConsignments));

			foreach (IssuedConsignmentDto issuedConsignment in Consignment.IssuedConsignments)
			{
				string text = string.Format(
					@"<a href='javascript:void(0)' class='object-link' onclick='{0}.viewIssuedConsignment(""{1}"", ""{2}"")'><img src='static/style/travel/img/xls_ico.gif' class='printed_icon'/> {3}</a>",
					typeof(ConsignmentViewForm).FullName, issuedConsignment.Id, Consignment.Number,
					issuedConsignment.Number);

				sb.Append(string.Format(@"<div class='itemValue'>{0}({1})  </div>", text, Format.date(issuedConsignment.TimeStamp, "d.m.Y")));
			}

			sb.Append("</div>");

			return sb.ToString();
		}

		public static void ViewIssuedConsignment(string id, string consignmentNumber)
		{
			ReportPrinter.GetIssuedConsignment(id, consignmentNumber);
		}

		private ConsignmentItemGridControl _itemsGrid;
		private Panel _fieldPanel;
		private Panel _itemsPanel;
	}
}