using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{

	[DataContract(Name = "Airport")]
	[XmlType(TypeName = "Airport")]
	public class AirportContract : Entity3Contract
	{
		public AirportContract() { }

		public AirportContract(Airport r) : base(r)
		{
			Code = r.Code;
			Country = r.Country;
			Settlement = r.Settlement;
			LocalizedSettlement = r.LocalizedSettlement;
		}


		[DataMember]
		public string Code { get; set; }
		[DataMember]
		public CountryContract Country { get; set; }
		[DataMember]
		public string Settlement { get; set; }
		[DataMember]
		public string LocalizedSettlement { get; set; }

		public static implicit operator AirportContract(Airport r)
			=> r == null ? null : new AirportContract(r);

	}

}
