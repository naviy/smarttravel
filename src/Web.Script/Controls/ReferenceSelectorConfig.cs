using System;

using Ext.data;
using Ext.form;

using LxnBase.Data;


namespace Luxena.Travel
{

	public class ReferenceSelectorConfig : ComboBoxConfig
	{
		public ReferenceSelectorConfig(Reference[] data) : base()
		{
			JsonStoreConfig storeConfig = new JsonStoreConfig().fields(new string[] { "Id", "Name" });

			if (Script.IsValue(data))
				storeConfig.data(data);

			this
				.store(new JsonStore(storeConfig.ToDictionary()))
				.mode("local")
				.editable(false)
				.displayField("Name")
				.valueField("Id")
				.triggerAction("all")
				.selectOnFocus(true);
		}
	}

}
