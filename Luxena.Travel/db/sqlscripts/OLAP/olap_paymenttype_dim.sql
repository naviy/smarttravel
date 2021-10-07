drop view if exists olap_paymenttype_dim;


create view olap_paymenttype_dim
as 
select 0 as paymenttype, 'Unknown' as name union all 
select 1 as paymenttype, 'Cash' as name union all 
select 2 as paymenttype, 'Invoice' as name union all 
select 3 as paymenttype, 'Check' as name union all 
select 4 as paymenttype, 'Credit Card' as name union all 
select 5 as paymenttype, 'Exchange' as name union all 
select 6 as paymenttype, 'Without Payment' as name;