using System.Runtime.CompilerServices;

using Ext.data;


namespace LxnBase.Data
{
	[Imported]
	public class GenericStore : Store
	{
		public GenericStore(object config) : base(config)
		{
		}

		public int Start
		{
			get { return 0; }
			set { }
		}

		public int Limit
		{
			get { return 0; }
			set { }
		}

		public string SortField
		{
			get { return null; }
			set { }
		}

		public string SortDirection
		{
			get { return null; }
			set { }
		}

		public string SearchMode
		{
			get { return null; }
			set { }
		}

		public string SuggestionQuery
		{
			get { return null; }
			set { }
		}

		public PropertyFilter[] Filter
		{
			get { return null; }
			set { }
		}
	}
}