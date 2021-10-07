using System.Collections.Generic;

using NHibernate.Cfg;
using NHibernate.Util;


namespace Luxena.Base.Data.NHibernate
{
	public class NamingStrategy : INamingStrategy
	{
		public string TablePrefix { get; set; }

		public string TableWordSeparator { get; set; }

		public IDictionary<string, string> KeywordReplacements { get; set; }

		public string ClassToTableName(string className)
		{
			return TableName(StringHelper.Unqualify(className));
		}

		public string PropertyToColumnName(string propertyName)
		{
			return ColumnName(StringHelper.Unqualify(propertyName));
		}

		public string TableName(string tableName)
		{
			if (!string.IsNullOrEmpty(TableWordSeparator))
				tableName = ImprovedNamingStrategy.Instance.TableName(tableName);

			return GetIdentifier(TablePrefix + tableName);
		}

		public string ColumnName(string columnName)
		{
			return GetIdentifier(columnName);
		}

		public string PropertyToTableName(string className, string propertyName)
		{
			return ClassToTableName(className + propertyName);
		}

		public string LogicalColumnName(string columnName, string propertyName)
		{
			return StringHelper.IsNotEmpty(columnName) ? columnName : StringHelper.Unqualify(propertyName);
		}

		private string GetIdentifier(string name)
		{
			if (!string.IsNullOrEmpty(name))
			{
				name = name.ToLower();

				string replacement;

				if (KeywordReplacements != null && KeywordReplacements.TryGetValue(name, out replacement))
					return replacement;
			}

			return name;
		}
	}
}