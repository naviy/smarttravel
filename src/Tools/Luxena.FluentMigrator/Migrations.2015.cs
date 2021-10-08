using System;

using FluentMigrator;


namespace Luxena.Travel.DbMigrator
{

	[Migration(2015012201)]
	public class Migration_2015012201 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product")
				.AddColumn("printunticketedflightsegments").AsBoolean().Nullable();
		}
	}

	[Migration(2015020901)]
	public class Migration_2015020901 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Create.Table("lt_contract")
				.AsEntity2()
				.WithColumn("customer").AsRefecence("lt_party").NotNullable()
				.WithColumn("number_").AsText().Nullable()
				.WithColumn("issuedate").AsDate().Nullable()
				.WithColumn("discountpc").AsDecimal().Nullable()
				.WithColumn("note").AsText().Nullable();
		}
	}

	[Migration(2015052601)]
	public class Migration_2015052601 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Insert.IntoTable("lt_sequence").Row(new
			{
				id = 21,
				name = "CompletionCertificate",
				format = "A.{0:yy}-{1:00000}",
				timestamp = DateTime.Now,
				current = 0,
			});
		}
	}

	[Migration(2015052701)]
	public class Migration_2015052701 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_issued_consignment")
				.AddColumn("version").AsInt32().WithDefaultValue(1).NotNullable();
		}
	}

	[Migration(2015052901)]
	public class Migration_2015052901 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_party")
				.AddColumn("details").AsText().Nullable();
		}
	}

	[Migration(2015060502)]
	public class Migration_2015060502 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_party")
				.AddColumn("invoicesuffix").AsText().Nullable();
		}
	}

	[Migration(2015063001)]
	public class Migration_2015063001 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("invoiceprinter_footerdetails").AsText().Nullable()
			;
		}
	}

	[Migration(2015071301)]
	public class Migration_2015071301 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product")
				.AddColumn("taxmode").AsInt32().NotNullable().WithDefaultValue(0)
			;
		}
	}

	[Migration(2015071401)]
	public class Migration_2015071401 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"update lt_product set class = 'CarRental' where class = '11'");
		}
	}


	[Migration(2015072602)]
	public class Migration_2015072602 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_order_check")
				.AsEntity2()
				.WithColumn("date_").AsDateTime().NotNullable()
				.WithColumn("order_").AsRefecence("lt_order").NotNullable()
				.WithColumn("person").AsRefecence("lt_party").NotNullable()
				.WithColumn("checktype").AsInt32().NotNullable()
				.WithColumn("checknumber").AsText().Nullable()
				.WithMoneyColumn("checkamount")
				.WithMoneyColumn("payamount")
				.WithColumn("description").AsText().Nullable()
			;
		}
	}

	[Migration(2015080401)]
	public class Migration_2015080401 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_flight_segment")
				.AddMoneyColumn("couponamount")
			;
		}
	}

	[Migration(2015080701)]
	public class Migration_2015080701 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_order_check")
				.AddColumn("paymenttype").AsInt32().Nullable()
			;
		}
	}

	[Migration(2015080702)]
	public class Migration_2015080702 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_order_check")
				.AddColumn("currency").AsText().Nullable()
				.AddColumn("checkamount").AsDecimal(19, 5).Nullable()
				.AddColumn("payamount").AsDecimal(19, 5).Nullable();

			Execute.Sql(@"
update lt_order_check set 
	currency = checkamount_currency,
	checkamount = checkamount_amount,
	payamount = payamount_amount
");


			Delete.Column("checkamount_amount").FromTable("lt_order_check");
			Delete.Column("checkamount_currency").FromTable("lt_order_check");
			Delete.Column("payamount_amount").FromTable("lt_order_check");
			Delete.Column("payamount_currency").FromTable("lt_order_check");
		}
	}


	[Migration(2015091001)]
	public class Migration_2015091001 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
alter table lt_product
  alter cardtype drop not null
, alter serviceclass drop not null
, alter issale drop not null
, alter gdspassportstatus drop not null
, alter domestic drop not null
, alter interline drop not null
");
		}
	}

	[Migration(2015091002)]
	public class Migration_2015091002 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_order")
				.AddColumn("separateservicefee").AsBoolean().Nullable();
		}
	}

	[Migration(2015091701)]
	public class Migration_2015091701 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
alter table lt_sequence
	alter id type varchar(32),
	add version int;

update lt_sequence set
	id = replace(cast(uuid_generate_v4() as varchar), '-', ''),
	version = 1;

alter table lt_sequence
	alter version set not null;
");
		}
	}

	[Migration(2015091702)]
	public class Migration_2015091702 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_order_check")
				.AddColumn("checkvat").AsDecimal(19, 5).Nullable();
		}
	}

	[Migration(2015102901)]
	public class Migration_2015102901 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product")
				.AddColumn("taxrateofproduct").AsInt32().WithDefaultValue(0).Nullable()
				.AddColumn("taxrateofservicefee").AsInt32().WithDefaultValue(0).Nullable();

			Execute.Sql(@"
update lt_product set
	taxrateofproduct = case taxmode when 0 then 0 else 5 end,
	taxrateofservicefee = case taxmode when 0 then 0 when 2 then 1 else -1 end
");
		}
	}

	[Migration(2015102902)]
	public class Migration_2015102902 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"alter table lt_product drop column taxmode cascade");
		}
	}

	[Migration(2016015101)]
	public class Migration_2016015101 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_modification")
				.AddColumn("version").AsInt32().NotNullable().WithDefaultValue(1);
		}
	}

}