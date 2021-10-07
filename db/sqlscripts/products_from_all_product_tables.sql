--begin work;

/*** AviaRefund ***/

alter table lt_product
    add column isrefund boolean not null default false,
    add column refundedproduct varchar(32),
    add column refundservicefee_amount numeric(19,5),
    add column refundservicefee_currency varchar(32),
    add column servicefeepenalty_amount numeric(19,5),
    add column servicefeepenalty_currency varchar(32),
    
    add constraint "FK_lt_product_refundedproduct_lt_product_id"
        foreign key (refundedproduct) references lt_product (id),
    add constraint "FK_lt_product_refundservicefee_currency_lt_currency_id"
        foreign key (refundservicefee_currency) references lt_currency (id),
    add constraint "FK_lt_product_servicefeepenalty_currency_lt_currency_id" 
        foreign key (servicefeepenalty_currency) references lt_currency (id)
;      

create index "IX_lt_product_refundedproduct" on lt_product (refundedproduct);
create index "IX_lt_product_refundservicefee_currency" on lt_product (refundservicefee_currency);
create index "IX_lt_product_servicefeepenalty_currency" on lt_product (servicefeepenalty_currency);


update lt_product p set
    isrefund = true,
    refundedproduct = r.refundeddocument,
    refundservicefee_amount = r.refundservicefee_amount,
    refundservicefee_currency = r.refundservicefee_currency,
    servicefeepenalty_amount = r.servicefeepenalty_amount,
    servicefeepenalty_currency = r.servicefeepenalty_currency
  from lt_avia_refund r
 where r.id = p.id;


/*** AviaDocument ***/

alter table lt_product
    add column airlineiatacode citext2,
    add column airlineprefixcode citext2,
    add column airlinename citext2,
    add column number_ citext2,
    add column conjunctionnumbers citext2,
    add column gdspassportstatus integer not null default (0),
    add column gdspassport citext2,
    add column itinerary citext2,
    add column paymentform citext2,
    add column bookeroffice citext2,
    add column bookercode citext2,
    add column ticketeroffice citext2,
    add column ticketercode citext2,
    add column originator integer not null default (0),
    add column origin integer not null default (0),
    add column airlinepnrcode citext2,
    add column remarks citext2,
    add column displaystring citext2,
    add column booker varchar(32),
    add column ticketer varchar(32),
    add column originaldocument varchar(32),
    add column isticketerrobot boolean,
    add column paymentdetails citext2,
    
    add constraint "FK_lt_product_booker_lt_person_id"
        foreign key (booker) references lt_person (id),
    add constraint "FK_lt_product_originaldocument_lt_gds_file_id"
        foreign key (originaldocument) references lt_gds_file (id),
    add constraint "FK_lt_product_ticketer_lt_person_id"
        foreign key (ticketer) references lt_person (id)
;

create index "IX_lt_product_number_" on lt_product (number_);
create index "IX_lt_product_booker" on lt_product (booker);
create index "IX_lt_product_originaldocument" on lt_product (originaldocument);
create index "IX_lt_product_ticketer" on lt_product (ticketer);


update lt_product p set
    airlineiatacode = d.airlineiatacode,
    airlineprefixcode = d.airlineprefixcode,
    airlinename = d.airlinename,
    number_ = d.number_,
    conjunctionnumbers = d.conjunctionnumbers,
    gdspassportstatus = d.gdspassportstatus,
    gdspassport = d.gdspassport,
    itinerary = d.itinerary,
    paymentform = d.paymentform,
    bookeroffice = d.bookeroffice,
    bookercode = d.bookercode,
    ticketeroffice = d.ticketeroffice,
    ticketercode = d.ticketercode,
    originator = d.originator,
    origin = d.origin,
    airlinepnrcode = d.airlinepnrcode,
    remarks = d.remarks,
    displaystring = d.displaystring,
    booker = d.booker,
    ticketer = d.ticketer,
    originaldocument = d.originaldocument,
    isticketerrobot = d.isticketerrobot,
    paymentdetails = d.paymentdetails
  from lt_avia_document d
 where p.id = d.id;

 
/*** AviaTicket ***/

alter table lt_product
    add column domestic boolean not null default false,
    add column interline boolean not null default false,
    add column segmentclasses citext2,
    add column departure timestamp,
    add column endorsement citext2,
    add column faretotal_amount numeric(19,5),
    add column faretotal_currency varchar(32),
    
    add constraint "FK_lt_product_faretotal_currency_lt_currency_id" 
        foreign key (faretotal_currency) references lt_currency (id)
;

create index "IX_lt_product_faretotal_currency" on lt_product (faretotal_currency);


update lt_product p set
    domestic = t.domestic,
    interline = t.interline,
    segmentclasses = t.segmentclasses,
    departure = t.departure,
    endorsement = t.endorsement,
    faretotal_amount = t.faretotal_amount,
    faretotal_currency = t.faretotal_currency
  from lt_avia_ticket t
 where p.id = t.id;


/*** AviaMco ***/

alter table lt_product
    add column description citext2,
    add column inconnectionwith varchar(32),
    add constraint "FK_lt_product_inconnectionwith_lt_product_id" 
        foreign key (inconnectionwith) references lt_product (id)
;

create index "IX_lt_product_inconnectionwith" on lt_product (inconnectionwith);

update lt_product p set
    description = m.description,
    inconnectionwith = m.inconnectionwith
  from lt_avia_mco m
 where m.id = p.id;



/*** AviaDocument details ***/

alter table lt_flight_segment
    drop constraint flightsegment_ticket_fkey,
    add constraint "FK_lt_flight_segment_ticket_lt_product_id"
        foreign key (ticket) references lt_product (id)
;
alter table lt_penalize_operation
    drop constraint penalizeoperation_ticket_fkey,
    add constraint "FK_lt_penalize_operation_ticket_lt_product_id"
        foreign key (ticket) references lt_product (id)
;
alter table lt_avia_document_fee
    drop constraint aviadocumentfee_document_fkey,
    add constraint "FK_lt_avia_document_fee_document_lt_product_id"
        foreign key (document) references lt_product (id)
;
alter table lt_avia_document_voiding
    drop constraint aviadocumentvoiding_document_fkey,
    add constraint "FK_lt_avia_document_voiding_document_lt_product_id"
        foreign key (document) references lt_product (id)
;


drop view olap_direction_dim cascade;
drop view olap_document cascade;
drop view olap_fare_segment_dim cascade;

drop view olap_ticketeroffice_dim cascade;
drop view olap_bookeroffice_dim cascade;
drop view olap_itinerary_dim cascade;
drop view olap_departuredate_dim cascade;
drop view olap_segment_dim cascade;
drop view olap_segmentclass_dim cascade;
drop view olap_tourcode_dim cascade;


drop table lt_avia_ticket;
drop table lt_avia_mco;
drop table lt_avia_refund;
drop table lt_avia_document;



/*** lt_accommodation ***/

alter table lt_product
    add column startdate timestamp,
    add column finishdate timestamp,
    add column hotelname citext2,
    add column hoteloffice citext2,
    add column hotelcode citext2,
    add column placementname citext2,
    add column placementoffice citext2,
    add column placementcode citext2,
    add column accommodationtype varchar(32),
    add column cateringtype varchar(32),

    add constraint "FK_lt_product_accommodationtype_lt_accommodation_type_id" 
        foreign key (accommodationtype) references lt_accommodation_type (id),
    add constraint "FK_lt_product_cateringtype_lt_catering_type_id" 
        foreign key (cateringtype) references lt_catering_type (id)    
;

create index "IX_lt_product_startdate" on lt_product (startdate);
create index "IX_lt_product_finishdate" on lt_product (finishdate);
create index "IX_lt_product_accommodationtype" on lt_product (accommodationtype);
create index "IX_lt_product_cateringtype" on lt_product (cateringtype);

update lt_product p set
    startdate = d.startdate,
    finishdate = d.finishdate,
    hotelname = d.hotelname,
    hoteloffice = d.hoteloffice,
    hotelcode = d.hotelcode,
    placementname = d.placementname,
    placementoffice = d.placementoffice,
    placementcode = d.placementcode,
    accommodationtype = d.accommodationtype,
    cateringtype = d.cateringtype
  from lt_accommodation d
 where p.id = d.id;

drop table lt_accommodation;


/*** lt_bus_ticket ***/

alter table lt_product
    add column departureplace citext2,
    add column departuredate timestamp,
    add column departuretime citext2,
    add column arrivalplace citext2,
    add column arrivaldate timestamp,
    add column arrivaltime citext2,
    add column seatnumber citext2
;

update lt_product p set
    departureplace = d.departureplace,
    departuredate = d.departuredate,
    departuretime = d.departuretime,
    arrivalplace = d.arrivalplace,
    arrivaldate = d.arrivaldate,
    arrivaltime = d.arrivaltime,
    seatnumber = d.seatnumber
  from lt_bus_ticket d
 where p.id = d.id;

drop table lt_bus_ticket;


/*** lt_car_rental ***/

alter table lt_product
    add column carbrand citext2
;

update lt_product p set
    startdate = d.startdate,
    finishdate = d.finishdate,
    carbrand = d.carbrand
  from lt_car_rental d
 where p.id = d.id;

drop table lt_car_rental;


/*** lt_excursion ***/

alter table lt_product
    add column tourname citext2
;

update lt_product p set
    startdate = d.startdate,
    finishdate = d.finishdate,
    tourname = d.tourname
  from lt_excursion d
 where p.id = d.id;

drop table lt_excursion;


/*** lt_generic_product ***/

alter table lt_product
    add column generictype varchar(32),

    add constraint "FK_lt_product_generictype_lt_generic_product_type_id" 
        foreign key (generictype) references lt_generic_product_type (id)
;

create index "IX_lt_product_generictype" on lt_product (generictype);

update lt_product p set
    generictype = d.generictype,
    number_ = d.number_,
    startdate = d.startdate,
    finishdate = d.finishdate
  from lt_generic_product d
 where p.id = d.id;

drop table lt_generic_product;


/*** lt_insurance ***/

update lt_product p set
    startdate = d.startdate,
    finishdate = d.finishdate,
    number_ = d.number_
  from lt_insurance d
 where p.id = d.id;

drop table lt_insurance;


/*** lt_isic ***/

alter table lt_product
    add column cardtype int not null default 0,
    add column number1 citext2,
    add column number2 citext2
;

update lt_product p set
    cardtype = d.cardtype::int,
    number1 = d.number1,
    number2 = d.number2
  from lt_isic d
 where p.id = d.id;

drop table lt_isic;


/*** lt_pasteboard ***/

alter table lt_product
    add column firststation citext2,
    add column laststation citext2,
    add column serviceclass integer not null default 0,
    add column trainnumber citext2,
    add column carnumber citext2
;

update lt_product p set
    number_ = d.number_,
    firststation = d.firststation,
    laststation = d.laststation,
    startdate = d.startdate,
    serviceclass = d.serviceclass,
    trainnumber = d.trainnumber,
    carnumber = d.carnumber
  from lt_pasteboard d
 where p.id = d.id;

drop table lt_pasteboard;


/*** lt_sim_card ***/

alter table lt_product
    add column issale boolean not null default false
;

update lt_product p set
    number_ = d.number_,
    issale = d.issale
  from lt_sim_card d
 where p.id = d.id;

drop table lt_sim_card;


/*** lt_tour ***/

alter table lt_product
    add column aviadescription citext2,
    add column transferdescription citext2 
;

update lt_product p set
    startdate = d.startdate,
    finishdate = d.finishdate,
    hotelname = d.hotelname,
    hoteloffice = d.hoteloffice,
    hotelcode = d.hotelcode,
    placementname = d.placementname,
    placementoffice = d.placementoffice,
    placementcode = d.placementcode,
    aviadescription = d.aviadescription,
    transferdescription = d.transferdescription,
    accommodationtype = d.accommodationtype,
    cateringtype = d.cateringtype
  from lt_tour d
 where p.id = d.id;

drop table lt_tour;


/*** lt_transfer ***/

update lt_product p set
    startdate = d.startdate
  from lt_transfer d
 where p.id = d.id;

drop table lt_transfer;

--rollback;