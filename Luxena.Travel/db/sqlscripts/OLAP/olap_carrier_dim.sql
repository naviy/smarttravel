drop view if exists olap_carrier_dim;


create view olap_carrier_dim
as
select pt.id, pt.name::varchar(4000)
  from lt_party pt
 where isairline and pt.id in (select distinct carrier from lt_flight_segment)
 order by 2;
