drop view if exists olap_transactiondate_dim;
drop view if exists olap_transaction_dim;


create view olap_transaction_dim
as
    select 
        p.customer, 
        p.order_ as "order", 
        p.issuedate, 
        p.name::varchar(4000) as number, 
        p.grandtotal_currency as currency, 
        case
            when p.type = 2 then - p.grandtotal_amount
            else p.grandtotal_amount
        end as amount
      from lt_product p
     where not p.isvoid
union all 
    select 
        p.payer as customer, 
        p.order_ as "order", 
        p.postedon as issuedate, 
        p.number_::varchar(4000) as number, 
        p.amount_currency as currency, 
        p.sign * p.amount_amount as amount
       from lt_payment p
      where not p.isvoid and not p.postedon is null;


create view olap_transactiondate_dim
as
select distinct 
    date_part('year', issuedate)::integer as year,
    to_char(issuedate, 'mm month') as month,
    to_char(issuedate, 'dd') as day,
    issuedate
  from olap_transaction_dim
 order by issuedate;
