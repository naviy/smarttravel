alter table lt_avia_document add column commissiondiscount_currency varchar(32) DEFAULT NULL;
alter table lt_avia_document add column commissiondiscount_amount decimal(19,5) DEFAULT NULL;

alter table lt_avia_document 
add  CONSTRAINT `aviadocument_commissiondiscount_currency_fk` FOREIGN KEY (`commissiondiscount_currency`) REFERENCES `lt_currency` (`id`);