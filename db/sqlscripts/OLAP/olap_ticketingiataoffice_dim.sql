drop view if exists olap_ticketingiataoffice_dim;


create view olap_ticketingiataoffice_dim
as
select distinct 
    btrim(nullif(nullif(ticketingiataoffice, ''), ''''''))::varchar(4000) as ticketingiataoffice
  from lt_product
 order by 1;
