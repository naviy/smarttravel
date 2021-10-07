using System;
using System.Linq;

using Luxena.Base.Metamodel;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping;


namespace Luxena.Base.Data.NHibernate
{

	public static class ConfigurationExtensions
	{

		static ConfigurationExtensions()
		{
			Class.TypeResolver = new TypeResolver();
		}

		public static void UpdateMetamodel(this Configuration cfg)
		{
			foreach (var clazz in cfg.ClassMappings)
			{
				try
				{
					var cls = clazz.MappedClass.GetClass();

					cls.IsPersistent = true;

					if (clazz.IdentifierProperty != null)
					{
						var prop = cls.GetProperty(clazz.IdentifierProperty.Name);

						prop.IsRequired = true;
						prop.DataPath = clazz.IdentifierProperty.Name;
						prop.SetType(clazz.IdentifierProperty.Type.ReturnedClass);

						cls.IdentifierProperty = prop;
					}

					if (clazz.IsVersioned)
						cls.VersionProperty = cls.GetProperty(clazz.Version.Name);

					foreach (var property in clazz.PropertyClosureIterator)
					{
						Metamodel.Property prop;

						try
						{
							prop = cls.GetProperty(property.Name);
						}
						catch (MetamodelException)
						{
							continue;
						}

						if (!string.IsNullOrEmpty(prop.DataPath))
							throw new MetamodelException($"{cls.Type}.{prop.Name} has both data expression attribute and mapping");

						prop.DataPath = property.Name;

						prop.SetType(property.Type.ReturnedClass);

						prop.IsRequired = !property.IsNullable;

						prop.IsComposite = property.IsComposite;

						if (property.ColumnSpan == 1)
							foreach (var column in property.ColumnIterator.OfType<Column>())
							{
								if (!column.IsLengthDefined() && property.Type.Equals(NHibernateUtil.StringClob))
									prop.Length = int.MaxValue;
								else
									prop.Length = column.Length;
							}
					}
				}
				catch (Exception ex)
				{
					throw new Exception("UpdateMetamodel: configure class " + clazz.ClassName, ex);
				}
			}
		}

	}

}