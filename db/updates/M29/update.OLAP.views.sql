
set client_encoding to 'win1251';

drop view olap_carrier_dim;
drop view olap_customer_dim;
drop view olap_direction_dim;
drop view olap_document;
drop view olap_gds_dim;
drop view olap_number_dim;
drop view olap_order_dim;
drop view olap_segment_dim;
drop view olap_segmentclass_dim;
drop view olap_ticketingiataoffice_dim;
drop view olap_transactiondate_dim;
drop view olap_transaction_dim;

create view olap_carrier_dim
as
select id, name 
  from lt_airline
 where id in (select distinct carrier from lt_flight_segment)
 order by name;

create view olap_customer_dim
as
select id, name 
  from lt_party 
 where id in (
    select distinct customer from lt_avia_document union 
    select distinct customer from lt_order union
    select distinct payer from lt_payment) 
 order by name;

create view olap_direction_dim
as
select t.id as ticket, 
   (select 
        case
            when s.toairportcode = 'kbp' then 
               (select s2.fromairport
                  from lt_flight_segment s2
                 where s2.ticket = s.ticket and s2."position" <= s."position" 
                   and s2."position" > 
                          (select coalesce(max(s3."position"), (-1)) as "coalesce"
                             from lt_flight_segment s3
                            where s3.ticket = s.ticket and s3.stopover and s3."position" < s."position")
                 order by s2."position"
                 limit 1)
            else s.toairport
        end as airport
      from lt_flight_segment s
        left join lt_flight_segment next on next.ticket = s.ticket and next."position" = s."position" + 1
     where s.stopover and s.type = 0 and s.ticket::text = t.id::text
     order by 
        case
          when next.id is null then 0::double precision
          else date_part('epoch', next.departuretime - s.arrivaltime)
        end desc
    limit 1
    ) as airport
   from lt_avia_ticket t;


create view olap_document 
as
select 
    d.id, d.issuedate, d.type, d.number_ as number, d.itinerary, 
    d.paymenttype, d.airline, d.seller, d.customer, d.owner, 
    case
        when d.type = 0 then d.id
        else null::character varying
    end as direction, 
    case
        when t.id is null then ''
        when t.domestic then 'd'
        else 'i'
    end as flighttype, 
    btrim(d.ticketeroffice) as ticketeroffice, 
    coalesce(d.equalfare_amount, 0) as equalfare, 
    coalesce(d.total_amount, 0) as total, 
    coalesce(d.commission_amount, 0) as commission, 
    coalesce(d.servicefee_amount, 0) as servicefee, 
    coalesce(d.handling_amount, 0) as handling, 
    coalesce(d.discount_amount, 0) + coalesce(d.commissiondiscount_amount, 0) as discount, 
    coalesce(d.grandtotal_amount, 0) as grandtotal, d.tourcode, 
    coalesce(d.total_amount, 0) - coalesce(d.commission_amount, 0) as totaltotransfer, 
    coalesce(d.commission_amount, 0) + coalesce(d.servicefee_amount, 0) + coalesce(d.handling_amount, 0) - coalesce(d.discount_amount, 0) - coalesce(d.commissiondiscount_amount, 0) as profit, 
    1 as count, 
    btrim(d.ticketingiataoffice::text) as ticketingiataoffice, 
    d.originator as gds, d.total_currency as currency, 
    t.departure::date as departure, 
    btrim(d.bookeroffice) as bookeroffice
   from lt_avia_document d
      left join lt_avia_ticket t on t.id = d.id
  where d.type <> 1 and not d.requiresprocessing and not d.isvoid
  
union all 

select 
    d.id, d.issuedate, d.type, d.number_ as number, d.itinerary, 
    d.paymenttype, d.airline, d.seller, d.customer, d.owner, 
    t.id as direction, 
    case
        when t.id is null then ''
        when t.domestic then 'd'
        else 'i'
    end as flighttype, 
    btrim(d.ticketeroffice) as ticketeroffice, 
    - coalesce(d.equalfare_amount, 0) as equalfare, 
    - coalesce(d.total_amount, 0) as total, 
    - coalesce(d.commission_amount, 0) as commission, 
    - coalesce(d.servicefee_amount, 0) as servicefee, 
    - coalesce(d.handling_amount, 0) as handling, 
    - (coalesce(d.discount_amount, 0) + coalesce(d.commissiondiscount_amount, 0)) as discount, 
    - coalesce(d.grandtotal_amount, 0) as grandtotal, 
    d.tourcode, 
    coalesce(d.commission_amount, 0) - coalesce(d.total_amount, 0) as totaltotransfer, 
    (- coalesce(d.commission_amount, 0)) - coalesce(d.servicefee_amount, 0) - coalesce(d.handling_amount, 0) + coalesce(d.discount_amount, 0) + coalesce(r.cancelcommission_amount, 0) + coalesce(d.commissiondiscount_amount, 0) as profit, 
    (-1) as count, 
    btrim(d.ticketingiataoffice::text) as ticketingiataoffice, 
    d.originator as gds, d.total_currency as currency, 
    t.departure::date as departure, 
    btrim(d.bookeroffice) as bookeroffice
   from lt_avia_document d
      join lt_avia_refund r on r.id = d.id
   left join lt_avia_ticket t on t.id = r.refundeddocument
  where not d.requiresprocessing and not d.isvoid;


create view olap_gds_dim 
as
select 0 as type, 'unknown' as name union all 
select 1 as type, 'amadeus' as name union all 
select 2 as type, 'galileo' as name union all 
select 3 as type, 'sirena' as name union all 
select 4 as type, 'airline' as name;


create view olap_number_dim 
as
select distinct number_ as number 
  from lt_avia_document;


create view olap_order_dim 
as
select id, number_ 
  from lt_order 
 order by number_;


create view olap_segment_dim 
as
select 
    s.id, s.carrier, d.airline as validator_, 
    s.departuretime::date as departure, d.issuedate, 
    s.serviceclasscode as serviceclass, d.originator, d.owner, d.seller, 
    d.ticketingiataoffice as iataoffice, d.tourcode, s.fromairport, s.toairport
  from lt_flight_segment s
    join lt_avia_document d on d.id = s.ticket
 where d.number_ is not null and not d.isvoid;


create view olap_segmentclass_dim
as
select distinct 
    s.serviceclasscode as serviceclass
  from lt_flight_segment s
    join lt_avia_document d on d.id = s.ticket
 where d.number_ is not null and not d.isvoid;


create view olap_ticketingiataoffice_dim 
as
select distinct 
    btrim(ticketingiataoffice)::citext2 as ticketingiataoffice
  from lt_avia_document
 order by 1;


create view olap_transaction_dim
as
select 
    d.customer, d.order_ as "order", d.issuedate, 
    d.displaystring as number, d.grandtotal_currency as currency, 
    case
        when r.id is null then - d.grandtotal_amount
        else d.grandtotal_amount
    end as amount
  from lt_avia_document d
    left join lt_avia_refund r on r.id::text = d.id::text
 where not d.isvoid
union all 
select 
    p.payer as customer, p.order_ as "order", 
    p.postedon as issuedate, p.number_ as number, 
    p.amount_currency as currency, p.amount_amount as amount
   from lt_payment p
  where not p.isvoid and not p.postedon is null;


create view olap_transactiondate_dim
as
select distinct 
    date_part('year', issuedate)::integer as year, 
    to_char(issuedate::timestamp with time zone, 'mm month') as month, 
    to_char(issuedate::timestamp with time zone, 'dd') as day, 
    issuedate
  from olap_transaction_dim
 order by issuedate;