using System.Collections.Generic;
using System.Data.Entity;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	[RU("Банковский счёт", "Банковские счёта"), Small]
	[SupervisorPrivileges]
	public partial class BankAccount : Entity3D
	{

		[RU("Использовать по умолчанию")]
		public bool IsDefault { get; set; }

		[Patterns.Note]
		public string Note { get; set; }


		//public virtual ICollection<Party> Parties_DefaultBankAccount { get; set; }

		//public virtual ICollection<Order> Orders { get; set; }
	}


	partial class Domain
	{
		public DbSet<BankAccount> BankAccounts { get; set; }
	}

}