using System;
using System.Collections.Generic;
using System.Reflection;

using Luxena.Base.Data.NHibernate;
using Luxena.Base.Data.NHibernate.Mapping;
using Luxena.Base.Data.NHibernate.Type;
using Luxena.Domain.NHibernate;
using Luxena.Travel.Domain;

using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;


namespace Luxena.Travel.Config
{

	public static class ConfigurationBuilder
	{

		public static Configuration Build(string config)
		{
			var cfg = new Configuration();

			if (config.No())
			{
				cfg.Configure();
			}
			else
			{
				cfg.Configure(config);
			}

			cfg.SetNamingStrategy(new NamingStrategy
			{
				TablePrefix = "lt_",
				TableWordSeparator = "_",
				KeywordReplacements = new Dictionary<string, string>
				{
					{ "order", "order_" },
					{ "number", "number_" },
					{ "user", "user_" },
					{ "comment", "comment_" },
					{ "date", "date_" }
				}
			});

			var mapper = new ModelMapper();

			mapper.BeforeMapJoinedSubclass += (modelInspector, type, joinedSubclassCustomizer) =>
				joinedSubclassCustomizer.Key(k =>
				{
					k.Column("Id");
					k.ForeignKey(type.Name.ToLower() + "_fkey");
				});

			mapper.BeforeMapOneToOne += (modelInspector, member, propertyCustomizer) =>
			{
				var property = member.LocalMember as PropertyInfo;

				if (property == null || property.DeclaringType == null)
					return;

				propertyCustomizer.ForeignKey(string.Format("{0}_{1}_fkey", property.DeclaringType.Name, property.PropertyType.Name));
			};

			mapper.BeforeMapMap += (modelInspector, member, propertyCustomizer) =>
			{
				var property = member.LocalMember as PropertyInfo;

				if (property == null || property.DeclaringType == null)
					return;

				propertyCustomizer.Table(property.DeclaringType.Name + property.Name);
				propertyCustomizer.Key(k => k.Column(property.DeclaringType.Name));
			};

			mapper.BeforeMapManyToOne += (modelInspector, member, propertyCustomizer) =>
			{
				var property = member.LocalMember as PropertyInfo;

				if (property == null || property.DeclaringType == null)
					return;

				if (modelInspector.IsComponent(property.DeclaringType))
				{
					var columnName = member.PreviousPath.LocalMember.Name + "_" + member.LocalMember.Name;
					propertyCustomizer.Column(columnName);
					propertyCustomizer.ForeignKey(string.Format("{0}_{1}_fk", member.PreviousPath.LocalMember.DeclaringType.Name, columnName).ToLower());
					propertyCustomizer.Index(string.Format("{0}_{1}_idx", member.PreviousPath.LocalMember.DeclaringType.Name, columnName).ToLower());
				}
				else
				{
					propertyCustomizer.ForeignKey(string.Format("{0}_{1}_fkey", property.DeclaringType.Name, member.LocalMember.Name).ToLower());
					propertyCustomizer.Index(string.Format("{0}_{1}_idx", property.DeclaringType.Name, member.LocalMember.Name).ToLower());
				}
			};

			mapper.BeforeMapProperty += delegate(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
			{
				var property = member.LocalMember as PropertyInfo;

				if (property == null)
					return;

				if (property.PropertyType.Is<string>())
				{
					propertyCustomizer.Column(m => m.SqlType("citext2"));
				}
				else if (property.PropertyType.Is<DateTime>() || property.PropertyType.Is<DateTime?>())
				{
					propertyCustomizer.Type<UtcKindDateTimeType>();
				}
				else if (modelInspector.IsComponent(property.DeclaringType))
				{
					propertyCustomizer.Column(member.PreviousPath.LocalMember.Name + "_" + member.LocalMember.Name);
				}
			};

			mapper.AddMappings(typeof(ModificationMap).Assembly.GetTypes());
			mapper.AddMappings(typeof(CurrencyMap).Assembly.GetTypes());

			cfg.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

			cfg.AddAssembly(typeof(CurrencyMap).Assembly);


			cfg.LinqToHqlGeneratorsRegistry<LinqRegistry>(); 

			cfg.UpdateMetamodel();

			return cfg;
		}


		public class LinqRegistry : DomainLinqRegistry
		{
			public LinqRegistry()
			{
				Register((Party a) => a.NameForDocuments, r => r.LegalName != null && r.LegalName != "" ? r.LegalName : r.Name);
				//RegisterEntity<Party>();
			}
		}


	}

}