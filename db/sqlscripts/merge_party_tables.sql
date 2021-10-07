begin work;


alter table lt_party
    add column "class" varchar(20),
    add column organization character varying(32),

    add column birthday date,
    add column milescardsstring citext2,
    add column title citext2,
    add column bonuscardnumber citext2,

    add column code citext2,
    add column isprovider boolean not null default false,
    add column isaccommodationprovider boolean not null default false,
    add column isbusticketprovider boolean not null default false,
    add column iscarrentalprovider boolean not null default false,
    add column isinsuranceprovider boolean not null default false,
    add column ispasteboardprovider boolean not null default false,
    add column istourprovider boolean not null default false,
    add column istransferprovider boolean not null default false,
    add column isairline boolean not null default false,
    add column airlineiatacode citext2,
    add column airlineprefixcode citext2,
    add column airlinepassportrequirement integer not null default 0,
    add column isinsurancecompany boolean not null default false,
    add column isroamingoperator boolean not null default false,


    add constraint "FK_lt_party_organization_lt_party_id" 
        foreign key (organization) references lt_party (id)

;

update lt_party p set
    class = 'Department',
    organization = d.organization
  from lt_department d
 where p.id = d.id
;


update lt_party p set
    class = 'Person',
    organization = d.organization,
    birthday = d.birthday,
    milescardsstring = d.milescardsstring,
    title = d.title,
    bonuscardnumber = d.bonuscardnumber
  from lt_person d
 where p.id = d.id
;


update lt_party p set
    class = 'Organization',
    code = d.code,
    isprovider = d.isprovider,
    isaccommodationprovider = d.isaccommodationprovider,
    isbusticketprovider = d.isbusticketprovider,
    iscarrentalprovider = d.iscarrentalprovider,
    isinsuranceprovider = d.isinsuranceprovider,
    ispasteboardprovider = d.ispasteboardprovider,
    istourprovider = d.istourprovider,
    istransferprovider = d.istransferprovider,
    isairline = d.isairline,
    airlineiatacode = d.airlineiatacode,
    airlineprefixcode = d.airlineprefixcode,
    airlinepassportrequirement = d.airlinepassportrequirement,
    isinsurancecompany = d.isinsurancecompany,
    isroamingoperator = d.isroamingoperator
  from lt_organization d
 where p.id = d.id
;

create index "IX_lt_party_class" on lt_party (class);
create index "IX_lt_party_organization" on lt_party (organization);
create index "IX_lt_party_airlineiatacode" on lt_party (airlineiatacode);
create index "IX_lt_party_airlineprefixcode" on lt_party (airlineprefixcode);


alter table lt_avia_document_voiding
    drop constraint aviadocumentvoiding_agent_fkey,
    add constraint "FK_lt_avia_document_voiding_agent_lt_party_id"
        foreign key (agent) references lt_party (id);

alter table lt_document_access
    drop constraint documentaccess_person_fkey,
    add constraint "FK_lt_document_access_person_lt_party_id"
        foreign key (person) references lt_party (id);

alter table lt_file
    drop constraint file_uploadedby_fkey,
    add constraint "FK_lt_file_uploadedby_lt_party_id"
        foreign key (uploadedby) references lt_party (id);

alter table lt_gds_agent
    drop constraint gdsagent_person_fkey,
    add constraint "FK_lt_gds_agent_person_lt_party_id"
        foreign key (person) references lt_party (id);

alter table lt_invoice
    drop constraint invoice_issuedby_fkey,
    add constraint "FK_lt_invoice_issuedby_lt_party_id"
        foreign key (issuedby) references lt_party (id);

alter table lt_issued_consignment
    drop constraint issuedconsignment_issuedby_fkey,
    add constraint "FK_lt_issued_consignment_issuedby_lt_party_id"
        foreign key (issuedby) references lt_party (id);

alter table lt_miles_card
    drop constraint milescard_owner_fkey,
    add constraint "FK_lt_miles_card_owner_lt_party_id"
        foreign key (owner) references lt_party (id);

alter table lt_order
    drop constraint order_assignedto_fkey,
    add constraint "FK_lt_order_assignedto_lt_party_id"
        foreign key (assignedto) references lt_party (id);

alter table lt_passport
    drop constraint passport_owner_fkey,
    add constraint "FK_lt_passport_owner_lt_party_id"
        foreign key (owner) references lt_party (id);

alter table lt_payment
    drop constraint payment_assignedto_fkey,
    add constraint "FK_lt_payment_assignedto_lt_party_id"
        foreign key (assignedto) references lt_party (id);

alter table lt_payment
    drop constraint payment_registeredby_fkey,
    add constraint "FK_lt_payment_registeredby_lt_party_id"
        foreign key (registeredby) references lt_party (id);

alter table lt_product_passenger
    drop constraint product_passenger_passenger_fkey,
    add constraint "FK_lt_product_passenger__lt_party_id"
        foreign key (passenger) references lt_party (id);

alter table lt_product
    drop constraint product_seller_fkey,
    add constraint "FK_lt_product_seller_lt_party_id"
        foreign key (seller) references lt_party (id);

alter table lt_system_configuration
    drop constraint systemconfiguration_birthdaytaskresponsible_fkey,
    add constraint "FK_lt_system_configuration_birthdaytaskresponsible_lt_party_id"
        foreign key (birthdaytaskresponsible) references lt_party (id);

alter table lt_user
    drop constraint user_person_fkey,
    add constraint "FK_lt_user_person_lt_party_id"
        foreign key (person) references lt_party (id);

alter table lt_product
    drop constraint "FK_lt_product_booker_lt_person_id",
    add constraint "FK_lt_product_booker_lt_party_id"
        foreign key (booker) references lt_party (id);

alter table lt_product
    drop constraint "FK_lt_product_ticketer_lt_person_id",
    add constraint "FK_lt_product_ticketer_lt_party_id"
        foreign key (ticketer) references lt_party (id);

alter table lt_miles_card
    drop constraint milescard_organization_fkey,
    add constraint "FK_lt_miles_card_organization_lt_party_id"
        foreign key (organization) references lt_party (id);

alter table lt_system_configuration
    drop constraint systemconfiguration_company_fkey,
    add constraint "FK_lt_system_configuration_company_lt_party_id"
        foreign key (company) references lt_party (id);

alter table lt_product
    drop constraint fk_lt_product_provider_lt_organization_id,
    add constraint "FK_lt_product_provider_lt_party_id"
        foreign key (provider) references lt_party (id);

alter table lt_airline_commission_percents
    drop constraint "FK_lt_airline_commission_percents_airline_lt_organization_id",
    add constraint "FK_lt_airline_commission_percents_airline_lt_party_id"
        foreign key (airline) references lt_party (id);

alter table lt_airline_service_class
    drop constraint "FK_lt_airline_service_class_airline_lt_organization_id",
    add constraint "FK_lt_airline_service_class_airline_lt_party_id"
        foreign key (airline) references lt_party (id);

alter table lt_flight_segment
    drop constraint "FK_lt_flight_segment_carrier_lt_organization_id",
    add constraint "FK_lt_flight_segment_carrier_lt_party_id"
        foreign key (carrier) references lt_party (id);

alter table lt_product
    drop constraint "FK_lt_product_producer_lt_organization_id",
    add constraint "FK_lt_product_producer_lt_party_id"
        foreign key (producer) references lt_party (id);


drop view olap_airline_dim;
drop view olap_carrier_dim;

drop table lt_department;
drop table lt_person;
drop table lt_organization;

alter table lt_party alter column class set not null;

rollback;