
/*
begin work;

delete from lt_flight_segment sf
 where ticket in (
    select id 
      from lt_temp_charter_documents d 
        inner join lt_product p on d.number_ = p.number_
 );

delete from lt_product_passenger
 where product in (
    select id 
      from lt_temp_charter_documents d 
        inner join lt_product p on d.number_ = p.number_
 );

delete from lt_order_item oi
 where product in (
    select id 
      from lt_temp_charter_documents d 
        inner join lt_product p on d.number_ = p.number_
 );

delete from lt_product
 where id in (
    select id 
      from lt_temp_charter_documents d 
        inner join lt_product p on d.number_ = p.number_
 );

commit;
*/


begin work;


insert into lt_product (
    id,
    version,
    class,
    createdby,
    createdon,
    type,
    issuedate,
    airlineiatacode,
    airlineprefixcode,
    airlinename,
    producer,
    name,
    number_,
    isprocessed,
    isvoid,
    isreservation,
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
    handling_amount,
    handling_currency,
    commission_amount,
    commission_currency,
    grandtotal_amount,
    grandtotal_currency,

    domestic,
    interline,
    departure,
    faretotal_amount,
    faretotal_currency,
    
    order_
)
select 
    replace(cast(uuid_generate_v4() as varchar), '-', '') as id,
    1 as version,
    'AviaTicket' as class,
    d.seller as createdby,
    d.issuedate as createdon,
    0 as type,
    d.issuedate,
    al.airlineiatacode,
    al.airlineprefixcode,
    al.name as airlinename,
    al.id as airline,
    concat(al.airlineprefixcode, '-', d.number_) as name,
    d.number_,
    true as isprocessed,
    false as isvoid,
    false as isreservation,
    false as requiresprocessing,
    d.passengername,
    0 as gdspassportstatus,
    6 as paymenttype,
    0 as originator,
    5 as origin,
    concat(d.airportcode1, '-', d.airportcode2, '-', d.airportcode1) as itinerary,
    d.seller,
    d.owner,
    o.customer,
    d.fare_amount as fare_amount,
    d.fare_currency as fare_currency,
    d.equalfare_amount,
    'UAH' as equalfare_currency,
    d.feestotal_amount,
    'UAH' as feestotal_currency,
    0 as servicefee_amount,
    'UAH' as servicefee_currency,
    d.equalfare_amount + d.feestotal_amount as total_amount,
    'UAH' as total_currency,
    d.handling_amount,
    'UAH' as handling_currency,
    d.commission_amount,
    'UAH' as commission_currency,
    d.grandtotal_amount,
    'UAH' as grandtotal_currency,

    false as domestic,
    false as interline,
    d.departuretime1 as departure,
    d.equalfare_amount as faretotal_amount,
    'UAH' as faretotal_currency,
    
    o.id as order_
  from lt_temp_charter_documents d
    left join lt_party al on d.airlineprefixcode = al.airlineprefixcode
    left join lt_order o on d.orderNumber = o.number_
 order by d.number_;


insert into lt_product_passenger (
    id,
    version,
    createdby,
    createdon,

    product,
    passengername
)
select
    replace(cast(uuid_generate_v4() as varchar), '-', '') as id,
    1 as version,
    'SYSTEM' as createdby,
    d.issuedate as createdon,

    p.id as product,
    d.passengername
  from lt_temp_charter_documents d
    inner join lt_product p on d.number_ = p.number_;


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
    'SYSTEM' as createdby,
    d.issuedate as createdon,

    p.id as ticket,
    num + 1 as "position",

    0 as type,

    case when num = 0 then ap1.id else ap2.id end as fromairport,
    case when num = 0 then ap1.code else ap2.code end as fromairportcode,
    case when num = 0 then ap1.name else ap2.name end as fromairportname,
    
    case when num = 0 then ap2.id else ap1.id end as toairport,
    case when num = 0 then ap2.code else ap1.code end as toairportcode,
    case when num = 0 then ap2.name else ap1.name end as toairportname,

    case when num = 0 then al1.id else al2.id end as carrier,
    case when num = 0 then al1.airlineiatacode else al2.airlineiatacode end as carrieriatacode,
    case when num = 0 then al1.airlineprefixcode else al2.airlineprefixcode end as carrierprefixcode,
    case when num = 0 then al1.name else al2.name end as carriername,
     
    case when num = 0 then d.flightnumber1 else d.flightnumber2 end as flightnumber,
    case when num = 0 then d.departuretime1 else d.departuretime2 end as departuretime,
    false as stopover,
    0 as surcharges,
    false as isinclusive,
    false as issidetrip,
    0 as distance


  from lt_temp_charter_documents d
    inner join lt_product p on d.number_ = p.number_
    left join lt_airport ap1 on d.airportcode1 = ap1.code
    left join lt_airport ap2 on d.airportcode2 = ap2.code
    left join lt_party al1 on d.carriercode1 = al1.airlineiatacode
    left join lt_party al2 on d.carriercode2 = al2.airlineiatacode
    cross join (select 0 as num union select 1) n_
 where (num <> 0 or d.departuretime1 is not null) and (num <> 1 or d.departuretime2 is not null)
 order by d.number_, num;


insert into lt_order_item (
    id,
    version,
    createdby,
    createdon,
    order_,
    product,
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
    replace(cast(uuid_generate_v4() as varchar), '-', '') as id,
    1 as version,
    d.seller as createdby,
    d.issuedate as createdon,
    p.order_,
    p.id as product,
    coalesce((select max("position") from lt_order_item where order_ = p.order_), 0) + row_number() over(order by d.number_) as "position",
    concat('Авіаквиток № ', p.airlineprefixcode, '-', p.number_, ' від ', to_char(p.issuedate, 'DD.MM.YYYY'), /*', маршрут ', p.itinerary,*/ ', ', d.passengername) as text,
    1 as quantity,
    false as hasvat,
    p.grandtotal_amount as price_amount,
    p.grandtotal_currency as price_currency,
    0 as discount_amount,
    p.grandtotal_currency as discount_currency,
    p.grandtotal_amount,
    p.grandtotal_currency
  from lt_temp_charter_documents d
    inner join lt_product p on d.number_ = p.number_
 order by d.number_;


commit;
--rollback;

    
