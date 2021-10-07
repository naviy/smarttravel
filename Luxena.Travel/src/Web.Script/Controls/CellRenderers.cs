using System;

using Ext.data;

using Record = Ext.data.Record;


namespace Luxena.Travel
{

	public static class CellRenderers
	{

		public static object Right(object value, object metadata, Record record, int index, int colIndex, Store store)
		{
			if (Script.IsValue(value))
				return string.Format("<div style='text-align: right'>{0}</div>", value);

			return null;
		}

	}

}
