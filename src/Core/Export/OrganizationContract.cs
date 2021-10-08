using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{


	[DataContract(Name = "Organization")]
	[XmlType(TypeName = "Organization")]
	public class OrganizationContract : PartyContract
	{

		public OrganizationContract(Organization r) : base(r)
		{
			Code = r.Code;
		}

		[DataMember] public string Code  { get; set; }


		//[DataMember] public string BonusCardNumber { get; set; }

	}

}
