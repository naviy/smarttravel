drop view if exists olap_itinerary_dim;


create view olap_itinerary_dim
as 
select
	replace(replace(itinerary::varchar(255), 'ń', 'n'), 'ł', 'l') as itinerary
  from (
	select distinct itinerary::citext2
	  from lt_product
  ) a;
