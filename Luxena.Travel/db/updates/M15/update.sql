CREATE TABLE lt_file (
	`id` varchar(32) NOT NULL,
	`filename` varchar(255) NOT NULL,
	`timestamp` datetime NOT NULL,
	`content` longblob NOT NULL,
	`uploadedby` varchar(32) default NULL,
	`party` varchar(32) default NULL,
	KEY `party` ( `party` ),
	PRIMARY KEY  ( `id` ),
	KEY `uploadedby` ( `uploadedby` )
)
ENGINE = InnoDB
CHARACTER SET = utf8
ROW_FORMAT = Compact
;

 
ALTER TABLE lt_file ADD CONSTRAINT `FK_lt_file_party`
	FOREIGN KEY ( `party` ) REFERENCES `lt_party` ( `id` );

ALTER TABLE lt_file ADD CONSTRAINT `FK_lt_file_uploadedby`
	FOREIGN KEY ( `uploadedby` ) REFERENCES `lt_person` ( `id` );



