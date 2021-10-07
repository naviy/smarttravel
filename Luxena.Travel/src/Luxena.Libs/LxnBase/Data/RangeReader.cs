using System.Runtime.CompilerServices;

using Ext.data;


namespace LxnBase.Data
{
	[Imported]
	public class RangeReader : JsonReader
	{
		public RangeReader()
		{
		}

		public RangeReader(object meta) : base(meta)
		{
		}

		public RangeReader(object meta, object recordType) : base(meta, recordType)
		{
		}

		public object ReadData(object obj)
		{
			return null;
		}
	}
}