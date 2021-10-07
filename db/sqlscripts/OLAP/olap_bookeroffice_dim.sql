drop view if exists olap_bookeroffice_dim;


create view olap_bookeroffice_dim
as 
select
	bookeroffice::varchar(255) as bookeroffice
  from (
select distinct 
    btrim(bookeroffice)::citext2 as bookeroffice
  from lt_product
 order by 1
) a;