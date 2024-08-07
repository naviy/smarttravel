﻿using System.Linq;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{


	[RU("Провайдер дополнительных услуг", "Провайдеры дополнительных услуг"), Icon("")]
	public class GenericProductProvider : Domain.EntityQuery<Organization>
	{
		protected override IQueryable<Organization> GetQuery()
		{
			return db.Organizations.Where(a => a.IsGenericProductProvider);
		}

		public override void CalculateDefaults(Organization r)
		{
			r.IsGenericProductProvider = true;
		}
	}


	partial class Domain
	{
		public GenericProductProvider GenericProductProviders { get; set; }
	}

}