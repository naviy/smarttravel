using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{

	[DataContract(Name = "AviaMco", Namespace = "")]
	[XmlType(TypeName = "AviaMco")]
	public class AviaMcoContract : AviaDocumentContract
	{
		public AviaMcoContract() { }

		public AviaMcoContract(AviaMco r) : base(r)
		{
			Description = r.Description;
			ReissueFor = r.ReissueFor;
			InConnectionWith = r.InConnectionWith;
		}


		[DataMember] public string Description { get; set; }
		[DataMember] public ProductReference ReissueFor { get; set; }
		[DataMember] public ProductReference InConnectionWith { get; set; }

	}

}