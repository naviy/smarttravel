--set search_path = utb, public;

begin work;

-- select max(number_) + 1 as MaxNumber from lt_avia_document where number_ < 10000000

drop table if exists charters;


create temp table charters -- on commit drop
as
select
	replace(cast(uuid_generate_v4() as varchar), '-', '') as id,
	tno, rowno, ticketCount,
	9154846 + 119 * tno + rowno as number_,
	al.id as airline, airlineprefixcode, tourcode, issuedate,
	'8b7006c355bb4b9c9e83f0b1b3e18104'::varchar(32) as seller, 
	'57ebe1c76c5f47f4ac5cd14b5a770c81'::varchar(32) as owner
  from (
	select 0 as tno, '042'::citext2 as airlineprefixcode, '5761'::citext2 as tourcode, '2014-07-10'::date as issuedate, 119 as ticketCount union
	select 1 as tno, '042'::citext2 as airlineprefixcode, '5761'::citext2 as tourcode, '2014-07-21'::date as issuedate, 119 as ticketCount union
	select 2 as tno, '042'::citext2 as airlineprefixcode, '5761'::citext2 as tourcode, '2014-07-31'::date as issuedate, 119 as ticketCount union
	select 3 as tno, '042'::citext2 as airlineprefixcode, '5761'::citext2 as tourcode, '2014-08-11'::date as issuedate, 119 as ticketCount union
	select 4 as tno, '042'::citext2 as airlineprefixcode, '5761'::citext2 as tourcode, '2014-08-21'::date as issuedate, 119 as ticketCount union
	select 5 as tno, '042'::citext2 as airlineprefixcode, '5761'::citext2 as tourcode, '2014-08-31'::date as issuedate, 119 as ticketCount
	--select 0 as tno, '566' as airlineprefixcode, '5081' as tourcode, '2014-06-06'::date as issuedate, 9 as ticketCount union
	--select 0 as tno, '042'::citext2 as airlineprefixcode, '5761'::citext2 as tourcode, '2014-06-30'::date as issuedate, 116 as ticketCount		
	--select 0 as tno, '461'::citext2 as airlineprefixcode, '7233д'::citext2 as tourcode, '2014-06-10'::date as issuedate, 50 as ticketCount	
) q
	inner join generate_series(1, 200) rowno on rowno <= q.ticketCount
	inner join lt_airline al on q.airlineprefixcode = al.prefixcode
 order by tno, rowno
-- limit 1
;

--select * from charters;

insert into lt_product (
    id,
    version,
    createdby,
    createdon,
    type,
    issuedate,
    name,
    isprocessed,
    isvoid,
    requiresprocessing,
    isreservation,
    passengername,
    paymenttype,
    seller,
    owner,
    customer,
    tourcode,
    fare_amount,
    fare_currency,
    equalfare_amount,
    equalfare_currency,
    feestotal_amount,
    feestotal_currency,
    commission_amount,
    commission_currency,
    servicefee_amount,
    servicefee_currency,
    total_amount,
    total_currency,
    grandtotal_amount,
    grandtotal_currency
)
select 
    d.id,
    1 as version,
    'cc68a242538811dfbffbc430a29b25e7' as createdby,
    '2014-06-25' as createdon,
    0 as type,
    d.issuedate,
    d.number_ as name,
    false as isprocessed,
    false as isvoid,
    true as requiresprocessing,
    false as isreservation,
    null as passengername,
    6 as paymenttype,
    d.seller,
    d.owner,
    null as customer,
    tourcode,
    0 as fare_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as fare_currency,
    0 as equalfare_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as equalfare_currency,
    0 as feestotal_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as feestotal_currency,
    0 as commission_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as commission_currency,
    0 as servicefee_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as servicefee_currency,
    0 as total_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as total_currency,
    0 as grandtotal_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as grandtotal_currency
  from charters d
 order by d.number_;



insert into lt_avia_document (
    id,
    airlineiatacode,
    airlineprefixcode,
    airlinename,
    airline,
    number_,
    
    gdspassportstatus,
    originator,
    origin,
    itinerary
)
select 
    d.id,
    al.iatacode as airlineiatacode,
    al.prefixcode as airlineprefixcode,
    al.name as airlinename,
    d.airline,
    d.number_,
    0 as gdspassportstatus,
    0 as originator,
    5 as origin,
    null as itinerary
  from charters d
	left join lt_airline al on d.airline = al.id
 order by d.number_;

insert into lt_avia_ticket (
    id,
    domestic,
    interline,
    departure,
    faretotal_amount,
    faretotal_currency
)
select
    d.id,
    false as domestic,
    false as interline,
    d.issuedate,
    0 as faretotal_amount,
    'af496e4b4de647afa7b61eacfd1a6f4f' as faretotal_currency
  from charters d;




--rollback;
commit;