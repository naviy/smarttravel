SET search_path = demo;

-- begin work;

drop table if exists newPersonNames;

create temp table newPersonNames --on commit drop
as
select
    id,
    name,
    row_number() over(order by id) as idRowNo,
    row_number() over(order by name) as nameRowNo
  from lt_party
 where class = 'Person';
 
--begin work;

update lt_party p
    set name = p.id
  from newPersonNames nn1
 where nn1.id = p.id;

update lt_party p
    set name = nn2.name
  from newPersonNames nn1
    inner join newPersonNames nn2 on nn1.idRowNo = nn2.nameRowNo
 where nn1.id = p.id;

--rollback;


drop table if exists newOrganizationNames;

create temp table newOrganizationNames --on commit drop
as
select
    id,
    name
  from (
    select
        id,
        trim(replace(concat(name1, ' ', name2, ' ', name3), '  ', ' ')) as name
      from (
        select
            id,
            names[1 + random() * (array_length(names, 1) - 1)] as name1,
            names[1 + random() * 1.2 * array_length(names, 1)] as name2,
            names[1 + random() * 4 * array_length(names, 1)] as name3
          from 
            (select id from lt_party 
              where class = 'Organization' and not isairline
			) o
            cross join (
                select array_agg(name) as names 
                  from (
                    select distinct regexp_split_to_table(name, E'\\s+|/') as name
                      from lt_party
                     where class = 'Organization'
                       and name not similar to '%[а-яА-Я(0-9]%'
                  ) q
                 where length(name) >= 3
            ) q2
        ) q3
    ) q4
  where not exists(select null from lt_party where name = q4.name);

delete from newOrganizationNames n1
 where exists(select null from newOrganizationNames where id <> n1.id and name = n1.name);

--select * from newOrganizationNames

update lt_party pt set 
    name = n.name
  from newOrganizationNames n
 where pt.id = n.id;


-- -- -- drop table if exists newOrganizationNames;

-- -- -- create temp table newOrganizationNames --on commit drop
-- -- -- as
-- -- -- select 
-- -- --     id, 
-- -- --     name,
-- -- --     row_number() over(order by id) as idRowNo,
-- -- --     row_number() over(order by name) as nameRowNo
-- -- --   from lt_party
-- -- --  where class = 'Organization' and not isairline;


-- -- -- -- begin work;

-- -- -- update lt_party p
-- -- --     set name = p.id
-- -- --   from newOrganizationNames nn1
-- -- --  where nn1.id = p.id;

-- -- -- update lt_party p
-- -- --     set name = nn2.name
-- -- --   from newOrganizationNames nn1
-- -- --     inner join newOrganizationNames nn2 on nn1.idRowNo = nn2.nameRowNo
-- -- --  where nn1.id = p.id;
 
-- rollback;


drop table if exists newDepartmentNames;

create temp table newDepartmentNames --on commit drop
as
select 
    id, 
    name,
    row_number() over(order by id) as idRowNo,
    row_number() over(order by name) as nameRowNo
  from lt_party
 where class = 'Department' and not isairline;


-- begin work;

update lt_party p
    set name = p.id
  from newDepartmentNames nn1
 where nn1.id = p.id;

update lt_party p
    set name = nn2.name
  from newDepartmentNames nn1
    inner join newDepartmentNames nn2 on nn1.idRowNo = nn2.nameRowNo
 where nn1.id = p.id;
 
-- rollback;


update lt_party set 
    legalName = name,
    phone1 = null, phone2 = null,
    email1 = null, email2 = null,
    fax = null, webAddress = null;


update lt_passport psp set 
    firstname = split_part(name, ' ', 1),
    lastname = split_part(name, ' ', 2)
  from lt_party p
 where p.id = psp.owner;
 
update lt_product_passenger psg set
    passengername = upper(pt.name)
  from lt_party pt
 where psg.passenger = pt.id;
 
 update lt_product p set
    passengername = psg.passengername
  from lt_product_passenger psg
 where psg.product = p.id;

 update lt_party p set code = null;


/*** GDS agents ***/

create temp table newGdsOffices on commit drop
as
select officeCode, concat('OFF', round(random() * 8999) + 1000) as newCode
  from (
    select distinct officeCode from lt_gds_agent
) q;

update lt_gds_agent a set 
    officeCode = (select newCode from newGdsOffices where officeCode = a.officeCode),
    code = concat('AG', round(random() * 8999) + 1000);

update lt_product p set
    bookerOffice = a.officeCode,
    bookerCode = a.code
  from lt_gds_agent a
 where a.person = booker;

update lt_product p set
    ticketerOffice = a.officeCode,
    ticketerCode = a.code
  from lt_gds_agent a
 where a.person = ticketer;


/*** Имена пользователей ***/

update lt_identity i 
   set name = concat('admin', rowno)
  from (
	select u.id, row_number() over (order by active desc, p.name) as rowno
	  from lt_identity u
		inner join lt_party p on u.person = p.id
	 where u.isadministrator
   ) q
 where i.id = q.id;

update lt_identity i 
   set name = concat('supervisor', rowno)
  from (
	select u.id, row_number() over (order by active desc, p.name) as rowno
	  from lt_identity u
		inner join lt_party p on u.person = p.id
	 where u.issupervisor and not u.isadministrator
   ) q
 where i.id = q.id;

update lt_identity i 
   set name = concat('agent', rowno)
  from (
	select u.id, row_number() over (order by active desc, p.name) as rowno
	  from lt_identity u
		inner join lt_party p on u.person = p.id
	 where u.isagent and not u.issupervisor and not u.isadministrator
   ) q
 where i.id = q.id;



drop table if exists orderIssueDateOffsets;

create temp table orderIssueDateOffsets
as
select id, round(random() * 30 - 15) * '1 day'::INTERVAL as offset
  from lt_order;

update lt_order o set 
	issuedate = issuedate + oo.offset
  from orderIssueDateOffsets oo
 where o.id = oo.id;

update lt_product p set 
	issuedate = issuedate + oo.offset
  from orderIssueDateOffsets oo
 where p.order_ = oo.id;

update lt_payment p set 
	date_ = date_ + oo.offset
  from orderIssueDateOffsets oo
 where p.order_ = oo.id;

update lt_system_configuration 
   set companydetails = replace(replace(replace(replace(companydetails, '1', '9'), '2', '8'), '3', '7'), '4', '6');

