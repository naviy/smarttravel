drop view if exists olap_airport_dim;

drop view if exists olap_direction_dim;

drop view if exists olap_settlement_dim;
drop view if exists olap_direction_from_dim;


create view olap_direction_dim
as
select t.id as ticket,
   (select
		case
			when s.toairportcode = 'KBP' then
			   (select s2.fromairport
				  from lt_flight_segment s2
				 where s2.ticket = s.ticket and s2.position <= s.position
				   and s2.position >
						  (select coalesce(max(s3.position), -1)
							 from lt_flight_segment s3
							where s3.ticket = s.ticket and s3.stopover and s3.position < s.position)
				 order by s2.position
				 limit 1)
			else s.toairport
		end as airport
	  from lt_flight_segment s
		left join lt_flight_segment next
		  on next.ticket = s.ticket and next.position = s.position + 1
	 where s.stopover and s.type = 0 and s.ticket = coalesce(t.refundedproduct, t.id)
	 order by
		case
		  when next.id is null then 0::double precision
		  else date_part('epoch', next.departuretime - s.arrivaltime)
		end desc
	 limit 1
	)::varchar(4000) as airport

  from lt_product t;


-- select * from olap_direction_dim where airport is not null;

create view olap_direction_from_dim
as
select
	p.id as ticket,
	a.id as airport
  from lt_product p
	left join lt_product rp on p.refundedproduct = rp.id
	left join lt_airport a
		on a.code = trim(left(coalesce(rp.itinerary, p.itinerary), 3))
;
-- where t.type = 0;
