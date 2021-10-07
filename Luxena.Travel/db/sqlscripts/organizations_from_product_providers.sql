alter table lt_organization add column ppid varchar(32);
create index IX_lt_organization_ppid on lt_organization (ppid);

update lt_organization o set
	ppid = pp.id,
	isprovider = useforaccommodation or useforbusticket or useforcarrental or useforinsurance or useforpasteboard or usefortour or usefortransfer,
	isaccommodationprovider = useforaccommodation,
	isbusticketprovider = useforbusticket,
	iscarrentalprovider = useforcarrental,
	isinsuranceprovider = useforinsurance,
	ispasteboardprovider = useforpasteboard,
	istourprovider = usefortour,
	istransferprovider = usefortransfer
  from lt_product_provider pp
    inner join lt_party p on trim(pp.name)::citext2 = trim(p.name)::citext2
 where o.id = p.id;

insert into lt_party (
	id, version, name, 
	createdby, createdon,
	iscustomer, issupplier
)
select 
	pp.id, 1, pp.name, 
	pp.createdby, pp.createdon,
	false, false
  from lt_product_provider pp
    left join lt_organization o on pp.id = o.ppid
 where o.id is null;

insert into lt_organization (
	id, ppid,
	isprovider,
	isaccommodationprovider,
	isbusticketprovider,
	iscarrentalprovider,
	isinsuranceprovider,
	ispasteboardprovider,
	istourprovider,
	istransferprovider
)
select 
	pp.id, pp.id,
	useforaccommodation or useforbusticket or useforcarrental or useforinsurance or useforpasteboard or usefortour or usefortransfer,
	useforaccommodation,
	useforbusticket,
	useforcarrental,
	useforinsurance,
	useforpasteboard,
	usefortour,
	usefortransfer
  from lt_product_provider pp
    left join lt_organization o on pp.id = o.ppid
 where o.id is null;


alter table lt_product drop constraint fk_lt_product_provider_lt_product_provider_id;


update lt_product p set
	provider = o.id
  from lt_organization o
 where p.provider = o.ppid;

 alter table lt_product add constraint fk_lt_product_provider_lt_organization_id
 	foreign key (provider) references lt_organization (id);

drop table lt_product_provider;

alter table lt_organization drop column ppid;