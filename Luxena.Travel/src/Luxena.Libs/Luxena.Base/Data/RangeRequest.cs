using System.Collections.Generic;

using Luxena.Base.Serialization;


namespace Luxena.Base.Data
{

	[DataContract]
	public class RangeRequest
	{
		public string ClassName { get; set; }

		public string[] NamedFilters { get; set; }

		public PropertyFilter[] Filters { get; set; }

		[ContractMember("query")]
		public string Query { get; set; }

		public string GeneralFilter { get; set; }

		public string[] VisibleProperties { get; set; }

		public string[] HiddenProperties { get; set; }

		[ContractMember("start")]
		public int Start { get; set; }

		[ContractMember("limit")]
		public int Limit { get; set; }

		[ContractMember("sort")]
		public string Sort { get; set; }

		[ContractMember("dir")]
		public string Dir { get; set; }

		public object PositionableObjectId { get; set; }

		[ContractMember("params")]
		public Dictionary<string, object> Params { get; set; }

	}

}