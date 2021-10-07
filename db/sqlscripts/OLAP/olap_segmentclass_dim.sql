drop view if exists olap_segmentclass_dim;


create view olap_segmentclass_dim
as
select distinct
    s.serviceclasscode::varchar(255) as serviceclass
  from lt_flight_segment s
    inner join lt_product p on s.ticket = p.id
 where not p.isreservation
   and not p.isvoid;
