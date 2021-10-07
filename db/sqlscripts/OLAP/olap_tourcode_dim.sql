drop view if exists olap_tourcode_dim;


create view olap_tourcode_dim
as
select
	tourcode::varchar(255) as tourcode
  from (
	select distinct 
		coalesce(btrim(tourcode), '---')::citext2 as tourcode
	  from lt_product p
	 order by 1
  ) a;