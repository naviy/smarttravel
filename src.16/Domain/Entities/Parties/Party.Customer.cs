using System;
using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Заказчик", "Заказчики"), Icon("")]
	public class Customer : Domain.EntityQuery<Party>
	{
		protected override IQueryable<Party> GetQuery()
		{
			return db.Parties.Where(a => a.IsCustomer);
		}

		public override void CalculateDefaults(Party r)
		{
			r.IsCustomer = true;
		}
	}


	partial class Domain
	{
		public Customer Customers { get; set; }
	}


	[Localization(typeof(Customer)), Lookup(typeof(Customer))]
	public class CustomerAttribute : Attribute { }

}