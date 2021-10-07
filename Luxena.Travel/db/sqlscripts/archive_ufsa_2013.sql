-- begin work;

delete from lt_modification where "timestamp" < '2016-1-1';
delete from lt_modification_items mi
 where not exists(select id from lt_modification where id = mi.modification);


drop table if exists delOrders cascade;

create temp table delOrders
as
select id from lt_order o 
 where issuedate < '2013-1-1';

create index id_delOrders1 on delOrders (id);

delete from delOrders
 where id in (
    select fromorder from lt_internal_transfer
     where toorder in (select id from lt_order o where not exists(select id from delOrders where id = o.id))
    union
    select toorder from lt_internal_transfer
     where fromorder in (select id from lt_order o where not exists(select id from delOrders where id = o.id))
);

-- select * from delOrders where id = '9c4e4701ee174b1fa3630c478bfe28a1';


drop table if exists delProducts;

create temp table delProducts 
as
select distinct product as id from lt_order_item oi
 where order_ in (select id from delOrders)
union
select id from lt_product
 where issuedate < '2013-1-1' and order_ is null;

create index id_delProducts on delProducts (id);

-- select count(*) from delProducts


drop table if exists addProducts;

create temp table addProducts
as
with recursive docs(id) as (
    select id, null::varchar(32) as id2 from lt_product p
     where not exists(select id from delProducts where id = p.id)
  union
    select q.id, q.id2
      from docs d, (
		select distinct id, reissuefor as id2 from lt_product
		union
		select distinct id, inconnectionwith as id2 from lt_product
		union
		select distinct id, refundedproduct as id2 from lt_product
        union
        select distinct reissuefor as id, id as id2 from lt_product
        union
        select distinct inconnectionwith as id, id as id2 from lt_product
        union
        select distinct refundedproduct as id, id as id2 from lt_product
      ) q
     where d.id = q.id2
)
select distinct id2 as id from docs where id is not null
union
select distinct id from docs where id2 is not null;

create index id_addProducts1 on addProducts (id);


-- select count(*) from addProducts

delete from delProducts
 where id in (select id from addProducts);

-- 'a5c6cc20a25a483aa8d7e20f26e914dc'


delete from lt_order_item
 where order_ in (select id from delOrders);
 
delete from lt_order_item
 where product in (select id from delProducts);
 

delete from lt_avia_document_fee
 where document in (select id from delProducts);

delete from lt_avia_document_voiding
 where document in (select id from delProducts);

delete from lt_flight_segment
 where ticket in (select id from delProducts);

delete from lt_penalize_operation
 where ticket in (select id from delProducts);

delete from lt_product_passenger
 where product in (select id from delProducts);

update lt_product set
    reissuefor = null,
    inconnectionwith = null,
    refundedproduct = null
 where id in (select id from delProducts);
 
delete from lt_product
 where id in (select id from delProducts);

delete from lt_gds_file f
 where not exists(select id from lt_product where originaldocument = f.id);


delete from lt_payment
 where order_ in (select id from delOrders);

delete from lt_invoice
 where order_ in (select id from delOrders);

delete from lt_issued_consignment ic
 where consignment in (
    select id from lt_consignment c
     where issuedate < '2013-1-1' and not exists(select id from lt_order_item where consignment = c.id)
 );

delete from lt_consignment c
 where issuedate < '2013-1-1' and not exists(select id from lt_order_item where consignment = c.id);

delete from lt_task
 where order_ in (select id from delOrders);

update lt_product set
    order_ = null
 where order_ in (select id from delOrders);

delete from lt_internal_transfer
 where fromorder in (select id from delOrders) or toorder in (select id from delOrders);

delete from lt_order
 where id in (select id from delOrders);
-- select * from delOrders where id = '9c4e4701ee174b1fa3630c478bfe28a1'

-- --commit;
--rollback;
