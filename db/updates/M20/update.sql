alter table lt_person add column organization varchar(32) DEFAULT NULL;
alter table lt_person add column title varchar(100) DEFAULT NULL;

alter table lt_person
add key `organization` ( `organization` );

ALTER TABLE `lt_person` ADD CONSTRAINT `person_organization_fk` FOREIGN KEY ( `organization` ) REFERENCES `lt_organization` ( `id` );

 

