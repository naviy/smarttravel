drop view if exists olap_settlement_dim;


create view olap_settlement_dim
as
select
	settlement::varchar(4000) as settlement,
	country
  from (
select distinct
    coalesce(a.settlement, '')::citext2 as settlement, 
    coalesce(a.country, '') as country
  from lt_airport a
 where id in (
    select distinct fromairport from lt_flight_segment union 
    select distinct toairport from lt_flight_segment union 
	select distinct airport from olap_direction_from_dim)
 order by 1
) a;