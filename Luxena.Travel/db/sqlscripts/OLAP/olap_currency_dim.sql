drop view if exists olap_currency_dim cascade;


create view olap_currency_dim
as 
select c.id, c.code::varchar(255)
  from (
    select distinct coalesce(p.grandtotal_currency, p.total_currency, p.equalfare_currency) as id 
      from lt_product p
     where not isvoid
    ) q
    inner join lt_currency c on q.id = c.id
 order by 2;


/*
select * from olap_currency_dim
 */