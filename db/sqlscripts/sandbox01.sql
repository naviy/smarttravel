begin work;

create table lt_order (
    id varchar(32) not null primary key,
    version integer not null,
    createdby citext2 not null,
    createdon timestamp not null,
    modifiedby citext2,
    modifiedon timestamp,
    
    number citext2,
    total decimal(18,4),
    vat decimal(18,4),
    separateservicefee boolean
);

create table lt_product (
    id varchar(32) not null primary key,
    version integer not null,
    createdby citext2 not null,
    createdon timestamp not null,
    modifiedby citext2,
    modifiedon timestamp,
    
    name citext2,
    order_ varchar(32) not null,
    feetotal decimal(18,4),
    servicefee decimal(18,4),
    total decimal(18,4)
);

create table lt_order_item (
    id varchar(32) not null primary key,
    version integer not null,
    createdby citext2 not null,
    createdon timestamp not null,
    modifiedby citext2,
    modifiedon timestamp,
    
    order_ varchar(32) not null,
    product varchar(32) not null,
    linktype int not null,
    text citext2,
    total decimal(18,4),
    vat decimal(18,4),
    discounttotal decimal(18,4),
    discountvat decimal(18,4)
);


create table lt_order_item_discount (
    id varchar(32) not null primary key,
    version integer not null,
    createdby citext2 not null,
    createdon timestamp not null,
    modifiedby citext2,
    modifiedon timestamp,
    
    orderitem varchar(32) not null,
    total decimal(18,4),
    vat decimal(18,4)
);

rollback;