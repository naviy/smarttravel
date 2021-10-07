drop view if exists olap_order_dim;


create view olap_order_dim
as
select id, number_::varchar(255)
  from lt_order 
 order by 2;
