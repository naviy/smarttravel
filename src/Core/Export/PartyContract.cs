using System.Runtime.Serialization;

using Luxena.Travel.Domain;


namespace Luxena.Travel.Export
{

	[DataContract]
	public class PartyContract
	{

		public PartyContract(Party r)
		{
			Id = r.Id.ToString();
			Type = r.Type;
			Name = r.Name;
			LegalName = r.LegalName;
			BonusCardNumber = r.BonusCardNumber;
			BonusAmount = r.BonusAmount;
			Phone1 = r.Phone1;
			Phone2 = r.Phone2;
			Email1 = r.Email1;
			Email2 = r.Email2;
			Fax = r.Fax;
			WebAddress = r.WebAddress;
			IsCustomer = r.IsCustomer;
			IsSupplier = r.IsSupplier;
			LegalAddress = r.LegalAddress;
			ActualAddress = r.ActualAddress;
			Note = r.Note;
		}


		[DataMember] public string Id { get; set; }
		[DataMember] public PartyType Type { get; set; }
		[DataMember] public string LegalName { get; set; }

		[DataMember] public string BonusCardNumber { get; set; }
		[DataMember] public decimal? BonusAmount { get; set; }
		
		[DataMember] public string Phone1 { get; set; }
		[DataMember] public string Phone2 { get; set; }
		[DataMember] public string Email1 { get; set; }
		[DataMember] public string Email2 { get; set; }
		[DataMember] public string Fax { get; set; }
		[DataMember] public string WebAddress { get; set; }
		[DataMember] public bool IsCustomer { get; set; }
		[DataMember] public bool IsSupplier { get; set; }
		[DataMember] public string LegalAddress { get; set; }
		[DataMember] public string ActualAddress { get; set; }
		[DataMember] public string Note { get; set; }
		[DataMember] public string Name { get; set; }
		
	}

}