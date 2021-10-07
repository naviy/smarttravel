using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Страна", "Страны"), Icon("globe")]
	[SupervisorPrivileges]
	public partial class Country : Entity3
	{

		[SemanticSetup]
		public static void SemanticSetup(SemanticSetup<Country> sm)
		{
			sm.For(a => a.Name)
				.Length(16);
		}

		[RU("Код (2-х сим.)"), String(2)]
		public string TwoCharCode { get; set; }

		[RU("Код (3-х сим.)"), String(3)]
		public string ThreeCharCode { get; set; }

		public virtual ICollection<Airport> Airports { get; set; }

		//public virtual ICollection<Passport> Passports_Citizenship { get; set; }

		//public virtual ICollection<Passport> Passports_IssuedBy { get; set; }

		//public virtual ICollection<Product> Products { get; set; }

	}


	partial class Domain
	{
		public DbSet<Country> Countries { get; set; }
	}


	public static class CountrySetExtentions
	{
		public static Country ByCode(this IQueryable<Country> query, string code)
		{
			return
				code.No() ? null :
				code.Length == 2 ? query.By(a => a.TwoCharCode == code) :
				code.Length == 3 ? query.By(a => a.ThreeCharCode == code) :
				null;
		}
	}


}