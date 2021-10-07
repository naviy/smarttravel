begin work;


insert into lt_avia_document (
    id,
    version,
    createdby,
    createdon,
    type,
    issuedate,
    airlineiatacode,
    airlineprefixcode,
    airlinename,
    airline,
    number_,
    isprocessed,
    isvoid,
    requiresprocessing,
    passengername,
    gdspassportstatus,
    paymenttype,
    originator,
    origin,
    itinerary,
    seller,
    owner,
    customer,
    fare_amount,
    fare_currency,
    equalfare_amount,
    equalfare_currency,
    feestotal_amount,
    feestotal_currency,
    servicefee_amount,
    servicefee_currency,
    total_amount,
    total_currency,
    grandtotal_amount,
    grandtotal_currency,
    order_
)
select 
    replace(cast(uuid_generate_v4() as varchar), '-', '') as id,
    1 as version,
    'cc68a242538811dfbffbc430a29b25e7' as createdby,
    '2013-08-29' as createdon,
    0 as type,
    '2013-08-29' as issuedate,
    d.airline as airlineiatacode,
    al.prefixcode as airlineprefixcode,
    al.name as airlinename,
    al.id as airline,
    d.number_,
    false as isprocessed,
    false as isvoid,
    false as requiresprocessing,
    d.passengername,
    0 as gdspassportstatus,
    6 as paymenttype,
    0 as originator,
    5 as origin,
    concat(d.airport1, '-', d.carrier1, '-', d.airport2, '-', d.carrier2, '-', d.airport3) as itinerary,
    p1.id as seller,
    p2.id as owner,
    o.customer,
    d.fare as fare_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as fare_currency,
    d.fare as equalfare_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as equalfare_currency,
    d.feestotal as feestotal_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as feestotal_currency,
    d.servicefee as servicefee_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as servicefee_currency,
    d.fare + d.feestotal + d.servicefee as total_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as total_currency,
    d.totaldue as grandtotal_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as grandtotal_currency,
    o.id as order_
  from lt_temp_charter_documents d
    left join lt_airline al on d.airline = al.iatacode
    left join lt_order o on d.orderNo = o.number_
    left join lt_party p1 on d.sellername = p1.name
    left join lt_party p2 on d.ownername = p2.name
 order by d.rowNo;

insert into lt_avia_ticket (
    id,
    domestic,
    interline,
    departure,
    faretotal_amount,
    faretotal_currency
)
select
    t.id,
    false as domestic,
    false as interline,
    d.departuretime1,
    t.fare_amount as faretotal_amount,
    t.fare_currency as faretotal_currency
  from lt_temp_charter_documents d
    inner join lt_avia_document t on d.number_ = t.number_;


insert into lt_flight_segment (
    id,
    version,
    createdby,
    createdon,

    ticket,
    "position",

    type,

    fromairport,
    fromairportcode,
    fromairportname,
    
    toairport,
    toairportcode,
    toairportname,

    carrier,
    carrieriatacode,
    carrierprefixcode,
    carriername,
     
    flightnumber,
    departuretime,
    stopover,
    surcharges,
    isinclusive,
    issidetrip,
    distance
)
select
    replace(cast(uuid_generate_v4() as varchar), '-', '') as id,
    1 as version,
    'cc68a242538811dfbffbc430a29b25e7' as createdby,
    '2013-08-29' as createdon,

    t.id as ticket,
    num + 1 as "position",

    0 as type,

    case when num = 0 then ap1.id else ap2.id end as fromairport,
    case when num = 0 then ap1.code else ap2.code end as fromairportcode,
    case when num = 0 then ap1.name else ap2.name end as fromairportname,
    
    case when num = 0 then ap2.id else ap3.id end as toairport,
    case when num = 0 then ap2.code else ap3.code end as toairportcode,
    case when num = 0 then ap2.name else ap3.name end as toairportname,

    case when num = 0 then al1.id else al2.id end as carrier,
    case when num = 0 then al1.iatacode else al2.iatacode end as carrieriatacode,
    case when num = 0 then al1.prefixcode else al2.prefixcode end as carrierprefixcode,
    case when num = 0 then al1.name else al2.name end as carriername,
     
    case when num = 0 then d.flightnumber1 else d.flightnumber2 end as flightnumber,
    case when num = 0 then d.departuretime1 else d.departuretime2 end as departuretime,
    false as stopover,
    0 as surcharges,
    false as isinclusive,
    false as issidetrip,
    0 as distance


  from lt_temp_charter_documents d
    inner join lt_avia_document t on d.number_ = t.number_
    left join lt_airport ap1 on d.airport1 = ap1.code
    left join lt_airport ap2 on d.airport2 = ap2.code
    left join lt_airport ap3 on d.airport3 = ap3.code    
    left join lt_airline al1 on d.carrier1 = al1.iatacode
    left join lt_airline al2 on d.carrier2 = al2.iatacode
    cross join (select 0 as num union select 1) n_
 where (num <> 1 or d.carrier2 is not null)
 order by d.rowNo, num;


create temp table order_items on commit drop 
as
select
    replace(cast(uuid_generate_v4() as varchar), '-', '') as id,
    1 as version,
    cast('cc68a242538811dfbffbc430a29b25e7' as varchar(32)) as createdby,
    cast('2013-08-29' as timestamp without time zone) as createdon,
    t.order_,
    t.id as document,
    2 as linktype,
    coalesce((select max("position") from lt_order_item where order_ = t.order_), 0) + row_number() over(order by d.rowNo) as "position",
    concat('Оплата за авіаквиток №', t.airlineprefixcode, '-', t.number_, ' від ', to_char(t.issuedate, 'DD.MM.YYYY'), ', маршрут ', d.airport1, '-', d.airport2, '-', d.airport3) as text,
    1 as quantity,
    false as hasvat,
    t.grandtotal_amount as price_amount,
    t.grandtotal_currency as price_currency,
    0 as discount_amount,
    t.grandtotal_currency as discount_currency,
    t.grandtotal_amount,
    t.grandtotal_currency
  from lt_temp_charter_documents d
    inner join lt_avia_document t on d.number_ = t.number_
 order by d.rowNo;


insert into lt_order_item (
    id,
    version,
    createdby,
    createdon,
    order_,
    "position",    
    text,
    quantity,
    hasvat,
    price_amount,
    price_currency,
    discount_amount,
    discount_currency,
    grandtotal_amount,
    grandtotal_currency
)
select
    id,
    version,
    createdby,
    createdon,
    order_,
    "position",    
    text,
    quantity,
    hasvat,
    price_amount,
    price_currency,
    discount_amount,
    discount_currency,
    grandtotal_amount,
    grandtotal_currency
  from order_items;


insert into lt_order_item_source_link (id)
select id
  from order_items;

insert into lt_order_item_avia_link (id, linktype, document)
select id, linktype, document
  from order_items;


commit;
--rollback;

    
