using System.Collections.Generic;
using Luxena.Base.Data;
using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public class ListConfig
	{
		public ColumnConfig[] Columns { get; set; }

		public string Caption { get; set; }

		public bool Filterable { get; set; }

		public OperationStatus IsCreationAllowed { get; set; }

		public OperationStatus IsCopyingAllowed { get; set; }

		public OperationStatus IsEditAllowed { get; set; }

		public OperationStatus IsRemovingAllowed { get; set; }

		public bool IsQuickEditAllowed { get; set; }

		public bool SingleSelect { get; set; }

		public Dictionary<string, OperationStatus> CustomActionPermissions { get; set; }
	}
}