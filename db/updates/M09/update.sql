alter table lt_invoice
	change acquirer acquirer varchar(32) NOT NULL,
	add `grandtotal_currency` varchar(32) default NULL after discount_amount,
	add `grandtotal_amount` decimal(19,5) default NULL after grandtotal_currency,
	add KEY `grandtotal_currency` ( `grandtotal_currency` ),
	add CONSTRAINT `invoice_grandtotal_currency_fk` FOREIGN KEY ( `grandtotal_currency` ) REFERENCES `lt_currency` ( `id` ),
	add UNIQUE INDEX `number_`(`number_`),
	drop foreign key `invoice_servicefee_currency_fk`,
	drop key `servicefee_currency`;

update lt_invoice as t
	set t.grandtotal_currency = t.total_currency,
			t.grandtotal_amount = t.total_amount + ifnull(t.servicefee_amount, 0) - ifnull(t.discount_amount, 0);

update lt_invoice as t
	set t.total_amount = t.total_amount + ifnull(t.servicefee_amount, 0) - ifnull(t.vat_amount, 0);
	
alter table lt_invoice
	drop servicefee_currency,
	drop servicefee_amount;

alter table lt_invoice_item
	change `text` `text` text NOT NULL,
	add `price_currency` varchar(32) default NULL after quantity,
	add	`price_amount` decimal(19,5) default NULL after price_currency,	
	add `position` int(11) default NULL,
	drop FOREIGN KEY invoice_item_avia_document_fk,
	drop FOREIGN KEY  `invoice_item_total_currency_fk`,
	drop key ticket,
	drop key `total_currency`;

update lt_invoice_item
	set position = 0;
	
alter table lt_invoice_item
	change `position` `position` int(11) not NULL;

alter table lt_invoice_item
	change ticket `document` varchar(32) default NULL,
	add KEY `document` ( `document` ),
	add KEY `price_currency` ( `price_currency` ),
	ADD CONSTRAINT `invoice_item_price_currency_fk` FOREIGN KEY ( `price_currency` ) REFERENCES `lt_currency` ( `id` ),
	ADD CONSTRAINT `invoice_item_avia_document_fk`	FOREIGN KEY ( `document` ) REFERENCES `lt_avia_document` ( `id` );

update lt_invoice_item as t
	set t.price_amount = t.total_amount + ifnull(t.servicefee_amount,0) - ifnull(t.vat_amount, 0),
			t.price_currency = t.total_currency;

update lt_invoice_item as t
	set t.servicefee_amount = t.servicefee_amount - ifnull(t.vat_amount, 0);
		
alter table lt_invoice_item
	drop total_amount,
	drop total_currency;

