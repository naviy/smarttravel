drop view if exists olap_flighttype_dim;


create view olap_flighttype_dim
as 
select 'D' as id, 'Внутренний' as value union all 
select 'I' as id, 'Международный' as value union all 
select '' as id, '(не указан)' as value;