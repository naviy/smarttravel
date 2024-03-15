drop view if exists olap_itinerary_dim;


create view olap_itinerary_dim
as 
select distinct 
    replace(replace(replace(replace(replace(itinerary::varchar(255), 'Ł', 'L'), 'Ń', 'N'), 'Ó', 'O'), 'Ś', 'S'), 'Ź', 'Z') as itinerary
  from (
	select distinct upper(itinerary)::citext2 as itinerary
	  from lt_product
  ) a;
