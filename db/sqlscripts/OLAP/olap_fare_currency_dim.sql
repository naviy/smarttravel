drop view if exists olap_fare_currency_dim;


create view olap_fare_currency_dim
as 
select 
    c.id, c.code::varchar(255)
  from (select distinct amount_currency as id from lt_flight_segment) q
    inner join lt_currency c on q.id = c.id
 order by c.code;