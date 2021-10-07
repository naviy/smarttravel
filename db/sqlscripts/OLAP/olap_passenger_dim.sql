drop view if exists olap_passenger_dim;


create view olap_passenger_dim
as
select distinct replace(replace(replace(replace(upper(trim(replace(replace(p.passengername, chr(9), ''), chr(160), ' '))), 'Ё', 'Е'), 'Š', 'S'), 'Ć', 'C'), 'Ž', 'Z')::varchar(4000) as passenger
  from lt_product p
 where passengername is not null and not p.isreservation and not p.requiresprocessing and not p.isvoid
 order by 1;

-- select * from olap_passenger_dim