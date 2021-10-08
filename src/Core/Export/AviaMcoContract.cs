using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{

	[DataContract(Name = "Insurance", Namespace = "")]
	[XmlType(TypeName = "Insurance")]
	public class InsuranceContract : ProductContract
	{

		public InsuranceContract() { }

		public InsuranceContract(Insurance r) : base(r)
		{
			Producer = r.Producer;
		}


		[DataMember]
		public PartyReference Producer { get; set; }

	}

}