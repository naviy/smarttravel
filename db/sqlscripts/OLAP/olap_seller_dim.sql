drop view if exists olap_seller_dim;


create view olap_seller_dim
as
select p.id, p.name::varchar(4000)
  from (select distinct seller as id from lt_product) q
    inner join lt_party p on q.id = p.id

-- select p.id, p.name || ' (УФСА)' as name
--   from (select distinct seller as id from ufsa.lt_product) q
--     inner join ufsa.lt_party p on q.id = p.id
-- union all
-- select p.id, p.name || ' (УТБ)'
--   from (select distinct seller as id from utb.lt_product) q
--     inner join utb.lt_party p on q.id = p.id
--  order by 2;