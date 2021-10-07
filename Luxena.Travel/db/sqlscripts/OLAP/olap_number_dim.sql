drop view if exists olap_number_dim;


create view olap_number_dim
as
select
	number::varchar(255) as number
  from (
	select distinct trim(name)::citext2 as number
	  from lt_product
  ) a;