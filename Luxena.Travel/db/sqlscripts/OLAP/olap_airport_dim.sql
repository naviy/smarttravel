--drop view if exists olap_airport_dim;


create view olap_airport_dim
as 
select 
    a.id, 
	a.code::varchar(255), 
	a.name::varchar(4000), 
    coalesce(a.settlement, '')::varchar(4000) as settlement, 
    coalesce(a.country, '')::varchar(4000) as country
  from lt_airport a
 where 
    id in (
        select distinct fromairport from lt_flight_segment union 
        select distinct toairport from lt_flight_segment union 
        select distinct airport from olap_direction_from_dim
    )
 order by a.code;
