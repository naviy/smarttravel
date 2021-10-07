drop view if exists olap_owner_dim;
drop view if exists olap_booker_dim;
drop view if exists olap_ticketer_dim;


create view olap_owner_dim
as
select p.id, p.name::varchar(4000)
  from (select distinct owner as id from lt_product) q
    inner join lt_party p on p.id = q.id
  order by 2;
 
create view olap_booker_dim
as
select p.id, p.name::varchar(4000)
  from (select distinct booker as id from lt_product) q
    inner join lt_party p on p.id = q.id
  order by 2;
  
create view olap_ticketer_dim
as
select p.id, p.name::varchar(4000)
  from (select distinct ticketer as id from lt_product) q
    inner join lt_party p on p.id = q.id
  order by 2;