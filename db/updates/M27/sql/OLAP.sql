
drop view if exists olap_fare_segment_dim;
drop view if exists olap_departuredate_dim;
drop view if exists olap_currency_dim;
drop view if exists olap_fare_currency_dim;


create view olap_fare_segment_dim
as 
select s.id, d.id as ticket,
    d.airline as validator_, 
    d.issuedate,
    d.originator, d.owner, d.seller, 
    d.ticketingiataoffice as iataoffice, d.tourcode, 
    s.departuretime::date as departure,
    s.carrier, s.carrieriatacode, 
    s.distance,
    s.amount_amount as amount, s.amount_currency as currency
  from lt_avia_document d
    inner join lt_avia_ticket t on d.id = t.id
    inner join lt_flight_segment s on d.id = s.ticket
 where d.number_ is not null and not d.isvoid
   and s.type = 0 and t.reissuefor is null;


create view olap_departuredate_dim
as
select distinct 
    s.departuretime::date as departure, 
    date_part('year', s.departuretime) as year, 
    date_part('quarter', s.departuretime) as quarter,     
    date_part('month', s.departuretime) as month, 
    date_part('day', s.departuretime) as day
  from lt_flight_segment s
    inner join lt_avia_document d on d.id = s.ticket
 where d.number_ is not null and not d.isvoid
 order by s.departuretime::date;

-- select * from olap_departuredate_dim


create view olap_currency_dim
as 
select c.id, c.code
  from 
   (select distinct total_currency as id from lt_avia_document where not isvoid) q
    inner join lt_currency c on  q.id = c.id
 order by c.code;
 
create view olap_fare_currency_dim
as 
select c.id, c.code
  from 
   (select distinct amount_currency as id from lt_flight_segment) q
    inner join lt_currency c on q.id = c.id
 order by c.code;
 

