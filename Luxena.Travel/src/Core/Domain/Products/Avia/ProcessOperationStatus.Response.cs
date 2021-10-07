using Luxena.Base.Data;
using Luxena.Base.Serialization;


namespace Luxena.Travel.Domain
{

	[DataContract]
	public class ProcessOperationPermissionsResponse : OperationPermissions
	{
		public AviaDocumentProcessArgs Args { get; set; }
	}

}