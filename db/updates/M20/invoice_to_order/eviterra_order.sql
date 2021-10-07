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

drop table lt_invoice;

CREATE TABLE `lt_invoice` (
  `id` varchar(32) NOT NULL,
  `version` int(11) NOT NULL,
  `number_` varchar(255) NOT NULL,
  `agreement` varchar(255) DEFAULT NULL,
  `type` int(11) NOT NULL,
  `issuedate` datetime NOT NULL,
  `timestamp` datetime NOT NULL,
  `content` longblob NOT NULL,
  `order_` varchar(32) DEFAULT NULL,
  `issuedby` varchar(32) DEFAULT NULL,
  `total_amount` decimal(19,5) DEFAULT NULL,
  `total_currency` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `order_` (`order_`),
  KEY `issuedby` (`issuedby`),
  KEY `total_currency` (`total_currency`),
  CONSTRAINT `invoice_issuedby_fk` FOREIGN KEY (`issuedby`) REFERENCES `lt_person` (`id`),
  CONSTRAINT `invoice_order_fk` FOREIGN KEY (`order_`) REFERENCES `lt_order` (`id`),
  CONSTRAINT `invoice_total_currency_fk` FOREIGN KEY (`total_currency`) REFERENCES `lt_currency` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


drop table lt_order_item_avia_link;
drop table lt_order_item_source_link;
drop table lt_order_item;
	
	CREATE TABLE `lt_order_item` (
  `id` varchar(32) NOT NULL,
  `version` int(11) NOT NULL,
  `createdby` varchar(32) NOT NULL,
  `createdon` datetime NOT NULL,
  `modifiedby` varchar(32) DEFAULT NULL,
  `modifiedon` datetime DEFAULT NULL,
  `position` int(11) NOT NULL,
  `text` text NOT NULL,
  `quantity` int(11) NOT NULL,
  `hasvat` tinyint(1) NOT NULL,
  `order_` varchar(32) NOT NULL,
  `consignment` varchar(32) DEFAULT NULL,
  `price_amount` decimal(19,5) DEFAULT NULL,
  `price_currency` varchar(32) DEFAULT NULL,
  `discount_amount` decimal(19,5) DEFAULT NULL,
  `discount_currency` varchar(32) DEFAULT NULL,
  `grandtotal_amount` decimal(19,5) DEFAULT NULL,
  `grandtotal_currency` varchar(32) DEFAULT NULL,
  `givenvat_amount` decimal(19,5) DEFAULT NULL,
  `givenvat_currency` varchar(32) DEFAULT NULL,
  `taxedtotal_amount` decimal(19,5) DEFAULT NULL,
  `taxedtotal_currency` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `order_` (`order_`),
  KEY `consignment` (`consignment`),
  KEY `price_currency` (`price_currency`),
  KEY `discount_currency` (`discount_currency`),
  KEY `grandtotal_currency` (`grandtotal_currency`),
  KEY `givenvat_currency` (`givenvat_currency`),
  KEY `taxedtotal_currency` (`taxedtotal_currency`),
  CONSTRAINT `orderitem_consignment_fk` FOREIGN KEY (`consignment`) REFERENCES `lt_consignment` (`id`),
  CONSTRAINT `orderitem_order_fk` FOREIGN KEY (`order_`) REFERENCES `lt_order` (`id`),
  CONSTRAINT `order_item_discount_currency_fk` FOREIGN KEY (`discount_currency`) REFERENCES `lt_currency` (`id`),
  CONSTRAINT `order_item_givenvat_currency_fk` FOREIGN KEY (`givenvat_currency`) REFERENCES `lt_currency` (`id`),
  CONSTRAINT `order_item_grandtotal_currency_fk` FOREIGN KEY (`grandtotal_currency`) REFERENCES `lt_currency` (`id`),
  CONSTRAINT `order_item_price_currency_fk` FOREIGN KEY (`price_currency`) REFERENCES `lt_currency` (`id`),
  CONSTRAINT `order_item_taxedtotal_currency_fk` FOREIGN KEY (`taxedtotal_currency`) REFERENCES `lt_currency` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


CREATE TABLE `lt_order_item_source_link` (
  `id` varchar(32) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id` (`id`),
  CONSTRAINT `FKFBFB9E64C5454921` FOREIGN KEY (`id`) REFERENCES `lt_order_item` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `lt_order_item_avia_link` (
  `id` varchar(32) NOT NULL,
  `linktype` int(11) NOT NULL,
  `document` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id` (`id`),
  KEY `document` (`document`),
  CONSTRAINT `orderitemavialink_document_fk` FOREIGN KEY (`document`) REFERENCES `lt_avia_document` (`id`),
  CONSTRAINT `FK8B6135D8A33F687E` FOREIGN KEY (`id`) REFERENCES `lt_order_item_source_link` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


drop table lt_order_status_record;

CREATE TABLE `lt_order_status_record` (
  `id` varchar(32) NOT NULL,
  `status` int(11) NOT NULL,
  `timestamp` datetime NOT NULL,
  `changedby` varchar(32) NOT NULL,
  `order_` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `changedby` (`changedby`),
  KEY `order_` (`order_`),
  CONSTRAINT `orderstatusrecord_order_fk` FOREIGN KEY (`order_`) REFERENCES `lt_order` (`id`),
  CONSTRAINT `orderstatusrecord_changedby_fk` FOREIGN KEY (`changedby`) REFERENCES `lt_person` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


drop table lt_payment;


CREATE TABLE `lt_payment` (
  `id` varchar(32) NOT NULL,
  `class` varchar(20) NOT NULL,
  `version` int(11) NOT NULL,
  `createdby` varchar(32) NOT NULL,
  `createdon` datetime NOT NULL,
  `modifiedby` varchar(32) DEFAULT NULL,
  `modifiedon` datetime DEFAULT NULL,
  `number_` varchar(20) NOT NULL,
  `documentnumber` varchar(20) DEFAULT NULL,
  `documentuniquecode` varchar(30) DEFAULT NULL,
  `paymentform` int(11) NOT NULL,
  `receivedfrom` varchar(200) DEFAULT NULL,
  `note` text,
  `isvoid` tinyint(1) NOT NULL,
  `date_` datetime NOT NULL,
  `postedon` datetime DEFAULT NULL,
  `printeddocument` longblob,
  `payer` varchar(32) NOT NULL,
  `registeredby` varchar(32) DEFAULT NULL,
  `order_` varchar(32) DEFAULT NULL,
  `invoice` varchar(32) DEFAULT NULL,
  `owner` varchar(32) DEFAULT NULL,
  `amount_amount` decimal(19,5) DEFAULT NULL,
  `amount_currency` varchar(32) DEFAULT NULL,
  `vat_amount` decimal(19,5) DEFAULT NULL,
  `vat_currency` varchar(32) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `documentuniquecode` (`documentuniquecode`),
  KEY `payer` (`payer`),
  KEY `registeredby` (`registeredby`),
  KEY `order_` (`order_`),
  KEY `invoice` (`invoice`),
  KEY `owner` (`owner`),
  KEY `amount_currency` (`amount_currency`),
  KEY `vat_currency` (`vat_currency`),
  CONSTRAINT `payment_vat_currency_fk` FOREIGN KEY (`vat_currency`) REFERENCES `lt_currency` (`id`),
  CONSTRAINT `payment_amount_currency_fk` FOREIGN KEY (`amount_currency`) REFERENCES `lt_currency` (`id`),
  CONSTRAINT `payment_invoice_fk` FOREIGN KEY (`invoice`) REFERENCES `lt_invoice` (`id`),
  CONSTRAINT `payment_order_fk` FOREIGN KEY (`order_`) REFERENCES `lt_order` (`id`),
  CONSTRAINT `payment_owner_fk` FOREIGN KEY (`owner`) REFERENCES `lt_party` (`id`),
  CONSTRAINT `payment_payer_fk` FOREIGN KEY (`payer`) REFERENCES `lt_party` (`id`),
  CONSTRAINT `payment_registeredby_fk` FOREIGN KEY (`registeredby`) REFERENCES `lt_person` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



alter table lt_system_configuration
	change aviainvoiceitemgenerationoption `aviaorderitemgenerationoption` int(11) NOT NULL,
	change allowagentsetinvoicevat `allowagentsetordervat` tinyint(1) NOT NULL,
	drop column allowagenteditdictionaries;


alter table lt_avia_document_fee
drop FOREIGN KEY aviadocument_fees_fk;

ALTER TABLE `lt_avia_document_fee` ADD CONSTRAINT `aviadocumentfee_aviadocument_fk`
	FOREIGN KEY ( `document` ) REFERENCES `lt_avia_document` ( `id` );
	
alter table lt_avia_document_voiding
drop FOREIGN key aviadocument_voidings_fk;

ALTER TABLE `lt_avia_document_voiding` ADD CONSTRAINT `aviadocumentvoiding_aviadocument_fk`
	FOREIGN KEY ( `document` ) REFERENCES `lt_avia_document` ( `id` );

alter table lt_flight_segment
drop FOREIGN key aviaticket_segments_fk;

ALTER TABLE `lt_flight_segment` ADD CONSTRAINT `flightsegment_aviaticket_fk`
	FOREIGN KEY ( `ticket` ) REFERENCES `lt_avia_ticket` ( `id` );

ALTER TABLE `lt_miles_card` 
drop FOREIGN KEY `person_milescards_fk`;

ALTER TABLE `lt_miles_card` ADD CONSTRAINT `milescard_person_fk`
	FOREIGN KEY ( `owner` ) REFERENCES `lt_person` ( `id` );

alter table lt_passport
 drop FOREIGN KEY person_passports_fk;
 
ALTER TABLE `lt_passport` ADD CONSTRAINT `passport_person_fk`
	FOREIGN KEY ( `owner` ) REFERENCES `lt_person` ( `id` );
 
 
ALTER TABLE `lt_penalize_operation` 
drop FOREIGN KEY `aviaticket_penalizeoperations_fk`;

ALTER TABLE `lt_penalize_operation` ADD CONSTRAINT `penalizeoperation_aviaticket_fk`
	FOREIGN KEY ( `ticket` ) REFERENCES `lt_avia_ticket` ( `id` );

ALTER TABLE `lt_task_comment` 
drop FOREIGN KEY `task_comments_fk`;

ALTER TABLE `lt_task_comment` ADD CONSTRAINT `taskcomment_task_fk`
	FOREIGN KEY ( `task` ) REFERENCES `lt_task` ( `id` );

update lt_avia_document doc
	inner join lt_order_item_avia_link link on link.document = doc.id
	inner join lt_order_item item on item.id = link.id
	inner join lt_order o on o.id = item.order_ and o.status <> 2
set doc.order_ = o.id;

ALTER TABLE `lt_department` 
drop FOREIGN KEY `organization_departments_fk`;
	
ALTER TABLE `lt_department` ADD CONSTRAINT `department_organization_fk`
	FOREIGN KEY ( `organization` ) REFERENCES `lt_organization` ( `id` );
	
ALTER TABLE `lt_file` drop  FOREIGN KEY `party_files_fk`;

ALTER TABLE `lt_file` ADD CONSTRAINT `file_party_fk`
	FOREIGN KEY ( `party` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_issued_consignment` drop FOREIGN KEY `consignment_issuedconsignments_fk`;

ALTER TABLE `lt_issued_consignment` ADD CONSTRAINT `issuedconsignment_consignment_fk`
	FOREIGN KEY ( `consignment` ) REFERENCES `lt_consignment` ( `id` );

ALTER TABLE `lt_modification_item` drop FOREIGN KEY `modification_items_fk`;

ALTER TABLE `lt_modification_item` ADD CONSTRAINT `modificationitem_modification_fk`
	FOREIGN KEY ( `modification` ) REFERENCES `lt_modification` ( `id` );

alter table lt_order
	drop column `type`,
	drop column `agreement`;

ALTER TABLE `lt_preferences` drop FOREIGN KEY `identity_preferences_fk`;


ALTER TABLE `lt_preferences` ADD CONSTRAINT `preferences_identity_fk`
	FOREIGN KEY ( `identity` ) REFERENCES `lt_identity` ( `id` );


update lt_sequence
set name  = 'CashOrderPayment', format = 'C.{0:yy}-{1:00000}'
where id = 3;

INSERT INTO lt_sequence (id, name, format, timestamp, current) VALUES (4, 'Consignment', '{1:00000}', curdate(), 0);
INSERT INTO lt_sequence (id, name, format, timestamp, current) VALUES (5, 'Order', 'O.{0:yy}-{1:00000}', curdate(), 0);
INSERT INTO lt_sequence (id, name, format, timestamp, current) VALUES (6, 'Receipt', 'R.{0:yy}-{1:00000}', curdate(), 0);
