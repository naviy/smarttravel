drop view if exists olap_departure_dim;
drop view if exists olap_document;
drop view if exists olap_seller_dim;


create view olap_document
as
select 
    *,
    cast(equalfare * addcommissionpc / 100.0000 as numeric(18, 4)) as addcommission,

    cast(eur_rate * equalfare as numeric(18, 2)) as equalfare_eur,
    cast(rub_rate * equalfare as numeric(18, 2)) as equalfare_rub,
    cast(uah_rate * equalfare as numeric(18, 2)) as equalfare_uah,
    cast(usd_rate * equalfare as numeric(18, 2)) as equalfare_usd,

    cast(eur_rate * total as numeric(18, 2)) as total_eur,
    cast(rub_rate * total as numeric(18, 2)) as total_rub,
    cast(uah_rate * total as numeric(18, 2)) as total_uah,
    cast(usd_rate * total as numeric(18, 2)) as total_usd,

    cast(eur_rate * fee_yr as numeric(18, 2)) as fee_yr_eur,
    cast(rub_rate * fee_yr as numeric(18, 2)) as fee_yr_rub,
    cast(uah_rate * fee_yr as numeric(18, 2)) as fee_yr_uah,
    cast(usd_rate * fee_yr as numeric(18, 2)) as fee_yr_usd,

    cast(eur_rate * fee_yq as numeric(18, 2)) as fee_yq_eur,
    cast(rub_rate * fee_yq as numeric(18, 2)) as fee_yq_rub,
    cast(uah_rate * fee_yq as numeric(18, 2)) as fee_yq_uah,
    cast(usd_rate * fee_yq as numeric(18, 2)) as fee_yq_usd,

    cast(eur_rate * fee_rest as numeric(18, 2)) as fee_rest_eur, -- Прочие таксы
    cast(rub_rate * fee_rest as numeric(18, 2)) as fee_rest_rub,
    cast(uah_rate * fee_rest as numeric(18, 2)) as fee_rest_uah,
    cast(usd_rate * fee_rest as numeric(18, 2)) as fee_rest_usd,

    cast(eur_rate * commission as numeric(18, 2)) as commission_eur,
    cast(rub_rate * commission as numeric(18, 2)) as commission_rub,
    cast(uah_rate * commission as numeric(18, 2)) as commission_uah,
    cast(usd_rate * commission as numeric(18, 2)) as commission_usd,

    cast(eur_rate * commissionwovat as numeric(18, 2)) as commissionwovat_eur,
    cast(rub_rate * commissionwovat as numeric(18, 2)) as commissionwovat_rub,
    cast(uah_rate * commissionwovat as numeric(18, 2)) as commissionwovat_uah,
    cast(usd_rate * commissionwovat as numeric(18, 2)) as commissionwovat_usd,

    cast(eur_rate * servicefee as numeric(18, 2)) as servicefee_eur,
    cast(rub_rate * servicefee as numeric(18, 2)) as servicefee_rub,
    cast(uah_rate * servicefee as numeric(18, 2)) as servicefee_uah,
    cast(usd_rate * servicefee as numeric(18, 2)) as servicefee_usd,

    cast(eur_rate * handling as numeric(18, 2)) as handling_eur,
    cast(rub_rate * handling as numeric(18, 2)) as handling_rub,
    cast(uah_rate * handling as numeric(18, 2)) as handling_uah,
    cast(usd_rate * handling as numeric(18, 2)) as handling_usd,

    cast(eur_rate * handlingn as numeric(18, 2)) as handlingn_eur,
    cast(rub_rate * handlingn as numeric(18, 2)) as handlingn_rub,
    cast(uah_rate * handlingn as numeric(18, 2)) as handlingn_uah,
    cast(usd_rate * handlingn as numeric(18, 2)) as handlingn_usd,

    cast(eur_rate * discount as numeric(18, 2)) as discount_eur,
    cast(rub_rate * discount as numeric(18, 2)) as discount_rub,
    cast(uah_rate * discount as numeric(18, 2)) as discount_uah,
    cast(usd_rate * discount as numeric(18, 2)) as discount_usd,

    cast(eur_rate * grandtotal as numeric(18, 2)) as grandtotal_eur,
    cast(rub_rate * grandtotal as numeric(18, 2)) as grandtotal_rub,
    cast(uah_rate * grandtotal as numeric(18, 2)) as grandtotal_uah,
    cast(usd_rate * grandtotal as numeric(18, 2)) as grandtotal_usd,

    cast(eur_rate * totaltotransfer as numeric(18, 2)) as totaltotransfer_eur,
    cast(rub_rate * totaltotransfer as numeric(18, 2)) as totaltotransfer_rub,
    cast(uah_rate * totaltotransfer as numeric(18, 2)) as totaltotransfer_uah,
    cast(usd_rate * totaltotransfer as numeric(18, 2)) as totaltotransfer_usd,

    cast(eur_rate * profit as numeric(18, 2)) as profit_eur,
    cast(rub_rate * profit as numeric(18, 2)) as profit_rub,
    cast(uah_rate * profit as numeric(18, 2)) as profit_uah,
    cast(usd_rate * profit as numeric(18, 2)) as profit_usd,

    cast(eur_rate * profitwovat as numeric(18, 2)) as profitwovat_eur,
    cast(rub_rate * profitwovat as numeric(18, 2)) as profitwovat_rub,
    cast(uah_rate * profitwovat as numeric(18, 2)) as profitwovat_uah,
    cast(usd_rate * profitwovat as numeric(18, 2)) as profitwovat_usd

  from (
    select 
        *,
        (profit / vatf)::numeric(18, 2) as profitwovat,
        (commission / vatf)::numeric(18, 2) as commissionwovat
      from (
        select 
            p.id, 
            p.issuedate, 
            p.type, 
            trim(p.name)::varchar(255) as number, 
            coalesce(rp.itinerary, p.itinerary)::varchar(255) as itinerary, 
            p.paymenttype, 
            coalesce(p.producer, p.provider) as airline,
        	replace(replace(replace(replace(upper(trim(replace(replace(p.passengername, chr(9), ''), chr(160), ' '))), 'Ё', 'Е'), 'Š', 'S'), 'Ć', 'C'), 'Ž', 'Z')::varchar(4000) as passenger,
            p.seller, 
            p.customer,
            p.intermediary,
            p.owner,
            p.booker,
            p.ticketer,
            case
                when p.type in (0, 1) then p.id
                else null
            end as direction, 
            case
                when p.id is null then ''
                when p.domestic then 'D'
                else 'I'
            end::varchar(255) as flighttype, 
            btrim(p.ticketeroffice)::varchar(255) as ticketeroffice, 
            coalesce(btrim(p.tourcode), '---')::varchar(255) as tourcode,
            1 + p.vatpc / 100 as vatf,            
            p.sign * coalesce(p.equalfare_amount, 0) as equalfare, 
            p.sign * coalesce(p.total_amount, 0) as total, 
			p.sign * coalesce(f.fee_yr, 0) as fee_yr, 
			p.sign * coalesce(f.fee_yq, 0) as fee_yq, 
			p.sign * coalesce(f.fee_rest, 0) as fee_rest, 
            p.sign * coalesce(p.commission_amount, 0) as commission, 
            coalesce(amc.commissionpc, 0) as addcommissionpc,
            p.sign * coalesce(p.servicefee_amount, 0) as servicefee, 
            p.sign * coalesce(p.handling_amount, 0) as handling, 
            p.sign * coalesce(p.handlingn_amount, 0) as handlingn, 
            p.sign * coalesce(p.discount_amount, 0) + coalesce(p.bonusdiscount_amount, 0) + coalesce(p.commissiondiscount_amount, 0) as discount, 
            p.sign * coalesce(p.grandtotal_amount, 0) as grandtotal, 
            p.sign * (coalesce(p.total_amount, 0) - coalesce(p.commission_amount, 0)) as totaltotransfer, 
            p.sign * (
                  coalesce(p.commission_amount, 0) 
                + coalesce(p.servicefee_amount, 0) 
                + coalesce(p.handling_amount, 0) 
                - coalesce(p.handlingn_amount, 0) 
                - coalesce(p.discount_amount, 0) 
                - coalesce(p.bonusdiscount_amount, 0) 
                - coalesce(p.cancelcommission_amount, 0) 
                - coalesce(p.commissiondiscount_amount, 0)
            ) as profit, 
            p.sign as count, 
            btrim(nullif(nullif(p.ticketingiataoffice, ''), ''''''))::varchar(255) as ticketingiataoffice, 
            p.originator as gds, 
            p.currency, 
			coalesce(rp.departure::date, p.departure::date) as departure,
			(select max(departuretime)::date from lt_flight_segment where ticket = coalesce(rp.id, p.id)) as backdeparture,        
            btrim(p.bookeroffice)::varchar(255) as bookeroffice,

            cast(case p.currency 
                when 'EUR' then 1
                when 'RUB' then 1 / r.rub_eur
                when 'UAH' then 1 / r.uah_eur
                when 'USD' then r.eur_usd
            end as numeric(18, 8)) as eur_rate,

            cast(case p.currency 
                when 'EUR' then r.rub_eur
                when 'RUB' then 1
                when 'UAH' then 1 / r.uah_rub
                when 'USD' then r.rub_usd
            end as numeric(18, 8)) as rub_rate,

            cast(case p.currency 
                when 'EUR' then r.uah_eur
                when 'RUB' then r.uah_rub
                when 'UAH' then 1
                when 'USD' then r.uah_usd
            end as numeric(18, 8)) as uah_rate,

            cast(case p.currency 
                when 'EUR' then 1 / r.eur_usd
                when 'RUB' then 1 / r.rub_usd
                when 'UAH' then 1 / r.uah_usd
                when 'USD' then 1
            end as numeric(18, 8)) as usd_rate
            
          from 
           (select 
                p.*, 
                case when isrefund then -1 else 1 end as sign,
                coalesce(p.grandtotal_currency, p.total_currency, p.equalfare_currency) as currency
              from lt_product p
            ) p
			left join lt_product rp on p.refundedproduct = rp.id
            left join lt_currency_daily_rate r on r.date_ = p.issuedate
			left join lt_airline_month_commission amc 
				on p.issuedate between amc.datefrom and amc.dateto
			   and amc.airline = coalesce(p.producer, p.provider)
			left join (
				select 
					document,
					sum(case when code = 'YR' then amount_amount end) as fee_yr,
					sum(case when code = 'YQ' then amount_amount end) as fee_yq,
					sum(case when code <> 'YR' and code <> 'YQ' then amount_amount end) as fee_rest
				  from lt_avia_document_fee 
				 group by document
			) f on f.document = p.id

         where not p.isreservation and not p.requiresprocessing and not p.isvoid
         
        ) p
    ) p
;


create view olap_departure_dim
as 
select distinct 
    departure::date as departure, 
    date_part('year', departure)::integer as year, 
    date_part('quarter', departure)::integer as quarter, 
    date_part('month', departure)::integer as month, 
    date_part('day', departure)::integer as day
  from lt_product p
 where not p.isreservation and not p.requiresprocessing and not p.isvoid
 order by 1;


create view olap_seller_dim
as
select p.id, (p.name || ' (УФСА)')::varchar(4000) as name
  from (select distinct seller as id from ufsa.lt_product) q
	inner join ufsa.lt_party p on q.id = p.id
union select p.id, p.name || ' (УТБ)'
  from (select distinct seller as id from utb.lt_product) q
	inner join utb.lt_party p on q.id = p.id
union all
select p.id, p.name || ' (ТОВ УФСА)' as name
  from (select distinct seller as id from tovufsa.lt_product) q
	inner join tovufsa.lt_party p on q.id = p.id
 order by 2;
 