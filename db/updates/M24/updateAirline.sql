--script by 10.01.2012. refactoring airline inheritence from entity instead of organization

--------------------delete column if they are existing--------------------------------
alter table lt_airline drop if exists version;
alter table lt_airline drop if exists createdby;
alter table lt_airline drop if exists createdon;
alter table lt_airline drop if exists modifiedby;
alter table lt_airline drop if exists modifiedon;
alter table lt_airline drop if exists name;
alter table lt_airline drop if exists organization;

alter table lt_airline drop  CONSTRAINT if exists lt_airline_name_key; 
alter table lt_airline drop  CONSTRAINT if exists airline_fkey; 


----------------------add if existscolumn------------------------------------------------------
alter table lt_airline add  version integer; 
alter table lt_airline add  createdby public.citext2;
alter table lt_airline add  createdon timestamp without time zone;
alter table lt_airline add  modifiedby public.citext2;
alter table lt_airline add  modifiedon timestamp without time zone;
alter table lt_airline add  name public.citext2;
alter table lt_airline add  organization character varying(32);


---update lt_airline------------------------------------------------------------------------------------
UPDATE lt_airline air
   SET createdby=p.createdby, name=p.name, version=p.version, createdon=p.createdon, modifiedby=p.modifiedby, modifiedon=p.modifiedon, organization = o.id 
   from lt_party as p
   left join lt_organization as o on p.id = o.id
   where p.id=air.id;

   


--add constraint---------------------------------------------------------------------------------------
alter table lt_airline add  CONSTRAINT lt_airline_name_key UNIQUE (name); 
alter table lt_airline add constraint lt_airline_lt_ogranization foreign key(organization) references lt_organization(id) match simple on update no action on delete no action; 

----------------------alter table ------------------------------------------------------
alter table lt_airline alter  column version set NOT NULL;
alter table lt_airline alter  column createdby set NOT NULL;
alter table lt_airline alter  column createdon set NOT NULL;
alter table lt_airline alter  column name set NOT NULL;


 