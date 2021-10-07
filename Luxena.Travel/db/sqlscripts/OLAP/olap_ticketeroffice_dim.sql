drop view if exists olap_ticketeroffice_dim;


create view olap_ticketeroffice_dim
as
select
	ticketeroffice::varchar(255) as ticketeroffice
  from (
	select distinct 
		btrim(ticketeroffice)::citext2 as ticketeroffice
	  from lt_product
	 order by 1
  ) a;