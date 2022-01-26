using System;
using System.Collections.Generic;
using System.Text;

using Luxena.Base.Metamodel;




namespace Luxena.Travel.Domain
{



	//===g






	[RU("Персона", "Персоны")]
	public partial class Person : Party
	{

		//---g



		//		[AnnotationSetup]
		//		public static void AnnotationSetup(DataAnnotationBag<Person> bag)
		//		{
		//			bag.For(a=> a.Title)
		//		}


		public override PartyType Type => PartyType.Person;


		[RU("Дата рождения")]
		public virtual DateTime? Birthday { get; set; }

		public virtual Organization Organization { get; set; }

		[RU("Должность")]
		public virtual string Title { get; set; }


		public virtual IList<Passport> Passports => _passports;


		[Hidden(true)]
		public virtual string MilesCardsString { get; set; }

		public virtual IList<MilesCard> MilesCards => _milesCards;



		//---g



		public virtual void AddPassport(Passport passport)
		{
			passport.Owner = this;

			_passports.Add(passport);
		}



		public virtual void RemovePassport(Passport passport)
		{
			passport.Owner = null;

			_passports.Remove(passport);
		}



		public virtual void AddMilesCard(MilesCard card)
		{
			card.Owner = this;

			_milesCards.Add(card);

			MilesCardsString = GetMilesCardString();
		}



		public virtual void RemoveMilesCard(MilesCard card)
		{
			card.Owner = null;

			_milesCards.Remove(card);

			MilesCardsString = GetMilesCardString();
		}



		public virtual void ClearMilesCards()
		{
			_milesCards.Clear();

			MilesCardsString = null;
		}



		public virtual void ClearPassports()
		{
			_passports.Clear();
		}



		private string GetMilesCardString()
		{

			if (MilesCards.Count == 0)
				return null;


			var builder = new StringBuilder();
			var separator = string.Empty;


			foreach (var card in _milesCards)
			{
				builder.Append(separator).Append(card.Number);

				separator = ", ";
			}


			return builder.ToString();

		}



		//---g



		private readonly IList<Passport> _passports = new List<Passport>();
		private readonly IList<MilesCard> _milesCards = new List<MilesCard>();



		//---g

	}






	//===g






	public partial class PersonListDetailDto : PartyListDetailDto
	{
		public string Title { get; set; }
	}






	public partial class PersonListDetailContractService : PartyListDetailContractService<Person, Person.Service, PersonListDetailDto>
	{

		public PersonListDetailContractService()
		{
			ContractFromEntity += (r, c) =>
			{
				c.Title = r.Title;
			};
		}

	}






	//===g



}