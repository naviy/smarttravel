drop view if exists lt_airport cascade;
drop view if exists lt_country cascade;
drop view if exists lt_currency cascade;
drop view if exists lt_currency_daily_rate cascade;
drop view if exists lt_flight_segment cascade;
drop view if exists lt_order cascade;
drop view if exists lt_party cascade;
drop view if exists lt_payment cascade;
drop view if exists lt_product cascade;
drop view if exists lt_airline_month_commission cascade;
drop view if exists lt_avia_document_fee;

-- create view lt_airline
-- as 
-- select id, name from utb.lt_airline
-- union
-- select id, name from ufsa.lt_airline;

create view lt_airport
as 
select 
	id, code, name, settlement, country 
  from ufsa.lt_airport
union
select
	a.id, 
	case when c.id is null then a.code else a.code || ' (УТБ)' end as code, 
	a.name, a.settlement, a.country 
  from utb.lt_airport a
    left join ufsa.lt_airport b on a.id = b.id
    left join ufsa.lt_airport c on a.code = c.code
 where b.id is null
union
select
	a.id, 
	case when c.id is null then a.code else a.code || ' (ТОВ УФСА)' end as code, 
	a.name, a.settlement, a.country 
  from tovufsa.lt_airport a
    left join ufsa.lt_airport b on a.id = b.id
    left join ufsa.lt_airport c on a.code = c.code
 where b.id is null;
 

create view lt_product
as 
select 
    *, case when issuedate < '2016-1-1' then 4::numeric(19,5) else 5::numeric(19,5) end as vatpc 
  from tovufsa.lt_product
 where issuedate <= case when date_part('hour', current_time) < 21 then current_date - 1 else current_date end
union all
select 
    *, case when issuedate < '2016-1-1' then 4::numeric(19,5) else 5::numeric(19,5) end as vatpc 
  from utb.lt_product
 where issuedate <= case when date_part('hour', current_time) < 21 then current_date - 1 else current_date end
union all
select 
    *, 20::numeric(19,5) as vatpc 
  from ufsa.lt_product
 where issuedate <= case when date_part('hour', current_time) < 21 then current_date - 1 else current_date end
   and owner not in ('006382cbfb92403ca8613fbde34da5da', 'dfc9912b31e546b5bf83cdfd6e310b03');


create view lt_avia_document_fee
as
select document, code, amount_amount from tovufsa.lt_avia_document_fee
union all
select document, code, amount_amount from utb.lt_avia_document_fee
union all
select document, code, amount_amount from ufsa.lt_avia_document_fee
;
	


create view lt_country
as 
select id, name from tovufsa.lt_country
union
select id, name from utb.lt_country
union
select id, name from ufsa.lt_country;

create view lt_currency
as 
select id, code from tovufsa.lt_currency
union
select id, code from utb.lt_currency
union
select id, code from ufsa.lt_currency;

create view lt_currency_daily_rate
as 
select * from ufsa.lt_currency_daily_rate;

create view lt_flight_segment
as 
select * from tovufsa.lt_flight_segment
union all
select * from utb.lt_flight_segment
union all
select f.* 
  from ufsa.lt_flight_segment f
    inner join ufsa.lt_product p on f.ticket = p.id
 where p.owner not in ('006382cbfb92403ca8613fbde34da5da', 'dfc9912b31e546b5bf83cdfd6e310b03');


create view lt_order
as 
select * from tovufsa.lt_order
union all
select * from utb.lt_order
union all
select * from ufsa.lt_order
 where owner not in ('006382cbfb92403ca8613fbde34da5da', 'dfc9912b31e546b5bf83cdfd6e310b03');


create view lt_party
as 
select 
    id, 
    array_to_string(array_agg(name::citext), ', ')::citext2 as name,
    max(isairline::int)::boolean as isairline,
    max(isaccommodationprovider::int)::boolean as isaccommodationprovider,
    max(isbusticketprovider::int)::boolean as isbusticketprovider,
    max(iscarrentalprovider::int)::boolean as iscarrentalprovider,
    max(ispasteboardprovider::int)::boolean as ispasteboardprovider,
    max(istourprovider::int)::boolean as istourprovider,
    max(istransferprovider::int)::boolean as istransferprovider,
    max(isgenericproductprovider::int)::boolean as isgenericproductprovider,
    max(isinsurancecompany::int)::boolean as isinsurancecompany,
    max(isroamingoperator::int)::boolean as isroamingoperator
  from (
	select 
        id, name, 
        isairline, isaccommodationprovider, isbusticketprovider, iscarrentalprovider, ispasteboardprovider, 
        istourprovider, istransferprovider, isgenericproductprovider, isinsurancecompany, isroamingoperator
	  from tovufsa.lt_party
	UNION 
	select 
        id, name, 
        isairline, isaccommodationprovider, isbusticketprovider, iscarrentalprovider, ispasteboardprovider, 
        istourprovider, istransferprovider, isgenericproductprovider, isinsurancecompany, isroamingoperator
	  from utb.lt_party
	UNION 
	select 
        id, name, 
        isairline, isaccommodationprovider, isbusticketprovider, iscarrentalprovider, ispasteboardprovider, 
        istourprovider, istransferprovider, isgenericproductprovider, isinsurancecompany, isroamingoperator
	  from ufsa.lt_party
  ) q
 group by id;

create view lt_payment
as 
select * from tovufsa.lt_payment
union all
select * from utb.lt_payment
union all
select * from ufsa.lt_payment
 where owner not in ('006382cbfb92403ca8613fbde34da5da', 'dfc9912b31e546b5bf83cdfd6e310b03');


create view lt_airline_month_commission
as
select * from ufsa.lt_airline_month_commission;


/***

grant select on table tovufsa.lt_party to utb_ufsa;
grant select on table tovufsa.lt_airport to utb_ufsa;	
grant select on table tovufsa.lt_product to utb_ufsa;
grant select on table tovufsa.lt_country to utb_ufsa;
grant select on table tovufsa.lt_currency to utb_ufsa;
grant select on table tovufsa.lt_flight_segment to utb_ufsa;
grant select on table tovufsa.lt_order to utb_ufsa;
grant select on table tovufsa.lt_party to utb_ufsa;
grant select on table tovufsa.lt_payment to utb_ufsa;
grant select on table tovufsa.lt_product to utb_ufsa;
grant select on table tovufsa.lt_avia_document_fee to utb_ufsa;

grant select on table utb.lt_party to utb_ufsa;
grant select on table utb.lt_airport to utb_ufsa;
grant select on table utb.lt_product to utb_ufsa;
grant select on table utb.lt_country to utb_ufsa;
grant select on table utb.lt_currency to utb_ufsa;
grant select on table utb.lt_flight_segment to utb_ufsa;
grant select on table utb.lt_order to utb_ufsa;
grant select on table utb.lt_party to utb_ufsa;
grant select on table utb.lt_payment to utb_ufsa;
grant select on table utb.lt_product to utb_ufsa;
grant select on table utb.lt_avia_document_fee to utb_ufsa;

grant select on table ufsa.lt_party to utb_ufsa;
grant select on table ufsa.lt_airport to utb_ufsa;
grant select on table ufsa.lt_product to utb_ufsa;
grant select on table ufsa.lt_country to utb_ufsa;
grant select on table ufsa.lt_currency to utb_ufsa;
grant select on table ufsa.lt_flight_segment to utb_ufsa;
grant select on table ufsa.lt_order to utb_ufsa;
grant select on table ufsa.lt_party to utb_ufsa;
grant select on table ufsa.lt_payment to utb_ufsa;
grant select on table ufsa.lt_product to utb_ufsa;
grant select on table ufsa.lt_currency_daily_rate to utb_ufsa;
grant select on table ufsa.lt_airline_month_commission to utb_ufsa;
grant select on table ufsa.lt_avia_document_fee to utb_ufsa;

***/

-- select * from lt_airport;