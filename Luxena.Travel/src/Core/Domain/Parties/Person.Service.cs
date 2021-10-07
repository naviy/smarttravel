using System.Linq;
using System.Text.RegularExpressions;


namespace Luxena.Travel.Domain
{

	partial class Person
	{

		public new class Service : Service<Person>
		{

			public Person ByPassengerName(AviaDocument document)
			{
				return document?.Passengers.Select(a => ByPassengerName(a.PassengerName)).By(a => a != null);
			}

			public Person ByPassengerName(string passengerName)
			{
				if (passengerName.No())
					return null;

				var parts = passengerName.Split('/');

				var lastName = parts[0];
				var firstName = string.Empty;

				if (parts.Length > 1)
					firstName = parts[1];

				if (firstName.Yes())
					firstName = _reByPassengerName.Replace(firstName, string.Empty);

				var name = (lastName + " " + firstName).Trim();

				return ByName(name);
			}

			static readonly Regex _reByPassengerName = new Regex(@"(MR|MRS|MSTR|MISS)$", RegexOptions.Compiled);

		}

	}

}