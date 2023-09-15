drop view if exists olap_country_dim cascade;


create view olap_country_dim
as
select
	id,
	name::varchar(255) as name
  from (
	select distinct 
		coalesce(c.id, '') as id, 
		coalesce(c.name, '')::citext2 as name
	  from lt_airport a
		left join lt_country c on c.id = a.country
	 where 
		a.id in (
			select distinct fromairport from lt_flight_segment union 
			select distinct toairport from lt_flight_segment union
			select distinct airport from olap_direction_from_dim
		)
	 order by 2
  ) a;