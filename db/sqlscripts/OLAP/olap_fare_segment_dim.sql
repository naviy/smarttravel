drop view if exists olap_fare_segment_dim;


create view olap_fare_segment_dim
as 
select 
    s.id, 
    p.id as ticket, 
    p.producer as validator_, 
    p.issuedate, 
    p.originator, 
    p.owner, 
    p.seller, 
    btrim(nullif(nullif(p.ticketingiataoffice, ''), ''''''))::varchar(4000) as iataoffice, 
    p.tourcode::varchar(4000), 
    s.departuretime::date as departure, 
    s.carrier::varchar(255), 
    s.carrieriatacode::varchar(255), 
    s.distance, 
    s.amount_amount as amount, 
    s.amount_currency as currency
  from lt_product p
    inner join lt_flight_segment s on p.id = s.ticket
 where not p.isreservation
   and not p.isvoid 
   and s.type = 0 
   and p.reissuefor is null;