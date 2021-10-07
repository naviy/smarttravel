alter TABLE `lt_party` 
	add `namealias` varchar(100) default NULL after name,
	add UNIQUE INDEX `namealias` ( `namealias` );

alter table `lt_system_configuration`
	add `usedefaultcurrencyforinput` tinyint(1) default NULL after defaultcurrency;

update lt_system_configuration
	set usedefaultcurrencyforinput = false;
	
alter table `lt_system_configuration`
	modify `usedefaultcurrencyforinput` tinyint(1) not NULL;
	
alter table `lt_system_configuration`
	modify `defaultcurrency` varchar(32) NOT NULL;

alter TABLE `lt_invoice` 
	add `paymenttimestamp` datetime default NULL after daystillexpiration;

ALTER TABLE `lt_airport` 
	DROP COLUMN `localcode`,
	add `localizedsettlement` varchar(200) default NULL after settlement;
