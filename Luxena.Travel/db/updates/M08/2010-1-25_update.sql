begin;

INSERT INTO lt_sequence
(id, format, timestamp, current) 
VALUES ('Invoice', 'I.{0:yy}-{1:00000}', curdate(), 0);

CREATE TABLE `lt_invoice` (
	`id` varchar(32) NOT NULL,
	`version` int(11) NOT NULL,
	`createdby` varchar(32) NOT NULL,
	`createdon` datetime NOT NULL,
	`modifiedby` varchar(32) default NULL,
	`modifiedon` datetime default NULL,
	`issuedate` datetime NOT NULL,
	`number_` varchar(255) NOT NULL,
	`status` int(11) NOT NULL,
	`signedby` varchar(32) default NULL,
	`isaviacompanyandagencydatasplitted` tinyint(1) NOT NULL,
	`supplier` varchar(32) default NULL,
	`acquirer` varchar(32) default NULL,
	`payer` varchar(32) default NULL,
	`agreement` varchar(255) default NULL,
	`daystillexpiration` int(11) default NULL,
	`total_currency` varchar(32) default NULL,
	`total_amount` decimal(19,5) default NULL,
	`vat_currency` varchar(32) default NULL,
	`vat_amount` decimal(19,5) default NULL,
	`discount_currency` varchar(32) default NULL,
	`discount_amount` decimal(19,5) default NULL,
	`servicefee_currency` varchar(32) default NULL,
	`servicefee_amount` decimal(19,5) default NULL,
	KEY `acquirer` ( `acquirer` ),
	KEY `discount_currency` ( `discount_currency` ),
	KEY `payer` ( `payer` ),
	PRIMARY KEY  ( `id` ),
	KEY `servicefee_currency` ( `servicefee_currency` ),
	KEY `signedby` ( `signedby` ),
	KEY `supplier` ( `supplier` ),
	KEY `total_currency` ( `total_currency` ),
	KEY `vat_currency` ( `vat_currency` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

 
ALTER TABLE `lt_invoice` ADD CONSTRAINT `FK_lt_invoice_acquirer`
	FOREIGN KEY ( `acquirer` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_invoice` ADD CONSTRAINT `FK_lt_invoice_payer`
	FOREIGN KEY ( `payer` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_invoice` ADD CONSTRAINT `FK_lt_invoice_signedby`
	FOREIGN KEY ( `signedby` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_invoice` ADD CONSTRAINT `FK_lt_invoice_supplier`
	FOREIGN KEY ( `supplier` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_invoice` ADD CONSTRAINT `invoice_discount_currency_fk`
	FOREIGN KEY ( `discount_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_invoice` ADD CONSTRAINT `invoice_servicefee_currency_fk`
	FOREIGN KEY ( `servicefee_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_invoice` ADD CONSTRAINT `invoice_total_currency_fk`
	FOREIGN KEY ( `total_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_invoice` ADD CONSTRAINT `invoice_vat_currency_fk`
	FOREIGN KEY ( `vat_currency` ) REFERENCES `lt_currency` ( `id` );

 
CREATE TABLE `lt_invoice_item` (
	`id` varchar(32) NOT NULL,
	`version` int(11) NOT NULL,
	`createdby` varchar(32) NOT NULL,
	`createdon` datetime NOT NULL,
	`modifiedby` varchar(32) default NULL,
	`modifiedon` datetime default NULL,
	`text` varchar(255) NOT NULL,
	`quantity` int(11) NOT NULL,
	`total_currency` varchar(32) default NULL,
	`total_amount` decimal(19,5) default NULL,
	`vat_currency` varchar(32) default NULL,
	`vat_amount` decimal(19,5) default NULL,
	`discount_currency` varchar(32) default NULL,
	`discount_amount` decimal(19,5) default NULL,
	`servicefee_currency` varchar(32) default NULL,
	`servicefee_amount` decimal(19,5) default NULL,
	`ticket` varchar(32) default NULL,
	`invoice` varchar(32) default NULL,
	KEY `discount_currency` ( `discount_currency` ),
	KEY `invoice` ( `invoice` ),
	PRIMARY KEY  ( `id` ),
	KEY `servicefee_currency` ( `servicefee_currency` ),
	KEY `ticket` ( `ticket` ),
	KEY `total_currency` ( `total_currency` ),
	KEY `vat_currency` ( `vat_currency` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

 
ALTER TABLE `lt_invoice_item` ADD CONSTRAINT `invoice_item_avia_document_fk`
	FOREIGN KEY ( `ticket` ) REFERENCES `lt_avia_document` ( `id` );

ALTER TABLE `lt_invoice_item` ADD CONSTRAINT `invoice_item_discount_currency_fk`
	FOREIGN KEY ( `discount_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_invoice_item` ADD CONSTRAINT `invoice_item_invoice_fk`
	FOREIGN KEY ( `invoice` ) REFERENCES `lt_invoice` ( `id` );

ALTER TABLE `lt_invoice_item` ADD CONSTRAINT `invoice_item_servicefee_currency_fk`
	FOREIGN KEY ( `servicefee_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_invoice_item` ADD CONSTRAINT `invoice_item_total_currency_fk`
	FOREIGN KEY ( `total_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_invoice_item` ADD CONSTRAINT `invoice_item_vat_currency_fk`
	FOREIGN KEY ( `vat_currency` ) REFERENCES `lt_currency` ( `id` );

 
CREATE TABLE `lt_printed_invoice` (
	`id` varchar(32) NOT NULL,
	`timestamp` datetime NOT NULL,
	`content` longblob NOT NULL,
	`invoice` varchar(32) default NULL,
	KEY `invoice` ( `invoice` ),
	PRIMARY KEY  ( `id` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

 
ALTER TABLE `lt_printed_invoice` ADD CONSTRAINT `FK_lt_printed_invoice_invoice`
	FOREIGN KEY ( `invoice` ) REFERENCES `lt_invoice` ( `id` );

 
alter table lt_avia_document 
change number_ number_ bigint(20) default NULL;

alter table lt_avia_document
add displaystring  varchar(70) default NULL after note;

update lt_avia_document AS t
set displaystring = concat(lpad(t.airlineprefixcode, 3, 0),'-', t.number_)
where t.number_ is not null and t.airlineprefixcode is not null;

ALTER TABLE `lt_system_configuration` DROP FOREIGN KEY `configuration_birthdaytaskresposible`;

ALTER TABLE `lt_system_configuration` DROP FOREIGN KEY `configuration_defaultcurrency`;

/* Header line. Object: lt_system_configuration. Script date: 25.01.2010 12:21:34. */
DROP TABLE IF EXISTS `_temp_lt_system_configuration`;

CREATE TABLE `_temp_lt_system_configuration` (
	`id` varchar(32) NOT NULL,
	`version` int(11) NOT NULL,
	`company` varchar(32) default NULL,
	`companydetails` text default NULL,
	`defaultcurrency` varchar(32) default NULL,
	`birthdaytaskresposible` varchar(32) default NULL,
	`modifiedby` varchar(32) default NULL,
	`modifiedon` datetime default NULL,
	KEY `birthdaytaskresposible` ( `birthdaytaskresposible` ),
	KEY `company` ( `company` ),
	KEY `defaultcurrency` ( `defaultcurrency` ),
	PRIMARY KEY  ( `id` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

INSERT INTO `_temp_lt_system_configuration`
( `birthdaytaskresposible`, `defaultcurrency`, `id`, `modifiedby`, `modifiedon`, `version` )
SELECT
`birthdaytaskresposible`, `defaultcurrency`, `id`, `modifiedby`, `modifiedon`, `version`
FROM `lt_system_configuration`;

DROP TABLE `lt_system_configuration`;

ALTER TABLE `_temp_lt_system_configuration` RENAME `lt_system_configuration`;

-- Update foreign keys of lt_system_configuration
ALTER TABLE `lt_system_configuration` ADD CONSTRAINT `configuration_birthdaytaskresposible`
	FOREIGN KEY ( `birthdaytaskresposible` ) REFERENCES `lt_person` ( `id` );

ALTER TABLE `lt_system_configuration` ADD CONSTRAINT `configuration_company`
	FOREIGN KEY ( `company` ) REFERENCES `lt_organization` ( `id` );

ALTER TABLE `lt_system_configuration` ADD CONSTRAINT `configuration_defaultcurrency`
	FOREIGN KEY ( `defaultcurrency` ) REFERENCES `lt_currency` ( `id` );


commit;