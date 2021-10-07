drop view if exists olap_backdeparture_dim;


create view olap_backdeparture_dim
as 
select distinct 
	s.departuretime::date as departure, 
	date_part('year', s.departuretime) as year, 
	date_part('quarter', s.departuretime) as quarter, 
	date_part('month', s.departuretime) as month, 
	date_part('day', s.departuretime) as day
  from (
	select ticket, max(departuretime) as departuretime
	  from lt_flight_segment 
	 group by ticket
	) s
	inner join lt_product p on p.id = s.ticket
 where not p.isreservation and not p.isvoid
;

/*
select * from olap_lastdeparture_dim
*/