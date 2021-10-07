drop view if exists olap_customer_dim;


create view olap_customer_dim
as
select id, name::varchar(4000) 
  from lt_party 
 where id in (
    select distinct customer from lt_product union 
    select distinct customer from lt_order union
    select distinct payer from lt_payment) 
 order by 2;
