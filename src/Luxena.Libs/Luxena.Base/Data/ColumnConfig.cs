using System.Diagnostics;


namespace Luxena.Base.Data
{

	[DebuggerDisplay("{Name}")]
	public class ColumnConfig
	{
		public string Name { get; set; }

		public RecordConfig RecordConfig { get; set; }
	}

}