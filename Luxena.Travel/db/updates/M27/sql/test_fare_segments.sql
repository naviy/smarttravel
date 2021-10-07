select
    --f.name, d.id, --d.Total_Amount,
    --f.content,
    case f.filetype 
        when 0 then substring(f.content from 'Q-(.*?END)') 
        else  replace(substring(f.content from 'A09010(.*?)\rA\d\d'), chr(13), '')
    end as fare_content___________________________________________________,
    concat(c0.Code, ' ', d.total_amount::numeric(19,2)) as Total___________,
    concat(c1.Code, ' ', t.faretotal_amount::numeric(19,2)) as FareTotal_____,
    concat(c3.Code, ' ', d.fare_amount::numeric(19,2)) as Fare_________,
    --s.position, 
    concat(s.fromairportcode, ' ', s.carrieriatacode, ' ', s.toairportcode) as segment________, 
    concat(
        c2.Code, ' ', s.Amount_Amount::numeric(19,2), ' = ', 
        s.surcharges::numeric(19,2), ' + ', s.fare::numeric(19,2), ' + ', s.stopoverortransfercharge::numeric(19,2)
    ) as Amount________________________,
    s.distance    
  from lt_gds_file f
    inner join lt_avia_document d on d.originaldocument = f.id
    inner join lt_avia_ticket t on d.id = t.id
    inner join lt_flight_segment s on s.ticket = d.id 
    left join lt_currency c0 on d.total_Currency = c0.id
    left join lt_currency c1 on t.faretotal_Currency = c1.id
    left join lt_currency c2 on s.amount_Currency = c2.id
    left join lt_currency c3 on d.fare_Currency = c3.id
 where s.departuretime between '2013-1-1' and '2013-3-30'
   --and f.filetype in (0,1) and d.number_ is not null 
   and t.reissuefor is null and s.type = 0
   and s.carrieriatacode = 'LO' --and c2.Code <> 'NUC'
   --and s.distance = 0
   --and t.faretotal_currency is null and f.content !~ ' M/IT '
--    and exists(
--            select null 
--              from lt_flight_segment 
--             where ticket = d.id and type = 0
--               and (fromairportcode !~ '^[A-Z]{3}$' or toairportcode !~ '^[A-Z]{3}$' or carrieriatacode !~ '^[A-Z\d]{2}$'
--                    or coalesce(amount_amount, 0) = 0 and coalesce(d.total_amount, 0) > 0  and coalesce(distance, 0) > 0
--                    or t.faretotal_amount > 0 and t.faretotal_Currency is not null and s.amount_currency <> t.faretotal_Currency
--                   )
--        ) 
 order by f.name, d.id, s.position
 --limit 100
;



/*

select
sum(s.amount)
    --concat(sum(s.amount)::numeric(19,2), ' ', c2.Code)
  from olap_fare_segment_dim s
    left join lt_currency c2 on s.currency = c2.id
 where s.carrieriatacode = 'OK'
   and s.departure between '2013-1-1' and '2013-3-30'   
-- group by c2.Code


select
  d.number_,
  d.issuedate,
  case
    when g.filetype = 0 then substring(g.content from 'Q-([^\n]*)')
    when g.filetype = 1 then substring(g.content from 'A09.*?END[^\r]*')
  end fare_source,
  g.content,
  fa.cnt fare_segments,
  f.cnt flight_segments,
  d.originaldocument
from
  ufsa.lt_avia_ticket t
  inner join ufsa.lt_avia_document d on d.id = t.id
  inner join ufsa.lt_gds_file g on g.id = d.originaldocument
  inner join (select ticket, count(*) cnt from ufsa.lt_fare_segment group by ticket) fa on fa.ticket = t.id
  inner join (select ticket, count(*) cnt from ufsa.lt_flight_segment where type = 0 group by ticket) fl on f.ticket = t.id
where
  t.reissuefor is null
  and fa.cnt <> f.cnt
  and g.timestamp >= '2013-01-01'
  and case
    when g.filetype = 0 then substring(g.content from 'Q-[^\n]*END')
    when g.filetype = 1 then substring(g.content from 'A09.*?END')
  end ~ '\(.{3,}\)';

  */
   