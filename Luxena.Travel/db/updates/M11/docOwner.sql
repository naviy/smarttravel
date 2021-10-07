alter table lt_gds_agent
change office officecode varchar(20) NOT NULL;

alter table lt_gds_file
change office officecode varchar(20) DEFAULT NULL;

alter table lt_avia_document
	add column `owner` varchar(32) default NULL after `seller`,
	add KEY `owner` ( `owner` );

alter table lt_gds_agent
	add column `office` varchar(32) default NULL after person,
	add KEY `office` ( `office` );
	
ALTER TABLE `lt_gds_agent` ADD CONSTRAINT `gdsagent_office_fk`
	FOREIGN KEY ( `office` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE `lt_avia_document` ADD CONSTRAINT `aviadocument_owner_fk`
	FOREIGN KEY ( `owner` ) REFERENCES `lt_party` ( `id` );

CREATE TABLE `lt_avia_document_owner` (
	`id` varchar(32) NOT NULL,
	KEY `id` ( `id` ),
	PRIMARY KEY  ( `id` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

ALTER TABLE `lt_avia_document_owner` ADD CONSTRAINT `aviadocumentowner_party_fk`
	FOREIGN KEY ( `id` ) REFERENCES `lt_party` ( `id` );
	
update lt_gds_agent
	set office = 'f2e54b481b334eeb97f980924e4cf7f9'
where officecode = 'BSV.Artema' or officecode = 'IEVU23561' or officecode = '7Y5X';

update lt_gds_agent
	set office = '462fdfb3d7ba4b958a2e9354b8775e97'
where office is null;



 
