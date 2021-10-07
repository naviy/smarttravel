
drop table if exists openflights_org_data;

create table openflights_org_data
(
	airport_id integer,         -- Unique OpenFlights identifier for this airport.
	name text,                  -- Name of airport. May or may not contain the City name.
	city text,                  -- Main city served by airport. May be spelled differently from Name.
	country text,               -- Country or territory where airport is located.
	iata_faa char(3),           -- 3-letter FAA code, for airports located in Country "United States of America".
								-- 3-letter IATA code, for all other airports.
								-- Blank if not assigned.
	icao char(4),               -- 4-letter ICAO code.
								-- Blank if not assigned.
	latitude double precision,  -- Decimal degrees, usually to six significant digits. Negative is South, positive is North.
	longitude double precision, -- Decimal degrees, usually to six significant digits. Negative is West, positive is East.
	altitude integer,           -- In feet.
	timezone double precision,  -- Hours offset from UTC. Fractional hours are expressed as decimals, eg. India is 5.5.
	dst char(1)                 -- Daylight savings time. One of E (Europe), A (US/Canada), S (South America), O (Australia), Z (New Zealand), N (None) or U (Unknown)
);

\copy openflights_org_data from 'sql/openflights.org_data.csv' delimiter ',' csv;

delete from
  openflights_org_data
where
  not (iata_faa ~ '[A-Z]{3}' and not (latitude = 0 and longitude = 0))
  or airport_id in (3769, 8797, 7697);

update lt_airport set version = 1 where version = 0;

alter table lt_airport add longitude double precision;
alter table lt_airport add latitude double precision;

delete from
  lt_airport
where not code in (
  select
    code::text
  from
    lt_airport
  where
    id in (
      select fromairport from lt_flight_segment where fromairport is not null
      union
      select toairport from lt_flight_segment where toairport is not null
    )
  union
  select
    iata_faa
  from
    openflights_org_data
);

update lt_airport a set 
    longitude = coalesce(a.longitude, b.longitude), 
    latitude = coalesce(a.latitude, b.latitude)
  from openflights_org_data b
 where a.code = b.iata_faa;

insert into lt_airport (
    id, version, createdby, createdon,
    code, name, country, settlement, 
    longitude, latitude
)
select 
    replace(cast(public.uuid_generate_v4() as varchar), '-', ''), 1, 'SYSTEM', now(),
    a.iata_faa, a.name, c.id, a.city,
    a.longitude, a.latitude
  from openflights_org_data a
    left join lt_country c on c.name = a.country
 where
	not a.iata_faa in (select code from lt_airport);

drop table openflights_org_data;

update lt_airport
set
  country = (select id from lt_country where name = 'France'),
  name = 'Gare de Lorraine TGV',
  latitude = 48.9475,
  longitude = 6.169722 
where
  code = 'XZI';

update lt_airport
set
  latitude = 48.533333,
  longitude = 7.63333
where
  code = 'XER';

update lt_airport
set
  country = (select id from lt_country where name = 'Russian Federation'),
  settlement = 'Lensk',
  name = 'Lensk Airport',
  latitude = 60.72248,
  longitude = 114.83152 
where
  code = 'ULK';

update lt_airport
set
  latitude = 41.892942,
  longitude = 12.482518 
where
  code = 'ROM';

update lt_airport
set
  country = (select id from lt_country where name = 'Russian Federation'),
  settlement = 'Bodaybo',
  name = 'Bodaybo Airport',
  latitude = 57.86639,
  longitude = 114.2425
where
  code = 'ODO';

update lt_airport
set
  country = (select id from lt_country where name = 'Russian Federation'),
  settlement = 'Igrim',
  name = 'Igrim Airport',
  latitude = 63.200756,
  longitude = 64.433945
where
  code = 'IRM';

update lt_airport
set
  country = (select id from lt_country where name = 'Russian Federation'),
  settlement = 'Nazran',
  name = 'Nazran Airport',
  latitude = 43.31778,
  longitude = 45.001667
where
  code = 'IGT';

update lt_airport
set
  country = (select id from lt_country where name = 'Russian Federation'),
  settlement = 'Beryozovo',
  name = 'Beryozovo Airport',
  latitude = 63.924583,
  longitude = 65.04485
where
  code = 'EZV';

update lt_airport
set
  latitude = 42.331389,
  longitude = -83.045833
where
  code = 'DTT';


update lt_airport
set
  country = (select id from lt_country where name = 'Argentina'),
  settlement = 'Buenos Aires',
  latitude = -34.689400,
  longitude = -58.474998
where
  code = 'BUE';
