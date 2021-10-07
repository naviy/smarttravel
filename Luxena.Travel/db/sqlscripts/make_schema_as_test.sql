begin work;
-- 
-- create temp table delDocs --on commit drop
-- as
-- with recursive docs(id) as (
--     select id from lt_avia_document 
--      where issueDate < '2013-10-1'
--   union all
--     select q.id
--       from docs d, (
--         select distinct id, reissuefor as id2 from lt_avia_document union
--         select distinct id, inconnectionwith as id2 from lt_avia_mco
--       ) q
--      where d.id = q.id2
-- )
-- select distinct id from docs;
-- 
-- create unique index ix_delDocs on delDocs (id);




-- delete from lt_avia_document_fee
--  where document in (select id from delDocs);
-- 
-- delete from lt_avia_document_voiding
--  where document in (select id from delDocs);
-- 
-- delete from lt_flight_segment
--  where ticket in (select id from delDocs);
-- 

-- delete from lt_penalize_operation
--  where ticket in (select id from delDocs);
-- 

-- delete from lt_avia_ticket
--  where id in (select id from delDocs);
-- 
-- delete from lt_avia_mco
--  where id in (select id from delDocs);
-- 
-- 
-- delete from lt_avia_refund
--  where id in (select id from delDocs);
-- 
-- delete from lt_avia_refund
--  where refundeddocument in (select id from delDocs);


-- create temp table delAviaLinks --on commit drop
-- as
-- select id 
--   from lt_order_item_avia_link
--  where document in (select id from delDocs);
-- create unique index ix_delAviaLinks on delAviaLinks (id);

-- delete from lt_order_item_avia_link
--  where id in (select id from delAviaLinks);
--  
-- delete from lt_order_item_source_link
--  where id in (select id from delAviaLinks);
-- 
-- delete from lt_order_item
--  where id in (select id from delAviaLinks);

-- delete 
--   from lt_avia_document a
--     using delDocs b
--  where a.id = b.id;

-- delete from lt_gds_file f
--  where not exists(select id from lt_avia_document where f.id = originaldocument);
--  

-- drop table if exists delOrders;
-- create temp table delOrders --on commit drop
-- as
-- select id 
--   from lt_order o
--  where issuedate < '2013-10-1';-- and not exists(select id from lt_avia_document where o.id = order_);
-- create unique index ix_delOrders on delOrders (id);
-- 
-- --select count(*) from lt_order;
-- --select count(*) from delOrders;
-- 
-- 
-- drop table if exists delOrderItems;
-- create temp table delOrderItems --on commit drop
-- as
-- select id 
--   from lt_order_item
--  where order_ in (select id from delOrders);
-- create unique index ix_delOrderItems on delOrderItems (id);
-- 
-- delete from lt_order_item_avia_link
--  where id in (select id from delOrderItems);
--  
-- delete from lt_order_item_source_link
--  where id in (select id from delOrderItems);
-- 
-- delete from lt_order_item
--  where id in (select id from delOrderItems);
-- 
-- 
-- delete from lt_payment
--  where order_ in (select id from delOrders);
-- 
-- delete from lt_invoice
--  where order_ in (select id from delOrders);
-- 
-- 
-- delete from lt_invoice
--  where order_ in (select id from delOrders);
-- 
-- delete from lt_internal_transfer
--  where fromorder in (select id from delOrders);
-- delete from lt_internal_transfer
--  where toorder in (select id from delOrders);
-- 
-- update lt_avia_document
--    set order_ = null
--  where order_ in (select id from delOrders);
-- 
-- delete from lt_order
--  where id in (select id from delOrders);


-- drop table if exists delConsignments;
-- create temp table delConsignments --on commit drop
-- as
-- select id 
--   from lt_consignment
--  where issuedate < '2013-10-1';
-- create unique index ix_delConsignments on delConsignments (id);
-- 
-- 
-- delete from lt_issued_consignment
--  where consignment in (select id from delConsignments);
--  
-- delete from lt_consignment
--  where id in (select id from delConsignments);
-- 
-- 


-- /*** Удаление персон ***/

-- drop table if exists delPersons;
-- create temp table delPersons -- on commit drop
-- as
--     select id from lt_party
--     except select booker from lt_avia_document 
--     except select customer from lt_avia_document 
--     except select intermediary from lt_avia_document 
--     except select passenger from lt_avia_document
--     except select seller from lt_avia_document 
--     except select owner from lt_avia_document 
--     except select ticketer from lt_avia_document 
--     except select owner from lt_document_owner
--     except select person from lt_document_access
--     except select person from lt_gds_agent
--     except select billto from lt_order
--     except select customer from lt_order
--     except select person from lt_user
--     except select relatedto from lt_task
--     except select assignedto from lt_task
--     except select reportsto from lt_party
--     except select owner from lt_miles_card;
-- 
-- -- 
-- delete from lt_passport
--  where owner in (select id from delPersons); 
-- -- 
-- delete from lt_person
--  where id in (select id from delPersons); 
-- 
-- 
-- delete from lt_organization
--  where id in (
--     select id from lt_party     
--     except select organization from lt_airline
--     except select organization from lt_person
--     except select organization from lt_department
--     except select customer from lt_avia_document 
--     except select intermediary from lt_avia_document 
--     except select owner from lt_avia_document 
--     except select owner from lt_document_owner
--     except select billto from lt_order
--     except select customer from lt_order
--     except select relatedto from lt_task
--     except select assignedto from lt_task
--     except select organization from lt_miles_card
-- 
--     except select reportsto from lt_party
-- ); 
-- 
-- 
-- delete from lt_opening_balance
--  where party in (
--     select id from lt_party
--     except select id from lt_organization  
--     except select id from lt_person
-- );
-- 
-- 
-- delete from lt_file
--  where party in (
--     select id from lt_party 
--     except select id from lt_department
--     except select id from lt_person
--     except select id from lt_organization
-- );
-- 
-- 
-- delete from lt_party
--  where id in (
--     select id from lt_party 
--     except select id from lt_department
--     except select id from lt_person
--     except select id from lt_organization
-- );


truncate table lt_modification_items;
truncate table lt_modification;

--commit;
rollback;
