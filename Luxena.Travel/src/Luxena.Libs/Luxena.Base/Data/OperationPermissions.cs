using System.Collections.Generic;
using Luxena.Base.Serialization;


namespace Luxena.Base.Data
{

	[DataContract]
	public class OperationPermissions
	{

		public OperationStatus CanList { get; set; }

		public OperationStatus CanCreate { get; set; }

		public OperationStatus CanUpdate { get; set; }

		public OperationStatus CanDelete { get; set; }

		public Dictionary<string, OperationStatus> CustomActionPermissions { get; set; }


		public static implicit operator OperationPermissions(OperationStatus a)
		{
			return new OperationPermissions { CanList = a};
		}

	}

}