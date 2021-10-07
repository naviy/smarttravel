drop view if exists olap_departuredate_dim;


create view olap_departuredate_dim
as 
select distinct 
    s.departuretime::date as departure, 
    date_part('year', s.departuretime) as year, 
    date_part('quarter', s.departuretime) as quarter, 
    date_part('month', s.departuretime) as month, 
    date_part('day', s.departuretime) as day
  from lt_flight_segment s
    inner join lt_product p on p.id = s.ticket
 where not p.isreservation and not p.isvoid
 order by 1;