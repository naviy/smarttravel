alter table lt_avia_document 
  change bookercode bookercode VARCHAR(20),
  change ticketercode ticketercode VARCHAR(20);

alter table lt_currency
  add cyrilliccode VARCHAR(3) after code;
  
alter table lt_flight_segment
  change fromairport fromairport VARCHAR(32),
  change toairport toairport VARCHAR(32);
  

alter table lt_airline
add code1 varchar(3) after prefixcode; 

update lt_airline
set code1 = lpad(prefixcode, 3, 0);

alter table lt_airline
drop column prefixcode; 

alter table lt_airline
change code1 prefixcode varchar(3) default null;


alter table lt_avia_document
add code1 varchar(3) after airlineprefixcode; 

update lt_avia_document
set code1 = lpad(airlineprefixcode, 3, 0);

alter table lt_avia_document
drop column airlineprefixcode; 


alter table lt_avia_document
change code1 airlineprefixcode varchar(3) not null;

alter table lt_flight_segment
add code1 varchar(3) after carrierprefixcode; 

update lt_flight_segment
set code1 = lpad(carrierprefixcode, 3, 0);

alter table lt_flight_segment
drop column carrierprefixcode; 

alter table lt_flight_segment
change code1 carrierprefixcode varchar(3);

drop index airlineprefixcode on lt_avia_document;
create index airlineprefixcode on lt_avia_document (airlineprefixcode, number_) using btree;

alter table lt_avia_document_voiding
change agentcode agentcode varchar(20);

CREATE TABLE `lt_payment_system` (
	`id` varchar(32) NOT NULL,
	`version` int(11) NOT NULL,
	`createdby` varchar(32) NOT NULL,
	`createdon` datetime NOT NULL,
	`modifiedby` varchar(32) default NULL,
	`modifiedon` datetime default NULL,
	`name` varchar(100) NOT NULL,
	UNIQUE INDEX `name` ( `name` ),
	PRIMARY KEY  ( `id` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

alter table lt_payment
  add `authorizationcode` varchar(50) default NULL after vat_currency,
  add `paymentsystem` varchar(32) default NULL after authorizationcode;

alter table lt_airline
add UNIQUE INDEX `prefixcode` ( `prefixcode` );

alter table lt_currency
add  UNIQUE INDEX `cyrilliccode` ( `cyrilliccode` );

alter table lt_payment
add  KEY `paymentsystem` ( `paymentsystem` );

ALTER TABLE `lt_payment` ADD CONSTRAINT `electronicpayment_paymentsystem_fk`
 FOREIGN KEY ( `paymentsystem` ) REFERENCES `lt_payment_system` ( `id` );
 