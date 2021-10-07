drop view if exists olap_intermediary_dim;


create view olap_intermediary_dim
as
select pt.id, pt.name::varchar(4000)
  from lt_party pt
 where pt.id in (select distinct intermediary from lt_product)
 order by 2;

-- select * from olap_intermediary_dim