CREATE TABLE `lt_finance_obligation` (
	`id` varchar(32) NOT NULL,
	`version` int(11) NOT NULL,
	`createdby` varchar(32) NOT NULL,
	`createdon` datetime NOT NULL,
	`modifiedby` varchar(32) default NULL,
	`modifiedon` datetime default NULL,
	`type` int(11) NOT NULL,
	`acquirer` varchar(32) NOT NULL,
	`number_` varchar(20) NOT NULL,
	`issuedate` datetime NOT NULL,
	`signedby` varchar(32) default NULL,
	`paymenttimestamp` datetime default NULL,
	`status` int(11) NOT NULL,
	`discount_currency` varchar(32) default NULL,
	`discount_amount` decimal(19,5) default NULL,
	`grandtotal_currency` varchar(32) default NULL,
	`grandtotal_amount` decimal(19,5) default NULL,
	`includedvat_currency` varchar(32) default NULL,
	`includedvat_amount` decimal(19,5) default NULL,
	KEY `acquirer` ( `acquirer` ),
	KEY `discount_currency` ( `discount_currency` ),
	KEY `grandtotal_currency` ( `grandtotal_currency` ),
	KEY `includedvat_currency` ( `includedvat_currency` ),
	UNIQUE INDEX `number_` ( `number_` ),
	PRIMARY KEY  ( `id` ),
	KEY `signedby` ( `signedby` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

/* Header line. Object: lt_payment. Script date: 10.06.2010 17:00:30. */
CREATE TABLE `lt_payment` (
	`id` varchar(32) NOT NULL,
	`version` int(11) NOT NULL,
	`createdby` varchar(32) NOT NULL,
	`createdon` datetime NOT NULL,
	`modifiedby` varchar(32) default NULL,
	`modifiedon` datetime default NULL,
	`whencharged` datetime NOT NULL,
	`number_` varchar(20) NOT NULL,
	`payer` varchar(32) NOT NULL,
	`amount_currency` varchar(32) default NULL,
	`amount_amount` decimal(19,5) default NULL,
	`paymentform` int(11) NOT NULL,
	`registeredby` varchar(32) NOT NULL,
	`receivedfrom` varchar(200) default NULL,
	`note` text default NULL,
	`isvoid` tinyint(1) NOT NULL,
	KEY `amount_currency` ( `amount_currency` ),
	KEY `payer` ( `payer` ),
	PRIMARY KEY  ( `id` ),
	KEY `registeredby` ( `registeredby` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

/* Header line. Object: lt_payment_finance_obligation_link. Script date: 10.06.2010 17:00:30. */
CREATE TABLE `lt_payment_finance_obligation_link` (
	`id` varchar(32) NOT NULL,
	`version` int(11) NOT NULL,
	`createdby` varchar(32) NOT NULL,
	`createdon` datetime NOT NULL,
	`modifiedby` varchar(32) default NULL,
	`modifiedon` datetime default NULL,
	`payment` varchar(32) NOT NULL,
	`financeobligation` varchar(32) NOT NULL,
	KEY `financeobligation` ( `financeobligation` ),
	KEY `payment` ( `payment` ),
	PRIMARY KEY  ( `id` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

-- Update foreign keys of lt_finance_obligation
ALTER TABLE `lt_finance_obligation` ADD CONSTRAINT `financeobligation_acquirer_fk`
	FOREIGN KEY ( `acquirer` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_finance_obligation` ADD CONSTRAINT `financeobligation_discount_currency_fk`
	FOREIGN KEY ( `discount_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_finance_obligation` ADD CONSTRAINT `financeobligation_grandtotal_currency_fk`
	FOREIGN KEY ( `grandtotal_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_finance_obligation` ADD CONSTRAINT `financeobligation_includedvat_currency_fk`
	FOREIGN KEY ( `includedvat_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_finance_obligation` ADD CONSTRAINT `financeobligation_signedby_fk`
	FOREIGN KEY ( `signedby` ) REFERENCES `lt_person` ( `id` );

-- Update foreign keys of lt_payment
ALTER TABLE `lt_payment` ADD CONSTRAINT `payment_amount_currency_fk`
	FOREIGN KEY ( `amount_currency` ) REFERENCES `lt_currency` ( `id` );

ALTER TABLE `lt_payment` ADD CONSTRAINT `payment_payer`
	FOREIGN KEY ( `payer` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_payment` ADD CONSTRAINT `payment_registeredby`
	FOREIGN KEY ( `registeredby` ) REFERENCES `lt_person` ( `id` );

-- Update foreign keys of lt_payment_finance_obligation_link
ALTER TABLE `lt_payment_finance_obligation_link` ADD CONSTRAINT `paymentfinanceobligationlink_invoice`
	FOREIGN KEY ( `financeobligation` ) REFERENCES `lt_finance_obligation` ( `id` );

ALTER TABLE `lt_payment_finance_obligation_link` ADD CONSTRAINT `paymentinvoicelink_payment`
	FOREIGN KEY ( `payment` ) REFERENCES `lt_payment` ( `id` );

INSERT INTO lt_finance_obligation
(id, version, createdby, createdon, modifiedby, modifiedon, acquirer, status, signedby, type,
	number_, issuedate, paymenttimestamp, discount_currency, discount_amount, grandtotal_currency, grandtotal_amount, includedvat_currency, includedvat_amount) 
SELECT id, version, createdby, createdon, modifiedby, modifiedon, acquirer, status, signedby, 0,
	number_, issuedate,  paymenttimestamp, discount_currency, discount_amount, grandtotal_currency, grandtotal_amount, vat_currency, vat_amount 
FROM lt_invoice;


ALTER TABLE `lt_invoice` DROP FOREIGN KEY `invoice_discount_currency_fk`;
ALTER TABLE `lt_invoice` DROP FOREIGN KEY `invoice_grandtotal_currency_fk`;
ALTER TABLE `lt_invoice` DROP FOREIGN KEY `invoice_vat_currency_fk`;
ALTER TABLE `lt_invoice` DROP FOREIGN KEY `invoice_acquirer_fk`;
ALTER TABLE `lt_invoice` DROP FOREIGN KEY `invoice_signedby_fk`;

alter table lt_invoice
drop column version,
drop column createdby,
drop column createdon,
drop column modifiedby,
drop column modifiedon,
drop column acquirer,
drop column number_,
drop column issuedate,
drop column signedby,
drop column paymenttimestamp,
drop column status,
drop column discount_currency,
drop column discount_amount,
drop column grandtotal_currency,
drop column grandtotal_amount,
drop column vat_currency,
drop column vat_amount;

ALTER TABLE `lt_invoice`
ADD KEY `id` ( `id` ),
ADD CONSTRAINT `invoice_id_fk` FOREIGN KEY ( `id` ) REFERENCES `lt_finance_obligation` ( `id` );

alter TABLE `lt_penalize_operation` 
modify `description` varchar(128) default NULL;

INSERT INTO lt_sequence
(id, format, timestamp, current) 
VALUES ('Payment', 'P.{0:yy}-{1:00000}', curdate(), 0);

ALTER TABLE lt_user CHANGE isreporter iscashier tinyint(1) NOT NULL