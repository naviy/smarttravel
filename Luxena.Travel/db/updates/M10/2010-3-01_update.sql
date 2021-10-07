alter TABLE `lt_invoice` 
	add `paymenttimestamp` datetime default NULL after daystillexpiration;

ALTER TABLE `lt_airport` 
	DROP COLUMN `localcode`,
	add `localizedsettlement` varchar(200) default NULL after settlement;
