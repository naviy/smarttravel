using System;

using FluentMigrator;


namespace Luxena.Travel.DbMigrator
{

	[Migration(2014013001)]
	public class Migration_2014 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("invoice_numbermode")
				.AsInt32().NotNullable().WithDefaultValue(0);

			Alter.Table("lt_order")
				.AddColumn("invoicelastindex")
				.AsInt32().Nullable();
		}
	}


	[Migration(2014012301)]
	public class Migration_2014012301 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Update.Table("lt_system_configuration")
				.Set(new { neutralairlinecode = 100 })
				.AllRows();
		}
	}

	[Migration(2014012701)]
	public class Migration_2014012701 : ForwardOnlyMigration
	{
		public override void Up()
		{
			// Чистка имён от лишних пробелов
			Execute.Sql(@"
update lt_party p set name = 
	case when exists(select id from lt_party where id <> p.id and trim(name)::citext2 = trim(p.name))
		then concat(trim(name), ' (', 
			case 
				when name like ' %' then '4'
				when name like '%   ' then '3' 
				when name like '%  ' then '2' 
				when name like '% ' then '1' 
				else '5' 
			end, ')')
		else trim(name)
	end 
 where name <> trim(name)
");
		}
	}


	[Migration(2014040701)]
	public class Migration_2014040701 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.EmbeddedScript("create_products.sql");
		}
	}

	[Migration(2014040702)]
	public class Migration_2014040702 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_invoice")
				.AddColumn("vat_amount").AsDecimal(19, 5).Nullable()
				.AddColumn("vat_currency").AsString(32).Nullable()
				.Indexed()
				.ForeignKey("lt_currency", "id");
		}
	}

	[Migration(2014040703)]
	public class Migration_2014040703 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update 
	lt_invoice i 
set
	vat_amount = o.vat_amount,
	vat_currency = o.vat_currency
from 
	lt_order o
where 
	i.order_ = o.id
");
		}
	}

	[Migration(2014040801)]
	public class Migration_2014040801 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_order")
				.AddColumn("consignmentnumbers").AsText().Nullable();
		}
	}


	[Migration(2014041901)]
	public class Migration_2014041901 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_pasteboard")
				.WithIdColumn().ForeignKey("lt_product", "id")
				.WithColumn("number_").AsText().Nullable().Indexed()
				.WithColumn("firststation").AsText().Nullable()
				.WithColumn("laststation").AsText().Nullable()
				.WithColumn("startdate").AsDateTime().Nullable()
				;
		}
	}

	[Migration(2014051403)]
	public class Migration_2014051403 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_sim_card")
				.WithIdColumn().ForeignKey("lt_product", "id")
				.WithColumn("number_").AsText().NotNullable()
				.WithColumn("operator").AsInt32().NotNullable().WithDefaultValue(0)
				.WithColumn("issale").AsBoolean().NotNullable().WithDefaultValue(false)
				;

			Create.Table("lt_isic")
				.WithIdColumn().ForeignKey("lt_product", "id")
				.WithColumn("cardtype").AsText().NotNullable().WithDefaultValue(0)
				.WithColumn("number1").AsText().NotNullable()
				.WithColumn("number2").AsText().NotNullable()
				;
		}
	}

	[Migration(2014051501)]
	public class Migration_2014051501 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_excursion")
				.WithIdColumn().ForeignKey("lt_product", "id")

				.WithColumn("startdate").AsDateTime().NotNullable()
				.WithColumn("finishdate").AsDateTime().Nullable()

				.WithColumn("tourname").AsText().NotNullable()
				;
		}
	}

	[Migration(2014051901)]
	public class Migration_2014051901 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_tour")
				.WithIdColumn().ForeignKey("lt_product", "id")

				.WithColumn("startdate").AsDateTime().NotNullable()
				.WithColumn("finishdate").AsDateTime().Nullable()

				.WithColumn("hotelname").AsText().Nullable()
				.WithColumn("hoteloffice").AsText().Nullable()
				.WithColumn("hotelcode").AsText().Nullable()
				.WithColumn("hotelroom").AsText().Nullable()

				.WithColumn("placementname").AsText().Nullable()
				.WithColumn("placementoffice").AsText().Nullable()
				.WithColumn("placementcode").AsText().Nullable()

				.WithColumn("diet").AsText().Nullable()

				.WithColumn("aviadescription").AsText().Nullable()
				.WithColumn("transferdescription").AsText().Nullable()
				;
		}
	}

	[Migration(2014052101)]
	public class Migration_2014052101 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_accommodation")
				.WithIdColumn().ForeignKey("lt_product", "id")

				.WithColumn("startdate").AsDateTime().NotNullable()
				.WithColumn("finishdate").AsDateTime().Nullable()

				.WithColumn("hotelname").AsText().Nullable()
				.WithColumn("hoteloffice").AsText().Nullable()
				.WithColumn("hotelcode").AsText().Nullable()
				.WithColumn("hotelroom").AsText().Nullable()

				.WithColumn("placementname").AsText().Nullable()
				.WithColumn("placementoffice").AsText().Nullable()
				.WithColumn("placementcode").AsText().Nullable()

				.WithColumn("diet").AsText().Nullable()
				;
		}
	}

	[Migration(2014052102)]
	public class Migration_2014052102 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_transfer")
				.WithIdColumn().ForeignKey("lt_product", "id")

				.WithColumn("startdate").AsDateTime().NotNullable()
				;
		}
	}

	[Migration(2014052201)]
	public class Migration_2014052201 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_insurance")
				.WithIdColumn().ForeignKey("lt_product", "id")

				.WithColumn("startdate").AsDateTime().Nullable()
				.WithColumn("finishdate").AsDateTime().Nullable()
				;
		}
	}

	[Migration(2014052203)]
	public class Migration_2014052203 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_car_rental")
				.WithIdColumn().ForeignKey("lt_product", "id")

				.WithColumn("startdate").AsDateTime().Nullable()
				.WithColumn("finishdate").AsDateTime().Nullable()
				.WithColumn("carbrand").AsText().Nullable()
				;
		}
	}

	[Migration(2014052301)]
	public class Migration_2014052301 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_pasteboard")
				.AddColumn("serviceclass").AsInt32().NotNullable()
				;
		}
	}

	[Migration(2014052602)]
	public class Migration_2014052602 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_product_provider")
				.AsEntity3()
				.WithColumn("useforaccommodation").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("useforcarrental").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("useforinsurance").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("useforpasteboard").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("usefortransfer").AsBoolean().NotNullable().WithDefaultValue(false)
				;
		}
	}

	[Migration(2014052701)]
	public class Migration_2014052701 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_accommodation").AddColumn("provider").AsRefecence("lt_product_provider").Nullable();
			Alter.Table("lt_car_rental").AddColumn("provider").AsRefecence("lt_product_provider").Nullable();
			Alter.Table("lt_insurance").AddColumn("provider").AsRefecence("lt_product_provider").Nullable();
			Alter.Table("lt_pasteboard").AddColumn("provider").AsRefecence("lt_product_provider").Nullable();
			Alter.Table("lt_transfer").AddColumn("provider").AsRefecence("lt_product_provider").Nullable();
		}
	}

	[Migration(2014052801)]
	public class Migration_2014052801 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_generic_product_type")
				.AsEntity3()
				;
		}
	}

	[Migration(2014052803)]
	public class Migration_2014052803 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Insert.IntoTable("lt_generic_product_type")
				.Row(new { id = "2892065a0869477aa3c4fdf8e15efbfd", name = "Доставка", createdon = DateTime.Now, createdby = "SYSTEM", version = 0 })
				.Row(new { id = "b07e435454304b73a321c6ad544a7571", name = "Конференция", createdon = DateTime.Now, createdby = "SYSTEM", version = 0 })
				.Row(new { id = "cbdad2003e0949239008fd7d4077c39b", name = "Вип-залы", createdon = DateTime.Now, createdby = "SYSTEM", version = 0 })
				//.Row(new { id = "b1b71e196bed499980c4f9cddcbc1f74", name = "Доставка", createdon = DateTime.Now, createdby = "SYSTEM" })
				;
		}
	}

	[Migration(2014052804)]
	public class Migration_2014052804 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_generic_product")
				.WithIdColumn().ForeignKey("lt_product", "id")

				.WithColumn("provider").AsRefecence("lt_product_provider").Nullable()

				.WithColumn("generictype").AsRefecence("lt_generic_product_type").NotNullable()
				.WithColumn("number_").AsText().Nullable()

				.WithColumn("startdate").AsDateTime().Nullable()
				.WithColumn("finishdate").AsDateTime().Nullable()
				;
		}
	}

	[Migration(2014060401)]
	public class Migration_2014060401 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_pasteboard")
				.AddColumn("trainnumber").AsText().Nullable()
				.AddColumn("carnumber").AsText().Nullable()
				;
		}
	}

	[Migration(2014060502)]
	public class Migration_2014060502 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_insurance")
				.AddColumn("number_").AsText().Nullable()
				;
		}
	}

	[Migration(2014061001)]
	public class Migration_2014061001 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update 
	lt_order o 
set
	consignmentnumbers = (
		select
			array_to_string(array_agg(number_::text), ', ')
		from (
			select distinct 
				oi.order_, c.number_
			from 
				lt_order_item oi
				inner join lt_consignment c on c.id = oi.consignment
			where
				o.id = oi.order_
		) q
	)
");

		}
	}

	[Migration(2014061002)]
	public class Migration_2014061002 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Create.Index("IX_lt_order_consignmentnumbers").OnTable("lt_order").OnColumn("consignmentnumbers");
		}
	}

	[Migration(2014061301)]
	public class Migration_2014061301 : AutoReversingMigration
	{
		public override void Up()
		{
			Create.Table("lt_bus_ticket")
				.WithIdColumn().ForeignKey("lt_product", "id")

				.WithColumn("provider").AsRefecence("lt_product_provider").Nullable()

				.WithColumn("departureplace").AsText().Nullable()
				.WithColumn("departuredate").AsDateTime().Nullable()
				.WithColumn("departuretime").AsText().Nullable()

				.WithColumn("arrivalplace").AsText().Nullable()
				.WithColumn("arrivaldate").AsDateTime().Nullable()
				.WithColumn("arrivaltime").AsText().Nullable()

				.WithColumn("seatnumber").AsText().Nullable()
				;
		}
	}

	[Migration(2014061302)]
	public class Migration_2014061302 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product_provider")
				.AddColumn("useforbusticket").AsBoolean().NotNullable().WithDefaultValue(false)
				;
		}
	}

	[Migration(2014062301)]
	public class Migration_2014062301 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product")
				.AddMoneyColumn("bonusaccumulation")
				.AddMoneyColumn("bonusdiscount")
				;
		}
	}

	[Migration(2014070101)]
	public class Migration_2014070101 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_person")
				.AddColumn("bonuscardnumber").AsText().Nullable()
				;
		}
	}

	//	[Migration(2014071201)]
	//	public class Migration_2014071201 : AutoReversingMigration
	//	{
	//		public override void Up()
	//		{
	//			Create
	//				.UniqueConstraint("lt_avia_document_number__type_airlineprefixcode_key")
	//				.OnTable("lt_avia_document")
	//				.Columns(new [] { "number_", "type", "airlineprefixcode" })
	//			;
	//		}
	//	}


	[Migration(2014072002)]
	public class Migration_2014072002 : ForwardOnlyMigration
	{
		public override void Up()
		{

			Create.Table("lt_accommodation_type")
				.AsEntity3D()
			;

			Insert.IntoTable("lt_accommodation_type")
				.Row_Entity3D("SGL (Single)", "одноместное")
				.Row_Entity3D("DBL (Double)", "двухместное. При этом в номере может находиться одна двуспальная кровать (double) или две отдельных (twin)")
				.Row_Entity3D("TRPL (Triple)", "трехместное. Чаще всего две кровати и дополнительная раскладывающаяся кровать или диван")
				.Row_Entity3D("QDPL (Quadriple)", "четырехместное")
				.Row_Entity3D("ExB (Extra Bed)", "дополнительное место: кровать, диван или раскладушка")
				.Row_Entity3D("Promo Rooms - R.O.H", "ограниченное количество номеров, предлагаемых по акции. Расселение на усмотрение отеля")
				.Row_Entity3D("STD (Standard)", "стандартная комната")
				.Row_Entity3D("STD (Standard) with Air Condition", "стандартная комната с кондиционером")
				.Row_Entity3D("STD (Standard) without Air Condition", "стандартная комната без кондиционера")
				.Row_Entity3D("BDR, BDRM (Bedroom)", "номер со спальней")
				.Row_Entity3D("Balcony", "номер с балконом")
				.Row_Entity3D("Superior (Classic)", "комната большего размера, чем стандартная")
				.Row_Entity3D("Corner room", "угловая комната")
				.Row_Entity3D("Studio", "студия, однокомнатный номер больше стандартного c встроенной кухней")
			;

			Delete
				.Column("hotelroom")
				.FromTable("lt_accommodation");
			Alter
				.Table("lt_accommodation")
				.AddColumn("accommodationtype").AsRefecence("lt_accommodation_type").Nullable();

			Delete
				.Column("hotelroom")
				.FromTable("lt_tour");
			Alter
				.Table("lt_tour")
				.AddColumn("accommodationtype").AsRefecence("lt_accommodation_type").Nullable();
		}
	}


	[Migration(2014072003)]
	public class Migration_2014072003 : ForwardOnlyMigration
	{
		public override void Up()
		{

			Create.Table("lt_catering_type")
				.AsEntity3D()
			;

			Insert.IntoTable("lt_catering_type")
				.Row_Entity3D("RO (Room only)", "без питания")
				.Row_Entity3D("EP (European Plan)", "без питания")
				.Row_Entity3D("BO (Bed Only)", "без питания")
				.Row_Entity3D("AO (Accommodation Only)", "без питания")
				.Row_Entity3D("NO", "без питания")
				.Row_Entity3D("BB (Bed & breakfast)", "завтрак. Как правило под этим подразумевается завтрак-буфет. Вариант с обслуживанием по системе «шведский стол». Включает как холодные, так и горячие блюда. Разнообразие блюд сильно отличается от страны к стране")
				.Row_Entity3D("HB (Half board)", "полупансион. Как правило завтрак и ужин, но возможен и вариант завтрак и обед")
				.Row_Entity3D("FB (Full board)", "полный пансион (завтрак, обед и ужин)")
				.Row_Entity3D("AI (All inclusive)", "всё включено — завтрак, обед и ужин (шведский стол). В течение дня предлагаются напитки (алкогольные и безалкогольные) в неограниченном количестве плюс дополнительное питание (второй завтрак, полдник, поздний ужин, легкие закуски, барбекю в барах отеля и т. п.)")
				.Row_Entity3D("CB (Continental breakfast)", "континентальный завтрак (также встречается название французский завтрак). Лёгкий завтрак, состоящий из кофе (чая), сока, булочки, масла и джема. Предлагается, как правило, в европейских городских отелях. «Кофе+круассан» очень хорошо характеризует такие завтраки. Минимальное разнообразие, но и существенная экономия в случае оплаты дополнительно")
				.Row_Entity3D("AB (American breakfast)", "американский завтрак (также встречается название английский завтрак). Основное отличие от завтрака-буфета — из горячих блюд только омлет с беконом (иногда сосисками)")
				.Row_Entity3D("UAI (Ultra all inclusive)", "завтрак, поздний завтрак, обед, полдник и ужин (шведский стол). Большой выбор сладостей, десертов, всевозможных закусок, а также широкий выбор напитков местного и импортного производства. Большинство отелей, работающих по системе Ultra All Inclusive, предлагают гостям дополнительное бесплатное питание в ресторанах с кухней разных народов мира. Основное отличие от системы All inclusive более высокий уровень питания и напитков. Как правило, встречается в таких странах, как Турция и Египет. Отели в остальных странах редко используют такой термин")
			;

			Delete
				.Column("diet")
				.FromTable("lt_accommodation");
			Alter
				.Table("lt_accommodation")
				.AddColumn("cateringtype").AsRefecence("lt_catering_type").Nullable();

			Delete
				.Column("diet")
				.FromTable("lt_tour");
			Alter
				.Table("lt_tour")
				.AddColumn("cateringtype").AsRefecence("lt_catering_type").Nullable();
		}
	}


	[Migration(2014072801)]
	public class Migration_2014072801 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(
@"
alter table lt_product add column provider varchar(32);

create index IX_lt_product_provider on lt_product (provider);

alter table lt_product 
	add constraint FK_lt_product_provider_lt_product_provider_id foreign key (provider) references lt_product_provider (id);


update lt_product p set provider = q.provider
  from lt_accommodation q
 where p.id = q.id;

update lt_product p set provider = q.provider
  from lt_car_rental q
 where p.id = q.id;

update lt_product p set provider = q.provider
  from lt_insurance q
 where p.id = q.id;

update lt_product p set provider = q.provider
  from lt_pasteboard q
 where p.id = q.id;

update lt_product p set provider = q.provider
  from lt_transfer q
 where p.id = q.id;

update lt_product p set provider = q.provider
  from lt_generic_product q
 where p.id = q.id;

update lt_product p set provider = q.provider
  from lt_bus_ticket q
 where p.id = q.id;


alter table lt_accommodation drop column provider;
alter table lt_car_rental drop column provider;
alter table lt_insurance drop column provider;
alter table lt_pasteboard drop column provider;
alter table lt_transfer drop column provider;
alter table lt_generic_product drop column provider;
alter table lt_bus_ticket drop column provider;

");
		}
	}

	[Migration(2014072802)]
	public class Migration_2014072802 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product_provider")
				.AddColumn("usefortour").AsBoolean().NotNullable().WithDefaultValue(false)
			;
		}
	}


	[Migration(2014081101)]
	public class Migration_2014081101 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product")
				.AddColumn("airline").AsRefecence("lt_airline").Nullable()
				.AddColumn("ticketingiataoffice").AsText().Nullable();

			Execute.Sql(
@"
update lt_product p set 
	airline = d.airline,
	ticketingiataoffice = d.ticketingiataoffice
  from lt_avia_document d
 where d.id = p.id;

alter table lt_avia_document 
	drop column airline cascade, 
	drop column ticketingiataoffice cascade;
"
			);
		}
	}


	[Migration(2014081801)]
	public class Migration_2014081801 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_payment")
				.AddColumn("sign").AsInt32().NotNullable().WithDefaultValue(1)
			;
		}
	}


	[Migration(2014081901)]
	public class Migration_2014081901 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_system_configuration")
				.AddColumn("galileowebservice_loadedon").AsDateTime().Nullable()
			;
		}
	}


	[Migration(2014082601)]
	public class Migration_2014082601 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Insert.IntoTable("lt_sequence").Row(new
			{
				id = 20,
				name = "CashOutOrderPayment",
				format = "PO.{0:yy}-{1:00000}",
				timestamp = DateTime.Now,
				current = 0,
			});
		}
	}


	[Migration(2014090802)]
	public class Migration_2014090802 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_organization")
				.AddColumn("isprovider").AsBoolean().NotNullable().WithDefaultValue(false)
				.AddColumn("isaccommodationprovider").AsBoolean().NotNullable().WithDefaultValue(false)
				.AddColumn("isbusticketprovider").AsBoolean().NotNullable().WithDefaultValue(false)
				.AddColumn("iscarrentalprovider").AsBoolean().NotNullable().WithDefaultValue(false)
				.AddColumn("isinsuranceprovider").AsBoolean().NotNullable().WithDefaultValue(false)
				.AddColumn("ispasteboardprovider").AsBoolean().NotNullable().WithDefaultValue(false)
				.AddColumn("istourprovider").AsBoolean().NotNullable().WithDefaultValue(false)
				.AddColumn("istransferprovider").AsBoolean().NotNullable().WithDefaultValue(false)
			;
		}
	}

	[Migration(2014090803)]
	public class Migration_2014090803 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_organization")
				.AddColumn("isairline").AsBoolean().NotNullable().WithDefaultValue(false)
				.AddColumn("airlineiatacode").AsText().Nullable()
				.AddColumn("airlineprefixcode").AsText().Nullable()
				.AddColumn("airlinepassportrequirement").AsInt32().NotNullable().WithDefaultValue(0)
			;
		}
	}

	[Migration(2014091001)]
	public class Migration_2014091001 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.EmbeddedScript("organizations_from_product_providers.sql");
		}
	}


	[Migration(2014091002)]
	public class Migration_2014091002 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.EmbeddedScript("organizations_from_airlines.sql");
		}
	}


	[Migration(2014091401)]
	public class Migration_2014091401 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_product")
				.AddColumn("producer").AsRefecence("lt_organization").Nullable();

			Execute
				.Sql(@"update lt_product set producer = airline");

			Execute
				.Sql(@"alter table lt_product drop column airline cascade");
		}
	}

	[Migration(2014091402)]
	public class Migration_2014091402 : AutoReversingMigration
	{
		public override void Up()
		{
			Alter.Table("lt_organization")
				.AddColumn("isinsurancecompany").AsBoolean().NotNullable().WithDefaultValue(false)
			;
		}
	}

	[Migration(2014091403)]
	public class Migration_2014091403 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute
				.Sql(@"update lt_organization set isinsurancecompany = isinsuranceprovider");
		}
	}

	[Migration(2014091901)]
	public class Migration_2014091901 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_airline_service_class set
	serviceclass = case 
		when serviceclass <> 0 then serviceclass
		when code in ('Z','D','C','J') then 3
		when code in ('F') then 4
		when code is null then null
		else 1
	end;

update lt_flight_segment set
	serviceclass = case 
		when serviceclass <> 0 then serviceclass
		when serviceclasscode in ('Z','D','C','J') then 3
		when serviceclasscode in ('F') then 4
		when serviceclasscode is null then null
		else 1
	end;
"
			);
		}
	}


	//	[Migration(2014092301)]
	//	public class Migration_2014092301 : ForwardOnlyMigration
	//	{
	//		public override void Up()
	//		{
	//			Execute.EmbeddedScript("aviadocuments_from_all_avia_tables.sql");
	//		}
	//	}

	[Migration(2014092501)]
	public class Migration_2014092501 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_organization")
				.AddColumn("isroamingoperator").AsBoolean().NotNullable().WithDefaultValue(false);
		}
	}

	[Migration(2014092502)]
	public class Migration_2014092502 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(
@"
insert into lt_party (
	id, name, 
	version, createdby, createdon, iscustomer, issupplier
)
select 
	'5abc493283d24205ac1bf5ff45ef7b12', 'Travel Sim',
	1, 'SYSTEM', now(), false, true
 where not exists(select id from lt_party where name like 'travel%sim' limit 1);

insert into lt_organization (id, isroamingoperator)
select 
	'5abc493283d24205ac1bf5ff45ef7b12', true
 where not exists(select id from lt_party where name like 'travel%sim' and id <> '5abc493283d24205ac1bf5ff45ef7b12' limit 1);


insert into lt_party (
	id, name, 
	version, createdby, createdon, iscustomer, issupplier
)
select 
	'e70fcc54d9f44bb49b3d4eba857dc0f3', 'Eureka',
	1, 'SYSTEM', now(), false, true
 where not exists(select id from lt_party where name = 'Eureka' limit 1);

insert into lt_organization (id, isroamingoperator)
select 
	'e70fcc54d9f44bb49b3d4eba857dc0f3', true
 where not exists(select id from lt_party where name = 'Eureka' and id <> 'e70fcc54d9f44bb49b3d4eba857dc0f3' limit 1);


update lt_product p set 
	producer = case 
		when operator = 1 then o1.id
		when operator = 2 then o2.id
	end
  from lt_sim_card sc
	cross join (select id from lt_party where name like 'travel%sim' limit 1) o1
	cross join (select id from lt_party where name = 'Eureka' limit 1) o2
 where p.id = sc.id;
");
		}
	}


	[Migration(2014092503)]
	public class Migration_2014092503 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Delete.Column("operator").FromTable("lt_sim_card");
		}
	}


	[Migration(2014092504)]
	public class Migration_2014092504 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(
@"
update lt_organization o set 
	isroamingoperator = true
  from lt_party p
 where p.id = o.id and p.name like 'travel%sim';

update lt_organization o set 
	isroamingoperator = true
  from lt_party p
 where p.id = o.id and p.name like 'Eureka'; 
");
		}
	}



	[Migration(2014100801)]
	public class Migration_2014100801 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** AviaRefund ***/

alter table lt_product
	add column isrefund boolean not null default false,
	add column refundedproduct varchar(32),
	add column refundservicefee_amount numeric(19,5),
	add column refundservicefee_currency varchar(32),
	add column servicefeepenalty_amount numeric(19,5),
	add column servicefeepenalty_currency varchar(32),
	
	add constraint ""FK_lt_product_refundedproduct_lt_product_id""
		foreign key (refundedproduct) references lt_product (id),
	add constraint ""FK_lt_product_refundservicefee_currency_lt_currency_id""
		foreign key (refundservicefee_currency) references lt_currency (id),
	add constraint ""FK_lt_product_servicefeepenalty_currency_lt_currency_id"" 
		foreign key (servicefeepenalty_currency) references lt_currency (id)
;      

create index ""IX_lt_product_refundedproduct"" on lt_product (refundedproduct);
create index ""IX_lt_product_refundservicefee_currency"" on lt_product (refundservicefee_currency);
create index ""IX_lt_product_servicefeepenalty_currency"" on lt_product (servicefeepenalty_currency);


update lt_product p set
	isrefund = true,
	refundedproduct = r.refundeddocument,
	refundservicefee_amount = r.refundservicefee_amount,
	refundservicefee_currency = r.refundservicefee_currency,
	servicefeepenalty_amount = r.servicefeepenalty_amount,
	servicefeepenalty_currency = r.servicefeepenalty_currency
  from lt_avia_refund r
 where r.id = p.id;
");
		}
	}

	[Migration(2014100810)]
	public class Migration_2014100810 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** AviaDocument ***/

alter table lt_product
	add column airlineiatacode citext2,
	add column airlineprefixcode citext2,
	add column airlinename citext2,
	add column number_ citext2,
	add column conjunctionnumbers citext2,
	add column gdspassportstatus integer not null default (0),
	add column gdspassport citext2,
	add column itinerary citext2,
	add column paymentform citext2,
	add column bookeroffice citext2,
	add column bookercode citext2,
	add column ticketeroffice citext2,
	add column ticketercode citext2,
	add column originator integer not null default (0),
	add column origin integer not null default (0),
	add column airlinepnrcode citext2,
	add column remarks citext2,
	add column displaystring citext2,
	add column booker varchar(32),
	add column ticketer varchar(32),
	add column originaldocument varchar(32),
	add column isticketerrobot boolean,
	add column paymentdetails citext2,
	
	add constraint ""FK_lt_product_booker_lt_person_id""
		foreign key (booker) references lt_person (id),
	add constraint ""FK_lt_product_originaldocument_lt_gds_file_id""
		foreign key (originaldocument) references lt_gds_file (id),
	add constraint ""FK_lt_product_ticketer_lt_person_id""
		foreign key (ticketer) references lt_person (id)
;
");
		}
	}

	[Migration(2014100811)]
	public class Migration_2014100811 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
create index ""IX_lt_product_number_"" on lt_product (number_);
create index ""IX_lt_product_booker"" on lt_product (booker);
create index ""IX_lt_product_originaldocument"" on lt_product (originaldocument);
create index ""IX_lt_product_ticketer"" on lt_product (ticketer);
");
		}
	}

	[Migration(2014100812)]
	public class Migration_2014100812 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_product p set
	airlineiatacode = d.airlineiatacode,
	airlineprefixcode = d.airlineprefixcode,
	airlinename = d.airlinename,
	number_ = d.number_,
	conjunctionnumbers = d.conjunctionnumbers,
	gdspassportstatus = d.gdspassportstatus,
	gdspassport = d.gdspassport,
	itinerary = d.itinerary,
	paymentform = d.paymentform,
	bookeroffice = d.bookeroffice,
	bookercode = d.bookercode,
	ticketeroffice = d.ticketeroffice,
	ticketercode = d.ticketercode,
	originator = d.originator,
	origin = d.origin,
	airlinepnrcode = d.airlinepnrcode,
	remarks = d.remarks,
	displaystring = d.displaystring,
	booker = d.booker,
	ticketer = d.ticketer,
	originaldocument = d.originaldocument,
	isticketerrobot = d.isticketerrobot,
	paymentdetails = d.paymentdetails
  from lt_avia_document d
 where p.id = d.id and p.issuedate <= '2004-1-1';
");
		}
	}

	[Migration(2014100813)]
	public class Migration_2014100813 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_product p set
	airlineiatacode = d.airlineiatacode,
	airlineprefixcode = d.airlineprefixcode,
	airlinename = d.airlinename,
	number_ = d.number_,
	conjunctionnumbers = d.conjunctionnumbers,
	gdspassportstatus = d.gdspassportstatus,
	gdspassport = d.gdspassport,
	itinerary = d.itinerary,
	paymentform = d.paymentform,
	bookeroffice = d.bookeroffice,
	bookercode = d.bookercode,
	ticketeroffice = d.ticketeroffice,
	ticketercode = d.ticketercode,
	originator = d.originator,
	origin = d.origin,
	airlinepnrcode = d.airlinepnrcode,
	remarks = d.remarks,
	displaystring = d.displaystring,
	booker = d.booker,
	ticketer = d.ticketer,
	originaldocument = d.originaldocument,
	isticketerrobot = d.isticketerrobot,
	paymentdetails = d.paymentdetails
  from lt_avia_document d
 where p.id = d.id and p.issuedate between '2004-1-1' and '2006-1-1';
");
		}
	}

	[Migration(2014100814)]
	public class Migration_2014100814 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_product p set
	airlineiatacode = d.airlineiatacode,
	airlineprefixcode = d.airlineprefixcode,
	airlinename = d.airlinename,
	number_ = d.number_,
	conjunctionnumbers = d.conjunctionnumbers,
	gdspassportstatus = d.gdspassportstatus,
	gdspassport = d.gdspassport,
	itinerary = d.itinerary,
	paymentform = d.paymentform,
	bookeroffice = d.bookeroffice,
	bookercode = d.bookercode,
	ticketeroffice = d.ticketeroffice,
	ticketercode = d.ticketercode,
	originator = d.originator,
	origin = d.origin,
	airlinepnrcode = d.airlinepnrcode,
	remarks = d.remarks,
	displaystring = d.displaystring,
	booker = d.booker,
	ticketer = d.ticketer,
	originaldocument = d.originaldocument,
	isticketerrobot = d.isticketerrobot,
	paymentdetails = d.paymentdetails
  from lt_avia_document d
 where p.id = d.id and p.issuedate between '2006-1-1' and '2008-1-1';
");
		}
	}

	[Migration(2014100815)]
	public class Migration_2014100815 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_product p set
	airlineiatacode = d.airlineiatacode,
	airlineprefixcode = d.airlineprefixcode,
	airlinename = d.airlinename,
	number_ = d.number_,
	conjunctionnumbers = d.conjunctionnumbers,
	gdspassportstatus = d.gdspassportstatus,
	gdspassport = d.gdspassport,
	itinerary = d.itinerary,
	paymentform = d.paymentform,
	bookeroffice = d.bookeroffice,
	bookercode = d.bookercode,
	ticketeroffice = d.ticketeroffice,
	ticketercode = d.ticketercode,
	originator = d.originator,
	origin = d.origin,
	airlinepnrcode = d.airlinepnrcode,
	remarks = d.remarks,
	displaystring = d.displaystring,
	booker = d.booker,
	ticketer = d.ticketer,
	originaldocument = d.originaldocument,
	isticketerrobot = d.isticketerrobot,
	paymentdetails = d.paymentdetails
  from lt_avia_document d
 where p.id = d.id and p.issuedate between '2008-1-1' and '2010-1-1';
");
		}
	}

	[Migration(2014100816)]
	public class Migration_2014100816 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_product p set
	airlineiatacode = d.airlineiatacode,
	airlineprefixcode = d.airlineprefixcode,
	airlinename = d.airlinename,
	number_ = d.number_,
	conjunctionnumbers = d.conjunctionnumbers,
	gdspassportstatus = d.gdspassportstatus,
	gdspassport = d.gdspassport,
	itinerary = d.itinerary,
	paymentform = d.paymentform,
	bookeroffice = d.bookeroffice,
	bookercode = d.bookercode,
	ticketeroffice = d.ticketeroffice,
	ticketercode = d.ticketercode,
	originator = d.originator,
	origin = d.origin,
	airlinepnrcode = d.airlinepnrcode,
	remarks = d.remarks,
	displaystring = d.displaystring,
	booker = d.booker,
	ticketer = d.ticketer,
	originaldocument = d.originaldocument,
	isticketerrobot = d.isticketerrobot,
	paymentdetails = d.paymentdetails
  from lt_avia_document d
 where p.id = d.id and p.issuedate between '2010-1-1' and '2012-1-1';
");
		}
	}

	[Migration(2014100817)]
	public class Migration_2014100817 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_product p set
	airlineiatacode = d.airlineiatacode,
	airlineprefixcode = d.airlineprefixcode,
	airlinename = d.airlinename,
	number_ = d.number_,
	conjunctionnumbers = d.conjunctionnumbers,
	gdspassportstatus = d.gdspassportstatus,
	gdspassport = d.gdspassport,
	itinerary = d.itinerary,
	paymentform = d.paymentform,
	bookeroffice = d.bookeroffice,
	bookercode = d.bookercode,
	ticketeroffice = d.ticketeroffice,
	ticketercode = d.ticketercode,
	originator = d.originator,
	origin = d.origin,
	airlinepnrcode = d.airlinepnrcode,
	remarks = d.remarks,
	displaystring = d.displaystring,
	booker = d.booker,
	ticketer = d.ticketer,
	originaldocument = d.originaldocument,
	isticketerrobot = d.isticketerrobot,
	paymentdetails = d.paymentdetails
  from lt_avia_document d
 where p.id = d.id and p.issuedate between '2012-1-1' and '2013-1-1';
");
		}
	}

	[Migration(2014100818)]
	public class Migration_2014100818 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_product p set
	airlineiatacode = d.airlineiatacode,
	airlineprefixcode = d.airlineprefixcode,
	airlinename = d.airlinename,
	number_ = d.number_,
	conjunctionnumbers = d.conjunctionnumbers,
	gdspassportstatus = d.gdspassportstatus,
	gdspassport = d.gdspassport,
	itinerary = d.itinerary,
	paymentform = d.paymentform,
	bookeroffice = d.bookeroffice,
	bookercode = d.bookercode,
	ticketeroffice = d.ticketeroffice,
	ticketercode = d.ticketercode,
	originator = d.originator,
	origin = d.origin,
	airlinepnrcode = d.airlinepnrcode,
	remarks = d.remarks,
	displaystring = d.displaystring,
	booker = d.booker,
	ticketer = d.ticketer,
	originaldocument = d.originaldocument,
	isticketerrobot = d.isticketerrobot,
	paymentdetails = d.paymentdetails
  from lt_avia_document d
 where p.id = d.id and p.issuedate between '2013-1-1' and '2014-1-1';
");
		}
	}

	[Migration(2014100819)]
	public class Migration_2014100819 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_product p set
	airlineiatacode = d.airlineiatacode,
	airlineprefixcode = d.airlineprefixcode,
	airlinename = d.airlinename,
	number_ = d.number_,
	conjunctionnumbers = d.conjunctionnumbers,
	gdspassportstatus = d.gdspassportstatus,
	gdspassport = d.gdspassport,
	itinerary = d.itinerary,
	paymentform = d.paymentform,
	bookeroffice = d.bookeroffice,
	bookercode = d.bookercode,
	ticketeroffice = d.ticketeroffice,
	ticketercode = d.ticketercode,
	originator = d.originator,
	origin = d.origin,
	airlinepnrcode = d.airlinepnrcode,
	remarks = d.remarks,
	displaystring = d.displaystring,
	booker = d.booker,
	ticketer = d.ticketer,
	originaldocument = d.originaldocument,
	isticketerrobot = d.isticketerrobot,
	paymentdetails = d.paymentdetails
  from lt_avia_document d
 where p.id = d.id and p.issuedate >= '2014-1-1';
");
		}
	}
	


	[Migration(2014100820)]
	public class Migration_2014100820 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** AviaTicket ***/

alter table lt_product
	add column domestic boolean not null default false,
	add column interline boolean not null default false,
	add column segmentclasses citext2,
	add column departure timestamp,
	add column endorsement citext2,
	add column faretotal_amount numeric(19,5),
	add column faretotal_currency varchar(32),
	
	add constraint ""FK_lt_product_faretotal_currency_lt_currency_id"" 
		foreign key (faretotal_currency) references lt_currency (id)
;
");
		}
	}

	[Migration(2014100821)]
	public class Migration_2014100821 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
create index ""IX_lt_product_faretotal_currency"" on lt_product (faretotal_currency);
");
		}
	}

	[Migration(2014100822)]
	public class Migration_2014100822 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_product p set
	domestic = t.domestic,
	interline = t.interline,
	segmentclasses = t.segmentclasses,
	departure = t.departure,
	endorsement = t.endorsement,
	faretotal_amount = t.faretotal_amount,
	faretotal_currency = t.faretotal_currency
  from lt_avia_ticket t
 where p.id = t.id;
");
		}
	}

	[Migration(2014100830)]
	public class Migration_2014100830 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** AviaMco ***/

alter table lt_product
	add column description citext2,
	add column inconnectionwith varchar(32),
	add constraint ""FK_lt_product_inconnectionwith_lt_product_id"" 
		foreign key (inconnectionwith) references lt_product (id)
;

create index ""IX_lt_product_inconnectionwith"" on lt_product (inconnectionwith);

update lt_product p set
	description = m.description,
	inconnectionwith = m.inconnectionwith
  from lt_avia_mco m
 where m.id = p.id;
");
		}
	}

	[Migration(2014100840)]
	public class Migration_2014100840 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** AviaDocument details ***/

alter table lt_flight_segment
	drop constraint flightsegment_ticket_fkey,
	add constraint ""FK_lt_flight_segment_ticket_lt_product_id""
		foreign key (ticket) references lt_product (id)
;
alter table lt_penalize_operation
	drop constraint penalizeoperation_ticket_fkey,
	add constraint ""FK_lt_penalize_operation_ticket_lt_product_id""
		foreign key (ticket) references lt_product (id)
;
alter table lt_avia_document_fee
	drop constraint aviadocumentfee_document_fkey,
	add constraint ""FK_lt_avia_document_fee_document_lt_product_id""
		foreign key (document) references lt_product (id)
;
alter table lt_avia_document_voiding
	drop constraint aviadocumentvoiding_document_fkey,
	add constraint ""FK_lt_avia_document_voiding_document_lt_product_id""
		foreign key (document) references lt_product (id)
;


drop view if exists olap_direction_dim cascade;
drop view if exists olap_document cascade;
drop view if exists olap_fare_segment_dim cascade;

drop view if exists olap_ticketeroffice_dim cascade;
drop view if exists olap_bookeroffice_dim cascade;
drop view if exists olap_itinerary_dim cascade;
drop view if exists olap_departuredate_dim cascade;
drop view if exists olap_segment_dim cascade;
drop view if exists olap_segmentclass_dim cascade;
drop view if exists olap_tourcode_dim cascade;


drop table lt_avia_ticket;
drop table lt_avia_mco;
drop table lt_avia_refund;
drop table lt_avia_document;
");
		}
	}



	[Migration(2014100906)]
	public class Migration_2014100906 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_accommodation ***/

alter table lt_product
	add column startdate timestamp,
	add column finishdate timestamp,
	add column hotelname citext2,
	add column hoteloffice citext2,
	add column hotelcode citext2,
	add column placementname citext2,
	add column placementoffice citext2,
	add column placementcode citext2,
	add column accommodationtype varchar(32),
	add column cateringtype varchar(32),

	add constraint ""FK_lt_product_accommodationtype_lt_accommodation_type_id"" 
		foreign key (accommodationtype) references lt_accommodation_type (id),
	add constraint ""FK_lt_product_cateringtype_lt_catering_type_id"" 
		foreign key (cateringtype) references lt_catering_type (id)    
;

create index ""IX_lt_product_startdate"" on lt_product (startdate);
create index ""IX_lt_product_finishdate"" on lt_product (finishdate);
create index ""IX_lt_product_accommodationtype"" on lt_product (accommodationtype);
create index ""IX_lt_product_cateringtype"" on lt_product (cateringtype);

update lt_product p set
	startdate = d.startdate,
	finishdate = d.finishdate,
	hotelname = d.hotelname,
	hoteloffice = d.hoteloffice,
	hotelcode = d.hotelcode,
	placementname = d.placementname,
	placementoffice = d.placementoffice,
	placementcode = d.placementcode,
	accommodationtype = d.accommodationtype,
	cateringtype = d.cateringtype
  from lt_accommodation d
 where p.id = d.id;

drop table lt_accommodation;

");
		}
	}

	[Migration(2014100907)]
	public class Migration_2014100907 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_bus_ticket ***/

alter table lt_product
	add column departureplace citext2,
	add column departuredate timestamp,
	add column departuretime citext2,
	add column arrivalplace citext2,
	add column arrivaldate timestamp,
	add column arrivaltime citext2,
	add column seatnumber citext2
;

update lt_product p set
	departureplace = d.departureplace,
	departuredate = d.departuredate,
	departuretime = d.departuretime,
	arrivalplace = d.arrivalplace,
	arrivaldate = d.arrivaldate,
	arrivaltime = d.arrivaltime,
	seatnumber = d.seatnumber
  from lt_bus_ticket d
 where p.id = d.id;

drop table lt_bus_ticket;
");
		}
	}

	[Migration(2014100908)]
	public class Migration_2014100908 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_car_rental ***/

alter table lt_product
	add column carbrand citext2
;

update lt_product p set
	startdate = d.startdate,
	finishdate = d.finishdate,
	carbrand = d.carbrand
  from lt_car_rental d
 where p.id = d.id;

drop table lt_car_rental;
");
		}
	}

	[Migration(2014100909)]
	public class Migration_2014100909 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_excursion ***/

alter table lt_product
	add column tourname citext2
;

update lt_product p set
	startdate = d.startdate,
	finishdate = d.finishdate,
	tourname = d.tourname
  from lt_excursion d
 where p.id = d.id;

drop table lt_excursion;
");
		}
	}

	[Migration(2014100910)]
	public class Migration_2014100910 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_generic_product ***/

alter table lt_product
	add column generictype varchar(32),

	add constraint ""FK_lt_product_generictype_lt_generic_product_type_id"" 
		foreign key (generictype) references lt_generic_product_type (id)
;

create index ""IX_lt_product_generictype"" on lt_product (generictype);

update lt_product p set
	generictype = d.generictype,
	number_ = d.number_,
	startdate = d.startdate,
	finishdate = d.finishdate
  from lt_generic_product d
 where p.id = d.id;

drop table lt_generic_product;
");
		}
	}

	[Migration(2014100911)]
	public class Migration_2014100911 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_insurance ***/

update lt_product p set
	startdate = d.startdate,
	finishdate = d.finishdate,
	number_ = d.number_
  from lt_insurance d
 where p.id = d.id;

drop table lt_insurance;
");
		}
	}

	[Migration(2014100912)]
	public class Migration_2014100912 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_isic ***/

alter table lt_product
	add column cardtype int not null default 0,
	add column number1 citext2,
	add column number2 citext2
;

update lt_product p set
	cardtype = d.cardtype::int,
	number1 = d.number1,
	number2 = d.number2
  from lt_isic d
 where p.id = d.id;

drop table lt_isic;
");
		}
	}

	[Migration(2014100913)]
	public class Migration_2014100913 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_pasteboard ***/

alter table lt_product
	add column firststation citext2,
	add column laststation citext2,
	add column serviceclass integer not null default 0,
	add column trainnumber citext2,
	add column carnumber citext2
;

update lt_product p set
	number_ = d.number_,
	firststation = d.firststation,
	laststation = d.laststation,
	startdate = d.startdate,
	serviceclass = d.serviceclass,
	trainnumber = d.trainnumber,
	carnumber = d.carnumber
  from lt_pasteboard d
 where p.id = d.id;

drop table lt_pasteboard;
");
		}
	}

	[Migration(2014100914)]
	public class Migration_2014100914 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_sim_card ***/

alter table lt_product
	add column issale boolean not null default false
;

update lt_product p set
	number_ = d.number_,
	issale = d.issale
  from lt_sim_card d
 where p.id = d.id;

drop table lt_sim_card;
");
		}
	}

	[Migration(2014100915)]
	public class Migration_2014100915 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_tour ***/

alter table lt_product
	add column aviadescription citext2,
	add column transferdescription citext2 
;

update lt_product p set
	startdate = d.startdate,
	finishdate = d.finishdate,
	hotelname = d.hotelname,
	hoteloffice = d.hoteloffice,
	hotelcode = d.hotelcode,
	placementname = d.placementname,
	placementoffice = d.placementoffice,
	placementcode = d.placementcode,
	aviadescription = d.aviadescription,
	transferdescription = d.transferdescription,
	accommodationtype = d.accommodationtype,
	cateringtype = d.cateringtype
  from lt_tour d
 where p.id = d.id;

drop table lt_tour;
");
		}
	}

	[Migration(2014100916)]
	public class Migration_2014100916 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
/*** lt_transfer ***/

update lt_product p set
	startdate = d.startdate
  from lt_transfer d
 where p.id = d.id;

drop table lt_transfer;
");
		}
	}


	[Migration(2014101201)]
	public class Migration_2014101201 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
alter table lt_product add column class varchar(20);

update lt_product set
	class = case type
		when 0 then 'AviaTicket'
		when 1 then 'AviaRefund'
		when 2 then 'AviaMco'
		when 3 then 'Pasteboard'
		when 4 then 'SimCard'
		when 5 then 'Isic'
		when 6 then 'Excursion'
		when 7 then 'Tour'
		when 8 then 'Accommodation'
		when 9 then 'Transfer'
		when 10 then 'Insurance'
		when 11 then 'CarRental'
		when 12 then 'GenericProduct'
		when 13 then 'BusTicket'
	end
;

alter table lt_product alter column class set not null;

create index ""IX_lt_product_class"" on lt_product (class);
");
		}
	}


	[Migration(2014101401)]
	public class Migration_2014101401 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Create.Index().OnTable("lt_party").OnColumn("name");
			Create.Index().OnTable("lt_party").OnColumn("legalname");
			Create.Index().OnTable("lt_organization").OnColumn("airlineiatacode");
			Create.Index().OnTable("lt_organization").OnColumn("airlineprefixcode");
		}
	}

	[Migration(2014103001)]
	public class Migration_2014103001 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"

alter table lt_party
	add column class varchar(20),
	add column type integer,
	add column organization character varying(32),

	add column birthday date,
	add column milescardsstring citext2,
	add column title citext2,
	add column bonuscardnumber citext2,

	add column code citext2,
	add column isprovider boolean not null default false,
	add column isaccommodationprovider boolean not null default false,
	add column isbusticketprovider boolean not null default false,
	add column iscarrentalprovider boolean not null default false,
	add column isinsuranceprovider boolean not null default false,
	add column ispasteboardprovider boolean not null default false,
	add column istourprovider boolean not null default false,
	add column istransferprovider boolean not null default false,
	add column isairline boolean not null default false,
	add column airlineiatacode citext2,
	add column airlineprefixcode citext2,
	add column airlinepassportrequirement integer not null default 0,
	add column isinsurancecompany boolean not null default false,
	add column isroamingoperator boolean not null default false,


	add constraint ""FK_lt_party_organization_lt_party_id"" 
		foreign key (organization) references lt_party (id)
;

update lt_party p set
	class = 'Department',
	type = 0,
	organization = d.organization
  from lt_department d
 where p.id = d.id
;

update lt_party p set
	class = 'Organization',
	type = 1,
	code = d.code,
	isprovider = d.isprovider,
	isaccommodationprovider = d.isaccommodationprovider,
	isbusticketprovider = d.isbusticketprovider,
	iscarrentalprovider = d.iscarrentalprovider,
	isinsuranceprovider = d.isinsuranceprovider,
	ispasteboardprovider = d.ispasteboardprovider,
	istourprovider = d.istourprovider,
	istransferprovider = d.istransferprovider,
	isairline = d.isairline,
	airlineiatacode = d.airlineiatacode,
	airlineprefixcode = d.airlineprefixcode,
	airlinepassportrequirement = d.airlinepassportrequirement,
	isinsurancecompany = d.isinsurancecompany,
	isroamingoperator = d.isroamingoperator
  from lt_organization d
 where p.id = d.id
;

update lt_party p set
	class = 'Person',
	type = 2,
	organization = d.organization,
	birthday = d.birthday,
	milescardsstring = d.milescardsstring,
	title = d.title,
	bonuscardnumber = d.bonuscardnumber
  from lt_person d
 where p.id = d.id
;


create index ""IX_lt_party_class"" on lt_party (class);
create index ""IX_lt_party_organization"" on lt_party (organization);
create index ""IX_lt_party_airlineiatacode"" on lt_party (airlineiatacode);
create index ""IX_lt_party_airlineprefixcode"" on lt_party (airlineprefixcode);


alter table lt_avia_document_voiding
	drop constraint aviadocumentvoiding_agent_fkey,
	add constraint ""FK_lt_avia_document_voiding_agent_lt_party_id""
		foreign key (agent) references lt_party (id);

alter table lt_document_access
	drop constraint documentaccess_person_fkey,
	add constraint ""FK_lt_document_access_person_lt_party_id""
		foreign key (person) references lt_party (id);

alter table lt_file
	drop constraint file_uploadedby_fkey,
	add constraint ""FK_lt_file_uploadedby_lt_party_id""
		foreign key (uploadedby) references lt_party (id);

alter table lt_gds_agent
	drop constraint gdsagent_person_fkey,
	add constraint ""FK_lt_gds_agent_person_lt_party_id""
		foreign key (person) references lt_party (id);

alter table lt_invoice
	drop constraint invoice_issuedby_fkey,
	add constraint ""FK_lt_invoice_issuedby_lt_party_id""
		foreign key (issuedby) references lt_party (id);

alter table lt_issued_consignment
	drop constraint issuedconsignment_issuedby_fkey,
	add constraint ""FK_lt_issued_consignment_issuedby_lt_party_id""
		foreign key (issuedby) references lt_party (id);

alter table lt_miles_card
	drop constraint milescard_owner_fkey,
	add constraint ""FK_lt_miles_card_owner_lt_party_id""
		foreign key (owner) references lt_party (id);

alter table lt_order
	drop constraint order_assignedto_fkey,
	add constraint ""FK_lt_order_assignedto_lt_party_id""
		foreign key (assignedto) references lt_party (id);

alter table lt_passport
	drop constraint passport_owner_fkey,
	add constraint ""FK_lt_passport_owner_lt_party_id""
		foreign key (owner) references lt_party (id);

alter table lt_payment
	drop constraint payment_assignedto_fkey,
	add constraint ""FK_lt_payment_assignedto_lt_party_id""
		foreign key (assignedto) references lt_party (id);

alter table lt_payment
	drop constraint payment_registeredby_fkey,
	add constraint ""FK_lt_payment_registeredby_lt_party_id""
		foreign key (registeredby) references lt_party (id);

alter table lt_product_passenger
	drop constraint product_passenger_passenger_fkey,
	add constraint ""FK_lt_product_passenger__lt_party_id""
		foreign key (passenger) references lt_party (id);

alter table lt_product
	drop constraint product_seller_fkey,
	add constraint ""FK_lt_product_seller_lt_party_id""
		foreign key (seller) references lt_party (id);

alter table lt_system_configuration
	drop constraint systemconfiguration_birthdaytaskresponsible_fkey,
	add constraint ""FK_lt_system_configuration_birthdaytaskresponsible_lt_party_id""
		foreign key (birthdaytaskresponsible) references lt_party (id);

alter table lt_user
	drop constraint user_person_fkey,
	add constraint ""FK_lt_user_person_lt_party_id""
		foreign key (person) references lt_party (id);

alter table lt_product
	drop constraint ""FK_lt_product_booker_lt_person_id"",
	add constraint ""FK_lt_product_booker_lt_party_id""
		foreign key (booker) references lt_party (id);

alter table lt_product
	drop constraint ""FK_lt_product_ticketer_lt_person_id"",
	add constraint ""FK_lt_product_ticketer_lt_party_id""
		foreign key (ticketer) references lt_party (id);

alter table lt_miles_card
	drop constraint milescard_organization_fkey,
	add constraint ""FK_lt_miles_card_organization_lt_party_id""
		foreign key (organization) references lt_party (id);

alter table lt_system_configuration
	drop constraint systemconfiguration_company_fkey,
	add constraint ""FK_lt_system_configuration_company_lt_party_id""
		foreign key (company) references lt_party (id);

alter table lt_product
	drop constraint fk_lt_product_provider_lt_organization_id,
	add constraint ""FK_lt_product_provider_lt_party_id""
		foreign key (provider) references lt_party (id);

alter table lt_airline_commission_percents
	drop constraint ""FK_lt_airline_commission_percents_airline_lt_organization_id"",
	add constraint ""FK_lt_airline_commission_percents_airline_lt_party_id""
		foreign key (airline) references lt_party (id);

alter table lt_airline_service_class
	drop constraint ""FK_lt_airline_service_class_airline_lt_organization_id"",
	add constraint ""FK_lt_airline_service_class_airline_lt_party_id""
		foreign key (airline) references lt_party (id);

alter table lt_flight_segment
	drop constraint ""FK_lt_flight_segment_carrier_lt_organization_id"",
	add constraint ""FK_lt_flight_segment_carrier_lt_party_id""
		foreign key (carrier) references lt_party (id);

alter table lt_product
	drop constraint ""FK_lt_product_producer_lt_organization_id"",
	add constraint ""FK_lt_product_producer_lt_party_id""
		foreign key (producer) references lt_party (id);


drop view if exists olap_airline_dim;
drop view if exists olap_carrier_dim;

drop table lt_department;
drop table lt_person;
drop table lt_organization;

alter table lt_party alter column class set not null;
alter table lt_party alter column type set not null;


");
		}
	}


	[Migration(2014112504)]
	public class Migration_2014112504 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Create.Table("lt_currency_daily_rate")
				.AsEntity2()
				.WithColumn("date_").AsDate().NotNullable().Unique()
				.WithColumn("uah_eur").AsDecimal(12, 4).Nullable()
				.WithColumn("uah_rub").AsDecimal(12, 4).Nullable()
				.WithColumn("uah_usd").AsDecimal(12, 4).Nullable()
				.WithColumn("rub_eur").AsDecimal(12, 4).Nullable()
				.WithColumn("rub_usd").AsDecimal(12, 4).Nullable()
				.WithColumn("eur_usd").AsDecimal(12, 4).Nullable();
		}
	}


	[Migration(2014120501)]
	public class Migration_2014120501 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_product set 
	departuredate = startdate,
	departureplace = firststation,
	arrivalplace = laststation
 where type = 3");
		}
	}

	[Migration(2014121001)]
	public class Migration_2014121001 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Create.Index().OnTable("lt_product").OnColumn("name");
		}
	}

	[Migration(2014121002)]
	public class Migration_2014121002 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Alter.Table("lt_party")
				.AddColumn("isgenericproductprovider").AsBoolean().NotNullable().WithDefaultValue(false);
		}
	}

	[Migration(2014121003)]
	public class Migration_2014121003 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"
update lt_party pt set isgenericproductprovider = true
 where exists(
	select id from lt_product 
	 where type = 12 and provider = pt.id
 )");
		}
	}


	[Migration(2014121501)]
	public class Migration_2014121501 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Create.Table("lt_bank_account")
				.AsEntity3D()
				.WithColumn("isdefault").AsBoolean().NotNullable().WithDefaultValue(false)
				.WithColumn("note").AsText().Nullable()
			;

			Alter.Table("lt_party")
				.AddColumn("defaultbankaccount").AsRefecence("lt_bank_account").Nullable();

		}
	}

	[Migration(2014121601)]
	public class Migration_2014121601 : ForwardOnlyMigration
	{
		public override void Up()
		{

			Alter.Table("lt_order")
				.AddColumn("bankaccount").AsRefecence("lt_bank_account").Nullable();


			var id = "'" + Guid.NewGuid().ToString("N") + "'";

			Execute.Sql(@"
insert into lt_bank_account (
	id, version, createdon, createdby, name, description, isdefault
)
select
	" + id + @", 0, now(), 'SYSTEM', 'Основной', companydetails, true
  from lt_system_configuration;

update lt_system_configuration set companydetails = null;
");
		}
	}


	[Migration(2014122701)]
	public class Migration_2014122701 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql("drop table lt_currency_daily_rate cascade");

			Create.Table("lt_currency_daily_rate")
				.AsEntity2()
				.WithColumn("date_").AsDate().NotNullable().Unique()
				.WithColumn("uah_eur").AsDecimal(12, 4).Nullable()
				.WithColumn("uah_rub").AsDecimal(12, 4).Nullable()
				.WithColumn("uah_usd").AsDecimal(12, 4).Nullable()
				.WithColumn("rub_eur").AsDecimal(12, 4).Nullable()
				.WithColumn("rub_usd").AsDecimal(12, 4).Nullable()
				.WithColumn("eur_usd").AsDecimal(12, 4).Nullable();
		}
	}


	[Migration(2014122901)]
	public class Migration_2014122901 : ForwardOnlyMigration
	{
		public override void Up()
		{
			Execute.Sql(@"

alter table lt_identity
	add column class varchar(20),
	add column createdby citext2,
	add column createdon timestamp,
	add column modifiedby citext2,
	add column modifiedon timestamp,
	add column password citext2,
	add column active boolean not null default(true),
	add column person character varying(32),
	add column isadministrator boolean not null default(false),
	add column issupervisor boolean not null default(false),
	add column isagent boolean not null default(false),
	add column iscashier boolean not null default(false),
	add column isanalyst boolean not null default(false),
	add column issubagent boolean not null default(false),
	add column description citext2,
	add column sessionid citext2;

update lt_identity i set
	class = 'Internal',
	createdby = 'SYSTEM',
	createdon = now(),
	description = ii.description
  from lt_internal_identity ii
 where i.id = ii.id;


update lt_identity i set
	class = 'User',
	createdby = u.createdby,
	createdon = u.createdon,
	modifiedby = u.modifiedby,
	modifiedon = u.modifiedon,
	password = u.password,
	active = u.active,
	person = u.person,
	isadministrator = u.isadministrator,
	issupervisor = u.issupervisor,
	isagent = u.isagent,
	iscashier = u.iscashier,
	isanalyst = u.isanalyst,
	issubagent = u.issubagent,
	sessionid = u.sessionid
  from lt_user u
 where i.id = u.id;

alter table lt_identity
	alter column class set not null,
	alter column createdby set not null,
	alter column createdon set not null;


drop table lt_internal_identity;
drop table lt_user;

");
		}
	}

}