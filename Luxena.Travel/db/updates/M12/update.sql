CREATE TABLE `lt_penalize_operation` (
	`id` varchar(32) NOT NULL,
	`version` int(11) NOT NULL,
	`createdby` varchar(32) NOT NULL,
	`createdon` datetime NOT NULL,
	`modifiedby` varchar(32) default NULL,
	`modifiedon` datetime default NULL,
	`ticket` varchar(32) default NULL,
	`type` int(11) default NULL,
	`status` int(11) default NULL,
	`description` varchar(100) default NULL,
	PRIMARY KEY  ( `id` ),
	KEY `ticket` ( `ticket` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

 
ALTER TABLE `lt_penalize_operation` ADD CONSTRAINT `penalizeoperation_ticket_fk`
	FOREIGN KEY ( `ticket` ) REFERENCES `lt_avia_ticket` ( `id` );

 
