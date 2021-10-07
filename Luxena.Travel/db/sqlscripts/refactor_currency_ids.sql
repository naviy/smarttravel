--begin work;

/*

select
    -- concat('alter table ', relname, ' alter column ', colname, ' type char(3);')
    concat('alter table ', relname, ' drop constraint "', conname, '";')
  from (
select
    --ns.nspname, conname
    c.conname, rel.relname, --pg_get_constraintdef(c.oid),
    substring(pg_get_constraintdef(c.oid) from 'FOREIGN KEY \((.+?)\)') as colname
  from pg_constraint c
    inner join pg_class cls on cls.oid = c.confrelid
    inner join pg_catalog.pg_namespace ns on ns.oid = c.connamespace
    inner join pg_class rel on rel.oid = c.conrelid
 where cls.relname = 'lt_currency'
   and c.contype = 'f'
   and ns.nspname = 'bsv'
 order by 1
 ) q;

*/

/*
drop view if exists lt_currency_daily_rate cascade;
drop view  if exists lt_currency cascade;
drop view  if exists lt_product cascade;
drop view  if exists lt_flight_segment cascade;
drop view  if exists lt_order cascade;
drop view  if exists lt_payment cascade;

drop view if exists olap_fare_currency_dim cascade;
drop view  if exists olap_currency_dim cascade;
drop view  if exists olap_document cascade;
drop view  if exists olap_fare_segment_dim cascade;
drop view  if exists olap_transaction_dim cascade;
*/

alter table lt_invoice
    drop constraint "FK_lt_invoice_vat_currency_lt_currency_id",
    add constraint "FK_lt_invoice_vat_currency_lt_currency_id" foreign key (vat_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "FK_lt_product_bonusaccumulation_currency_lt_currency_id",
    add constraint "FK_lt_product_bonusaccumulation_currency_lt_currency_id" foreign key (bonusaccumulation_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "FK_lt_product_bonusdiscount_currency_lt_currency_id",
    add constraint "FK_lt_product_bonusdiscount_currency_lt_currency_id" foreign key (bonusdiscount_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "FK_lt_product_faretotal_currency_lt_currency_id",
    add constraint "FK_lt_product_faretotal_currency_lt_currency_id" foreign key (faretotal_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "FK_lt_product_refundservicefee_currency_lt_currency_id",
    add constraint "FK_lt_product_refundservicefee_currency_lt_currency_id" foreign key (refundservicefee_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "FK_lt_product_servicefeepenalty_currency_lt_currency_id",
    add constraint "FK_lt_product_servicefeepenalty_currency_lt_currency_id" foreign key (servicefeepenalty_currency) references lt_currency (id) on update cascade;

alter table lt_avia_document_fee
    drop constraint "aviadocumentfee_amount_currency_fk",
    add constraint "aviadocumentfee_amount_currency_fk" foreign key (amount_currency) references lt_currency (id) on update cascade;

alter table lt_consignment
    drop constraint "consignment_discount_currency_fk",
    add constraint "consignment_discount_currency_fk" foreign key (discount_currency) references lt_currency (id) on update cascade;

alter table lt_consignment
    drop constraint "consignment_grandtotal_currency_fk",
    add constraint "consignment_grandtotal_currency_fk" foreign key (grandtotal_currency) references lt_currency (id) on update cascade;

alter table lt_consignment
    drop constraint "consignment_vat_currency_fk",
    add constraint "consignment_vat_currency_fk" foreign key (vat_currency) references lt_currency (id) on update cascade;

alter table lt_flight_segment
    drop constraint "flightsegment_amount_currency_fk",
    add constraint "flightsegment_amount_currency_fk" foreign key (amount_currency) references lt_currency (id) on update cascade;

alter table lt_invoice
    drop constraint "invoice_total_currency_fk",
    add constraint "invoice_total_currency_fk" foreign key (total_currency) references lt_currency (id) on update cascade;

alter table lt_order
    drop constraint "order_discount_currency_fk",
    add constraint "order_discount_currency_fk" foreign key (discount_currency) references lt_currency (id) on update cascade;

alter table lt_order
    drop constraint "order_paid_currency_fk",
    add constraint "order_paid_currency_fk" foreign key (paid_currency) references lt_currency (id) on update cascade;

alter table lt_order
    drop constraint "order_total_currency_fk",
    add constraint "order_total_currency_fk" foreign key (total_currency) references lt_currency (id) on update cascade;

alter table lt_order
    drop constraint "order_totaldue_currency_fk",
    add constraint "order_totaldue_currency_fk" foreign key (totaldue_currency) references lt_currency (id) on update cascade;

alter table lt_order
    drop constraint "order_vat_currency_fk",
    add constraint "order_vat_currency_fk" foreign key (vat_currency) references lt_currency (id) on update cascade;

alter table lt_order
    drop constraint "order_vatdue_currency_fk",
    add constraint "order_vatdue_currency_fk" foreign key (vatdue_currency) references lt_currency (id) on update cascade;

alter table lt_order_item
    drop constraint "orderitem_discount_currency_fk",
    add constraint "orderitem_discount_currency_fk" foreign key (discount_currency) references lt_currency (id) on update cascade;

alter table lt_order_item
    drop constraint "orderitem_givenvat_currency_fk",
    add constraint "orderitem_givenvat_currency_fk" foreign key (givenvat_currency) references lt_currency (id) on update cascade;

alter table lt_order_item
    drop constraint "orderitem_grandtotal_currency_fk",
    add constraint "orderitem_grandtotal_currency_fk" foreign key (grandtotal_currency) references lt_currency (id) on update cascade;

alter table lt_order_item
    drop constraint "orderitem_price_currency_fk",
    add constraint "orderitem_price_currency_fk" foreign key (price_currency) references lt_currency (id) on update cascade;

alter table lt_order_item
    drop constraint "orderitem_taxedtotal_currency_fk",
    add constraint "orderitem_taxedtotal_currency_fk" foreign key (taxedtotal_currency) references lt_currency (id) on update cascade;


alter table lt_payment
    drop constraint "payment_amount_currency_fk",
    add constraint "payment_amount_currency_fk" foreign key (amount_currency) references lt_currency (id) on update cascade;

alter table lt_payment
    drop constraint "payment_vat_currency_fk",
    add constraint "payment_vat_currency_fk" foreign key (vat_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_cancelcommission_currency_fk",
    add constraint "product_cancelcommission_currency_fk" foreign key (cancelcommission_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_cancelfee_currency_fk",
    add constraint "product_cancelfee_currency_fk" foreign key (cancelfee_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_commission_currency_fk",
    add constraint "product_commission_currency_fk" foreign key (commission_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_commissiondiscount_currency_fk",
    add constraint "product_commissiondiscount_currency_fk" foreign key (commissiondiscount_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_discount_currency_fk",
    add constraint "product_discount_currency_fk" foreign key (discount_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_equalfare_currency_fk",
    add constraint "product_equalfare_currency_fk" foreign key (equalfare_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_fare_currency_fk",
    add constraint "product_fare_currency_fk" foreign key (fare_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_feestotal_currency_fk",
    add constraint "product_feestotal_currency_fk" foreign key (feestotal_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_grandtotal_currency_fk",
    add constraint "product_grandtotal_currency_fk" foreign key (grandtotal_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_handling_currency_fk",
    add constraint "product_handling_currency_fk" foreign key (handling_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_servicefee_currency_fk",
    add constraint "product_servicefee_currency_fk" foreign key (servicefee_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_total_currency_fk",
    add constraint "product_total_currency_fk" foreign key (total_currency) references lt_currency (id) on update cascade;

alter table lt_product
    drop constraint "product_vat_currency_fk",
    add constraint "product_vat_currency_fk" foreign key (vat_currency) references lt_currency (id) on update cascade;

alter table lt_system_configuration
    drop constraint "systemconfiguration_defaultcurrency_fkey",
    add constraint "systemconfiguration_defaultcurrency_fkey" foreign key (defaultcurrency) references lt_currency (id) on update cascade;


update lt_currency set id = code;


alter table lt_invoice drop constraint "FK_lt_invoice_vat_currency_lt_currency_id";
alter table lt_product drop constraint "FK_lt_product_bonusaccumulation_currency_lt_currency_id";
alter table lt_product drop constraint "FK_lt_product_bonusdiscount_currency_lt_currency_id";
alter table lt_product drop constraint "FK_lt_product_faretotal_currency_lt_currency_id";
alter table lt_product drop constraint "FK_lt_product_refundservicefee_currency_lt_currency_id";
alter table lt_product drop constraint "FK_lt_product_servicefeepenalty_currency_lt_currency_id";
alter table lt_avia_document_fee drop constraint "aviadocumentfee_amount_currency_fk";
alter table lt_consignment drop constraint "consignment_discount_currency_fk";
alter table lt_consignment drop constraint "consignment_grandtotal_currency_fk";
alter table lt_consignment drop constraint "consignment_vat_currency_fk";
alter table lt_flight_segment drop constraint "flightsegment_amount_currency_fk";
alter table lt_invoice drop constraint "invoice_total_currency_fk";
alter table lt_order drop constraint "order_discount_currency_fk";
alter table lt_order drop constraint "order_paid_currency_fk";
alter table lt_order drop constraint "order_total_currency_fk";
alter table lt_order drop constraint "order_totaldue_currency_fk";
alter table lt_order drop constraint "order_vat_currency_fk";
alter table lt_order drop constraint "order_vatdue_currency_fk";
alter table lt_order_item drop constraint "orderitem_discount_currency_fk";
alter table lt_order_item drop constraint "orderitem_givenvat_currency_fk";
alter table lt_order_item drop constraint "orderitem_grandtotal_currency_fk";
alter table lt_order_item drop constraint "orderitem_price_currency_fk";
alter table lt_order_item drop constraint "orderitem_taxedtotal_currency_fk";
alter table lt_payment drop constraint "payment_amount_currency_fk";
alter table lt_payment drop constraint "payment_vat_currency_fk";
alter table lt_product drop constraint "product_cancelcommission_currency_fk";
alter table lt_product drop constraint "product_cancelfee_currency_fk";
alter table lt_product drop constraint "product_commission_currency_fk";
alter table lt_product drop constraint "product_commissiondiscount_currency_fk";
alter table lt_product drop constraint "product_discount_currency_fk";
alter table lt_product drop constraint "product_equalfare_currency_fk";
alter table lt_product drop constraint "product_fare_currency_fk";
alter table lt_product drop constraint "product_feestotal_currency_fk";
alter table lt_product drop constraint "product_grandtotal_currency_fk";
alter table lt_product drop constraint "product_handling_currency_fk";
alter table lt_product drop constraint "product_servicefee_currency_fk";
alter table lt_product drop constraint "product_total_currency_fk";
alter table lt_product drop constraint "product_vat_currency_fk";
alter table lt_system_configuration drop constraint "systemconfiguration_defaultcurrency_fkey";


drop view olap_fare_currency_dim cascade;
drop view olap_currency_dim cascade;
drop view olap_document cascade;
drop view olap_fare_segment_dim cascade;
drop view olap_transaction_dim cascade;

alter table lt_currency alter column id type char(3);

alter table lt_invoice alter column vat_currency type char(3);
alter table lt_product alter column bonusaccumulation_currency type char(3);
alter table lt_product alter column bonusdiscount_currency type char(3);
alter table lt_product alter column faretotal_currency type char(3);
alter table lt_product alter column refundservicefee_currency type char(3);
alter table lt_product alter column servicefeepenalty_currency type char(3);
alter table lt_avia_document_fee alter column amount_currency type char(3);
alter table lt_consignment alter column discount_currency type char(3);
alter table lt_consignment alter column grandtotal_currency type char(3);
alter table lt_consignment alter column vat_currency type char(3);
alter table lt_flight_segment alter column amount_currency type char(3);
alter table lt_invoice alter column total_currency type char(3);
alter table lt_order alter column discount_currency type char(3);
alter table lt_order alter column paid_currency type char(3);
alter table lt_order alter column total_currency type char(3);
alter table lt_order alter column totaldue_currency type char(3);
alter table lt_order alter column vat_currency type char(3);
alter table lt_order alter column vatdue_currency type char(3);
alter table lt_order_item alter column discount_currency type char(3);
alter table lt_order_item alter column givenvat_currency type char(3);
alter table lt_order_item alter column grandtotal_currency type char(3);
alter table lt_order_item alter column price_currency type char(3);
alter table lt_order_item alter column taxedtotal_currency type char(3);
alter table lt_payment alter column amount_currency type char(3);
alter table lt_payment alter column vat_currency type char(3);
alter table lt_product alter column cancelcommission_currency type char(3);
alter table lt_product alter column cancelfee_currency type char(3);
alter table lt_product alter column commission_currency type char(3);
alter table lt_product alter column commissiondiscount_currency type char(3);
alter table lt_product alter column discount_currency type char(3);
alter table lt_product alter column equalfare_currency type char(3);
alter table lt_product alter column fare_currency type char(3);
alter table lt_product alter column feestotal_currency type char(3);
alter table lt_product alter column grandtotal_currency type char(3);
alter table lt_product alter column handling_currency type char(3);
alter table lt_product alter column servicefee_currency type char(3);
alter table lt_product alter column total_currency type char(3);
alter table lt_product alter column vat_currency type char(3);
alter table lt_system_configuration alter column defaultcurrency type char(3);



truncate table lt_currency_daily_rate;

alter table lt_currency_daily_rate 
    alter column uah_eur type numeric(16,8),
    alter column uah_rub type numeric(16,8),
    alter column uah_usd type numeric(16,8),
    alter column rub_eur type numeric(16,8),
    alter column rub_usd type numeric(16,8),
    alter column eur_usd type numeric(16,8);

insert into lt_currency_daily_rate (
    id, version, createdby, createdon, 
    date_, uah_eur, uah_rub, uah_usd
)
select
    replace(cast(uuid_generate_v4() as varchar), '-', '') as id, 1,
    'SYSTEM', now(), 
    q.*
  from (
    select cast('2013-01-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-01-31' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-02-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-03-31' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-04-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-05-31' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-06-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-07-31' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-08-31' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-09-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-10-31' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-11-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-03' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-04' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-05' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-06' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-07' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-08' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-09' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-10' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-11' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-12' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-13' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-14' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-15' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-16' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-17' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-18' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-19' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-20' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-21' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-22' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-23' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-24' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-25' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-26' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-27' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-28' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-29' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-30' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2013-12-31' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-01' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-02' as date) as date_, cast(null as numeric(16,8)) as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-03' as date) as date_, 11.2291 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-04' as date) as date_, 11.2291 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-05' as date) as date_, 11.2291 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-06' as date) as date_, 11.2291 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-07' as date) as date_, 11.2291 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-08' as date) as date_, 11.1942 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-09' as date) as date_, 11.2465 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-10' as date) as date_, 11.2709 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-11' as date) as date_, 11.3311 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-12' as date) as date_, 11.3311 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-13' as date) as date_, 11.3635 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-14' as date) as date_, 11.3908 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-15' as date) as date_, 11.3572 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-16' as date) as date_, 11.3461 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-17' as date) as date_, 11.3835 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-18' as date) as date_, 11.3835 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-19' as date) as date_, 11.3835 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-20' as date) as date_, 11.3721 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-21' as date) as date_, 11.3883 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-22' as date) as date_, 11.45 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-23' as date) as date_, 11.5227 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-24' as date) as date_, 11.7061 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-25' as date) as date_, 11.7061 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-26' as date) as date_, 11.7061 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-27' as date) as date_, 11.8906 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-28' as date) as date_, 11.6463 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-29' as date) as date_, 11.6019 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-30' as date) as date_, 11.7416 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-01-31' as date) as date_, 11.7326 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-02-01' as date) as date_, 11.7326 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-02-02' as date) as date_, 11.7326 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-02-03' as date) as date_, 11.7985 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-02-04' as date) as date_, 12.0357 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-02-05' as date) as date_, 12.2349 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-02-06' as date) as date_, 11.969 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 7.993 as uah_usd union all
    select cast('2014-02-07' as date) as date_, 11.5908 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.708 as uah_usd union all
    select cast('2014-02-08' as date) as date_, 11.5908 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.708 as uah_usd union all
    select cast('2014-02-09' as date) as date_, 11.5908 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.708 as uah_usd union all
    select cast('2014-02-10' as date) as date_, 11.5865 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.5282 as uah_usd union all
    select cast('2014-02-11' as date) as date_, 11.8144 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.5532 as uah_usd union all
    select cast('2014-02-12' as date) as date_, 11.8239 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.5507 as uah_usd union all
    select cast('2014-02-13' as date) as date_, 12.0766 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.5507 as uah_usd union all
    select cast('2014-02-14' as date) as date_, 12.0787 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.6309 as uah_usd union all
    select cast('2014-02-15' as date) as date_, 12.0787 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.6309 as uah_usd union all
    select cast('2014-02-16' as date) as date_, 12.0787 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.6309 as uah_usd union all
    select cast('2014-02-17' as date) as date_, 12.1831 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.6405 as uah_usd union all
    select cast('2014-02-18' as date) as date_, 12.2044 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.6458 as uah_usd union all
    select cast('2014-02-19' as date) as date_, 12.3753 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.6869 as uah_usd union all
    select cast('2014-02-20' as date) as date_, 12.4423 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.838 as uah_usd union all
    select cast('2014-02-21' as date) as date_, 12.585 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.838 as uah_usd union all
    select cast('2014-02-22' as date) as date_, 12.585 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.838 as uah_usd union all
    select cast('2014-02-23' as date) as date_, 12.585 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.838 as uah_usd union all
    select cast('2014-02-24' as date) as date_, 12.8467 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 8.938 as uah_usd union all
    select cast('2014-02-25' as date) as date_, 13.4515 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.0377 as uah_usd union all
    select cast('2014-02-26' as date) as date_, 14.4164 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.4269 as uah_usd union all
    select cast('2014-02-27' as date) as date_, 15.2934 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.5179 as uah_usd union all
    select cast('2014-02-28' as date) as date_, 13.387 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11 as uah_usd union all
    select cast('2014-03-01' as date) as date_, 13.387 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.1 as uah_usd union all
    select cast('2014-03-02' as date) as date_, 13.387 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.1 as uah_usd union all
    select cast('2014-03-03' as date) as date_, 14.4595 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.1 as uah_usd union all
    select cast('2014-03-04' as date) as date_, 13.0816 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.5 as uah_usd union all
    select cast('2014-03-05' as date) as date_, 12.912 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.6 as uah_usd union all
    select cast('2014-03-06' as date) as date_, 12.7171 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.4 as uah_usd union all
    select cast('2014-03-07' as date) as date_, 12.7 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.45 as uah_usd union all
    select cast('2014-03-08' as date) as date_, 12.7 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.2 as uah_usd union all
    select cast('2014-03-09' as date) as date_, 12.7 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.2 as uah_usd union all
    select cast('2014-03-10' as date) as date_, 12.7 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.2 as uah_usd union all
    select cast('2014-03-11' as date) as date_, 12.804 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.2 as uah_usd union all
    select cast('2014-03-12' as date) as date_, 13.1347 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.24 as uah_usd union all
    select cast('2014-03-13' as date) as date_, 13.7349 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.35 as uah_usd union all
    select cast('2014-03-14' as date) as date_, 13.8066 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 9.9 as uah_usd union all
    select cast('2014-03-15' as date) as date_, 13.8066 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.05 as uah_usd union all
    select cast('2014-03-16' as date) as date_, 13.8066 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.05 as uah_usd union all
    select cast('2014-03-17' as date) as date_, 14.404 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.05 as uah_usd union all
    select cast('2014-03-18' as date) as date_, 14.2075 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.1 as uah_usd union all
    select cast('2014-03-19' as date) as date_, 14.2035 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.3 as uah_usd union all
    select cast('2014-03-20' as date) as date_, 14.3124 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.2 as uah_usd union all
    select cast('2014-03-21' as date) as date_, 15.0552 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.4 as uah_usd union all
    select cast('2014-03-22' as date) as date_, 15.0552 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.63 as uah_usd union all
    select cast('2014-03-23' as date) as date_, 15.0552 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.63 as uah_usd union all
    select cast('2014-03-24' as date) as date_, 14.8724 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.63 as uah_usd union all
    select cast('2014-03-25' as date) as date_, 15.2134 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 10.9 as uah_usd union all
    select cast('2014-03-26' as date) as date_, 15.5671 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11 as uah_usd union all
    select cast('2014-03-27' as date) as date_, 15.5541 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.2 as uah_usd union all
    select cast('2014-03-28' as date) as date_, 15.5484 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.3 as uah_usd union all
    select cast('2014-03-29' as date) as date_, 15.5484 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.3 as uah_usd union all
    select cast('2014-03-30' as date) as date_, 15.5484 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.3 as uah_usd union all
    select cast('2014-03-31' as date) as date_, 15.9846 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.3 as uah_usd union all
    select cast('2014-04-01' as date) as date_, 15.8597 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.45 as uah_usd union all
    select cast('2014-04-02' as date) as date_, 15.7891 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.5 as uah_usd union all
    select cast('2014-04-03' as date) as date_, 15.8874 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.5 as uah_usd union all
    select cast('2014-04-04' as date) as date_, 15.9668 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-04-05' as date) as date_, 15.9668 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-04-06' as date) as date_, 15.9668 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-04-07' as date) as date_, 16.1868 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-04-08' as date) as date_, 16.3264 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.9 as uah_usd union all
    select cast('2014-04-09' as date) as date_, 17.1765 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.8 as uah_usd union all
    select cast('2014-04-10' as date) as date_, 18.3074 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.15 as uah_usd union all
    select cast('2014-04-11' as date) as date_, 18.4541 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.9 as uah_usd union all
    select cast('2014-04-12' as date) as date_, 18.4541 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.5 as uah_usd union all
    select cast('2014-04-13' as date) as date_, 18.4541 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.5 as uah_usd union all
    select cast('2014-04-14' as date) as date_, 17.9755 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.5 as uah_usd union all
    select cast('2014-04-15' as date) as date_, 16.908 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.4 as uah_usd union all
    select cast('2014-04-16' as date) as date_, 15.7715 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.3 as uah_usd union all
    select cast('2014-04-17' as date) as date_, 15.8716 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.4 as uah_usd union all
    select cast('2014-04-18' as date) as date_, 16.3094 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.4 as uah_usd union all
    select cast('2014-04-19' as date) as date_, 16.3094 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.4 as uah_usd union all
    select cast('2014-04-20' as date) as date_, 16.3094 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.4 as uah_usd union all
    select cast('2014-04-21' as date) as date_, 16.3094 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.4 as uah_usd union all
    select cast('2014-04-22' as date) as date_, 16.37 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.4 as uah_usd union all
    select cast('2014-04-23' as date) as date_, 16.12 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.7 as uah_usd union all
    select cast('2014-04-24' as date) as date_, 15.9006 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-04-25' as date) as date_, 16.047 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.5 as uah_usd union all
    select cast('2014-04-26' as date) as date_, 16.047 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-04-27' as date) as date_, 16.047 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-04-28' as date) as date_, 16.4295 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-04-29' as date) as date_, 16.0783 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.9 as uah_usd union all
    select cast('2014-04-30' as date) as date_, 16.1994 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.8 as uah_usd union all
    select cast('2014-05-01' as date) as date_, 16.1994 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-05-02' as date) as date_, 16.1994 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-05-03' as date) as date_, 16.1994 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-05-04' as date) as date_, 16.1994 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-05-05' as date) as date_, 16.4449 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.6 as uah_usd union all
    select cast('2014-05-06' as date) as date_, 16.7286 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.8 as uah_usd union all
    select cast('2014-05-07' as date) as date_, 16.6505 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-05-08' as date) as date_, 16.5255 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.2 as uah_usd union all
    select cast('2014-05-09' as date) as date_, 16.5255 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.8 as uah_usd union all
    select cast('2014-05-10' as date) as date_, 16.5255 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.8 as uah_usd union all
    select cast('2014-05-11' as date) as date_, 16.5255 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.8 as uah_usd union all
    select cast('2014-05-12' as date) as date_, 16.2457 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.8 as uah_usd union all
    select cast('2014-05-13' as date) as date_, 16.247 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.85 as uah_usd union all
    select cast('2014-05-14' as date) as date_, 16.466 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.85 as uah_usd union all
    select cast('2014-05-15' as date) as date_, 16.3197 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-05-16' as date) as date_, 16.4199 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-05-17' as date) as date_, 16.4199 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-05-18' as date) as date_, 16.4199 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-05-19' as date) as date_, 16.5334 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-05-20' as date) as date_, 16.5117 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.1 as uah_usd union all
    select cast('2014-05-21' as date) as date_, 16.321 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-05-22' as date) as date_, 16.4048 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.05 as uah_usd union all
    select cast('2014-05-23' as date) as date_, 16.4249 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.95 as uah_usd union all
    select cast('2014-05-24' as date) as date_, 16.4249 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-05-25' as date) as date_, 16.4249 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-05-26' as date) as date_, 16.2892 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-05-27' as date) as date_, 16.3236 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.95 as uah_usd union all
    select cast('2014-05-28' as date) as date_, 16.2517 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.95 as uah_usd union all
    select cast('2014-05-29' as date) as date_, 16.1334 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.95 as uah_usd union all
    select cast('2014-05-30' as date) as date_, 16.1719 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.9 as uah_usd union all
    select cast('2014-05-31' as date) as date_, 16.1719 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.85 as uah_usd union all
    select cast('2014-06-01' as date) as date_, 16.1719 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.85 as uah_usd union all
    select cast('2014-06-02' as date) as date_, 16.4057 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.85 as uah_usd union all
    select cast('2014-06-03' as date) as date_, 16.3796 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.9 as uah_usd union all
    select cast('2014-06-04' as date) as date_, 16.3508 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.05 as uah_usd union all
    select cast('2014-06-05' as date) as date_, 16.1479 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12 as uah_usd union all
    select cast('2014-06-06' as date) as date_, 16.2407 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.93 as uah_usd union all
    select cast('2014-06-07' as date) as date_, 16.2407 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.88 as uah_usd union all
    select cast('2014-06-08' as date) as date_, 16.2407 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.88 as uah_usd union all
    select cast('2014-06-09' as date) as date_, 16.2407 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.88 as uah_usd union all
    select cast('2014-06-10' as date) as date_, 15.809 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.88 as uah_usd union all
    select cast('2014-06-11' as date) as date_, 15.9233 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.86 as uah_usd union all
    select cast('2014-06-12' as date) as date_, 16.0021 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.72 as uah_usd union all
    select cast('2014-06-13' as date) as date_, 16.1457 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.75 as uah_usd union all
    select cast('2014-06-14' as date) as date_, 16.1457 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.9 as uah_usd union all
    select cast('2014-06-15' as date) as date_, 16.1457 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.9 as uah_usd union all
    select cast('2014-06-16' as date) as date_, 16.1417 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.9 as uah_usd union all
    select cast('2014-06-17' as date) as date_, 16.2417 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.94 as uah_usd union all
    select cast('2014-06-18' as date) as date_, 16.2764 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.96 as uah_usd union all
    select cast('2014-06-19' as date) as date_, 16.2098 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.98 as uah_usd union all
    select cast('2014-06-20' as date) as date_, 16.2876 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.97 as uah_usd union all
    select cast('2014-06-21' as date) as date_, 16.2876 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.94 as uah_usd union all
    select cast('2014-06-22' as date) as date_, 16.2876 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.94 as uah_usd union all
    select cast('2014-06-23' as date) as date_, 16.2628 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.94 as uah_usd union all
    select cast('2014-06-24' as date) as date_, 16.2779 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.96 as uah_usd union all
    select cast('2014-06-25' as date) as date_, 16.2239 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.96 as uah_usd union all
    select cast('2014-06-26' as date) as date_, 16.2127 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.95 as uah_usd union all
    select cast('2014-06-27' as date) as date_, 16.1121 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.93 as uah_usd union all
    select cast('2014-06-28' as date) as date_, 16.1121 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.86 as uah_usd union all
    select cast('2014-06-29' as date) as date_, 16.1121 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.86 as uah_usd union all
    select cast('2014-06-30' as date) as date_, 16.1121 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.86 as uah_usd union all
    select cast('2014-07-01' as date) as date_, 16.3044 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.85 as uah_usd union all
    select cast('2014-07-02' as date) as date_, 16.2348 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.86 as uah_usd union all
    select cast('2014-07-03' as date) as date_, 16.15 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.91 as uah_usd union all
    select cast('2014-07-04' as date) as date_, 16.0417 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.9 as uah_usd union all
    select cast('2014-07-05' as date) as date_, 16.0417 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.862 as uah_usd union all
    select cast('2014-07-06' as date) as date_, 16.0417 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.862 as uah_usd union all
    select cast('2014-07-07' as date) as date_, 16.0181 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.862 as uah_usd union all
    select cast('2014-07-08' as date) as date_, 15.85 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.8129 as uah_usd union all
    select cast('2014-07-09' as date) as date_, 16.06 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.68 as uah_usd union all
    select cast('2014-07-10' as date) as date_, 16 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.79 as uah_usd union all
    select cast('2014-07-11' as date) as date_, 15.9494 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.73 as uah_usd union all
    select cast('2014-07-12' as date) as date_, 15.9494 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.74 as uah_usd union all
    select cast('2014-07-13' as date) as date_, 15.9494 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.74 as uah_usd union all
    select cast('2014-07-14' as date) as date_, 15.9888 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.74 as uah_usd union all
    select cast('2014-07-15' as date) as date_, 15.98 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.75 as uah_usd union all
    select cast('2014-07-16' as date) as date_, 15.865 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.75 as uah_usd union all
    select cast('2014-07-17' as date) as date_, 15.8521 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.75 as uah_usd union all
    select cast('2014-07-18' as date) as date_, 15.85 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.72 as uah_usd union all
    select cast('2014-07-19' as date) as date_, 15.85 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.69 as uah_usd union all
    select cast('2014-07-20' as date) as date_, 15.85 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.69 as uah_usd union all
    select cast('2014-07-21' as date) as date_, 15.81 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.69 as uah_usd union all
    select cast('2014-07-22' as date) as date_, 15.76 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.7 as uah_usd union all
    select cast('2014-07-23' as date) as date_, 15.7596 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.69 as uah_usd union all
    select cast('2014-07-24' as date) as date_, 15.88 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.7 as uah_usd union all
    select cast('2014-07-25' as date) as date_, 16.05 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.78 as uah_usd union all
    select cast('2014-07-26' as date) as date_, 16.05 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.94 as uah_usd union all
    select cast('2014-07-27' as date) as date_, 16.05 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.94 as uah_usd union all
    select cast('2014-07-28' as date) as date_, 16.3 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 11.94 as uah_usd union all
    select cast('2014-07-29' as date) as date_, 16.389 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.18 as uah_usd union all
    select cast('2014-07-30' as date) as date_, 16.36 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.2 as uah_usd union all
    select cast('2014-07-31' as date) as date_, 16.45 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.25 as uah_usd union all
    select cast('2014-08-01' as date) as date_, 16.6 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.35 as uah_usd union all
    select cast('2014-08-02' as date) as date_, 16.6 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.4 as uah_usd union all
    select cast('2014-08-03' as date) as date_, 16.6 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.4 as uah_usd union all
    select cast('2014-08-04' as date) as date_, 16.78 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.4 as uah_usd union all
    select cast('2014-08-05' as date) as date_, 16.7295 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.5 as uah_usd union all
    select cast('2014-08-06' as date) as date_, 16.68 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.48 as uah_usd union all
    select cast('2014-08-07' as date) as date_, 16.8444 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.5 as uah_usd union all
    select cast('2014-08-08' as date) as date_, 17.0098 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.6 as uah_usd union all
    select cast('2014-08-09' as date) as date_, 17.0098 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.71 as uah_usd union all
    select cast('2014-08-10' as date) as date_, 17.0098 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.71 as uah_usd union all
    select cast('2014-08-11' as date) as date_, 17.6781 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.71 as uah_usd union all
    select cast('2014-08-12' as date) as date_, 18.1498 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.2 as uah_usd union all
    select cast('2014-08-13' as date) as date_, 17.82 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.6 as uah_usd union all
    select cast('2014-08-14' as date) as date_, 17.41 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.3 as uah_usd union all
    select cast('2014-08-15' as date) as date_, 17.602 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.15 as uah_usd union all
    select cast('2014-08-16' as date) as date_, 17.602 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.18 as uah_usd union all
    select cast('2014-08-17' as date) as date_, 17.602 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.18 as uah_usd union all
    select cast('2014-08-18' as date) as date_, 17.6742 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.18 as uah_usd union all
    select cast('2014-08-19' as date) as date_, 17.65 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.22 as uah_usd union all
    select cast('2014-08-20' as date) as date_, 17.8 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.23 as uah_usd union all
    select cast('2014-08-21' as date) as date_, 17.9057 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.42 as uah_usd union all
    select cast('2014-08-22' as date) as date_, 18.3116 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.45 as uah_usd union all
    select cast('2014-08-23' as date) as date_, 18.3116 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.7 as uah_usd union all
    select cast('2014-08-24' as date) as date_, 18.3116 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.7 as uah_usd union all
    select cast('2014-08-25' as date) as date_, 18.3116 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.7 as uah_usd union all
    select cast('2014-08-26' as date) as date_, 18.868 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.7 as uah_usd union all
    select cast('2014-08-27' as date) as date_, 18.5 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.17 as uah_usd union all
    select cast('2014-08-28' as date) as date_, 18.3923 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14 as uah_usd union all
    select cast('2014-08-29' as date) as date_, 18.2 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.95 as uah_usd union all
    select cast('2014-08-30' as date) as date_, 18.2 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.2 as uah_usd union all
    select cast('2014-08-31' as date) as date_, 18.2 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.2 as uah_usd union all
    select cast('2014-09-01' as date) as date_, 17.5162 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.2 as uah_usd union all
    select cast('2014-09-02' as date) as date_, 16.9357 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.5 as uah_usd union all
    select cast('2014-09-03' as date) as date_, 16.7797 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.9 as uah_usd union all
    select cast('2014-09-04' as date) as date_, 17.0462 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 12.7 as uah_usd union all
    select cast('2014-09-05' as date) as date_, 17.2131 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.1 as uah_usd union all
    select cast('2014-09-06' as date) as date_, 17.2131 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.3 as uah_usd union all
    select cast('2014-09-07' as date) as date_, 17.2131 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.3 as uah_usd union all
    select cast('2014-09-08' as date) as date_, 17.3375 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.3 as uah_usd union all
    select cast('2014-09-09' as date) as date_, 17.1686 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.35 as uah_usd union all
    select cast('2014-09-10' as date) as date_, 17.3192 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.25 as uah_usd union all
    select cast('2014-09-11' as date) as date_, 17.5205 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.4 as uah_usd union all
    select cast('2014-09-12' as date) as date_, 17.8905 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.66 as uah_usd union all
    select cast('2014-09-13' as date) as date_, 17.8905 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.8 as uah_usd union all
    select cast('2014-09-14' as date) as date_, 17.8905 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.8 as uah_usd union all
    select cast('2014-09-15' as date) as date_, 18.2895 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.8 as uah_usd union all
    select cast('2014-09-16' as date) as date_, 18.411 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.2 as uah_usd union all
    select cast('2014-09-17' as date) as date_, 18.4851 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.3 as uah_usd union all
    select cast('2014-09-18' as date) as date_, 18.3095 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.25 as uah_usd union all
    select cast('2014-09-19' as date) as date_, 18.7857 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.3 as uah_usd union all
    select cast('2014-09-20' as date) as date_, 18.7857 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.8 as uah_usd union all
    select cast('2014-09-21' as date) as date_, 18.7857 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.8 as uah_usd union all
    select cast('2014-09-22' as date) as date_, 18.9297 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.8 as uah_usd union all
    select cast('2014-09-23' as date) as date_, 19.2729 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.19 as uah_usd union all
    select cast('2014-09-24' as date) as date_, 18.292 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15 as uah_usd union all
    select cast('2014-09-25' as date) as date_, 16.9182 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.4 as uah_usd union all
    select cast('2014-09-26' as date) as date_, 17.3128 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.4 as uah_usd union all
    select cast('2014-09-27' as date) as date_, 17.3128 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.4 as uah_usd union all
    select cast('2014-09-28' as date) as date_, 17.3128 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.4 as uah_usd union all
    select cast('2014-09-29' as date) as date_, 17.4127 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.4 as uah_usd union all
    select cast('2014-09-30' as date) as date_, 17.4087 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.4 as uah_usd union all
    select cast('2014-10-01' as date) as date_, 16.7151 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.4 as uah_usd union all
    select cast('2014-10-02' as date) as date_, 16.6963 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.35 as uah_usd union all
    select cast('2014-10-03' as date) as date_, 16.5686 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.32 as uah_usd union all
    select cast('2014-10-04' as date) as date_, 16.5686 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.32 as uah_usd union all
    select cast('2014-10-05' as date) as date_, 16.5686 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.32 as uah_usd union all
    select cast('2014-10-06' as date) as date_, 16.4127 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.15 as uah_usd union all
    select cast('2014-10-07' as date) as date_, 16.5033 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.15 as uah_usd union all
    select cast('2014-10-08' as date) as date_, 16.5693 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.11 as uah_usd union all
    select cast('2014-10-09' as date) as date_, 16.7132 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.18 as uah_usd union all
    select cast('2014-10-10' as date) as date_, 16.58 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.15 as uah_usd union all
    select cast('2014-10-11' as date) as date_, 16.58 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.12 as uah_usd union all
    select cast('2014-10-12' as date) as date_, 16.58 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.12 as uah_usd union all
    select cast('2014-10-13' as date) as date_, 16.6285 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.12 as uah_usd union all
    select cast('2014-10-14' as date) as date_, 16.7906 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.14 as uah_usd union all
    select cast('2014-10-15' as date) as date_, 16.651 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.18 as uah_usd union all
    select cast('2014-10-16' as date) as date_, 16.7576 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.18 as uah_usd union all
    select cast('2014-10-17' as date) as date_, 16.7739 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.1 as uah_usd union all
    select cast('2014-10-18' as date) as date_, 16.7739 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.1 as uah_usd union all
    select cast('2014-10-19' as date) as date_, 16.7739 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.1 as uah_usd union all
    select cast('2014-10-20' as date) as date_, 16.6786 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.1 as uah_usd union all
    select cast('2014-10-21' as date) as date_, 16.7342 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.11 as uah_usd union all
    select cast('2014-10-22' as date) as date_, 16.5984 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.11 as uah_usd union all
    select cast('2014-10-23' as date) as date_, 16.7312 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.12 as uah_usd union all
    select cast('2014-10-24' as date) as date_, 16.6464 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.11 as uah_usd union all
    select cast('2014-10-25' as date) as date_, 16.6464 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.11 as uah_usd union all
    select cast('2014-10-26' as date) as date_, 16.6464 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.11 as uah_usd union all
    select cast('2014-10-27' as date) as date_, 16.583 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.11 as uah_usd union all
    select cast('2014-10-28' as date) as date_, 16.8053 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.13 as uah_usd union all
    select cast('2014-10-29' as date) as date_, 16.9187 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.5 as uah_usd union all
    select cast('2014-10-30' as date) as date_, 16.7482 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.5 as uah_usd union all
    select cast('2014-10-31' as date) as date_, 16.645 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.45 as uah_usd union all
    select cast('2014-11-01' as date) as date_, 16.645 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.48 as uah_usd union all
    select cast('2014-11-02' as date) as date_, 16.645 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.48 as uah_usd union all
    select cast('2014-11-03' as date) as date_, 16.7396 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.48 as uah_usd union all
    select cast('2014-11-04' as date) as date_, 16.5932 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.46 as uah_usd union all
    select cast('2014-11-05' as date) as date_, 17.2803 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 13.46 as uah_usd union all
    select cast('2014-11-06' as date) as date_, 17.8486 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14 as uah_usd union all
    select cast('2014-11-07' as date) as date_, 18.4379 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 14.6 as uah_usd union all
    select cast('2014-11-08' as date) as date_, 18.4379 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.2 as uah_usd union all
    select cast('2014-11-09' as date) as date_, 18.4379 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.2 as uah_usd union all
    select cast('2014-11-10' as date) as date_, 19.5727 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.2 as uah_usd union all
    select cast('2014-11-11' as date) as date_, 19.9932 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.9 as uah_usd union all
    select cast('2014-11-12' as date) as date_, 20.0011 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.25 as uah_usd union all
    select cast('2014-11-13' as date) as date_, 19.8395 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.05 as uah_usd union all
    select cast('2014-11-14' as date) as date_, 19.5427 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.92 as uah_usd union all
    select cast('2014-11-15' as date) as date_, 19.5427 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.74 as uah_usd union all
    select cast('2014-11-16' as date) as date_, 19.5427 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.74 as uah_usd union all
    select cast('2014-11-17' as date) as date_, 19.5435 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.74 as uah_usd union all
    select cast('2014-11-18' as date) as date_, 19.5046 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.675 as uah_usd union all
    select cast('2014-11-19' as date) as date_, 19.5011 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.59 as uah_usd union all
    select cast('2014-11-20' as date) as date_, 19.5355 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.54 as uah_usd union all
    select cast('2014-11-21' as date) as date_, 19.1314 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.56 as uah_usd union all
    select cast('2014-11-22' as date) as date_, 19.1314 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.4 as uah_usd union all
    select cast('2014-11-23' as date) as date_, 19.1314 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.4 as uah_usd union all
    select cast('2014-11-24' as date) as date_, 19.0553 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.4 as uah_usd union all
    select cast('2014-11-25' as date) as date_, 19.0949 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.34 as uah_usd union all
    select cast('2014-11-26' as date) as date_, 19.2078 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.325 as uah_usd union all
    select cast('2014-11-27' as date) as date_, 19.2955 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.37 as uah_usd union all
    select cast('2014-11-28' as date) as date_, 19.1696 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.45 as uah_usd union all
    select cast('2014-11-29' as date) as date_, 19.1696 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.38 as uah_usd union all
    select cast('2014-11-30' as date) as date_, 19.1696 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.38 as uah_usd union all
    select cast('2014-12-01' as date) as date_, 19.2688 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.38 as uah_usd union all
    select cast('2014-12-02' as date) as date_, 19.2416 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.42 as uah_usd union all
    select cast('2014-12-03' as date) as date_, 19.2051 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.52 as uah_usd union all
    select cast('2014-12-04' as date) as date_, 19.4631 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.6 as uah_usd union all
    select cast('2014-12-05' as date) as date_, 19.4542 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.71 as uah_usd union all
    select cast('2014-12-06' as date) as date_, 19.4542 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.85 as uah_usd union all
    select cast('2014-12-07' as date) as date_, 19.4542 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.85 as uah_usd union all
    select cast('2014-12-08' as date) as date_, 19.4883 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.85 as uah_usd union all
    select cast('2014-12-09' as date) as date_, 19.7856 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.87 as uah_usd union all
    select cast('2014-12-10' as date) as date_, 19.848 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 15.96 as uah_usd union all
    select cast('2014-12-11' as date) as date_, 19.8671 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16 as uah_usd union all
    select cast('2014-12-12' as date) as date_, 20.0445 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.04 as uah_usd union all
    select cast('2014-12-13' as date) as date_, 20.0445 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.1 as uah_usd union all
    select cast('2014-12-14' as date) as date_, 20.0445 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.1 as uah_usd union all
    select cast('2014-12-15' as date) as date_, 20.062 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.1 as uah_usd union all
    select cast('2014-12-16' as date) as date_, 20.3271 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.14 as uah_usd union all
    select cast('2014-12-17' as date) as date_, 20.5864 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.25 as uah_usd union all
    select cast('2014-12-18' as date) as date_, 20.1857 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.53 as uah_usd union all
    select cast('2014-12-19' as date) as date_, 19.9081 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.45 as uah_usd union all
    select cast('2014-12-20' as date) as date_, 19.9081 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.21 as uah_usd union all
    select cast('2014-12-21' as date) as date_, 19.9081 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.21 as uah_usd union all
    select cast('2014-12-22' as date) as date_, 20.0287 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.21 as uah_usd union all
    select cast('2014-12-23' as date) as date_, 19.7105 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.35 as uah_usd union all
    select cast('2014-12-24' as date) as date_, 19.8331 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.2 as uah_usd union all
    select cast('2014-12-25' as date) as date_, 19.8817 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.27 as uah_usd union all
    select cast('2014-12-26' as date) as date_, 19.7657 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.23 as uah_usd union all
    select cast('2014-12-27' as date) as date_, 19.7657 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.24 as uah_usd union all
    select cast('2014-12-28' as date) as date_, 19.7657 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.24 as uah_usd union all
    select cast('2014-12-29' as date) as date_, 19.9688 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.24 as uah_usd union all
    select cast('2014-12-30' as date) as date_, 19.9508 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.38 as uah_usd union all
    select cast('2014-12-31' as date) as date_, 19.9508 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.38 as uah_usd union all
    select cast('2015-01-01' as date) as date_, 19.9508 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.38 as uah_usd union all
    select cast('2015-01-02' as date) as date_, 19.9508 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.38 as uah_usd union all
    select cast('2015-01-03' as date) as date_, 19.9508 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.38 as uah_usd union all
    select cast('2015-01-04' as date) as date_, 19.9508 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.38 as uah_usd union all
    select cast('2015-01-05' as date) as date_, 19.9508 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.38 as uah_usd union all
    select cast('2015-01-06' as date) as date_, 19.4186 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.38 as uah_usd union all
    select cast('2015-01-07' as date) as date_, 19.4186 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.31 as uah_usd union all
    select cast('2015-01-08' as date) as date_, 19.1465 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.31 as uah_usd union all
    select cast('2015-01-09' as date) as date_, 19.2014 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.23 as uah_usd union all
    select cast('2015-01-10' as date) as date_, 19.2014 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.3 as uah_usd union all
    select cast('2015-01-11' as date) as date_, 19.2014 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.3 as uah_usd union all
    select cast('2015-01-12' as date) as date_, 19.4531 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.3 as uah_usd union all
    select cast('2015-01-13' as date) as date_, 19.2494 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.3 as uah_usd union all
    select cast('2015-01-14' as date) as date_, 19.1839 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.37 as uah_usd union all
    select cast('2015-01-15' as date) as date_, 19.0232 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.27 as uah_usd union all
    select cast('2015-01-16' as date) as date_, 18.8562 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.35 as uah_usd union all
    select cast('2015-01-17' as date) as date_, 18.8562 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.33 as uah_usd union all
    select cast('2015-01-18' as date) as date_, 18.8562 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.33 as uah_usd union all
    select cast('2015-01-19' as date) as date_, 19.0816 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.33 as uah_usd union all
    select cast('2015-01-20' as date) as date_, 18.7873 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.42 as uah_usd union all
    select cast('2015-01-21' as date) as date_, 18.88 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.21 as uah_usd union all
    select cast('2015-01-22' as date) as date_, 18.6535 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.22 as uah_usd union all
    select cast('2015-01-23' as date) as date_, 18.3469 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.27 as uah_usd union all
    select cast('2015-01-24' as date) as date_, 18.3469 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.32 as uah_usd union all
    select cast('2015-01-25' as date) as date_, 18.3469 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.32 as uah_usd union all
    select cast('2015-01-26' as date) as date_, 18.3539 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.32 as uah_usd union all
    select cast('2015-01-27' as date) as date_, 18.5396 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.29 as uah_usd union all
    select cast('2015-01-28' as date) as date_, 18.5451 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.3 as uah_usd union all
    select cast('2015-01-29' as date) as date_, 18.6784 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.4 as uah_usd union all
    select cast('2015-01-30' as date) as date_, 18.7337 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.48 as uah_usd union all
    select cast('2015-01-31' as date) as date_, 18.7337 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.58 as uah_usd union all
    select cast('2015-02-01' as date) as date_, 18.7337 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.58 as uah_usd union all
    select cast('2015-02-02' as date) as date_, 18.8371 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.58 as uah_usd union all
    select cast('2015-02-03' as date) as date_, 19.2158 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.62 as uah_usd union all
    select cast('2015-02-04' as date) as date_, 19.5059 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 16.8 as uah_usd union all
    select cast('2015-02-05' as date) as date_, 25.3524 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 17.1 as uah_usd union all
    select cast('2015-02-06' as date) as date_, 28.5768 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 22.2 as uah_usd union all
    select cast('2015-02-07' as date) as date_, 28.5768 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 25.2 as uah_usd union all
    select cast('2015-02-08' as date) as date_, 28.5768 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 25.2 as uah_usd union all
    select cast('2015-02-09' as date) as date_, 29.3395 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 25.2 as uah_usd union all
    select cast('2015-02-10' as date) as date_, 29.2954 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 25.9 as uah_usd union all
    select cast('2015-02-11' as date) as date_, 29.5929 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 25.9 as uah_usd union all
    select cast('2015-02-12' as date) as date_, 30.3979 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 26.2 as uah_usd union all
    select cast('2015-02-13' as date) as date_, 29.9355 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 26.7 as uah_usd union all
    select cast('2015-02-14' as date) as date_, 29.9355 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 26.25 as uah_usd union all
    select cast('2015-02-15' as date) as date_, 29.9355 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 26.25 as uah_usd union all
    select cast('2015-02-16' as date) as date_, 30.3532 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 26.25 as uah_usd union all
    select cast('2015-02-17' as date) as date_, 30.666 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 26.6 as uah_usd union all
    select cast('2015-02-18' as date) as date_, 30.9827 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 26.9 as uah_usd union all
    select cast('2015-02-19' as date) as date_, 31.2647 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 27.3 as uah_usd union all
    select cast('2015-02-20' as date) as date_, 33.5431 as uah_eur, cast(null as numeric(16,8)) as uah_rub, 27.5 as uah_usd union all
    select cast('2015-02-21' as date) as date_, 33.5431 as uah_eur, 0.4779 as uah_rub, 29.7 as uah_usd union all
    select cast('2015-02-22' as date) as date_, 33.5431 as uah_eur, 0.4779 as uah_rub, 29.7 as uah_usd union all
    select cast('2015-02-23' as date) as date_, 33.5431 as uah_eur, 0.4779 as uah_rub, 29.7 as uah_usd
) q;


--rollback;    