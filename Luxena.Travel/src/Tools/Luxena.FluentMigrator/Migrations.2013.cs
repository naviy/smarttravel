using FluentMigrator;


namespace Luxena.Travel.DbMigrator
{

	[Migration(2013072601)]
	public class Migration_2013072601 : ForwardOnlyMigration
	{
		public override void Up()
		{
			if (Schema.Table("lt_system_db_version").Exists())
				Delete.Table("lt_system_db_version");
		}
	}


	[Migration(2013082002)]
	public class Migration_2013082002 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Column("vatoptions").OnTable("lt_consignment")
				.AsInt32().NotNullable().WithDefaultValue(0);
			Create.Column("consignmentvatoptions").OnTable("lt_system_configuration")
				.AsInt32().NotNullable().WithDefaultValue(0);
		}
	}

	[Migration(2013082901)]
	public class Migration_2013082901 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Column("document_replication_last_time").OnTable("lt_system_configuration")
				.AsDateTime().Nullable();
		}
	}


	[Migration(2013112301)]
	public class Migration_2013112301 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Create.Column("reissuefor").OnTable("lt_avia_document").AsString(32).Nullable();
			Create.ForeignKey().FromTable("lt_avia_document").ForeignColumn("reissuefor").ToTable("lt_avia_document").PrimaryColumn("id");

			Execute.Sql(@"update lt_avia_document d set reissuefor = (select reissuefor from lt_avia_ticket where id = d.id)");

			Execute.Sql("drop view olap_fare_segment_dim");
			Delete.ForeignKey("aviaticket_reissuefor_fkey").OnTable("lt_avia_ticket");
			Delete.Column("reissuefor").FromTable("lt_avia_ticket");
		}
	}

	[Migration(2013112302)]
	public class Migration_2013112302 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"update lt_avia_document d set reissuefor = coalesce(reissuefor, (select reissuefor from lt_avia_mco where id = d.id))");

			Delete.ForeignKey("aviamco_reissuefor_fkey").OnTable("lt_avia_mco");
			Delete.Column("reissuefor").FromTable("lt_avia_mco");
		}
	}

	[Migration(2013112801)]
	public class Migration_2013112801 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Column("neutralairlinecode").OnTable("lt_system_configuration").AsCustom("citext2").Nullable();
		}
	}

	[Migration(2013120501)]
	public class Migration_2013120501 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Delete.Column("document_replication_last_time").FromTable("lt_system_configuration");
		}
	}

	[Migration(2013122601)]
	public class Migration_2013122601 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Column("useservicefeeonlyinvat").OnTable("lt_order").AsBoolean().WithDefaultValue(false).NotNullable();
		}
	}

	[Migration(2013122701)]
	public class Migration_2013122701 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Column("order_useservicefeeonlyinvat").OnTable("lt_system_configuration").AsBoolean().WithDefaultValue(false).NotNullable();
		}
	}


}
