--begin work;

truncate table merci2.lt_party cascade;
truncate table merci2.lt_order cascade;
truncate table merci2.lt_currency cascade;
truncate table merci2.lt_country cascade;
truncate table merci2.lt_passport cascade;
truncate table merci2.lt_sequence cascade;
truncate table merci2.lt_system_variables cascade;
truncate table merci2.lt_identity cascade;
truncate table merci2.lt_gds_file cascade;
truncate table merci2.lt_modification cascade;
truncate table merci2.lt_modification_items cascade;


insert into merci2.lt_party (
    id, version, createdby, createdon, modifiedby, modifiedon, name, 
    legalname, phone1, phone2, fax, email1, email2, webaddress, iscustomer, 
    issupplier, legaladdress, actualaddress, note, reportsto
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, name, 
    legalname, phone1, phone2, fax, email1, email2, webaddress, iscustomer, 
    issupplier, legaladdress, actualaddress, note, reportsto
  from merci.lt_party;

insert into merci2.lt_organization (id, code)
select id, code
  from merci.lt_organization;

insert into merci2.lt_person (
    id, birthday, milescardsstring, title, organization
)
select id, birthday, milescardsstring, title, organization
  from merci.lt_person;

insert into merci2.lt_department
select id, organization
   from merci.lt_department;


insert into merci2.lt_miles_card (
    id, version, createdby, createdon, modifiedby, modifiedon, number_, owner, organization
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, number_, owner, organization
  from merci.lt_miles_card;

insert into merci2.lt_file (id, filename, "timestamp", content, uploadedby, party)
select id, filename, "timestamp", content, uploadedby, party
  from merci.lt_file;


insert into merci2.lt_currency (
    id, version, createdby, createdon, modifiedby, modifiedon, name, 
    code, cyrilliccode, numericcode
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, name, 
    code, cyrilliccode, numericcode
  from merci.lt_currency;

insert into merci2.lt_country (
    id, version, createdby, createdon, modifiedby, modifiedon, name, 
    twocharcode, threecharcode
)
select id, version, createdby, createdon, modifiedby, modifiedon, name, 
       twocharcode, threecharcode
  from merci.lt_country;

insert into merci2.lt_passport (
    id, version, createdby, createdon, modifiedby, modifiedon, number_, 
    firstname, middlename, lastname, birthday, gender, expiredon, 
    note, owner, citizenship, issuedby
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, number_, 
    firstname, middlename, lastname, birthday, gender, expiredon, 
    note, owner, citizenship, issuedby
  from merci.lt_passport;


  
insert into merci2.lt_airline (
    id, iatacode, prefixcode, passportrequirement, version, createdby, 
    createdon, modifiedby, modifiedon, name, organization
)  
select 
    id, iatacode, prefixcode, passportrequirement, version, createdby, 
    createdon, modifiedby, modifiedon, name, organization
  from merci.lt_airline;  

insert into merci2.lt_airline_commission_percents (
    id, version, domestic, interlinedomestic, interlineinternational, 
    international, airline
)
select id, version, domestic, interlinedomestic, interlineinternational, 
       international, airline
  from merci.lt_airline_commission_percents;

insert into merci2.lt_airline_service_class (
    id, version, createdby, createdon, modifiedby, modifiedon, code, 
    serviceclass, airline
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, code, 
    serviceclass, airline
  from merci.lt_airline_service_class;

insert into merci2.lt_airport (
    id, version, createdby, createdon, modifiedby, modifiedon, code, 
    settlement, localizedsettlement, name, country
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, code, 
    settlement, localizedsettlement, name, country
  from merci.lt_airport;


insert into merci2.lt_sequence (id, name, discriminator, format, "timestamp", current)
select id, name, discriminator, format, "timestamp", current
  from merci.lt_sequence;

insert into merci2.lt_system_variables (id, version, modifiedby, modifiedon, birthdaytasktimestamp)
select id, version, modifiedby, modifiedon, birthdaytasktimestamp
  from merci.lt_system_variables;


insert into merci2.lt_identity (id, version, name)
select id, version, name
  from merci.lt_identity;

insert into merci2.lt_internal_identity (id, description)
select id, description
  from merci.lt_internal_identity;

insert into merci2.lt_user (
    id, createdby, createdon, modifiedby, modifiedon, password, active, 
    isadministrator, issupervisor, isagent, isanalyst, iscashier, 
    issubagent, person, sessionid
)
select 
    id, createdby, createdon, modifiedby, modifiedon, password, active, 
    isadministrator, issupervisor, isagent, isanalyst, iscashier, 
    issubagent, person, sessionid
  from merci.lt_user;


insert into merci2.lt_gds_agent (
    id, version, createdby, createdon, modifiedby, modifiedon, origin, 
    code, officecode, person, office
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, origin, 
    code, officecode, person, office
  from merci.lt_gds_agent;

insert into merci2.lt_gds_file (
    id, class, version, createdby, createdon, modifiedby, modifiedon, 
    name, "timestamp", content, importresult, importoutput, filetype, 
    filepath, username, office, officecode
)
select 
    id, class, version, createdby, createdon, modifiedby, modifiedon, 
    name, "timestamp", content, importresult, importoutput, filetype, 
    filepath, username, office, officecode
  from merci.lt_gds_file;



insert into merci2.lt_avia_document (
    id, version, createdby, createdon, modifiedby, modifiedon, type, 
    issuedate, airlineiatacode, airlineprefixcode, airlinename, number_, 
    conjunctionnumbers, isprocessed, isvoid, requiresprocessing, 
    passengername, gdspassportstatus, gdspassport, itinerary, commissionpercent, 
    paymenttype, paymentform, bookeroffice, bookercode, ticketeroffice, 
    ticketercode, originator, origin, pnrcode, airlinepnrcode, tourcode, 
    note, remarks, displaystring, airline, passenger, booker, ticketer, 
    seller, owner, customer, intermediary, originaldocument, 
    fare_amount, fare_currency, equalfare_amount, equalfare_currency, 
    commission_amount, commission_currency, commissiondiscount_amount, 
    commissiondiscount_currency, feestotal_amount, feestotal_currency, 
    vat_amount, vat_currency, total_amount, total_currency, servicefee_amount, 
    servicefee_currency, handling_amount, handling_currency, discount_amount, 
    discount_currency, grandtotal_amount, grandtotal_currency, ticketingiataoffice, 
    isticketerrobot
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, type, 
    issuedate, airlineiatacode, airlineprefixcode, airlinename, number_, 
    conjunctionnumbers, isprocessed, isvoid, requiresprocessing, 
    passengername, gdspassportstatus, gdspassport, itinerary, commissionpercent, 
    paymenttype, paymentform, bookeroffice, bookercode, ticketeroffice, 
    ticketercode, originator, origin, pnrcode, airlinepnrcode, tourcode, 
    note, remarks, displaystring, airline, passenger, booker, ticketer, 
    seller, owner, customer, intermediary, originaldocument, 
    fare_amount, fare_currency, equalfare_amount, equalfare_currency, 
    commission_amount, commission_currency, commissiondiscount_amount, 
    commissiondiscount_currency, feestotal_amount, feestotal_currency, 
    vat_amount, vat_currency, total_amount, total_currency, servicefee_amount, 
    servicefee_currency, handling_amount, handling_currency, discount_amount, 
    discount_currency, grandtotal_amount, grandtotal_currency, ticketingiataoffice, 
    isticketerrobot
  from merci.lt_avia_document;


insert into merci2.lt_avia_mco (id, reissuefor)
select id, reissuefor
  from merci.lt_avia_mco;

insert into merci2.lt_avia_refund (
    id, cancelcommissionpercent, refundeddocument, refundservicefee_amount, 
    refundservicefee_currency, servicefeepenalty_amount, servicefeepenalty_currency, 
    cancelfee_amount, cancelfee_currency, cancelcommission_amount, 
    cancelcommission_currency
)
select 
    id, cancelcommissionpercent, refundeddocument, refundservicefee_amount, 
    refundservicefee_currency, servicefeepenalty_amount, servicefeepenalty_currency, 
    cancelfee_amount, cancelfee_currency, cancelcommission_amount, 
    cancelcommission_currency
  from merci.lt_avia_refund;

insert into merci2.lt_avia_ticket (
    id, domestic, interline, segmentclasses, departure, endorsement, reissuefor
)
select 
    id, domestic, interline, segmentclasses, departure, endorsement, reissuefor
  from merci.lt_avia_ticket;

insert into merci2.lt_avia_document_fee (
    id, version, createdby, createdon, modifiedby, modifiedon, code, 
    document, amount_amount, amount_currency
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, code, 
    document, amount_amount, amount_currency
  from merci.lt_avia_document_fee;

insert into merci2.lt_avia_document_voiding (
    id, version, createdby, createdon, modifiedby, modifiedon, isvoid, 
    "timestamp", agentoffice, agentcode, document, agent, iataoffice
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, isvoid, 
    "timestamp", agentoffice, agentcode, document, agent, iataoffice
  from merci.lt_avia_document_voiding;


insert into merci2.lt_document_access (
    id, version, createdby, createdon, modifiedby, modifiedon, fulldocumentcontrol, owner, person
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, fulldocumentcontrol, owner, person
  from merci.lt_document_access;

insert into merci2.lt_document_owner (id, version, isactive, owner)
select id, version, isactive, owner
  from merci.lt_document_owner;

insert into merci2.lt_flight_segment (
    id, version, createdby, createdon, modifiedby, modifiedon, "position", 
    type, fromairportcode, fromairportname, toairportcode, toairportname, 
    carrieriatacode, carrierprefixcode, carriername, flightnumber, 
    serviceclasscode, serviceclass, departuretime, arrivaltime, mealcodes, 
    mealtypes, numberofstops, luggage, checkinterminal, checkintime, 
    duration, arrivalterminal, seat, farebasis, stopover, ticket, 
    fromairport, toairport, carrier,
    isinclusive, issidetrip, distance
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, "position", 
    type, fromairportcode, fromairportname, toairportcode, toairportname, 
    carrieriatacode, carrierprefixcode, carriername, flightnumber, 
    serviceclasscode, serviceclass, departuretime, arrivaltime, mealcodes, 
    mealtypes, numberofstops, luggage, checkinterminal, checkintime, 
    duration, arrivalterminal, seat, farebasis, stopover, ticket, 
    fromairport, toairport, carrier,
    false, false, 0
  from merci.lt_flight_segment;


insert into merci2.lt_order (
    id, version, createdby, createdon, modifiedby, modifiedon, number_, 
    issuedate, isvoid, note, customer, shipto, billto, assignedto, 
    owner, discount_amount, discount_currency, vat_amount, vat_currency, 
    total_amount, total_currency, paid_amount, paid_currency, totaldue_amount, 
    totaldue_currency, vatdue_amount, vatdue_currency, ispublic, 
    issubjectofpaymentscontrol, ispaid,
    deliveryBalance
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, number_, 
    issuedate, status = 2 as isvoid, note, customer, shipto, billto, assignedto, 
    owner, discount_amount, discount_currency, vat_amount, vat_currency, 
    total_amount, total_currency, paid_amount, paid_currency, totaldue_amount, 
    totaldue_currency, vatdue_amount, vatdue_currency, ispublic, 
    issubjectofpaymentscontrol, coalesce(totaldue_amount, 0) <= 0 as ispaid,
    paid_amount
  from merci.lt_order;

insert into merci2.lt_order_item (
    id, version, createdby, createdon, modifiedby, modifiedon, "position", 
    text, quantity, hasvat, order_, consignment, price_amount, price_currency, 
    discount_amount, discount_currency, grandtotal_amount, grandtotal_currency, 
    givenvat_amount, givenvat_currency, taxedtotal_amount, taxedtotal_currency
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, "position", 
    text, quantity, hasvat, order_, consignment, price_amount, price_currency, 
    discount_amount, discount_currency, grandtotal_amount, grandtotal_currency, 
    givenvat_amount, givenvat_currency, taxedtotal_amount, taxedtotal_currency
  from merci.lt_order_item;


insert into merci2.lt_order_item_source_link (id)
select id
  from merci.lt_order_item_source_link;

insert into merci2.lt_order_item_avia_link (id, linktype, document)
select id, linktype, document
  from merci.lt_order_item_avia_link;


update merci2.lt_avia_document d
   set order_ = (select order_ from merci.lt_avia_document where id = d.id);


insert into merci2.lt_invoice (
    id, version, number_, agreement, type, issuedate, "timestamp", 
    content, order_, issuedby, total_amount, total_currency
)
select 
    id, version, number_, agreement, type, issuedate, "timestamp", 
    content, order_, issuedby, total_amount, total_currency
  from merci.lt_invoice;


insert into merci2.lt_payment_system (id, version, createdby, createdon, modifiedby, modifiedon, name)
select id, version, createdby, createdon, modifiedby, modifiedon, name
  from merci.lt_payment_system;

insert into merci2.lt_payment (
    id, class, version, createdby, createdon, modifiedby, modifiedon, 
    number_, documentnumber, documentuniquecode, paymentform, receivedfrom, 
    note, isvoid, date_, postedon, printeddocument, payer, assignedto, 
    registeredby, order_, invoice, owner, amount_amount, amount_currency, 
    vat_amount, vat_currency, authorizationcode, paymentsystem
)
select 
    id, class, version, createdby, createdon, modifiedby, modifiedon, 
    number_, documentnumber, documentuniquecode, paymentform, receivedfrom, 
    note, isvoid, date_, postedon, printeddocument, payer, assignedto, 
    registeredby, order_, invoice, owner, amount_amount, amount_currency, 
    vat_amount, vat_currency, authorizationcode, paymentsystem
  from merci.lt_payment;

insert into merci2.lt_internal_transfer (
    id, version, createdby, createdon, modifiedby, modifiedon, number_, 
    date_, fromparty, fromorder, toparty, toorder, amount
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, number_, 
    date_, fromparty, fromorder, toparty, toorder, amount
  from merci.lt_internal_transfer;

insert into merci2.lt_consignment (
    id, version, createdby, createdon, modifiedby, modifiedon, number_, 
    issuedate, totalsupplied, supplier, acquirer, grandtotal_amount, 
    grandtotal_currency, vat_amount, vat_currency, discount_amount, 
    discount_currency
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, number_, 
    issuedate, totalsupplied, supplier, acquirer, grandtotal_amount, 
    grandtotal_currency, vat_amount, vat_currency, discount_amount, 
    discount_currency
  from merci.lt_consignment;

insert into merci2.lt_issued_consignment (id, number_, "timestamp", content, consignment, issuedby)
select id, number_, "timestamp", content, consignment, issuedby
  from merci.lt_issued_consignment;

insert into merci2.lt_modification (
    id, "timestamp", author, type, comment_, instancetype, instanceid, instancestring
)
select 
    id, "timestamp", author, type, comment_, instancetype, instanceid, instancestring
  from merci.lt_modification;

insert into merci2.lt_modification_items (property, modification, oldvalue)
select property, modification, oldvalue
  from merci.lt_modification_items;

insert into merci2.lt_closed_period (
    id, version, createdby, createdon, modifiedby, modifiedon, dateto, datefrom, periodstate
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, dateto, datefrom, periodstate
  from merci.lt_closed_period;


insert into merci.lt_opening_balance (
    id, version, createdby, createdon, modifiedby, modifiedon, number_, date_, balance, party
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, number_, date_, balance, party
  from merci.lt_opening_balance;

insert into merci2.lt_preferences (id, version, createdby, createdon, modifiedby, modifiedon, identity)
select id, version, createdby, createdon, modifiedby, modifiedon, identity
  from merci.lt_preferences;

insert into merci2.lt_task (
    id, version, createdby, createdon, modifiedby, modifiedon, number_, 
    subject, description, status, duedate, relatedto, assignedto
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, number_, 
    subject, description, status, duedate, relatedto, assignedto
  from merci.lt_task;


insert into merci2.lt_task_comment (id, version, createdby, createdon, modifiedby, modifiedon, text, task)
select id, version, createdby, createdon, modifiedby, modifiedon, text, task
  from merci.lt_task_comment;

insert into merci2.lt_penalize_operation (
    id, version, createdby, createdon, modifiedby, modifiedon, type, 
    status, description, ticket
)
select 
    id, version, createdby, createdon, modifiedby, modifiedon, type, 
    status, description, ticket
  from merci.lt_penalize_operation;

insert into merci2.lt_system_configuration (
    id, version, modifiedby, modifiedon, companydetails, amadeusrizusingmode, 
    vatrate, usedefaultcurrencyforinput, ispassengerpassportrequired, 
    aviaorderitemgenerationoption, separatedocumentaccess, allowagentsetordervat, 
    useaviadocumentvatinorder, aviadocumentvatoptions, incomingcashordercorrespondentaccount, 
    accountantdisplaystring, useaviahandling, isorganizationcoderequired, 
    daysbeforedeparture, company, birthdaytaskresponsible, defaultcurrency, 
    country, isorderrequiredforprocesseddocument,
    mcorequiresdescription, reservationsinofficemetrics
)
select 
    id, version, modifiedby, modifiedon, companydetails, amadeusrizusingmode, 
    vatrate, usedefaultcurrencyforinput, ispassengerpassportrequired, 
    aviaorderitemgenerationoption, separatedocumentaccess, allowagentsetordervat, 
    useaviadocumentvatinorder, aviadocumentvatoptions, incomingcashordercorrespondentaccount, 
    accountantdisplaystring, useaviahandling, isorganizationcoderequired, 
    daysbeforedeparture, company, birthdaytaskresponsible, defaultcurrency, 
    country, isorderrequiredforprocesseddocument,
    false, false
  from merci.lt_system_configuration;


--rollback;