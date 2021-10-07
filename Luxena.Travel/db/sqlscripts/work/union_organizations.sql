-- begin work;

-- alter table lt_organization add column ppid varchar(32);
-- create index IX_lt_organization_ppid on lt_organization (ppid);
-- 
-- update lt_organization o set
-- 	ppid = pp.id,
-- 	isprovider = useforaccommodation or useforbusticket or useforcarrental or useforinsurance or useforpasteboard or usefortour or usefortransfer,
-- 	isaccommodationprovider = useforaccommodation,
-- 	isbusticketprovider = useforbusticket,
-- 	iscarrentalprovider = useforcarrental,
-- 	isinsuranceprovider = useforinsurance,
-- 	ispasteboardprovider = useforpasteboard,
-- 	istourprovider = usefortour,
-- 	istransferprovider = usefortransfer	
--   from lt_product_provider pp
--     inner join lt_party p on trim(pp.name)::citext2 = trim(p.name)::citext2
--  where o.id = p.id;
-- 
-- insert into lt_party (
-- 	id, version, name, 
-- 	createdby, createdon,
-- 	iscustomer, issupplier
-- )
-- select 
-- 	pp.id, 1, pp.name, 
-- 	pp.createdby, pp.createdon,
-- 	false, false
--   from lt_product_provider pp
--     left join lt_organization o on pp.id = o.ppid
--  where o.id is null;
-- 
-- insert into lt_organization (
-- 	id, ppid,
-- 	isprovider,
-- 	isaccommodationprovider,
-- 	isbusticketprovider,
-- 	iscarrentalprovider,
-- 	isinsuranceprovider,
-- 	ispasteboardprovider,
-- 	istourprovider,
-- 	istransferprovider
-- )
-- select 
-- 	pp.id, pp.id,
-- 	useforaccommodation or useforbusticket or useforcarrental or useforinsurance or useforpasteboard or usefortour or usefortransfer,
-- 	useforaccommodation,
-- 	useforbusticket,
-- 	useforcarrental,
-- 	useforinsurance,
-- 	useforpasteboard,
-- 	usefortour,
-- 	usefortransfer	
--   from lt_product_provider pp
--     left join lt_organization o on pp.id = o.ppid
--  where o.id is null;
-- 
-- 
-- alter table lt_product drop constraint fk_lt_product_provider_lt_product_provider_id;
-- 
-- 
-- update lt_product p set
-- 	provider = o.id
--   from lt_organization
--  where p.provider = o.ppid;
-- 
--  alter table lt_product add constraint fk_lt_product_provider_lt_organization_id
--  	foreign key (provider) references lt_organization (id);
-- 
-- alter table lt_organization drop column ppid;
    
--select count(*) from lt_product_provider;
--select count(*) from lt_organization where ppid is not null;
--select count(*) from lt_product_provider where useforaccommodation or useforbusticket or useforcarrental or useforinsurance or useforpasteboard or usefortour or usefortransfer;
--select count(*) from lt_organization where isprovider;
--select count(*) from lt_product where provider is not null;

-- rollback;



 begin work;

-- alter table lt_organization add column airline varchar(32);
-- create index IX_lt_organization_airline on lt_organization (airline);
-- 
-- 
-- update lt_organization o set airline = null;
-- 
-- update lt_organization o set
-- 	airline = al.id,
-- 	isairline = true,
-- 	airlineiatacode = al.iatacode,
-- 	airlineprefixcode = al.prefixcode,
-- 	airlinepassportrequirement = al.passportrequirement
--   from lt_airline al
--     inner join lt_party pt on trim(al.name)::citext2 = trim(pt.name)::citext2 or pt.id = al.id
--  where o.id = pt.id;
--  
-- update lt_organization o set
-- 	airline = al.id,
-- 	isairline = true,
-- 	airlineiatacode = al.iatacode,
-- 	airlineprefixcode = al.prefixcode,
-- 	airlinepassportrequirement = al.passportrequirement
--   from lt_airline al
--  where o.id = al.organization and airline is null;
-- 
-- -- 
-- -- -- select --al.id, al.organization, al.name as airline, pt.name as organization, pt0.name as organization0, pt1.name as organization1
-- -- -- 	al.id, al.organization, al.name as airline, pt0.name as organization0, pt1.name as organization1
-- -- --   from lt_airline al
-- -- --     --inner join lt_party pt on trim(al.name)::citext2 = trim(pt.name)::citext2
-- -- --     --inner join lt_organization o on o.id = pt.id
-- -- --     left join lt_organization o0 on al.organization = o0.id
-- -- --     left join lt_party pt0 on o0.id = pt0.id
-- -- --     left join lt_organization o1 on o1.airline = al.id
-- -- --     left join lt_party pt1 on o1.id = pt1.id
-- -- --  where al.organization <> o1.id;
-- -- -- -- where o1.id is null or al.organization <> o1.id;
-- -- -- -- where al.organization is null or al.organization <> o.id;
-- 
-- 
-- -- -- select *
-- -- --   from
-- -- --    (select pt.id, pt.name, o.airline
-- -- -- 	  from lt_party pt 
-- -- -- 		inner join lt_organization o on o.id = pt.id
-- -- -- 	 where pt.name = 'NOVAsim') q1,
-- -- --    (select al.id, al.name, al.organization, pt0.name
-- -- -- 	  from lt_airline al
-- -- -- 		left join lt_party pt0 on pt0.id = al.organization
-- -- -- 	 where al.name = 'NOVAsim') q2
-- -- -- ;
-- 
-- -- -- select *
-- -- --   from
-- -- --    (select pt.id, pt.name, o.airline
-- -- -- 	  from lt_party pt 
-- -- -- 		left join lt_organization o on o.id = pt.id
-- -- -- 	 where pt.id = 'c92fb54ece21476bb7380f1b5de5317a') q1,
-- -- --    (select al.id, al.name, al.organization, pt0.name
-- -- -- 	  from lt_airline al
-- -- -- 		left join lt_party pt0 on pt0.id = al.organization
-- -- -- 	 where al.id = 'c92fb54ece21476bb7380f1b5de5317a') q2
-- -- -- ;
-- -- -- 
-- -- -- select pt.name, al.name
-- -- --   from lt_party pt, lt_airline al
-- -- --  where pt.id = al.id and trim(al.name)::citext2 <> trim(pt.name)::citext2;
-- 
-- 
-- insert into lt_party (
-- 	id, version, name, 
-- 	createdby, createdon,
-- 	iscustomer, issupplier
-- )
-- select 
-- 	al.id, 1, al.name, 
-- 	al.createdby, al.createdon,
-- 	false, false
--   from lt_airline al
--     left join lt_organization o on al.id = o.airline
--  where o.id is null;
-- 
-- insert into lt_organization (
-- 	id, airline,
-- 	isairline,
-- 	airlineiatacode,
-- 	airlineprefixcode,
-- 	airlinepassportrequirement
-- )
-- select 
-- 	al.id, al.id,
-- 	true,
-- 	al.iatacode,
-- 	al.prefixcode,
-- 	al.passportrequirement
--   from lt_airline al
--     left join lt_organization o on al.id = o.airline
--  where o.id is null;


-- -- -- select 
-- -- -- 	al.name as airline, pt1.name as organization1, al.id, pt1.id
-- -- --   from lt_airline al
-- -- --     left join lt_organization o1 on o1.airline = al.id
-- -- --     left join lt_party pt1 on o1.id = pt1.id
-- -- --  order by al.name
-- -- -- ;
-- 
-- -- -- select count(*) from lt_organization where airline is not null;


-- alter table lt_product drop constraint "FK_lt_product_airline_lt_airline_id";
-- 
-- update lt_product p set
-- 	airline = o.id
--   from lt_organization o
--  where p.airline = o.airline;
-- 
-- alter table lt_product add constraint "FK_lt_product_airline_lt_organization_id"
--	foreign key (airline) references lt_organization (id);


-- alter table lt_airline_commission_percents drop constraint "airlinecommissionpercents_airline_fkey";
-- 
-- update lt_airline_commission_percents p set
-- 	airline = o.id
--   from lt_organization o
--  where p.airline = o.airline;
-- 
-- alter table lt_airline_commission_percents add constraint "FK_lt_airline_commission_percents_airline_lt_organization_id"
-- 	foreign key (airline) references lt_organization (id);


-- alter table lt_airline_service_class drop constraint airlineserviceclass_airline_fkey;
-- 
-- update lt_airline_service_class p set
-- 	airline = o.id
--   from lt_organization o
--  where p.airline = o.airline;
-- 
-- alter table lt_airline_service_class add constraint "FK_lt_airline_service_class_airline_lt_organization_id"
-- 	foreign key (airline) references lt_organization (id);


alter table lt_flight_segment drop constraint "flightsegment_carrier_fkey";

update lt_flight_segment p set
	carrier = o.id
  from lt_organization o
 where p.carrier = o.airline;

alter table lt_flight_segment add constraint "FK_lt_flight_segment_carrier_lt_organization_id"
	foreign key (carrier) references lt_organization (id);



alter table lt_organization drop column airline;



drop table lt_airline cascade;
    
-- -- select count(*) from lt_airline;
-- -- select count(*) from lt_organization where airline is not null;
-- -- select count(*) from lt_organization where isairline;
-- -- select count(*) from lt_product where provider is not null;

rollback;
 