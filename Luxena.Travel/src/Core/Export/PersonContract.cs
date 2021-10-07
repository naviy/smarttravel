using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{


	[DataContract(Name = "Person")]
	[XmlType(TypeName = "Person")]
	public class PersonContract : PartyContract
	{

		public PersonContract(Person r) : base(r)
		{
			Title = r.Title;
			Birthday = r.Birthday;
			Organization = r.Organization;
		}

		[DataMember] public string Title { get; set; }
		[DataMember] public DateTime? Birthday { get; set; }
		[DataMember] public PartyReference Organization { get; set; }

	}

}
