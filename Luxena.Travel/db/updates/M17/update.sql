alter table lt_airline add passportrequirement int(11) not null;



Alter table lt_avia_document change passenger passengername varchar(100) default null;
alter table lt_avia_document add `passenger` varchar(32) DEFAULT NULL;
alter table lt_avia_document add `gdspassportstatus` int(11) DEFAULT 1;
alter table lt_avia_document add `gdspassport` varchar(150) DEFAULT NULL;
 
alter table `lt_avia_document`
drop key `passenger`;

alter table `lt_avia_document`
add KEY `passenger` ( `passenger` );

alter table `lt_avia_document`
add KEY `passengername` ( `passengername`, `number_` );

ALTER TABLE `lt_avia_document` ADD CONSTRAINT `aviadocument_passenger_fk`
 FOREIGN KEY ( `passenger` ) REFERENCES `lt_person` ( `id` );
 

alter table lt_passport drop index number_; 
alter table lt_passport add unique key (number_, issuedby);


ALTER TABLE lt_printed_invoice drop FOREIGN KEY `FK_lt_printed_invoice_issuedby`;

alter table lt_printed_invoice drop key FK_lt_printed_invoice_issuedby;

alter table `lt_printed_invoice` add KEY `issuedby` ( `issuedby` );

ALTER TABLE `lt_printed_invoice` ADD CONSTRAINT `printedinvoice_issuedby_fk`
FOREIGN KEY ( `issuedby` ) REFERENCES `lt_person` ( `id` );

ALTER TABLE `lt_printed_invoice` DROP FOREIGN KEY `FK_lt_printed_invoice_invoice`;
ALTER TABLE `lt_printed_invoice` ADD CONSTRAINT `printedinvoice_invoice_fk`
FOREIGN KEY ( `invoice` ) REFERENCES `lt_invoice` ( `id` );
 


ALTER TABLE `lt_file`
DROP FOREIGN KEY `FK_lt_file_party`,
DROP FOREIGN KEY `FK_lt_file_uploadedby`;

ALTER TABLE `lt_file` ADD CONSTRAINT `file_party_fk`
FOREIGN KEY ( `party` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_file` ADD CONSTRAINT `file_uploadedby_fk`
FOREIGN KEY ( `uploadedby` ) REFERENCES `lt_person` ( `id` );



alter table `lt_system_configuration`
add `ispassengerpassportrequired` tinyint(1) NOT NULL after country;

alter table lt_payment
drop FOREIGN key payment_owner_fk,
drop FOREIGN key payment_vat_currency_fk;

alter table lt_payment
DROP key payment_owner_fk;

alter table lt_payment
DROP key payment_vat_currency_fk;

ALTER TABLE `lt_payment`
drop FOREIGN KEY invoice_payment;

ALTER TABLE `lt_payment`
drop FOREIGN KEY payment_registeredby;

ALTER TABLE `lt_payment`
add KEY `owner` ( `owner` ),
add KEY `vat_currency` ( `vat_currency` );

ALTER TABLE `lt_payment` ADD CONSTRAINT `payment_invoice_fk`
FOREIGN KEY ( `invoice` ) REFERENCES `lt_invoice` ( `id` );

ALTER TABLE `lt_payment` ADD CONSTRAINT `payment_owner_fk`
FOREIGN KEY ( `owner` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_payment` ADD CONSTRAINT `payment_registeredby_fk`
FOREIGN KEY ( `registeredby` ) REFERENCES `lt_person` ( `id` );

ALTER TABLE `lt_payment` ADD CONSTRAINT `payment_vat_currency_fk`
FOREIGN KEY ( `vat_currency` ) REFERENCES `lt_currency` ( `id` );



 

