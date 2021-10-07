using System;


namespace Luxena.Travel.Domain
{

	public partial class PersonDto : PartyDto
	{

		public string Title { get; set; }

		public DateTime? Birthday { get; set; }

		public Organization.Reference Organization { get; set; }

		public PassportDto[] Passports { get; set; }

		public MilesCardDto[] MilesCards { get; set; }

	}


	public partial class PersonContractService : PartyContractService<Person, Person.Service, PersonDto>
	{
		public PersonContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Title = r.Title;

				c.Birthday = r.Birthday;

				c.Organization = r.Organization;

				c.Passports = dc.Passport.New(r.Passports);

				c.MilesCards = dc.MilesCard.New(r.MilesCards);
			};

			EntityFromContract += (r, c) =>
			{
				r.Title = c.Title + db;

				r.Birthday = c.Birthday + db;

				r.Organization = c.Organization + db;

				dc.Passport.Update(
					r, r.Passports, c.Passports,
					r.AddPassport, r.RemovePassport
				);

				dc.MilesCard.Update(
					r, r.MilesCards, c.MilesCards,
					r.AddMilesCard, r.RemoveMilesCard
				);
			};
		}
	}

}