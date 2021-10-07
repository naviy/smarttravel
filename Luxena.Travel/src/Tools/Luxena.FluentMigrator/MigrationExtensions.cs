using System;
using System.Collections.Generic;

using FluentMigrator.Builders;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Builders.Execute;
using FluentMigrator.Builders.Insert;
using FluentMigrator.Infrastructure;


namespace Luxena.Travel.DbMigrator
{

	internal static class MigrationExtensions
	{

		public static object ExecuteScalar(this IExecuteExpressionRoot me, string sql)
		{
			object result = null;

			me.WithConnection((cn, tran) =>
			{
				var cmd = cn.CreateCommand();
				cmd.CommandText = sql;
				result = cmd.ExecuteScalar();
			});

			return result;
		}


		public static ICreateTableColumnOptionOrWithColumnSyntax WithIdColumn(this ICreateTableWithColumnSyntax me, string name = null)
		{
			return me
				.WithColumn(name ?? "id")
				.AsString(32)
				.NotNullable()
				.PrimaryKey();
		}

		public static ICreateTableColumnOptionOrWithColumnSyntax WithMoneyColumn(this ICreateTableWithColumnSyntax me, string name)
		{
			return me
				.WithColumn(name + "_amount").AsDecimal(19, 5).Nullable()
				.WithColumn(name + "_currency").AsString(32).Nullable();
		}



		public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AddMoneyColumn(this IAlterTableAddColumnOrAlterColumnSyntax me, string name)
		{
			return me
				.AddColumn(name + "_amount").AsDecimal(19, 5).Nullable()
				.AddColumn(name + "_currency").AsString(32).Nullable();
		}
		public static TNext AsText<TNext>(this IColumnTypeSyntax<TNext> me)
			where TNext : IFluentSyntax
		{
			return me.AsCustom("citext2");
		}

		public static ICreateTableColumnOptionOrWithColumnSyntax AsRefecence(this ICreateTableColumnAsTypeSyntax me, string primaryTableName, string primaryColumnName = "id")
		{
			return me.AsString(32).Indexed().ForeignKey(primaryTableName, primaryColumnName);
		}

		public static IAlterTableColumnOptionOrAddColumnOrAlterColumnOrForeignKeyCascadeSyntax AsRefecence(this IAlterTableColumnAsTypeSyntax me, string primaryTableName, string primaryColumnName = "id")
		{
			return me
				.AsString(32)
				.Indexed()
				.ForeignKey(primaryTableName, primaryColumnName);
		}

		public static ICreateTableColumnOptionOrWithColumnSyntax AsEntity(this ICreateTableWithColumnSyntax me)
		{
			return me
				.WithIdColumn()
				.WithColumn("version").AsInt32().NotNullable()
			;
		}

		public static ICreateTableColumnOptionOrWithColumnSyntax AsEntity2(this ICreateTableWithColumnSyntax me)
		{
			return me
				.AsEntity()
				.WithColumn("createdby").AsText().NotNullable()
				.WithColumn("createdon").AsDateTime().NotNullable()
				.WithColumn("modifiedby").AsText().Nullable()
				.WithColumn("modifiedon").AsDateTime().Nullable()
			;
		}

		public static ICreateTableColumnOptionOrWithColumnSyntax AsEntity3(this ICreateTableWithColumnSyntax me)
		{
			return me
				.AsEntity2()
				.WithColumn("name").AsText().NotNullable().Indexed()
			;
		}

		public static ICreateTableColumnOptionOrWithColumnSyntax AsEntity3D(this ICreateTableWithColumnSyntax me)
		{
			return me
				.AsEntity3()
				.WithColumn("description").AsText().Nullable()
			;
		}

		public static IInsertDataSyntax Row_Entity2(this IInsertDataSyntax me, Dictionary<string, object> values)
		{
			if (values == null)
				values = new Dictionary<string, object>();

			values["id"] = Guid.NewGuid().ToString("N");
			values["version"] = 0;
			values["createdon"] = DateTime.Now;
			values["createdby"] = "SYSTEM";

			return me.Row(values);
		}



		public static IInsertDataSyntax Row_Entity3(this IInsertDataSyntax me, string name, Dictionary<string, object> values = null)
		{
			if (values == null)
				values = new Dictionary<string, object>();

			values["id"] = Guid.NewGuid().ToString("N");
			values["version"] = 0;
			values["createdon"] = DateTime.Now;
			values["createdby"] = "SYSTEM";

			values["name"] = name;

			return me.Row(values);
		}

		public static IInsertDataSyntax Row_Entity3D(this IInsertDataSyntax me, string name, string description, Dictionary<string, object> values = null)
		{
			if (values == null)
				values = new Dictionary<string, object>();

			values["id"] = Guid.NewGuid().ToString("N");
			values["version"] = 0;
			values["createdon"] = DateTime.Now;
			values["createdby"] = "SYSTEM";

			values["name"] = name;
			values["description"] = description;

			return me.Row(values);
		}

	}

}