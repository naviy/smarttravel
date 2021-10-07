--begin work;

--drop table if exists lt_product;


create table lt_product
(
    id varchar(32) not null,
    constraint lt_product_pkey primary key (id),
    
    version integer not null,

    createdby citext2 not null,
    createdon timestamp without time zone not null,
    modifiedby citext2,
    modifiedon timestamp without time zone,

    type integer not null,

    name citext2,

    order_ varchar(32),
    constraint product_order_fkey foreign key (order_) references lt_order (id),

    reissuefor varchar(32),
    constraint product_reissuefor_fkey foreign key (reissuefor) references lt_product (id),
    
    issuedate date not null,
    passengername citext2,

    owner varchar(32),
    constraint product_owner_fkey foreign key (owner) references lt_party (id),
      
    customer varchar(32),
    constraint product_customer_fkey foreign key (customer) references lt_party (id),
      
    seller varchar(32),
    constraint product_seller_fkey foreign key (seller) references lt_person (id),
      
    intermediary varchar(32),
    constraint product_intermediary_fkey foreign key (intermediary) references lt_party (id),

    isreservation boolean not null,
    isprocessed boolean not null,
    isvoid boolean not null,
    requiresprocessing boolean not null,

	country varchar(32),
    constraint product_country_fkey foreign key (country) references lt_country (id),

    pnrcode citext2,
	tourcode citext2,

    fare_amount numeric(19,5),
    fare_currency varchar(32),
    constraint product_fare_currency_fk foreign key (fare_currency) references lt_currency (id),
      
    equalfare_amount numeric(19,5),
    equalfare_currency varchar(32),
    constraint product_equalfare_currency_fk foreign key (equalfare_currency) references lt_currency (id),

    feestotal_amount numeric(19,5),
    feestotal_currency varchar(32),
    constraint product_feestotal_currency_fk foreign key (feestotal_currency) references lt_currency (id),

    cancelfee_amount numeric(19,5),
    cancelfee_currency varchar(32),
    constraint product_cancelfee_currency_fk foreign key (cancelfee_currency) references lt_currency (id),

    cancelcommissionpercent numeric(19,5),
    
    cancelcommission_amount numeric(19,5),
    cancelcommission_currency varchar(32),
    constraint product_cancelcommission_currency_fk foreign key (cancelcommission_currency) references lt_currency (id),

    total_amount numeric(19,5),
    total_currency varchar(32),
    constraint product_total_currency_fk foreign key (total_currency) references lt_currency (id),


    vat_amount numeric(19,5),
    vat_currency varchar(32),
    constraint product_vat_currency_fk foreign key (vat_currency) references lt_currency (id),

    commissionpercent numeric(19,5),
    
    commission_amount numeric(19,5),
    commission_currency varchar(32),
    constraint product_commission_currency_fk foreign key (commission_currency) references lt_currency (id),

    commissiondiscount_amount numeric(19,5),
    commissiondiscount_currency varchar(32),
    constraint product_commissiondiscount_currency_fk foreign key (commissiondiscount_currency) references lt_currency (id),

    servicefee_amount numeric(19,5),
    servicefee_currency varchar(32),
    constraint product_servicefee_currency_fk foreign key (servicefee_currency) references lt_currency (id),

    handling_amount numeric(19,5),
    handling_currency varchar(32),
    constraint product_handling_currency_fk foreign key (handling_currency) references lt_currency (id),

    discount_amount numeric(19,5),
    discount_currency varchar(32),
    constraint product_discount_currency_fk foreign key (discount_currency) references lt_currency (id),

    grandtotal_amount numeric(19,5),
    grandtotal_currency varchar(32),
    constraint product_grandtotal_currency_fk foreign key (grandtotal_currency) references lt_currency (id),

    paymenttype integer not null,
    note citext2
    
);


create index product_owner_issuedate_name_idx on lt_product (owner desc, issuedate desc, name desc);
create index product_issuedate_name_owner_idx on lt_product (issuedate desc, name desc, owner desc);
alter table lt_product cluster on product_issuedate_name_owner_idx;


create index product_order_idx on lt_product (order_);


create index product_owner_idx on lt_product (owner);
create index product_customer_idx on lt_product (customer);
create index product_seller_idx on lt_product (seller);
create index product_intermediary_idx on lt_product (intermediary);
create index product_country_idx on lt_product (country);

create index product_fare_currency_idx on lt_product (fare_currency);
create index product_equalfare_currency_idx on lt_product (equalfare_currency);
create index product_feestotal_currency_idx on lt_product (feestotal_currency);
create index product_cancelfee_currency_idx on lt_product (cancelfee_currency);
create index product_cancelcommission_currency_idx on lt_product (cancelcommission_currency);
create index product_total_currency_idx on lt_product (total_currency);


create index product_vat_currency_idx on lt_product (vat_currency);
create index product_commission_currency_idx on lt_product (commission_currency);
create index product_commissiondiscount_currency_idx on lt_product (commissiondiscount_currency);
create index product_servicefee_currency_idx on lt_product (servicefee_currency);
create index product_handling_currency_idx on lt_product (handling_currency);
create index product_discount_currency_idx on lt_product (discount_currency);
create index product_grandtotal_currency_idx on lt_product (grandtotal_currency);


insert into 
    lt_product
select 
    d.id, 
    d.version, 

    createdby, 
    createdon,
    modifiedby, 
    modifiedon,
    
    type,

    case when number_ is null
        then concat(pnrcode, '-', passengername)
        else concat(airlineprefixcode, '-', lpad(number_::varchar(10), 10, '0'))
    end::varchar(70) as name,

    order_,
    reissuefor,
    
    issuedate,
    passengername,

    owner,
    customer,
    seller,
    intermediary,

    number_ is null as isreservation,
    isprocessed,
    isvoid,
    requiresprocessing,

    null as country,
    pnrcode,
    tourcode,

    fare_amount, fare_currency,
    equalfare_amount, equalfare_currency,
    feestotal_amount, feestotal_currency,                   -- Таксы    
    cancelfee_amount, cancelfee_currency,                   -- Штраф за отмену
    cancelcommissionpercent,
    cancelcommission_amount, cancelcommission_currency,     -- Комисия за возврат
    total_amount, total_currency,                           -- Итого по АК
    
    vat_amount, vat_currency,                               -- В т.ч. НДС
    commissionpercent,
    commission_amount, commission_currency,                 -- Комиссия
    commissiondiscount_amount, commissiondiscount_currency, -- Скидка от ком. АК
    servicefee_amount, servicefee_currency,                 -- Сервисный сбор
    handling_amount, handling_currency,                     -- Доп. доход от АК
    discount_amount, discount_currency,                     -- Скидка
    grandtotal_amount, grandtotal_currency,                 -- К оплате
    
    paymentType,
    note
    
from 
    lt_avia_document d
    left join lt_avia_mco m on m.id = d.id
    left join lt_avia_refund r on r.id = d.id
    left join lt_avia_ticket t on t.id = d.id
 ;


alter table lt_avia_document 
    add constraint avia_document_fkey foreign key (id) references lt_product (id);


create table lt_product_passenger
(
    id varchar(32) not null,
    constraint lt_product_passenger_pkey primary key (id),
    
    version integer not null,

    createdby citext2 not null,
    createdon timestamp without time zone not null,
    modifiedby citext2,
    modifiedon timestamp without time zone,

    product varchar(32) not null,
    constraint product_passenger_product_fkey foreign key (product) references lt_product (id),    
    
    passengername citext2,
    passenger varchar(32),
    constraint product_passenger_passenger_fkey foreign key (passenger) references lt_person (id)
    
);

create index product_passenger_product_idx on lt_product_passenger (product);
create index product_passenger_passenger_idx on lt_product_passenger (passenger);

insert into 
    lt_product_passenger 
    (id, version, createdby, createdon, product, passengername, passenger)
select 
    reverse(id), version, createdby, createdon, id, passengername, passenger
from
    lt_avia_document
where
    passengername is not null or passenger is not null;


alter table lt_avia_document
    drop column version,

    drop column createdby,
    drop column createdon,
    drop column modifiedby,
    drop column modifiedon,

    drop column type cascade,

    drop column order_ cascade,
    drop column reissuefor cascade,
    drop column issuedate cascade,

	drop column passengername cascade,
    drop column passenger cascade,

    drop column owner cascade,
    drop column customer cascade,
    drop column seller cascade,
    drop column intermediary cascade,

    drop column isprocessed cascade,
    drop column isvoid cascade,
    drop column requiresprocessing cascade,

    drop column pnrcode cascade,
    drop column tourcode cascade,

    drop column fare_amount,
    drop column fare_currency cascade,
    drop column equalfare_amount,
    drop column equalfare_currency cascade,

    drop column feestotal_amount,
    drop column feestotal_currency cascade,

    drop column total_amount,
    drop column total_currency cascade,

    drop column vat_amount,
    drop column vat_currency cascade,

    drop column commissionpercent,
    drop column commission_amount,
    drop column commission_currency cascade,

    drop column commissiondiscount_amount,
    drop column commissiondiscount_currency cascade,

    drop column servicefee_amount,
    drop column servicefee_currency cascade,

    drop column handling_amount,
    drop column handling_currency cascade,

    drop column discount_amount,
    drop column discount_currency cascade,

    drop column grandtotal_amount,
    drop column grandtotal_currency cascade,

    drop column paymenttype,
    drop column note
;

alter table lt_avia_refund
    drop column cancelfee_amount,
    drop column cancelfee_currency cascade,
    drop column cancelcommissionpercent,
    drop column cancelcommission_amount,
    drop column cancelcommission_currency cascade
;



alter table lt_order_item add column linktype integer;
alter table lt_order_item add column product varchar(32);

alter table lt_order_item add constraint orderitem_product_fkey 
foreign key (product) references lt_product (id);

create index orderitem_product_idx on lt_order_item (product);

update 
    lt_order_item as oi 
set 
    linktype = s.linktype, 
    product = s.document 
from 
    lt_order_item_avia_link s
where 
    s.id = oi.id;


drop table lt_order_item_avia_link cascade;
drop table lt_order_item_source_link cascade;


--commit;
--rollback;