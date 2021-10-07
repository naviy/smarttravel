drop view if exists olap_provider_type_dim;
drop view if exists olap_airline_dim;


create view olap_airline_dim
as 
select 
    pt.id, pt.name::varchar(4000), 

    trim(trailing ', ' from concat(
        case when isairline then 'Авиакомпания, ' else '' end, 
        case when ispasteboardprovider then 'Провайдер ж/д билетов, ' else '' end,
        case when isinsurancecompany then 'Страховая компания, ' else '' end,
        case when isroamingoperator then 'Роуминг-оператор, ' else '' end,
        case when isaccommodationprovider then 'Провайдер проживаний, ' else '' end,
        case when isbusticketprovider then 'Провайдер автобусных билетов, ' else '' end,
        case when iscarrentalprovider then 'Провайдер аренды авто, ' else '' end,
        case when istourprovider then 'Провайдер туров, ' else '' end,
        case when istransferprovider then 'Провайдер трансферов, ' else '' end,
        case when isgenericproductprovider then 'Провайдер дополнительных услуг, ' else '' end
    )) as type,
    
    isairline, isaccommodationprovider, isbusticketprovider, iscarrentalprovider, ispasteboardprovider, 
    istourprovider, istransferprovider, isgenericproductprovider, isinsurancecompany, isroamingoperator
  from lt_party pt
	inner join (
        select distinct producer as id from lt_product union 
        select distinct provider as id from lt_product
    ) q on pt.id = q.id
;


create view olap_provider_type_dim
as
select distinct type from olap_airline_dim
;
/*
select * from olap_airline_dim;
*/