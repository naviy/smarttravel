using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{


	[DataContract(Name = "Penalty")]
	[XmlType(TypeName = "Penalty")]
	public class PenalizeOperationContract
	{
		public PenalizeOperationContract() { }

		public PenalizeOperationContract(PenalizeOperation operation)
		{
			Type = operation.Type;
			Status = operation.Status;
			Description = operation.Description;
		}

		[DataMember] public PenalizeOperationType Type { get; set; }
		[DataMember] public PenalizeOperationStatus Status { get; set; }
		[DataMember] public string Description { get; set; }
	}
}