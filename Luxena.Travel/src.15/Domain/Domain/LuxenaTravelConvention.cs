using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

using Luxena.Domain;


namespace Luxena.Travel.Domain
{

	public class EntityBaseAttribute : Attribute { }



	public class LuxenaTravelConvention : Convention
	{

		public LuxenaTravelConvention()
		{
			var reTblName = new Regex("([a-z])([A-Z])");

			Types<Domain.Entity>().Configure(c =>
			{
				var baseType = c.ClrType.BaseType;
				if (baseType != null && baseType.IsAbstract && !baseType.HasAttribute<EntityBaseAttribute>(false))
					return;

				var name = reTblName.Replace(c.ClrType.Name, "$1_$2");
				c.ToTable("lt_" + name.ToLower());

				c.Ignore(a => a.LastChangedPropertyName);
			});

			Properties()
				.Where(a => a.IsDbMapped())
				.Configure(c =>
				{
					var name = GetPropertyName(c.ClrPropertyInfo.Name);
					c.HasColumnName(name);
				});

		}

		public static string GetPropertyName(string name)
		{
			name = name.TrimEnd("Id");
			name = name.ToLower();

			return
				name == "date" ? "date_" :
				name == "number" ? "number_" :
				name == "order" ? "order_" :
				name;
		}

	}

}