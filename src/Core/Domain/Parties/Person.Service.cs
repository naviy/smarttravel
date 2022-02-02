using System;
using System.Linq;
using System.Text.RegularExpressions;




namespace Luxena.Travel.Domain
{



	//===g






	partial class Person
	{

		//---g



		static readonly Regex _reByPassengerName = new Regex(@"(MR|MRS|MSTR|MISS)$", RegexOptions.Compiled);



		public static bool PassengerNamesIsEquals(string name1, string name2)
		{

			name1 = _reByPassengerName.Replace(name1, string.Empty).Clip();
			name2 = _reByPassengerName.Replace(name2, string.Empty).Clip();

			return name1.Equals(name2, StringComparison.InvariantCultureIgnoreCase);

		}



		//---g



		public new class Service : Service<Person>
		{

			//---g



			public Person ByPassengerName(AviaDocument document)
			{

				return document?.Passengers
					.Select(a => ByPassengerName(a.PassengerName))
					.By(a => a != null)
				;

			}



			public Person ByPassengerName(string passengerName)
			{

				if (passengerName.No())
					return null;


				var parts = passengerName.Split('/');

				var lastName = parts[0];

				var firstName = string.Empty;

				if (parts.Length > 1)
				{
					firstName = parts[1];
				}


				if (firstName.Yes())
				{
					firstName = _reByPassengerName.Replace(firstName, string.Empty);
				}


				var name = (lastName + " " + firstName).Trim();


				return ByName(name);

			}



			//---g

		}




		//---g

	}






	//===g



}