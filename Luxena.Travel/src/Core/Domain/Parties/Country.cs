using Luxena.Domain;




namespace Luxena.Travel.Domain
{

	[RU("Страна", "Страны")]
	[SupervisorPrivileges]
	public partial class Country : Entity3
	{

		[RU("Код (2-х сим.)")]
		public virtual string TwoCharCode { get; set; }

		[RU("Код (3-х сим.)")]
		public virtual string ThreeCharCode { get; set; }
		
		[Patterns.Note, Text(10)]
		public virtual string Note { get; set; }


		public class Service : Entity3Service<Country>
		{
			public Country ByCode(string code)
			{
				return
					code.Length == 2 ? By(c => c.TwoCharCode == code) :
					code.Length == 3 ? By(c => c.ThreeCharCode == code) :
					null;
			}
		}

	}

}