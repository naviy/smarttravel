using System;
using System.Html;
using System.Serialization;

using Ext.data;

using LxnBase.Data;
using LxnBase.UI.AutoControls;


namespace LxnBase.UI.Controls
{

	public static class ObjectLink
	{

		public static string Render(object id, object text, string type)
		{
			return 
				"<a href='#' class='object-link' onclick='" + typeof(ObjectLink).FullName + 
				".viewObject(event, " + Json.Stringify(type) + ", " + Json.Stringify(id) + ")'>"+ text + "</a>";
		}

		public static string RenderInfo(Reference info)
		{
			return Script.IsValue(info) ? Render(info.Id, info.Name, info.Type) : "";
		}

		public static string RenderInfos(Reference[] infos)
		{
			if (Script.IsNullOrUndefined(infos))
				return string.Empty;

			string s = "";

			foreach (Reference info in infos)
			{
				s += RenderInfo(info) + " ";
			}

			return s;
		}

		public static GridRenderDelegate ReferencesRenderer(string columnName)
		{
			return delegate(object value, object metadata, Ext.data.Record record, int rowIndex, int colIndex, Store store)
			{
				return RenderInfos(record.get(columnName) as Reference[]);
			};
		}

		public static string RenderArray(object[] values)
		{
			return Render(values[Reference.IdPos], values[Reference.NamePos], (string) values[Reference.TypePos]);
		}

		public static string RenderValue(object value)
		{
			object[] arr = value as object[];
			if (arr != null)
				return RenderArray(arr);

			Reference info = (Reference)value;
			return RenderInfo(info);
		}

		public static void ViewObject(ElementEvent e, string type, object id)
		{
			if (e.CtrlKey)
			{
				e.CancelBubble = true;
				e.ReturnValue = false;
			}

			FormsRegistry.ViewObject(type, id, e.CtrlKey);
		}
	}

}