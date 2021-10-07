SET search_path = bsv;


-- create or replace view "lt_Country" as
-- select
-- 	id::uuid as "Id",
-- 	createdby::varchar(64) as "CreatedBy",
-- 	createdon as "CreatedOn",
-- 	modifiedby::varchar(64) as "ModifiedBy",
-- 	modifiedon as "ModifiedOn",
-- 	name::varchar(255) as "Name",
-- 	twocharcode::varchar(2) as "TwoCharCode",
-- 	threecharcode::varchar(3) as "ThreeCharCode"
--   from lt_country  
-- ;

create or replace view "lt_Airport" as
select
 	id::uuid as "Id",
	coalesce(nullif(createdby, ''), 'SYSTEM')::varchar(64) as "CreatedBy",
	createdon as "CreatedOn",
	modifiedby::varchar(64) as "ModifiedBy",
	modifiedon as "ModifiedOn",
	name::varchar(255) as "Name",
    code::varchar(3) as "Code",
    country::uuid as "CountryId",
    settlement::varchar(255) as "Settlement",
    localizedsettlement::varchar(255) as "LocalizedSettlement",
	longitude as "Longitude",
    latitude as "Latitude"
  from lt_airport
;