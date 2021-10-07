using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{


	[DataContract(Name = "Country")]
	[XmlType(TypeName = "Country")]
	public class CountryContract : Entity3Contract
	{
		public CountryContract() { }

		public CountryContract(Country r) : base(r)
		{
			TwoCharCode = r.TwoCharCode;
			ThreeCharCode = r.ThreeCharCode;
		}

		[DataMember]
		public string TwoCharCode { get; set; }
		[DataMember]
		public string ThreeCharCode { get; set; }

		public static implicit operator CountryContract(Country r)
			=> new CountryContract(r);

	}

}
