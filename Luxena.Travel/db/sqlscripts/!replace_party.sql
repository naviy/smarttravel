/*


select pt.name
  from (
	select distinct p1.seller 
	  from ufsa.lt_product p1
		inner join tovufsa.lt_product p2 on p2.seller = p1.seller
	) as p
	inner join tovufsa.lt_party pt on p.seller = pt.id
;


select name from ufsa.lt_party    where id = '181d74e334974551bd2136bfd01bc15f';
select name from tovufsa.lt_party where id = '181d74e334974551bd2136bfd01bc15f';
*/


select p1.id, p1.name 
  from tovufsa.lt_party p1
    inner join ufsa.lt_party p2 on p1.id = p2.id
 where exists(select id from tovufsa.lt_product where seller = p1.id)
;


select
    --concat('select ''', tablename, '.', colname, ''', count(*) from ', tablename, ' where ', colname, ' = ''', oldPartyId, ''' union')
    concat('update ', p.schema_, '.', tablename, ' set ', colname, ' = ''', newPartyId, ''' where ', colname, ' = ''', oldPartyId, ''';')
  from (
    select
        'tovufsa'::citext as schema_,
        'baa1aafd9ff348098aca14ceee229a06' as oldPartyId,
        '79818e9d87c349979062b1ef0de0dd74' as newPartyId
    ) p, (
    select
        ns.nspname as schema_,
        --ns.nspname, conname
        c.conname, rel.relname as tablename, --pg_get_constraintdef(c.oid),
        substring(pg_get_constraintdef(c.oid) from 'FOREIGN KEY \((.+?)\)') as colname
      from pg_constraint c
        inner join pg_class cls on cls.oid = c.confrelid
        inner join pg_catalog.pg_namespace ns on ns.oid = c.connamespace
        inner join pg_class rel on rel.oid = c.conrelid
     where cls.relname = 'lt_party'
       and c.contype = 'f'
     order by 1
    ) q
  where p.schema_ = q.schema_