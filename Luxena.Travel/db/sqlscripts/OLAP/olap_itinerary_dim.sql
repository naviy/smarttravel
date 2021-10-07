drop view if exists olap_itinerary_dim;


create view olap_itinerary_dim
as 
select
	itinerary::varchar(255) as itinerary
  from (
	select distinct itinerary::citext2
	  from lt_product
  ) a;
