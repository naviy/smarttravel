drop view if exists olap_segment_dim;


create view olap_segment_dim
as
select 
    s.id, 
    s.carrier,
    p.producer as validator_, 
    s.departuretime::date as departure, 
    p.issuedate, 
    s.serviceclasscode::varchar(255) as serviceclass, 
    p.originator, 
    p.owner, 
    p.seller, 
    p.ticketingiataoffice as iataoffice, 
    p.tourcode::varchar(255), 
    s.fromairport, 
    s.toairport
  from lt_flight_segment s
    inner join lt_product p on s.ticket = p.id
 where not p.isreservation and not p.isvoid;
