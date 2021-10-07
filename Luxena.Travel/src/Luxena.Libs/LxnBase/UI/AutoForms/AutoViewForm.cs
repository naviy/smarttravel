using System;

using Ext;

using LxnBase.Data;
using LxnBase.Net;
using LxnBase.Services;
using LxnBase.UI.AutoControls;



namespace LxnBase.UI.AutoForms
{


	public class AutoViewForm : BaseClassViewForm
	{

		public AutoViewForm(string tabId, object id, string type) : base(tabId, id, type)
		{
		}


		protected override void GetInstance()
		{
			GenericService.Get(InstanceType, Id, true, Load, OnInstanceFailure);
		}


		protected override void OnLoad()
		{
			ColumnConfig[] columns = ItemConfig.Columns;

			string caption = null;

			StringBuilder template = new StringBuilder();

			foreach (ColumnConfig t in columns)
			{
				object val = Type.GetField(Instance, t.Name);

				if (t.IsReference)
				{
					if (val is string)
						caption = (string)val;
					else if (val is Array)
						caption = (string)((Array)val)[Reference.NamePos];
				}

				if (Script.IsNullOrUndefined(val) || t.Hidden || t.Type == TypeEnum.Bool && !(bool)val)
					continue;

				RenderDelegate renderer = (RenderDelegate)ControlFactory.CreateRenderer(t);
				string text = (string)(renderer == null ? val : renderer.Invoke(val));
				text = text.Split("\n").Join("<br/>");

				template.Append("<div class='viewItem'>");

				template.Append(string.Format("<div class='itemCaption'>{0}</div>", (t.Caption ?? t.Name) + ":"));
				template.Append(string.Format("<div class='itemValue'>{0}</div>", text));

				template.Append("</div>");
			}


			if (caption == null)
				caption = ItemConfig.Caption ?? _type;

			setTitle(caption);

			new Template(template.ToString()).overwrite(body, Instance);

			doLayout();
		}


		private void OnInstanceFailure(WebServiceFailureArgs args)
		{
			Close();
		}

	}



}