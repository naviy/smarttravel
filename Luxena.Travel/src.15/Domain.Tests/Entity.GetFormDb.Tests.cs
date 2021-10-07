using System.Linq;

using Luxena.Domain;

using NUnit.Framework;


namespace Luxena.Travel.Domain.Tests
{

	[InitDomain]
	public class EntityGetFormDbTests : DomainTest
	{

		[Test]
		public void Airport01()
		{

			var r = db.Airports.First(a => a.Country != null);

			var airportName = r.Name;

			r.Name += "TEST";
			r.Country.Name += "TEST";

			var r2 = r.GetFromDb(db);

			Assert.AreEqual(airportName, r2.Name);
			Assert.AreEqual(r.Country.Name, r2.Country.Name);

			db.Commit(() =>
			{
				r.Save(db);
			});

			Assert.AreEqual(airportName + "TEST", r.Name);

		}

		[Test]
		public void Person02()
		{
			var r = db.Persons.First(a => a.MilesCards.Any());

			var newName = r.Name += "TEST";

			db.Commit(() =>
			{
				r.Save(db);
			});

			Assert.AreEqual(newName, r.Name);
		}

		[Test]
		public void MilesCard02()
		{
			var r = db.MilesCards.First();

			var newName = r.Number += "TEST";

db.Commit(() =>
{
	r.Save(db);
			});

			Assert.AreEqual(newName, r.Number);
		}

		[Test]
		public void Passport02()
		{
			var r = db.Passports.First();

			var newName = r.Number += "TEST";

			db.Commit(() =>
			{
				r.Save(db);
			});

			Assert.AreEqual(newName, r.Number);
		}

	}

}
