using System;
using System.Text.RegularExpressions;

using FluentMigrator;
using FluentMigrator.VersionTableInfo;


namespace Luxena.Travel.DbMigrator
{
	[VersionTableMetaData]
	public class TravelVersionTable : DefaultVersionTableMetaData
	{
		static TravelVersionTable()
		{
			MigrationConventions.GetDefaultSchemaName = () => DefaultSchemaName;
		}

		public override string TableName { get { return "lt_system_version_info"; } }

		public override string UniqueIndexName { get { return "uc_lt_system_version_info"; } }


		public override string SchemaName { get { return DefaultSchemaName; } }


		public static string DefaultSchemaName
		{
			get
			{
				return _defaultSchemaName ?? (_defaultSchemaName = _reUserId.Match(Environment.CommandLine).Groups[1].Value);
			}
		}
		private static string _defaultSchemaName;
		private static readonly Regex _reUserId = new Regex(";User ID=(.+?);", RegexOptions.Compiled);
	}
}
