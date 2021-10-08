using System;
using System.Collections;

using Ext.data;
using Ext.grid;
using Ext.util;

using LxnBase.Data;
using LxnBase.UI.AutoControls;
using LxnBase.UI.Controls;

using Record = Ext.data.Record;


namespace Luxena.Travel
{

	public partial class ModificationListTab
	{

		protected override void OnInitGrid(AutoGridArgs args, EditorGridPanelConfig config)
		{
			base.OnInitGrid(args, config);

			args.ForcedProperties = new string[]
			{
				"InstanceType", "InstanceId",
			};

			args.SetDefaultSort("TimeStamp", "DESC");

		}

		protected override void CreateColumnConfigs()
		{
			AddColumns(new object[]
			{
				se.TimeStamp.ToColumn(false, 80),
				//	delegate(object value, object metadata, Record record, int index, int colIndex, Store store)
				//	{
				//		return ObjectLink.Render(record.get("Id"), Format.date((Date)value, "d.m.Y G:i:s"), "Modification");
				//	}
				//),
				se.Author.ToColumn(false, 100),
				se.Type.ToColumn(false, 50),
				se.InstanceType.ToColumn(true, 100),
				se.InstanceId.ToColumn(true, 100),
				se.InstanceString.ToColumn(false, 100,
					delegate(object value, object metadata, Record record, int index, int colIndex, Store store)
					{
						string type = record.get("InstanceType") as string;
						return ObjectLink.Render(record.get("InstanceId"), value ?? type, type);
					}
				),
				se.ItemsJson.ToColumn(false, 500,
					delegate(object value, object metadata, Record record, int index, int colIndex, Store store)
					{
						if (!Script.IsValue(value)) return "";

						ArrayList s = new ArrayList();
						ArrayList props = (ArrayList)Script.Eval((string)value);
						foreach (ArrayList prop in props)
						{
							s.Add(prop[0] + ": " + prop[1]);
						}

						return s.Join(", ");
					}
				),
			});
		}

	}


	public partial class ModificationEditForm
	{

		protected override void CreateControls()
		{
			Window.width = -1;

			Form.add(MainDataPanel(new object[]
			{
	
			}));
		}

	}

}