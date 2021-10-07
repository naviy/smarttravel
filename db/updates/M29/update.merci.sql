set client_encoding to 'win1251';

--begin work;

drop table lt_order_status_record;

alter table lt_avia_document
    add column paymentdetails public.citext2;

alter table lt_avia_mco
    add column description public.citext2,
    add column inconnectionwith character varying(32);

alter table lt_gds_file
    add column officeiata public.citext2;

alter table lt_modification_items
    alter column property type character varying(50),
    alter column oldvalue type text;


alter table lt_order
    drop column status,
    add column isvoid boolean not null default false,
    add column deliverybalance numeric(19,5) not null default 0,
    add column ispaid boolean not null default false;

alter table lt_order
    alter column isvoid drop default,
    alter column deliverybalance drop default,
    alter column ispaid drop default;


alter table lt_system_configuration
    add column metricsfromdate date,
    add column mcorequiresdescription boolean not null default false,
    add column reservationsinofficemetrics boolean not null default false;

alter table lt_system_configuration
    alter column mcorequiresdescription drop default,
    alter column reservationsinofficemetrics drop default;


alter table lt_task
    add column order_ character varying(32),
    alter column subject set not null;


/*alter table lt_user alter column password type text using upper(encode(password, 'hex'));*/
alter table lt_user alter column password type public.citext2;

alter table lt_avia_mco
	add constraint aviamco_inconnectionwith_fkey foreign key (inconnectionwith) references lt_avia_document(id);

alter table lt_task
	add constraint task_order_fkey foreign key (order_) references lt_order(id);

create index aviamco_inconnectionwith_idx on lt_avia_mco using btree (inconnectionwith);

create index task_order_idx on lt_task using btree (order_);

alter table lt_avia_document cluster on aviadocument_issuedate_displaystring_owner_idx;

 --rollback;