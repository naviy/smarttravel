/*
-- UFSA

GRANT ALL ON SCHEMA ufsa TO tovufsa;
GRANT ALL ON SCHEMA ufsa TO utb;

grant all on table lt_party to tovufsa;
grant all on table lt_party to utb;
*/


drop table if exists dblAirlines;

create temp table dblAirlines
as
select p1.id, p2.id as id2, p1.name 
  from ufsa.lt_party p1
	inner join lt_party p2 
		on p2.name = p1.name and p2.id <> p1.id
 where p1.isairline and p2.isairline
;

--select * from dblAirlines;

begin work;

insert into lt_party (
	id, version, name, createdby, createdon, modifiedby, modifiedon, 
	legalname, phone1, phone2, fax, email1, email2, webaddress, iscustomer, 
	issupplier, legaladdress, actualaddress, note, reportsto, class, 
	type, organization, birthday, milescardsstring, title, bonuscardnumber, 
	code, isprovider, isaccommodationprovider, isbusticketprovider, 
	iscarrentalprovider, isinsuranceprovider, ispasteboardprovider, 
	istourprovider, istransferprovider, isairline, airlineiatacode, 
	airlineprefixcode, airlinepassportrequirement, isinsurancecompany, 
	isroamingoperator, isgenericproductprovider, defaultbankaccount, 
	details, invoicesuffix, bonusamount
)
select 
	p2.id, version, concat(' ', p.name) as name, createdby, createdon, modifiedby, modifiedon, 
	legalname, phone1, phone2, fax, email1, email2, webaddress, iscustomer, 
	issupplier, legaladdress, actualaddress, note, reportsto, class, 
	type, organization, birthday, milescardsstring, title, bonuscardnumber, 
	code, isprovider, isaccommodationprovider, isbusticketprovider, 
	iscarrentalprovider, isinsuranceprovider, ispasteboardprovider, 
	istourprovider, istransferprovider, isairline, airlineiatacode, 
	airlineprefixcode, airlinepassportrequirement, isinsurancecompany, 
	isroamingoperator, isgenericproductprovider, defaultbankaccount, 
	details, invoicesuffix, bonusamount
  from lt_party p
	inner join dblAirlines p2 on p2.id2 = p.id;


update lt_airline_service_class a
   set airline = p.id
  from dblAirlines p
 where airline = p.id2;

update lt_flight_segment a
   set carrier = p.id
  from dblAirlines p
 where a.carrier = p.id2;

update lt_product a
   set producer = p.id
  from dblAirlines p
 where a.producer = p.id2;


delete from lt_party p
--select p.name from lt_party p
 where exists(select id from dblAirlines where id2 = p.id);

--rollback;
commit;