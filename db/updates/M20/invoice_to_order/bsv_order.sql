rename table lt_invoice to lt_order;
rename table lt_invoice_item to lt_order_item;
rename table lt_invoice_item_avia_link to lt_order_item_avia_link;
rename table lt_invoice_item_source_link to lt_order_item_source_link;
rename table lt_void_item to lt_order_status_record;
rename table lt_printed_invoice to lt_invoice;

alter table lt_avia_document
  add `order_` varchar(32) default NULL after intermediary;
	
alter table lt_avia_document add key `order_` ( `order_` );

ALTER TABLE `lt_avia_document` ADD CONSTRAINT `aviadocument_order_fk`
	FOREIGN KEY ( `order_` ) REFERENCES `lt_order` ( `id` );

	
alter table lt_document_owner
  change isactive `isactive` tinyint(1) NOT NULL;

alter table lt_avia_document
  drop column ispaid;
  
alter table lt_gds_file
  change class `class` varchar(10) NOT NULL;
	
	
alter table lt_order
	drop FOREIGN KEY invoice_acquirer_fk,
	drop FOREIGN KEY invoice_discount_currency_fk,
	drop FOREIGN KEY invoice_grandtotal_currency_fk,
	drop FOREIGN KEY invoice_owner_fk,
	drop FOREIGN KEY invoice_payer_fk,
	drop FOREIGN KEY invoice_signedby_fk,
	drop FOREIGN KEY invoice_supplier_fk,
	drop FOREIGN KEY invoice_vat_currency_fk;

alter table lt_order 
	drop key acquirer,
	drop key grandtotal_currency,
	drop key payer,
	drop key signedby,
	drop key supplier;
	
alter table lt_order
  add `note` text default NULL after agreement,
	add `customer` varchar(32) default NULL after note,
	drop supplier,
	drop daystillexpiration,
	change acquirer shipto varchar(32) default NULL,
	change payer `billto` varchar(32) default NULL,
	change signedby `assignedto` varchar(32) default NULL,
	change grandtotal_amount `total_amount` decimal(19,5) default NULL,
	change grandtotal_currency `total_currency` varchar(32) default NULL,
	add `paid_amount` decimal(19,5) default NULL after total_amount,
	add `paid_currency` varchar(32) default NULL after paid_amount,
	add `totaldue_amount` decimal(19,5) default NULL after paid_currency,
	add `totaldue_currency` varchar(32) default NULL after totaldue_amount,
	add `vatdue_amount` decimal(19,5) default NULL after totaldue_currency,
	add `vatdue_currency` varchar(32) default NULL after vatdue_amount,
	drop printedinvoicescounter;
	
update lt_order 
 set customer = shipto;

alter table lt_order 
 change customer `customer` varchar(32) not NULL;
	
update lt_order 
  set status = 0
where status = 1 and paymenttimestamp is null;

update lt_order
set paid_amount = 0,
	paid_currency = total_currency,
	totaldue_amount = total_amount,
	totaldue_currency = total_currency,
	vatdue_amount = vat_amount,
	vatdue_currency = total_currency;

update lt_order o
  inner join (
		select o.id, sum(p.amount_amount) paid, o.total_amount - sum(p.amount_amount) total_due, round(o.vat_amount - sum(p.vat_amount), 2) vat_due
		from lt_order o
			inner join lt_payment p on p.invoice = o.id and p.isvoid = 0 and p.whenposted is not null
		group by o.id) t on o.id = t.id
	
	set paid_amount = t.paid,
		totaldue_amount = t.total_due,
		vatdue_amount = t.vat_due;
		
alter table lt_order
	drop paymenttimestamp;

alter table lt_order
	add KEY `assignedto` ( `assignedto` ),
	add KEY `billto` ( `billto` ),
	add KEY `customer` ( `customer` ),
	add KEY `paid_currency` ( `paid_currency` ),
	add KEY `shipto` ( `shipto` ),
	add KEY `total_currency` ( `total_currency` ),
	add KEY `totaldue_currency` ( `totaldue_currency` ),
	add KEY `vatdue_currency` ( `vatdue_currency` );
	
	
ALTER TABLE `lt_order` ADD CONSTRAINT `order_assignedto_fk`
	FOREIGN KEY ( `assignedto` ) REFERENCES `lt_person` ( `id` );

ALTER TABLE `lt_order` ADD CONSTRAINT `order_billto_fk`
	FOREIGN KEY ( `billto` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_order` ADD CONSTRAINT `order_customer_fk`
	FOREIGN KEY ( `customer` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_order` ADD CONSTRAINT `order_discount_currency_fk`
	FOREIGN KEY ( `discount_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_order` ADD CONSTRAINT `order_owner_fk`
	FOREIGN KEY ( `owner` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_order` ADD CONSTRAINT `order_paid_currency_fk`
	FOREIGN KEY ( `paid_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_order` ADD CONSTRAINT `order_shipto_fk`
	FOREIGN KEY ( `shipto` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_order` ADD CONSTRAINT `order_total_currency_fk`
	FOREIGN KEY ( `total_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_order` ADD CONSTRAINT `order_totaldue_currency_fk`
	FOREIGN KEY ( `totaldue_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_order` ADD CONSTRAINT `order_vat_currency_fk`
	FOREIGN KEY ( `vat_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_order` ADD CONSTRAINT `order_vatdue_currency_fk`
	FOREIGN KEY ( `vatdue_currency` ) REFERENCES `lt_currency` ( `id` );


alter table lt_invoice
  drop FOREIGN KEY printedinvoice_invoice_fk,
	drop key invoice;
	
alter table lt_invoice
  add `version` int(11) default NULL after id,
	add `agreement` varchar(255) default NULL after number_,
	add `type` int(11) default NULL after agreement,
	add `issuedate` datetime default NULL after type,
	change invoice `order_` varchar(32) default NULL,
	add `total_amount` decimal(19,5) default NULL after issuedby,
	add `total_currency` varchar(32) default NULL after total_amount,
	add KEY `order_` ( `order_` ),
	add KEY `total_currency` ( `total_currency` );
	

ALTER TABLE `lt_invoice` ADD CONSTRAINT `invoice_total_currency_fk`
	FOREIGN KEY ( `total_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_invoice` ADD CONSTRAINT `order_invoices_fk`
	FOREIGN KEY ( `order_` ) REFERENCES `lt_order` ( `id` );
	

update lt_invoice inv
  inner join lt_order o on inv.order_ = o.id
set
	inv.version = 1,
	inv.agreement = o.agreement,
	inv.type = o.type,
	inv.issuedate = inv.timestamp,
	inv.total_amount = o.total_amount,
	inv.total_currency = o.total_currency;
	
update lt_invoice inv
	inner join lt_order o on inv.order_ = o.id
set inv.number_ = o.number_
where inv.number_ is null;

alter table lt_invoice
  change version `version` int(11) NOT NULL,
	change number_ 	`number_` varchar(255) NOT NULL,
	change `type` `type` int(11) NOT NULL,
	change `issuedate` `issuedate` datetime NOT NULL
	;
	
ALTER TABLE `lt_invoice` 
drop FOREIGN KEY `order_invoices_fk`;

ALTER TABLE `lt_invoice` 
drop FOREIGN KEY `printedinvoice_issuedby_fk`;

ALTER TABLE `lt_invoice` ADD CONSTRAINT `invoice_issuedby_fk`
	FOREIGN KEY ( `issuedby` ) REFERENCES `lt_person` ( `id` );

ALTER TABLE `lt_invoice` ADD CONSTRAINT `invoice_order_fk`
	FOREIGN KEY ( `order_` ) REFERENCES `lt_order` ( `id` );

alter table lt_order_item
	drop FOREIGN KEY `invoice_item_consignment_fk`,
	drop FOREIGN KEY `invoice_item_discount_currency_fk`,
	drop FOREIGN KEY `invoice_item_givenvat_currency_fk`,
	drop FOREIGN KEY `invoice_item_grandtotal_currency_fk`,
	drop FOREIGN KEY `invoice_item_invoice_fk`,
	drop FOREIGN KEY `invoice_item_price_currency_fk`,
	drop FOREIGN KEY `invoice_item_taxedtotal_currency_fk`;

alter table lt_order_item
	drop key invoice;

alter table  lt_order_item
	change `invoice` `order_` varchar(32) NOT NULL;

alter table lt_order_item
	add KEY `order_` ( `order_` );
	
ALTER TABLE `lt_order_item` ADD CONSTRAINT `orderitem_consignment_fk`
	FOREIGN KEY ( `consignment` ) REFERENCES `lt_consignment` ( `id` );

ALTER TABLE `lt_order_item` ADD CONSTRAINT `order_item_discount_currency_fk`
	FOREIGN KEY ( `discount_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_order_item` ADD CONSTRAINT `order_item_givenvat_currency_fk`
	FOREIGN KEY ( `givenvat_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_order_item` ADD CONSTRAINT `order_item_grandtotal_currency_fk`
	FOREIGN KEY ( `grandtotal_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_order_item` ADD CONSTRAINT `order_item_price_currency_fk`
	FOREIGN KEY ( `price_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_order_item` ADD CONSTRAINT `order_item_taxedtotal_currency_fk`
	FOREIGN KEY ( `taxedtotal_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_order_item` ADD CONSTRAINT `orderitem_order_fk`
	FOREIGN KEY ( `order_` ) REFERENCES `lt_order` ( `id` );
	
ALTER TABLE `lt_order_item_avia_link` 
	drop FOREIGN KEY `invoice_item_avia_link_document_fk`;


ALTER TABLE `lt_order_item_avia_link` ADD CONSTRAINT `orderitemavialink_document_fk`
	FOREIGN KEY ( `document` ) REFERENCES `lt_avia_document` ( `id` );
	

alter table lt_order_status_record
	drop FOREIGN KEY void_item_invoice_fk,
	drop FOREIGN KEY void_item_voidedby_fk;

alter table lt_order_status_record
	drop key invoice,
	drop key voidedby;
	
alter table lt_order_status_record
	change voidedby `changedby` varchar(32) default NULL,
	change invoice `order_` varchar(32) default NULL,
	add status  int(11) default NULL after id,
	add `timestamp` datetime default NULL after status;
	
update lt_order_status_record rec
	inner join lt_order o on rec.order_ = o.id
set 
	rec.status = if (rec.voided = 0, 0, 2),
	timestamp = o.modifiedon;
	
alter table lt_order_status_record
	drop column voided,
	change status status  int(11) not NULL,
	change `timestamp` `timestamp` datetime not NULL;
	
alter table lt_order_status_record
	add KEY `order_` ( `order_` ),
	add key changedby ( `changedby` );
	
ALTER TABLE `lt_order_status_record` ADD CONSTRAINT `orderstatusrecord_order_fk`
	FOREIGN KEY ( `order_` ) REFERENCES `lt_order` ( `id` );

ALTER TABLE `lt_order_status_record` ADD CONSTRAINT `orderstatusrecord_changedby_fk`
	FOREIGN KEY ( `changedby` ) REFERENCES `lt_person` ( `id` );

alter table lt_payment
	add class varchar(20) default null after id,
	add `documentnumber` varchar(20) default NULL after number_,
	change whencharged `date_` datetime NOT NULL,
	change whenposted `postedon` datetime default NULL,
	add `printeddocument` longblob default NULL after postedon;
	
alter table lt_payment
	drop foreign key payment_invoice_fk,
	drop index invoice;

alter table lt_payment
	change invoice order_ varchar(32) default NULL,
	add `invoice` varchar(32) default NULL;

update lt_payment
 	set 
		class = case paymentform when 0 then 'CashOrder' when 1 then 'WireTransfer' when 2 then 'CashOrder' when 3 then 'Check' end,
		paymentform = case paymentform when 0 then 0 when 1 then 1 when 2 then 0 when 3 then 2 end;
	
alter table lt_payment
	change `class` `class` varchar(20) NOT NULL,
	add KEY `order_` ( `order_` ),
	add KEY `invoice` ( `invoice` );

alter table lt_payment
	add `documentuniquecode` varchar(30) DEFAULT NULL after `documentnumber`;

alter table lt_payment
	add UNIQUE KEY `documentuniquecode` (`documentuniquecode`);

update lt_payment
set documentuniquecode =  concat(documentnumber, '_', year(date_))
where class = 'CashOrder' and isvoid = 0 and documentnumber is not null;


alter table lt_payment 
	drop FOREIGN KEY payment_payer;
	
ALTER TABLE `lt_payment` ADD CONSTRAINT `payment_order_fk`
	FOREIGN KEY ( `order_` ) REFERENCES `lt_order` ( `id` );

ALTER TABLE `lt_payment` ADD CONSTRAINT `payment_invoice_fk`
	FOREIGN KEY ( `invoice` ) REFERENCES `lt_invoice` ( `id` );
	
ALTER TABLE `lt_payment` ADD CONSTRAINT `payment_payer_fk`
	FOREIGN KEY ( `payer` ) REFERENCES `lt_party` ( `id` );

alter table lt_system_configuration
	change aviainvoiceitemgenerationoption `aviaorderitemgenerationoption` int(11) NOT NULL,
	change allowagentsetinvoicevat `allowagentsetordervat` tinyint(1) NOT NULL,
	drop column allowagenteditdictionaries;

alter table lt_system_configuration
	drop FOREIGN KEY configuration_birthdaytaskresposible,
	drop FOREIGN KEY configuration_company,
	drop FOREIGN KEY configuration_country,
	drop FOREIGN KEY configuration_defaultcurrency;
 

ALTER TABLE `lt_system_configuration` ADD CONSTRAINT `systemconfiguration_birthdaytaskresponsible_fk`
	FOREIGN KEY ( `birthdaytaskresponsible` ) REFERENCES `lt_person` ( `id` );

ALTER TABLE `lt_system_configuration` ADD CONSTRAINT `systemconfiguration_company_fk`
	FOREIGN KEY ( `company` ) REFERENCES `lt_organization` ( `id` );

ALTER TABLE `lt_system_configuration` ADD CONSTRAINT `systemconfiguration_country_fk`
	FOREIGN KEY ( `country` ) REFERENCES `lt_country` ( `id` );

ALTER TABLE `lt_system_configuration` ADD CONSTRAINT `systemconfiguration_defaultcurrency_fk`
	FOREIGN KEY ( `defaultcurrency` ) REFERENCES `lt_currency` ( `id` );

alter table lt_avia_document_fee
drop FOREIGN KEY aviadocumentfee_document_fk;

ALTER TABLE `lt_avia_document_fee` ADD CONSTRAINT `aviadocumentfee_aviadocument_fk`
	FOREIGN KEY ( `document` ) REFERENCES `lt_avia_document` ( `id` );
	
alter table lt_avia_document_voiding
drop FOREIGN key aviadocumentvoiding_document_fk;

ALTER TABLE `lt_avia_document_voiding` ADD CONSTRAINT `aviadocumentvoiding_aviadocument_fk`
	FOREIGN KEY ( `document` ) REFERENCES `lt_avia_document` ( `id` );

alter table lt_flight_segment
drop FOREIGN key flightsegment_ticket_fk;

ALTER TABLE `lt_flight_segment` ADD CONSTRAINT `flightsegment_aviaticket_fk`
	FOREIGN KEY ( `ticket` ) REFERENCES `lt_avia_ticket` ( `id` );

ALTER TABLE `lt_miles_card` 
drop FOREIGN KEY `milescard_owner_fk`;

ALTER TABLE `lt_miles_card` ADD CONSTRAINT `milescard_person_fk`
	FOREIGN KEY ( `owner` ) REFERENCES `lt_person` ( `id` );

alter table lt_passport
 drop FOREIGN KEY passport_owner_fk;
 
ALTER TABLE `lt_passport` ADD CONSTRAINT `passport_person_fk`
	FOREIGN KEY ( `owner` ) REFERENCES `lt_person` ( `id` );
 
 
ALTER TABLE `lt_penalize_operation` 
drop FOREIGN KEY `penalizeoperation_ticket_fk`;

ALTER TABLE `lt_penalize_operation` ADD CONSTRAINT `penalizeoperation_aviaticket_fk`
	FOREIGN KEY ( `ticket` ) REFERENCES `lt_avia_ticket` ( `id` );

ALTER TABLE `lt_task_comment` 
drop FOREIGN KEY `task_comment_fk`;

ALTER TABLE `lt_task_comment` ADD CONSTRAINT `taskcomment_task_fk`
	FOREIGN KEY ( `task` ) REFERENCES `lt_task` ( `id` );

update lt_avia_document doc
	inner join lt_order_item_avia_link link on link.document = doc.id
	inner join lt_order_item item on item.id = link.id
	inner join lt_order o on o.id = item.order_ and o.status <> 2
set doc.order_ = o.id;

alter table lt_order
drop column `agreement`;


update lt_payment p
  inner join lt_order o  on p.order_ = o.id
	set p.documentnumber = o.number_
where o.type = 1 and p.isvoid = 0 and p.postedon is not null and p.amount_amount > 0
;


update lt_payment p
  inner join lt_order o on p.order_ = o.id and p.documentnumber is not null
  inner join lt_invoice i on i.order_ = o.id
	set p.printeddocument = i.content
where o.type = 1 and i.issuedate = (select max(ii.issuedate) from lt_invoice ii where ii.order_ = o.id)
;


update lt_payment
set documentnumber = null
where documentnumber is not null and printeddocument is null
;

delete i
from lt_invoice i
		inner join lt_order o on i.order_ = o.id
		inner join lt_payment p on p.order_ = o.id and p.documentnumber is not null
	where o.type = 1
;


alter table lt_order
	drop column type;

update lt_order
set number_ = concat(replace(number_, 'I', 'O'), 'I')
where number_ like 'I%';

update lt_order
set number_ = concat(replace(number_, 'A', 'O'), 'A')
where number_ like 'A%';

update lt_order
set number_ = concat(replace(number_, 'C', 'O'), 'C')
where number_ like 'C%';


update lt_sequence
set name = 'CashOrderPayment'
where name = 'CashInvoice';

INSERT INTO lt_sequence (id, name, format, timestamp, current) VALUES (6, 'Order', 'O.{0:yy}-{1:00000}', curdate(), 0);
INSERT INTO lt_sequence (id, name, format, timestamp, current) VALUES (7, 'Receipt', 'R.{0:yy}-{1:00000}', curdate(), 0);


update lt_sequence s1
  inner join (select max(current) c from lt_sequence where id in (1,3,4)) s2
set s1.current  = s2.c
where s1.id = 6;
