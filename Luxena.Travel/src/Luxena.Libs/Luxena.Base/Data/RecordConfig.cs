using System.Collections.Generic;


namespace Luxena.Base.Data
{

	public class RecordConfig
	{
		public RecordConfig()
		{
			IncludeIdentifier = true;
		}

		public bool IncludeType { get; set; }

		public bool IncludeIdentifier { get; set; }

		public bool IncludeVersion { get; set; }

		public bool IncludeDisplayString { get; set; }

		public IList<ColumnConfig> Columns => _columns;

		public RecordConfig Add(string name)
		{
			return Add(name, null);
		}

		public RecordConfig AddLink(string name)
		{
			return Add(name, _link);
		}

		public RecordConfig Add(string name, RecordConfig config)
		{
			_columns.Add(new ColumnConfig
			{
				Name = name,
				RecordConfig = config
			});

			return this;
		}

		private static readonly RecordConfig _link = new RecordConfig
		{
			IncludeType = true,
			IncludeDisplayString = true
		};

		private readonly List<ColumnConfig> _columns = new List<ColumnConfig>();
	}

}