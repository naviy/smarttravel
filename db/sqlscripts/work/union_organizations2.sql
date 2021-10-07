alter table lt_organization add column airline varchar(32);
create index IX_lt_organization_airline on lt_organization (airline);


update lt_organization o set airline = null;

update lt_organization o set
	airline = al.id,
	isairline = true,
	airlineiatacode = al.iatacode,
	airlineprefixcode = al.prefixcode,
	airlinepassportrequirement = al.passportrequirement
  from lt_airline al
    inner join lt_party pt on trim(al.name)::citext2 = trim(pt.name)::citext2 or pt.id = al.id
 where o.id = pt.id;
 
update lt_organization o set
	airline = al.id,
	isairline = true,
	airlineiatacode = al.iatacode,
	airlineprefixcode = al.prefixcode,
	airlinepassportrequirement = al.passportrequirement
  from lt_airline al
 where o.id = al.organization and airline is null;


insert into lt_party (
	id, version, name, 
	createdby, createdon,
	iscustomer, issupplier
)
select 
	al.id, 1, al.name, 
	al.createdby, al.createdon,
	false, false
  from lt_airline al
    left join lt_organization o on al.id = o.airline
 where o.id is null;

insert into lt_organization (
	id, airline,
	isairline,
	airlineiatacode,
	airlineprefixcode,
	airlinepassportrequirement
)
select 
	al.id, al.id,
	true,
	al.iatacode,
	al.prefixcode,
	al.passportrequirement
  from lt_airline al
    left join lt_organization o on al.id = o.airline
 where o.id is null;


alter table lt_product drop constraint "FK_lt_product_airline_lt_airline_id";

update lt_product p set
	airline = o.id
  from lt_organization o
 where p.airline = o.airline;

alter table lt_product add constraint "FK_lt_product_airline_lt_organization_id"
	foreign key (airline) references lt_organization (id);


alter table lt_airline_commission_percents drop constraint "airlinecommissionpercents_airline_fkey";

update lt_airline_commission_percents p set
	airline = o.id
  from lt_organization o
 where p.airline = o.airline;

alter table lt_airline_commission_percents add constraint "FK_lt_airline_commission_percents_airline_lt_organization_id"
	foreign key (airline) references lt_organization (id);


alter table lt_airline_service_class drop constraint airlineserviceclass_airline_fkey;

update lt_airline_service_class p set
	airline = o.id
  from lt_organization o
 where p.airline = o.airline;

alter table lt_airline_service_class add constraint "FK_lt_airline_service_class_airline_lt_organization_id"
	foreign key (airline) references lt_organization (id);


alter table lt_flight_segment drop constraint "flightsegment_carrier_fkey";

update lt_flight_segment p set
	carrier = o.id
  from lt_organization o
 where p.carrier = o.airline;

alter table lt_flight_segment add constraint "FK_lt_flight_segment_carrier_lt_organization_id"
	foreign key (carrier) references lt_organization (id);

alter table lt_organization drop column airline;

drop table lt_airline cascade;