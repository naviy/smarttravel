alter table lt_avia_document
add column ispaid tinyint(1) default null after isvoid;

update lt_avia_document as doc
set ispaid = 0;

alter table lt_avia_document
modify ispaid tinyint(1) not null ;

update lt_avia_document as doc
set doc.ispaid = 1
where exists (select link.document
								from lt_invoice_item_avia_link as link
									inner join lt_invoice_item_source_link as source on link.id = source.id
									inner join lt_invoice_item as item on source.id = item.id
									inner join lt_invoice as i on item.invoice = i.id
									where i.status = 1 and i.paymenttimestamp is not null and link.document = doc.id)
;


alter table lt_invoice
add column printedinvoicescounter int(11) after vat_amount;

alter table lt_printed_invoice
add column number_ varchar(255) default NULL after id;

alter table lt_printed_invoice
add column issuedby varchar(32) default NULL after invoice;

ALTER TABLE lt_printed_invoice ADD CONSTRAINT FK_lt_printed_invoice_issuedby
	FOREIGN KEY ( issuedby ) REFERENCES lt_person ( id );

alter table lt_payment
add column vat_currency varchar(32) default NULL after amount_amount;

alter table lt_payment
add column vat_amount decimal(19,5) default NULL after vat_currency;

ALTER TABLE lt_payment ADD CONSTRAINT payment_vat_currency_fk
	FOREIGN KEY ( vat_currency ) REFERENCES `lt_currency` ( id );

CREATE TABLE lt_void_item (
	id varchar(32) NOT NULL,
	voidedby varchar(32) default NULL,
	voided tinyint(1) default NULL,
	invoice varchar(32) default NULL,
	KEY invoice ( invoice ),
	PRIMARY KEY  ( id ),
	KEY voidedby ( voidedby )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;
 
ALTER TABLE lt_void_item ADD CONSTRAINT void_item_invoice_fk
	FOREIGN KEY ( invoice ) REFERENCES lt_invoice ( id );

ALTER TABLE lt_void_item ADD CONSTRAINT void_item_voidedby_fk
	FOREIGN KEY ( voidedby ) REFERENCES lt_person ( id );

 
