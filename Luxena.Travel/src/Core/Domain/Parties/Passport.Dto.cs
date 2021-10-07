using System;

using Luxena.Domain.Contracts;


namespace Luxena.Travel.Domain
{

	public partial class PassportDto : EntityContract
	{

		public string Number { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public Person.Reference Owner { get; set; }

		public int? Gender { get; set; }

		public string GenderString { get; set; }

		public Country.Reference Citizenship { get; set; }

		public Country.Reference IssuedBy { get; set; }

		public string IssuedByCode { get; set; }

		public DateTime? Birthday { get; set; }

		public DateTime? ExpiredOn { get; set; }
		public int? ExpiredDays { get; set; }

		public string Note { get; set; }

		public string AmadeusString { get; set; }

		public string GalileoString { get; set; }

	}


	public partial class PassportContractService : EntityContractService<Passport, Passport.Service, PassportDto>
	{
		public PassportContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Number = r.Number;

				c.FirstName = r.FirstName;
				c.MiddleName = r.MiddleName;
				c.LastName = r.LastName;

				c.Owner = r.Owner;

				c.Gender = (int?)r.Gender;

				if (r.Gender.HasValue)
					c.GenderString = r.Gender.Value.ToDisplayString();

				c.Citizenship = r.Citizenship;
				c.IssuedBy = r.IssuedBy;

				if (r.IssuedBy != null)
					c.IssuedByCode = r.IssuedBy.ThreeCharCode;

				c.Birthday = r.Birthday;
				c.ExpiredOn = r.ExpiredOn;
				c.ExpiredDays = r.ExpiredOn == null ? (int?)null : (r.ExpiredOn.Value - DateTime.Today).Days;

				c.Note = r.Note;

				c.AmadeusString = r.AmadeusString;
				c.GalileoString = r.GalileoString;
			};

			EntityFromContract += (r, c) =>
			{
				r.Number = c.Number + db;

				r.FirstName = c.FirstName + db;
				r.MiddleName = c.MiddleName + db;
				r.LastName = c.LastName + db;

				r.Citizenship = c.Citizenship + db;
				r.Birthday = c.Birthday;

				if (c.Gender != null)
					r.Gender = (Gender)c.Gender;

				r.IssuedBy = c.IssuedBy + db;
				r.ExpiredOn = c.ExpiredOn;

				r.Note = c.Note + db;
			};
		}

		public PassportValidationResponse ValidatePassengerPassports(AviaDocument ticket, Person passenger, bool isGdsPassportNull)
		{
			Passport passport;
			var result = db.AviaDocument.ValidatePassengerPassports(ticket, passenger, isGdsPassportNull, out passport);

			return new PassportValidationResponse
			{
				Passport = New(passport),
				PassportValidationResult = result
			};
		}

		public PassportValidationResponse ValidatePassengerPassports(AviaDocument ticket)
		{
			return ValidatePassengerPassports(ticket, ticket.Passenger, false);
		}

		public PassportValidationResponse ValidatePassengerPassport(string ticketId, string passengerId, bool isGdsPassportNull)
		{
			return db.Commit(() =>
			{
				var passenger = db.Person.By(passengerId);
				var document = db.AviaDocument.By(ticketId);
				return ValidatePassengerPassports(document, passenger, isGdsPassportNull);
			});
		}
	}

}