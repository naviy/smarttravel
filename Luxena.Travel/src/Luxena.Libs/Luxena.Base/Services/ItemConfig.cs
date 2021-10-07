using System.Collections.Generic;
using Luxena.Base.Data;
using Luxena.Base.Serialization;


namespace Luxena.Base.Services
{
	[DataContract]
	public class ItemConfig
	{
		public ColumnConfig[] Columns { get; set; }

		public string Caption { get; set; }

		public string ListCaption { get; set; }

		public OperationStatus IsListAllowed { get; set; }

		public OperationStatus IsCreationAllowed { get; set; }

		public OperationStatus IsCopyingAllowed { get; set; }

		public OperationStatus IsEditAllowed { get; set; }

		public OperationStatus IsRemovingAllowed { get; set; }

		public Dictionary<string, OperationStatus> CustomActionPermissions { get; set; }
	}
}