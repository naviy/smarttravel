drop view if exists olap_issuedate_dim;


create view olap_issuedate_dim
as 
select distinct 
    date_part('year', issuedate)::integer as year, 
    date_part('quarter', issuedate)::integer as quarter, 
    to_char(issuedate, 'MM Month') as month,
    case
        when date_part('day', issuedate) >= 1 and date_part('day', issuedate) <= 7 then 1
        when date_part('day', issuedate) >= 8 and date_part('day', issuedate) <= 15 then 2
        when date_part('day', issuedate) >= 16 and date_part('day', issuedate) <= 23 then 3
        else 4
    end as period, 
    to_char(issuedate, 'DD') as day, 
    issuedate
  from lt_product
 order by issuedate;


/*
select * from olap_issuedate_dim
*/