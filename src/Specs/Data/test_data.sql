--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- Name: test; Type: SCHEMA; Schema: -; Owner: -
--

drop schema if exists test cascade;
CREATE SCHEMA test;


SET search_path = test, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: lt_airline; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_airline (
    id character varying(32) NOT NULL,
    iatacode public.citext2,
    prefixcode public.citext2,
    passportrequirement integer NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    name public.citext2 NOT NULL,
    organization character varying(32)
);


--
-- Name: lt_airline_commission_percents; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_airline_commission_percents (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    domestic numeric(19,5) NOT NULL,
    interlinedomestic numeric(19,5) NOT NULL,
    interlineinternational numeric(19,5) NOT NULL,
    international numeric(19,5) NOT NULL,
    airline character varying(32)
);


--
-- Name: lt_airline_service_class; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_airline_service_class (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    code public.citext2 NOT NULL,
    serviceclass integer NOT NULL,
    airline character varying(32) NOT NULL
);


--
-- Name: lt_airport; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_airport (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    code public.citext2 NOT NULL,
    settlement public.citext2,
    localizedsettlement public.citext2,
    name public.citext2,
    country character varying(32)
);


--
-- Name: lt_avia_document; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_avia_document (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    type integer NOT NULL,
    issuedate date NOT NULL,
    airlineiatacode public.citext2,
    airlineprefixcode public.citext2,
    airlinename public.citext2,
    number_ bigint,
    conjunctionnumbers public.citext2,
    isprocessed boolean NOT NULL,
    isvoid boolean NOT NULL,
    requiresprocessing boolean NOT NULL,
    passengername public.citext2,
    gdspassportstatus integer NOT NULL,
    gdspassport public.citext2,
    itinerary public.citext2,
    commissionpercent numeric(19,5),
    paymenttype integer NOT NULL,
    paymentform public.citext2,
    bookeroffice public.citext2,
    bookercode public.citext2,
    ticketeroffice public.citext2,
    ticketercode public.citext2,
    originator integer NOT NULL,
    origin integer NOT NULL,
    pnrcode public.citext2,
    airlinepnrcode public.citext2,
    tourcode public.citext2,
    note public.citext2,
    remarks public.citext2,
    displaystring public.citext2,
    airline character varying(32),
    passenger character varying(32),
    booker character varying(32),
    ticketer character varying(32),
    seller character varying(32),
    owner character varying(32),
    customer character varying(32),
    intermediary character varying(32),
    order_ character varying(32),
    originaldocument character varying(32),
    fare_amount numeric(19,5),
    fare_currency character varying(32),
    equalfare_amount numeric(19,5),
    equalfare_currency character varying(32),
    commission_amount numeric(19,5),
    commission_currency character varying(32),
    commissiondiscount_amount numeric(19,5),
    commissiondiscount_currency character varying(32),
    feestotal_amount numeric(19,5),
    feestotal_currency character varying(32),
    vat_amount numeric(19,5),
    vat_currency character varying(32),
    total_amount numeric(19,5),
    total_currency character varying(32),
    servicefee_amount numeric(19,5),
    servicefee_currency character varying(32),
    handling_amount numeric(19,5),
    handling_currency character varying(32),
    discount_amount numeric(19,5),
    discount_currency character varying(32),
    grandtotal_amount numeric(19,5),
    grandtotal_currency character varying(32),
    ticketingiataoffice public.citext2,
    isticketerrobot boolean
);


--
-- Name: lt_avia_document_fee; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_avia_document_fee (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    code public.citext2 NOT NULL,
    document character varying(32),
    amount_amount numeric(19,5),
    amount_currency character varying(32)
);


--
-- Name: lt_avia_document_voiding; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_avia_document_voiding (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    isvoid boolean NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    agentoffice public.citext2,
    agentcode public.citext2,
    document character varying(32) NOT NULL,
    agent character varying(32),
    iataoffice public.citext2
);


--
-- Name: lt_avia_mco; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_avia_mco (
    id character varying(32) NOT NULL,
    reissuefor character varying(32)
);


--
-- Name: lt_avia_refund; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_avia_refund (
    id character varying(32) NOT NULL,
    cancelcommissionpercent numeric(19,5),
    refundeddocument character varying(32),
    refundservicefee_amount numeric(19,5),
    refundservicefee_currency character varying(32),
    servicefeepenalty_amount numeric(19,5),
    servicefeepenalty_currency character varying(32),
    cancelfee_amount numeric(19,5),
    cancelfee_currency character varying(32),
    cancelcommission_amount numeric(19,5),
    cancelcommission_currency character varying(32)
);


--
-- Name: lt_avia_ticket; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_avia_ticket (
    id character varying(32) NOT NULL,
    domestic boolean NOT NULL,
    interline boolean NOT NULL,
    segmentclasses public.citext2,
    departure timestamp without time zone,
    endorsement public.citext2,
    reissuefor character varying(32)
);


--
-- Name: lt_closed_period; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_closed_period (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    dateto date NOT NULL,
    datefrom date NOT NULL,
    periodstate integer NOT NULL
);


--
-- Name: lt_consignment; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_consignment (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    number_ public.citext2 NOT NULL,
    issuedate date NOT NULL,
    totalsupplied public.citext2,
    supplier character varying(32),
    acquirer character varying(32) NOT NULL,
    grandtotal_amount numeric(19,5),
    grandtotal_currency character varying(32),
    vat_amount numeric(19,5),
    vat_currency character varying(32),
    discount_amount numeric(19,5),
    discount_currency character varying(32)
);


--
-- Name: lt_country; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_country (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    name public.citext2 NOT NULL,
    twocharcode public.citext2,
    threecharcode public.citext2
);


--
-- Name: lt_currency; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_currency (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    name public.citext2,
    code public.citext2 NOT NULL,
    cyrilliccode public.citext2,
    numericcode integer NOT NULL
);


--
-- Name: lt_department; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_department (
    id character varying(32) NOT NULL,
    organization character varying(32) NOT NULL
);


--
-- Name: lt_document_access; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_document_access (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    fulldocumentcontrol boolean NOT NULL,
    owner character varying(32),
    person character varying(32) NOT NULL
);


--
-- Name: lt_document_owner; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_document_owner (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    isactive boolean NOT NULL,
    owner character varying(32) NOT NULL
);


--
-- Name: lt_file; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_file (
    id character varying(32) NOT NULL,
    filename public.citext2 NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    content bytea NOT NULL,
    uploadedby character varying(32),
    party character varying(32)
);


--
-- Name: lt_flight_segment; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_flight_segment (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    "position" integer NOT NULL,
    type integer NOT NULL,
    fromairportcode public.citext2,
    fromairportname public.citext2,
    toairportcode public.citext2,
    toairportname public.citext2,
    carrieriatacode public.citext2,
    carrierprefixcode public.citext2,
    carriername public.citext2,
    flightnumber public.citext2,
    serviceclasscode public.citext2,
    serviceclass integer,
    departuretime timestamp without time zone,
    arrivaltime timestamp without time zone,
    mealcodes public.citext2,
    mealtypes integer,
    numberofstops integer,
    luggage public.citext2,
    checkinterminal public.citext2,
    checkintime public.citext2,
    duration public.citext2,
    arrivalterminal public.citext2,
    seat public.citext2,
    farebasis public.citext2,
    stopover boolean NOT NULL,
    ticket character varying(32) NOT NULL,
    fromairport character varying(32),
    toairport character varying(32),
    carrier character varying(32)
);


--
-- Name: lt_gds_agent; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_gds_agent (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    origin integer NOT NULL,
    code public.citext2 NOT NULL,
    officecode public.citext2 NOT NULL,
    person character varying(32) NOT NULL,
    office character varying(32)
);


--
-- Name: lt_gds_file; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_gds_file (
    id character varying(32) NOT NULL,
    class character varying(10) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    name public.citext2 NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    content public.citext2 NOT NULL,
    importresult integer NOT NULL,
    importoutput public.citext2,
    filetype integer NOT NULL,
    filepath public.citext2,
    username public.citext2,
    office public.citext2,
    officecode public.citext2
);


--
-- Name: lt_identity; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_identity (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    name public.citext2 NOT NULL
);


--
-- Name: lt_internal_identity; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_internal_identity (
    id character varying(32) NOT NULL,
    description public.citext2
);


--
-- Name: lt_internal_transfer; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_internal_transfer (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    number_ public.citext2 NOT NULL,
    date_ date NOT NULL,
    fromparty character varying(32) NOT NULL,
    fromorder character varying(32),
    toparty character varying(32) NOT NULL,
    toorder character varying(32),
    amount numeric(19,5) NOT NULL
);


--
-- Name: lt_invoice; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_invoice (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    number_ public.citext2 NOT NULL,
    agreement public.citext2,
    type integer NOT NULL,
    issuedate date NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    content bytea NOT NULL,
    order_ character varying(32),
    issuedby character varying(32),
    total_amount numeric(19,5),
    total_currency character varying(32)
);


--
-- Name: lt_issued_consignment; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_issued_consignment (
    id character varying(32) NOT NULL,
    number_ public.citext2,
    "timestamp" timestamp without time zone NOT NULL,
    content bytea NOT NULL,
    consignment character varying(32),
    issuedby character varying(32)
);


--
-- Name: lt_miles_card; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_miles_card (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    number_ public.citext2 NOT NULL,
    owner character varying(32) NOT NULL,
    organization character varying(32)
);


--
-- Name: lt_modification; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_modification (
    id integer NOT NULL,
    "timestamp" timestamp without time zone,
    author public.citext2,
    type integer,
    comment_ public.citext2,
    instancetype public.citext2,
    instanceid character varying(32),
    instancestring public.citext2
);


--
-- Name: lt_modification_id_seq; Type: SEQUENCE; Schema: test; Owner: -
--

CREATE SEQUENCE lt_modification_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- Name: lt_modification_id_seq; Type: SEQUENCE OWNED BY; Schema: test; Owner: -
--

ALTER SEQUENCE lt_modification_id_seq OWNED BY lt_modification.id;


--
-- Name: lt_modification_id_seq; Type: SEQUENCE SET; Schema: test; Owner: -
--

SELECT pg_catalog.setval('lt_modification_id_seq', 306643, true);


--
-- Name: lt_modification_items; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_modification_items (
    property character varying(255) NOT NULL,
    modification integer NOT NULL,
    oldvalue public.citext2
);


--
-- Name: lt_opening_balance; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_opening_balance (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    number_ public.citext2 NOT NULL,
    date_ date NOT NULL,
    balance numeric(19,5) NOT NULL,
    party character varying(32) NOT NULL
);


--
-- Name: lt_order; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_order (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    number_ public.citext2 NOT NULL,
    issuedate date NOT NULL,
    status integer NOT NULL,
    note public.citext2,
    customer character varying(32) NOT NULL,
    shipto character varying(32),
    billto character varying(32),
    assignedto character varying(32),
    owner character varying(32),
    discount_amount numeric(19,5),
    discount_currency character varying(32),
    vat_amount numeric(19,5),
    vat_currency character varying(32),
    total_amount numeric(19,5),
    total_currency character varying(32),
    paid_amount numeric(19,5),
    paid_currency character varying(32),
    totaldue_amount numeric(19,5),
    totaldue_currency character varying(32),
    vatdue_amount numeric(19,5),
    vatdue_currency character varying(32),
    ispublic boolean NOT NULL,
    issubjectofpaymentscontrol boolean NOT NULL
);


--
-- Name: lt_order_item; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_order_item (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    "position" integer NOT NULL,
    text public.citext2 NOT NULL,
    quantity integer NOT NULL,
    hasvat boolean NOT NULL,
    order_ character varying(32) NOT NULL,
    consignment character varying(32),
    price_amount numeric(19,5),
    price_currency character varying(32),
    discount_amount numeric(19,5),
    discount_currency character varying(32),
    grandtotal_amount numeric(19,5),
    grandtotal_currency character varying(32),
    givenvat_amount numeric(19,5),
    givenvat_currency character varying(32),
    taxedtotal_amount numeric(19,5),
    taxedtotal_currency character varying(32)
);


--
-- Name: lt_order_item_avia_link; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_order_item_avia_link (
    id character varying(32) NOT NULL,
    linktype integer NOT NULL,
    document character varying(32)
);


--
-- Name: lt_order_item_source_link; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_order_item_source_link (
    id character varying(32) NOT NULL
);


--
-- Name: lt_order_status_record; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_order_status_record (
    id character varying(32) NOT NULL,
    status integer NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    changedby character varying(32) NOT NULL,
    order_ character varying(32)
);


--
-- Name: lt_organization; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_organization (
    id character varying(32) NOT NULL,
    code public.citext2
);


--
-- Name: lt_party; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_party (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    name public.citext2 NOT NULL,
    legalname public.citext2,
    phone1 public.citext2,
    phone2 public.citext2,
    fax public.citext2,
    email1 public.citext2,
    email2 public.citext2,
    webaddress public.citext2,
    iscustomer boolean NOT NULL,
    issupplier boolean NOT NULL,
    legaladdress public.citext2,
    actualaddress public.citext2,
    note public.citext2,
    reportsto character varying(32)
);


--
-- Name: lt_passport; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_passport (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    number_ public.citext2 NOT NULL,
    firstname public.citext2,
    middlename public.citext2,
    lastname public.citext2,
    birthday date,
    gender integer,
    expiredon date,
    note public.citext2,
    owner character varying(32) NOT NULL,
    citizenship character varying(32),
    issuedby character varying(32)
);


--
-- Name: lt_payment; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_payment (
    id character varying(32) NOT NULL,
    class character varying(20) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    number_ public.citext2 NOT NULL,
    documentnumber public.citext2,
    documentuniquecode public.citext2,
    paymentform integer NOT NULL,
    receivedfrom public.citext2,
    note public.citext2,
    isvoid boolean NOT NULL,
    date_ date NOT NULL,
    postedon timestamp without time zone,
    printeddocument bytea,
    payer character varying(32) NOT NULL,
    assignedto character varying(32) NOT NULL,
    registeredby character varying(32) NOT NULL,
    order_ character varying(32),
    invoice character varying(32),
    owner character varying(32),
    amount_amount numeric(19,5),
    amount_currency character varying(32),
    vat_amount numeric(19,5),
    vat_currency character varying(32),
    authorizationcode public.citext2,
    paymentsystem character varying(32)
);


--
-- Name: lt_payment_system; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_payment_system (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    name public.citext2 NOT NULL
);


--
-- Name: lt_penalize_operation; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_penalize_operation (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    type integer,
    status integer,
    description public.citext2,
    ticket character varying(32)
);


--
-- Name: lt_person; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_person (
    id character varying(32) NOT NULL,
    birthday date,
    milescardsstring public.citext2,
    title public.citext2,
    organization character varying(32)
);


--
-- Name: lt_preferences; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_preferences (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    identity character varying(32) NOT NULL
);


--
-- Name: lt_sequence; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_sequence (
    id integer NOT NULL,
    name public.citext2 NOT NULL,
    discriminator public.citext2,
    format public.citext2 NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    current bigint NOT NULL
);


--
-- Name: lt_system_configuration; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_system_configuration (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    companydetails public.citext2,
    amadeusrizusingmode integer NOT NULL,
    vatrate numeric(19,5) NOT NULL,
    usedefaultcurrencyforinput boolean NOT NULL,
    ispassengerpassportrequired boolean NOT NULL,
    aviaorderitemgenerationoption integer NOT NULL,
    separatedocumentaccess boolean NOT NULL,
    allowagentsetordervat boolean NOT NULL,
    useaviadocumentvatinorder boolean NOT NULL,
    aviadocumentvatoptions integer NOT NULL,
    incomingcashordercorrespondentaccount public.citext2 NOT NULL,
    accountantdisplaystring public.citext2,
    useaviahandling boolean NOT NULL,
    isorganizationcoderequired boolean NOT NULL,
    daysbeforedeparture integer NOT NULL,
    company character varying(32),
    birthdaytaskresponsible character varying(32),
    defaultcurrency character varying(32) NOT NULL,
    country character varying(32),
    isorderrequiredforprocesseddocument boolean NOT NULL
);


--
-- Name: lt_system_shutdown; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_system_shutdown (
    id character varying(32) NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    note public.citext2 NOT NULL,
    launchplannedon timestamp without time zone NOT NULL
);


--
-- Name: lt_system_variables; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_system_variables (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    birthdaytasktimestamp timestamp without time zone NOT NULL
);


--
-- Name: lt_task; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_task (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    number_ public.citext2 NOT NULL,
    subject public.citext2 NOT NULL,
    description public.citext2,
    status integer NOT NULL,
    duedate date,
    relatedto character varying(32),
    order_ character varying(32),
    assignedto character varying(32)
);


--
-- Name: lt_task_comment; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_task_comment (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    text public.citext2 NOT NULL,
    task character varying(32) NOT NULL
);


--
-- Name: lt_user; Type: TABLE; Schema: test; Owner: -; Tablespace: 
--

CREATE TABLE lt_user (
    id character varying(32) NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    password text NOT NULL,
    active boolean NOT NULL,
    isadministrator boolean NOT NULL,
    issupervisor boolean NOT NULL,
    isagent boolean NOT NULL,
    isanalyst boolean NOT NULL,
    iscashier boolean NOT NULL,
    issubagent boolean NOT NULL,
    person character varying(32) NOT NULL,
    sessionid public.citext2
);


--
-- Name: id; Type: DEFAULT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_modification ALTER COLUMN id SET DEFAULT nextval('lt_modification_id_seq'::regclass);


--
-- Data for Name: lt_airline; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_airline (id, iatacode, prefixcode, passportrequirement, version, createdby, createdon, modifiedby, modifiedon, name, organization) FROM stdin;
3980b753af9d408881e2f8abcb71db6a	VV	870	0	1	viarosh	2012-02-16 15:39:35	\N	\N	AEROSVIT AIRLINES	\N
48c384d42f35436f9e1b2daee1371d3c	B2	628	0	1	admin	2012-05-15 15:56:50	\N	\N	BELAVIA	\N
\.


--
-- Data for Name: lt_airline_commission_percents; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_airline_commission_percents (id, version, domestic, interlinedomestic, interlineinternational, international, airline) FROM stdin;
\.


--
-- Data for Name: lt_airline_service_class; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_airline_service_class (id, version, createdby, createdon, modifiedby, modifiedon, code, serviceclass, airline) FROM stdin;
101e9b133f0b4b68bf2c97628614c4bb	1	viarosh	2012-02-16 15:39:35	\N	\N	K	0	3980b753af9d408881e2f8abcb71db6a
a038373c0bfd45c3b79ac7a4608c48d3	1	admin	2012-05-15 15:56:50	\N	\N	V	0	48c384d42f35436f9e1b2daee1371d3c
\.


--
-- Data for Name: lt_airport; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_airport (id, version, createdby, createdon, modifiedby, modifiedon, code, settlement, localizedsettlement, name, country) FROM stdin;
0034b167c35d4873ad0ddc312cc54560	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BIQ	BIARRITZ	\N	BAYONNE ANGLET	ffd49325645b4cfe90b12af209a6cdd8
0076472194f7446d8326941b1d0ea093	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BER	BERLIN	\N	BERLIN	f5ae6ece20784948ab85e545c81c4f06
0113477ff0ca41f394eea2fe4467ceca	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PLQ	KLAIPEDA/PALANGA	\N	INTERNATIONAL	44c9a4d7bc1449bfa2b17a43a0e23033
02337b235e6e4e55a046f4a067768a34	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MXP	MILAN	\N	MALPENSA	ba77bb2e374a4850aee10ea489625361
0256735434a34890b3f1a5b8a22e48eb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KUF	SAMARA	\N	SAMARA	5ecfeef727294990ac6facf7502b9905
031f6c6123dc4ac9941cde3fb95380c6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SDV	\N	\N	SDV	\N
0388728d04334ba58518cbbad7a16b04	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LPB	\N	\N	PAZ	\N
038ebed585c1482d8e3197f8258948d6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ASU	ASUNCION	\N	SILVIO PETTIROSSI 	d00ae58bace04a28a899fb077b8db10e
03ae8f755c184939b759264a7f5eaff7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NSK	NORILSK	\N	NORILSK	5ecfeef727294990ac6facf7502b9905
0469fd6b6df5432796ab906f92de3273	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FRA	FRANKFURT	\N	FRANKFURT	f5ae6ece20784948ab85e545c81c4f06
051e37f161214856935cdfc63fe137be	1	SYSTEM	2010-03-24 00:00:00	\N	\N	YVR	VANCOUVER (BRITISH COLUMBIA)	\N	VANCOUVER INTL	4fa75470195843bca3fed516645b6e32
05de127e3f904c9bb3383c2877d1cc46	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BOJ	\N	\N	\N	\N
05edd2620efd4568b0320f1d078b558f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	YEG	EDMONTON	\N	EDMONTON INTL	4fa75470195843bca3fed516645b6e32
07356411411d4a3580184d033ef7d242	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BGY	MILAN	\N	BERGAMO ORIO AL SERIO	ba77bb2e374a4850aee10ea489625361
0771bd864b5646b0b581be13edf45b9c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ASF	ASTRAKHAN	\N	ASTRAKHAN	5ecfeef727294990ac6facf7502b9905
07e24501c2824ff1a32777a4ce9a5188	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TNA	\N	\N	\N	\N
082bd5c0c6a54210bcdfd25b482349ea	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DLM	DALAMAN	\N	DALAMAN	972fe95d3b8e4745bcb2697ee9fc0ef5
0891494a14bd42ec8647f1b591c6ecbe	1	SYSTEM	2010-03-24 00:00:00	\N	\N	RZE	RZESZOW	\N	JASIONKA	ca502fc306fa42bba100929aeeebbde0
099d33d61c004f3cbe9984382714f145	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HKT	PHUKET	\N	PHUKET INTERNATIONAL	e9e616a02d5d44739e04cfd592703021
09b7d0cd412843afa362ae57df9029b4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JKG	JONKOPING	\N	AXAMO	f95a221cc315402e86c5345587c54aa4
09c06358796f46afa6e8cac7cf44d572	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ERF	\N	\N	APT	\N
09f36059d6504b308c8c4c32fd3c9ff9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FAO	FARO	\N	FARO	014c0f454bf247d3a54ce5be2888a5d3
0a0748d0f3ef4b4db2e4b8556b45d5b1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CUN	CANCUN	\N	CANCUN	18c759527c87482a81bd7378010d2928
0a0a49acafca49db9d341d2f9ce9b38e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LNZ	LINZ	\N	BLUE DANUBE	3ce2c010c85846378131c57cd0e8cdca
0b1468cb6e5944bab860c90bf53779b1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GOJ	NIZHNIY NOVGOROD	\N	NIZHNIY NOVGOROD	5ecfeef727294990ac6facf7502b9905
0b78ab22456240c2a1df66ad03ef09fc	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GIG	\N	\N	GIG	\N
0c51b370f08d4b169d1fd230e2c5eba8	1	SYSTEM	2010-09-29 18:21:48	\N	\N	IXR	\N	\N	RANCHI	\N
0c61ce5fb5da446ead74c299579a2b68	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CIT	SHIMKENT	\N	SHIMKENT	4039977f975746d2bc4191627f8e769f
0d1ed6acfe674bba901deb98fcda9a8b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	EAS	SAN SEBASTIAN	\N	SAN SEBASTIAN	62118ce1533a40689ca4a01f289f588a
0d316270c94246bdbb54428598c9bb57	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ROM	ROME	\N	\N	ba77bb2e374a4850aee10ea489625361
0e45fe22d38c4bc7920f01ebba64d6d1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SEL	SEOUL	\N	\N	b86d41d36824417d8424c9e89fb7943c
0e9b00c25f1d48ccac4d981b1e2592b9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NAP	NAPLES	\N	CAPODICHINO	ba77bb2e374a4850aee10ea489625361
0ee2620f525543f2b8809e2d94b40301	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ICN	SEOUL	\N	INCHEON INTERNATIONAL	b86d41d36824417d8424c9e89fb7943c
0f269f5a774c4359a0ecee8e498c08d8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ADD	ADDIS ABABA	\N	BOLE	f9924f9e7a374bb3a56c08956d217fc0
0f7f612bed294398bfff5d476c2fa660	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SFO	SAN FRANCISCO	\N	SAN FRANCISCO	cb823aa64f954b3c83aacf75d552dc02
0ffd99239fa54dc39e0f2617aa7993ea	1	SYSTEM	2010-03-24 00:00:00	\N	\N	URC	\N	\N	\N	\N
10963bf39f9f453d93e61c1d0b55ad93	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TPA	TAMPA (FLORIDA)	\N	TAMPA INTERNATIONAL	cb823aa64f954b3c83aacf75d552dc02
113a50f8156241b1b1cb64e32250cae8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UCT	\N	\N	\N	\N
1260668f1e094a04a476b2c06fdad8ea	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PWM	\N	\N	\N	\N
12b3830dd0a643329e77ef3e62ba1c96	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ODS	ODESSA	\N	ODESSA	c6597a0f59204b00b63fe1c0f121df7e
12b7b3376e7144fdbc1a8a76e05b3183	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JRO	\N	\N	\N	\N
135af2f7708f494fb2d337c41495d6c5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SYR	SYRACUSE	\N	SYRACUSE	\N
13724683bb0641bb878c4b6ad6c0c424	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ALC	ALICANTE	\N	ALICANTE	62118ce1533a40689ca4a01f289f588a
15833d839e304cf999ac6385e2e3f2bf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SSA	SALVADOR	\N	LUIS E. MAGALHAES 	cf3ca3bf06c347549e18dc270cd8a50f
158e739618044b40bc6735322c1708ab	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DPS	DENPASAR BALI	\N	NGURAH RAI	c36323ead77544ea9c0ef4903d760126
16368316512b478cb70a1866ca7a32b9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BGO	BERGEN	\N	FLESLAND	c13361d24a884f34bbad5cd169b60b44
16b54eb121df4b57a70d11a6395820d4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SKP	SKOPJE	\N	ALEXANDER THE GREAT	13c21eebeddf47c9bd5a514693c8d864
16c8a983bed243388e460c8c61ad0f8b	1	SYSTEM	2011-06-24 17:33:04	\N	\N	TYS	\N	\N	KNOXVILLE	\N
1708397abf6d46d8a0da2bd72efc1049	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ATL	ATLANTA (us GEORGIA)	\N	HARTSFIELD-JACKSON	cb823aa64f954b3c83aacf75d552dc02
1730973ac46648e0a222ad87007617b6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CHI	CHICAGO	\N	\N	cb823aa64f954b3c83aacf75d552dc02
1791c7e963854e6aaf60b1018e468892	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KUT	KUTAISI	\N	KOPITNARI	472d137c8a7c44719fd4c809948fd1c8
17cd3945ca80477c9229ee78761d0eb6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DMM	DAMMAM	\N	KING FAHAD INTL	58126c7610404d0f86ed2ab232c65622
17d0d65127f6461e896d69736a8bd041	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DME	MOSCOW	\N	DOMODEDOVO	5ecfeef727294990ac6facf7502b9905
180e4f43b3a3429abd680252c770573c	1	SYSTEM	2010-08-18 17:01:06	\N	\N	NNG	\N	\N	NANNING	\N
183caa36ff8b484d886fa14f3d5dffe3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CMH	COLUMBUS	\N	PORT COLUMBUS	cb823aa64f954b3c83aacf75d552dc02
184a39929e9543728627349262834a41	1	SYSTEM	2011-08-19 10:08:41	\N	\N	IND	\N	\N	INDIANAPOLIS	\N
185296f52f0148c5a660e25353268edf	1	SYSTEM	2011-06-30 17:09:27	\N	\N	SHE	\N	\N	SHENYANG	\N
185b0a7ecf5243fab6b973ad59b8d11f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MAD	MADRID	\N	BARAJAS	62118ce1533a40689ca4a01f289f588a
1955d631f7664ae09f6524d9222ba31e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ANC	\N	\N	INTL	\N
1a0eb7b9690449a0a55e9d5971b2c399	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KAJ	\N	\N	\N	\N
1bad920dcb28474fab9053d4308b0f6f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CLE	\N	\N	HOPKINS	\N
1c1129f6164d46ad8d9e198e50443027	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OSW	\N	\N	\N	\N
1de9d27be3c648e2b9d819ec402d92ce	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MCO	ORLANDO (FLORIDA)	\N	ORLANDO INTERNATIONAL	cb823aa64f954b3c83aacf75d552dc02
1f46ad304df941bfadbd9c4d8d57308a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DTM	\N	\N	\N	\N
1fc15738bae54b72a5e3add571d30b6e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TUN	TUNIS	\N	CARTHAGE	11b3d912caac4d94babf84248333c4dc
207cae0ab0654e7caea3321d216a29a3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DYU	DUSHANBE	\N	DUSHANBE	21f596351b424abb993bf46ea23a6135
20e03c9ab04443a388a1b86f7d7eaac6	1	SYSTEM	2011-08-29 11:58:36	\N	\N	DYR	\N	\N	ANADYR UGOLNY	\N
21147014412545c2934c4d25c89debd3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MPL	MONTPELLIER	\N	MEDITERRANEE	ffd49325645b4cfe90b12af209a6cdd8
212bd41febac41ba87d045bb382e4ab1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SEZ	MAHE ISLAND	\N	MAHE ISLAND	7eccd310b36f47989c040e2570457d84
213786a4ce3c462d96045b14b5a13baf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BUH	\N	\N	\N	\N
215502de059e47b09a2ca9d2ab95da3d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VKO	MOSCOW	\N	VNUKOVO INTL	5ecfeef727294990ac6facf7502b9905
22ee959890c84eff8c6d3b2ff659b078	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SZB	\N	\N	SZB	\N
233d7e45e62145f5ad1671517ea9636b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BCN	BARCELONA	\N	BARCELONA	62118ce1533a40689ca4a01f289f588a
23aff6ea9c7140efb3ce0a66779bf5e6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PTY	PANAMA CITY	\N	PANAMA CITY	712914f841344761a9cce4063fc0e361
23ca315a1a2d4985b02e80c4409fc591	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JTR	\N	\N	\N	\N
246d4d7e990548158450aef6aadf85e8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AYT	ANTALYA	\N	ANTALYA	972fe95d3b8e4745bcb2697ee9fc0ef5
24808fd0ff324f948e37c81aaab35e34	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PHL	PHILADELPHIA (PENNSYLVANIA)	\N	INTERNATIONAL	cb823aa64f954b3c83aacf75d552dc02
260c5210c3be4c4a860237c5a4dc3e81	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SKD	SAMARKAND	\N	SAMARKAND	9dcfccbd69504bc2863a6f6f7a274ae1
27152660d7d0457eb6dca2c9fc262eb0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	INN	INNSBRUCK	\N	INNSBRUCK	3ce2c010c85846378131c57cd0e8cdca
272aca180daf496dba71c232ab36d1bb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ETH	ELAT	\N	ELAT	e053c11fe6834a9b908fce39f0bfbe66
2783bdc9c5424956a6a15b3d8b53e3c3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	RHO	\N	\N	\N	\N
27a237ea316b446a8a71bc67d9b631b6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CLO	CALI	\N	ALFONSO B ARAGON	dfb5529c9c064317835d30b1422ebf02
27a536fe3b414d6fbe1390c77a77c5b5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DMK	\N	\N	MUEANG	\N
27d347cd23274810a65cbd340e96e7bf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SXB	STRASBOURG	\N	ENTZHEIM 	ffd49325645b4cfe90b12af209a6cdd8
284185be96ac409a997a82b8ea3a4311	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LHR	LONDON	\N	HEATHROW	ac5ebc5d80254c1fac59f8cd0170f591
287ced83bb0c4d12bb113d8517c79033	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TIA	TIRANA	\N	TIRANA	e586a48c0c6e49c6a2dbe685dc927a59
28e0ce2eb5664595830a20665aef67d4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NCL	\N	\N	\N	\N
2903d225997c49bba2fff0ab24eea8d1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DBV	DUBROVNIK	\N	DUBROVNIK	42c32392d4d74af390e1652a0962756e
2921c2d7cdd14f50b0a8307f4272f30e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	STL	\N	\N	LAMBERT	\N
29522643429542bc94742559699ee62b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NYC	NEW YORK	\N	\N	cb823aa64f954b3c83aacf75d552dc02
295cab454d1e46cfb4eaa3cda49465c6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BZG	\N	\N	\N	\N
2981812f4efe4dbabf73eb71f247b03c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ARH	ARKHANGELSK	\N	ARKHANGELSK	5ecfeef727294990ac6facf7502b9905
2a298d5b208b47389e9045e1d2a361b5	1	SYSTEM	2010-06-30 14:19:32	\N	\N	UKS	\N	\N	SEVASTOPOL BELBEK	\N
2ae0da727f60491c9c91c879922e778a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VSG	LUHANSK	\N	LUHANSK	c6597a0f59204b00b63fe1c0f121df7e
2b1697bd1e344732a63a673c77c002a1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PLN	\N	\N	\N	\N
2be9634ec0724d798e33bd6c9ce042b4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AXD	\N	\N	\N	\N
2ca8e5a460234a0d9ef30d4b8020b8b3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GDX	MAGADAN	\N	MAGADAN	5ecfeef727294990ac6facf7502b9905
2cf23425abcc4a5e9cb46c01521c9b17	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PER	\N	\N	\N	\N
2d13bdbc727b4b8094d74f36473b4ffa	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OVB	NOVOSIBIRSK	\N	NOVOSIBIRSK	5ecfeef727294990ac6facf7502b9905
2de645685a5e486bba7c0873d1c71b96	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UFA	UFA 	\N	UFA 	5ecfeef727294990ac6facf7502b9905
2f1f2ce60e8f4e93b81a359ac6d91f1c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TUS	\N	\N	L	\N
2fcc1f993c454a5da83c5eebefdc234e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SJO	SAN JOSE	\N	JUAN SANTAMARIA INTL	1cd09ebf05414d3eb88a687cd7229a8f
2fd9a333e6f74089b7e65d98f143e62f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JMK	MYKONOS	\N	MYKONOS	09b013d7b7b047edb047d82d58474adf
2feb5096c38a4bb28187ac182c2ed93a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	COK	\N	\N	\N	\N
30673f7b5c4d4fc5813f6dadcc6442d9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	RMI	RIMINI	\N	MIRAMARE	ba77bb2e374a4850aee10ea489625361
30700e4047574944bbf4337949baf5ac	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OPO	PORTO	\N	PORTO	014c0f454bf247d3a54ce5be2888a5d3
30d31c887e7c45789cde701107a7f541	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MGA	MANAGUA	\N	MANAGUA	d0e4dd833452425cb0a74397a4ba7945
310c32c63fb34f24815c74b885e09dda	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TLV	TEL AVIV	\N	BEN GURION INTL	e053c11fe6834a9b908fce39f0bfbe66
3173024b579e4365aeef84a7cd78abe5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TXL	BERLIN	\N	TEGEL	f5ae6ece20784948ab85e545c81c4f06
317761dbc2d746759d690901a77f15d1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SVO	MOSCOW	\N	SHEREMETYEVO	5ecfeef727294990ac6facf7502b9905
3318b22f6293450eb219fe9f6253e080	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ACC	\N	\N	\N	\N
34e3f044a6464180b77069f213b3f8f6	1	SYSTEM	2010-06-21 17:43:26	\N	\N	SMI	\N	\N	SAMOS	\N
356d9c62cbce47b4ad9f49ff5bbd658a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AMS	AMSTERDAM	\N	SCHIPHOL	b1d662d056274ad6b3e8499f0ead121d
36e81865d450447eb035b49021d5c9bc	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BEG	BELGRADE	\N	BEOGRAD	1694e7f6941f4f499ad64a4b980eec2d
374a2e6fa1c94778a12f3c41b0cdc055	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IKT	IRKUTSK	\N	IRKUTSK	5ecfeef727294990ac6facf7502b9905
3784399cb5584ed3bef2feb44cb2d16a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PLX	SEMIPALATINSK	\N	SEMIPALATINSK	4039977f975746d2bc4191627f8e769f
37cbbc5dbe1f46edbfc098365ecda9d9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CNX	CHIANG MAI	\N	CHIANG MAI INTL	e9e616a02d5d44739e04cfd592703021
3876f9c612b74a63ba409d441ebcfed3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KAN	\N	\N	\N	\N
388688bf581f4968a3d59d4eed03cfc4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LPQ	\N	\N	PRABANG	\N
396083797d934617a437d4ae2ce432f2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NUX	NOVY URENGOY	\N	NOVY URENGOY	5ecfeef727294990ac6facf7502b9905
39adc67442d34dbdad78224199c93cd7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GOA	GENOA	\N	CRISTOFORO COLOMBO	ba77bb2e374a4850aee10ea489625361
39fea157d9a44ce6b8dd7b349398f553	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GRU	GUARULHOS INTL.	\N	GUARULHOS INTL.	cf3ca3bf06c347549e18dc270cd8a50f
3a2b246fe07b4e0f8c9502ddf6be0c8e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MDW	CHICAGO	\N	MIDWAY	cb823aa64f954b3c83aacf75d552dc02
3b40b8cc75014cab9c6303598ffb2267	1	SYSTEM	2011-07-29 11:11:07	\N	\N	LPK	\N	\N	LIPETSK	\N
3b66bb3d31eb4e75a90975e3f3ffde33	1	SYSTEM	2011-03-24 12:34:30	\N	\N	BRS	\N	\N	BRISTOL	\N
3c5118c4ea6643bcb76341530ca411fe	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BOG	BOGOTA	\N	ELDORADO	dfb5529c9c064317835d30b1422ebf02
3ca04ce2e3644d5687ef3f2dcc9e1b4e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MIA	MIAMI	\N	MIAMI INTL	cb823aa64f954b3c83aacf75d552dc02
3d59c46c98ef42f48fa15f28f1dc4e99	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KVD	GYANDZHA	\N	GYANDZHA	7494ca54409e44e0922d910042297847
3d8033af92c840da87a30b7193353e03	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MLE	MALE	\N	MALE	05c4eb7da68741238d707c24944d52f2
3edf33b06ebb4f89bfcd449d50967e2b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KUL	KUALA LUMPUR	\N	INTERNATIONAL	c8ce167e9bdc4d74ae564678a643845a
3f1d7032047c4de88fec11e51d6ae3c9	1	SYSTEM	2010-05-26 11:55:45	\N	\N	LBA	\N	\N	LEEDS	\N
3f21726ac08142b38ef51306f2a6d954	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LGA	NEW YORK	\N	LA GUARDIA	cb823aa64f954b3c83aacf75d552dc02
3f78329154f643ed809144afabffb493	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TBW	\N	\N	\N	\N
3f7fd8a3b8084f3a82cd44c80a5c77ff	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TFS	TENERIFE	\N	TENERIFE SUR REINA SOFI	62118ce1533a40689ca4a01f289f588a
3fabf41aae02428f834e89bd681b8ccf	1	SYSTEM	2011-06-14 12:21:42	\N	\N	UKK	\N	\N	UST KAMENOGORSK	\N
3fce02a8c7644f43b218fa4f371dc670	1	SYSTEM	2010-03-24 00:00:00	\N	\N	STR	STUTTGART	\N	ECHTERDINGEN	f5ae6ece20784948ab85e545c81c4f06
4070ece5e43b416dbaf775194f344c27	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SAH	SANAA	\N	SANAA INTERNATIONAL	ac76d742e7124299ab78120cc1440fd2
41a55c0e0cfb406eba821cca01634fb5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DSM	DES MOINES	\N	DES MOINES	cb823aa64f954b3c83aacf75d552dc02
41dfa4d800e44c218bbee96a64e18e03	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MBJ	MONTEGO BAY	\N	SANGSTER INTL	ada3f225e3cb49f48494e5e9c22151fa
423f51e071c14c9f954ca06b9813cd9b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DRS	DRESDEN	\N	DRESDEN	f5ae6ece20784948ab85e545c81c4f06
42a756abf47d4132a720c47b6913e06f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PMO	PALERMO	\N	PUNTA RAISI	ba77bb2e374a4850aee10ea489625361
433845e807994632b9df6c34a4f729e0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DEN	DENVER	\N	DENVER INTL	cb823aa64f954b3c83aacf75d552dc02
439c5e8c2f084489905d5c65c80f3cab	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VAA	VAASA	\N	VAASA	e1203d77a54b474d8552ac07b9d77e8d
44550261277a4306b6986674d4865695	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LUX	LUXEMBOURG	\N	LUXEMBOURG	812775c789b7433b9ee5bf52b674995a
4480f50a2528463a954500e3447a0084	1	SYSTEM	2011-09-01 15:51:58	\N	\N	TOS	\N	\N	TROMSO	\N
448a97bd33334e7da76ba7890a831af7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PEK	PEKIN	\N	BEIJING	f8630cfeacd14386bc9c1c2168b53346
450741d4148b4740a55dc27507725061	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BHX	\N	\N	\N	\N
4552a87b9de64930a10cf9b0c5455d79	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PWQ	PAVLODAR	\N	PAVLODAR	4039977f975746d2bc4191627f8e769f
4558948aa54e49289c8393034ede6b36	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TYO	TOKYO	\N	\N	1e75098ec9744dd9a85846d3d9b42a93
464f20bc4f7246c588bbb07598e83ffb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IEV	KIEV	\N	ZHULHANY	c6597a0f59204b00b63fe1c0f121df7e
46b95b7cff2d4e658d9af6f9f7713c78	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AKX	AKTYUBINSK	\N	AKTYUBINSK	4039977f975746d2bc4191627f8e769f
46d1516a410a4aa3b1dba569c182ce65	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BKK	BANGKOK	\N	SUVARNABHUMI	e9e616a02d5d44739e04cfd592703021
48312d350e394b469bb5a697593ea76a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OTP	BUCHAREST	\N	OTOPENI INTERNATIONAL	fa312c56668a46359afc0c5cfed20fff
48fa41b95a2845dca530580ce09d0ae6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MEL	MELBOURNE	\N	TULLAMARINE	c5ce29564592499d85714252c8bee03d
491bc444ae4c4f3f83c365303e28d841	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TRS	TRIESTE	\N	RONCHI DEI LEGIONARI	ba77bb2e374a4850aee10ea489625361
49aae95f248c475caa9e2dd01c6f9d10	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DGA	DANGRIGA	\N	DANGRIGA	b0b358771ccc43e6a2d565ff3c050f9b
49c354a8f3074df5baf17d52a4a49740	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NJC	NIZHNEVARTOVSK	\N	NIZHNEVARTOVSK	5ecfeef727294990ac6facf7502b9905
49d0d65ba3a34a35997877a47adbf567	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BOS	BOSTON	\N	LOGAN INTL	cb823aa64f954b3c83aacf75d552dc02
4aa5775d36e04b9582c10400ef862641	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BOD	BORDEAUX	\N	MERIGNAC	ffd49325645b4cfe90b12af209a6cdd8
4ecf8ba84131402aaceac633cd48f91d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PEE	PERM	\N	PERM	5ecfeef727294990ac6facf7502b9905
4ef476eccae14cb4ac4fce7c88434f41	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FRU	BISHKEK	\N	BISHKEK	55981c7060d444058e7c3b1727629475
4f94c21d3412498fa2cc74524adc6a7b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	RUH	RIYADH	\N	KING KHALED INTL	58126c7610404d0f86ed2ab232c65622
4fa49024ac2241e2973e8a352b6792d0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BJM	\N	\N	\N	\N
4fb93d2157c34c7ab3cc55cdea1e26bf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LFW	LOME	\N	LOME	bee575f7d0384d2cab8ed17d3ebac1ca
4fd3447171d8414fb07b4f8487f76e74	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GYD	HEYDAR ALIYEV	\N	HEYDAR ALIYEV	7494ca54409e44e0922d910042297847
50a2077bf1b540b494805176031ed56b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AKL	AUCKLAND 	\N	AUCKLAND INTERNATIONAL	1337b17c54a74bf1a566cfa950887870
50d3ba7333df42259f00cc7a08201710	1	SYSTEM	2010-03-24 00:00:00	\N	\N	RIX	RIGA	\N	RIGA	4be71a368fe84053bd1f8a80e80cafa5
51b04aa08a394e31967103e61f8d12a5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BRE	BREMEN	\N	BREMEN	f5ae6ece20784948ab85e545c81c4f06
520571cdf07f4040a73288bd0a6f53c7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MEX	MEXICO	\N	JUAREZ INTERNATIONAL	18c759527c87482a81bd7378010d2928
52d46b61914648de92cbd337e08a070c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AMD	AHMEDABAD	\N	SARDAR VALLABH B PATE	a8e3c76e0b184a24b334120ce3b8a6ba
534c457fa8f54acab8b57ab399ed500f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TRF	OSLO	\N	SANDEFJORD-TORP	c13361d24a884f34bbad5cd169b60b44
5370d6adaeb944b18b91c6d7ef74b90f	1	SYSTEM	2010-12-08 17:21:20	\N	\N	GMP	\N	\N	SEOUL GIMPO INTL	\N
5383f6f93fe14acba38f6565073e2c7c	1	SYSTEM	2010-12-24 09:05:24	\N	\N	SXF	\N	\N	BERLIN SCHOENEFEL	\N
54c60bc7dc3b4a158b5fc90f53708228	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BDL	\N	\N	\N	\N
567db38134e641eaae4a4603960ce224	1	SYSTEM	2011-04-21 12:38:54	\N	\N	AAL	\N	\N	AALBORG	\N
e45a7d0db872469f9aca98ab4bed56b2	1	SYSTEM	2011-02-17 11:13:09	\N	\N	SDU	\N	\N	RIO JANEIRO SDU	\N
56dc7eaebc2747f18c89ac851f03d4a7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MCT	MUSCAT	\N	MUSCAT	daac39b5d4d5424ab11354c37caed7b6
5732b171abf84946a53622246b50963c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SXM	MAARTEN (ANTILLES)	\N	PRINCE JULIANA	b1d662d056274ad6b3e8499f0ead121d
57a4ce5973ab47b5ae6dd1c7978b4024	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CNS	CAIRNS	\N	CAIRNS	3ce2c010c85846378131c57cd0e8cdca
584fadef2dfe4530b2e5c15542cc9ebc	1	SYSTEM	2011-02-14 17:57:58	\N	\N	CNF	\N	\N	BELO HORIZ  CNF	\N
5911c4fc47464b18bc77a2020353b890	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TIP	TRIPOLI	\N	TRIPOLI INTERNATIONAL	66f07ab3c3f643f5a96661c385d5380d
595ce7219c664b6b89146f990db33856	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PUJ	PUNTA CANA	\N	PUNTA CANA	b2e7dd2bed0e460e85d4636bc93c8c37
5a5e519cefa848d6800b90834ea6365a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DOK	DONETSK	\N	DONETSK	c6597a0f59204b00b63fe1c0f121df7e
5acb0a83333b462caa4b223ab265d4d2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VRN	VERONA	\N	VILLAFRANCA	ba77bb2e374a4850aee10ea489625361
5c038d47c0a44935a08d096db3b2c572	1	SYSTEM	2010-10-07 16:39:19	\N	\N	CAK	\N	\N	CANTON AKRON	\N
5c4f8a84a49c485b888b7f0aea53f581	1	SYSTEM	2010-03-24 00:00:00	\N	\N	RBA	RABAT	\N	SALE	01a4a3414c6f4933beaf9c38ee8cb2fc
5d2459b31a684c8196bed8173bb7bd01	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CMB	COLOMBO	\N	BANDARANAIKE	d6162bc71e404b1a8b020e65794721cf
5df2251c9f0345a1a3ab48cc0a18979a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BWI	WASHINGTON	\N	BALTIMORE	cb823aa64f954b3c83aacf75d552dc02
5e54cf633732462a9474dca1579a87d4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OMS	OMSK	\N	OMSK	5ecfeef727294990ac6facf7502b9905
5edf2a7b5b48439598348f85c9e7886a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HUY	HUMBERSIDE	\N	HUMBERSIDE	ac5ebc5d80254c1fac59f8cd0170f591
5ee872bbc3ed455db58b655ee99905f7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UUS	YUZHNO SAKHALINSK	\N	YUZHNO SAKHALINSK	5ecfeef727294990ac6facf7502b9905
5f0fe68aa0554504b004aec9a13476c7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HER	IRAKLEION	\N	NIKOS KAZANTZAKIS	09b013d7b7b047edb047d82d58474adf
5f2f08f38c13484da0a43bbb77fa59f8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DAD	\N	\N	NANG	\N
5f39d73e234a4c919b11237a07de75be	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CWL	\N	\N	\N	\N
5fc92286610d40dba330c0b769fe4a79	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MSP	MINNEAPOLIS	\N	ST PAUL INTL	cb823aa64f954b3c83aacf75d552dc02
6058ae17b06249c4a72fdfeb3381012f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OVD	\N	\N	\N	\N
60991ee4264345d0b8035e323fa9bd40	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CAG	CAGLIARI	\N	ELMAS	ba77bb2e374a4850aee10ea489625361
61329ec66a794a3188738066d1a4b5ad	1	SYSTEM	2010-03-24 00:00:00	\N	\N	STW	STAVROPOL	\N	STAVROPOL	5ecfeef727294990ac6facf7502b9905
6242711080934189a3f86899338834d5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SCL	\N	\N	MER	\N
62c6c0777800423c9789fd65bd54be61	1	SYSTEM	2011-07-15 17:56:48	\N	\N	PPT	\N	\N	PAPEETE	\N
6323f1d5bfd74931880e34edb1ab932e	1	SYSTEM	2010-08-21 14:50:52	\N	\N	HTY	\N	\N	HATAY	\N
642ff73a689d49718cbde8c9683b8c5e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AGP	MALAGA	\N	MALAGA	62118ce1533a40689ca4a01f289f588a
64b98e76d4b14a028d11ff4a50b5d9c6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CPT	\N	\N	INTER	\N
64bd37f192c94df8806ca8da2d4da2c9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ULN	ULAANBAATAR	\N	BUYANT UHAA	5f0eb9c2f53d46068f94ac48a3e1f9f1
6650e380ced74936b33f87d905ece70b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NAS	\N	\N	INTL	\N
6724dd7a60a44869b52535daab5e7614	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GLA	GLASGOW	\N	GLASGOW INTL	ac5ebc5d80254c1fac59f8cd0170f591
67373005e4ae492cb52b9603885b2641	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VAR	VARNA	\N	VARNA	10342e32734f4c069a94a05fa61c3d7c
67386c73c40d43d5b90e2f26069599d3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AMI	\N	\N	\N	\N
676eae1baeff45848bef0a05dce60f20	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CLT	CHARLOTTE	\N	DOUGLAS	cb823aa64f954b3c83aacf75d552dc02
67c9b454428b42188f2d777ca6c38650	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SYX	\N	\N	\N	\N
681e15426f4a46ffb9cb689d87d810a5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LAS	LAS VEGAS	\N	MCCARRAN INTL	cb823aa64f954b3c83aacf75d552dc02
69e56a7fd596488482ebfd8f6b818714	1	SYSTEM	2010-10-19 12:57:22	\N	\N	SJJ	\N	\N	SARAJEVO INTL	\N
6a296355766e4dfebf0e10e8d8401610	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HAV	HAVANA	\N	JOSE MARTI INTL	9ef764499dbe451bb410df391d5ec71c
6ad56c05def6408d9ef226ce121def38	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OUA	OUAGADOUGOU	\N	OUAGADOUGOU	e8759daacbb447018784c70e03c3b53b
6b097f50ff1f4885be7a41ee121198a8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HEL	HELSINKI	\N	VANTAA	e1203d77a54b474d8552ac07b9d77e8d
6b326f6a1cea40baaa59b1bc6eb5a152	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IAD	WASHINGTON	\N	DULLES INTERNATIONAL	cb823aa64f954b3c83aacf75d552dc02
6b331d58c8ae4e578a0b89a164dc565f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MIL	MILAN	\N	\N	ba77bb2e374a4850aee10ea489625361
6b40d644c1cf43c7a794315625069da0	1	SYSTEM	2011-04-26 13:41:24	\N	\N	NGO	\N	\N	NAGOYA CHUBU	\N
6b5259c36a2445b6947ddff15e471f7d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LGW	LONDON	\N	GATWICK	ac5ebc5d80254c1fac59f8cd0170f591
6b663ad0490e4d85ac5309907bf2124a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ROV	ROSTOV	\N	ROSTOV	5ecfeef727294990ac6facf7502b9905
6cb91e29932442a492ef2ca1392ac288	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LED	ST PETERSBURG	\N	PULKOVO	5ecfeef727294990ac6facf7502b9905
6d8e92bb9f2f4810ae757471e8c473d8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FLR	FLORENCE	\N	AMERIGO VESPUCCI	ba77bb2e374a4850aee10ea489625361
6eccefe51be141f18468999871781179	1	SYSTEM	2011-02-02 15:37:01	\N	\N	SLZ	\N	\N	SAO LUIZ	\N
6ed993602fe043098ca05a4b93f1cd41	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HMA	KHANTY-MANSIYSK	\N	KHANTY-MANSIYSK	5ecfeef727294990ac6facf7502b9905
6f62f82ec5a243f6a8275289f39ed81a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KHI	KARACHI	\N	QUAID-E-AZAM INTL	6f845ca9a679442d8d5ee6b64f612dc2
70d004ff32af4e1f982057fec46ffed2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DXB	DUBAI	\N	DUBAI	a0b8dadf3b344cbe8542c52c6db4da99
710b94c9e22e45b99e7992957014befb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SGC	SURGUT	\N	SURGUT	5ecfeef727294990ac6facf7502b9905
718d4da096e04f12b2bbc600cb475dec	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MHP	\N	\N	INT	\N
72c16e56d1fc47e9aa4cbad6a4b5ddd9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KHV	KHABAROVSK	\N	NOVYY	5ecfeef727294990ac6facf7502b9905
74d6965f75c84546a45791b04128df43	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IBZ	IBIZA TOWN 	\N	IBIZA	62118ce1533a40689ca4a01f289f588a
75040df028e24ea588e40618e71f08e5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UDJ	UZHGOROD	\N	UZHGOROD	c6597a0f59204b00b63fe1c0f121df7e
750f9c6103f64a44a9b6ebb4b317771d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SKG	THESSALONIKI	\N	MAKEDONIA	09b013d7b7b047edb047d82d58474adf
75ad081e49b34cd5839b603e261d1ae4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KEF	REYKJAVIK	\N	REYKJAVIK KEFLAVIK INTL	dc233d6caadb417aa232abf97a1eeaa5
75b6bbfe2da94d649cadb9070ff08fde	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DNK	DNEPROPETROVSK	\N	DNEPROPETROVSK	c6597a0f59204b00b63fe1c0f121df7e
75d22d9bdc7b41f1a9bf25d93fbf5155	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NOZ	NOVOKUZNETSK	\N	NOVOKUZNETSK	5ecfeef727294990ac6facf7502b9905
75f196c152204e13b5823cbb764ed9b5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DOH	DOHA	\N	DOHA	9e2c99f915eb41448134331510819ce8
763f3ed5c24c49cbaed593d9c8a47451	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ORY	PARIS	\N	ORLY	ffd49325645b4cfe90b12af209a6cdd8
76725287847240f194d77b89b5133f6d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	REN	\N	\N	\N	\N
76b59436f9d5483799db1d4d6092fa31	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BAH	\N	\N	\N	\N
76fc5ce57e0b47a08900443d85fbf793	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SYD	SYDNEY	\N	KINGSFORD SMITH	c5ce29564592499d85714252c8bee03d
771945c86eae4c3aaa3979235ca180ad	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NBO	NAIROBI	\N	JOMO KENYATTA INT	85e4737e6c684af488f3ad36ddb2c76f
771c02533efd465a847d26fae32e04bb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	EGO	Belgorod	\N	Belgorod	5ecfeef727294990ac6facf7502b9905
77376aa403754543ab4960ca9e2880d6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NDJ	NDJAMENA	\N	NDJAMENA	2f9b06b7aa9d4e208b5f0c7ee3e0c702
77f16fcdcb13441fbb165aedda76277e	1	SYSTEM	2010-07-19 17:35:13	\N	\N	AJA	\N	\N	AJACCIO	\N
782e7dc4de7e4675b6240d6210417214	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KJA	KRASNOJARSK	\N	KRASNOJARSK	5ecfeef727294990ac6facf7502b9905
783fd8e401a1456d9c3d6798d68c73ac	1	SYSTEM	2010-04-23 08:26:58	\N	\N	MAA	\N	\N	CHENNAI	\N
78a75fec3760484cb6432d1b1e1b7611	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BLR	BANGALORE	\N	BANGALORE	a8e3c76e0b184a24b334120ce3b8a6ba
7955e58428114b18bdbc1d9cc9e21a65	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SNN	\N	\N	\N	\N
7964aff46564452cbb77bd570d09e3fd	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TJM	TYUMEN	\N	TYUMEN	5ecfeef727294990ac6facf7502b9905
7b7856f18eb14a4cae73633a1d987bb8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TGD	PODGORICA	\N	GOLUBOVCI	6c523458ac204ed28a1740eec6390bbc
7b8e7ce1dbd840129832118ccc4d0652	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ALP	\N	\N	\N	\N
7ce07ac3f1f745ff89ea3b0ab07b2414	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TIV	TIVAT	\N	TIVAT	6c523458ac204ed28a1740eec6390bbc
7d3195f034a2407d9d586931d321deb8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GDN	GDANSK	\N	LECH WALESA	ca502fc306fa42bba100929aeeebbde0
7df1aa28b42b4bd0940ec064a076e0b9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ORD	CHICAGO	\N	O HARE INTERNATIONAL	cb823aa64f954b3c83aacf75d552dc02
7e26bbf2224243ee9da55c32846c90d3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SAW	\N	\N	\N	\N
7efac8eec61a4319a7cd02bbcd866bf7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GUA	\N	\N	CITY	\N
7f0986713ca9487eb3816829b8dc8499	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KHH	\N	\N	\N	\N
7fb9df4475d64aaba781c6b3f0675b8b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KRK	KRAKOW	\N	JOHN PAUL II - BALICE	ca502fc306fa42bba100929aeeebbde0
8080188386884dce88d7584083051d78	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IGU	\N	\N	FALLS	\N
82834db9c7314aa68bb3e3e2506d5982	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MQF	\N	\N	\N	\N
82cf664aa2994a5b9de7f479a73134f7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BFS	BELFAST	\N	BELFAST INTERNATIONAL	ac5ebc5d80254c1fac59f8cd0170f591
83c7fbd53af4415fae2ed4699a03236f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FMO	MUENSTER/OSNABRUECK	\N	INTERNATIONAL	f5ae6ece20784948ab85e545c81c4f06
84ceb217cd6c4ac4b85e2fd20685db07	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BRI	BARI	\N	PALESE	ba77bb2e374a4850aee10ea489625361
8520ea849fc2481d86d4b78bdff488f3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FLL	MIAMI	\N	FORT LAUDERDALE	cb823aa64f954b3c83aacf75d552dc02
8533c4081e7e44f1a2d91ec422e20633	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AUH	ABU DHABI	\N	ABU DHABI INTERNATIONAL	a0b8dadf3b344cbe8542c52c6db4da99
86100b18f90e421e90be28340ce69335	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ZRH	ZURICH	\N	ZURICH AIRPORT	0eb799259bc74a63abf4a82d7ef41ce7
8694e048d45945eea75f0a317335af25	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SAN	SAN DIEGO	\N	SAN DIEGO INTL	cb823aa64f954b3c83aacf75d552dc02
88328010b40f429c8ae743dde782b9d3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PUY	PULA	\N	PULA	42c32392d4d74af390e1652a0962756e
88a3c77bc7fb4504acbec70937757973	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JFK	NEW YORK	\N	JOHN F KENNEDY INTL	cb823aa64f954b3c83aacf75d552dc02
890ceaf7fc4d4c89beadb471a8baec43	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PAP	PORT AU PRINCE	\N	MAIS GATE	a8c9ac4ab9444a99bedbdf75146c5391
89c14d3ec2484c77abb8edbd81640b16	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BMA	STOCKHOLM	\N	BROMMA	f95a221cc315402e86c5345587c54aa4
8a77241d7bb545f19ea414a4a6bf4204	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SXR	SRINAGAR	\N	SRINAGAR	a8e3c76e0b184a24b334120ce3b8a6ba
8a91ad0e98b5462c97f96f88610029a4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MGW	\N	\N	\N	\N
8abe7f9751bb43f78c14ed5226485a83	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OLB	OLBIA	\N	COSTA SMERALDA	ba77bb2e374a4850aee10ea489625361
8adf219198bd41b0b27bf3d00a0d5c67	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ADL	ADELAIDE	\N	SA-ADELAIDE INTL	c5ce29564592499d85714252c8bee03d
8bee5308e57c4da5b39b81b73fa27d5d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SSG	MALABO	\N	SANTA ISABEL	5ecd386d9d4240d190ed30781c4ce431
8cafc24266764c65900940b749273369	1	SYSTEM	2010-03-24 00:00:00	\N	\N	YUL	MONTREAL (QUEBEC)	\N	PIERRE-ELLIOTT-TRUDEAU	4fa75470195843bca3fed516645b6e32
8cebe88a8b3a4297856e031dc954db5e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GOT	GOTHENBURG	\N	LANDVETTER	f95a221cc315402e86c5345587c54aa4
8cec4ce22ee14f96891f469b3af236e1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KZN	KAZAN	\N	KAZAN	5ecfeef727294990ac6facf7502b9905
8d9a9b7cf5954dccb486ebb842f7c533	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BRU	BRUSSELS	\N	BRUSSELS	042a41b9b4614f7b84db525a60e225dd
8e7c6a62d1c84283b2a1a6ef889cbeeb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DTT	DETROIT	\N	\N	cb823aa64f954b3c83aacf75d552dc02
8f49568762cb49829374f9e5a4c6969f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HAM	HAMBURG	\N	FUHLSBUETTEL	f5ae6ece20784948ab85e545c81c4f06
904aa0ff0b3a4b96afd1b44e7ff31243	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MMK	MURMANSK	\N	MURMANSK	5ecfeef727294990ac6facf7502b9905
90e815d319804289a7978f4f5a015d94	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TOX	TOBOLSK 	\N	TOBOLSK 	5ecfeef727294990ac6facf7502b9905
91b0c5ca2885498f881794f0b6052598	1	SYSTEM	2011-03-09 14:25:36	\N	\N	GRZ	\N	\N	GRAZ	\N
91c4af113cc94b85b4d23884c184d5c8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BHD	BELFAST	\N	BELFAST CITY	ac5ebc5d80254c1fac59f8cd0170f591
92046552f25148fd857e5683e2554a63	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UUD	ULAN UDE	\N	ULAN UDE	5ecfeef727294990ac6facf7502b9905
920fb83e6f16485aafda11618384198c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JNB	JOHANNESBURG	\N	O.R. TAMBO ARPT	186baa1734204ba48b4c86bdf779e035
926dbdf233c947be88b3b84a8313daec	1	SYSTEM	2010-12-08 17:21:20	\N	\N	KPO	\N	\N	POHANG	\N
92d2b259be4744caa8f95be499954def	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KAO	KUUSAMO	\N	KUUSAMO	e1203d77a54b474d8552ac07b9d77e8d
92d56488f494447f88df058e27d6b7aa	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TSR	TIMISOARA	\N	TIMISOARA	fa312c56668a46359afc0c5cfed20fff
93bc950fa86146dbaf44cb7a11de0d07	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MNL	MANILA	\N	NINOY AQUINO INTL	138c9da840a74e04952de9b2035b8884
93feacdad43b44c4aa3bc06296c6c15a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GZT	\N	\N	\N	\N
93ff33aafe864b0c8936d912cc396b66	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NRT	TOKYO	\N	 NARITA	1e75098ec9744dd9a85846d3d9b42a93
943097778a1842adb7985138058c1d94	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HRK	KHARKOV	\N	KHARKOV	c6597a0f59204b00b63fe1c0f121df7e
944c506e486a48adb744b6be09574894	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LPA	LAS PALMAS	\N	ARPT DE GRAN CANARIA 	62118ce1533a40689ca4a01f289f588a
953a5763fd304872b070dd1d76586c4d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TPE	\N	\N	\N	\N
956a7592d1cc4ec49a571592e95cd8ab	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LUG	LUGANO 	\N	LUGANO 	0eb799259bc74a63abf4a82d7ef41ce7
961206a5686a4c34b3e43bb49f13e2b6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KLU	\N	\N	\N	\N
9666062b51144c7f949d88be2e217de0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AOI	ANCONA	\N	FALCONARA	ba77bb2e374a4850aee10ea489625361
9748861db0194a4eb5aefa20340975d5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HRE	\N	\N	\N	\N
97aa270c5b5b4bc283746abca937a884	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OSL	OSLO	\N	OSLO AIRPORT	c13361d24a884f34bbad5cd169b60b44
97d74a7125a348819a7124f537c59804	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CAI	CAIRO	\N	CAIRO INTL	8b0698f993504bcbaf3bf5d3348d8dde
9853310f995a4ea1a427b2feee663b30	1	SYSTEM	2010-03-24 00:00:00	\N	\N	EBB	ENTEBBE	\N	ENTEBBE	bdb7ada13af94bca90f88bd798094a75
98581801be8248d88a5bd17cf1ccda1e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SCW	SYKTYVKAR	\N	SYKTYVKAR	5ecfeef727294990ac6facf7502b9905
99c024af3df241ce9558e94049e28c06	1	SYSTEM	2011-05-16 10:32:50	\N	\N	WUH	\N	\N	WUHAN	\N
9a057d11e4364aaebb7604f2931e8584	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SVX	EKATERINBURG	\N	EKATERINBURG	5ecfeef727294990ac6facf7502b9905
9a35f1c76d2445009340e8202cafefcf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CRW	\N	\N	\N	\N
9a66ac7e61d64b3c91d63099d10b5791	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LEJ	\N	\N	\N	\N
9a8ace87bbc4445fb039c2a6e1f37235	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ABJ	ABIDJAN	\N	FELIX HOUPHOUET BOIGNY	b6002aae3ea5461cb0d43106e08d4fc2
9a9dbfee0d8746a095e50d5427641435	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VVO	VLADIVOSTOK 	\N	VLADIVOSTOK 	5ecfeef727294990ac6facf7502b9905
9ad39627513b4c0a8e2daa1b720da160	1	SYSTEM	2010-08-18 17:01:06	\N	\N	HGH	\N	\N	HANGZHOU	\N
9b455906465b416c85b9c1d30e80fed7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VNO	VILNIUS	\N	VILNIUS	44c9a4d7bc1449bfa2b17a43a0e23033
9ba6d0bd0c59491eaee70cf010eebc00	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BOI	BOISE	\N	AIR TERM GOWEN FLD	cb823aa64f954b3c83aacf75d552dc02
9becfefe2212462ea36f3f4a97292d6e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IFO	IVANO-FRANKOV	\N	IVANO-FRANKOV	c6597a0f59204b00b63fe1c0f121df7e
9bf61692245141339ae90c964f7f1320	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CDG	PARIS	\N	CHARLES DE GAULLE	ffd49325645b4cfe90b12af209a6cdd8
9fb60cec695a4e9386cd3dcd8dd63dd2	1	SYSTEM	2010-07-05 14:07:25	\N	\N	BIA	\N	\N	BASTIA	\N
9fff195c22cb4e788442aaac8588a1af	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BLQ	BOLOGNA	\N	GUGLIELMO MARCONI	ba77bb2e374a4850aee10ea489625361
a0846230abed470c8aa4f222c53d6ddd	1		2010-11-17 16:54:30	\N	\N	NLV	NLV	\N	\N	\N
a09f4b91ca2a447fa580f1cf1323d5e2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PRG	PRAGUE	\N	RUZYNE	a6dcb60df39042c1842075577ce31b24
a23fd60789ae420f9fb6f97d2bb68467	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DEL	DELHI	\N	INDIRA GANDHI INTL	a8e3c76e0b184a24b334120ce3b8a6ba
a254366808b64498ae17148fbae36dcf	1	SYSTEM	2011-09-28 15:22:47	\N	\N	DFW	\N	\N	DALLAS FORT WORTH	\N
a296138b90d046ba805d4ebce035c2ea	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ZYR	\N	\N	STN	\N
a2b58b3565734c8e8bed1ceb9a69b52d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SCN	\N	\N	\N	\N
a382535fc4784d89ac61d02d8e577b00	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ORL	ORLANDO	\N	HERNDON	cb823aa64f954b3c83aacf75d552dc02
a42ab0fe093b43278a5262fa4d840d9a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AHO	\N	\N	\N	\N
a477d125d5d0419a991c4d8be11d6a8e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CAN	GUANGZHOU	\N	GUANGZHOU	f8630cfeacd14386bc9c1c2168b53346
a4930d672f33409f9d211028fe0b87d1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BAK	BAKU	\N	HEYDAR ALIYEV INTL	7494ca54409e44e0922d910042297847
a4ac90881c65479a9ba0f3a22a902d0c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FCO	ROME	\N	FIUMICINO	ba77bb2e374a4850aee10ea489625361
a4e5fd59362e46e7a8cb690fc6d9b44b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PDX	PORTLAND	\N	PORTLAND	cb823aa64f954b3c83aacf75d552dc02
a4f9a1883af245d0bcc9e5279e309c2c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PIT	\N	\N	PIT	\N
a5781eba678440f18ee9c29b794d6aa8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	RGN	MINGALADON	\N	YANGON	c7e6b1f32e1a4d78abbdd80bb77295f6
a618b226dc5a4e2f89f7b8c46774e894	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TOF	TOMSK	\N	TOMSK	5ecfeef727294990ac6facf7502b9905
a6cfa924135a445d922a5e223c2842aa	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TSE	ASTANA	\N	ASTANA	4039977f975746d2bc4191627f8e769f
a714cbbeb54f4e4b9816a319338ed502	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DLC	DALIAN	\N	DALIAN	f8630cfeacd14386bc9c1c2168b53346
a72b339fb69e4933a8e792d23e703085	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LON	LONDON	\N	\N	ac5ebc5d80254c1fac59f8cd0170f591
a7a9433875834499be853fe5d9b7ee73	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IST	ISTANBUL	\N	ATATURK	972fe95d3b8e4745bcb2697ee9fc0ef5
a85004fd77904c3bb6ad6aac5da7bac8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	YTO	\N	\N	\N	\N
a86e6507297a4c7abeb2052d10d987c3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SEA	SEATTLE (WASHINGTON)	\N	SEATTLE/TACOMA INTL	cb823aa64f954b3c83aacf75d552dc02
a878756dd4f54804a0695862d085aee4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FNC	FUNCHAL	\N	FUNCHAL	014c0f454bf247d3a54ce5be2888a5d3
a9076d9ec9004116b0ae5d2370054660	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ZAG	ZAGREB	\N	ZAGREB	42c32392d4d74af390e1652a0962756e
a91a94cb44f243b2adc08c296189fb11	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HRG	HURGHADA	\N	HURGHADA	8b0698f993504bcbaf3bf5d3348d8dde
a9c2f9b928f2471c8876da5f9619fa43	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CUZ	\N	\N	\N	\N
aa3005099d0f420fb4e289187448dc84	1	SYSTEM	2010-06-29 14:46:39	\N	\N	VBY	\N	\N	VISBY	\N
aaa1aaa8669c4acaac5ab8ab6e2c86f8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BUD	BUDAPEST	\N	FERIHEGY	fc62fec2835e483ca9ff66b3bc828ca1
ab754fe9786841649dccc219c5b66ec9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PKC	PETROPAVLOVSK-KAMCHATSKIY	\N	PETROPAVLOVSK-KAMCHATSKIY	5ecfeef727294990ac6facf7502b9905
abba8c6884e64f5e920d9be8a98195d0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CEK	CHELYABINSK	\N	CHELYABINSK	5ecfeef727294990ac6facf7502b9905
abddac580cc34e7fab8e9e7ec13e2b53	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PHX	PHOENIX (ARIZONA)	\N	SKY HARBOR INTL	cb823aa64f954b3c83aacf75d552dc02
abec8f8568ad4bb5817abc350b8d17b9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NGB	NINGBO	\N	NINGBO	f8630cfeacd14386bc9c1c2168b53346
abf330b1eda549ae8ec308c73ee070d8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LAX	LOS ANGELES 	\N	LOS ANGELES INTL	cb823aa64f954b3c83aacf75d552dc02
ac6c15364f3846e18f8e2b0a18ece6c6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SIN	SINGAPORE	\N	CHANGI	eb90d5238bd04601a24e104b000e6ef4
adb70b919d044ce7a287856eee6f0f97	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ATH	ATHENS	\N	ATHENS	09b013d7b7b047edb047d82d58474adf
ae2771852bd6437fb7ba6271e6c0ad9d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	YKS	YAKUTSK	\N	YAKUTSK	5ecfeef727294990ac6facf7502b9905
ae83af655e714790a1807a3b0a5690e9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AAQ	ANAPA	\N	ANAPA	5ecfeef727294990ac6facf7502b9905
ae919a64c0cf4cd08489b489113cf762	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MCX	\N	\N	\N	\N
af63da30ec8e47dc865f7589e5181ecd	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ANU	ANTIGUA	\N	VC BIRD INTLERNATIONAL	de42af2ae39e441690729b2442ba8ca4
b07badd83288436e8701674b7612595e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MAO	\N	\N	\N	\N
b1081d813dc74f2ab3734f302b860e7f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TAS	TASHKENT	\N	TASHKENT	9dcfccbd69504bc2863a6f6f7a274ae1
b16437f3b1d944968703e6c248b902b8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SCQ	\N	\N	\N	\N
b262998901b4439c8056c06f37133d06	1	SYSTEM	2010-06-11 16:10:15	\N	\N	KRN	\N	\N	KIRUNA	\N
b26fae873a4042b8acd96f77b94c5382	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SOF	SOFIA	\N	SOFIA	10342e32734f4c069a94a05fa61c3d7c
b35caa1ee5824933b298d35c58a0a0c3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	YWG	WINNIPEG (MANITOBA)	\N	YWG/WINNIPEG	4fa75470195843bca3fed516645b6e32
b3b48d9eb6214c52a8c5ad591af3bf8c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UGC	URGENCH	\N	URGENCH	9dcfccbd69504bc2863a6f6f7a274ae1
b40c6cd5955e4b76a94eafa4f7c90c6d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LCA	LARNACA	\N	LARNACA	05923023b058422aa719e64c6d687d54
b46d27f05a2043a780400350b149684e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HAJ	HANNOVER	\N	HANNOVER AIRPORT	f5ae6ece20784948ab85e545c81c4f06
b49e86bb579346559ca23d547959d0fb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KGF	KARAGANDA	\N	KARAGANDA	4039977f975746d2bc4191627f8e769f
b4b9a33d099e46719e0d4aaa8917a255	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SLC	SALT LAKE CITY (UTAH)	\N	SALT LAKE CITY INTERNATIONAL	cb823aa64f954b3c83aacf75d552dc02
b4e7d90e5c004a178fe22dd9293f608d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	EDI	EDINBURGH	\N	EDINBURGH	ac5ebc5d80254c1fac59f8cd0170f591
b5526cf021cf4944aa34e8a9f55215d9	1	SYSTEM	2011-02-17 11:13:08	\N	\N	BSB	\N	\N	BRASILIA	\N
b56c4b04121d44e093a39e12e8af368e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	YYC	CALGARY (ALBERTA)	\N	CALGARY	4fa75470195843bca3fed516645b6e32
b58ea86944bc40c2a2bd2ffcb7750264	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MAN	MANCHESTER	\N	MANCHESTER INT	ac5ebc5d80254c1fac59f8cd0170f591
b5ed770bf19741768770f11167275a8b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SZZ	SZCZECIN	\N	GOLENIOW	ca502fc306fa42bba100929aeeebbde0
b5f061a2447f4eaebf98cb8e7258f05e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CMN	CASABLANKA	\N	MOHAMED	01a4a3414c6f4933beaf9c38ee8cb2fc
b6106cc490e942bcb71264682c7f1a22	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AER	SOCHI	\N	ADLER-SOCHI	5ecfeef727294990ac6facf7502b9905
b6750fac271047649aa7bb27014f3e78	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CTA	CATANIA	\N	FONTANAROSSA 	ba77bb2e374a4850aee10ea489625361
b68d0cda36c046ce9367ac03bbb4b53e	1	SYSTEM	2010-08-18 17:01:06	\N	\N	TAO	\N	\N	QINGDAO	\N
b69d81f9f1ae42f6901a58f611dbbed2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PSA	PISA	\N	GALIELO GALILEI	ba77bb2e374a4850aee10ea489625361
b6b469732d754260b8d7fe67746582cc	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TRN	TURIN	\N	CASELLE	ba77bb2e374a4850aee10ea489625361
b6be1936dfe2410fa92ed867de68d644	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BEY	BEIRUT	\N	BEIRUT INTERNATIONAL	875144ac00ce43779ee47eb9be8ce8a0
b6e9f70022eb49d3b91cfed2f7d4c0cd	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LIM	\N	\N	\N	\N
b7e9df62b2774d40b830c8a5a64ba97f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DAC	DHAKA	\N	ZIA INTERNATIONAL	1db653d97dc44bedb27e9eda4fc45a5a
b9a4feed7e5e486c9c0604dd6bed29e7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CEE	\N	\N	\N	\N
bb0cc7ee138b42c8968844e0f9f7daaf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UIO	QUITO	\N	MARISCAL SUCR	9d827a4d315c438d8d2a9094aeeb5266
bbda947a20aa401787d9597f898d0a8d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BNA	\N	\N	\N	\N
bcbe93e97ce34ff7b3e9808cb0ae276c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CEB	\N	\N	\N	\N
bcdba7bb9a2745819b8550988f24d322	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VCE	VENICE	\N	MARCO POLO	ba77bb2e374a4850aee10ea489625361
bd0e91b26dc3455fb70605be61b8ce23	1	SYSTEM	2011-01-12 15:32:09	\N	\N	DED	\N	\N	DEHRA DUN	\N
bd53f9be3f334116ac2e4d74ef077b05	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GUW	ATYRAU	\N	ATYRAU	4039977f975746d2bc4191627f8e769f
bda8f6bbbdf94e15b3ea94099318ec7e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PMI	PALMA DE MALLORCA	\N	PALMA MALLORCA	62118ce1533a40689ca4a01f289f588a
be695521557847dea2a34d49d5938063	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CPH	COPENHAGEN	\N	KASTRUP	b2620ad7e7084d3195cb5464e683780b
be90c14c98ed495da10457df4550235a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LCY	LONDON	\N	LONDON CITY AIRPORT 	ac5ebc5d80254c1fac59f8cd0170f591
bee9c4201012403ea82c3a09bf36fb8a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KIV	CHISINAU	\N	CHISINAU	c28d0203dd524a7a8bcb9a1716faae10
bfa20fc0c9e34081994a41ca66a1a147	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ALA	ALMATY	\N	ALMATY	4039977f975746d2bc4191627f8e769f
c0423144ca804f249f0254e52603af71	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ACE	LANZAROTE	\N	LANZAROTE	62118ce1533a40689ca4a01f289f588a
c0821966cac14f31938909552f6183cf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LOS	\N	\N	\N	\N
c0f522430161493e887ddae675f81514	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VIE	VIENNA	\N	VIENNA	3ce2c010c85846378131c57cd0e8cdca
c23fb7f418974e57849770d094296144	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KEJ	KEMEROVO	\N	KEMEROVO	5ecfeef727294990ac6facf7502b9905
c302e9604f65439d9ab3a9a993b9f950	1	SYSTEM	2010-03-24 00:00:00	\N	\N	EZE	BUENOS AIRES	\N	MINISTRO PISTARINI	0970d26a42be4a0c80da8177f21bf993
c325aacb3a3a4f5c8bc416ec26b29b9d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BUF	\N	\N	\N	\N
c35caa9559864ab9b4b1f5d7b5ba9c84	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MOW	MOSCOW	\N	\N	5ecfeef727294990ac6facf7502b9905
c46ef879b3804eaa960229e5a34b3933	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UTP	\N	\N	\N	\N
c4adac19c8f149d08ad8d4e8bbc5bce2	1	SYSTEM	2010-06-17 12:36:26	\N	\N	ISB	\N	\N	ISLAMABAD	\N
c50cf264565248a58652627174dd310f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OSR	\N	\N	\N	\N
c5bdbb845f5c41db858ec6d0260b6158	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ESB	ANKARA	\N	ESENBO	972fe95d3b8e4745bcb2697ee9fc0ef5
c5e469aadb9b43b5b7300765e1752424	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BZV	\N	\N	\N	\N
c6009d3e161247cd8e8a49d92351486f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	REP	SIEM REAP	\N	SIEM REAP	0110ef197bc641288ce9f5f4ab2ae51b
c6a1142992024ba5a1355ba523deb05b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ZNZ	\N	\N	\N	\N
c6ba49801cad4ab697cbbd1189926e28	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NBC	NABEREZHNYE CHELNU	\N	NABEREZHNYE CHELNY	5ecfeef727294990ac6facf7502b9905
c711c4de37e249c98271689dc23f94f9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JIK	\N	\N	ISLAND	\N
c83ddecc5f5842d0b4b55ae8fed2da21	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CAS	CASABLANCA	\N	CASABLANCA	01a4a3414c6f4933beaf9c38ee8cb2fc
c8f3a009e24040bb940348aef0b8d6d2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PVG	SHANGHAI	\N	SHANGHAI	f8630cfeacd14386bc9c1c2168b53346
c9408b5b5cf244b08f903cc616f0b667	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SJU	SAN JUAN	\N	LUIS MUNOZ MARIN INTL 	7230c9f7cba841eca72fafe1d30088c2
ca1d2dadcb6b48ceae885f700eaf240a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CWC	CHERNOVTSY	\N	CHERNOVTSY	c6597a0f59204b00b63fe1c0f121df7e
cac5b4040d384f9daf9956ffd4cdafd5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PFO	PAPHOS	\N	INTERNATIONAL	05923023b058422aa719e64c6d687d54
cb81c742e67f4215b6cb547f9110bc3f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SPU	SPLIT	\N	SPLIT	42c32392d4d74af390e1652a0962756e
cbf515c4ac9948b98311ab053ccbb5db	1	SYSTEM	2010-03-24 00:00:00	\N	\N	YYZ	TORONYO	\N	TORONTO INTL	4fa75470195843bca3fed516645b6e32
cccde8fb785c47acb5df3c8482fd9765	1	SYSTEM	2011-04-01 12:25:36	\N	\N	MTY	\N	\N	MONTERREY GEN MAR	\N
ccd4793803d44ba9a69990a5a6b4a508	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TFN	TENERIFE	\N	TENERIFE-NORTE	62118ce1533a40689ca4a01f289f588a
cd2d60aa86484345ac3436f96e897912	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MRS	MARSEILLE	\N	PROVENCE	ffd49325645b4cfe90b12af209a6cdd8
cd40182930e34e58b3c4e0a2a549a54f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TRD	\N	\N	\N	\N
cdcf3ed7422c4879939efca418c82e7e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VLC	VALENCIA	\N	VALENCIA	62118ce1533a40689ca4a01f289f588a
cdf81cfa4b884241b95ae3091510b33f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GSO	GREENSBORO/HIGH POINT	\N	PIEDMONT TRIAD INTL	cb823aa64f954b3c83aacf75d552dc02
ce04bf2fc5a7437a862c0cf1a5e104d0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AMM	AMMAN 	\N	QUEEN ALIA INTL	dcf2a0a2478e4fd691245e360ccc8a60
ce72918d53b54682abdb6b1331528481	1	SYSTEM	2010-03-24 00:00:00	\N	\N	POZ	POZNAN 	\N	LAWICA	ca502fc306fa42bba100929aeeebbde0
cef1d38b10e3467badd214486cbc3ccc	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BSL	BASEL MULHOUSE	\N	BASEL EUROAIRPORT	0eb799259bc74a63abf4a82d7ef41ce7
cfbdb1976f704e618aeeb61288643027	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ADB	IZMIR	\N	ADNAN MENDERES	972fe95d3b8e4745bcb2697ee9fc0ef5
d2cef21e697c45cc8509d30c43179de3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PAR	PARIS	\N	\N	ffd49325645b4cfe90b12af209a6cdd8
d2d8c128fa074e51bb0f01d79ae15a21	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SUB	\N	\N	\N	\N
d47af04fd78b40cf845af61a2160123f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TRI	\N	\N	TENN	\N
d4a55711797b49d3bf7f56ebebdde53f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TCI	\N	\N	\N	\N
d5227d1302e74972ab5835503628f7b2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DCA	WASHINGTON	\N	RONALD REAGAN NATIONAL	cb823aa64f954b3c83aacf75d552dc02
d6090836297c4d829464369d451f7e56	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TLL	TALLINN	\N	TALLINN	b9c97b9076e644bcbfb9960878f7f512
d6168c7de73940b283cb8ac101638914	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NNM	NARYAN-MAR	\N	NARYAN-MAR	5ecfeef727294990ac6facf7502b9905
d622e1f308cd4581922e44c13f581f95	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SHA	SHANGHAI	\N	HONGQIAO INTL	f8630cfeacd14386bc9c1c2168b53346
d6ebdf903f8f411cbb262781ae4db65e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SBZ	SIBIU	\N	SIBIU	fa312c56668a46359afc0c5cfed20fff
d7006017ac7d4b2b83f1bd32a3f4727c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	EVN	EREVAN	\N	EREVAN	64ca530164694215846288128b9fafaa
d7852f82ab2144c2b5e92c11ea7cdac8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	QKL	\N	\N	RAIL	\N
d7b6ec18cc5042d0b22e2b133bcf930c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CKY	CONAKRY	\N	CONAKRY	5f9d06de98a949c6bacc2630ba664ade
d82411e7c8e54475a83f8f5f54c043cc	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LWO	LVIV	\N	SNILOW	c6597a0f59204b00b63fe1c0f121df7e
d825ca8cec814ae6b203c5ff96de13ae	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KGL	\N	\N	\N	\N
d85dc118a1d64bde853d1adeb1bf7160	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DUS	DUSSELDORF	\N	DUSSELDORF	f5ae6ece20784948ab85e545c81c4f06
d875070cfc7f42efa5d80bd0f2fa1966	1	SYSTEM	2010-08-02 18:15:34	\N	\N	DUR	\N	\N	DURBAN KING SHAKA	\N
d8d0a4a2f3cb43c1be3c5dfa1bc4cedb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JED	\N	\N	ABDUL	\N
d953798cbd1b4197a4660e28adafadbf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NCE	NICE	\N	COTE D AZUR	ffd49325645b4cfe90b12af209a6cdd8
da72d931cdb14c3f8e69e8c02c90f403	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BGI	BRIDGETOWN	\N	GRANTLEY ADAMS	7eaf957a19574d65a187571e8e8a7881
daacef2f9eba48f98d802b051e1dcce8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MUC	MUNICH	\N	MUNICH	f5ae6ece20784948ab85e545c81c4f06
dadfaa1a14b5495581ca30a3ca745071	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CGN	COLOGNE	\N	BONN	f5ae6ece20784948ab85e545c81c4f06
daf4cb68c2ce46bc9483ba617605d119	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IVL	IVALO	\N	IVALO	e1203d77a54b474d8552ac07b9d77e8d
db82a529b16449b592494464e75cc12c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LIN	MILAN	\N	LINATE	ba77bb2e374a4850aee10ea489625361
dbaa66880cd84a6585a49aefdfc94891	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BJS	\N	\N	\N	\N
dbc77a6ea2114b579659faf4f9f6c9a7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KIN	KINGSTON	\N	NORMAN MANLEY	ada3f225e3cb49f48494e5e9c22151fa
dc2ea6b3c3e94367ab7b502d3e114af6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VOG	VOLGOGRAD	\N	VOLGOGRAD	5ecfeef727294990ac6facf7502b9905
dc8150f229294fa6b74304ae259412f2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TBS	TBILISI	\N	TBILISI	472d137c8a7c44719fd4c809948fd1c8
dd37301cb8734053b4e9cf0d01c1a3d7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MLA	MALTA	\N	LUQA INTERNATIONAL	9769fe0147e046c98638fe1f62b16400
defbe0ed4f674718aa71cd55421733e1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DAM	DAMASCUS	\N	DAMASCUS	6bf4e60591064e7da13127dca4554d36
dfb15971049b4630a1d265204d10dd93	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DUB	DUBLIN	\N	DUBLIN	2e0dc262782f4f27ae12011f6778a664
e0b97933723941f6b08ad8fd3ebf7ca7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VOZ	\N	\N	\N	\N
e1312a39673c434eb60ab0b278edd350	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SZG	SALZBURG	\N	W.A. MOZART	3ce2c010c85846378131c57cd0e8cdca
e1702f349726495a90d635f27a93a722	1	SYSTEM	2010-10-25 08:34:05	\N	\N	TUL	\N	\N	TULSA INT	\N
e1b6367625a64b5590215b30b93afd53	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ADA	ADANA	\N	ADANA	972fe95d3b8e4745bcb2697ee9fc0ef5
e1fddaee4915405598083de92f95b55a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	STO	STOCKHOLM	\N	\N	f95a221cc315402e86c5345587c54aa4
e218b120d041419a8a831dbe39a4f637	1	SYSTEM	2010-09-13 11:26:51	\N	\N	AGA	\N	\N	AGADIR AL MASSIRA	\N
e29bf00b308b498f99a3a47d6c4f6835	1	SYSTEM	2011-02-17 11:13:08	\N	\N	IMP	\N	\N	IMPERATRIZ	\N
e35fc1f543d044f3951a76edb1a561c2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MEM	MEMPHIS (TENNESSEE)	\N	MEMPHIS INTERNATIONAL	cb823aa64f954b3c83aacf75d552dc02
e38f9d3bd56046d49d87b11450e76537	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LIS	LISBON	\N	LISBON	014c0f454bf247d3a54ce5be2888a5d3
e3f023bffcd24da6abd15c5f3d66c2c3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MRV	MINERALNYE VODY	\N	MINERALNYE VODY	5ecfeef727294990ac6facf7502b9905
e46b1a560db2440a9704258b4a94b148	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ALG	ALGIERS	\N	HOUARI BOUMEDIENE	1d042e208d3d4873acc158313949b5ac
e4cbd10c81f34afd9fd900ead11c74f4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BUS	BATUMI	\N	BATUMI	472d137c8a7c44719fd4c809948fd1c8
e4ed6170747b43a39a416d795be3790d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DTW	DETROIT	\N	DETROIT METRO	cb823aa64f954b3c83aacf75d552dc02
e52c8498cbd74386ab918580e9479c7a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GOI	\N	\N	\N	\N
e52ee1eb5cf04e81870224b8fc85b177	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KTW	\N	\N	\N	\N
e5ee340670d14cf28fe3fcb99dda808f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KRR	KRASNODAR	\N	KRASNODAR	5ecfeef727294990ac6facf7502b9905
e62491e7c07949989d0e8270de8c06b1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OZH	ZAPOROZHYE	\N	ZAPOROZHYE	c6597a0f59204b00b63fe1c0f121df7e
e63dac491d1746ad8114c90808af29d6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CFU	KERKYRA	\N	IOANNIS KAPODISTRIAS	09b013d7b7b047edb047d82d58474adf
e68b802367ac4eeca36ebed23f798070	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IAH	HOUSTON (TEXAS)	\N	GEORGE BUSH INTERCONTL	cb823aa64f954b3c83aacf75d552dc02
e6a23fdca8fc4b6e9f10ad651522acdf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BRN	BERN	\N	BELP	0eb799259bc74a63abf4a82d7ef41ce7
e6c65d9cc7dc4ebb80927d2acb35cf42	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ARN	STOCKHOLM	\N	ARLANDA	f95a221cc315402e86c5345587c54aa4
e7667cf6ae5141afabb16860c05e3b3a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SDQ	SANTO DOMINGO	\N	LAS AMERICAS	b2e7dd2bed0e460e85d4636bc93c8c37
e771956abf51453c82a3f32ace647aeb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BTS	BRATISLAVA	\N	IVANKA	cd849748cde640ca808905a88f686738
e77b1fcadb124ab78186b293b818b9b7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BJV	BODRUM	\N	BODRUM	972fe95d3b8e4745bcb2697ee9fc0ef5
e8086601ce5c4228b96eca290e6a75cc	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SGN	HO CHI MINH CITY	\N	TAN SON NHAT	63f032a619fc4d0083504602d945064b
e8d038b9576f4ad891a7415be49376cd	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PRN	PRISTINA	\N	PRISTINA	1694e7f6941f4f499ad64a4b980eec2d
e94a64eb0ff24c52acffca1ff1a7a6f3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LHE	LAHORE	\N	LAHORE	6f845ca9a679442d8d5ee6b64f612dc2
e9df80614bcc4c73b693ba7309c5171a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BNE	BRISBANE	\N	BRISBANE	c5ce29564592499d85714252c8bee03d
ea008076052349c99afe2a54a1518811	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KSC	Kosice	\N	Barca	cd849748cde640ca808905a88f686738
eb216934460d41c3ab841c078316cd89	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BHK	BUKHARA	\N	BUKHARA	9dcfccbd69504bc2863a6f6f7a274ae1
eb3741de9467405d8a1da909552887af	1	SYSTEM	2010-06-01 16:36:48	\N	\N	CLJ	\N	\N	CLUJ NAPOCA	\N
eb3da8cddb514b1488c96dd486460200	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CGK	\N	\N	INTL	\N
eb6715ab6c644e6fbf76fca1e36fe9b5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	XER	STRASBOURG	\N	STRASBOURG BUS	ffd49325645b4cfe90b12af209a6cdd8
eb7ab6c8013f4f778d38fa751f8f191f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	POP	PUERTO PLATA	\N	LA UNION	b2e7dd2bed0e460e85d4636bc93c8c37
eb81838306944d9191d850202439ca33	1	SYSTEM	2010-03-24 00:00:00	\N	\N	WAW	WARSAW	\N	FREDERIC CHOPIN AIRPORT	ca502fc306fa42bba100929aeeebbde0
ebce01c58ab2411db4140fa939376b41	1	SYSTEM	2010-10-28 12:04:16	\N	\N	RVN	\N	\N	ROVANIEMI	\N
ec14028a41744a078bc869c630a92166	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LYS	LYON	\N	SAINT EXUPERY	ffd49325645b4cfe90b12af209a6cdd8
ecf3e90280db4441aac20d50cec824fb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KGD	KALININGRAD	\N	KALININGRAD	5ecfeef727294990ac6facf7502b9905
ecfa2a3dc54841d5a1b53f586e11c49a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ASB	ASHGABAT	\N	ASHGABAT	3c278243d0094977a57ac4ab89a68cc3
ee9afee9e03f409e88102957248daf50	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LJU	LJUBLJANA	\N	BRNIK	8e6b3ae48e2a4d93a5fbeba9a7a59ad3
ef85c526756e4d9191bc44dd4d9b3016	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MJT	MYTILINI	\N	ODYSSEAS ELYTIS	09b013d7b7b047edb047d82d58474adf
f05cbe5653084ddaa8c0869a0888ba40	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HKG	HONG KONG	\N	HONG KONG	f8630cfeacd14386bc9c1c2168b53346
f106805a79e045c680e9e8a7967d6190	1	SYSTEM	2010-03-24 00:00:00	\N	\N	WRO	WROCLAW	\N	WROCLAW	ca502fc306fa42bba100929aeeebbde0
f15c975559224279880a34cad3c4d674	1	SYSTEM	2010-03-24 00:00:00	\N	\N	USM	\N	\N	SAMUI	\N
f17da6de1d204013b4e2d52f33f08898	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KWI	\N	\N	\N	\N
f1c51c67fbbb45aaa4f48297d28cc5b5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IKA	TEHRAN	\N	IMAM KHOMEINI INTL	9132a6a4c8d7445d9857943e1bb0b6fc
f1ed6af24b484943b9ab43cdcec15273	1	SYSTEM	2010-03-24 00:00:00	\N	\N	YAM	SAULT STE MARIE	\N	SAULT STE MARIE	4fa75470195843bca3fed516645b6e32
f3df50d38df548dabc996cdf09b15a37	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KRT	KHARTOUM	\N	KHARTOUM	e3d3f91fa65c4da09c157c12760fc847
f420def951b143e18eb4bce8ae92685b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NUE	NUREMBERG	\N	NUREMBERG	f5ae6ece20784948ab85e545c81c4f06
f44aa91e325747cea3092ca1dcb3a8d9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OUL	OULU	\N	OULU	e1203d77a54b474d8552ac07b9d77e8d
f4594d60b91547ae8760c80ab4457fd1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MDE	\N	\N	\N	\N
f48ca023ebb54b76be6c5de9b254baa9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FIH	KINSHASA	\N	DJILI	32ed55c6ca054dc6a3662d2f968fb1f1
f4bbcdfc69a0433e83467306af991414	1	SYSTEM	2011-07-01 14:09:04	\N	\N	VKT	\N	\N	VORKUTA	\N
f523649d6a92458dbeb9acc02bb4bb47	1	SYSTEM	2010-03-24 00:00:00	\N	\N	EWR	NEW YORK	\N	NEWARK LIBERTY INTL	cb823aa64f954b3c83aacf75d552dc02
f69899c049474a8eaf159472bb225131	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BOM	MUMBAI	\N	CHHATRAPATI SHIVAJI	a8e3c76e0b184a24b334120ce3b8a6ba
f6ff4ec61b3b4b4bb0673dd03abdeb80	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KBP	KIEV	\N	BORISPOL	c6597a0f59204b00b63fe1c0f121df7e
f9013a47130d40778e6fe8d0d32b0e51	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GVA	GENEVA	\N	GENEVA	0eb799259bc74a63abf4a82d7ef41ce7
f955266e510d4fe9925198069d8a000a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SJD	SAN JOSE CABO	\N	LOS CABOS	18c759527c87482a81bd7378010d2928
f9708a20d2b84e9d853bc5e52c369e0e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BIO	\N	\N	\N	\N
f981d3c3d9674919b10e69ee8bc7d249	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LXR	LUXOR	\N	LUXOR	8b0698f993504bcbaf3bf5d3348d8dde
f9a24612d7f644a584daa7ded83525fa	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TZX	TRABZON	\N	TRABZON	972fe95d3b8e4745bcb2697ee9fc0ef5
fa01a64d028b4cbeb9b06ee7d4d93714	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MRU	MAURITIUS 	\N	MAURITIUS	f79daa9cef8d49438de4b06b3407464e
fa73f734fe7e4673916e7892df5e12ab	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MSQ	MINSK	\N	MINSK 2 INTERNATIONAL	f9868259f8994a8d9a30dc27f9a74274
fae22b8f1a8f4323a954afa2f8a041d5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HNL	HONOLULU (HAWAII)	\N	INTERNATIONAL	cb823aa64f954b3c83aacf75d552dc02
fb536717eb664f0fa1c4fe8216c9e31b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KTM	KATHMANDU	\N	KATHMANDU	f685f5e3d21c4ceb9cc8f3ca9df69672
fba345ab78e84361a94b377996a6721f	1	SYSTEM	2011-07-11 14:15:51	\N	\N	BGW	\N	\N	BAGHDAD BGW	\N
fc1f5068cc084acdae873dff037e04ef	1	SYSTEM	2010-08-31 12:05:35	\N	\N	KGP	KGP	\N	\N	\N
fca8e1f8d40c411b8914c6ed9178b7b1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TLS	TOULOUSE	\N	BLAGNAC	ffd49325645b4cfe90b12af209a6cdd8
fcedf9acc9284d929c17b513e06e75ed	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KIX	\N	\N	INTL	\N
fd76e344793d44db8a981231c1366b6c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LEI	ALMERIA	\N	ALMERIA	62118ce1533a40689ca4a01f289f588a
fdd710c2135249ee88293ec3bed096ce	1	SYSTEM	2011-04-21 11:51:46	\N	\N	ULV	\N	\N	ULYANOVSK	\N
fe2901d417c645159a5bf15fe10648d6	1	SYSTEM	2011-07-12 10:31:31	\N	\N	BQS	\N	\N	BLAGOVESHCHENSK	\N
fe5cd4e73e2e49c0ae0947b8c5e0b548	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BAX	BARNAUL	\N	BARNAUL	5ecfeef727294990ac6facf7502b9905
fe6ddbd756ca43b28557d7ac3b6d1adf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SIP	SIMFEROPOL	\N	SIMFEROPOL	c6597a0f59204b00b63fe1c0f121df7e
ffe4761c377148d4b3503d1f7a451fc0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HAN	HANOI	\N	NOIBAI	63f032a619fc4d0083504602d945064b
86136768c74e4c17b8ee4e2d44b6a25c	1	SYSTEM	2011-10-21 18:46:04	\N	\N	TYN	\N	\N	TAIYUAN	\N
19e25d8d324e44e387986926890bfbc5	1	SYSTEM	2011-10-22 15:10:54	\N	\N	LIR	\N	\N	LIBERIA	\N
4ec531735d164f25aaccd111e36f26c0	1	SYSTEM	2011-10-24 14:41:25	\N	\N	NAN	\N	\N	NADI	\N
cef403e57ae040e6943e01dc22f5554d	1	SYSTEM	2011-11-17 10:56:42	\N	\N	SCO	\N	\N	AKTAU	\N
62d8cf55a9a047ffa5e1e9e2fa9a13da	1	SYSTEM	2011-12-01 14:30:10	\N	\N	ECN	\N	\N	ERCAN	\N
36eb900275a0490f81b46340962fe6d4	2	svetlana.vasilkova	2012-05-30 20:38:13	svetlana.vasilkova	2012-05-30 20:38:49	DLT	For Deletion	Для удаления	Аэропорт для удаления	\N
\.


--
-- Data for Name: lt_avia_document; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_avia_document (id, version, createdby, createdon, modifiedby, modifiedon, type, issuedate, airlineiatacode, airlineprefixcode, airlinename, number_, conjunctionnumbers, isprocessed, isvoid, requiresprocessing, passengername, gdspassportstatus, gdspassport, itinerary, commissionpercent, paymenttype, paymentform, bookeroffice, bookercode, ticketeroffice, ticketercode, originator, origin, pnrcode, airlinepnrcode, tourcode, note, remarks, displaystring, airline, passenger, booker, ticketer, seller, owner, customer, intermediary, order_, originaldocument, fare_amount, fare_currency, equalfare_amount, equalfare_currency, commission_amount, commission_currency, commissiondiscount_amount, commissiondiscount_currency, feestotal_amount, feestotal_currency, vat_amount, vat_currency, total_amount, total_currency, servicefee_amount, servicefee_currency, handling_amount, handling_currency, discount_amount, discount_currency, grandtotal_amount, grandtotal_currency, ticketingiataoffice, isticketerrobot) FROM stdin;
2b48aa82cc5140b699bdfe0fa64adb57	1	admin	2012-05-15 15:56:50	\N	\N	0	2011-12-05	B2	628	BELAVIA	3544667874	\N	f	f	t	SINELNIKOV/VOLODYMYR MR	0	\N	KBP-MSQ-KBP	\N	1	CASH	DOKC32530	7777SV	DOKC32530	7777SV	1	0	2ASQ9P	B2 NWYDRF	\N	\N	\N	628-3544667874	48c384d42f35436f9e1b2daee1371d3c	\N	d52cb22e2e754178afb8184957fbaba8	d52cb22e2e754178afb8184957fbaba8	d52cb22e2e754178afb8184957fbaba8	a354460c7cfc42d0bdf1e7624018905a	\N	\N	\N	777367f7c2b143de8a91214a2ca881a6	185.00000	83a42d146f5a44b2817f7c384d6bc129	1479.00000	af496e4b4de647afa7b61eacfd1a6f4f	1.00000	af496e4b4de647afa7b61eacfd1a6f4f	\N	\N	184.00000	af496e4b4de647afa7b61eacfd1a6f4f	\N	\N	1663.00000	af496e4b4de647afa7b61eacfd1a6f4f	\N	\N	\N	\N	\N	\N	\N	\N	72320942	f
\.


--
-- Data for Name: lt_avia_document_fee; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_avia_document_fee (id, version, createdby, createdon, modifiedby, modifiedon, code, document, amount_amount, amount_currency) FROM stdin;
dfe23c4c67c94f1793d19a7ad8e45857	1	admin	2012-05-15 15:56:50	\N	\N	YK	2b48aa82cc5140b699bdfe0fa64adb57	136.00000	af496e4b4de647afa7b61eacfd1a6f4f
3b1d29a89a48413793264981075cf10e	1	admin	2012-05-15 15:56:50	\N	\N	UD	2b48aa82cc5140b699bdfe0fa64adb57	16.00000	af496e4b4de647afa7b61eacfd1a6f4f
b2b676a31d1c411ca9779bdb18aa4875	1	admin	2012-05-15 15:56:50	\N	\N	UA	2b48aa82cc5140b699bdfe0fa64adb57	32.00000	af496e4b4de647afa7b61eacfd1a6f4f
\.


--
-- Data for Name: lt_avia_document_voiding; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_avia_document_voiding (id, version, createdby, createdon, modifiedby, modifiedon, isvoid, "timestamp", agentoffice, agentcode, document, agent, iataoffice) FROM stdin;
\.


--
-- Data for Name: lt_avia_mco; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_avia_mco (id, reissuefor) FROM stdin;
\.


--
-- Data for Name: lt_avia_refund; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_avia_refund (id, cancelcommissionpercent, refundeddocument, refundservicefee_amount, refundservicefee_currency, servicefeepenalty_amount, servicefeepenalty_currency, cancelfee_amount, cancelfee_currency, cancelcommission_amount, cancelcommission_currency) FROM stdin;
\.


--
-- Data for Name: lt_avia_ticket; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_avia_ticket (id, domestic, interline, segmentclasses, departure, endorsement, reissuefor) FROM stdin;
2b48aa82cc5140b699bdfe0fa64adb57	f	f	V-V	2011-12-06 11:30:00	CHANGING/PENALTIES APPLIES TKT RESTR APPLY	\N
\.


--
-- Data for Name: lt_closed_period; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_closed_period (id, version, createdby, createdon, modifiedby, modifiedon, dateto, datefrom, periodstate) FROM stdin;
\.


--
-- Data for Name: lt_consignment; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_consignment (id, version, createdby, createdon, modifiedby, modifiedon, number_, issuedate, totalsupplied, supplier, acquirer, grandtotal_amount, grandtotal_currency, vat_amount, vat_currency, discount_amount, discount_currency) FROM stdin;
\.


--
-- Data for Name: lt_country; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_country (id, version, createdby, createdon, modifiedby, modifiedon, name, twocharcode, threecharcode) FROM stdin;
0110ef197bc641288ce9f5f4ab2ae51b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CAMBODIA	KH	KHM
014c0f454bf247d3a54ce5be2888a5d3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PORTUGAL	PT	PRT
01a4a3414c6f4933beaf9c38ee8cb2fc	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MOROCCO	MA	MAR
042a41b9b4614f7b84db525a60e225dd	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BELGIUM	BE	BEL
05923023b058422aa719e64c6d687d54	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CYPRUS	CY	CYP
05c4eb7da68741238d707c24944d52f2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MALDIVES ISLAND	MV	MDV
0970d26a42be4a0c80da8177f21bf993	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ARGENTINA	AR	ARG
09b013d7b7b047edb047d82d58474adf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GREECE	GR	GRC
0eb799259bc74a63abf4a82d7ef41ce7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SWITZERLAND	CH	CHE
10342e32734f4c069a94a05fa61c3d7c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BULGARIA	BG	BGR
11b3d912caac4d94babf84248333c4dc	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TUNISIA	TN	TUN
1337b17c54a74bf1a566cfa950887870	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NEW ZEALAND	NZ	NZL
138c9da840a74e04952de9b2035b8884	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PHILIPPINES	PH	PHL
13c21eebeddf47c9bd5a514693c8d864	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MACEDONIA	MK	MKD
1694e7f6941f4f499ad64a4b980eec2d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SERBIA	RS	SRB
186baa1734204ba48b4c86bdf779e035	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SOUTH AFRICA	ZA	ZAF
18c759527c87482a81bd7378010d2928	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MEXICO	MX	MEX
1cd09ebf05414d3eb88a687cd7229a8f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	COSTA RICA	CR	CRI
1d042e208d3d4873acc158313949b5ac	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ALGERIA	DZ	DZA
1db653d97dc44bedb27e9eda4fc45a5a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BANGLADESH	BD	BGD
1e75098ec9744dd9a85846d3d9b42a93	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JAPAN	JP	JPN
21f596351b424abb993bf46ea23a6135	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TAJIKISTAN	TJ	TJK
2e0dc262782f4f27ae12011f6778a664	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IRELAND	IE	IRL
2f9b06b7aa9d4e208b5f0c7ee3e0c702	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CHAD	TD	TCD
32ed55c6ca054dc6a3662d2f968fb1f1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CONGO THE DEMOCRATIC REPUBLIC	CD	COD
3c278243d0094977a57ac4ab89a68cc3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TURKMENISTAN	TM	TKM
3ce2c010c85846378131c57cd0e8cdca	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AUSTRIA	AT	AUT
4039977f975746d2bc4191627f8e769f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KAZAKHSTAN	KZ	KAZ
42c32392d4d74af390e1652a0962756e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CROATIA	HR	HRV
44c9a4d7bc1449bfa2b17a43a0e23033	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LITHUANIA	LT	LTU
472d137c8a7c44719fd4c809948fd1c8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GEORGIA	GE	GEO
4be71a368fe84053bd1f8a80e80cafa5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LATVIA	LV	LVA
4fa75470195843bca3fed516645b6e32	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CANADA	CA	CAN
55981c7060d444058e7c3b1727629475	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KYRGYZSTAN	KG	KGZ
58126c7610404d0f86ed2ab232c65622	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SAUDI ARABIA	SA	SAU
5ecd386d9d4240d190ed30781c4ce431	1	SYSTEM	2010-03-24 00:00:00	\N	\N	EQUATORIAL GUINEA	GQ	GNQ
5ecfeef727294990ac6facf7502b9905	1	SYSTEM	2010-03-24 00:00:00	\N	\N	RUSSIA	RU	RUS
5f0eb9c2f53d46068f94ac48a3e1f9f1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MONGOLIA	MN	MNG
5f9d06de98a949c6bacc2630ba664ade	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GUINEA	GN	GIN
62118ce1533a40689ca4a01f289f588a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SPAIN	ES	ESP
63f032a619fc4d0083504602d945064b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	VIETNAM	VN	VNM
64ca530164694215846288128b9fafaa	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ARMENIA	AM	ARM
66f07ab3c3f643f5a96661c385d5380d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LIBYAN ARAB JAMAHIRIYA	LY	LBY
6bf4e60591064e7da13127dca4554d36	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SYRIA	SY	SYR
6c523458ac204ed28a1740eec6390bbc	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MONTENEGRO	ME	MNE
6f845ca9a679442d8d5ee6b64f612dc2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PAKISTAN	PK	PAK
712914f841344761a9cce4063fc0e361	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PANAMA	PA	PAN
7230c9f7cba841eca72fafe1d30088c2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PUERTO RICO	PR	PRI
7494ca54409e44e0922d910042297847	1	SYSTEM	2010-03-24 00:00:00	\N	\N	AZERBAIJAN	AZ	AZE
7eaf957a19574d65a187571e8e8a7881	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BARBADOS	BB	BRB
7eccd310b36f47989c040e2570457d84	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SEYCHELLES ISLANDS	SC	SYC
812775c789b7433b9ee5bf52b674995a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LUXEMBOURG	LU	LUX
85e4737e6c684af488f3ad36ddb2c76f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	KENYA 	KE	KEN
875144ac00ce43779ee47eb9be8ce8a0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	LEBANON	LB	LBN
8b0698f993504bcbaf3bf5d3348d8dde	1	SYSTEM	2010-03-24 00:00:00	\N	\N	EGYPT	EG	EGY
8e6b3ae48e2a4d93a5fbeba9a7a59ad3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SLOVENIA	SI	SVN
9132a6a4c8d7445d9857943e1bb0b6fc	1	SYSTEM	2010-03-24 00:00:00	\N	\N	IRAN	IR	IRN
972fe95d3b8e4745bcb2697ee9fc0ef5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TURKEY	TR	TUR
9769fe0147e046c98638fe1f62b16400	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MALTA	MT	MLT
9d827a4d315c438d8d2a9094aeeb5266	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ECUADOR	ec	\N
9dcfccbd69504bc2863a6f6f7a274ae1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UZBEKISTAN	UZ	UZB
9e2c99f915eb41448134331510819ce8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	QATAR	QA	QAT
9ef764499dbe451bb410df391d5ec71c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CUBA	CU	CUB
a0b8dadf3b344cbe8542c52c6db4da99	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UNITED ARAB EMIRATES	AE	ARE
a6dcb60df39042c1842075577ce31b24	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CZECH REPUBLIC	CZ	CZE
a8c9ac4ab9444a99bedbdf75146c5391	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HAITI	HT	HTI
a8e3c76e0b184a24b334120ce3b8a6ba	1	SYSTEM	2010-03-24 00:00:00	\N	\N	INDIA	IN	IND
ac5ebc5d80254c1fac59f8cd0170f591	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UNITED KINGDOM	GB	GBR
ac76d742e7124299ab78120cc1440fd2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	YEMEN	YE	YEM
ada3f225e3cb49f48494e5e9c22151fa	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JAMAICA	JM	JAM
b0b358771ccc43e6a2d565ff3c050f9b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BELIZE	BZ	BLZ
b1d662d056274ad6b3e8499f0ead121d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NETHERLANDS	NL	NLD
b2620ad7e7084d3195cb5464e683780b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DENMARK	DK	DNK
b2e7dd2bed0e460e85d4636bc93c8c37	1	SYSTEM	2010-03-24 00:00:00	\N	\N	DOMINICAN REPUBLIC	DO	DOM
b6002aae3ea5461cb0d43106e08d4fc2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	COTE D IVOIRE	CI	CIV
b86d41d36824417d8424c9e89fb7943c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	REPUBLIC OF KOREA	KR	KOR
b9c97b9076e644bcbfb9960878f7f512	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ESTONIA	EE	EST
ba77bb2e374a4850aee10ea489625361	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ITALY	IT	ITA
bdb7ada13af94bca90f88bd798094a75	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UGANDA	UG	UGA
bee575f7d0384d2cab8ed17d3ebac1ca	1	SYSTEM	2010-03-24 00:00:00	\N	\N	TOGO	TG	TGO
c13361d24a884f34bbad5cd169b60b44	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NORWAY	NO	NOR
c28d0203dd524a7a8bcb9a1716faae10	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MOLDOVA	MD	MDA
c36323ead77544ea9c0ef4903d760126	1	SYSTEM	2010-03-24 00:00:00	\N	\N	INDONESIA	ID	IDN
c5ce29564592499d85714252c8bee03d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Australia	AU	AUS
c6597a0f59204b00b63fe1c0f121df7e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UKRAINE	UA	UKR
c7e6b1f32e1a4d78abbdd80bb77295f6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MYANMAR	MM	MMR
c8ce167e9bdc4d74ae564678a643845a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MALAYSIA	MY	MYS
ca502fc306fa42bba100929aeeebbde0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	POLAND	PL	POL
cb823aa64f954b3c83aacf75d552dc02	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UNITED STATES OF AMERICA	US	USA
cd849748cde640ca808905a88f686738	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SLOVAKIA	SK	SVK
cf3ca3bf06c347549e18dc270cd8a50f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BRAZIL 	BR	BRA
d00ae58bace04a28a899fb077b8db10e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	PARAGUAY 	PY	PRY
d0e4dd833452425cb0a74397a4ba7945	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NICARAGUA 	NI	NIC
d6162bc71e404b1a8b020e65794721cf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SRI LANKA	LK	LKA
daac39b5d4d5424ab11354c37caed7b6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	OMAN	OM	OMN
dc233d6caadb417aa232abf97a1eeaa5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ICELAND	IS	ISL
dcf2a0a2478e4fd691245e360ccc8a60	1	SYSTEM	2010-03-24 00:00:00	\N	\N	JORDAN	JO	JOR
de42af2ae39e441690729b2442ba8ca4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ANTIGUA AND BARBUDA	AG	ATG
dfb5529c9c064317835d30b1422ebf02	1	SYSTEM	2010-03-24 00:00:00	\N	\N	COLOMBIA	CO	COL
e053c11fe6834a9b908fce39f0bfbe66	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ISRAEL	IL	ISR
e1203d77a54b474d8552ac07b9d77e8d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FINLAND	FI	FIN
e3d3f91fa65c4da09c157c12760fc847	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SUDAN	SD	SDN
e586a48c0c6e49c6a2dbe685dc927a59	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ALBANIA	AL	ALB
e8759daacbb447018784c70e03c3b53b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BURKINA FASO	BF	BFA
e9e616a02d5d44739e04cfd592703021	1	SYSTEM	2010-03-24 00:00:00	\N	\N	THAILAND	TH	THA
eb90d5238bd04601a24e104b000e6ef4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SINGAPORE	SG	SGP
f5ae6ece20784948ab85e545c81c4f06	1	SYSTEM	2010-03-24 00:00:00	\N	\N	GERMANY	DE	DEU
f685f5e3d21c4ceb9cc8f3ca9df69672	1	SYSTEM	2010-03-24 00:00:00	\N	\N	NEPAL	NP	\N
f79daa9cef8d49438de4b06b3407464e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	MAURITIUS 	MU	MUS
f8630cfeacd14386bc9c1c2168b53346	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CHINA	CN	CHN
f95a221cc315402e86c5345587c54aa4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SWEDEN	SE	SWE
f9868259f8994a8d9a30dc27f9a74274	1	SYSTEM	2010-03-24 00:00:00	\N	\N	BELARUS	BY	BLR
f9924f9e7a374bb3a56c08956d217fc0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ETHIOPIA	ET	ETH
fa312c56668a46359afc0c5cfed20fff	1	SYSTEM	2010-03-24 00:00:00	\N	\N	ROMANIA	RO	ROU
fc62fec2835e483ca9ff66b3bc828ca1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	HUNGARY	HU	HUN
ffd49325645b4cfe90b12af209a6cdd8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	FRANCE	FR	FRA
c5c461808de44e2f85e2305102a3356b	2	svetlana.vasilkova	2012-05-31 00:13:47	svetlana.vasilkova	2012-05-31 00:13:59	Страна для удаления	DL	DEL
\.


--
-- Data for Name: lt_currency; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_currency (id, version, createdby, createdon, modifiedby, modifiedon, name, code, cyrilliccode, numericcode) FROM stdin;
004440fd14e24adc9cad2caa49ac2402	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Nepalese Rupee	NPR	\N	524
0195c99c32b743d69c6fed8f9691a565	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Namibia Dollar	NAD	\N	516
026ddda5735848b3974b287b1fce66b0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Slovenia, Tolars	SIT	\N	91
02b39ad1e40148c387f83fbd483fa281	1	sergey.buturlakin	2011-07-11 19:33:16	\N	\N	WIR Euro	CHE	\N	947
03b88c53f0584e3788bfda5aae756cc8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Philippine Peso	PHP	\N	608
05b3416cb9054f49be60a469e0218628	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Mexican Peso	MXN	\N	484
070e2b0a9bd7434f882fe4731b2fe585	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Turkish Lira	TRY	\N	949
0a39e628264b470daed5632c807e3f01	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Rand	ZAR	\N	710
0a6a3abf42f54c119c46a2a84196e075	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Pound Sterling	GBP	\N	826
0c8a09dc8e5949e483b9e622362b908c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Pataca	MOP	\N	446
0d3ca8a1fbe6475da6833259b9679bd5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Forint	HUF	\N	348
0d43861b5f6a4e50bebd5e5ea2e17a55	1	sergey.buturlakin	2011-07-11 19:31:48	\N	\N	Unidades de fomento	CLF	\N	990
0e2a178dda8b4f9595a352dc414b0acd	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Fiji Dollar	FJD	\N	242
0e5c20a5a015486dbc86aad14ba48cc1	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Cayman Islands Dollar	KYD	\N	136
0e6129464f26452eb242a00cc819c97c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Aruban Guilder	AWG	\N	533
0f06b2d6cf3b42a99715e1d5efa81a36	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Syrian Pound	SYP	\N	760
0f93b1741413471b85e0c1f3d4b6f598	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Trinidata and Tobago Dollar	TTD	\N	780
0fb4d7f3008e43fda58e3b7a28a476d5	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Dobra	STD	\N	678
10922e428d67412e9ec77ac3a8abc92e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Nakfa	ERN	\N	232
121b177046a54083b5a90694b05020c4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Mozambique, Meticais	MZM	\N	366
136721ef7f6e437480975ec182930206	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Jamaican Dollar	JMD	\N	388
13d345ee89ff4cb8aec6b85fc37fc61d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Argentine Peso	ARS	\N	32
17734151d97149dfb589046a307fb1e5	1	sergey.buturlakin	2011-07-11 19:29:00	\N	\N	Manat	TMT	\N	934
1788f2e318a0401a9928a3efee6d0808	1	sergey.buturlakin	2011-07-11 19:34:17	\N	\N	WIR Franc	CHW	\N	948
19cfd575f27e4a45b6eb1dbb67c8826b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Quetzal	GTQ	\N	320
1a878798913d4cd3a0a1a9677b2ec6df	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Vatu	VUV	\N	548
1b35f684a4d04e2aa2c5e1730e117037	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Cape Verde Escudo	CVE	\N	132
1e63ca9094e140adace1eeb05996496c	1	sergey.buturlakin	2011-07-11 19:25:21	\N	\N	Cedi	GHS	\N	936
1e7eaa3b03b04a7fa28c3fa6c02f907a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CFA Franc BEAC	XAF	\N	950
251c27402ae04fac886d5bcdbe5b796c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	New Leu	RON	\N	946
29849f25d99541c0a0e3abbddc14f7bb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Falkland Island Pound	FKP	\N	238
29a66d4ce5494620a4e0beb448ff25ca	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Uzbekistan Sum	UZS	\N	860
2a0ea54fddbe403d8add8b957e9671f2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Mauritius Rupee	MUR	\N	480
2e86d90f9ae243bbb0cba3c79ae72f7c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Euro	EUR	\N	978
2ede8accfc3b4f72bec44244bf17d364	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Loti	LSL	\N	426
2f94ecd3ed2a414baaf9910e45eeba2e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Russian Ruble	RUB	\N	643
31a091ca29374f23bf7b8accea1efbcd	1	SYSTEM	2010-03-24 00:00:00	\N	\N	New Israeli Sheqel	ILS	\N	376
342491db15054bb7886af95c9abff390	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Lari	GEL	\N	981
36172337d74044a3afdc5c8811dd68f0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Solomon Islands Dollar	SBD	\N	90
362e6f2dceeb41c3951ca8ae552efb54	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Nuevo Sol	PEN	\N	604
3883e41a68dc443c844353e78625ea52	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Lilangeni	SZL	\N	748
395ac4ef5a8a4231b70cd2b85eb9cb72	1	sergey.buturlakin	2011-07-11 19:29:34	\N	\N	Mexican Unidad de Inversion (UDI)	MXV	\N	979
3adcac83cd2b4063929108e4971db320	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Dominican Peso	DOP	\N	214
3d3d38736fb4410693f26c6f7b4e5bba	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Seychelles Rupee	SCR	\N	690
3d66d277e15c495c9f45a9411bd02a63	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Moroccan Dirham	MAD	\N	504
3e1d67ff44e44b1584a3f24578d0d130	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Gibraltar Pound	GIP	\N	292
3fcefaad4e824454a2fd38e969b4ec29	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Malaysian Ringgit	MYR	\N	458
409ad043aeb748079138505f6d9ef52b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Iraqi Dinar	IQD	\N	368
411db11c99b34c1daff79623ccee8fc3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Lebanese Pound	LBP	\N	422
453f5f4973984eabb858620d2709c054	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Latvian Lats	LVL	\N	428
454e3ec4f9074de79fbd6d4ed9a84a2b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Malagasy Ariary	MGA	\N	969
4704490ee131462db0ab3f8fa048f385	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Pula	BWP	\N	72
48111242305a4556aa9a10d297998167	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Cordoba Oro	NIO	\N	558
488e9be47aad4092bebe960a87e3060a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Jordanian Dinar	JOD	\N	400
49e2c21cc7734974a769b959e54318d9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Yuan Renminbi	CNY	\N	156
49e7924dbbbb48daaa1e133113e47a1e	1	sergey.buturlakin	2011-07-11 19:29:20	\N	\N	Metical	MZN	\N	943
4bb4fc3c4d414299ac2c7cabf262b71b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Canadian Dollar	CAD	\N	124
4c83a02dee4148a18b5f17a92c5dcd82	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Somali Shilling	SOS	\N	706
4c9fe5e73c7d450fb74c80099e89dd5a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Surinam Dollar	SRD	\N	968
4df05d1c2ce6437b8661f0471ded9da8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Costa Rican Colon	CRC	\N	188
4ef41b17afce4162aff5fe12b6088538	1	sergey.buturlakin	2011-07-11 19:29:56	\N	\N	Mvdol	BOV	\N	984
52f83089cb514a6caa63aaf096eb39d4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Hong Kong Dollar	HKD	\N	344
53203def24f842c78aa5dd7c85c02426	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Palladium	XPD	\N	964
53bc8686012c44c1a1d3b02f95228a0a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	SDR	XDR	\N	960
575ee37600e748ed96782f8287aa319f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Kwacha	MWK	\N	454
5959a4b786254f39b529847dfb54d0fb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Barbados Dollar	BBD	\N	52
5a40b1ee61c04690b39c3b002705bc78	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Armenian Dram	AMD	\N	51
5b92accf7e2746a8a1dd5461b9381aff	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Afghani	AFN	\N	971
5c17467782fb45658dc98596df4c36f8	1	sergey.buturlakin	2011-07-11 19:26:11	\N	\N	European Monetary Unit (E.M.U.-6)	XBB	\N	956
5c183385557045d99fc0c3750f82a83b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Algerian Dinar	DZD	\N	12
5d77b6c8cefd4927852e0cccf8d88c63	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Zloty	PLN	\N	985
5ef28ab9626c44a4848811ba492e65e9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Bermudian Dollar	BMD	\N	60
60643b2441ea4ddd84b800e850c24ef9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Dong	VND	\N	704
606f3a58566c46828ae28df221936d0c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Yen	JPY	\N	392
61de0e8a27cc4e8dba22c9132e4bb455	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Danish Krone	DKK	\N	208
651773db40f149539a41aa0c92acf804	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Swedish Krona	SEK	\N	752
67984c94a3c8406aa8561c87e7651bb2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	El Salvador Colon	SVC	\N	222
68139f13ca3b4745a2f816e3f269991b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Kwanza	AOA	\N	973
6bbfc3855d114105a4c6cf1d8af4cfd2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Burundi Franc	BIF	\N	108
6d7f798474ba4f1985186af11594cf7c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Saint Helena Pound	SHP	\N	654
6dcf3ee7fc6b42c9ac2ec7de4e7515c2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Gourde	HTG	\N	332
708437c01eaf47519969b8d40b640690	1	sergey.buturlakin	2011-07-11 19:32:28	\N	\N	US Dollar (Next Day)	USN	\N	997
71186c33a31349fb8142f507662c90ce	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Bahraini Dinar	BHD	\N	48
740c5e5fe70d47d8852252b2842a21a3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Denar	MKD	\N	807
7449184c05cc42fd8cbf885473f121de	1	SYSTEM	2010-03-24 00:00:00	\N	\N	New Zealand Dollar	NZD	\N	554
7596207357114c8280e1d5a24e9d4bc3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Guarani	PYG	\N	600
7b02f75731c9472d94237acff9c3079f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Dalasi	GMD	\N	270
7ba62d4e3b0547f693281686647e33cb	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Boliviano	BOB	\N	68
7cae96a9f6a846e487e63b04ad022964	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Netherlands Antillian Guilder	ANG	\N	532
7d5dc90fb5a54255a47f184b57581e35	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Liberian Dollar	LRD	\N	430
7ea1aad5d2834aea98d471cd4acc2ee4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Indian Rupee	INR	\N	356
7f0c80ddd4ae4fd4b5699daa5f6d7749	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Egyptian Pound	EGP	\N	818
82e2e194d06e48c38c6f0c654b471786	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CFP Franc	XPF	\N	953
83a42d146f5a44b2817f7c384d6bc129	1	SYSTEM	2010-03-24 00:00:00	\N	\N	US Dollar	USD	\N	840
84ab278688bd4601b047a8af4a270323	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Ghana, Cedis	GHC	\N	276
8dd76f2813f4463597cdd7029213e081	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Chilean Peso	CLP	\N	152
8f86f85496e2479ea382dabc70da53a0	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Brazilian Real	BRL	\N	986
90f61b276ddc44a68105c76089078ff8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Lempira	HNL	\N	340
925849616d884292a66a930c23728602	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Bahamian Dollar	BSD	\N	44
939a3797c4244a259169252c133beff7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Slovakia, Koruny	SKK	\N	63
9480c5ead9bf4ed8b9d3b765f6edbd41	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Colombian Peso	COP	\N	170
9543d88d884949c0ba6e27b660e4e4c6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Cuban Peso	CUP	\N	192
978e3ceb15fe4c028e36841a79d33d1e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Iceland Krona	ISK	\N	352
98181e0f32494827b9e85d92af5eae48	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Croatian Kuna	HRK	\N	191
98bbb24b5aa441fa9809842018be52a4	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Baht	THB	\N	764
9a9f8cc464c34a048b6bce3677071722	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Lithuanian Litas	LTL	\N	440
9babd5384dec481083090da9bea1b383	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Kina	PGK	\N	598
a26114a2cde84f029ddd5001323d88c9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Kuwaiti Dinar	KWD	\N	414
a3c8595663d54a7088f3410b4eb06e54	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Qatari Rial	QAR	\N	634
a9f24e9029224f6a820427e04cab8fae	1	sergey.buturlakin	2011-07-11 19:32:45	\N	\N	US Dollar (Same Day)	USS	\N	998
aad5f1f8893d4a5ca96aa8d136bb807b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Zimbabwe, Zimbabwe Dollars	ZWD	\N	382
ab01a5783ba94890a45d70081b3b39ff	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Taka	BDT	\N	50
af011fb824ac4d58bc69e9fcec60802c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Tanzanian Shilling	TZS	\N	834
af496e4b4de647afa7b61eacfd1a6f4f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Hryvnia	UAH	\N	980
b167476f5e3f478da906f8e86c4bcd90	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Convertible Marks	BAM	\N	977
b359d55553c74dc38d8d8f264cac57a6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Romania, Lei [being phased out]	ROL	\N	66
b438f561a12646368232565be0528469	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Ethiopian Birr	ETB	\N	230
b51a704eadaf42e29f51e27611c47a5f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Brunei Dollar	BND	\N	96
b586da2243174d8a8fa1674ff98e4887	1	sergey.buturlakin	2011-07-11 19:26:42	\N	\N	European Unit of Account 17 (E.U.A.-17)	XBD	\N	958
b58cb8c115a44a829dc0d87edafd5d0e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	East Caribbean Dollar	XCD	\N	951
b8be91dbaa844a9eb451dd56cc90aebf	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Tunisian Dinar	TND	\N	788
b92b04d28454478dbc6d6984d2725136	1	sergey.buturlakin	2011-07-11 19:32:06	\N	\N	Uruguay Peso en Unidades Indexadas	UYI	\N	940
bab7187ca602474eb1ffd5b598ad3342	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Ngultrum	BTN	\N	64
bb1539b43b3d4c76abec8d8d95523066	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Malta, Liri	MTL	\N	46
bb18fbb9ea7b4c30a77880402427f2b7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Saudi Riyal	SAR	\N	682
bbeb9dea6cb84839a303de2111fdf53a	1	sergey.buturlakin	2011-07-11 19:25:45	\N	\N	Codes specifically reserved for testing purposes	XTS	\N	963
bca303dbda474ff8bb41d15394d3f61d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Norwegian Krone	NOK	\N	578
bdc46af8625943b68c3bcf9a0619587f	2	sergey.buturlakin	2011-07-11 19:24:00	sergey.buturlakin	2011-07-11 19:24:15	Bolivar Fuerte	VEF	\N	937
c1a06adb4e4f4de8b522eb14a4f82c52	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Naira	NGN	\N	566
c207ceb5fdc6440c81f47b16507e97d8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Swiss Franc	CHF	\N	756
c3195b776d614f5bb3356277c348a47b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Iranian Rial	IRR	\N	364
c6188a8e93934b35a3ea1c59da27144a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Gold	XAU	\N	959
c67c70aff8484592a2a97f723adbadc4	1	sergey.buturlakin	2011-07-11 19:27:32	\N	\N	Guinea-Bissau Peso	GWP	\N	624
c67df08a75704a398ebd8dda7cc9c4c7	1	sergey.buturlakin	2011-07-11 19:24:49	\N	\N	Bond Markets Units European Composite Unit (EURCO)	XBA	\N	955
c790df28b5da45e588caed8e2263500f	1	sergey.buturlakin	2011-07-11 19:33:28	\N	\N	Zimbabwe Dollar	ZWL	\N	932
ca9d5e359fdd4ee49c8336265d0fff19	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Balboa	PAB	\N	590
cbd5e98e1f3b4a9eb590ee21d584526b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Rwanda Franc	RWF	\N	646
cc30b24c610c40ad9b1117e295f2eb34	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Guyana Dollar	GYD	\N	328
cd24e8aa65cd40ac9b0e3ecf2c82255d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Tenge	KZT	\N	398
cf40ec4e75e346f799d42f984a649912	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Sudanese Pound	SDG	\N	938
cf49fff95e7c4ca2a59376266c6e5cb7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Rial Omani	OMR	\N	512
cf55966ecc5d49cb879f33bc73a8f456	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Comoro Franc	KMF	\N	174
cf98ebd514db48bba12dfc0daa1780c7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Riel	KHR	\N	116
d399295ab8224f8baa65e93ec8dbaeea	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Libyan Dinar	LYD	\N	434
d3ecc69a32bf4dc88cece855d2496713	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Tugrik	MNT	\N	496
d47cd03777bf40ed9416006c0aba62c3	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Bulgarian Lev	BGN	\N	975
d71b85c552ee485fac6dbd259e79f81c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Pakistan Rupee	PKR	\N	586
d86ee11ac2fd4ae99529afa4c813e68f	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Silver	XAG	\N	961
d90bf8482d38466da7d2a4d6bc403297	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Singapore Dollar	SGD	\N	702
dbf931fce18643608a39ce4d6e42c4b9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Sri Lanka Rupee	LKR	\N	144
dca4f7cf0ed6432ea6d114d0027e72ad	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Australian Dollar	AUD	\N	36
dcbf0c2ef443431c86bad620ebdbae2a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Rufiyaa	MVR	\N	462
df025af74b394b43ab3dea8eea045648	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Belarussian Ruble	BYR	\N	974
df0bf4a5a7494b50af36d44162989f4e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Kroon	EEK	\N	233
df97c5ee8f1c416bb2f694070456c93c	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Pa'anga	TOP	\N	776
e0aa812f698d42d5a9a801ac9a88700b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Kyat	MMK	\N	104
e11a45ced9e246c79c36d2f21917590b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Azerbaijanian Manat	AZN	\N	944
e2141a7bba4d42e7975d33786dd28983	1	SYSTEM	2010-03-24 00:00:00	\N	\N	UAE Dirham	AED	\N	784
e2e4bccf068849e2ae0842d5aa60ae01	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Won	KRW	\N	410
e6eeb733372f40ee8006302bba8bab7a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Czech Koruna	CZK	\N	203
e73ec8a5be164733b769022b0d647d2c	1	sergey.buturlakin	2011-07-11 19:30:37	\N	\N	Peso Convertible	CUC	\N	931
e78e240368004137b2fd5380b68f9d11	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Belize Dollar	BZD	\N	84
e81587a11c744172b26911e18c832b5b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Peso Uruguayo	UYU	\N	858
e8ddefbc49df4d11bdce59db9a626853	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Som	KGS	\N	417
eb48b56a661941268447842e349e4bd2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Somoni	TJS	\N	972
ebc9246b3c0f43648dc8f71835799fe2	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Kenyan Shilling	KES	\N	404
ee00d9eb88ca4996a5fad6ece4d0eaa9	1	sergey.buturlakin	2011-07-11 19:31:27	\N	\N	Unidad de Valor real	COU	\N	970
ef896fc80aad4061952fe8b4a7d7043e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Rupiah	IDR	\N	360
ef9ecb7f0ae54230bd633657e82e2d32	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Djibouti Franc	DJF	\N	262
efc3ae417f4e4d01ba348db0813c6272	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Zambian Kwacha	ZMK	\N	894
f298fea05b7c459f8c726ea3361c6630	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Moldovan Leu	MDL	\N	498
f2c31ad45b0f4458ab669f77895c25a9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Tala	WST	\N	882
f2e6b217d587436b930663fba39905b6	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Ouguiya	MRO	\N	478
f3743ec2ea72421083a345c74003d32d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Uganda Shilling	UGX	\N	800
f38dc1211e694f9397fe5ea750e79f8e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Leone	SLL	\N	694
f43a0844e3a243cc870c6686da8c579d	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Guinea Franc	GNF	\N	324
f55ff94884cd45f5ad3a66f23c1023d7	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Yemeni Rial	YER	\N	886
f5f17a31dc194c08bd7c767d2a291fa9	1	SYSTEM	2010-03-24 00:00:00	\N	\N	CFA Franc BCEAO	XOF	\N	952
f6375954a16347ae9cdd000f95cfda63	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Kip	LAK	\N	418
f7ec21d3ff594393aaec6f5b560f8cf1	1	sergey.buturlakin	2011-07-11 19:31:08	\N	\N	The codes assigned for transactions where no currency is involved are:	XXX	\N	999
f86d2ab781a54931b56d46e1ddc774d8	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Franc Congolais	CDF	\N	976
fb214dc1979a49c8bee43d8c72f3d881	1	sergey.buturlakin	2011-07-11 19:26:59	\N	\N	European Unit of Account 9 (E.U.A.-9)	XBC	\N	957
fc4b05fa2f034a7a976c1f317956d15b	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Platinum	XPT	\N	962
fc5417a6b2e747e28fee81813a20647f	1	sergey.buturlakin	2011-07-11 19:35:13	\N	\N	Serbian Dinar	RSD	\N	941
fe2675ccdccd4c1a982fbd1d9ec52f6a	1	SYSTEM	2010-03-24 00:00:00	\N	\N	North Korean Won	KPW	\N	408
ff53ceb8297b48689fc9daca8023216e	1	SYSTEM	2010-03-24 00:00:00	\N	\N	New Taiwan Dollar	TWD	\N	901
ffcd99b1c38c4054a9b2c155c964c652	1	SYSTEM	2010-03-24 00:00:00	\N	\N	Lek	ALL	\N	8
f42bfd6a66094743b9a842a8847662a1	2	svetlana.vasilkova	2012-05-31 00:23:03	svetlana.vasilkova	2012-05-31 00:23:36	Валюта для удаления	DLT	УДЛ	11111
\.


--
-- Data for Name: lt_department; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_department (id, organization) FROM stdin;
\.


--
-- Data for Name: lt_document_access; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_document_access (id, version, createdby, createdon, modifiedby, modifiedon, fulldocumentcontrol, owner, person) FROM stdin;
\.


--
-- Data for Name: lt_document_owner; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_document_owner (id, version, isactive, owner) FROM stdin;
\.


--
-- Data for Name: lt_file; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_file (id, filename, "timestamp", content, uploadedby, party) FROM stdin;
\.


--
-- Data for Name: lt_flight_segment; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_flight_segment (id, version, createdby, createdon, modifiedby, modifiedon, "position", type, fromairportcode, fromairportname, toairportcode, toairportname, carrieriatacode, carrierprefixcode, carriername, flightnumber, serviceclasscode, serviceclass, departuretime, arrivaltime, mealcodes, mealtypes, numberofstops, luggage, checkinterminal, checkintime, duration, arrivalterminal, seat, farebasis, stopover, ticket, fromairport, toairport, carrier) FROM stdin;
d78110b362ac4dd8bdaa8c8f82d18e1d	1	admin	2012-05-15 15:56:50	\N	\N	0	1	DOK	DONETSK	KBP	KIEV BORYSPIL	PS	\N	\N	0048	V	\N	2011-12-06 07:20:00	2011-12-06 08:30:00	S	16	0	\N	\N	\N	01:10	B	\N	\N	t	2b48aa82cc5140b699bdfe0fa64adb57	5a5e519cefa848d6800b90834ea6365a	f6ff4ec61b3b4b4bb0673dd03abdeb80	\N
53e95cae53d248009015667ed00aff30	1	admin	2012-05-15 15:56:50	\N	\N	1	0	KBP	KIEV BORYSPIL	MSQ	MINSK MINSK 2 INT	B2	\N	\N	0846	V	0	2011-12-06 11:30:00	2011-12-06 13:35:00		\N	0	20K	B	\N	01:05	\N	\N	VSR	t	2b48aa82cc5140b699bdfe0fa64adb57	f6ff4ec61b3b4b4bb0673dd03abdeb80	fa73f734fe7e4673916e7892df5e12ab	48c384d42f35436f9e1b2daee1371d3c
a3bd599a9ff04af9873aab453776c928	1	admin	2012-05-15 15:56:50	\N	\N	2	0	MSQ	MINSK MINSK 2 INT	KBP	KIEV BORYSPIL	B2	\N	\N	0845	V	0	2011-12-09 17:30:00	2011-12-09 17:35:00		\N	0	20K	\N	\N	01:05	B	\N	VSR	t	2b48aa82cc5140b699bdfe0fa64adb57	fa73f734fe7e4673916e7892df5e12ab	f6ff4ec61b3b4b4bb0673dd03abdeb80	48c384d42f35436f9e1b2daee1371d3c
a4b5e1d342a446929abfc06999e50d12	1	admin	2012-05-15 15:56:50	\N	\N	3	1	KBP	KIEV BORYSPIL	DOK	DONETSK	PS	\N	\N	0047	W	\N	2011-12-09 19:50:00	2011-12-09 21:00:00	S	16	0	\N	B	\N	01:10	\N	\N	\N	t	2b48aa82cc5140b699bdfe0fa64adb57	f6ff4ec61b3b4b4bb0673dd03abdeb80	5a5e519cefa848d6800b90834ea6365a	\N
\.


--
-- Data for Name: lt_gds_agent; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_gds_agent (id, version, createdby, createdon, modifiedby, modifiedon, origin, code, officecode, person, office) FROM stdin;
480edfe5c9e347658017a8145eab4a64	2	yulia.sosnina	2010-06-03 17:23:43	yulia.sosnina	2010-06-03 17:29:27	0	0002BB	IEVU23885	5cbde1ec38d94566adec7f5a3a0d7cc1	a354460c7cfc42d0bdf1e7624018905a
61cf0f5ffbbd4765aa61934cc3936575	2	tatyana.shevchenko	2010-04-19 19:22:14	tatyana.shevchenko	2010-04-19 19:33:25	0	2222MV	DOKC32530	9c47021f082c4e5b9e38ee71d6dd8fa3	a354460c7cfc42d0bdf1e7624018905a
6def13bfa28e4f8cb1c3b95101e16212	2	yulia.sosnina	2010-06-03 17:31:43	yulia.sosnina	2010-06-03 17:31:53	0	9999WS	IEVU22112	5cbde1ec38d94566adec7f5a3a0d7cc1	a354460c7cfc42d0bdf1e7624018905a
729cfa32ba134fd3850bd4f0ad4d2442	2	tatyana.shevchenko	2010-04-19 19:23:12	tatyana.shevchenko	2010-04-19 19:33:31	0	7777SV	DOKC32530	d52cb22e2e754178afb8184957fbaba8	a354460c7cfc42d0bdf1e7624018905a
7fc9b6c2c9c948a3bbc1e58deafa4ff2	1	yulia.sosnina	2010-06-03 17:23:07	\N	\N	0	0001AA	IEVU23885	5cbde1ec38d94566adec7f5a3a0d7cc1	a354460c7cfc42d0bdf1e7624018905a
8750f7c4ac484f89ad7b609720861fae	3	yulia.sosnina	2010-03-24 12:19:08	tatyana.shevchenko	2010-04-19 19:33:10	0	0001AA	DOKC32530	5cbde1ec38d94566adec7f5a3a0d7cc1	a354460c7cfc42d0bdf1e7624018905a
9656eeff50d9484484c2c78f433ba7fb	2	konstantin.tolstopyat	2010-11-02 13:25:35	konstantin.tolstopyat	2010-11-02 13:26:10	0	0001AA	DOKU23199	5cbde1ec38d94566adec7f5a3a0d7cc1	a354460c7cfc42d0bdf1e7624018905a
f08112660a69476ca25c0ce34d1f0d47	1	konstantin.tolstopyat	2010-07-08 16:46:15	\N	\N	0	1111JK	IEVU22112	5cbde1ec38d94566adec7f5a3a0d7cc1	a354460c7cfc42d0bdf1e7624018905a
f8d2f1aafe2f40469a60ceee73527042	1	sergey.buturlakin	2011-10-31 12:27:23	\N	\N	2	90	30AC	5cbde1ec38d94566adec7f5a3a0d7cc1	a354460c7cfc42d0bdf1e7624018905a
c40f4cd757ee4462928a6d6f84a8b8cc	1	sergey.buturlakin	2011-10-31 12:28:07	\N	\N	2	90	30AD	5cbde1ec38d94566adec7f5a3a0d7cc1	a354460c7cfc42d0bdf1e7624018905a
125e00a5750c4af682ee801613102d5f	1	sergey.buturlakin	2011-10-31 12:28:48	\N	\N	2	91	30AC	9c47021f082c4e5b9e38ee71d6dd8fa3	a354460c7cfc42d0bdf1e7624018905a
0888b1ff14c145afbfe8a09ed8276a35	1	sergey.buturlakin	2011-10-31 12:29:10	\N	\N	2	91	30AD	9c47021f082c4e5b9e38ee71d6dd8fa3	a354460c7cfc42d0bdf1e7624018905a
679cdcb8951548c6af1017c7aa456378	1	sergey.buturlakin	2011-10-31 12:29:35	\N	\N	2	92	30AC	d52cb22e2e754178afb8184957fbaba8	a354460c7cfc42d0bdf1e7624018905a
91672b92367d48769b596d4939d49bbf	1	sergey.buturlakin	2011-10-31 12:29:58	\N	\N	2	92	30AD 	d52cb22e2e754178afb8184957fbaba8	a354460c7cfc42d0bdf1e7624018905a
\.


--
-- Data for Name: lt_gds_file; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_gds_file (id, class, version, createdby, createdon, modifiedby, modifiedon, name, "timestamp", content, importresult, importoutput, filetype, filepath, username, office, officecode) FROM stdin;
c656340f1c844b0398870eb8b2970ec5	Air	2	SYSTEM	2011-12-01 17:19:02	SYSTEM	2011-12-01 17:19:02	AIR_20111201151756.03478.PDT	2011-12-01 17:17:00	AIR-BLK207;MA;;233;0100074150;1A1336371;001001\r\nAMD 0101324092;1/1;VOID01DEC;MVSU\r\nGW3718884;1A1336371\r\nMUC1A ZC7BVB006;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R6P83 \r\nI-001;01CHEREDNICHENKO/YURIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667792\r\nFPCASH\r\nENDX\r\n	1	2011-12-01 17:19:02: Void 566-3544667792 успешно импортирован\r\n	0	\N	\N	\N	\N
18e72eb54405414ca89b9799c3df5dee	Air	2	SYSTEM	2011-12-02 13:48:41	SYSTEM	2011-12-02 13:48:41	AIR_20111202114729.07948.PDT	2011-12-02 13:47:00	AIR-BLK207;RF;;190;0200028818;1A1336371;001001\r\nAMD 0201324878;1/1;    02DEC;SVSU\r\nGW3670821;1A1336371\r\nMUC1A          ;  01;DOKC32530;72320942;;;;;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;\r\nB-TRFP\r\nC-7906/ 7777SVSU-7777SVSU-M---\r\nD-111202;111202;111202\r\nRFDF;24NOV11;I;UAH2869;0;2869;;;;;;XT372;3241;08DEC11\r\nKRF ;QUAH196      YK   ;QUAH24       UD   ;QUAH96       YQ   ;QUAH56       UA   ;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nI-001;01FEDIN/KOSTYANTYN MR;;;;\r\nT-E870-3544617236\r\nTBS1-1234\r\nR-870-3544617236;02DEC11\r\nSAC870AIKQ1ZFI8K\r\nFM1.00P\r\nFPCASH/UAH3241\r\nFTITAGVV01-801\r\nENDX\r\n	1	2011-12-02 13:48:41: Возврат 870-3544617236 успешно импортирован\r\n	0	\N	\N	\N	\N
9322622fddf240e0bc2a009ea55538f1	Air	2	SYSTEM	2011-12-02 13:48:42	SYSTEM	2011-12-02 13:48:42	AIR_20111202114735.07951.PDT	2011-12-02 13:47:00	AIR-BLK207;RF;;190;0200023981;1A1336371;001001\r\nAMD 0201324879;1/1;    02DEC;SVSU\r\nGW3670821;1A1336371\r\nMUC1A          ;  01;DOKC32530;72320942;;;;;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;\r\nB-TRFP\r\nC-7906/ 7777SVSU-7777SVSU-M---\r\nD-111202;111202;111202\r\nRFDF;24NOV11;I;UAH2869;0;2869;;;;;;XT372;3241;08DEC11\r\nKRF ;QUAH196      YK   ;QUAH24       UD   ;QUAH96       YQ   ;QUAH56       UA   ;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nI-001;01MALYKH/DMITRIY MR;;;;\r\nT-E870-3544617237\r\nTBS1-1234\r\nR-870-3544617237;02DEC11\r\nSAC870AIKQ1ZFI8L\r\nFM1.00P\r\nFPCASH/UAH3241\r\nFTITAGVV01-801\r\nENDX\r\n	1	2011-12-02 13:48:42: Возврат 870-3544617237 успешно импортирован\r\n	0	\N	\N	\N	\N
e3acc099cdfc4326a6d754b5a5c5fd53	Air	2	SYSTEM	2011-12-03 14:49:26	SYSTEM	2011-12-03 14:49:26	AIR_20111203124822.13151.PDT	2011-12-03 14:48:00	AIR-BLK207;7A;;247;0300052196;1A1336371;001001\r\nAMD 0301325822;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A ZX7U4N002;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;UN QDSYW \r\nA-TRANSAERO AIRLINES;UN 6705\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111203;111203;111203\r\nG-X  ;;MOWLON;E1\r\nH-001;002ODME;MOSCOW DME       ;LHR;LONDON LHR       ;UN    0333 C C 05DEC1050 1115 05DEC;OK01;HK01;B ;0;763;;;35K;;;ET;0425 ;N;1592;RU;GB;1 \r\nK-FEUR1400.00    ;UAH15092      ;;;;;;;;;;;UAH15750      ;10.77997   ;;   \r\nKFTF; UAH496      YQ AC; UAH162      YR VB;;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH496      YQ ;UAH162      YR ;;\r\nL-\r\nM-COW            \r\nN-NUC1963.55\r\nO-XXXX\r\nQ-MOW UN LON1963.55NUC1963.55END ROE0.712992;FXP\r\nI-001;01TIMCHENKO/MAKSYM MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K670-3544667858\r\nFM*M*5\r\nFPCASH\r\nFVUN;S2;P1\r\nTKOK03DEC/DOKC32530\r\nRIZTICKET FARE UAH 15092.00\r\nRIZTICKET TAX UAH 658.00\r\nRIZTICKET TTL UAH 15750.00\r\nRIZSERVICE FEE UAH 150.00+VAT 30.00\r\nRIZGRAND TOTAL UAH 15930.00\r\nENDX\r\n	1	2011-12-03 14:49:26: Билет 670-3544667858 успешно импортирован\r\n	0	\N	\N	\N	\N
121620c512ac4a5895b35c3e067dc775	Air	2	SYSTEM	2011-12-02 09:15:30	SYSTEM	2011-12-02 09:15:30	AIR_20111202071420.04725.PDT	2011-12-02 09:14:00	AIR-BLK207;7A;;257;0200007619;1A1336371;001001\r\nAMD 0201324281;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZJNBK4004;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R5V7B ;VV ZJNBK4\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/S2/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111202;111202;111202\r\nG-   ;;DOKIEV;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0092 L L 05DEC0920 1045 05DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0125 ;N;347;UA;UA;B \r\nU-002X;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 V V 05DEC1950 2100 05DEC;OK01;HK01;S ;0;735;;;;B ;;ET;0110 ;N;;347;;UA;UA;  \r\nK-FUSD125.00     ;UAH999        ;;;;;;;;;;;UAH1320       ;7.9899     ;;   \r\nKFTF; UAH212      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH212      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-LOW1M5         \r\nN-USD125.00\r\nO-05DEC05DEC\r\nQ-DOK VV IEV125.00USD125.00END XT48YQ36YK21UA;FXB/S2\r\nI-001;01BIKEZINA/OLENA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667804\r\nFE.-44877533.-/NONENDO/REBOOK USD20 /VAT UAH 220.00;S2;P1\r\nFM*M*1A;S2;P1\r\nFPCASH\r\nFVVV;S2;P1\r\nTKOK02DEC/DOKC32530\r\nENDX\r\n	1	2011-12-02 09:15:30: Билет 870-3544667804 успешно импортирован\r\n	0	\N	\N	\N	\N
b37c6fba0e364e02875aba602db71efc	Air	2	SYSTEM	2011-12-02 08:50:01	SYSTEM	2011-12-02 08:50:01	AIR_20111201193034.04127.PDT	2011-12-01 21:30:00	AIR-BLK207;7A;;247;0100067504;1A1336371;001001\r\nAMD 0101324238;2/2;              \r\nGW3671287;1A1336371;IEV1A098A;AIR\r\nMUC1A 8SHYP8011;0302;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LH 8SHYP8\r\nA-LUFTHANSA;LH 2203\r\nB-TTP\r\nC-7906/ 0001AASU-7777SVSU-I-0--\r\nD-111128;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-001;004ODOK;DONETSK          ;MUC;MUNICH           ;LH    2543 S S 05JAN1540 1750 05JAN;OK02;HK02;M ;0;CR9;;LUFTHANSA CITYLINE;1PC;;1455 ;ET;0310 ; ;1193;UA;DE;2 \r\nY-001;004DOK;DONETSK;MUC;MUNICH;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nH-002;005OMUC;MUNICH           ;DOK;DONETSK          ;LH    2542 W W 15JAN1100 1500 15JAN;OK02;HK02;S ;0;CR9;;LUFTHANSA CITYLINE;1PC;2 ;1020 ;ET;0300 ; ;1193;DE;UA;  \r\nY-002;005MUC;MUNICH;DOK;DONETSK;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nK-FUSD442.00     ;UAH3532       ;;;;;;;;;;;UAH4831       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH772      YQ AC; UAH42       UA SE; UAH86       OY CB; UAH207      RA EB; UAH56       DE SE;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH1163     XT ;\r\nL-\r\nM-SNC2W2         ;WNC2W2         \r\nN-NUC204.00;238.00\r\nO-05JAN05JAN;15JAN15JAN\r\nQ-DOK LH MUC204.00LH DOK238.00NUC442.00END ROE1.000000XT772YQ42UA86OY207RA56DE;FXP\r\nI-001;01GORLOV/MYKOLA MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /PLS ISS AUTOMATIC TKT BY 01DEC11/2359Z OR ALL LH SEGS WILL BE XLD. APPLICABLE FARE RULE APPLIES IF IT\r\nSSR OTHS 1A  ////DEMANDS EARLIER TKTG.XE/I FF\r\nOSI LH  DS/EXP/UA23969\r\nT-K220-3544667798\r\nFENONREF/FL/CHG RESTRICTEDCHECK FARE NOTE;S4-5;P1-2\r\nFM*M*1A\r\nFPCASH\r\nFVLH;S4-5;P1-2\r\nTKOK28NOV/DOKC32530\r\nOPLDOKC32530/01DEC/3C0\r\nI-002;02GORLOVA/OLGA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /PLS ISS AUTOMATIC TKT BY 01DEC11/2359Z OR ALL LH SEGS WILL BE XLD. APPLICABLE FARE RULE APPLIES IF IT\r\nSSR OTHS 1A  ////DEMANDS EARLIER TKTG.XE/I FF\r\nOSI LH  DS/EXP/UA23969\r\nT-K220-3544667799\r\nFENONREF/FL/CHG RESTRICTEDCHECK FARE NOTE;S4-5;P1-2\r\nFM*M*1A\r\nFPCASH\r\nFVLH;S4-5;P1-2\r\nTKOK28NOV/DOKC32530\r\nOPLDOKC32530/01DEC/3C0\r\nENDX\r\n	1	2011-12-02 08:50:01: Билет 220-3544667798 успешно импортирован\r\nБилет 220-3544667799 успешно импортирован\r\n	0	\N	\N	\N	\N
6863a8064dcc48aba3c792805bee195e	Air	2	SYSTEM	2011-12-01 17:23:40	SYSTEM	2011-12-01 17:23:40	AIR_20111201152232.03508.PDT	2011-12-01 17:22:00	AIR-BLK207;MA;;233;0100049483;1A1336371;001001\r\nAMD 0101324097;1/1;VOID01DEC;MVSU\r\nGW3718884;1A1336371\r\nMUC1A ZCHQBK007;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZCHQBK\r\nI-001;01PLATONOVA/NATALYA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667789\r\nFPCASH\r\nENDX\r\n	1	2011-12-01 17:23:40: Void 870-3544667789 успешно импортирован\r\n	0	\N	\N	\N	\N
f67eb00fbf3945899f26c0d358ae3397	Air	2	SYSTEM	2011-12-01 11:14:14	SYSTEM	2011-12-01 11:14:14	AIR_20111201091306.60975.PDT	2011-12-01 11:13:00	AIR-BLK207;7A;;257;0100030670;1A1336371;001001\r\nAMD 0101323433;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y7SQCV010;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R6H3F ;VV Y7SQCV\r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/S3/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;IEVDOK;UA\r\nH-003;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 W W 02DEC1950 2100 02DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nU-001X;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 Y Y 01DEC1950 2100 01DEC;OK01;HK01;N ;0;735;;;;;;ET;0110 ;N;;347;;UA;UA;B \r\nK-FUSD191.00     ;UAH1526       ;;;;;;;;;;;UAH1947       ;7.9897     ;;   \r\nKFTF; UAH313      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH56       YR VA; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH313      HF ;UAH4        UD ;UAH104      XT ;\r\nL-\r\nM-WOWDOM   KK10  \r\nN-USD190.80\r\nO-XXXX\r\nQ-IEV PS DOK190.80USD190.80END XT16YQ56YR20YK12UA;FXP/ZO-10P*KK10/S3\r\nI-001;01DAVYDOVA/NATALYA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667756\r\nFENON END/REF RESTR/RBK FOC;S3;P1\r\nFM*M*1;S3;P1\r\nFPCASH\r\nFTKK4704/11;S3;P1\r\nFVPS;S3;P1\r\nTKOK01DEC/DOKC32530\r\nENDX\r\n	1	2011-12-01 11:14:14: Билет 566-3544667756 успешно импортирован\r\n	0	\N	\N	\N	\N
8a13c38ee99e4f9f9926c89fb43331b5	Air	2	SYSTEM	2011-12-01 13:09:37	SYSTEM	2011-12-01 13:09:37	AIR_20111201110832.01135.PDT	2011-12-01 13:08:00	AIR-BLK207;7A;;247;0100026942;1A1336371;001001\r\nAMD 0101323670;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y8HTS3007;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y8HTS3\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;IEVDOK;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0091 L L 04DEC0910 1025 04DEC;OK01;HK01;N ;0;733;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nK-FUSD113.00     ;UAH903        ;;;;;;;;;;;UAH1174       ;7.9897     ;;   \r\nKFTF; UAH187      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH187      HF ;UAH4        UD ;UAH80       XT ;\r\nL-\r\nM-LOW1M5   KK10  \r\nN-USD112.50\r\nO-04DEC04DEC\r\nQ-IEV VV DOK112.50USD112.50END XT48YQ20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01KOSTROMOV/OLEKSIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667766\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 195.67\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 903.00\r\nRIZTICKET TAX UAH 271.00\r\nRIZTICKET TTL UAH 1174.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1234.00\r\nENDX\r\n	1	2011-12-01 13:09:37: Билет 870-3544667766 успешно импортирован\r\n	0	\N	\N	\N	\N
8f6c62e1d4a545c4b1e9b08ef033d1a5	Air	2	SYSTEM	2011-12-02 09:15:32	SYSTEM	2011-12-02 09:15:32	AIR_20111202071426.04727.PDT	2011-12-02 09:14:00	AIR-BLK207;7A;;257;0200007798;1A1336371;001001\r\nAMD 0201324282;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZJNBK4005;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R5V7B ;VV ZJNBK4\r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/S3/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111202;111202;111202\r\nG-   ;;IEVDOK;UA\r\nH-002;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 V V 05DEC1950 2100 05DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nU-001X;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0092 L L 05DEC0920 1045 05DEC;OK01;HK01;N ;0;320;;;;;;ET;0125 ;N;;347;;UA;UA;B \r\nK-FUSD142.00     ;UAH1135       ;;;;;;;;;;;UAH1478       ;7.9899     ;;   \r\nKFTF; UAH235      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH56       YR VA; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH235      HF ;UAH4        UD ;UAH104      XT ;\r\nL-\r\nM-VOWDOM         \r\nN-USD142.00\r\nO-05DEC05DEC\r\nQ-IEV PS DOK142.00USD142.00END XT16YQ56YR20YK12UA;FXP/S3\r\nI-001;01BIKEZINA/OLENA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667805\r\nFENON END/REF REST/RBK 10USD;S3;P1\r\nFM*M*1;S3;P1\r\nFPCASH\r\nFVPS;S3;P1\r\nTKOK02DEC/DOKC32530\r\nENDX\r\n	1	2011-12-02 09:15:32: Билет 566-3544667805 успешно импортирован\r\n	0	\N	\N	\N	\N
9d49900f9e0d42598ec96cda50eae81b	Air	2	SYSTEM	2011-12-02 09:23:54	SYSTEM	2011-12-02 09:23:54	AIR_20111202072256.04773.PDT	2011-12-02 09:22:00	AIR-BLK207;7A;;239;0200007254;1A1336371;001001\r\nAMD 0201324285;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A ZBZOVT012;0505;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZBZOVT\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP\r\nC-7906/ 0001AASU-7777SVSU-M-E--\r\nD-111201;111202;111202\r\nG-X  ;;DNKDNK;E1\r\nH-003;006ODNK;DNIPROPETROVSK   ;SVO;MOSCOW SVO       ;VV    0503 Y Y 04DEC1910 2250 04DEC;OK05;HK05;L ;0;ER4;;;20K;;;ET;0140 ;N;530;UA;RU;C \r\nH-004;007OSVO;MOSCOW SVO       ;DNK;DNIPROPETROVSK   ;VV    0502 T T 10DEC1230 1215 10DEC;OK05;HK05;L ;0;ER4;;;20K;C ;;ET;0145 ;N;530;RU;UA;  \r\nK-FUSD464.00     ;UAH3708       ;;;;;;;;;;;UAH3824       ;7.9899     ;;   \r\nKFTF; UAH88       YK AE; UAH16       UD DP; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH88       YK ;UAH16       UD ;UAH12       UA ;\r\nL-\r\nM-YRT5     KK10  ;TEE6M5   KK10  \r\nN-NUC270.00;193.50\r\nO-04DEC04DEC;10DEC10DEC\r\nQ-DNK VV MOW270.00VV DNK193.50NUC463.50END ROE1.000000;FXP/ZO-10P*KK10\r\nI-005;01BOZHKO/RUSLAN MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667806\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGR 01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S6-7;P1-5\r\nTKOK01DEC/DOKC32530\r\nI-001;02MOROZOV/DMYTRO MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667807\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGR 01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S6-7;P1-5\r\nTKOK01DEC/DOKC32530\r\nI-002;03TITORENKO/OKSANA MRS;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667808\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGR 01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S6-7;P1-5\r\nTKOK01DEC/DOKC32530\r\nI-003;04TSURKAN/MYKHAYLO MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667809\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGR 01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S6-7;P1-5\r\nTKOK01DEC/DOKC32530\r\nI-004;05VAKHOVSKAYA/LILIYA MRS;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667810\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGR 01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S6-7;P1-5\r\nTKOK01DEC/DOKC32530\r\nENDX\r\n	1	2011-12-02 09:23:54: Билет 870-3544667806 успешно импортирован\r\nБилет 870-3544667807 успешно импортирован\r\nБилет 870-3544667808 успешно импортирован\r\nБилет 870-3544667809 успешно импортирован\r\nБилет 870-3544667810 успешно импортирован\r\n	0	\N	\N	\N	\N
0091962f231c4eb29758b1ef3665e0ba	Air	2	SYSTEM	2011-12-02 09:38:13	SYSTEM	2011-12-02 09:38:14	AIR_20111202073716.04856.PDT	2011-12-02 09:37:00	AIR-BLK207;MA;;225;0200009081;1A1336371;001001\r\nAMD 0201324294;1/1;VOID02DEC;SVSU\r\nGW3670821;1A1336371\r\nMUC1A ZBZOVT012;0501;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZBZOVT\r\nI-001;02MOROZOV/DMYTRO MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667807\r\nFPCASH\r\nENDX\r\n	1	2011-12-02 09:38:14: Void 870-3544667807 успешно импортирован\r\n	0	\N	\N	\N	\N
c00e89adbbc74f4a9c532e05b10018a3	Air	2	SYSTEM	2011-12-02 10:48:23	SYSTEM	2011-12-02 10:48:24	AIR_20111202084709.05567.PDT	2011-12-02 10:47:00	AIR-BLK207;RF;;190;0200013053;1A1336371;001001\r\nAMD 0201324403;1/1;    02DEC;AASU\r\nGW3671292;1A1336371\r\nMUC1A          ;  01;DOKC32530;72320942;;;;;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;\r\nB-TRFP\r\nC-7906/ 0001AASU-0001AASU-M---\r\nD-111202;111202;111202\r\nRFDF;01DEC11;I;UAH1574;0;1574;;;971;;;XT178;781;04DEC11\r\nKRF ;QUAH120      YK   ;QUAH16       UD   ;QUAH42       UA   ;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nI-001;01ORLOV/SERGIY MR;;;;\r\nT-E870-3544667784\r\nTBS1-1200\r\nR-870-3544667784;02DEC11\r\nSAC870AIKQ1ZGL8O\r\nFM5.00P\r\nFPCASH/UAH781\r\nFTITAGVV01-801\r\nENDX\r\n	1	2011-12-02 10:48:24: Возврат 870-3544667784 успешно импортирован\r\n	0	\N	\N	\N	\N
d4f046ee212244d39dd9f765f00df798	Air	2	SYSTEM	2011-12-01 15:03:57	SYSTEM	2011-12-01 15:03:57	AIR_20111201130250.02225.PDT	2011-12-01 15:02:00	AIR-BLK207;7A;;247;0100039093;1A1336371;001001\r\nAMD 0101323858;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZAUGBI007;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R6LXC \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;IEVDOK;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 V V 01DEC1950 2100 01DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD133.00     ;UAH1063       ;;;;;;;;;;;UAH1392       ;7.9897     ;;   \r\nKFTF; UAH221      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH56       YR VA; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH221      HF ;UAH4        UD ;UAH104      XT ;\r\nL-\r\nM-VOWDOM   KK10  \r\nN-USD133.20\r\nO-01DEC01DEC\r\nQ-IEV PS DOK133.20USD133.20END XT16YQ56YR20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01GOROBCHUK/SERGIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667773\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 232.00\r\nFM*M*1\r\nFPCASH\r\nFTITAGVV01-851\r\nFVPS;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1063.00\r\nRIZTICKET TAX UAH 329.00\r\nRIZTICKET TTL UAH 1392.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1452.00\r\nENDX\r\n	1	2011-12-01 15:03:57: Билет 566-3544667773 успешно импортирован\r\n	0	\N	\N	\N	\N
e3dfec72536b4c0f8a847c638249a77a	Air	2	SYSTEM	2011-12-01 15:17:52	SYSTEM	2011-12-01 15:17:52	AIR_20111201131655.02401.PDT	2011-12-01 15:16:00	AIR-BLK207;7A;;247;0100037379;1A1336371;001001\r\nAMD 0101323888;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A X5VJRV016;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV X5VJRV\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/P1/S3\r\nC-7906/ 7777SVSU-7777SVSU-M-V--\r\nD-111128;111201;111201\r\nG-   ;;DOKIEV;UA\r\nH-005;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 T T 01DEC1700 1820 01DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nU-003X;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0091 L L 01DEC0910 1025 01DEC;OK01;HK01;N ;0;735;;;;B ;;ET;0115 ;N;;347;;UA;UA;  \r\nK-RUSD225.00     ;UAH           ;;;;;;;;;;;UAH346        ;;;   \r\nKFTR;OUAH96       YQ AC;OUAH56       YK AE;OUAH8        UD DP;OUAH380      HF GO;OUAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-PD 96       YQ ;PD 56       YK ;PD 421      XT ;\r\nL-\r\nM-TPX2M5   KK10  \r\nN-USD103.50;121.50\r\nO-01DEC01DEC\r\nQ-IEV VV DOK103.50VV IEV121.50USD225.00END PD XT8UD380HF33UA;FXP/R,28NOV11/ZO-10P*KK10\r\nI-001;01GREBENYUK/NATALIYA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667774\r\nFENONENDO/REBOOK USD20;S3;P1\r\nFM*M*1\r\nFO870-3544644630DOK28NOV11/72320942/870-35446446302E2\r\nFPO/CASH+/CASH\r\nFTITAGVV01-851\r\nFVVV;S3;P1\r\nTKOK28NOV/DOKC32530//ETVV\r\nENDX\r\n	1	2011-12-01 15:17:52: Билет 870-3544667774 успешно импортирован\r\n	0	\N	\N	\N	\N
6dc698e7d41c48429603864a82c656dc	Air	2	SYSTEM	2011-12-01 09:30:28	SYSTEM	2011-12-01 09:30:28	AIR_20111201072932.59968.PDT	2011-12-01 09:29:00	AIR-BLK207;7A;;247;0100013755;1A1336371;001001\r\nAMD 0101323261;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A YWCOMO009;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YWCOMO\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-0001AASU-I-0--\r\nD-111130;111201;111201\r\nG-   ;;DOKIEV;UA\r\nH-002;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 T T 01DEC1700 1820 01DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nK-FUSD145.00     ;UAH1159       ;;;;;;;;;;;UAH1512       ;7.9897     ;;   \r\nKFTF; UAH244      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH244      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-TOW2M5         \r\nN-USD145.00\r\nO-01DEC01DEC\r\nQ-DOK VV IEV145.00USD145.00END XT48YQ36YK21UA;FXB\r\nI-001;01KANDYBA/ALINA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667751\r\nFE.-44877533.-/*M*NONENDO/REBOOK USD20 /VAT UAH 252.00;S2;P1\r\nFM*M*1A\r\nFPCASH\r\nFVVV;S2;P1\r\nTKOK30NOV/DOKC32530\r\nRIZTICKET FARE UAH 1159.00\r\nRIZTICKET TAX UAH 353.00\r\nRIZTICKET TTL UAH 1512.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1572.00\r\nENDX\r\n	1	2011-12-01 09:30:28: Билет 870-3544667751 успешно импортирован\r\n	0	\N	\N	\N	\N
f0a3221a21c34068bcfba54d27eeb0da	Air	2	SYSTEM	2011-12-01 11:27:51	SYSTEM	2011-12-01 11:27:52	AIR_20111201092642.61135.PDT	2011-12-01 11:26:00	AIR-BLK207;7A;;259;0100018919;1A1336371;001001\r\nAMD 0101323454;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A 7VHODT047;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LH 7VHODT;UA S9ZXNY;VV 7VHODT\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/T8\r\nC-7906/ 0001AASU-7777SVSU-M-E--\r\nD-111124;111201;111201\r\nG-   ;;DOKIEV;UA\r\nH-018;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 L L 22DEC1700 1820 22DEC;OK01;HK01;N ;0;733;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nU-014X;003OKBP;KIEV BORYSPIL    ;FRA;FRANKFURT        ;LH    1493 C C 23DEC0535 0740 23DEC;OK01;HK01;M ;0;320;;;;F ;0505 ;ET;0305 ; ;;988;;UA;DE;1 \r\nU-004X;004OFRA;FRANKFURT        ;DEN;DENVER           ;LH    0446 C C 23DEC1205 1415 23DEC;OK01;HK01;M ;0;744;;;;1 ;1135 ;ET;1010 ; ;;5041;;DE;US;  \r\nU-016X;005ODEN;DENVER           ;IAD;WASHINGTON DULLES;UA    0902 J J 02JAN1053 1608 02JAN;OK01;HK01;L ;0;763;;;;;;ET;0315 ;N;;1452;;US;US;  \r\nU-008X;006OIAD;WASHINGTON DULLES;MUC;MUNICH           ;LH    9281 Z Z 02JAN1731 0735 03JAN;OK01;HK01;M ;0;777;;;;;1631 ;ET;0804 ;N;;4263;;US;DE;2 \r\nX-008;006OIAD;WASHINGTON DULLES;MUC;MUNICH           ;UA    0902 Z Z 02JAN1731 0735 03JAN;OK01;HK01;M ;0;777;;;;;1631 ;ET;0804 ;N;2 \r\nY-008;006IAD;WASHINGTON DULLES;MUC;MUNICH;UA;UNITED AIRLINES;;;;;777;N\r\nU-007X;007OMUC;MUNICH           ;DOK;DONETSK          ;LH    2542 Z Z 03JAN1100 1500 03JAN;OK01;HK01;M ;0;CR9;;;;2 ;1030 ;ET;0300 ; ;;1193;;DE;UA;  \r\nY-007;007MUC;MUNICH;DOK;DONETSK;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nK-FUSD113.00     ;UAH903        ;;;;;;;;;;;UAH1204       ;7.9897     ;;   \r\nKFTF; UAH192      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH192      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-LOW1M5   KK10  \r\nN-USD112.50\r\nO-22DEC22DEC\r\nQ-DOK VV IEV112.50USD112.50END XT48YQ36YK21UA;FXP/S2/ZO-10P*KK10\r\nI-001;01MALANGONE/ANTHONY MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667759\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGR 01-801 /VAT UAH 200.67;S2;P1\r\nFM*M*1A;S2;P1\r\nFPCASH\r\nFTITAGVV01-801;S2;P1\r\nFVVV;S2;P1\r\nTKOK24NOV/DOKC32530\r\nRM PISHEM SEYCHAS///BRDS///MVZ2000////\r\nENDX\r\n	1	2011-12-01 11:27:52: Билет 870-3544667759 успешно импортирован\r\n	0	\N	\N	\N	\N
84c101414d4d43379cdca16ace4d7a84	Air	2	SYSTEM	2011-12-01 16:20:04	SYSTEM	2011-12-01 16:20:04	AIR_20111201141858.03022.PDT	2011-12-01 16:18:00	AIR-BLK207;7A;;247;0100066943;1A1336371;001001\r\nAMD 0101324007;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A YWCDY8019;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YWCDY8\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-M-E--\r\nD-111130;111201;111201\r\nG-   ;;DOKIEV;UA\r\nH-006;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 T T 01DEC1700 1820 01DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nK-FUSD131.00     ;UAH1047       ;;;;;;;;;;;UAH1377       ;7.9897     ;;   \r\nKFTF; UAH221      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH221      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-TOW2M5   KK10  \r\nN-USD130.50\r\nO-01DEC01DEC\r\nQ-DOK VV IEV130.50USD130.50END XT48YQ36YK21UA;FXB/ZO-10P*KK10\r\nI-001;01IVANISOVA/VERONIKA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667788\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 229.50\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK30NOV/DOKC32530\r\nENDX\r\n	1	2011-12-01 16:20:04: Билет 870-3544667788 успешно импортирован\r\n	0	\N	\N	\N	\N
210e47d09a7346e0b65256d8edaa4191	Air	2	SYSTEM	2011-12-01 11:00:55	SYSTEM	2011-12-01 11:00:55	AIR_20111201085949.60835.PDT	2011-12-01 10:59:00	AIR-BLK207;7A;;247;0100014507;1A1336371;001001\r\nAMD 0101323405;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A YX8YR8016;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YX8YR8\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111130;111201;111201\r\nG-   ;;DOKDOK;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 L L 02DEC1700 1820 02DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nH-002;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0091 K K 03DEC0910 1025 03DEC;OK01;HK01;N ;0;735;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nK-FUSD179.00     ;UAH1431       ;;;;;;;;;;;UAH1929       ;7.9897     ;;   \r\nKFTF; UAH305      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH305      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-LPX1M5   KK6   ;KSS1M    KK6   \r\nN-USD108.10;70.50\r\nO-02DEC02DEC;03DEC03DEC\r\nQ-DOK VV IEV108.10VV DOK70.50USD178.60END XT96YQ56YK33UA;FXP/ZO-6P*KK6\r\nI-001;01MERGALIEV/DUISEN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667752\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGREEMENT 01-851 /VAT UAH 321.50\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK30NOV/DOKC32530\r\nRIZTICKET FARE UAH 1431.00\r\nRIZTICKET TAX UAH 498.00\r\nRIZTICKET TTL UAH 1929.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1989.00\r\nENDX\r\n	1	2011-12-01 11:00:55: Билет 870-3544667752 успешно импортирован\r\n	0	\N	\N	\N	\N
80403f4bf0344c08bde970203a937ae0	Air	2	SYSTEM	2011-12-01 11:08:15	SYSTEM	2011-12-01 11:08:15	AIR_20111201090711.60913.PDT	2011-12-01 11:07:00	AIR-BLK207;7A;;247;0100016925;1A1336371;001001\r\nAMD 0101323418;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y7D845006;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y7D845\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;DOKIEV;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 Y Y 01DEC1950 2100 01DEC;OK01;HK01;N ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nK-FUSD204.00     ;UAH1630       ;;;;;;;;;;;UAH2078       ;7.9897     ;;   \r\nKFTF; UAH339      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH339      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-YOW5     KK15  \r\nN-USD204.00\r\nO-XXXX\r\nQ-DOK VV IEV204.00USD204.00END XT48YQ36YK21UA;FXP/ZO-15P*KK15\r\nI-001;01KUTSA/LESYA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667753\r\nFE.-44877533.-/VVONLY/NON ENDO KK15 AGREEMENT 01-851\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1630.00\r\nRIZTICKET TAX UAH 448.00\r\nRIZTICKET TTL UAH 2078.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2138.00\r\nENDX\r\n	1	2011-12-01 11:08:15: Билет 870-3544667753 успешно импортирован\r\n	0	\N	\N	\N	\N
3b3d17308ab643cf8186b0e908de32c2	Air	2	SYSTEM	2011-12-01 16:23:56	SYSTEM	2011-12-01 16:23:56	AIR_20111201142254.03048.PDT	2011-12-01 16:22:00	AIR-BLK207;7A;;247;0100067455;1A1336371;001001\r\nAMD 0101324013;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZCHQBK007;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZCHQBK\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;IEVDOK;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 L L 03DEC2130 2240 03DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD113.00     ;UAH903        ;;;;;;;;;;;UAH1174       ;7.9897     ;;   \r\nKFTF; UAH187      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH187      HF ;UAH4        UD ;UAH80       XT ;\r\nL-\r\nM-LOW1M5   KK10  \r\nN-USD112.50\r\nO-03DEC03DEC\r\nQ-IEV VV DOK112.50USD112.50END XT48YQ20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01PLATONOVA/NATALYA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667789\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 195.67\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 903.00\r\nRIZTICKET TAX UAH 271.00\r\nRIZTICKET TTL UAH 1174.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1234.00\r\nENDX\r\n	1	2011-12-01 16:23:56: Билет 870-3544667789 успешно импортирован\r\n	0	\N	\N	\N	\N
121057506a104479915711c0947814f3	Air	2	SYSTEM	2011-12-01 11:10:21	SYSTEM	2011-12-01 11:10:21	AIR_20111201090908.60936.PDT	2011-12-01 11:09:00	AIR-BLK207;7A;;247;0100015378;1A1336371;001001\r\nAMD 0101323424;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A Y7DFFD008;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R3ZP7 \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111201;111201;111201\r\nG-   ;;DOKDOK;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    0048 N N 05DEC0720 0830 05DEC;OK01;HK01;S ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nH-002;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 N N 06DEC1950 2100 06DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD145.00     ;UAH1159       ;;;;;;;;;;;UAH1653       ;7.9897     ;;   \r\nKFTF; UAH253      HF GO; UAH8        UD DP; UAH32       YQ AD; UAH112      YR VA; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH253      HF ;UAH8        UD ;UAH233      XT ;\r\nL-\r\nM-NFLYDOM        ;NFLYDOM        \r\nN-USD72.50;72.50\r\nO-05DEC05DEC;06DEC06DEC\r\nQ-DOK PS IEV72.50PS DOK72.50USD145.00END XT32YQ112YR56YK33UA;FXB\r\nI-001;01COCKUN/HUSAIN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /AUTO XX IF SSR TKNA/E/M/C NOT RCVD BY 1049/02DEC/IEV LT\r\nT-K566-3544667754\r\nFENON END/NO REF/RBK 20USD;S2-3;P1\r\nFM*M*1\r\nFPCASH\r\nFVPS;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nOPLDOKC32530/02DEC/3C0\r\nRIZTICKET FARE UAH 1159.00\r\nRIZTICKET TAX UAH 494.00\r\nRIZTICKET TTL UAH 1653.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1713.00\r\nENDX\r\n	1	2011-12-01 11:10:21: Билет 566-3544667754 успешно импортирован\r\n	0	\N	\N	\N	\N
d2d7cd58cc1d4922852f80d0be9e5877	Air	2	SYSTEM	2011-12-01 11:14:13	SYSTEM	2011-12-01 11:14:13	AIR_20111201091300.60973.PDT	2011-12-01 11:13:00	AIR-BLK207;7A;;257;0100030659;1A1336371;001001\r\nAMD 0101323432;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y7SQCV009;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R6H3F ;VV Y7SQCV\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/S2/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;DOKIEV;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 Y Y 01DEC1950 2100 01DEC;OK01;HK01;N ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nU-003X;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 W W 02DEC1950 2100 02DEC;OK01;HK01;S ;0;735;;;;B ;;ET;0110 ;N;;347;;UA;UA;  \r\nK-FUSD204.00     ;UAH1630       ;;;;;;;;;;;UAH2078       ;7.9897     ;;   \r\nKFTF; UAH339      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH339      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-YOW5     KK15  \r\nN-USD204.00\r\nO-XXXX\r\nQ-DOK VV IEV204.00USD204.00END XT48YQ36YK21UA;FXP/ZO-15P*KK15/S2\r\nI-001;01DAVYDOVA/NATALYA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667755\r\nFE.-44877533.-/VVONLY/NON ENDO KK15 AGREEMENT 01-851 /VAT UAH 346.33;S2;P1\r\nFM*M*1A;S2;P1\r\nFPCASH\r\nFTITAGVV01-851;S2;P1\r\nFVVV;S2;P1\r\nTKOK01DEC/DOKC32530\r\nENDX\r\n	1	2011-12-01 11:14:13: Билет 870-3544667755 успешно импортирован\r\n	0	\N	\N	\N	\N
b87b5b4ef3964c0f9094dc81d32b8f6b	Air	2	SYSTEM	2011-12-01 11:21:33	SYSTEM	2011-12-01 11:21:33	AIR_20111201092028.61063.PDT	2011-12-01 11:20:00	AIR-BLK207;7A;;257;0100015258;1A1336371;001001\r\nAMD 0101323443;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A YUGTYM019;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;SU OKWLMS;VV YUGTYM\r\nA-AEROFLOT;SU 5552\r\nB-TTP/T7/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111130;111201;111201\r\nG-X  ;;HANHAN;\r\nH-022;002OHAN;HANOI            ;SVO;MOSCOW SVO       ;SU    0542 T T 11DEC1025 1810 11DEC;OK01;HK01;L ;0;333;;;1PC;;;ET;1045 ; ;4199;VN;RU;D \r\nH-021;005OSVO;MOSCOW SVO       ;HAN;HANOI            ;SU    0541 K K 15DEC2050 0855 16DEC;OK01;HK01;D ;0;333;;;2PC;D ;;ET;0905 ; ;4199;RU;VN;  \r\nU-018X;003OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 V V 12DEC1235 1210 12DEC;OK01;HK01;L ;0;735;;;;C ;;ET;0135 ;N;;545;;RU;UA;  \r\nU-019X;004ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0301 V V 14DEC0740 1135 14DEC;OK01;HK01;L ;0;735;;;;;;ET;0155 ;N;;545;;UA;RU;C \r\nK-FUSD725.00     ;UAH5793       ;;;;;;;;;;;UAH7895       ;7.9897     ;;   \r\nKFTF; UAH1918     YQ AC; UAH72       YR VB; UAH112      JC AD;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH1918     YQ ;UAH72       YR ;UAH112      JC ;\r\nL-\r\nM-TEX            ;KEX            \r\nN-NUC;;;425.00\r\nO-11DEC11DEC;15DEC15DEC\r\nQ-HAN SU(FE)MOW300.00SU(FE)HAN425.00NUC725.00END ROE1.000000;FXP/S2,5\r\nI-001;01KRASNOBAEV/VLADIMIR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K555-3544667757\r\nFM*M*5;S2,5;P1\r\nFPCASH\r\nFVSU;S2,5;P1\r\nTKOK30NOV/DOKC32530\r\nENDX\r\n	1	2011-12-01 11:21:33: Билет 555-3544667757 успешно импортирован\r\n	0	\N	\N	\N	\N
d2699fe46e314aa8abf84b902b233180	Air	2	SYSTEM	2011-12-01 11:21:55	SYSTEM	2011-12-01 11:21:55	AIR_20111201092040.61068.PDT	2011-12-01 11:20:00	AIR-BLK207;7A;;257;0100018284;1A1336371;001001\r\nAMD 0101323445;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A YUGTYM020;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;SU OKWLMS;VV YUGTYM\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/T8/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111130;111201;111201\r\nG-X  ;;MOWMOW;E1\r\nH-018;003OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 V V 12DEC1235 1210 12DEC;OK01;HK01;L ;0;735;;;20K;C ;;ET;0135 ;N;545;RU;UA;  \r\nH-019;004ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0301 V V 14DEC0740 1135 14DEC;OK01;HK01;L ;0;735;;;20K;;;ET;0155 ;N;545;UA;RU;C \r\nU-022X;002OHAN;HANOI            ;SVO;MOSCOW SVO       ;SU    0542 T T 11DEC1025 1810 11DEC;OK01;HK01;L ;0;333;;;;;;ET;1045 ; ;;4199;;VN;RU;D \r\nU-021X;005OSVO;MOSCOW SVO       ;HAN;HANOI            ;SU    0541 K K 15DEC2050 0855 16DEC;OK01;HK01;D ;0;333;;;;D ;;ET;0905 ; ;;4199;;RU;VN;  \r\nK-FEUR160.00     ;UAH1716       ;;;;;;;;;;;UAH1894       ;10.72057   ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-VSX7     KK6   ;VSX7     KK6   \r\nN-NUC112.05;112.05\r\nO-12DEC12DEC;14DEC14DEC\r\nQ-MOW VV DOK112.05VV MOW112.05NUC224.10END ROE0.712992;FXP/ZO-6P*KK6/S3-4\r\nI-001;01KRASNOBAEV/VLADIMIR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667758\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGREEMENT 01-851;S3-4;P1\r\nFM*M*5;S3-4;P1\r\nFPCASH\r\nFTITAGVV01-851;S3-4;P1\r\nFVVV;S3-4;P1\r\nTKOK30NOV/DOKC32530\r\nENDX\r\n	1	2011-12-01 11:21:55: Билет 870-3544667758 успешно импортирован\r\n	0	\N	\N	\N	\N
6c292fdcf2c44f38a03e20733cde3aa5	Air	2	SYSTEM	2011-12-01 16:27:48	SYSTEM	2011-12-01 16:27:48	AIR_20111201142642.03084.PDT	2011-12-01 16:26:00	AIR-BLK207;7A;;239;0100067957;1A1336371;001001\r\nAMD 0101324025;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZBZV5N008;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZBZV5N\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 0001AASU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-X  ;;MOWDOK;E1\r\nH-003;002OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 K K 10DEC1235 1210 10DEC;OK01;HK01;L ;0;735;;;20K;C ;;ET;0135 ;N;545;RU;UA;  \r\nK-FEUR94.00      ;UAH1008       ;;;;;;;;;;;UAH1008       ;10.72057   ;;   \r\nTAX-;;;\r\nL-\r\nM-KOW7     KK6   \r\nN-NUC131.83\r\nO-10DEC10DEC\r\nQ-MOW VV DOK131.83NUC131.83END ROE0.712992;FXP/ZO-6P*KK6\r\nI-001;01BAYDA/VOLODYMYR MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667790\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGREEMENT 01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///MVZ4000/////\r\nENDX\r\n	1	2011-12-01 16:27:48: Билет 870-3544667790 успешно импортирован\r\n	0	\N	\N	\N	\N
7d6e0382ee2c49df93026bdce15ae605	Air	2	SYSTEM	2011-12-01 11:40:46	SYSTEM	2011-12-01 11:40:47	AIR_20111201093943.00063.PDT	2011-12-01 11:39:00	AIR-BLK207;7A;;259;0100018327;1A1336371;001001\r\nAMD 0101323481;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A 7VHODT053;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LH 7VHODT;UA S9ZXNY;VV 7VHODT\r\nA-LUFTHANSA;LH 2203\r\nB-TTP/T9\r\nC-7906/ 0001AASU-7777SVSU-I-0--\r\nD-111124;111201;111201\r\nG-X  ;;IEVDOK;\r\nH-014;003OKBP;KIEV BORYSPIL    ;FRA;FRANKFURT        ;LH    1493 C C 23DEC0535 0740 23DEC;OK01;HK01;M ;0;320;;;2PC;F ;0505 ;ET;0305 ; ;988;UA;DE;1 \r\nH-004;004XFRA;FRANKFURT        ;DEN;DENVER           ;LH    0446 C C 23DEC1205 1415 23DEC;OK01;HK01;M ;0;744;;;2PC;1 ;1135 ;ET;1010 ; ;5041;DE;US;  \r\nH-016;005ODEN;DENVER           ;IAD;WASHINGTON DULLES;UA    0902 J J 02JAN1053 1608 02JAN;OK01;HK01;L ;0;763;M;;2PC;;;ET;0315 ;N;1452;US;US;  \r\nH-008;006OIAD;WASHINGTON DULLES;MUC;MUNICH           ;LH    9281 Z Z 02JAN1731 0735 03JAN;OK01;HK01;M ;0;777;M;;2PC;;1631 ;ET;0804 ;N;4263;US;DE;2 \r\nX-008;006OIAD;WASHINGTON DULLES;MUC;MUNICH           ;UA    0902 Z Z 02JAN1731 0735 03JAN;OK01;HK01;M ;0;777;;;2PC;;1631 ;ET;0804 ;N;2 \r\nY-008;006IAD;WASHINGTON DULLES;MUC;MUNICH;UA;UNITED AIRLINES;;;;;777;N\r\nH-007;007XMUC;MUNICH           ;DOK;DONETSK          ;LH    2542 Z Z 03JAN1100 1500 03JAN;OK01;HK01;M ;0;CR9;;LUFTHANSA CITYLINE;2PC;2 ;1030 ;ET;0300 ; ;1193;DE;UA;  \r\nY-007;007MUC;MUNICH;DOK;DONETSK;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nU-018X;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 L L 22DEC1700 1820 22DEC;OK01;HK01;N ;0;733;;;;;;ET;0120 ;N;;347;;UA;UA;B \r\nK-FUSD7948.00    ;UAH63503      ;;;;;;;;;;;UAH67794      ;7.9897     ;;   \r\nKFTF; UAH136      YK AE; UAH16       UD DP; UAH3132     YQ AC; UAH32       UA SE; UAH372      RA EB; UAH125      DE SE; UAH44       YC AE; UAH131      US AP; UAH131      US AS; UAH40       XA CO; UAH56       XY CR; UAH40       AY SE; UAH36       XF   ;;;;;;;;;;;;;;;;;\r\nTAX-UAH136      YK ;UAH16       UD ;UAH4139     XT ;\r\nL-*LH *\r\nM-C77RT          ;C77RT          ;JUA            ;ZFFUAW         ;ZFFUAW         \r\nN-NUC;4576.50;1639.08;;1732.50\r\nO-XX23DEC;XX23DEC;XXXX;XX23DEC;XX23DEC\r\nQ-IEV LH X/FRA LH DEN M4576.50UA WAS Q55.82 1583.26LH X/MUC LH DOK1732.50NUC7948.08END ROE1.000000XT3132YQ32UA372RA125DE44YC131US131US40XA56XY40AY36XF IAD4.5;FXP/S3-7/R,VC-LH\r\nI-001;01MALANGONE/ANTHONY MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nSSR DOCS VV  HK1/P/USA/476084605/USA/20APR52/M/16SEP20/MALANGONE/ANTHONY/H;P1\r\nSSR DOCS LH  HK1/P/USA/476084605/USA/20APR52/M/16SEP20/MALANGONE/ANTHONY/H;P1\r\nSSR DOCS UA  HK1/P/USA/476084605/USA/20APR52/M/16SEP20/MALANGONE/ANTHONY/H;P1\r\nOSI LH  DS/EXP/UA23969\r\nT-K220-3544667760-61\r\nFEFL/CNX/CHG RESTRICTED CHECK FARE NOTE;S3-7;P1\r\nFM*M*1A;S3-7;P1\r\nFPCASH\r\nFTUA0299\r\nFVLH;S3-7;P1\r\nTKOK24NOV/DOKC32530\r\nRM PISHEM SEYCHAS///BRDS///MVZ2000////\r\nENDX\r\n	1	2011-12-01 11:40:47: Билет 220-3544667760 успешно импортирован\r\n	0	\N	\N	\N	\N
d6017102bae34ba2951aebd370476c25	Air	2	SYSTEM	2011-12-01 11:43:35	SYSTEM	2011-12-01 11:43:35	AIR_20111201094227.00095.PDT	2011-12-01 11:42:00	AIR-BLK207;7A;;239;0100018699;1A1336371;001001\r\nAMD 0101323484;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A Y6JC3S009;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y6JC3S\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP\r\nC-7906/ 0001AASU-7777SVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;DOKDOK;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 L L 02DEC1700 1820 02DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nH-002;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 L L 04DEC2130 2240 04DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD207.00     ;UAH1654       ;;;;;;;;;;;UAH2198       ;7.9897     ;;   \r\nKFTF; UAH351      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH351      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-LPX1M5   KK10  ;LPX1M5   KK10  \r\nN-USD103.50;103.50\r\nO-02DEC02DEC;04DEC04DEC\r\nQ-DOK VV IEV103.50VV DOK103.50USD207.00END XT96YQ56YK33UA;FXP/ZO-10P*KK10\r\nI-001;01SEMICHEV/DMYTRO MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667762\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGR 01-801 /VAT UAH 366.33\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS////OPLATA NALICHNYMI////\r\nRIZTICKET FARE UAH 1654.00\r\nRIZTICKET TAX UAH 544.00\r\nRIZTICKET TTL UAH 2198.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2258.00\r\nENDX\r\n	1	2011-12-01 11:43:35: Билет 870-3544667762 успешно импортирован\r\n	0	\N	\N	\N	\N
89fc4430cf744f0d8ece2e29705e5c80	Air	2	SYSTEM	2011-12-01 11:54:03	SYSTEM	2011-12-01 11:54:03	AIR_20111201095308.00252.PDT	2011-12-01 11:53:00	AIR-BLK207;7A;;247;0100021485;1A1336371;001001\r\nAMD 0101323513;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y8G4ND006;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y8G4ND\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;DOKDOK;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 Y Y 01DEC1950 2100 01DEC;OK01;HK01;N ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nH-002;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0093 Q Q 05DEC0700 0820 05DEC;OK01;HK01;N ;0;734;;;20K;B ;;ET;0120 ;N;347;UA;UA;  \r\nK-FUSD293.00     ;UAH2341       ;;;;;;;;;;;UAH3021       ;7.9897     ;;   \r\nKFTF; UAH487      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH487      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-YRT5     KK10  ;QPX1M5   KK10  \r\nN-USD207.00;85.50\r\nO-01DEC01DEC;05DEC05DEC\r\nQ-DOK VV IEV207.00VV DOK85.50USD292.50END XT96YQ56YK33UA;FXP/ZO-10P*KK10\r\nI-001;01KUBLITSKIY/IGOR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667763\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 503.50\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 2341.00\r\nRIZTICKET TAX UAH 680.00\r\nRIZTICKET TTL UAH 3021.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 3081.00\r\nENDX\r\n	1	2011-12-01 11:54:03: Билет 870-3544667763 успешно импортирован\r\n	0	\N	\N	\N	\N
8e81330698f24c3f88f2b7df244d32a9	Air	2	SYSTEM	2011-12-01 16:19:42	SYSTEM	2011-12-01 16:19:42	AIR_20111201141832.03017.PDT	2011-12-01 16:18:00	AIR-BLK207;7A;;247;0100046521;1A1336371;001001\r\nAMD 0101324006;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A X28Y9H017;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV X28Y9H\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111128;111201;111201\r\nG-   ;;DOKIEV;UA\r\nH-003;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 C C 01DEC1950 2100 01DEC;OK01;HK01;S ;0;735;;;30K;;;ET;0110 ;N;347;UA;UA;B \r\nK-FUSD288.00     ;UAH2301       ;;;;;;;;;;;UAH2883       ;7.9897     ;;   \r\nKFTF; UAH473      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH473      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-COW5     KK20  \r\nN-USD288.00\r\nO-XXXX\r\nQ-DOK VV IEV288.00USD288.00END XT48YQ36YK21UA;FXP/ZO-20P*KK20\r\nI-001;01TOLKACH/ALEXANDER MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870012009794;S2;P1\r\nT-K870-3544667787\r\nFE.-44877533.-/VVONLY/NON ENDO KK20 AGREEMENT 01-851 /VAT UAH 480.50\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK29NOV/IEVVV0009\r\nRIZTICKET FARE UAH 2301.00\r\nRIZTICKET TAX UAH 582.00\r\nRIZTICKET TTL UAH 2883.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2943.00\r\nENDX\r\n	1	2011-12-01 16:19:42: Билет 870-3544667787 успешно импортирован\r\n	0	\N	\N	\N	\N
9a12bb340c004220b1f8722630b34d6c	Air	2	SYSTEM	2011-12-01 12:18:24	SYSTEM	2011-12-01 12:18:24	AIR_20111201101711.00518.PDT	2011-12-01 12:17:00	AIR-BLK207;7A;;247;0100038889;1A1336371;001001\r\nAMD 0101323559;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A YR2YQ6014;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YR2YQ6\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111130;111201;111201\r\nG-   ;;DOKDOK;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0092 C C 06DEC0920 1045 06DEC;OK01;HK01;S ;0;735;;;30K;;;ET;0125 ;N;347;UA;UA;B \r\nH-002;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 J J 06DEC2130 2240 06DEC;OK01;HK01;S ;0;320;;;30K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD486.00     ;UAH3883       ;;;;;;;;;;;UAH4872       ;7.9897     ;;   \r\nKFTF; UAH796      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH796      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-CRT5     KK10  ;JPX3M5   KK10  \r\nN-USD301.50;184.50\r\nO-XX06MAR;XX06MAR\r\nQ-DOK VV IEV301.50VV DOK184.50USD486.00END XT96YQ56YK33UA;FXP/ZO-10P*KK10\r\nI-001;01DETYUK/SERGIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870000616303;S2;P1\r\nFQV VV  FQTV-VV870000616303;S3;P1\r\nT-K870-3544667764\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 812.00\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK30NOV/DOKC32530\r\nRIZTICKET FARE UAH 3883.00\r\nRIZTICKET TAX UAH 989.00\r\nRIZTICKET TTL UAH 4872.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 4932.00\r\nENDX\r\n	1	2011-12-01 12:18:24: Билет 870-3544667764 успешно импортирован\r\n	0	\N	\N	\N	\N
e54f21867d0d45e1bd218f8010c35811	Air	2	SYSTEM	2011-12-02 11:03:46	SYSTEM	2011-12-02 11:03:46	AIR_20111202090244.05743.PDT	2011-12-02 11:02:00	AIR-BLK207;7M;;247;0200006114;1A1336371;001001\r\nAMD 0201324432;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A YX8YR8021;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YX8YR8\r\nB-TTM/RT\r\nC-7906/ 2222MVSU-0001AASU-M---\r\nD-111130;111202;111202\r\nU-001X;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 L L 02DEC1700 1820 02DEC;OK01;HK01;N ;0;320;;;;;;ET;0120 ;N;;347;;UA;UA;B \r\nU-002X;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0091 K K 03DEC0910 1025 03DEC;OK01;HK01;N ;0;735;;;;B ;;ET;0115 ;N;;347;;UA;UA;  \r\nMCO100;004VV;8702;AEROSVIT AIRLINES;DOK;TO-AEROSVIT AIRLINES;AT-DONETSK;02DEC;M;A;PENALTY FOR REFUND;**-;;F;UAH        240;N;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;UAH        240;;;870-3544667752;;;;UAH        240;;;; ;P1\r\nI-001;01MERGALIEV/DUISEN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nTMCM870-9074146868;;L4\r\nMFM*M*0;L4\r\nMFPCASH;L4\r\nTKOK01DEC/DOKC32530//ETVV\r\nRIZTICKET FARE UAH 1431.00\r\nRIZTICKET TAX UAH 498.00\r\nRIZTICKET TTL UAH 1929.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1989.00\r\nENDX\r\n	1	2011-12-02 11:03:46: MCO 870-9074146868 успешно импортирован\r\n	0	\N	\N	\N	\N
00a59a609caa41df83fcddb801945096	Air	2	SYSTEM	2011-12-01 12:52:31	SYSTEM	2011-12-01 12:52:31	AIR_20111201105131.00947.PDT	2011-12-01 12:51:00	AIR-BLK207;7A;;239;0100042957;1A1336371;001001\r\nAMD 0101323631;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A YXGCRV014;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LH YXGCRV\r\nA-LUFTHANSA;LH 2203\r\nB-TTP\r\nC-7906/ 0001AASU-7777SVSU-I-0--\r\nD-111130;111201;111201\r\nG-X  ;;ZAGZAG;E1\r\nH-005;002OZAG;ZAGREB           ;MUC;MUNICH           ;LH    5989 M M 06DEC0830 0945 06DEC;OK01;HK01;R ;0;DH4;;;1PC;;0800 ;ET;0115 ;N;271;HR;DE;2 \r\nX-005;002OZAG;ZAGREB           ;MUC;MUNICH           ;OU    4436 Y M 06DEC0830 0945 06DEC;OK01;HK01;R ;0;DH4;;;1PC;;0800 ;ET;0115 ;N;2 \r\nY-005;002ZAG;ZAGREB;MUC;MUNICH;OU;CROATIA AIRLINES;;;;;DH4;N\r\nH-006;003XMUC;MUNICH           ;DOK;DONETSK          ;LH    2542 M M 06DEC1100 1500 06DEC;OK01;HK01;S ;0;CR9;;LUFTHANSA CITYLINE;1PC;2 ;1020 ;ET;0300 ; ;1193;DE;UA;  \r\nY-006;003MUC;MUNICH;DOK;DONETSK;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nH-007;004ODOK;DONETSK          ;MUC;MUNICH           ;LH    2543 M M 07DEC1540 1750 07DEC;OK01;HK01;M ;0;CR9;;LUFTHANSA CITYLINE;1PC;;1455 ;ET;0310 ; ;1193;UA;DE;2 \r\nY-007;004DOK;DONETSK;MUC;MUNICH;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nH-008;005XMUC;MUNICH           ;ZAG;ZAGREB           ;LH    5998 M M 07DEC2115 2225 07DEC;OK01;HK01;R ;0;DH4;;;1PC;2 ;2035 ;ET;0110 ;N;271;DE;HR;  \r\nX-008;005XMUC;MUNICH           ;ZAG;ZAGREB           ;OU    0437 Y M 07DEC2115 2225 07DEC;OK01;HK01;R ;0;DH4;;;1PC;2 ;2035 ;ET;0110 ;N;  \r\nY-008;005MUC;MUNICH;ZAG;ZAGREB;OU;CROATIA AIRLINES;;;;;DH4;N\r\nK-FEUR840.00     ;UAH9006       ;;;;;;;;;;;UAH11359      ;10.72057   ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH1544     YQ AC; UAH42       UA SE; UAH156      HR AE; UAH15       MI CA; UAH348      RA EB; UAH112      DE SE;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH2217     XT ;\r\nL-\r\nM-MFF64D1        ;MFF64D1        ;MFF64D1        ;MFF64D1        \r\nN-NUC;589.06;;589.06\r\nO-XXXX;XXXX;07DECXX;07DECXX\r\nQ-ZAG LH X/MUC LH DOK589.06LH X/MUC LH ZAG589.06NUC1178.12END ROE0.712992XT1544YQ42UA156HR15MI348RA112DE;FXP\r\nI-001;01PLAZIBAT/MARIO MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nSSR OTHS 1A  /PLS ISS AUTOMATIC TKT BY 05DEC11/0530Z OR ALL LH SEGS WILL BE XLD. APPLICABLE FARE RULE APPLIES IF IT\r\nSSR OTHS 1A  ////DEMANDS EARLIER TKTG.XE/I FF\r\nOSI LH  DS/EXP/UA23969\r\nT-K220-3544667765\r\nFEFL/CHG RESTRICTED CHECK FARE NOTE;S2-5;P1\r\nFM*M*1A\r\nFPCASH\r\nFTUA0299\r\nFVLH;S2-5;P1\r\nTKOK30NOV/DOKC32530\r\nRM PISHEM SEYCHAS////MVZ//4000////\r\nENDX\r\n	1	2011-12-01 12:52:31: Билет 220-3544667765 успешно импортирован\r\n	0	\N	\N	\N	\N
a72b6f8870c14c3991a82bd343efa2c4	Air	2	SYSTEM	2011-12-01 13:02:17	SYSTEM	2011-12-01 13:02:17	AIR_20111201110115.01070.PDT	2011-12-01 13:01:00	AIR-BLK207;7M;;247;0100028514;1A1336371;001001\r\nAMD 0101323658;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A Y9LRFP005;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y9LRFP\r\nB-TTM\r\nC-7906/ 7777SVSU-7777SVSU----\r\nD-111201;111201;111201\r\nU-001X;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0091 K K 18DEC0910 1025 18DEC;OK01;HK01;N ;0;735;;;;B ;;ET;0115 ;N;;347;;UA;UA;  \r\nMCO057;003VV;8702;AEROSVIT AIRLINES;KBP;TO-AEROSVIT AIRLINES;AT-KIEV;18DEC;M;A;Q;**-;;F;UAH        240;N;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;UAH        240;;;;;;;UAH        240;;;; ;P1\r\nI-001;01PELEKH/VOLODYMYR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nTMCM870-9074146866;;L3\r\nMFM*M*0;L3\r\nMFPCASH;L3\r\nTKOK01DEC/DOKC32530\r\nENDX\r\n	1	2011-12-01 13:02:17: MCO 870-9074146866 успешно импортирован\r\n	0	\N	\N	\N	\N
36b565f6fd0f489db79844e70ccd83a4	Air	2	SYSTEM	2011-12-01 13:16:15	SYSTEM	2011-12-01 13:16:15	AIR_20111201111511.01193.PDT	2011-12-01 13:15:00	AIR-BLK207;7A;;247;0100045721;1A1336371;001001\r\nAMD 0101323678;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y83SMX006;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y83SMX\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-001;002ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0301 Q Q 03DEC0740 1135 03DEC;OK01;HK01;L ;0;735;;;20K;;;ET;0155 ;N;545;UA;RU;C \r\nH-002;003OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0306 Q Q 04DEC2245 2230 04DEC;OK01;HK01;L ;0;735;;;20K;C ;;ET;0145 ;N;545;RU;UA;  \r\nK-FUSD207.00     ;UAH1654       ;;;;;;;;;;;UAH1832       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-Q2MRT    KK10  ;Q2MRT    KK10  \r\nN-NUC103.50;103.50\r\nO-XX03FEB;XX03FEB\r\nQ-DOK VV MOW103.50VV DOK103.50NUC207.00END ROE1.000000;FXP/ZO-10P*KK10\r\nI-001;01CHUKHLEBOV/VADYM MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667767\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1654.00\r\nRIZTICKET TAX UAH 178.00\r\nRIZTICKET TTL UAH 1832.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 1952.00\r\nENDX\r\n	1	2011-12-01 13:16:15: Билет 870-3544667767 успешно импортирован\r\n	0	\N	\N	\N	\N
3718653bc1c14f3695e89fc72c6fe66b	Air	2	SYSTEM	2011-12-01 13:18:42	SYSTEM	2011-12-01 13:18:42	AIR_20111201111746.01220.PDT	2011-12-01 13:17:00	AIR-BLK207;7A;;247;0100027720;1A1336371;001001\r\nAMD 0101323688;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A YZAU4E017;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YZAU4E\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 0001AASU-2222MVSU-M-E--\r\nD-111130;111201;111201\r\nG-   ;;DOKDOK;UA\r\nH-003;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 Y Y 01DEC1950 2100 01DEC;OK01;HK01;N ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nH-002;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 Q Q 03DEC2130 2240 03DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD293.00     ;UAH2341       ;;;;;;;;;;;UAH3021       ;7.9897     ;;   \r\nKFTF; UAH487      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH487      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-YRT5     KK10  ;QPX1M5   KK10  \r\nN-USD207.00;85.50\r\nO-01DEC01DEC;03DEC03DEC\r\nQ-DOK VV IEV207.00VV DOK85.50USD292.50END XT96YQ56YK33UA;FXP/ZO-10P*KK10\r\nI-001;01BONDARENKO/YEVGEN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870000616183;S2;P1\r\nFQV VV  FQTV-VV870000616183;S3;P1\r\nT-K870-3544667768\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 503.50\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK30NOV/DOKC32530\r\nRIZTICKET FARE UAH 2341.00\r\nRIZTICKET TAX UAH 680.00\r\nRIZTICKET TTL UAH 3021.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 3081.00\r\nENDX\r\n	1	2011-12-01 13:18:42: Билет 870-3544667768 успешно импортирован\r\n	0	\N	\N	\N	\N
f0dde9e19bfe43058b72ea824ea68b3c	Air	2	SYSTEM	2011-12-01 13:20:29	SYSTEM	2011-12-01 13:20:29	AIR_20111201111924.01232.PDT	2011-12-01 13:19:00	AIR-BLK207;7A;;257;0100046148;1A1336371;001001\r\nAMD 0101323692;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A 8R8YEA024;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LH 8R8YEA;OS 8R8YEA\r\nA-AUSTRIAN;OS 2575\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111128;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-001;002ODOK;DONETSK          ;VIE;VIENNA SCHWECHAT ;OS    0640 Q Q 11DEC1445 1635 11DEC;OK01;HK01;M ;0;F70;;TYROLEAN AIRWAYS;1PC;;1400 ;ET;0250 ;N;979;UA;AT;  \r\nY-001;002DOK;DONETSK;VIE;VIENNA SCHWECHAT;VO;TYROLEAN AIRWAYS;;;;;F70;N\r\nH-002;003XVIE;VIENNA SCHWECHAT ;CDG;PARIS CDG        ;OS    0417 Q Q 11DEC1715 1930 11DEC;OK01;HK01;S ;0;321;;;1PC;;1630 ;ET;0215 ;N;643;AT;FR;2D\r\nH-003;004OCDG;PARIS CDG        ;MUC;MUNICH           ;LH    2227 V V 17DEC0845 1015 17DEC;OK01;HK01;S ;0;CR9;;LUFTHANSA CITYLINE;1PC;1 ;0805 ;ET;0130 ; ;425;FR;DE;2 \r\nY-003;004CDG;PARIS CDG;MUC;MUNICH;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nH-004;005XMUC;MUNICH           ;DOK;DONETSK          ;LH    2542 V V 17DEC1100 1500 17DEC;OK01;HK01;S ;0;CR9;;LUFTHANSA CITYLINE;1PC;2 ;1020 ;ET;0300 ; ;1193;DE;UA;  \r\nY-004;005MUC;MUNICH;DOK;DONETSK;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nK-FUSD492.00     ;UAH3931       ;;;;;;;;;;;UAH6404       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH1544     YQ AC; UAH42       UA SE; UAH182      ZY AE; UAH49       AT SE; UAH121      QX AP; UAH43       IZ EB; UAH45       FR SE; UAH137      FR TI; UAH174      RA EB;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH2337     XT ;\r\nL-\r\nM-QFLYOS1U       ;QFLYOS1U       ;VNC66W3        ;VNC66W3        \r\nN-NUC;289.50;;202.00\r\nO-11DEC11DEC;11DEC11DEC;17DEC17DEC;17DEC17DEC\r\nQ-DOK OS X/VIE OS PAR289.50LH X/MUC LH DOK202.00NUC491.50END ROE1.000000XT1544YQ42UA182ZY49AT121QX43IZ45FR137FR174RA;FXP\r\nI-001;01KLYMENKO/OLENA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV OS  FQTV2-LH992229993794150;P1\r\nFQV LH  FQTV2-LH992229993794150;P1\r\nOSI OS  DS/EXP/UA23969\r\nOSI LH  DS/EXP/UA23969\r\nT-K257-3544667769\r\nFENONREF/FL/CHG RESTRICTEDCHECK FARE NOTE;S2-5;P1\r\nFM*M*1A\r\nFPCASH\r\nFTUA0331\r\nFVOS;S2-5;P1\r\nTKOK28NOV/DOKC32530\r\nRIZTICKET FARE UAH 3931.00\r\nRIZTICKET TAX UAH 2473.00\r\nRIZTICKET TTL UAH 6404.00\r\nRIZSERVICE FEE UAH 250.00+VAT 50.00\r\nRIZGRAND TOTAL UAH 6704.00\r\nENDX\r\n	1	2011-12-01 13:20:29: Билет 257-3544667769 успешно импортирован\r\n	0	\N	\N	\N	\N
f046f248f20a4d5fb92f5155fdd2f7d2	Air	2	SYSTEM	2011-12-01 13:27:27	SYSTEM	2011-12-01 13:27:27	AIR_20111201112618.01308.PDT	2011-12-01 13:26:00	AIR-BLK207;7A;;257;0100046969;1A1336371;001001\r\nAMD 0101323702;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A YJXZIH026;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS RN07S ;VV YJXZIH\r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/P1/S3\r\nC-7906/ 7777SVSU-7777SVSU-M-V--\r\nD-111129;111201;111201\r\nG-   ;;IEVDOK;UA\r\nH-007;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 H H 02DEC1950 2100 02DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nU-003X;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0092 L L 30NOV0920 1045 30NOV;OK01;HK01;N ;0;734;;;;;;ET;0125 ;N;;347;;UA;UA;B \r\nK-RUSD156.00     ;UAH           ;;;;;;;;;;;UAH392        ;;;   \r\nKFTR;OUAH16       YQ AD;OUAH56       YR VA;OUAH20       YK AE;OUAH4        UD DP;OUAH256      HF GO;OUAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-PD 16       YQ ;PD 56       YR ;PD 292      XT ;\r\nL-\r\nM-HOWDOM   KK10  \r\nN-USD155.70\r\nO-02DEC02DEC\r\nQ-IEV PS DOK155.70USD155.70END PD XT20YK4UD256HF12UA;FXP/S3/R,29NOV11/ZO-10P*KK10\r\nI-001;01MASLOV/IGOR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667770\r\nFH566-3544644668;P1\r\nFENON END/REF REST/RBK 10USD;S3;P1\r\nFM*M*1;S3;P1\r\nFO566-3544644668DOK29NOV11/72320942/566-35446446684E1\r\nFPO/CASH+/CASH\r\nFVPS;S3;P1\r\nTKOK29NOV/DOKC32530//ETPS\r\nENDX\r\n	1	2011-12-01 13:27:27: Билет 566-3544667770 успешно импортирован\r\n	0	\N	\N	\N	\N
46c009896bec4fdbad684550c826e5b1	Air	2	SYSTEM	2011-12-01 13:35:08	SYSTEM	2011-12-01 13:35:08	AIR_20111201113407.01385.PDT	2011-12-01 13:34:00	AIR-BLK207;7M;;257;0100027614;1A1336371;001001\r\nAMD 0101323727;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A YJXZIH032;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS RN07S ;VV YJXZIH\r\nB-TTM\r\nC-7906/ 7777SVSU-2222MVSU-M---\r\nD-111129;111201;111201\r\nU-003X;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0092 L L 30NOV0920 1045 30NOV;OK01;HK01;N ;0;734;;;;;;ET;0125 ;N;;347;;UA;UA;B \r\nU-007X;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 H H 02DEC1950 2100 02DEC;OK01;HK01;S ;0;735;;;;B ;;ET;0110 ;N;;347;;UA;UA;  \r\nMCO228;004PS;5666;UKRAINE INTL AIRLINES;KBP;TO-UKRAINE INTL AIRLINES;AT-KIEV;02DEC;M;A;PENALTY FOR CHANGE;**-;PENALTY FOR CHANGE;F;UAH        160;N;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;UAH        160;;;566-3544644668;;;;UAH        160;;;; ;P1\r\nI-001;01MASLOV/IGOR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nTMCM566-9074146867;;L4\r\nMFM*M*0;L4\r\nMFPCASH;L4\r\nENDX\r\n	1	2011-12-01 13:35:08: MCO 566-9074146867 успешно импортирован\r\n	0	\N	\N	\N	\N
eb66940328544331b267e7e3ef096e7f	Air	2	SYSTEM	2011-12-01 13:55:20	SYSTEM	2011-12-01 13:55:20	AIR_20111201115405.01587.PDT	2011-12-01 13:54:00	AIR-BLK207;RF;;190;0100034234;1A1336371;001001\r\nAMD 0101323764;1/1;    01DEC;SVSU\r\nGW3670821;1A1336371\r\nMUC1A          ;  01;DOKC32530;72320942;;;;;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;\r\nB-TRFP\r\nC-7906/ 7777SVSU-7777SVSU-M---\r\nD-111201;111201;111201\r\nRFDF;29NOV11;D;UAH903;0;903;;;;;;XT301;1204;01DEC11\r\nKRF ;QUAH192      HF   ;QUAH4        UD   ;QUAH48       YQ   ;QUAH36       YK   ;QUAH21       UA   ;;;;;;;;;;;;;;;;;;;;;;;;;\r\nI-001;01KHASANOV BATYROV/TYMUR MR;;;;\r\nT-E870-3544644658\r\nTBS1-1000\r\nR-870-3544644658;01DEC11\r\nSAC870AI7YNT9L4I\r\nFM1A\r\nFPCASH/UAH1204\r\nFTITAGVV01-851\r\nENDX\r\n	1	2011-12-01 13:55:20: Возврат 870-3544644658 успешно импортирован\r\n	0	\N	\N	\N	\N
9873c846519d45d19553b32bbd16875b	Air	2	SYSTEM	2011-12-01 14:06:09	SYSTEM	2011-12-01 14:06:09	AIR_20111201120506.01674.PDT	2011-12-01 14:05:00	AIR-BLK207;RF;;190;0100032910;1A1336371;001001\r\nAMD 0101323773;1/1;    01DEC;SVSU\r\nGW3670821;1A1336371\r\nMUC1A          ;  01;DOKC32530;72320942;;;;;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;\r\nB-TRFP\r\nC-7906/ 7777SVSU-7777SVSU-I---\r\nD-111201;111201;111201\r\nRFDM;24OCT11;I;UAH4866;4282;584;;;;;;XT384;968;03NOV11\r\nKRF ;QUAH136      YK   ;QUAH16       UD   ;QUAH200      YQ   ;QUAH32       UA   ;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nI-001;01KOPOLOVETS/MAKSYM MR;;;;\r\nT-E870-3544506088\r\nTBS1-0004\r\nR-870-3544506088;01DEC11\r\nSAC870AI8CFYEQCO\r\nFM1.00P\r\nFPCASH/UAH968\r\nENDX\r\n	1	2011-12-01 14:06:09: Возврат 870-3544506088 успешно импортирован\r\n	0	\N	\N	\N	\N
829d0d618d0f4e2f8a3e289749759eda	Air	2	SYSTEM	2011-12-01 14:30:10	SYSTEM	2011-12-01 14:30:10	AIR_20111201122911.01894.PDT	2011-12-01 14:29:00	AIR-BLK207;7A;;247;0100054185;1A1336371;001001\r\nAMD 0101323805;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A ZASSJ7005;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;TK SN77AK\r\nA-TURKISH AIRLINES;TK 2354\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111201;111201;111201\r\nG-X  ;;DOKECN;E1\r\nH-003;002ODOK;DONETSK          ;IST;ISTANBUL         ;TK    0454 W W 02DEC1200 1405 02DEC;OK01;HK01;M ;0;73W;;;40K;;;ET;0205 ;N;660;UA;TR;I \r\nH-004;003XIST;ISTANBUL         ;ECN;ERCAN            ;TK    0962 Y Y 02DEC1835 2010 02DEC;OK01;HK01;S ;0;320;;;40K;I ;;ET;0135 ;N;474;TR;CY;  \r\nK-FUSD119.00     ;UAH951        ;;;;;;;;;;;UAH1763       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH580      YR VA; UAH42       UA SE; UAH54       TR AE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH676      XT ;\r\nL-\r\nM-WCDPROW        ;WCDPROW        \r\nN-NUC;119.00\r\nO-02DEC02DEC;02DEC02DEC\r\nQ-DOK TK X/IST TK ECN119.00NUC119.00END ROE1.000000XT580YR42UA54TR;FXB\r\nI-001;01MALKAWI/MOHAMMED MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K235-3544667771\r\nFENONEND/TK ONLY;S2-3;P1\r\nFM*M*1\r\nFPCASH\r\nFVTK;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 951.00\r\nRIZTICKET TAX UAH 812.00\r\nRIZTICKET TTL UAH 1763.00\r\nRIZSERVICE FEE UAH 250.00+VAT 50.00\r\nRIZGRAND TOTAL UAH 2063.00\r\nENDX\r\n	1	2011-12-01 14:30:10: Билет 235-3544667771 успешно импортирован\r\n	0	\N	\N	\N	\N
957bfedb900643cc97b843a75043f301	Air	2	SYSTEM	2011-12-01 14:45:08	SYSTEM	2011-12-01 14:45:08	AIR_20111201124406.02043.PDT	2011-12-01 14:44:00	AIR-BLK207;7A;;247;0100035091;1A1336371;001001\r\nAMD 0101323829;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A Y9NDZQ005;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;UN P507N \r\nA-TRANSAERO AIRLINES;UN 6705\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111201;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-001;002ODOK;DONETSK          ;DME;MOSCOW DME       ;UN    0244 W W 04DEC1720 2050 04DEC;OK01;HK01;S ;0;735;;;20K;;;ET;0130 ;N;506;UA;RU;  \r\nH-002;003ODME;MOSCOW DME       ;MQF;MAGNITOGORSK     ;UN    0165 W W 05DEC0205 0615 05DEC;OK01;HK01;D ;0;735;;;20K;;;ET;0210 ;N;861;RU;RU;  \r\nH-003;004OMQF;MAGNITOGORSK     ;DME;MOSCOW DME       ;UN    0166 W W 06DEC0730 0805 06DEC;OK01;HK01;D ;0;735;;;20K;;;ET;0235 ;N;861;RU;RU;  \r\nH-004;005ODME;MOSCOW DME       ;DOK;DONETSK          ;UN    0243 W W 06DEC1045 1025 06DEC;OK01;HK01;S ;0;735;;;20K;;;ET;0140 ;N;506;RU;UA;  \r\nK-FUSD248.00     ;UAH1982       ;;;;;;;;;;;UAH3534       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH730      YQ AC; UAH644      YR VB; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH1416     XT ;\r\nL-\r\nM-WPR2M          ;WPR2M          ;WPR2M          ;WPR2M          \r\nN-NUC50.00;;88;;88;50.00\r\nO-04DEC04DEC;05DEC05DEC;06DEC06DEC;06DEC06DEC\r\nQ-DOK UN MOW50.00UN(FE)MQF73.88UN(FE)MOW73.88UN DOK50.00NUC247.76END ROE1.000000XT730YQ644YR42UA;FXP\r\nI-001;01ZHIRLITSYN/VOLODYMYR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K670-3544667772\r\nFM*M*5\r\nFPCASH\r\nFVUN;S2-5;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1982.00\r\nRIZTICKET TAX UAH 1552.00\r\nRIZTICKET TTL UAH 3534.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 3654.00\r\nENDX\r\n	1	2011-12-01 14:45:08: Билет 670-3544667772 успешно импортирован\r\n	0	\N	\N	\N	\N
001de11c56354cca869956aa6ebfdcb6	Air	2	SYSTEM	2011-12-01 15:21:43	SYSTEM	2011-12-01 15:21:43	AIR_20111201132036.02431.PDT	2011-12-01 15:20:00	AIR-BLK207;7A;;247;0100040775;1A1336371;001001\r\nAMD 0101323892;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A YYJN6J023;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;OS YYJN6J\r\nA-AUSTRIAN;OS 2575\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111130;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-017;002ODOK;DONETSK          ;VIE;VIENNA SCHWECHAT ;OS    0640 K K 14DEC1445 1635 14DEC;OK01;HK01;M ;0;100;;TYROLEAN AIRWAYS;1PC;;1400 ;ET;0250 ;N;979;UA;AT;  \r\nY-017;002DOK;DONETSK;VIE;VIENNA SCHWECHAT;VO;TYROLEAN AIRWAYS;;;;;100;N\r\nH-018;003XVIE;VIENNA SCHWECHAT ;CDG;PARIS CDG        ;OS    0417 K K 14DEC1715 1930 14DEC;OK01;HK01;S ;0;320;;;1PC;;1630 ;ET;0215 ;N;643;AT;FR;2D\r\nH-019;004OCDG;PARIS CDG        ;VIE;VIENNA SCHWECHAT ;OS    7112 T T 18DEC0715 0915 18DEC;OK01;HK01;;0;320;;;1PC;2D;0630 ;ET;0200 ;N;643;FR;AT;2D\r\nX-019;004OCDG;PARIS CDG        ;VIE;VIENNA SCHWECHAT ;AF    1138 Y T 18DEC0715 0915 18DEC;OK01;HK01;;0;320;;;1PC;2D;0630 ;ET;0200 ;N;2D\r\nY-019;004CDG;PARIS CDG;VIE;VIENNA SCHWECHAT;AF;AIR FRANCE;;;;;320;N\r\nH-020;005XVIE;VIENNA SCHWECHAT ;DOK;DONETSK          ;OS    0639 T T 18DEC1015 1340 18DEC;OK01;HK01;M ;0;F70;;TYROLEAN AIRWAYS;1PC;;0930 ;ET;0225 ;N;979;AT;UA;  \r\nY-020;005VIE;VIENNA SCHWECHAT;DOK;DONETSK;VO;TYROLEAN AIRWAYS;;;;;F70;N\r\nK-FUSD229.00     ;UAH1830       ;;;;;;;;;;;UAH3568       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH752      YQ AC; UAH42       UA SE; UAH364      ZY AE; UAH98       AT SE; UAH121      QX AP; UAH43       IZ EB; UAH45       FR SE; UAH137      FR TI;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH1602     XT ;\r\nL-\r\nM-KBUYOS1U       ;KBUYOS1U       ;TFLYOS1U       ;TFLYOS1U       \r\nN-NUC;79.50;;149.50\r\nO-14DEC14DEC;14DEC14DEC;18DEC18DEC;18DEC18DEC\r\nQ-DOK OS X/VIE OS PAR79.50OS X/VIE OS DOK149.50NUC229.00END ROE1.000000XT752YQ42UA364ZY98AT121QX43IZ45FR137FR;FXP\r\nI-002;01RIASHENTSEVA/IULIIAM;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS OS  /PLS CNFMS SEAT BRDS\r\nOSI OS  DS/EXP/UA23969\r\nT-K257-3544667775\r\nFENONREF/NO CHNGS NONENDO;S2-5;P1\r\nFM*M*1A\r\nFPCASH\r\nFVOS;S2-5;P1\r\nTKOK28NOV/DOKC32530\r\nRIZTICKET FARE UAH 1830.00\r\nRIZTICKET TAX UAH 1738.00\r\nRIZTICKET TTL UAH 3568.00\r\nRIZSERVICE FEE UAH 250.00+VAT 50.00\r\nRIZGRAND TOTAL UAH 3868.00\r\nENDX\r\n	1	2011-12-01 15:21:43: Билет 257-3544667775 успешно импортирован\r\n	0	\N	\N	\N	\N
641185cef26643d68297acbb83f2ab91	Air	2	SYSTEM	2011-12-01 15:23:51	SYSTEM	2011-12-01 15:23:51	AIR_20111201132235.02457.PDT	2011-12-01 15:22:00	AIR-BLK207;7A;;247;0100037932;1A1336371;001001\r\nAMD 0101323896;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A X379WQ040;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;OS X379WQ\r\nA-AUSTRIAN;OS 2575\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111128;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-017;002ODOK;DONETSK          ;VIE;VIENNA SCHWECHAT ;OS    0640 K K 14DEC1445 1635 14DEC;OK01;HK01;M ;0;100;;TYROLEAN AIRWAYS;1PC;;1400 ;ET;0250 ;N;979;UA;AT;  \r\nY-017;002DOK;DONETSK;VIE;VIENNA SCHWECHAT;VO;TYROLEAN AIRWAYS;;;;;100;N\r\nH-018;003XVIE;VIENNA SCHWECHAT ;CDG;PARIS CDG        ;OS    0417 K K 14DEC1715 1930 14DEC;OK01;HK01;S ;0;320;;;1PC;;1630 ;ET;0215 ;N;643;AT;FR;2D\r\nH-021;004OCDG;PARIS CDG        ;VIE;VIENNA SCHWECHAT ;OS    7112 T T 18DEC0715 0915 18DEC;OK01;HK01;;0;320;;;1PC;2D;0630 ;ET;0200 ;N;643;FR;AT;2D\r\nX-021;004OCDG;PARIS CDG        ;VIE;VIENNA SCHWECHAT ;AF    1138 Y T 18DEC0715 0915 18DEC;OK01;HK01;;0;320;;;1PC;2D;0630 ;ET;0200 ;N;2D\r\nY-021;004CDG;PARIS CDG;VIE;VIENNA SCHWECHAT;AF;AIR FRANCE;;;;;320;N\r\nH-022;005XVIE;VIENNA SCHWECHAT ;DOK;DONETSK          ;OS    0639 T T 18DEC1015 1340 18DEC;OK01;HK01;M ;0;F70;;TYROLEAN AIRWAYS;1PC;;0930 ;ET;0225 ;N;979;AT;UA;  \r\nY-022;005VIE;VIENNA SCHWECHAT;DOK;DONETSK;VO;TYROLEAN AIRWAYS;;;;;F70;N\r\nK-FUSD229.00     ;UAH1830       ;;;;;;;;;;;UAH3557       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH748      YQ AC; UAH42       UA SE; UAH360      ZY AE; UAH96       AT SE; UAH121      QX AP; UAH43       IZ EB; UAH45       FR SE; UAH136      FR TI;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH1591     XT ;\r\nL-\r\nM-KBUYOS1U       ;KBUYOS1U       ;TFLYOS1U       ;TFLYOS1U       \r\nN-NUC;79.50;;149.50\r\nO-14DEC14DEC;14DEC14DEC;18DEC18DEC;18DEC18DEC\r\nQ-DOK OS X/VIE OS PAR79.50OS X/VIE OS DOK149.50NUC229.00END ROE1.000000XT748YQ42UA360ZY96AT121QX43IZ45FR136FR;FXB\r\nI-001;01RYASHENTSEVA/ALONAMR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nOSI OS  DS/EXP/UA23969\r\nT-K257-3544667776\r\nFENONREF/NO CHNGS NONENDO;S2-5;P1\r\nFM*M*1A\r\nFPCASH\r\nFVOS;S2-5;P1\r\nTKOK28NOV/DOKC32530\r\nRIZTICKET FARE UAH 1830.00\r\nRIZTICKET TAX UAH 1727.00\r\nRIZTICKET TTL UAH 3557.00\r\nRIZSERVICE FEE UAH 250.00+VAT 50.00\r\nRIZGRAND TOTAL UAH 3857.00\r\nENDX\r\n	1	2011-12-01 15:23:51: Билет 257-3544667776 успешно импортирован\r\n	0	\N	\N	\N	\N
d12ac4eb66964dba8afad861ab408faf	Air	2	SYSTEM	2011-12-01 16:29:14	SYSTEM	2011-12-01 16:29:15	AIR_20111201142812.03101.PDT	2011-12-01 16:28:00	AIR-BLK207;7A;;239;0100047527;1A1336371;001001\r\nAMD 0101324028;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 7CO2AA029;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 7CO2AA\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 0001AASU-2222MVSU-M-E--\r\nD-111123;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-011;002ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0305 Q Q 04DEC1805 2155 04DEC;OK01;HK01;L ;0;735;;;20K;;;ET;0150 ;N;545;UA;RU;C \r\nH-012;003OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 Q Q 14DEC1235 1210 14DEC;OK01;HK01;L ;0;735;;;20K;C ;;ET;0135 ;N;545;RU;UA;  \r\nK-FUSD207.00     ;UAH1654       ;;;;;;;;;;;UAH1832       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-Q2MRT    KK10  ;Q2MRT    KK10  \r\nN-NUC103.50;103.50\r\nO-XX04FEB;XX04FEB\r\nQ-DOK VV MOW103.50VV DOK103.50NUC207.00END ROE1.000000;FXP/ZO-10P*KK10\r\nI-001;01MURATOVA/MARGARYTA MRS;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667791\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S2-3;P1\r\nTKOK29NOV/DOKC32530\r\nRM PISHEM SEYCHAS///BRDS///MVZ4000////\r\nENDX\r\n	1	2011-12-01 16:29:15: Билет 870-3544667791 успешно импортирован\r\n	0	\N	\N	\N	\N
eb9aac1d3eed49a8a63b7d58face4033	Air	2	SYSTEM	2011-12-01 17:14:27	SYSTEM	2011-12-01 17:14:28	AIR_20111201151320.03448.PDT	2011-12-01 17:13:00	AIR-BLK207;7A;;247;0100052218;1A1336371;001001\r\nAMD 0101324088;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZC7BVB006;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R6P83 \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;IEVDOK;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 W W 01DEC1950 2100 01DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD196.00     ;UAH1566       ;;;;;;;;;;;UAH1995       ;7.9897     ;;   \r\nKFTF; UAH321      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH56       YR VA; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH321      HF ;UAH4        UD ;UAH104      XT ;\r\nL-\r\nM-WOWDOM   KK10  \r\nN-USD196.20\r\nO-XXXX\r\nQ-IEV PS DOK196.20USD196.20END XT16YQ56YR20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01CHEREDNICHENKO/YURIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667792\r\nFENON END/REF RESTR/RBK FOC;S2;P1\r\nFM*M*1\r\nFPCASH\r\nFTKK4704/11\r\nFVPS;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1566.00\r\nRIZTICKET TAX UAH 429.00\r\nRIZTICKET TTL UAH 1995.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2055.00\r\nENDX\r\n	1	2011-12-01 17:14:28: Билет 566-3544667792 успешно импортирован\r\n	0	\N	\N	\N	\N
7eaf2733f20c48d0a6dc63732506ca92	Air	2	SYSTEM	2011-12-01 15:33:57	SYSTEM	2011-12-01 15:33:58	AIR_20111201133245.02558.PDT	2011-12-01 15:32:00	AIR-BLK207;7A;;247;0100026829;1A1336371;001001\r\nAMD 0101323913;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A Y83OAS004;0303;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y83OAS\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/ITR-EML-OLESYA.KRIVORUCHKO@METINVESTHOLDING.COM\r\nC-7906/ 7777SVSU-0001AASU-M-E--\r\nD-111201;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-001;004ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0305 K K 04DEC1805 2155 04DEC;OK03;HK03;L ;0;735;;;20K;;;ET;0150 ;N;545;UA;RU;C \r\nH-002;005OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 K K 10DEC1235 1210 10DEC;OK03;HK03;L ;0;735;;;20K;C ;;ET;0135 ;N;545;RU;UA;  \r\nK-FUSD197.00     ;UAH1574       ;;;;;;;;;;;UAH1752       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-KPX3M5   KK6   ;KPX3M5   KK6   \r\nN-NUC98.70;98.70\r\nO-04DEC04DEC;10DEC10DEC\r\nQ-DOK VV MOW98.70VV DOK98.70NUC197.40END ROE1.000000;FXP/ZO-6P*KK6\r\nI-001;01ROGANOV/OLEKSANDR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667777\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S4-5;P1-3\r\nTKOK01DEC/DOKC32530\r\nI-002;02RYPALOV/VOLODYMYR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667778\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S4-5;P1-3\r\nTKOK01DEC/DOKC32530\r\nI-003;03SALIMOV/RENAT MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667779\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S4-5;P1-3\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1574.00\r\nRIZTICKET TAX UAH 178.00\r\nRIZTICKET TTL UAH 1752.00\r\nRIZSERVICE FEE UAH 80.00+VAT 16.00\r\nRIZGRAND TOTAL UAH 1848.00\r\nENDX\r\n	1	2011-12-01 15:33:58: Билет 870-3544667777 успешно импортирован\r\nБилет 870-3544667778 успешно импортирован\r\nБилет 870-3544667779 успешно импортирован\r\n	0	\N	\N	\N	\N
4f7cc507fe97493ead18f0d42e0a06fb	Air	2	SYSTEM	2011-12-01 15:35:01	SYSTEM	2011-12-01 15:35:02	AIR_20111201133402.02573.PDT	2011-12-01 15:34:00	AIR-BLK207;7A;;247;0100044535;1A1336371;001001\r\nAMD 0101323918;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A Y96T5C004;0505;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y96T5C\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/ITR-EML-OLESYA.KRIVORUCHKO@METINVESTHOLDING.COM\r\nC-7906/ 0001AASU-0001AASU-M-E--\r\nD-111201;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-001;006ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0305 K K 04DEC1805 2155 04DEC;OK05;HK05;L ;0;735;;;20K;;;ET;0150 ;N;545;UA;RU;C \r\nH-002;007OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 K K 10DEC1235 1210 10DEC;OK05;HK05;L ;0;735;;;20K;C ;;ET;0135 ;N;545;RU;UA;  \r\nK-FUSD197.00     ;UAH1574       ;;;;;;;;;;;UAH1752       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-KPX3M5   KK6   ;KPX3M5   KK6   \r\nN-NUC98.70;98.70\r\nO-04DEC04DEC;10DEC10DEC\r\nQ-DOK VV MOW98.70VV DOK98.70NUC197.40END ROE1.000000;FXP/ZO-6P*KK6\r\nI-001;01BILOUS/VLADYSLAV MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667780\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S6-7;P1-5\r\nTKOK01DEC/DOKC32530\r\nI-002;02KAPLYA/VADIM MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667781\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S6-7;P1-5\r\nTKOK01DEC/DOKC32530\r\nI-003;03LEVCHENKO/OLEKSANDR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667782\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S6-7;P1-5\r\nTKOK01DEC/DOKC32530\r\nI-004;04MAKAROV/KYRYLO MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667783\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S6-7;P1-5\r\nTKOK01DEC/DOKC32530\r\nI-005;05ORLOV/SERGIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667784\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S6-7;P1-5\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1574.00\r\nRIZTICKET TAX UAH 178.00\r\nRIZTICKET TTL UAH 1752.00\r\nRIZSERVICE FEE UAH 80.00+VAT 16.00\r\nRIZGRAND TOTAL UAH 1848.00\r\nENDX\r\n	1	2011-12-01 15:35:02: Билет 870-3544667780 успешно импортирован\r\nБилет 870-3544667781 успешно импортирован\r\nБилет 870-3544667782 успешно импортирован\r\nБилет 870-3544667783 успешно импортирован\r\nБилет 870-3544667784 успешно импортирован\r\n	0	\N	\N	\N	\N
ba3ab877e45f4dd5bba3ae17e0092dfc	Air	2	SYSTEM	2011-12-01 16:12:19	SYSTEM	2011-12-01 16:12:19	AIR_20111201141122.02936.PDT	2011-12-01 16:11:00	AIR-BLK207;7A;;247;0100041026;1A1336371;001001\r\nAMD 0101323989;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y7CZJR008;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y7CZJR\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;DOKIEV;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 C C 01DEC1950 2100 01DEC;OK01;HK01;S ;0;735;;;30K;;;ET;0110 ;N;347;UA;UA;B \r\nK-FUSD288.00     ;UAH2301       ;;;;;;;;;;;UAH2883       ;7.9897     ;;   \r\nKFTF; UAH473      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH473      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-COW5     KK20  \r\nN-USD288.00\r\nO-XXXX\r\nQ-DOK VV IEV288.00USD288.00END XT48YQ36YK21UA;FXP/ZO-20P*KK20\r\nI-001;01TIMCHENKO/MAKSYM MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870000666833;S2;P1\r\nT-K870-3544667785\r\nFE.-44877533.-/VVONLY/NON ENDO KK20 AGREEMENT 01-851 /VAT UAH 480.50\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 2301.00\r\nRIZTICKET TAX UAH 582.00\r\nRIZTICKET TTL UAH 2883.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2943.00\r\nENDX\r\n	1	2011-12-01 16:12:19: Билет 870-3544667785 успешно импортирован\r\n	0	\N	\N	\N	\N
81ef8a3515f84edfac5eababbef63e58	Air	2	SYSTEM	2011-12-01 16:17:13	SYSTEM	2011-12-01 16:17:13	AIR_20111201141606.02989.PDT	2011-12-01 16:16:00	AIR-BLK207;7A;;247;0100046261;1A1336371;001001\r\nAMD 0101323996;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZBWZN7007;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZBWZN7\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;DOKIEV;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 C C 01DEC1950 2100 01DEC;OK01;HK01;S ;0;735;;;30K;;;ET;0110 ;N;347;UA;UA;B \r\nK-FUSD288.00     ;UAH2301       ;;;;;;;;;;;UAH2883       ;7.9897     ;;   \r\nKFTF; UAH473      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH473      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-COW5     KK20  \r\nN-USD288.00\r\nO-XXXX\r\nQ-DOK VV IEV288.00USD288.00END XT48YQ36YK21UA;FXP/ZO-20P*KK20\r\nI-001;01RYZHENKOV/YURIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870000434773;S2;P1\r\nT-K870-3544667786\r\nFE.-44877533.-/VVONLY/NON ENDO KK20 AGREEMENT 01-851 /VAT UAH 480.50\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 2301.00\r\nRIZTICKET TAX UAH 582.00\r\nRIZTICKET TTL UAH 2883.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2943.00\r\nENDX\r\n	1	2011-12-01 16:17:13: Билет 870-3544667786 успешно импортирован\r\n	0	\N	\N	\N	\N
35c55169d49f4b9da9a94a231ce77263	Air	2	SYSTEM	2011-12-01 17:21:31	SYSTEM	2011-12-01 17:21:31	AIR_20111201152033.03492.PDT	2011-12-01 17:20:00	AIR-BLK207;7A;;247;0100049281;1A1336371;001001\r\nAMD 0101324093;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZC7BVB018;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZC7BVB\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111201;111201\r\nG-   ;;IEVDOK;UA\r\nH-002;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 L L 01DEC2130 2240 01DEC;OK01;HK01;N ;0;321;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD113.00     ;UAH903        ;;;;;;;;;;;UAH1174       ;7.9897     ;;   \r\nKFTF; UAH187      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH187      HF ;UAH4        UD ;UAH80       XT ;\r\nL-\r\nM-LOW1M5   KK10  \r\nN-USD112.50\r\nO-01DEC01DEC\r\nQ-IEV VV DOK112.50USD112.50END XT48YQ20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01CHEREDNICHENKO/YURIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870000616683;S2;P1\r\nT-K870-3544667793\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 195.67\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 903.00\r\nRIZTICKET TAX UAH 271.00\r\nRIZTICKET TTL UAH 1174.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1234.00\r\nENDX\r\n	1	2011-12-01 17:21:31: Билет 870-3544667793 успешно импортирован\r\n	0	\N	\N	\N	\N
c6ca57e046024cc69f3e52d1d96a11c9	Air	2	SYSTEM	2011-12-01 17:22:35	SYSTEM	2011-12-01 17:22:35	AIR_20111201152121.03497.PDT	2011-12-01 17:21:00	AIR-BLK207;7A;;247;0100049370;1A1336371;001001\r\nAMD 0101324094;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A YIWX5E016;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YIWX5E\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/P1/S2\r\nC-7906/ 7777SVSU-7777SVSU-M-V--\r\nD-111129;111201;111201\r\nG-   ;;IEVDOK;UA\r\nH-003;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0095 T T 03DEC1345 1500 03DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nK-RUSD145.00     ;UAH           ;;;;;;;;;;;UAH192        ;;;   \r\nKFTR;OUAH48       YQ AC;OUAH20       YK AE;OUAH4        UD DP;OUAH239      HF GO;OUAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-PD 48       YQ ;PD 20       YK ;PD 255      XT ;\r\nL-\r\nM-TOW2M5         \r\nN-USD145.00\r\nO-03DEC03DEC\r\nQ-IEV VV DOK145.00USD145.00END PD XT4UD239HF12UA;FXP/R,30NOV11\r\nI-001;01PIROG/NINA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667794\r\nFH870-3544644732;P1\r\nFENONENDO/REBOOK USD20;S2;P1\r\nFM*M*1A\r\nFO870-3544644732DOK30NOV11/72320942/870-35446447323E1\r\nFPO/CASH+/CASH\r\nFVVV;S2;P1\r\nTKOK30NOV/DOKC32530//ETVV\r\nENDX\r\n	1	2011-12-01 17:22:35: Билет 870-3544667794 успешно импортирован\r\n	0	\N	\N	\N	\N
a61627a3df144f8c8f340ac1dec17bdd	Air	2	SYSTEM	2011-12-01 17:56:22	SYSTEM	2011-12-01 17:56:23	AIR_20111201155519.03756.PDT	2011-12-01 17:55:00	AIR-BLK207;7A;;247;0100053393;1A1336371;001001\r\nAMD 0101324151;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A ZC5WFB003;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;BA NOSYNC\r\nA-BRITISH AIRWAYS;BA 1256\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111201;111201;111201\r\nG-X  ;;LONLON;E1\r\nH-001;002OLHR;LONDON LHR       ;KBP;KIEV BORYSPIL    ;BA    0882 N N 16DEC0800 1315 16DEC;OK01;HK01;S ;0;320;;;1PC;5 ;;ET;0315 ;N;1370;GB;UA;F \r\nH-002;003OKBP;KIEV BORYSPIL    ;LHR;LONDON LHR       ;BA    0883 Q Q 18DEC1410 1550 18DEC;OK01;HK01;S ;0;320;;;1PC;F ;;ET;0340 ;N;1370;UA;GB;5 \r\nK-FGBP264.00     ;UAH3268       ;;;;;;;;;;;UAH4341       ;1.550868   ;7.9897     ;USD\r\nKFTF; UAH136      YK AE; UAH16       UD DP; UAH298      YQ AC; UAH62       YQ AD; UAH32       UA SE; UAH149      GB AD; UAH380      UB AS;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH136      YK ;UAH16       UD ;UAH921      XT ;\r\nL-\r\nM-NNCLON2        ;QNCLON2        \r\nN-NUC238.14;185.22\r\nO-XXXX;XXXX\r\nQ-LON BA IEV238.14BA LON185.22NUC423.36END ROE0.623580XT298YQ62YQ32UA149GB380UB;FXB\r\nI-001;01DOROFTEY/PAVLO MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K125-3544667795\r\nFENONREF/-NCLON2;S2-3;P1\r\nFM*M*1\r\nFPCASH\r\nFVBA;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 3268.00\r\nRIZTICKET TAX UAH 1073.00\r\nRIZTICKET TTL UAH 4341.00\r\nRIZSERVICE FEE UAH 250.00+VAT 50.00\r\nRIZGRAND TOTAL UAH 4641.00\r\nENDX\r\n	1	2011-12-01 17:56:23: Билет 125-3544667795 успешно импортирован\r\n	0	\N	\N	\N	\N
72c6a6fda9154c14a20eb31138e59474	Air	2	SYSTEM	2011-12-01 18:01:00	SYSTEM	2011-12-01 18:01:00	AIR_20111201155947.03793.PDT	2011-12-01 17:59:00	AIR-BLK207;MA;;233;0100053776;1A1336371;001001\r\nAMD 0101324155;1/1;VOID01DEC;SVSU\r\nGW3718884;1A1336371\r\nMUC1A ZC7BVB018;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZC7BVB\r\nI-001;01CHEREDNICHENKO/YURIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667793\r\nFPCASH\r\nENDX\r\n	1	2011-12-01 18:01:00: Void 870-3544667793 успешно импортирован\r\n	0	\N	\N	\N	\N
c38b328c6cb8464792b6796bf732e57c	Air	2	SYSTEM	2011-12-01 18:34:41	SYSTEM	2011-12-01 18:34:41	AIR_20111201163337.03940.PDT	2011-12-01 18:33:00	AIR-BLK207;7A;;247;0100059414;1A1336371;001001\r\nAMD 0101324188;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A YWXSZK021;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YWXSZK\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/S3\r\nC-7906/ 2222MVSU-7777SVSU-M-V--\r\nD-111130;111201;111201\r\nG-   ;;DOKIEV;UA\r\nH-004;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 Y Y 01DEC1950 2100 01DEC;OK01;HK01;N ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nU-001X;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0093 B B 01DEC0700 0820 01DEC;OK01;HK01;N ;0;734;;;;B ;;ET;0120 ;N;;347;;UA;UA;  \r\nK-RUSD390.00     ;UAH           ;;;;;;;;;;;UAH1188       ;;;   \r\nKFTR;OUAH96       YQ AC;OUAH56       YK AE;OUAH8        UD DP;OUAH643      HF GO;OUAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-PD 96       YQ ;PD 56       YK ;PD 684      XT ;\r\nL-\r\nM-YRT5           \r\nN-USD160.00;230.00\r\nO-XX01MAR\r\nQ-IEV VV DOK160.00VV IEV230.00USD390.00END PD XT8UD643HF33UA;FXP/R,30NOV11\r\nI-001;01KOSTYUK/DMYTRO MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667796\r\nFENONENDO;S3;P1\r\nFM*M*1A\r\nFO870-3544644737DOK30NOV11/72320942/870-35446447374E2\r\nFPO/CASH+/CASH\r\nFTITAGVV01-851\r\nFVVV;S3;P1\r\nTKOK30NOV/DOKC32530//ETVV\r\nRIZTICKET FARE UAH 2126.00\r\nRIZTICKET TAX UAH 638.00\r\nRIZTICKET TTL UAH 2764.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2824.00\r\nENDX\r\n	1	2011-12-01 18:34:41: Билет 870-3544667796 успешно импортирован\r\n	0	\N	\N	\N	\N
074b7ca300ca484382a699938f68929e	Air	2	SYSTEM	2011-12-02 08:49:58	SYSTEM	2011-12-02 08:50:00	AIR_20111201193033.04126.PDT	2011-12-01 21:30:00	AIR-BLK207;7A;;247;0100067502;1A1336371;001001\r\nAMD 0101324237;1/2;              \r\nGW3671287;1A1336371;IEV1A098A;AIR\r\nMUC1A 8SHYP8011;0301;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LH 8SHYP8\r\nA-LUFTHANSA;LH 2203\r\nB-TTP\r\nC-7906/ 0001AASU-7777SVSU-I-0--\r\nD-111128;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-001;004ODOK;DONETSK          ;MUC;MUNICH           ;LH    2543 S S 05JAN1540 1750 05JAN;OK01;HK01;M ;0;CR9;;LUFTHANSA CITYLINE;1PC;;1455 ;ET;0310 ; ;1193;UA;DE;2 \r\nY-001;004DOK;DONETSK;MUC;MUNICH;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nH-002;005OMUC;MUNICH           ;DOK;DONETSK          ;LH    2542 W W 15JAN1100 1500 15JAN;OK01;HK01;S ;0;CR9;;LUFTHANSA CITYLINE;1PC;2 ;1020 ;ET;0300 ; ;1193;DE;UA;  \r\nY-002;005MUC;MUNICH;DOK;DONETSK;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nK-FUSD332.00     ;UAH2653       ;;;;;;;;;;;UAH3952       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH772      YQ AC; UAH42       UA SE; UAH86       OY CB; UAH207      RA EB; UAH56       DE SE;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH1163     XT ;\r\nL-\r\nM-SNC2W2   CH25  ;WNC2W2   CH25  \r\nN-NUC153.00;178.50\r\nO-05JAN05JAN;15JAN15JAN\r\nQ-DOK LH MUC153.00LH DOK178.50NUC331.50END ROE1.000000XT772YQ42UA86OY207RA56DE;FXP\r\nI-003;03GORLOVA/YELYZAVETA MIST (CHD);;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR CHLD LH  HK1/14APR04;P3\r\nSSR OTHS 1A  /PLS ISS AUTOMATIC TKT BY 01DEC11/2359Z OR ALL LH SEGS WILL BE XLD. APPLICABLE FARE RULE APPLIES IF IT\r\nSSR OTHS 1A  ////DEMANDS EARLIER TKTG.XE/I FF\r\nOSI LH  DS/EXP/UA23969\r\nT-K220-3544667797\r\nFENONREF/FL/CHG RESTRICTEDCHECK FARE NOTE;S4-5;P3\r\nFM*M*1A\r\nFPCASH\r\nFVLH;S4-5;P3\r\nTKOK28NOV/DOKC32530\r\nOPLDOKC32530/01DEC/3C0\r\nEND\r\n	1	2011-12-02 08:50:00: Билет 220-3544667797 успешно импортирован\r\n	0	\N	\N	\N	\N
3cd9253b55424dea8815766102076c1c	Air	2	SYSTEM	2011-12-02 08:50:02	SYSTEM	2011-12-02 08:50:02	AIR_20111201193205.04128.PDT	2011-12-01 21:32:00	AIR-BLK207;7A;;247;0100095955;1A1336371;001001\r\nAMD 0101324239;1/1;              \r\nGW3671287;1A1336371;IEV1A098A;AIR\r\nMUC1A Y8HCDN009;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LH Y8HCDN\r\nA-LUFTHANSA;LH 2203\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111201;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-003;002ODOK;DONETSK          ;MUC;MUNICH           ;LH    2543 W W 05JAN1540 1750 05JAN;OK01;HK01;M ;0;CR9;;LUFTHANSA CITYLINE;1PC;;1455 ;ET;0310 ; ;1193;UA;DE;2 \r\nY-003;002DOK;DONETSK;MUC;MUNICH;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nH-004;003OMUC;MUNICH           ;DOK;DONETSK          ;LH    2542 W W 15JAN1100 1500 15JAN;OK01;HK01;S ;0;CR9;;LUFTHANSA CITYLINE;1PC;2 ;1020 ;ET;0300 ; ;1193;DE;UA;  \r\nY-004;003MUC;MUNICH;DOK;DONETSK;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nK-FUSD357.00     ;UAH2853       ;;;;;;;;;;;UAH4152       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH772      YQ AC; UAH42       UA SE; UAH86       OY CB; UAH207      RA EB; UAH56       DE SE;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH1163     XT ;\r\nL-\r\nM-WNC2W2   CH25  ;WNC2W2   CH25  \r\nN-NUC178.50;178.50\r\nO-05JAN05JAN;15JAN15JAN\r\nQ-DOK LH MUC178.50LH DOK178.50NUC357.00END ROE1.000000XT772YQ42UA86OY207RA56DE;FXP\r\nI-001;01GORLOV/DANIYIL MSTR (CHD);;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR CHLD LH  HK1/12AUG08;P1\r\nOSI LH  DS/EXP/UA23969\r\nT-K220-3544667800\r\nFENONREF/FL/CHG RESTRICTEDCHECK FARE NOTE;S2-3;P1\r\nFM*M*1A\r\nFPCASH\r\nFVLH;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nENDX\r\n	1	2011-12-02 08:50:02: Билет 220-3544667800 успешно импортирован\r\n	0	\N	\N	\N	\N
50e71322cbf4493ebb7491094af6cbad	Air	2	SYSTEM	2011-12-02 08:50:03	SYSTEM	2011-12-02 08:50:04	AIR_20111201193254.04129.PDT	2011-12-01 21:32:00	AIR-BLK207;7A;;247;0100067581;1A1336371;001001\r\nAMD 0101324240;1/1;              \r\nGW3671287;1A1336371;IEV1A098A;AIR\r\nMUC1A YSKMVB013;0202;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LH YSKMVB\r\nA-LUFTHANSA;LH 2203\r\nB-TTP\r\nC-7906/ 0001AASU-7777SVSU-I-0--\r\nD-111130;111201;111201\r\nG-X  ;;DOKDOK;E1\r\nH-007;003ODOK;DONETSK          ;MUC;MUNICH           ;LH    2543 W W 05JAN1540 1750 05JAN;OK02;HK02;M ;0;CR9;;LUFTHANSA CITYLINE;1PC;;1455 ;ET;0310 ; ;1193;UA;DE;2 \r\nY-007;003DOK;DONETSK;MUC;MUNICH;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nH-008;004OMUC;MUNICH           ;DOK;DONETSK          ;LH    2542 W W 15JAN1100 1500 15JAN;OK02;HK02;S ;0;CR9;;LUFTHANSA CITYLINE;1PC;2 ;1020 ;ET;0300 ; ;1193;DE;UA;  \r\nY-008;004MUC;MUNICH;DOK;DONETSK;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nK-FUSD476.00     ;UAH3803       ;;;;;;;;;;;UAH5102       ;7.9897     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH772      YQ AC; UAH42       UA SE; UAH86       OY CB; UAH207      RA EB; UAH56       DE SE;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH1163     XT ;\r\nL-\r\nM-WNC2W2         ;WNC2W2         \r\nN-NUC238.00;238.00\r\nO-05JAN05JAN;15JAN15JAN\r\nQ-DOK LH MUC238.00LH DOK238.00NUC476.00END ROE1.000000XT772YQ42UA86OY207RA56DE;FXP\r\nI-001;01GORLOV/VALERIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /PLS ISS AUTOMATIC TKT BY 03DEC11/2359Z OR ALL LH SEGS WILL BE XLD. APPLICABLE FARE RULE APPLIES IF IT\r\nSSR OTHS 1A  ////DEMANDS EARLIER TKTG.XE/I FF\r\nOSI LH  DS/EXP/UA23969\r\nT-K220-3544667801\r\nFENONREF/FL/CHG RESTRICTEDCHECK FARE NOTE;S3-4;P1-2\r\nFM*M*1A\r\nFPCASH\r\nFVLH;S3-4;P1-2\r\nTKOK30NOV/DOKC32530\r\nI-002;02GORLOVA/TETYANA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /PLS ISS AUTOMATIC TKT BY 03DEC11/2359Z OR ALL LH SEGS WILL BE XLD. APPLICABLE FARE RULE APPLIES IF IT\r\nSSR OTHS 1A  ////DEMANDS EARLIER TKTG.XE/I FF\r\nOSI LH  DS/EXP/UA23969\r\nT-K220-3544667802\r\nFENONREF/FL/CHG RESTRICTEDCHECK FARE NOTE;S3-4;P1-2\r\nFM*M*1A\r\nFPCASH\r\nFVLH;S3-4;P1-2\r\nTKOK30NOV/DOKC32530\r\nENDX\r\n	1	2011-12-02 08:50:04: Билет 220-3544667801 успешно импортирован\r\nБилет 220-3544667802 успешно импортирован\r\n	0	\N	\N	\N	\N
3f28eb612439480299188e00bb09f2c8	Air	2	SYSTEM	2011-12-02 08:55:39	SYSTEM	2011-12-02 08:55:39	AIR_20111202065439.04632.PDT	2011-12-02 08:54:00	AIR-BLK207;7A;;247;0200008081;1A1336371;001001\r\nAMD 0201324276;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A YE3UPF021;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YE3UPF\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111129;111202;111202\r\nG-   ;;DOKDOK;UA\r\nH-003;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0092 C C 02DEC0920 1045 02DEC;OK01;HK01;S ;0;734;;;30K;;;ET;0125 ;N;347;UA;UA;B \r\nH-004;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0091 D D 03DEC0910 1025 03DEC;OK01;HK01;S ;0;735;;;30K;B ;;ET;0115 ;N;347;UA;UA;  \r\nK-FUSD531.00     ;UAH4243       ;;;;;;;;;;;UAH5304       ;7.9899     ;;   \r\nKFTF; UAH868      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH868      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-CRT5     KK10  ;DEE6M5   KK10  \r\nN-USD301.50;229.50\r\nO-02DEC02DEC;03DEC03DEC\r\nQ-DOK VV IEV301.50VV DOK229.50USD531.00END XT96YQ56YK33UA;FXP/ZO-10P*KK10\r\nI-001;01DETUYK/SERGIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870000616303;S2;P1\r\nFQV VV  FQTV-VV870000616303;S3;P1\r\nT-K870-3544667803\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 884.00\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK30NOV/IEVVV0009\r\nRIZTICKET FARE UAH 4243.00\r\nRIZTICKET TAX UAH 1061.00\r\nRIZTICKET TTL UAH 5304.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 5364.00\r\nENDX\r\n	1	2011-12-02 08:55:39: Билет 870-3544667803 успешно импортирован\r\n	0	\N	\N	\N	\N
8fa693fb739a4702a893709ccafd876c	Air	2	SYSTEM	2011-12-02 09:46:57	SYSTEM	2011-12-02 09:46:58	AIR_20111202074559.04929.PDT	2011-12-02 09:45:00	AIR-BLK207;7A;;239;0200009378;1A1336371;001001\r\nAMD 0201324302;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZJ73PW011;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZJ73PW\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 0001AASU-2222MVSU-M-E--\r\nD-111202;111202;111202\r\nG-   ;;DOKDOK;UA\r\nH-005;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 Q Q 02DEC1700 1820 02DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nH-004;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0093 Q Q 05DEC0700 0820 05DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0120 ;N;347;UA;UA;  \r\nK-FUSD171.00     ;UAH1367       ;;;;;;;;;;;UAH1853       ;7.9899     ;;   \r\nKFTF; UAH293      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH293      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-QPX1M5   KK10  ;QPX1M5   KK10  \r\nN-USD85.50;85.50\r\nO-02DEC02DEC;05DEC05DEC\r\nQ-DOK VV IEV85.50VV DOK85.50USD171.00END XT96YQ56YK33UA;FXP/ZO-10P*KK10\r\nI-001;01IGNATOVSKYI/SERGIY MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667811\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-801 /VAT UAH 308.83\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S2-3;P1\r\nTKOK02DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS////OPLATA NALICHNYMI//\r\nENDX\r\n	1	2011-12-02 09:46:58: Билет 870-3544667811 успешно импортирован\r\n	0	\N	\N	\N	\N
1af4854f79db40a9b8393554cb536d48	Air	2	SYSTEM	2011-12-02 09:47:20	SYSTEM	2011-12-02 09:47:20	AIR_20111202074610.04931.PDT	2011-12-02 09:46:00	AIR-BLK207;7A;;247;0200010728;1A1336371;001001\r\nAMD 0201324304;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A YR2UUR046;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;OS YR2UUR\r\nA-AUSTRIAN;OS 2575\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111130;111202;111202\r\nG-X  ;;ZAGZAG;E1\r\nH-025;002OZAG;ZAGREB           ;VIE;VIENNA SCHWECHAT ;OS    0682 G G 05DEC0825 0925 05DEC;OK01;HK01;R ;0;F70;;TYROLEAN AIRWAYS;1PC;;;ET;0100 ;N;167;HR;AT;  \r\nY-025;002ZAG;ZAGREB;VIE;VIENNA SCHWECHAT;VO;TYROLEAN AIRWAYS;;;;;F70;N\r\nH-026;003XVIE;VIENNA SCHWECHAT ;DOK;DONETSK          ;OS    0639 G G 05DEC1015 1340 05DEC;OK01;HK01;M ;0;100;;TYROLEAN AIRWAYS;1PC;;0930 ;ET;0225 ;N;979;AT;UA;  \r\nY-026;003VIE;VIENNA SCHWECHAT;DOK;DONETSK;VO;TYROLEAN AIRWAYS;;;;;100;N\r\nH-027;004ODOK;DONETSK          ;VIE;VIENNA SCHWECHAT ;OS    0640 W W 11DEC1445 1635 11DEC;OK01;HK01;M ;0;F70;;TYROLEAN AIRWAYS;1PC;;1400 ;ET;0250 ;N;979;UA;AT;  \r\nY-027;004DOK;DONETSK;VIE;VIENNA SCHWECHAT;VO;TYROLEAN AIRWAYS;;;;;F70;N\r\nH-028;005XVIE;VIENNA SCHWECHAT ;ZAG;ZAGREB           ;OS    0677 W W 11DEC1710 1805 11DEC;OK01;HK01;R ;0;F70;;TYROLEAN AIRWAYS;1PC;;1625 ;ET;0055 ;N;167;AT;HR;  \r\nY-028;005VIE;VIENNA SCHWECHAT;ZAG;ZAGREB;VO;TYROLEAN AIRWAYS;;;;;F70;N\r\nK-FEUR419.00     ;UAH4492       ;;;;;;;;;;;UAH6847       ;10.72057   ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH1544     YQ AC; UAH42       UA SE; UAH156      HR AE; UAH15       MI CA; UAH364      ZY AE; UAH98       AT SE;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH2219     XT ;\r\nL-\r\nM-GFLYOS1C       ;GFLYOS1C       ;WFLYOS1C       ;WFLYOS1C       \r\nN-NUC;356.94;;230.71\r\nO-05DEC05DEC;05DEC05DEC;11DEC11DEC;11DEC11DEC\r\nQ-ZAG OS X/VIE OS DOK356.94OS X/VIE OS ZAG230.71NUC587.65END ROE0.712992XT1544YQ42UA156HR15MI364ZY98AT;FXB\r\nI-001;01BILIC/DALIBOR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /PLS ADV TKT NBRS OF OS SEG LATEST 02DEC11 1453GMT OR SEG WILL BE CANX\r\nOSI OS  DS/EXP/UA23969\r\nT-K257-3544667812\r\nFENONREF/CHNGS RESTRNONENDO;S2-5;P1\r\nFM*M*1A\r\nFPCASH\r\nFVOS;S2-5;P1\r\nTKOK30NOV/DOKC32530\r\nOPLDOKC32530/02DEC/3C0\r\nRIZTICKET FARE UAH 4492.00\r\nRIZTICKET TAX UAH 2355.00\r\nRIZTICKET TTL UAH 6847.00\r\nRIZSERVICE FEE UAH 250.00+VAT 50.00\r\nRIZGRAND TOTAL UAH 7147.00\r\nENDX\r\n	1	2011-12-02 09:47:20: Билет 257-3544667812 успешно импортирован\r\n	0	\N	\N	\N	\N
bf93c28efa204909aab153b8efaae8b9	Air	2	SYSTEM	2011-12-02 10:44:11	SYSTEM	2011-12-02 10:44:11	AIR_20111202084255.05512.PDT	2011-12-02 10:42:00	AIR-BLK207;7A;;239;0200013392;1A1336371;001001\r\nAMD 0201324399;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZK6SNB008;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZK6SNB\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 0001AASU-2222MVSU-M-E--\r\nD-111202;111202;111202\r\nG-   ;;DOKIEV;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 C C 02DEC1700 1820 02DEC;OK01;HK01;S ;0;320;;;30K;;;ET;0120 ;N;347;UA;UA;B \r\nK-FUSD306.00     ;UAH2445       ;;;;;;;;;;;UAH3056       ;7.9899     ;;   \r\nKFTF; UAH502      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH502      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-COW5     KK15  \r\nN-USD306.00\r\nO-XXXX\r\nQ-DOK VV IEV306.00USD306.00END XT48YQ36YK21UA;FXP/ZO-15P*KK15\r\nI-001;01SYRY/IGOR MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nFQV VV  FQTV-VV870052040533;S2;P1\r\nT-K870-3544667815\r\nFE.-44877533.-/VVONLY/NON ENDO KK15 AGREEMENT 01-801 /VAT UAH 509.33\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S2;P1\r\nTKOK02DEC/DOKC32530\r\nRM PISHEM SEYCHAS///BRDS///SCHET NA TSULINY//////!!!!!!!!!!!!!!!!!!\r\nENDX\r\n	1	2011-12-02 10:44:11: Билет 870-3544667815 успешно импортирован\r\n	0	\N	\N	\N	\N
55fd702c50d9468a8a05a37b2bc4d0e0	Air	2	SYSTEM	2011-12-02 10:24:59	SYSTEM	2011-12-02 10:24:59	AIR_20111202082353.05307.PDT	2011-12-02 10:23:00	AIR-BLK207;7A;;247;0200012498;1A1336371;001001\r\nAMD 0201324359;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A YEIC9M016;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;TK SMILNG\r\nA-TURKISH AIRLINES;TK 2354\r\nB-TTP\r\nC-7906/ 0001AASU-7777SVSU-I-0--\r\nD-111129;111202;111202\r\nG-X  ;;DOKDOK;\r\nH-007;002ODOK;DONETSK          ;IST;ISTANBUL         ;TK    0454 L L 09DEC1200 1405 09DEC;OK01;HK01;M ;0;73W;;;20K;;;ET;0205 ;N;660;UA;TR;I \r\nH-002;003XIST;ISTANBUL         ;PVG;SHANGHAI PUDONG  ;TK    0026 L L 09DEC2340 1605 10DEC;OK01;HK01;M ;0;77W;;;20K;I ;;ET;1025 ;N;5000;TR;CN;2 \r\nH-008;004OPVG;SHANGHAI PUDONG  ;IST;ISTANBUL         ;TK    0027 L L 19DEC2305 0520 20DEC;OK01;HK01;M ;0;77W;;;20K;2 ;;ET;1215 ;N;5000;CN;TR;I \r\nH-009;005XIST;ISTANBUL         ;DOK;DONETSK          ;TK    0453 L L 20DEC0900 1105 20DEC;OK01;HK01;M ;0;738;;;20K;I ;;ET;0205 ;N;660;TR;UA;  \r\nK-FUSD568.00     ;UAH4539       ;;;;;;;;;;;UAH7614       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH2676     YR VA; UAH42       UA SE; UAH108      TR AE; UAH113      CN AE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH2939     XT ;\r\nL-\r\nM-LDCPB1M        ;LDCPB1M        ;LDCPB1M        ;LDCPB1M        \r\nN-NUC;284.00;;284.00\r\nO-09DEC09DEC;09DEC09DEC;19DEC19DEC;20DEC20DEC\r\nQ-DOK TK X/IST TK SHA284.00TK X/IST TK DOK284.00NUC568.00END ROE1.000000XT2676YR42UA108TR113CN;FXP\r\nI-001;01BILASH/ARTEM MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K235-3544667813\r\nFENONEND/TK ONLY;S2-5;P1\r\nFM*M*1\r\nFPCASH\r\nFVTK;S2-5;P1\r\nTKOK29NOV/DOKC32530\r\nRIZTICKET FARE UAH 4539.00\r\nRIZTICKET TAX UAH 3075.00\r\nRIZTICKET TTL UAH 7614.00\r\nRIZSERVICE FEE UAH 250.00+VAT 50.00\r\nRIZGRAND TOTAL UAH 7914.00\r\nENDX\r\n	1	2011-12-02 10:24:59: Билет 235-3544667813 успешно импортирован\r\n	0	\N	\N	\N	\N
94296c3d308e42cfbda3134197c2c32f	Air	2	SYSTEM	2011-12-02 10:42:05	SYSTEM	2011-12-02 10:42:05	AIR_20111202084059.05486.PDT	2011-12-02 10:40:00	AIR-BLK207;7A;;247;0200028955;1A1336371;001001\r\nAMD 0201324393;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A ZKU5DH004;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZKU5DH\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111202;111202;111202\r\nG-   ;;DOKDOK;UA\r\nH-019;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0090 K K 06DEC0700 0820 06DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nH-020;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 K K 08DEC2130 2240 08DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD150.00     ;UAH1199       ;;;;;;;;;;;UAH1653       ;7.9899     ;;   \r\nKFTF; UAH261      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH261      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-KSS1M          ;KSS1M          \r\nN-USD75.00;75.00\r\nO-06DEC06DEC;08DEC08DEC\r\nQ-DOK VV IEV75.00VV DOK75.00USD150.00END XT96YQ56YK33UA;FXB\r\nI-001;01ARBUZOVA/SVITLANA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667814\r\nFE.-44877533.-/VVONLY/NON\r\nFM*M*1A\r\nFPCASH\r\nFVVV;S2-3;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 1199.00\r\nRIZTICKET TAX UAH 454.00\r\nRIZTICKET TTL UAH 1653.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1713.00\r\nENDX\r\n	1	2011-12-02 10:42:05: Билет 870-3544667814 успешно импортирован\r\n	0	\N	\N	\N	\N
f7a10cb83639426e83890aa40dca56a5	Air	2	SYSTEM	2011-12-02 10:49:07	SYSTEM	2011-12-02 10:49:07	AIR_20111202084803.05580.PDT	2011-12-02 10:48:00	AIR-BLK207;7A;;247;0200014462;1A1336371;001001\r\nAMD 0201324406;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A YZAYMG028;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R6QN7 \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 0001AASU-2222MVSU-M-E--\r\nD-111130;111202;111202\r\nG-   ;;IEVDOK;UA\r\nH-003;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 S S 02DEC1950 2100 02DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD213.00     ;UAH1702       ;;;;;;;;;;;UAH2159       ;7.9899     ;;   \r\nKFTF; UAH349      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH56       YR VA; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH349      HF ;UAH4        UD ;UAH104      XT ;\r\nL-\r\nM-SOWDOM   KK10  \r\nN-USD213.30\r\nO-XXXX\r\nQ-IEV PS DOK213.30USD213.30END XT16YQ56YR20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01DUDLYA/TARAS MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV PS  FQTV-PS3061447;S2;P1\r\nT-K566-3544667816\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851\r\nFM*M*1\r\nFPCASH\r\nFTITAGVV01-851\r\nFVPS;S2;P1\r\nTKOK30NOV/DOKC32530\r\nRIZTICKET FARE UAH 1702.00\r\nRIZTICKET TAX UAH 457.00\r\nRIZTICKET TTL UAH 2159.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2219.00\r\nENDX\r\n	1	2011-12-02 10:49:07: Билет 566-3544667816 успешно импортирован\r\n	0	\N	\N	\N	\N
1ec75dc07f5341838a4d8e9193f30ebe	Air	2	SYSTEM	2011-12-02 10:57:50	SYSTEM	2011-12-02 10:57:50	AIR_20111202085650.05680.PDT	2011-12-02 10:56:00	AIR-BLK207;7A;;230;0200015231;1A1336371;001001\r\nAMD 0201324422;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A ZDZTXA012;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;\r\nA-HAHN AIR;HR 1691\r\nB-TTP\r\nC-7906/ 0001AASU-7777SVSU-I-0--\r\nD-111201;111202;111202\r\nG-X  ;;MOWMOW;E1\r\nH-003;002ODME;MOSCOW DME       ;MUC;MUNICH           ;AB    8283 S S 11FEB1715 1735 11FEB;OK01;HK01;S ;0;738;;;20K;;1615 ;ET;0320 ;N;1209;RU;DE;1 \r\nH-004;003OMUC;MUNICH           ;DME;MOSCOW DME       ;AB    8282 Q Q 21FEB1020 1630 21FEB;OK01;HK01;S ;0;320;;;20K;1 ;0950 ;ET;0310 ;N;1209;DE;RU;  \r\nU-005X;004MIS 1A OK01;HK01 26OCT;BER;BERLIN;;\r\nK-FEUR168.00     ;UAH1811       ;;;;;;;;;;;UAH3062       ;10.77997   ;;   \r\nKFTF; UAH712      YQ AC; UAH136      RI DP; UAH51       UH SE; UAH87       OY CB; UAH209      RA EB; UAH56       DE SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH712      YQ ;UAH136      RI ;UAH403      XT ;\r\nL-*HR *\r\nM-SAB2RT         ;QAB2RT         \r\nN-NUC152.87;82.74\r\nO-XX30APR;XX30APR\r\nQ-MOW AB MUC152.87AB MOW82.74NUC235.61END ROE0.712992XT51UH87OY209RA56DE;FXP/R,VC-HR\r\nI-001;01RYAKHOVSKAYA/INNA MRS;;APDOK +380 62 3881760 - METINVEST - A;;\r\nSSR OTHS 1A  /PLS ISSUE TICKET BY 2359/04DEC2011\r\nSSR OTHS 1A  /AB RESERVES THE RIGHT TO AUTOCANCEL OR SEND ADM IF NO TKT IS ISSUED\r\nT-K169-3544667817\r\nFENO REFUND/RBK RESTRICTED;S2-3;P1\r\nFM*M*0\r\nFPCASH\r\nFVHR;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///OPLATA NALICHNYMI!!!!\r\nENDX\r\n	1	2011-12-02 10:57:50: Билет 169-3544667817 успешно импортирован\r\n	0	\N	\N	\N	\N
e2bb7500adad49aeae4d50c61c316f8d	Air	2	SYSTEM	2011-12-02 11:04:30	SYSTEM	2011-12-02 11:04:30	AIR_20111202090319.05752.PDT	2011-12-02 11:03:00	AIR-BLK207;RF;;190;0200012163;1A1336371;001001\r\nAMD 0201324434;1/1;    02DEC;AASU\r\nGW3671292;1A1336371\r\nMUC1A          ;  01;DOKC32530;72320942;;;;;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;\r\nB-TRFP\r\nC-7906/ 0001AASU-0001AASU-M---\r\nD-111202;111202;111202\r\nRFDF;01DEC11;D;UAH1431;0;1431;;;;;;XT498;1929;02DEC11\r\nKRF ;QUAH305      HF   ;QUAH8        UD   ;QUAH96       YQ   ;QUAH56       YK   ;QUAH33       UA   ;;;;;;;;;;;;;;;;;;;;;;;;;\r\nI-001;01MERGALIEV/DUISEN MR;;;;\r\nT-E870-3544667752\r\nTBS1-1200\r\nR-870-3544667752;02DEC11\r\nSAC870AIKQ1ZGL7S\r\nFM1A\r\nFPCASH/UAH1929\r\nFTITAGVV01-851\r\nENDX\r\n	1	2011-12-02 11:04:30: Возврат 870-3544667752 успешно импортирован\r\n	0	\N	\N	\N	\N
88330c9349464104a9fa4d5278836b47	Air	2	SYSTEM	2011-12-02 11:30:14	SYSTEM	2011-12-02 11:30:15	AIR_20111202092904.06132.PDT	2011-12-02 11:29:00	AIR-BLK207;7A;;247;0200017358;1A1336371;001001\r\nAMD 0201324512;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZECU6C014;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R6Q3Y \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-2222MVSU-M-E--\r\nD-111201;111202;111202\r\nG-   ;;IEVDOK;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 S S 02DEC1950 2100 02DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD213.00     ;UAH1702       ;;;;;;;;;;;UAH2159       ;7.9899     ;;   \r\nKFTF; UAH349      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH56       YR VA; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH349      HF ;UAH4        UD ;UAH104      XT ;\r\nL-\r\nM-SOWDOM   KK10  \r\nN-USD213.30\r\nO-XXXX\r\nQ-IEV PS DOK213.30USD213.30END XT16YQ56YR20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01CHEREDNICHENKO/YURIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667818\r\nFENON END/REF RESTR/RBK FOC;S2;P1\r\nFM*M*1\r\nFPCASH\r\nFTKK4704/11\r\nFVPS;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1702.00\r\nRIZTICKET TAX UAH 457.00\r\nRIZTICKET TTL UAH 2159.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2219.00\r\nENDX\r\n	1	2011-12-02 11:30:15: Билет 566-3544667818 успешно импортирован\r\n	0	\N	\N	\N	\N
c7377fd3e193412f8ea9fa7230065d83	Air	2	SYSTEM	2011-12-02 11:43:30	SYSTEM	2011-12-02 11:43:30	AIR_20111202094225.06316.PDT	2011-12-02 11:42:00	AIR-BLK207;7A;;247;0200017202;1A1336371;001001\r\nAMD 0201324543;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A Y7D48E023;0606;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;UN QGZFE \r\nA-TRANSAERO AIRLINES;UN 6705\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111201;111202;111202\r\nG-X  ;;DOKDOK;E1\r\nH-010;007ODOK;DONETSK          ;DME;MOSCOW DME       ;UN    0244 I I 27DEC1150 1530 27DEC;OK06;HK06;S ;0;735;;;20K;;;ET;0140 ;N;506;UA;RU;  \r\nH-011;008ODME;MOSCOW DME       ;DOK;DONETSK          ;UN    0243 X X 28DEC1640 1620 28DEC;OK06;HK06;S ;0;735;;;20K;;;ET;0140 ;N;506;RU;UA;  \r\nK-FUSD140.00     ;UAH1119       ;;;;;;;;;;;UAH1945       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH324      YQ AC; UAH324      YR VB; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH690      XT ;\r\nL-\r\nM-IPR3M          ;XEE5M          \r\nN-NUC57.50;82.50\r\nO-27DEC27DEC;28DEC28DEC\r\nQ-DOK UN MOW57.50UN DOK82.50NUC140.00END ROE1.000000XT324YQ324YR42UA;FXP\r\nI-001;01AKSAITOV/RENAT MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /ADTK IN SSR TKNE TILL 1103/DOK/02DEC OR PNR WILL BE CXLD\r\nSSR OTHS 1A  /WATERMARK - SEGS CANX AS NO TKT\r\nT-K670-3544667819\r\nFM*M*5\r\nFPCASH\r\nFVUN;S7-8;P1-6\r\nTKOK01DEC/DOKC32530\r\nOPLDOKC32530/02DEC/3C0\r\nI-002;02DONSKAYA/OLENA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /ADTK IN SSR TKNE TILL 1103/DOK/02DEC OR PNR WILL BE CXLD\r\nSSR OTHS 1A  /WATERMARK - SEGS CANX AS NO TKT\r\nT-K670-3544667820\r\nFM*M*5\r\nFPCASH\r\nFVUN;S7-8;P1-6\r\nTKOK01DEC/DOKC32530\r\nOPLDOKC32530/02DEC/3C0\r\nI-003;03KURKINA/IRYNA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /ADTK IN SSR TKNE TILL 1103/DOK/02DEC OR PNR WILL BE CXLD\r\nSSR OTHS 1A  /WATERMARK - SEGS CANX AS NO TKT\r\nT-K670-3544667821\r\nFM*M*5\r\nFPCASH\r\nFVUN;S7-8;P1-6\r\nTKOK01DEC/DOKC32530\r\nOPLDOKC32530/02DEC/3C0\r\nI-004;04KURYLO/MARIYA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /ADTK IN SSR TKNE TILL 1103/DOK/02DEC OR PNR WILL BE CXLD\r\nSSR OTHS 1A  /WATERMARK - SEGS CANX AS NO TKT\r\nT-K670-3544667822\r\nFM*M*5\r\nFPCASH\r\nFVUN;S7-8;P1-6\r\nTKOK01DEC/DOKC32530\r\nOPLDOKC32530/02DEC/3C0\r\nI-005;05KURYLO/IULIIA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /ADTK IN SSR TKNE TILL 1103/DOK/02DEC OR PNR WILL BE CXLD\r\nSSR OTHS 1A  /WATERMARK - SEGS CANX AS NO TKT\r\nT-K670-3544667823\r\nFM*M*5\r\nFPCASH\r\nFVUN;S7-8;P1-6\r\nTKOK01DEC/DOKC32530\r\nOPLDOKC32530/02DEC/3C0\r\nI-006;06MIROSHNICHENKO/PAVLO MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /ADTK IN SSR TKNE TILL 1103/DOK/02DEC OR PNR WILL BE CXLD\r\nSSR OTHS 1A  /WATERMARK - SEGS CANX AS NO TKT\r\nT-K670-3544667824\r\nFM*M*5\r\nFPCASH\r\nFVUN;S7-8;P1-6\r\nTKOK01DEC/DOKC32530\r\nOPLDOKC32530/02DEC/3C0\r\nENDX\r\n	1	2011-12-02 11:43:30: Билет 670-3544667819 успешно импортирован\r\nБилет 670-3544667820 успешно импортирован\r\nБилет 670-3544667821 успешно импортирован\r\nБилет 670-3544667822 успешно импортирован\r\nБилет 670-3544667823 успешно импортирован\r\nБилет 670-3544667824 успешно импортирован\r\n	0	\N	\N	\N	\N
324aceaeab054c88a8ae5dc1f24f4f55	Air	2	SYSTEM	2011-12-02 12:15:14	SYSTEM	2011-12-02 12:15:14	AIR_20111202101356.06760.PDT	2011-12-02 12:13:00	AIR-BLK207;7A;;247;0200023846;1A1336371;001001\r\nAMD 0201324624;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZL2XR6008;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZL2XR6\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111202;111202;111202\r\nG-   ;;IEVDOK;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0095 B B 02DEC1345 1500 02DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nK-FUSD153.00     ;UAH1223       ;;;;;;;;;;;UAH1559       ;7.9899     ;;   \r\nKFTF; UAH252      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH252      HF ;UAH4        UD ;UAH80       XT ;\r\nL-\r\nM-BOW3M5   KK10  \r\nN-USD153.00\r\nO-XXXX\r\nQ-IEV VV DOK153.00USD153.00END XT48YQ20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01CHEREDNICHENKO/YURIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870000616683;S2;P1\r\nT-K870-3544667825\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 259.83\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 1223.00\r\nRIZTICKET TAX UAH 336.00\r\nRIZTICKET TTL UAH 1559.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1619.00\r\nENDX\r\n	1	2011-12-02 12:15:14: Билет 870-3544667825 успешно импортирован\r\n	0	\N	\N	\N	\N
600819952ad14500b53506b9ec179e56	Air	2	SYSTEM	2011-12-02 12:30:35	SYSTEM	2011-12-02 12:30:35	AIR_20111202102920.06953.PDT	2011-12-02 12:29:00	AIR-BLK207;7A;;247;0200042836;1A1336371;001001\r\nAMD 0201324661;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZJ7CTT006;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZJ7CTT\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111202;111202;111202\r\nG-   ;;DOKDOK;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0092 D D 12DEC0920 1045 12DEC;OK01;HK01;S ;0;734;;;30K;;;ET;0125 ;N;347;UA;UA;B \r\nH-003;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 K K 12DEC2130 2240 12DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD330.00     ;UAH2637       ;;;;;;;;;;;UAH3378       ;7.9899     ;;   \r\nKFTF; UAH548      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH548      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-DEE6M5         ;KSS1M          \r\nN-USD255.00;75.00\r\nO-12DEC12DEC;12DEC12DEC\r\nQ-DOK VV IEV255.00VV DOK75.00USD330.00END XT96YQ56YK33UA;FXP\r\nI-001;01USOVA/NINA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667827\r\nFE.-44877533.-/NONENDO/REFRESTR/RBK USD30 /VAT UAH 563.00;S2-3;P1\r\nFM*M*1A\r\nFPCASH\r\nFVVV;S2-3;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 2637.00\r\nRIZTICKET TAX UAH 741.00\r\nRIZTICKET TTL UAH 3378.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 3438.00\r\nENDX\r\n	1	2011-12-02 12:30:35: Билет 870-3544667827 успешно импортирован\r\n	0	\N	\N	\N	\N
7daca440447142dcad959f0453296e82	Air	2	SYSTEM	2011-12-02 15:04:18	SYSTEM	2011-12-02 15:04:18	AIR_20111202130320.08829.PDT	2011-12-02 15:03:00	AIR-BLK207;7A;;247;0200037985;1A1336371;001001\r\nAMD 0201325029;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A ZJ57BU008;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;Z6 NKQRC \r\nA-DNIPROAVIA;Z6 1816\r\nB-TTP\r\nC-7906/ 2222MVSU-7777SVSU-I-0--\r\nD-111202;111202;111202\r\nG-   ;;SIPIEV;UA\r\nH-003;002OSIP;SIMFEROPOL       ;KBP;KIEV BORYSPIL    ;Z6    0024 D D 03DEC1150 1320 03DEC;OK01;HK01;S ;0;ER4;;;30K;;;ET;0130 ;N;398;UA;UA;B \r\nK-FUSD235.00     ;UAH1878       ;;;;;;;;;;;UAH2403       ;7.9899     ;;   \r\nKFTF; UAH385      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH48       YQ AD; UAH28       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH385      HF ;UAH4        UD ;UAH136      XT ;\r\nL-\r\nM-DOW3M          \r\nN-USD235.00\r\nO-03DEC03DEC\r\nQ-SIP Z6 IEV235.00USD235.00END XT48YQ48YQ28YK12UA;FXP\r\nI-001;01STRAKHOV/OLEKSANDR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /PNR CNCL AS NOT TKNM\r\nT-K181-3544667839\r\nFENON ENDO/RBK10USD REF RESTR /VAT UAH 400.50;S2;P1\r\nFM*M*1A\r\nFPCASH\r\nFVZ6;S2;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 1878.00\r\nRIZTICKET TAX UAH 525.00\r\nRIZTICKET TTL UAH 2403.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2463.00\r\nENDX\r\n	1	2011-12-02 15:04:18: Билет 181-3544667839 успешно импортирован\r\n	0	\N	\N	\N	\N
06bbb0edeebf4c3782a4bcd5b885af20	Mir	2		2011-12-02 12:18:50		2011-12-02 12:18:50	AAAAGGAL.MIR	2011-12-02 12:18:50	T51G773392014730001602DEC111017 VV870AEROSVIT AIRLINES       10JAN12FE195DFE1C8A\r30AC30AD72320942 SWXP7G         061193N90AG01DEC1100102DEC11010\rUAH0000000032610UAH00000000  00000000  00000000  00000000  00000000  000000000500               \rNNNYN5NNYAYH NNNX   UA                             \r000000003000002000000001000001001000001000000000\r\rA02GUSHANSKYI/SERGIIMR              336089379011297114951601000000008ADT   01  N\rA02BULGAKOVA/IRYNAMRS               336089380041297114951701000000008ADT   01  N\rA02KORNIENKO/VIKTORIYAMRS           336089381001297114951801000000008ADT   01  N\r\rA0401VV870AEROSVIT AIR 301R HK10JAN0740 1135 2DOKDONETSK      SVOMOSCOW/SHEREMINL   O0   735    00546F TK:YJT:01.55ANL:AEROSVIT AIRLINES       \rA0402VV870AEROSVIT AIR 302W HK22JAN1235 1210 2SVOMOSCOW/SHEREMDOKDONETSK      INL   O0   735  C 00546F TK:YJT:01.35ANL:AEROSVIT AIRLINES       \r\rA0701USD      136.00UAH        1265UAH        1087UAHT1:      42UAT2:      16UDT3:     120YK\r\rA080101RPR1M5  0000000010JAN1210JAN12      F:RPR1M5         E:NONENDO/REFRESTR/NONREBOOK                                  B:20K\rA080102WSALE   0000000022JAN1222JAN12      F:WSALE          E:NONENDO/REFRESTR/NONREBOOK                                  B:20K\r\rA09010DOK VV MOW 85.00VV DOK 51.00 NUC136.00END ROE1.0\r\rA11S         3795N                                     \r\rA12IEVT *380 62 334 32 22-TRAVEL SHOP test\r\rA14VL-131401DECMUCRM1AZBJBIX\r\rA24010DOK VV MOW 85.00VV DOK 51.00 NUC136.00END ROE1.0\r\r\f	1	2011-12-02 12:18:50: Билет 870-2971149516 успешно импортирован\r\nБилет 870-2971149517 успешно импортирован\r\nБилет 870-2971149518 успешно импортирован\r\n	1	\N	\N	\N	\N
27e69602ca31495db1740c3e76c6e445	Air	2	SYSTEM	2011-12-02 12:22:12	SYSTEM	2011-12-02 12:22:12	AIR_20111202102110.06833.PDT	2011-12-02 12:21:00	AIR-BLK207;7A;;247;0200013983;1A1336371;001001\r\nAMD 0201324638;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A Y77AQK013;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y77AQK\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/ITR-EML-SKRIPNICHENKO@MAC.COM\r\nC-7906/ 0001AASU-0001AASU-I-0--\r\nD-111201;111202;111202\r\nG-   ;;IEVIEV;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0091 K K 03DEC0910 1025 03DEC;OK01;HK01;N ;0;735;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nH-002;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0092 Q Q 04DEC0920 1045 04DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0125 ;N;347;UA;UA;B \r\nK-FUSD170.00     ;UAH1359       ;;;;;;;;;;;UAH1844       ;7.9897     ;;   \r\nKFTF; UAH292      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH292      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-KSS1M          ;QPX1M5         \r\nN-USD75.00;95.00\r\nO-03DEC03DEC;04DEC04DEC\r\nQ-IEV VV DOK75.00VV IEV95.00USD170.00END XT96YQ56YK33UA;FXB\r\nI-001;01SKRYPNYCHENKO/OLEG MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV87000088737;S2;P1\r\nFQV VV  FQTV-VV87000088737;S3;P1\r\nT-K870-3544667826\r\nFE.-44877533.-/ NONENDO/REFRESTR/RBK USD30 /VAT UAH 307.33;S2-3;P1\r\nFM*M*1A\r\nFPCASH\r\nFVVV;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1359.00\r\nRIZTICKET TAX UAH 485.00\r\nRIZTICKET TTL UAH 1844.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1904.00\r\nENDX\r\n	1	2011-12-02 12:22:12: Билет 870-3544667826 успешно импортирован\r\n	0	\N	\N	\N	\N
08d9e956205540c5ac938990f5224a7e	Air	2	SYSTEM	2011-12-02 13:06:27	SYSTEM	2011-12-02 13:06:27	AIR_20111202110514.07435.PDT	2011-12-02 13:05:00	AIR-BLK207;7A;;247;0200047628;1A1336371;001001\r\nAMD 0201324764;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y7DYKL012;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;UN QGZ9F \r\nA-TRANSAERO AIRLINES;UN 6705\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-2222MVSU-I-0--\r\nD-111201;111202;111202\r\nG-X  ;;MOWDOK;E1\r\nH-002;002ODME;MOSCOW DME       ;DOK;DONETSK          ;UN    0243 C C 04DEC1640 1620 04DEC;OK01;HK01;S ;0;735;;;35K;;;ET;0140 ;N;506;RU;UA;  \r\nK-FEUR285.00     ;UAH3073       ;;;;;;;;;;;UAH3397       ;10.77997   ;;   \r\nKFTF; UAH162      YQ AC; UAH162      YR VB;;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH162      YQ ;UAH162      YR ;;\r\nL-\r\nM-COW            \r\nN-NUC399.72\r\nO-XXXX\r\nQ-MOW UN DOK399.72NUC399.72END ROE0.712992;FXP\r\nI-001;01BOGOMOLSKY/EVGENY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K670-3544667828\r\nFM*M*5\r\nFPCASH\r\nFVUN;S2;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 3073.00\r\nRIZTICKET TAX UAH 324.00\r\nRIZTICKET TTL UAH 3397.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 3517.00\r\nENDX\r\n	1	2011-12-02 13:06:27: Билет 670-3544667828 успешно импортирован\r\n	0	\N	\N	\N	\N
97b5faba5dfb45af9d2a1adebcc8b924	Air	2	SYSTEM	2011-12-02 13:47:14	SYSTEM	2011-12-02 13:47:14	AIR_20111202114608.07921.PDT	2011-12-02 13:46:00	AIR-BLK207;RF;;190;0200024764;1A1336371;001001\r\nAMD 0201324873;1/1;    02DEC;SVSU\r\nGW3670821;1A1336371\r\nMUC1A          ;  01;DOKC32530;72320942;;;;;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;\r\nB-TRFP\r\nC-7906/ 7777SVSU-7777SVSU-M---\r\nD-111202;111202;111202\r\nRFDF;23NOV11;I;UAH2868;0;2868;;;;;;XT372;3240;08DEC11\r\nKRF ;QUAH196      YK   ;QUAH24       UD   ;QUAH96       YQ   ;QUAH56       UA   ;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nI-001;01HERASYMCHUK/OLEKSANDR MR;;;;\r\nT-E870-3544617201\r\nTBS1-1234\r\nR-870-3544617201;02DEC11\r\nSAC870AIKQ1ZFI7L\r\nFM1.00P\r\nFPCASH/UAH3240\r\nFTITAGVV01-801\r\nENDX\r\n	1	2011-12-02 13:47:14: Возврат 870-3544617201 успешно импортирован\r\n	0	\N	\N	\N	\N
6bb8b411f13a4b5d926e9f42dfccb0e5	Air	2	SYSTEM	2011-12-02 13:47:58	SYSTEM	2011-12-02 13:47:58	AIR_20111202114649.07935.PDT	2011-12-02 13:46:00	AIR-BLK207;RF;;190;0200027841;1A1336371;001001\r\nAMD 0201324876;1/1;    02DEC;SVSU\r\nGW3670821;1A1336371\r\nMUC1A          ;  01;DOKC32530;72320942;;;;;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;\r\nB-TRFP\r\nC-7906/ 7777SVSU-7777SVSU-M---\r\nD-111202;111202;111202\r\nRFDF;23NOV11;I;UAH2868;0;2868;;;;;;XT372;3240;08DEC11\r\nKRF ;QUAH196      YK   ;QUAH24       UD   ;QUAH96       YQ   ;QUAH56       UA   ;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nI-001;01FEDOTOV/OLEKSII MR;;;;\r\nT-E870-3544617200\r\nTBS1-1234\r\nR-870-3544617200;02DEC11\r\nSAC870AIKQ1ZFI7K\r\nFM1.00P\r\nFPCASH/UAH3240\r\nFTITAGVV01-801\r\nENDX\r\n	1	2011-12-02 13:47:58: Возврат 870-3544617200 успешно импортирован\r\n	0	\N	\N	\N	\N
0b2b7c2cbbc34826a5a8f987b63fc282	Air	2	SYSTEM	2011-12-02 15:06:45	SYSTEM	2011-12-02 15:06:45	AIR_20111202130547.08852.PDT	2011-12-02 15:05:00	AIR-BLK207;7A;;247;0200030351;1A1336371;001001\r\nAMD 0201325037;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A ZMHC77004;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZMHC77\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 0001AASU-0001AASU-I-0--\r\nD-111202;111202;111202\r\nG-X  ;;TBSDOK;E1\r\nH-002;002OTBS;TBILISI          ;DOK;DONETSK          ;VV    0320 B B 03DEC0615 0600 03DEC;OK01;HK01;L ;0;735;;;20K;;;ET;0145 ;N;566;GE;UA;  \r\nK-FEUR140.00     ;UAH1510       ;;;;;;;;;;;UAH2286       ;10.77997   ;;   \r\nKFTF; UAH560      YQ AC; UAH176      GE DP; UAH40       JA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH560      YQ ;UAH176      GE ;UAH40       JA ;\r\nL-\r\nM-BOW6M5         \r\nN-NUC196.35\r\nO-03DEC03DEC\r\nQ-TBS VV DOK196.35NUC196.35END ROE0.712992;FXB\r\nI-001;01ARTYUKH/SERGIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV87001245157;S2;P1\r\nT-K870-3544667840\r\nFE.-44877533.-/NON ENDO/REBOOK RESTR;S2;P1\r\nFM*M*1\r\nFPCASH\r\nFVVV;S2;P1\r\nTKOK02DEC/DOKC32530\r\nENDX\r\n	1	2011-12-02 15:06:45: Билет 870-3544667840 успешно импортирован\r\n	0	\N	\N	\N	\N
809be9c0fd344dd4809b2deb7aa1bbe3	Air	2	SYSTEM	2011-12-02 13:21:45	SYSTEM	2011-12-02 13:21:45	AIR_20111202112047.07623.PDT	2011-12-02 13:20:00	AIR-BLK207;7A;;247;0200049518;1A1336371;001001\r\nAMD 0201324808;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZJMU94007;0202;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZJMU94\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111202;111202;111202\r\nG-   ;;DOKIEV;UA\r\nH-002;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0090 L L 05DEC0700 0820 05DEC;OK02;HK02;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nK-FUSD125.00     ;UAH999        ;;;;;;;;;;;UAH1320       ;7.9899     ;;   \r\nKFTF; UAH212      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH212      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-LOW1M5         \r\nN-USD125.00\r\nO-05DEC05DEC\r\nQ-DOK VV IEV125.00USD125.00END XT48YQ36YK21UA;FXB\r\nI-002;01SVETLICHNAYA/LYUDMYLA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667829\r\nFE.-44877533.-/NONENDO/REBOOK USD20 /VAT UAH 220.00;S3;P1-2\r\nFM*M*1A\r\nFPCASH\r\nFVVV;S3;P1-2\r\nTKOK02DEC/DOKC32530\r\nI-001;02SVETLICHNYY/SERGIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667830\r\nFE.-44877533.-/NONENDO/REBOOK USD20 /VAT UAH 220.00;S3;P1-2\r\nFM*M*1A\r\nFPCASH\r\nFVVV;S3;P1-2\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 999.00\r\nRIZTICKET TAX UAH 321.00\r\nRIZTICKET TTL UAH 1320.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1380.00\r\nENDX\r\n	1	2011-12-02 13:21:45: Билет 870-3544667829 успешно импортирован\r\nБилет 870-3544667830 успешно импортирован\r\n	0	\N	\N	\N	\N
6be369ad16dc4fcd84259897d8443d35	Air	2	SYSTEM	2011-12-02 13:41:57	SYSTEM	2011-12-02 13:41:57	AIR_20111202114043.07865.PDT	2011-12-02 13:40:00	AIR-BLK207;7A;;239;0200032276;1A1336371;001001\r\nAMD 0201324864;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A 76V92W022;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 76V92W\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/P1\r\nC-7906/ 0001AASU-7777SVSU-M-V--\r\nD-111125;111202;111202\r\nG-X  ;;IEVDOK;E1\r\nH-007;002OKBP;KIEV BORYSPIL    ;SVO;MOSCOW SVO       ;VV    4805 K K 04DEC1035 1410 04DEC;OK01;HK01;;0;320;;;1PC;B ;;ET;0135 ; ;473;UA;RU;F \r\nX-007;002OKBP;KIEV BORYSPIL    ;SVO;MOSCOW SVO       ;SU    1805 V K 04DEC1035 1410 04DEC;OK01;HK01;;0;320;;;1PC;B ;;ET;0135 ; ;F \r\nH-006;003OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 K K 10DEC1235 1210 10DEC;OK01;HK01;L ;0;735;;;20K;C ;;ET;0135 ;N;545;RU;UA;  \r\nK-RUSD240.00     ;UAH           ;;;;;;;;;;;UAH112        ;;;   \r\nKFTR;OUAH32       YQ AC;OUAH136      YK AE;OUAH16       UD DP;OUAH32       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-PD 32       YQ ;PD 136      YK ;PD 48       XT ;\r\nL-\r\nM-KPX7           ;KPX3M5         \r\nN-NUC135.00;105.00\r\nO-04DEC04DEC;10DEC10DEC\r\nQ-IEV VV MOW135.00VV DOK105.00NUC240.00END ROE1.000000PD XT16UD32UA;FXP/R,25NOV11\r\nI-001;01KARPUNIN/IGOR MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nSSR OTHS 1A  /PLS ADV TKNA/TKNM BY 0300TUL/03DEC OR WILL BE CNLD\r\nT-K870-3544667831\r\nFENONENDO/REF/REBOOK RESTR;S2-3;P1\r\nFM*M*1\r\nFO870-3544644539DOK25NOV11/72320942/870-35446445396E12\r\nFPO/CASH+/CASH\r\nFTITAGVV01-801\r\nFVVV;S2-3;P1\r\nTKTL03DEC/0300/KBPVV0100\r\nRM PISHEM SEYCHAS/////BRDS///4000//MVZ////\r\nENDX\r\n	1	2011-12-02 13:41:57: Билет 870-3544667831 успешно импортирован\r\n	0	\N	\N	\N	\N
59b03b0f38b44c3893fadd3e83d8a064	Air	2	SYSTEM	2011-12-02 13:43:22	SYSTEM	2011-12-02 13:43:23	AIR_20111202114217.07888.PDT	2011-12-02 13:42:00	AIR-BLK207;7M;;239;0200031269;1A1336371;001001\r\nAMD 0201324869;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A 76V92W025;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 76V92W\r\nB-TTM\r\nC-7906/ 0001AASU-7777SVSU-M---\r\nD-111125;111202;111202\r\nU-007X;002OKBP;KIEV BORYSPIL    ;SVO;MOSCOW SVO       ;VV    4805 K K 04DEC1035 1410 04DEC;OK01;HK01;;0;320;;;;B ;;ET;0135 ; ;;473;;UA;RU;F \r\nX-007;002OKBP;KIEV BORYSPIL    ;SVO;MOSCOW SVO       ;SU    1805 K K 04DEC1035 1410 04DEC;OK01;HK01;;0;320;;;;B ;;ET;0135 ; ;F \r\nU-006X;003OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 K K 10DEC1235 1210 10DEC;OK01;HK01;L ;0;735;;;;C ;;ET;0135 ;N;;545;;RU;UA;  \r\nMCO185;004VV;8702;AEROSVIT AIRLINES;KBP;TO-AEROSVIT AIRLINES;AT-KIEV;04DEC;M;A;Q;**-;;F;UAH        324;N;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;UAH        324;;;;;;;UAH        324;;;; ;P1\r\nI-001;01KARPUNIN/IGOR MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nTMCM870-9074146869;;L4\r\nMFM*M*0;L4\r\nMFPCASH;L4\r\nTKOK02DEC/DOKC32530//ETVV\r\nRM PISHEM SEYCHAS/////BRDS///4000//MVZ////\r\nENDX\r\n	1	2011-12-02 13:43:23: MCO 870-9074146869 успешно импортирован\r\n	0	\N	\N	\N	\N
37cb43dd54244bc09496ba29e2bc9089	Air	2	SYSTEM	2011-12-02 14:06:28	SYSTEM	2011-12-02 14:06:29	AIR_20111202120522.08156.PDT	2011-12-02 14:05:00	AIR-BLK207;7A;;247;0200031420;1A1336371;001001\r\nAMD 0201324907;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y9L7NG012;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y9L7NG\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111202;111202\r\nG-X  ;;DOKDOK;E1\r\nH-001;002ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0301 Q Q 06DEC0740 1135 06DEC;OK01;HK01;L ;0;735;;;20K;;;ET;0155 ;N;545;UA;RU;C \r\nH-002;003OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0306 Q Q 08DEC2245 2230 08DEC;OK01;HK01;L ;0;735;;;20K;C ;;ET;0145 ;N;545;RU;UA;  \r\nK-FUSD207.00     ;UAH1654       ;;;;;;;;;;;UAH1832       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-Q2MRT    KK10  ;Q2MRT    KK10  \r\nN-NUC103.50;103.50\r\nO-XX06FEB;XX06FEB\r\nQ-DOK VV MOW103.50VV DOK103.50NUC207.00END ROE1.000000;FXP/ZO-10P*KK10\r\nI-001;01KOKIASHVILI/GEORGY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870012269454;S2;P1\r\nFQV VV  FQTV-VV870012269454;S3;P1\r\nT-K870-3544667832\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1654.00\r\nRIZTICKET TAX UAH 178.00\r\nRIZTICKET TTL UAH 1832.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 1952.00\r\nENDX\r\n	1	2011-12-02 14:06:29: Билет 870-3544667832 успешно импортирован\r\n	0	\N	\N	\N	\N
d73dc485a0f34f4f8fb4f1467ff3fd2a	Air	2	SYSTEM	2011-12-02 14:19:43	SYSTEM	2011-12-02 14:19:44	AIR_20111202121841.08316.PDT	2011-12-02 14:18:00	AIR-BLK207;MA;;233;0200030370;1A1336371;001001\r\nAMD 0201324936;1/1;VOID02DEC;MVSU\r\nGW3718884;1A1336371\r\nMUC1A ZECU6C014;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R6Q3Y \r\nI-001;01CHEREDNICHENKO/YURIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667818\r\nFPCASH\r\nENDX\r\n	1	2011-12-02 14:19:44: Void 566-3544667818 успешно импортирован\r\n	0	\N	\N	\N	\N
17effd1489c54d27b6c1afd581db74d6	Air	2	SYSTEM	2011-12-05 18:17:25	SYSTEM	2011-12-05 18:17:25	AIR_20111205161708.20543.PDT	2011-12-05 18:17:00	AIR-BLK207;MA;;233;0500048206;1A1336371;001001\r\nAMD 0501327079;1/1;VOID05DEC;SVSU\r\nGW3718884;1A1336371\r\nMUC1A 2FEPVR007;0301;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 2FEPVR\r\nI-002;03LESIV/VOLODYMYR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667901\r\nFPCASH\r\nENDX\r\n	1	2011-12-05 18:17:25: Void 870-3544667901 успешно импортирован\r\n	0	\N	\N	\N	\N
1f5b96fdaa4849d7a1f0a97adf2c5bb4	Air	2	SYSTEM	2011-12-02 14:23:42	SYSTEM	2011-12-02 14:23:43	AIR_20111202122226.08356.PDT	2011-12-02 14:22:00	AIR-BLK207;7A;;267;0200032754;1A1336371;001001\r\nAMD 0201324941;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A YVUQGG037;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;BD YVUQGG;LH YVUQGG;OS YVUQGG\r\nA-LUFTHANSA;LH 2203\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-2222MVSU-I-0--\r\nD-111130;111202;111202\r\nG-X  ;;IEVIEV;E1\r\nH-029;002OKBP;KIEV BORYSPIL    ;MUC;MUNICH           ;LH    2547 G G 05DEC0630 0805 05DEC;OK01;HK01;S ;0;321;;;1PC;F ;0545 ;ET;0235 ; ;872;UA;DE;2 \r\nH-030;003XMUC;MUNICH           ;LHR;LONDON LHR       ;LH    2472 G G 05DEC0915 1020 05DEC;OK01;HK01;S ;0;321;;;1PC;2 ;0835 ;ET;0205 ; ;594;DE;GB;1 \r\nH-031;004OLHR;LONDON LHR       ;VIE;VIENNA SCHWECHAT ;OS    8930 V V 07DEC1350 1705 07DEC;OK01;HK01;S ;0;319;;;20K;1 ;1250 ;ET;0215 ;N;800;GB;AT;  \r\nX-031;004OLHR;LONDON LHR       ;VIE;VIENNA SCHWECHAT ;BD    0427 V V 07DEC1350 1705 07DEC;OK01;HK01;S ;0;319;;;20K;1 ;1250 ;ET;0215 ;N;  \r\nY-031;004LHR;LONDON LHR;VIE;VIENNA SCHWECHAT;BD;BRITISH MIDLAND INTERNATIONAL;;;;;319;N\r\nH-033;005XVIE;VIENNA SCHWECHAT ;KBP;KIEV BORYSPIL    ;OS    7173 V V 07DEC1750 2045 07DEC;OK01;HK01;;0;735;;;20K;;1705 ;ET;0155 ;N;667;AT;UA;F \r\nX-033;005XVIE;VIENNA SCHWECHAT ;KBP;KIEV BORYSPIL    ;PS    0848 H V 07DEC1750 2045 07DEC;OK01;HK01;;0;735;;;20K;;1705 ;ET;0155 ;N;F \r\nY-033;005VIE;VIENNA SCHWECHAT;KBP;KIEV BORYSPIL;PS;UKRAINE INTL AIRLINES;;;;;735;N\r\nK-FUSD495.00     ;UAH3955       ;;;;;;;;;;;UAH6573       ;7.9899     ;;   \r\nKFTF; UAH136      YK AE; UAH16       UD DP; UAH1552     YQ AC; UAH32       UA SE; UAH170      RA EB; UAH56       DE SE; UAH149      GB AD; UAH275      UB AS; UAH183      ZY AE; UAH49       AT SE;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH136      YK ;UAH16       UD ;UAH2466     XT ;\r\nL-\r\nM-GNC66W2        ;GNC66W2        ;VFLYOS1U       ;VFLYOS1U       \r\nN-NUC;265.00;;229.50\r\nO-05DEC05DEC;05DEC05DEC;07DEC07DEC;07DEC07DEC\r\nQ-IEV LH X/MUC LH LON265.00OS X/VIE OS IEV229.50NUC494.50END ROE1.000000XT1552YQ32UA170RA56DE149GB275UB183ZY49AT;FXB\r\nI-001;01LEONENKO/RUSLAN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nOSI LH  DS/EXPUA23969\r\nT-K220-3544667833\r\nFENONREF/CHNGS RESTRNONENDO;S2-5;P1\r\nFM*M*1A\r\nFPCASH\r\nFTUA1307\r\nFVLH;S2-5;P1\r\nTKOK30NOV/DOKC32530\r\nRIZTICKET FARE UAH 3955.00\r\nRIZTICKET TAX UAH 2618.00\r\nRIZTICKET TTL UAH 6573.00\r\nRIZSERVICE FEE UAH 150.00+VAT 30.00\r\nRIZGRAND TOTAL UAH 6753.00\r\nENDX\r\n	1	2011-12-02 14:23:43: Билет 220-3544667833 успешно импортирован\r\n	0	\N	\N	\N	\N
0c338d44bbd34f8b8a65fc9891eeacd3	Air	2	SYSTEM	2011-12-02 14:32:06	SYSTEM	2011-12-02 14:32:06	AIR_20111202123056.08433.PDT	2011-12-02 14:30:00	AIR-BLK207;7A;;247;0200057803;1A1336371;001001\r\nAMD 0201324959;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZOCLUK006;0202;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R2J3T \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111202;111202;111202\r\nG-   ;;IEVDOK;UA\r\nH-001;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 S S 02DEC1950 2100 02DEC;OK02;HK02;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD213.00     ;UAH1702       ;;;;;;;;;;;UAH2159       ;7.9899     ;;   \r\nKFTF; UAH349      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH56       YR VA; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH349      HF ;UAH4        UD ;UAH104      XT ;\r\nL-\r\nM-SOWDOM   KK10  \r\nN-USD213.30\r\nO-XXXX\r\nQ-IEV PS DOK213.30USD213.30END XT16YQ56YR20YK12UA;FXP/ZO-10P*KK10\r\nI-002;01KULYASOVA/OLGA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667834\r\nFENON END/REF RESTR/RBK FOC;S3;P1-2\r\nFM*M*1\r\nFPCASH\r\nFTKK4704/11\r\nFVPS;S3;P1-2\r\nTKOK02DEC/DOKC32530\r\nI-001;02SOSHKINA/RUSLANA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667835\r\nFENON END/REF RESTR/RBK FOC;S3;P1-2\r\nFM*M*1\r\nFPCASH\r\nFTKK4704/11\r\nFVPS;S3;P1-2\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 1702.00\r\nRIZTICKET TAX UAH 457.00\r\nRIZTICKET TTL UAH 2159.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2219.00\r\nENDX\r\n	1	2011-12-02 14:32:06: Билет 566-3544667834 успешно импортирован\r\nБилет 566-3544667835 успешно импортирован\r\n	0	\N	\N	\N	\N
0c11ed3b4ac548749bc179de43a61f21	Air	2	SYSTEM	2011-12-02 14:39:07	SYSTEM	2011-12-02 14:39:07	AIR_20111202123804.08499.PDT	2011-12-02 14:38:00	AIR-BLK207;7A;;247;0200035703;1A1336371;001001\r\nAMD 0201324975;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y6M3PZ016;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y6M3PZ\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111201;111202;111202\r\nG-   ;;IEVIEV;UA\r\nH-003;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0095 L L 03DEC1345 1500 03DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nH-004;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 K K 06DEC1950 2100 06DEC;OK01;HK01;N ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nK-FUSD179.00     ;UAH1431       ;;;;;;;;;;;UAH1929       ;7.9899     ;;   \r\nKFTF; UAH305      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH305      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-LPX1M5   KK6   ;KSS1M    KK6   \r\nN-USD108.10;70.50\r\nO-03DEC03DEC;06DEC06DEC\r\nQ-IEV VV DOK108.10VV IEV70.50USD178.60END XT96YQ56YK33UA;FXP/ZO-6P*KK6\r\nI-001;01KOVALISHYN/ANTON MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667836\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGREEMENT 01-851 /VAT UAH 321.50\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1431.00\r\nRIZTICKET TAX UAH 498.00\r\nRIZTICKET TTL UAH 1929.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1989.00\r\nENDX\r\n	1	2011-12-02 14:39:07: Билет 870-3544667836 успешно импортирован\r\n	0	\N	\N	\N	\N
26d87fc4db524c12acb461278da2d3c3	Air	2	SYSTEM	2011-12-02 14:42:17	SYSTEM	2011-12-02 14:42:17	AIR_20111202124101.08532.PDT	2011-12-02 14:41:00	AIR-BLK207;7A;;247;0200034278;1A1336371;001001\r\nAMD 0201324982;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZL2P34004;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;SU JKFHIR\r\nA-AEROFLOT;SU 5552\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111202;111202;111202\r\nG-X  ;;IEVMOW;E1\r\nH-001;002OKBP;KIEV BORYSPIL    ;SVO;MOSCOW SVO       ;SU    1805 D D 04DEC1035 1410 04DEC;OK01;HK01;L ;0;320;;;2PC;B ;;ET;0135 ; ;473;UA;RU;F \r\nK-FUSD840.00     ;UAH6712       ;;;;;;;;;;;UAH6932       ;7.9899     ;;   \r\nKFTF; UAH136      YK AE; UAH16       UD DP; UAH36       YR VB; UAH32       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH136      YK ;UAH16       UD ;UAH68       XT ;\r\nL-\r\nM-DNOW           \r\nN-NUC840.00\r\nO-XXXX\r\nQ-IEV SU MOW840.00NUC840.00END ROE1.000000XT36YR32UA;FXP\r\nI-001;01TIMCHENKO/MAKSYM MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K555-3544667837\r\nFM*M*5\r\nFPCASH\r\nFVSU;S2;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 6712.00\r\nRIZTICKET TAX UAH 220.00\r\nRIZTICKET TTL UAH 6932.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 7052.00\r\nENDX\r\n	1	2011-12-02 14:42:17: Билет 555-3544667837 успешно импортирован\r\n	0	\N	\N	\N	\N
2a2263252a62473dae7a20761705ee0d	Air	2	SYSTEM	2011-12-02 14:50:40	SYSTEM	2011-12-02 14:50:40	AIR_20111202124934.08650.PDT	2011-12-02 14:49:00	AIR-BLK207;7A;;247;0200030835;1A1336371;001001\r\nAMD 0201325001;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A ZNWKOF007;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R2JKF \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 0001AASU-0001AASU-I-0--\r\nD-111202;111202;111202\r\nG-   ;;IEVDOK;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 Q Q 05DEC1950 2100 05DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD122.00     ;UAH975        ;;;;;;;;;;;UAH1286       ;7.9899     ;;   \r\nKFTF; UAH203      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH56       YR VA; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH203      HF ;UAH4        UD ;UAH104      XT ;\r\nL-\r\nM-QOWDOM         \r\nN-USD122.00\r\nO-05DEC05DEC\r\nQ-IEV PS DOK122.00USD122.00END XT16YQ56YR20YK12UA;FXB\r\nI-001;01KULIKOVA/OKSANA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667838\r\nFENON END/NO REF/RBK 20USD;S2;P1\r\nFM*M*1\r\nFPCASH\r\nFVPS;S2;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 975.00\r\nRIZTICKET TAX UAH 311.00\r\nRIZTICKET TTL UAH 1286.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1346.00\r\nENDX\r\n	1	2011-12-02 14:50:40: Билет 566-3544667838 успешно импортирован\r\n	0	\N	\N	\N	\N
14429dc628ac474aa932459ee8ff7775	Air	2	SYSTEM	2011-12-02 15:15:08	SYSTEM	2011-12-02 15:15:08	AIR_20111202131358.08951.PDT	2011-12-02 15:13:00	AIR-BLK207;7A;;247;0200063041;1A1336371;001001\r\nAMD 0201325057;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A ZNS2S3007;0404;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZNS2S3\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111202;111202;111202\r\nG-X  ;;DNKDNK;E1\r\nH-007;005ODNK;DNIPROPETROVSK   ;KBP;KIEV BORYSPIL    ;VV    4004 B B 08DEC1100 1200 08DEC;OK04;HK04;N ;0;ER4;;;20K;;;ET;0100 ; ;226;UA;UA;B \r\nX-007;005ODNK;DNIPROPETROVSK   ;KBP;KIEV BORYSPIL    ;Z6    0004 B B 08DEC1100 1200 08DEC;OK04;HK04;N ;0;ER4;;;20K;;;ET;0100 ; ;B \r\nH-008;006XKBP;KIEV BORYSPIL    ;LED;ST PETERSBURG    ;VV    4101 B B 08DEC1430 1835 08DEC;OK04;HK04;;0;E95;;;20K;B ;;ET;0205 ; ;665;UA;RU;2 \r\nX-008;006XKBP;KIEV BORYSPIL    ;LED;ST PETERSBURG    ;7D    0401 Y B 08DEC1430 1835 08DEC;OK04;HK04;;0;E95;;;20K;B ;;ET;0205 ; ;2 \r\nH-009;007OLED;ST PETERSBURG    ;KBP;KIEV BORYSPIL    ;VV    0402 L L 10DEC1935 1925 10DEC;OK04;HK04;L ;0;735;;;20K;2 ;;ET;0150 ;N;665;RU;UA;B \r\nH-010;008XKBP;KIEV BORYSPIL    ;DNK;DNIPROPETROVSK   ;VV    4007 L L 10DEC2115 2215 10DEC;OK04;HK04;N ;0;735;;;20K;A ;;ET;0100 ; ;226;UA;UA;  \r\nX-010;008XKBP;KIEV BORYSPIL    ;DNK;DNIPROPETROVSK   ;PS    0071 L L 10DEC2115 2215 10DEC;OK04;HK04;N ;0;735;;;20K;A ;;ET;0100 ; ;  \r\nK-FUSD499.00     ;UAH3987       ;;;;;;;;;;;UAH4359       ;7.9899     ;;   \r\nKFTF; UAH196      YK AE; UAH24       UD DP; UAH96       YQ AC; UAH56       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH196      YK ;UAH24       UD ;UAH152      XT ;\r\nL-\r\nM-BEE6M5         ;BEE6M5         ;LEE6M5         ;LEE6M5         \r\nN-NUC;299.50;;199.50\r\nO-08DEC08DEC;08DEC08DEC;10DEC10DEC;10DEC10DEC\r\nQ-DNK VV X/IEV VV LED299.50VV X/IEV VV DNK199.50NUC499.00END ROE1.000000XT96YQ56UA;FXB\r\nI-001;01FEDIN/KOSTYANTYN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667841\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851\r\nFM*M*1\r\nFPCASH\r\nFVVV;S5-8;P1-4\r\nTKOK02DEC/DOKC32530\r\nI-004;02FEDOTOV/OLEKSII MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667842\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851\r\nFM*M*1\r\nFPCASH\r\nFVVV;S5-8;P1-4\r\nTKOK02DEC/DOKC32530\r\nI-003;03MAKSIMENKO/OLEG MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667843\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851\r\nFM*M*1\r\nFPCASH\r\nFVVV;S5-8;P1-4\r\nTKOK02DEC/DOKC32530\r\nI-002;04MALYKH/DMITRIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667844\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851\r\nFM*M*1\r\nFPCASH\r\nFVVV;S5-8;P1-4\r\nTKOK02DEC/DOKC32530\r\nENDX\r\n	1	2011-12-02 15:15:08: Билет 870-3544667841 успешно импортирован\r\nБилет 870-3544667842 успешно импортирован\r\nБилет 870-3544667843 успешно импортирован\r\nБилет 870-3544667844 успешно импортирован\r\n	0	\N	\N	\N	\N
d3eae09aa6fa494d97f83274317a54fe	Air	2	SYSTEM	2011-12-02 15:24:53	SYSTEM	2011-12-02 15:24:53	AIR_20111202132341.09055.PDT	2011-12-02 15:23:00	AIR-BLK207;7A;;247;0200064283;1A1336371;001001\r\nAMD 0201325076;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y8HWK4012;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y8HWK4\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-2222MVSU-M-E--\r\nD-111201;111202;111202\r\nG-   ;;IEVIEV;UA\r\nH-003;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 Q Q 05DEC2130 2240 05DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nH-004;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0090 K K 08DEC0700 0820 08DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nK-FUSD160.00     ;UAH1279       ;;;;;;;;;;;UAH1747       ;7.9899     ;;   \r\nKFTF; UAH275      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH275      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-QPX1M5   KK6   ;KSS1M    KK6   \r\nN-USD89.30;70.50\r\nO-05DEC05DEC;08DEC08DEC\r\nQ-IEV VV DOK89.30VV IEV70.50USD159.80END XT96YQ56YK33UA;FXP/ZO-6P*KK6\r\nI-001;01MOROZ/YEVGEN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667845\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGREEMENT 01-851 /VAT UAH 291.17\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK01DEC/DOKC32530\r\nRIZTICKET FARE UAH 1279.00\r\nRIZTICKET TAX UAH 468.00\r\nRIZTICKET TTL UAH 1747.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1807.00\r\nENDX\r\n	1	2011-12-02 15:24:53: Билет 870-3544667845 успешно импортирован\r\n	0	\N	\N	\N	\N
1d35c3e39d654eab823431a316f6059a	Air	2	SYSTEM	2011-12-02 16:46:05	SYSTEM	2011-12-02 16:46:05	AIR_20111202144505.10051.PDT	2011-12-02 16:45:00	AIR-BLK207;7A;;257;0200049645;1A1336371;001001\r\nAMD 0201325305;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A ZKVZ8E019;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LH ZKVZ8E;OS ZKVZ8E\r\nA-AUSTRIAN;OS 2575\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111202;111202;111202\r\nG-X  ;;DOKDOK;E1\r\nH-001;002ODOK;DONETSK          ;VIE;VIENNA SCHWECHAT ;OS    0640 K K 14DEC1445 1635 14DEC;OK01;HK01;M ;0;100;;TYROLEAN AIRWAYS;1PC;;1400 ;ET;0250 ;N;979;UA;AT;  \r\nY-001;002DOK;DONETSK;VIE;VIENNA SCHWECHAT;VO;TYROLEAN AIRWAYS;;;;;100;N\r\nH-002;003XVIE;VIENNA SCHWECHAT ;CDG;PARIS CDG        ;OS    0417 K K 14DEC1715 1930 14DEC;OK01;HK01;S ;0;320;;;1PC;;1630 ;ET;0215 ;N;643;AT;FR;2D\r\nH-003;004OCDG;PARIS CDG        ;MUC;MUNICH           ;LH    2227 W W 17DEC0845 1015 17DEC;OK01;HK01;S ;0;CR9;;LUFTHANSA CITYLINE;1PC;1 ;0805 ;ET;0130 ; ;425;FR;DE;2 \r\nY-003;004CDG;PARIS CDG;MUC;MUNICH;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nH-004;005XMUC;MUNICH           ;DOK;DONETSK          ;LH    2542 W W 17DEC1100 1500 17DEC;OK01;HK01;S ;0;CR9;;LUFTHANSA CITYLINE;1PC;2 ;1020 ;ET;0300 ; ;1193;DE;UA;  \r\nY-004;005MUC;MUNICH;DOK;DONETSK;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nK-FUSD243.00     ;UAH1942       ;;;;;;;;;;;UAH3740       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH864      YQ AC; UAH42       UA SE; UAH183      ZY AE; UAH49       AT SE; UAH122      QX AP; UAH44       IZ EB; UAH45       FR SE; UAH138      FR TI; UAH175      RA EB;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH1662     XT ;\r\nL-\r\nM-KBUYOS1U       ;KBUYOS1U       ;WNC66W3        ;WNC66W3        \r\nN-NUC;79.50;;163.50\r\nO-14DEC14DEC;14DEC14DEC;17DEC17DEC;17DEC17DEC\r\nQ-DOK OS X/VIE OS PAR79.50LH X/MUC LH DOK163.50NUC243.00END ROE1.000000XT864YQ42UA183ZY49AT122QX44IZ45FR138FR175RA;FXP\r\nI-001;01ARTEMENKO/TETYANA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV OS  FQTV2-LH992221827087430;P1\r\nFQV LH  FQTV2-LH992221827087430;P1\r\nOSI OS  DS/EXP/UA23969\r\nOSI LH  DS/EXP/UA23969\r\nT-K257-3544667849\r\nFENONREF/NO CHNGS NONENDO;S2-5;P1\r\nFM*M*1A\r\nFPCASH\r\nFVOS;S2-5;P1\r\nTKOK02DEC/DOKC32530\r\nENDX\r\n	1	2011-12-02 16:46:05: Билет 257-3544667849 успешно импортирован\r\n	0	\N	\N	\N	\N
1201cc904b3e4151aff943311459c2ba	Air	2	SYSTEM	2011-12-02 17:01:47	SYSTEM	2011-12-02 17:01:47	AIR_20111202150034.10244.PDT	2011-12-02 17:00:00	AIR-BLK207;7A;;247;0200076630;1A1336371;001001\r\nAMD 0201325350;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A ZQSMFM005;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZQSMFM\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP\r\nC-7906/ 7777SVSU-7777SVSU-M-E--\r\nD-111202;111202;111202\r\nG-   ;;IEVDOK;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 Y Y 02DEC2130 2240 02DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD216.00     ;UAH1726       ;;;;;;;;;;;UAH2163       ;7.9899     ;;   \r\nKFTF; UAH353      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH353      HF ;UAH4        UD ;UAH80       XT ;\r\nL-\r\nM-YOW5     KK10  \r\nN-USD216.00\r\nO-XXXX\r\nQ-IEV VV DOK216.00USD216.00END XT48YQ20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01DAVYDOVA/NATALYA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667850\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 360.50\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 1726.00\r\nRIZTICKET TAX UAH 437.00\r\nRIZTICKET TTL UAH 2163.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2223.00\r\nENDX\r\n	1	2011-12-02 17:01:47: Билет 870-3544667850 успешно импортирован\r\n	0	\N	\N	\N	\N
bd5f06283d1345de980e484fec8062c1	Air	2	SYSTEM	2011-12-03 14:31:57	SYSTEM	2011-12-03 14:31:58	AIR_20111203123053.13086.PDT	2011-12-03 14:30:00	AIR-BLK207;7A;;247;0300050729;1A1336371;001001\r\nAMD 0301325813;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A ZX7L8L001;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZX7L8L\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111203;111203;111203\r\nG-   ;;DOKIEV;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 L L 03DEC1950 2100 03DEC;OK01;HK01;N ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nK-FUSD125.00     ;UAH999        ;;;;;;;;;;;UAH1320       ;7.9899     ;;   \r\nKFTF; UAH212      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH212      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-LOW1M5         \r\nN-USD125.00\r\nO-03DEC03DEC\r\nQ-DOK VV IEV125.00USD125.00END XT48YQ36YK21UA;FXB\r\nI-001;01KATKOVSKI/SAM MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667857\r\nFE.-44877533.-/NONENDO/REBOOK USD20 /VAT UAH 220.00;S2;P1\r\nFM*M*1A\r\nFPCASH\r\nFVVV;S2;P1\r\nTKOK03DEC/DOKC32530\r\nRIZTICKET FARE UAH 999.00\r\nRIZTICKET TAX UAH 321.00\r\nRIZTICKET TTL UAH 1320.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1380.00\r\nENDX\r\n	1	2011-12-03 14:31:58: Билет 870-3544667857 успешно импортирован\r\n	0	\N	\N	\N	\N
ceefe26198c74e059bc2c9008ee8ad3b	Air	2	SYSTEM	2011-12-02 15:31:52	SYSTEM	2011-12-02 15:31:52	AIR_20111202133052.09136.PDT	2011-12-02 15:30:00	AIR-BLK207;7A;;247;0200038466;1A1336371;001001\r\nAMD 0201325101;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZKVJQI008;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZKVJQI\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-2222MVSU-M-E--\r\nD-111202;111202;111202\r\nG-   ;;IEVIEV;UA\r\nH-003;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0095 K K 05DEC1345 1500 05DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nH-004;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 Q Q 09DEC1950 2100 09DEC;OK01;HK01;N ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nK-FUSD160.00     ;UAH1279       ;;;;;;;;;;;UAH1747       ;7.9899     ;;   \r\nKFTF; UAH275      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH275      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-KSS1M    KK6   ;QPX1M5   KK6   \r\nN-USD70.50;89.30\r\nO-05DEC05DEC;09DEC09DEC\r\nQ-IEV VV DOK70.50VV IEV89.30USD159.80END XT96YQ56YK33UA;FXP/ZO-6P*KK6\r\nI-001;01TSARIK/INNA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667846\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGREEMENT 01-851 /VAT UAH 291.17\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 1279.00\r\nRIZTICKET TAX UAH 468.00\r\nRIZTICKET TTL UAH 1747.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1807.00\r\nENDX\r\n	1	2011-12-02 15:31:52: Билет 870-3544667846 успешно импортирован\r\n	0	\N	\N	\N	\N
1e213db93dc7406ea5ef968834f82253	Air	2	SYSTEM	2011-12-02 15:37:08	SYSTEM	2011-12-02 15:37:08	AIR_20111202133551.09192.PDT	2011-12-02 15:35:00	AIR-BLK207;7A;;247;0200033664;1A1336371;001001\r\nAMD 0201325111;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A ZCIR6N027;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;YQ 3VKWM5\r\nA-HAHN AIR;HR 1691\r\nB-TTP/ITR-EML-SHABALDAKTM@DTEK.COM\r\nC-7906/ 7777SVSU-0001AASU-I-0--\r\nD-111201;111202;111202\r\nG-   ;;EGOMOW;RU\r\nH-006;002OEGO;BELGOROD         ;DME;MOSCOW DME       ;YQ    0774 Y Y 04DEC1145 1315 04DEC;OK01;HK01;;0;S20;;;20K;;;ET;0130 ; ;334;RU;RU;  \r\nK-FRUB4100       ;UAH1062       ;;;;;;;;;;;UAH1192       ;0.259      ;;   \r\nKFTF; UAH104      YQ AC; UAH26       YR VB;;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH104      YQ ;UAH26       YR ;;\r\nL-*HR *\r\nM-LAPEX5         \r\nN-RUB4100.00\r\nO-XXXX\r\nQ-EGO YQ MOW4100.00RUB4100.00END;FXB/R,VC-HR\r\nI-001;01SMIRNOV/ANDREY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K169-3544667847\r\nFM*M*0\r\nFPCASH\r\nFVHR;S2;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 1062.00\r\nRIZTICKET TAX UAH 130.00\r\nRIZTICKET TTL UAH 1192.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 1312.00\r\nENDX\r\n	1	2011-12-02 15:37:08: Билет 169-3544667847 успешно импортирован\r\n	0	\N	\N	\N	\N
db318f397f87461cbeb463de7e43c9a8	Air	2	SYSTEM	2011-12-02 15:41:40	SYSTEM	2011-12-02 15:41:41	AIR_20111202134023.09234.PDT	2011-12-02 15:40:00	AIR-BLK207;7A;;239;0200037005;1A1336371;001001\r\nAMD 0201325125;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZOOXD7009;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R2JF3 \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 0001AASU-2222MVSU-M-E--\r\nD-111202;111202;111202\r\nG-   ;;IEVDOK;UA\r\nH-002;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 H H 04DEC1950 2100 04DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD150.00     ;UAH1199       ;;;;;;;;;;;UAH1555       ;7.9899     ;;   \r\nKFTF; UAH248      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH56       YR VA; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH248      HF ;UAH4        UD ;UAH104      XT ;\r\nL-\r\nM-HOWDOM   KK10  \r\nN-USD150.30\r\nO-04DEC04DEC\r\nQ-IEV PS DOK150.30USD150.30END XT16YQ56YR20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01SYRY/IGOR MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K566-3544667848\r\nFENON END/REF REST/RBK 10USD;S2;P1\r\nFM*M*1\r\nFPCASH\r\nFTKK4715/11\r\nFVPS;S2;P1\r\nTKOK02DEC/DOKC32530\r\nRM PISHEM SEYCHAS///SCHET NA TSULINU!!!!!\r\nRIZTICKET FARE UAH 1199.00\r\nRIZTICKET TAX UAH 356.00\r\nRIZTICKET TTL UAH 1555.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1615.00\r\nENDX\r\n	1	2011-12-02 15:41:41: Билет 566-3544667848 успешно импортирован\r\n	0	\N	\N	\N	\N
645f867d5b8e4fc08c11c5fb62470cc2	Air	2	SYSTEM	2011-12-02 17:17:10	SYSTEM	2011-12-02 17:17:10	AIR_20111202151603.10453.PDT	2011-12-02 17:16:00	AIR-BLK207;7A;;247;0200051002;1A1336371;001001\r\nAMD 0201325398;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZMIDKN009;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZMIDKN\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 0001AASU-2222MVSU-M-E--\r\nD-111202;111202;111202\r\nG-   ;;IEVDOK;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0091 D D 03DEC0910 1025 03DEC;OK01;HK01;S ;0;735;;;30K;B ;;ET;0115 ;N;347;UA;UA;  \r\nK-FUSD243.00     ;UAH1942       ;;;;;;;;;;;UAH2422       ;7.9899     ;;   \r\nKFTF; UAH396      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH396      HF ;UAH4        UD ;UAH80       XT ;\r\nL-\r\nM-DOW6M5   KK10  \r\nN-USD243.00\r\nO-03DEC03DEC\r\nQ-IEV VV DOK243.00USD243.00END XT48YQ20YK12UA;FXP/ZO-10P*KK10\r\nI-001;01RYZHENKOV/YURIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870000434773;S2;P1\r\nT-K870-3544667851\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 403.67\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 1942.00\r\nRIZTICKET TAX UAH 480.00\r\nRIZTICKET TTL UAH 2422.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2482.00\r\nENDX\r\n	1	2011-12-02 17:17:10: Билет 870-3544667851 успешно импортирован\r\n	0	\N	\N	\N	\N
c247942867334b7387096f277f277666	Air	2	SYSTEM	2011-12-02 17:35:20	SYSTEM	2011-12-02 17:35:20	AIR_20111202153417.10682.PDT	2011-12-02 17:34:00	AIR-BLK207;7M;;247;0200054277;1A1336371;001001\r\nAMD 0201325439;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Y8HTS3016;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Y8HTS3\r\nB-TTM\r\nC-7906/ 7777SVSU-2222MVSU----\r\nD-111201;111202;111202\r\nU-002X;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 L L 05DEC2130 2240 05DEC;OK01;HK01;N ;0;320;;;;B ;;ET;0110 ;N;;347;;UA;UA;  \r\nMCO123;003VV;8702;AEROSVIT AIRLINES;KBP;TO-AEROSVIT AIRLINES;AT-KIEV;05DEC;M;A;PENALTY FOR CHANGE;**-;PENALTY FOR CHANGE;F;UAH        160;N;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;UAH        160;;;870-3544667766;;;;UAH        160;;;; ;P1\r\nI-001;01KOSTROMOV/OLEKSIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nTMCM870-9074146870;;L3\r\nMFM*M*0;L3\r\nMFPCASH;L3\r\nTKOK01DEC/DOKC32530//ETVV\r\nRIZTICKET FARE UAH 903.00\r\nRIZTICKET TAX UAH 271.00\r\nRIZTICKET TTL UAH 1174.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1234.00\r\nENDX\r\n	1	2011-12-02 17:35:20: MCO 870-9074146870 успешно импортирован\r\n	0	\N	\N	\N	\N
c06971f0e8814923a0882ea2588b4227	Air	2	SYSTEM	2011-12-02 17:41:18	SYSTEM	2011-12-02 17:41:18	AIR_20111202154005.10740.PDT	2011-12-02 17:40:00	AIR-BLK207;7A;;247;0200047569;1A1336371;001001\r\nAMD 0201325450;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZKUQSV011;0202;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R5WFR \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111202;111202;111202\r\nG-   ;;DOKDOK;UA\r\nH-004;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    0048 N N 06DEC0720 0830 06DEC;OK02;HK02;S ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nH-005;004OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 N N 06DEC1950 2100 06DEC;OK02;HK02;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD145.00     ;UAH1159       ;;;;;;;;;;;UAH1653       ;7.9899     ;;   \r\nKFTF; UAH253      HF GO; UAH8        UD DP; UAH32       YQ AD; UAH112      YR VA; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH253      HF ;UAH8        UD ;UAH233      XT ;\r\nL-\r\nM-NFLYDOM        ;NFLYDOM        \r\nN-USD72.50;72.50\r\nO-06DEC06DEC;06DEC06DEC\r\nQ-DOK PS IEV72.50PS DOK72.50USD145.00END XT32YQ112YR56YK33UA;FXB\r\nI-002;01NIKOLAEV/OLEKSANDR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667852\r\nFENON END/NO REF/RBK 20USD;S3-4;P1-2\r\nFM*M*1\r\nFPCASH\r\nFVPS;S3-4;P1-2\r\nTKOK02DEC/DOKC32530\r\nI-001;02YAROSLAVTSEVA/OLENA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667853\r\nFENON END/NO REF/RBK 20USD;S3-4;P1-2\r\nFM*M*1\r\nFPCASH\r\nFVPS;S3-4;P1-2\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 1159.00\r\nRIZTICKET TAX UAH 494.00\r\nRIZTICKET TTL UAH 1653.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1713.00\r\nENDX\r\n	1	2011-12-02 17:41:18: Билет 566-3544667852 успешно импортирован\r\nБилет 566-3544667853 успешно импортирован\r\n	0	\N	\N	\N	\N
03e223430588482fbfa318cec9193270	Air	2	SYSTEM	2011-12-02 17:44:08	SYSTEM	2011-12-02 17:44:08	AIR_20111202154305.10769.PDT	2011-12-02 17:43:00	AIR-BLK207;MA;;225;0200050751;1A1336371;001001\r\nAMD 0201325456;1/1;VOID02DEC;MVSU\r\nGW3670821;1A1336371\r\nMUC1A ZBZOVT012;0501;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZBZOVT\r\nI-005;01BOZHKO/RUSLAN MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667806\r\nFPCASH\r\nENDX\r\n	1	2011-12-02 17:44:08: Void 870-3544667806 успешно импортирован\r\n	0	\N	\N	\N	\N
d374c2b3deab4db68c77d2657179e4cd	Air	2	SYSTEM	2011-12-03 12:01:19	SYSTEM	2011-12-03 12:01:20	AIR_20111203100018.12426.PDT	2011-12-03 12:00:00	AIR-BLK207;7A;;247;0300032797;1A1336371;001001\r\nAMD 0301325704;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A ZJ6BKF011;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LO ZJ6BKF\r\nA-LOT POLISH AIRLINES;LO 0803\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-2222MVSU-I-0--\r\nD-111202;111203;111203\r\nG-X  ;;DOKDOK;E1\r\nH-003;002ODOK;DONETSK          ;WAW;WARSAW           ;LO    0758 O O 13DEC0435 0605 13DEC;OK01;HK01;S ;0;E75;;;20K;;;ET;0230 ;N;795;UA;PL;A \r\nH-004;003OWAW;WARSAW           ;DOK;DONETSK          ;LO    0757 U U 14DEC2230 0150 15DEC;OK01;HK01;S ;0;E75;;;20K;A ;;ET;0220 ;N;795;PL;UA;  \r\nK-FUSD86.00      ;UAH688        ;;;;;;;;;;;UAH1073       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH64       YQ AD; UAH42       UA SE; UAH2        ND AD; UAH141      XW AE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH249      XT ;\r\nL-\r\nM-OFMLO          ;UFMLO          \r\nN-NUC36.00;49.50\r\nO-13DEC13DEC;14DEC14DEC\r\nQ-DOK LO WAW36.00LO DOK49.50NUC85.50END ROE1.000000XT64YQ42UA2ND141XW;FXB\r\nI-001;01GOROVYY/SEMEN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /5024 ROBOT AUTO-PROCESS 03DEC - SET 12/02/11 0435\r\nSSR OTHS 1A  /PLS ISSUE TKT BY 2200/03DEC OR ITIN WILL BE CXLD\r\nT-K080-3544667854\r\nFENONREF/NOCHNG LO ONLY;S2-3;P1\r\nFM*M*4\r\nFPCASH\r\nFVLO;S2-3;P1\r\nTKOK02DEC/DOKC32530\r\nOPLDOKC32530/03DEC/3C0\r\nRIZTICKET FARE UAH 688.00\r\nRIZTICKET TAX UAH 385.00\r\nRIZTICKET TTL UAH 1073.00\r\nRIZSERVICE FEE UAH 250.00+VAT 50.00\r\nRIZGRAND TOTAL UAH 1373.00\r\nENDX\r\n	1	2011-12-03 12:01:20: Билет 080-3544667854 успешно импортирован\r\n	0	\N	\N	\N	\N
7cf97385618a49cb98722f271776e87c	Air	2	SYSTEM	2011-12-03 13:12:37	SYSTEM	2011-12-03 13:12:37	AIR_20111203111133.12760.PDT	2011-12-03 13:11:00	AIR-BLK207;7A;;247;0300018143;1A1336371;001001\r\nAMD 0301325756;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A ZXLV67002;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZXLV67\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111203;111203;111203\r\nG-X  ;;DOKMOW;E1\r\nH-002;002ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0305 P P 04DEC1805 2155 04DEC;OK01;HK01;L ;0;735;;;20K;;;ET;0150 ;N;545;UA;RU;C \r\nK-FUSD75.00      ;UAH600        ;;;;;;;;;;;UAH778        ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-P7OW           \r\nN-NUC75.00\r\nO-04DEC04DEC\r\nQ-DOK VV MOW75.00NUC75.00END ROE1.000000;FXB\r\nI-001;01GRIGORYAN/ARTAK MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667855\r\nFE.-44877533.-/NONENDO/REFRESTR/NONREBOOK;S2;P1\r\nFM*M*5\r\nFPCASH\r\nFVVV;S2;P1\r\nTKOK03DEC/DOKC32530\r\nRIZTICKET FARE UAH 600.00\r\nRIZTICKET TAX UAH 178.00\r\nRIZTICKET TTL UAH 778.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 898.00\r\nENDX\r\n	1	2011-12-03 13:12:37: Билет 870-3544667855 успешно импортирован\r\n	0	\N	\N	\N	\N
7ebea972b0e74309b2881634b5c8288d	Air	2	SYSTEM	2011-12-03 13:14:23	SYSTEM	2011-12-03 13:14:23	AIR_20111203111314.12765.PDT	2011-12-03 13:13:00	AIR-BLK207;7A;;247;0300043340;1A1336371;001001\r\nAMD 0301325757;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A ZO9L8Z010;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;OK Y573Y \r\nA-CZECH AIRLINES CSA;OK 0641\r\nB-TTP/RT\r\nC-7906/ 0001AASU-2222MVSU-I-0--\r\nD-111202;111203;111203\r\nG-X  ;;DOKDOK;E1\r\nH-003;002ODOK;DONETSK          ;PRG;PRAGUE           ;OK    0921 N N 21DEC0425 0615 21DEC;OK01;HK01;M ;0;735;;;1PC;;;ET;0250 ;N;1071;UA;CZ;1 \r\nH-004;003XPRG;PRAGUE           ;FRA;FRANKFURT        ;OK    0534 N N 21DEC0720 0835 21DEC;OK01;HK01;S ;0;319;;;1PC;2 ;;ET;0115 ;N;253;CZ;DE;2 \r\nH-001;004OFRA;FRANKFURT        ;PRG;PRAGUE           ;OK    0537 N N 25DEC1905 2015 25DEC;OK01;HK01;S ;0;319;;;1PC;2 ;;ET;0110 ;N;253;DE;CZ;2 \r\nH-002;005XPRG;PRAGUE           ;DOK;DONETSK          ;OK    0920 N N 25DEC2200 0140 26DEC;OK01;HK01;M ;0;735;;;1PC;1 ;;ET;0240 ;N;1071;CZ;UA;  \r\nK-FUSD150.00     ;UAH1199       ;;;;;;;;;;;UAH2679       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH736      YQ AC; UAH42       UA SE; UAH158      CZ EB; UAH87       OY CB; UAH251      RA EB; UAH70       DE SE;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH1344     XT ;\r\nL-\r\nM-NSPCUA         ;NSPCUA         ;NSPCUA         ;NSPCUA         \r\nN-NUC;75.00;;75.00\r\nO-21DEC21DEC;21DEC21DEC;25DEC25DEC;25DEC25DEC\r\nQ-DOK OK X/PRG OK FRA75.00OK X/PRG OK DOK75.00NUC150.00END ROE1.000000XT736YQ42UA158CZ87OY251RA70DE;FXP\r\nI-001;01NIKULINA/LYUDMYLA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K064-3544667856\r\nFEFARE RESTRICTIONS APPLY;S2-5;P1\r\nFM*M*1\r\nFPCASH\r\nFVOK;S2-5;P1\r\nTKOK02DEC/DOKC32530\r\nRIZTICKET FARE UAH 1199.00\r\nRIZTICKET TAX UAH 1480.00\r\nRIZTICKET TTL UAH 2679.00\r\nRIZSERVICE FEE UAH 250.00+VAT 50.00\r\nRIZGRAND TOTAL UAH 2979.00\r\nENDX\r\n	1	2011-12-03 13:14:23: Билет 064-3544667856 успешно импортирован\r\n	0	\N	\N	\N	\N
33a1351168384eb8a3fced0259d757c6	Air	2	SYSTEM	2011-12-03 15:04:05	SYSTEM	2011-12-03 15:04:06	AIR_20111203130246.13201.PDT	2011-12-03 15:02:00	AIR-BLK207;7A;;247;0300053018;1A1336371;001001\r\nAMD 0301325831;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A YIGPLK010;0302;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YIGPLK\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/T6/RT\r\nC-7906/ 7777SVSU-2222MVSU-I-0--\r\nD-111129;111203;111203\r\nG-X  ;;DOKDOK;\r\nH-001;004ODOK;DONETSK          ;TLV;TEL AVIV B GURION;VV    0249 K K 11DEC1120 1435 11DEC;OK02;HK02;L ;0;735;;;20K;;;ET;0315 ;N;1121;UA;IL;3 \r\nH-002;005OTLV;TEL AVIV B GURION;KBP;KIEV BORYSPIL    ;VV    0238 H H 15DEC0815 1140 15DEC;OK02;HK02;L ;0;321;;;20K;3 ;;ET;0325 ;N;1284;IL;UA;B \r\nH-003;006XKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0095 H H 15DEC1345 1500 15DEC;OK02;HK02;N ;0;733;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nK-FUSD533.00     ;UAH4259       ;;;;;;;;;;;UAH4720       ;7.9899     ;;   \r\nKFTF; UAH140      YK AE; UAH20       UD DP; UAH48       YQ AC; UAH54       UA SE; UAH199      IL EB;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH140      YK ;UAH20       UD ;UAH301      XT ;\r\nL-\r\nM-KPX1Y5         ;HPX1Y5         ;HPX1Y5         \r\nN-NUC250.00;;172.50\r\nO-11DEC11DEC;15DEC15DEC;15DEC15DEC\r\nQ-DOK VV TLV Q110.00 140.00VV X/IEV Q110.00VV DOK172.50NUC532.50END ROE1.000000XT48YQ54UA199IL;FXB\r\nI-001;01MORGUNOV/YURIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667859\r\nFENONENDO/REF/REBOOK RESTR;S4-6;P1,3\r\nFM*M*1\r\nFPCASH\r\nFVVV;S4-6;P1,3\r\nTKOK29NOV/DOKC32530\r\nI-003;03MORGUNOVA/MARYNA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667860\r\nFENONENDO/REF/REBOOK RESTR;S4-6;P1,3\r\nFM*M*1\r\nFPCASH\r\nFVVV;S4-6;P1,3\r\nTKOK29NOV/DOKC32530\r\nRIZTICKET FARE UAH 4259.00\r\nRIZTICKET TAX UAH 461.00\r\nRIZTICKET TTL UAH 4720.00\r\nRIZSERVICE FEE UAH 200.00+VAT 40.00\r\nRIZGRAND TOTAL UAH 4960.00\r\nENDX\r\n	1	2011-12-03 15:04:06: Билет 870-3544667859 успешно импортирован\r\nБилет 870-3544667860 успешно импортирован\r\n	0	\N	\N	\N	\N
1e1b2a45033b4112a60a9412df42edd1	Air	2	SYSTEM	2011-12-03 15:04:49	SYSTEM	2011-12-03 15:04:49	AIR_20111203130350.13206.PDT	2011-12-03 15:03:00	AIR-BLK207;7A;;247;0300053120;1A1336371;001001\r\nAMD 0301325835;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A YIGPLK013;0301;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV YIGPLK\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/T5/RT\r\nC-7906/ 7777SVSU-2222MVSU-I-0--\r\nD-111129;111203;111203\r\nG-X  ;;DOKDOK;\r\nH-001;004ODOK;DONETSK          ;TLV;TEL AVIV B GURION;VV    0249 K K 11DEC1120 1435 11DEC;OK01;HK01;L ;0;735;;;20K;;;ET;0315 ;N;1121;UA;IL;3 \r\nH-002;005OTLV;TEL AVIV B GURION;KBP;KIEV BORYSPIL    ;VV    0238 H H 15DEC0815 1140 15DEC;OK01;HK01;L ;0;321;;;20K;3 ;;ET;0325 ;N;1284;IL;UA;B \r\nH-003;006XKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0095 H H 15DEC1345 1500 15DEC;OK01;HK01;N ;0;733;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nK-FUSD454.00     ;UAH3628       ;;;;;;;;;;;UAH4089       ;7.9899     ;;   \r\nKFTF; UAH140      YK AE; UAH20       UD DP; UAH48       YQ AC; UAH54       UA SE; UAH199      IL EB;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH140      YK ;UAH20       UD ;UAH301      XT ;\r\nL-\r\nM-KPX1Y5   CH25  ;HPX1Y5   CH25  ;HPX1Y5   CH25  \r\nN-NUC215.00;;129.37\r\nO-11DEC11DEC;15DEC15DEC;15DEC15DEC\r\nQ-DOK VV TLV Q110.00 105.00VV X/IEV Q110.00VV DOK129.37NUC454.37END ROE1.000000XT48YQ54UA199IL;FXB\r\nI-002;02MORGUNOV/ARTEM MSTR(CHD);;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR CHLD VV  HK1/19AUG09;P2\r\nT-K870-3544667861\r\nFENONENDO/REF/REBOOK RESTR;S4-6;P2\r\nFM*M*1\r\nFPCASH\r\nFVVV;S4-6;P2\r\nTKOK29NOV/DOKC32530\r\nRIZTICKET FARE UAH 3628.00\r\nRIZTICKET TAX UAH 461.00\r\nRIZTICKET TTL UAH 4089.00\r\nRIZSERVICE FEE UAH 200.00+VAT 40.00\r\nRIZGRAND TOTAL UAH 4329.00\r\nENDX\r\n	1	2011-12-03 15:04:49: Билет 870-3544667861 успешно импортирован\r\n	0	\N	\N	\N	\N
2e61100a27da415d82c2716e528bbe70	Air	2	SYSTEM	2011-12-03 15:33:22	SYSTEM	2011-12-03 15:33:22	AIR_20111203133212.13285.PDT	2011-12-03 15:32:00	AIR-BLK207;7A;;247;0300054666;1A1336371;001001\r\nAMD 0301325858;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A ZWZAMV007;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZWZAMV\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-M-E--\r\nD-111203;111203;111203\r\nG-   ;;DOKDOK;UA\r\nH-003;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 K K 16DEC1700 1820 16DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nH-004;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 K K 18DEC2130 2240 18DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD141.00     ;UAH1127       ;;;;;;;;;;;UAH1565       ;7.9899     ;;   \r\nKFTF; UAH245      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH245      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-KSS1M    KK6   ;KSS1M    KK6   \r\nN-USD70.50;70.50\r\nO-16DEC16DEC;18DEC18DEC\r\nQ-DOK VV IEV70.50VV DOK70.50USD141.00END XT96YQ56YK33UA;FXP/ZO-6P*KK6\r\nI-001;01KAVERINA/YULIYA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667862\r\nFE.-44877533.-/VVONLY/NON ENDO KK6AGREEMENT 01-801 /VAT UAH 260.83;S2-3;P1\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-801;S2-3;P1\r\nFVVV;S2-3;P1\r\nTKOK03DEC/DOKC32530\r\nRIZTICKET FARE UAH 1127.00\r\nRIZTICKET TAX UAH 438.00\r\nRIZTICKET TTL UAH 1565.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1625.00\r\nENDX\r\n	1	2011-12-03 15:33:22: Билет 870-3544667862 успешно импортирован\r\n	0	\N	\N	\N	\N
df9c779a75fe4736bcdb05e1ccd217d2	Air	2	SYSTEM	2011-12-05 11:53:48	SYSTEM	2011-12-05 11:53:48	AIR_20111205095338.16664.PDT	2011-12-05 11:53:00	AIR-BLK207;7M;;247;0500030276;1A1336371;001001\r\nAMD 0501326321;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A Z6G527008;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Z6G527\r\nB-TTM\r\nC-7906/ 7777SVSU-7777SVSU-M---\r\nD-111204;111205;111205\r\nU-001X;002ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0305 K K 04DEC1805 2155 04DEC;OK01;HK01;L ;0;735;;;;;;ET;0150 ;N;;545;;UA;RU;C \r\nMCO157;003VV;8702;AEROSVIT AIRLINES;DOK;TO-AEROSVIT AIRLINES;AT-DONETSK;04DEC;M;A;REFUND CHARGE  870 3544667778;**-;;F;UAH       1512;N;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;UAH       1512;;;;;;;UAH       1512;;;; ;P1\r\nI-002;01RYPALOV/VOLODYMYR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nTMCM870-9074146872;;L3\r\nMFM*M*0;L3\r\nMFPCASH;L3\r\nTKOK01DEC/DOKC32530//ETVV\r\nRIZTICKET FARE UAH 1574.00\r\nRIZTICKET TAX UAH 178.00\r\nRIZTICKET TTL UAH 1752.00\r\nRIZSERVICE FEE UAH 80.00+VAT 16.00\r\nRIZGRAND TOTAL UAH 1848.00\r\nENDX\r\n	1	2011-12-05 11:53:48: MCO 870-9074146872 успешно импортирован\r\n	0	\N	\N	\N	\N
cf2d8e7a40f547469adfe9ec67f32732	Air	2	SYSTEM	2011-12-05 12:00:49	SYSTEM	2011-12-05 12:00:50	AIR_20111205100045.16758.PDT	2011-12-05 12:00:00	AIR-BLK207;7A;;247;0500025077;1A1336371;001001\r\nAMD 0501326341;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 2ATBVF005;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS RJ2BL \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111205;111205;111205\r\nG-   ;;DOKDOK;UA\r\nH-005;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    0048 V V 08DEC0720 0830 08DEC;OK01;HK01;S ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nH-004;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 W W 08DEC1950 2100 08DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD270.00     ;UAH2158       ;;;;;;;;;;;UAH2882       ;7.9899     ;;   \r\nKFTF; UAH451      HF GO; UAH8        UD DP; UAH32       YQ AD; UAH144      YR VA; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH451      HF ;UAH8        UD ;UAH265      XT ;\r\nL-\r\nM-VFLYDOM        ;WFLYDOM        \r\nN-USD112.50;157.50\r\nO-08DEC08DEC;08DEC08DEC\r\nQ-DOK PS IEV112.50PS DOK157.50USD270.00END XT32YQ144YR56YK33UA;FXP\r\nI-001;01LUKASHENKO/VALERIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667870\r\nFENON END/REF REST/RBK 10USD;S2-3;P1\r\nFM*M*1\r\nFPCASH\r\nFVPS;S2-3;P1\r\nTKOK05DEC/DOKC32530\r\nRIZTICKET FARE UAH 2158.00\r\nRIZTICKET TAX UAH 724.00\r\nRIZTICKET TTL UAH 2882.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2942.00\r\nENDX\r\n	1	2011-12-05 12:00:50: Билет 566-3544667870 успешно импортирован\r\n	0	\N	\N	\N	\N
4211cf63ee7a4d469b9a2b558540919d	Air	2	SYSTEM	2011-12-03 16:11:39	SYSTEM	2011-12-03 16:11:40	AIR_20111203141020.13361.PDT	2011-12-03 16:10:00	AIR-BLK207;7A;;247;0300056678;1A1336371;001001\r\nAMD 0301325876;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A ZYRGEH001;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS RDDJQ \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111203;111203;111203\r\nG-   ;;DNKIEV;UA\r\nH-001;002ODNK;DNIPROPETROVSK   ;KBP;KIEV BORYSPIL    ;PS    0072 Q Q 05DEC0750 0850 05DEC;OK01;HK01;M ;0;735;;;20K;;;ET;0100 ;N;226;UA;UA;B \r\nK-FUSD108.00     ;UAH863        ;;;;;;;;;;;UAH1175       ;7.9899     ;;   \r\nKFTF; UAH184      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH56       YR VA; UAH40       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH184      HF ;UAH4        UD ;UAH124      XT ;\r\nL-\r\nM-QOWDOM         \r\nN-USD108.00\r\nO-05DEC05DEC\r\nQ-DNK PS IEV108.00USD108.00END XT16YQ56YR40YK12UA;FXB\r\nI-001;01GIVEL/YAROSLAV MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667863\r\nFENON END/NO REF/RBK 20USD /VAT UAH 195.83;S2;P1\r\nFM*M*1\r\nFPCASH\r\nFVPS;S2;P1\r\nTKOK03DEC/DOKC32530\r\nRIZTICKET FARE UAH 863.00\r\nRIZTICKET TAX UAH 312.00\r\nRIZTICKET TTL UAH 1175.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1235.00\r\nENDX\r\n	1	2011-12-03 16:11:40: Билет 566-3544667863 успешно импортирован\r\n	0	\N	\N	\N	\N
3fe3a27153824ff1b62c396bc9f75a67	Air	2	SYSTEM	2011-12-05 09:56:22	SYSTEM	2011-12-05 09:56:24	AIR_20111205071927.15143.PDT	2011-12-05 09:19:00	AIR-BLK207;7A;;247;0500008914;1A1336371;001001\r\nAMD 0501326039;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZXKFCB014;0202;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;UN QC95W \r\nA-TRANSAERO AIRLINES;UN 6705\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-0001AASU-I-0--\r\nD-111203;111205;111205\r\nG-X  ;;TSEDOK;\r\nH-006;003OTSE;ASTANA           ;DME;MOSCOW DME       ;UN    0204 I I 06DEC0625 0800 06DEC;OK02;HK02;D ;0;738;;;20K;;;ET;0335 ;N;1406;KZ;RU;  \r\nH-007;004ODME;MOSCOW DME       ;DOK;DONETSK          ;UN    0243 W W 06DEC1045 1025 06DEC;OK02;HK02;S ;0;735;;;20K;;;ET;0140 ;N;506;RU;UA;  \r\nK-FKZT46777      ;UAH2533       ;;;;;;;;;;;UAH3320       ;0.00678223 ;7.9899     ;USD\r\nKFTF; UAH346      YQ AC; UAH324      YR VB; UAH117      UJ AP;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH346      YQ ;UAH324      YR ;UAH117      UJ ;\r\nL-\r\nM-IPROW          ;WPROW          \r\nN-NUC;;;17\r\nO-06DEC06DEC;06DEC06DEC\r\nQ-TSE UN(FE)MOW220.00UN DOK98.17NUC318.17END ROE147.017000;FXB\r\nI-001;01NAZHMETDINOV/DASTAN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K670-3544667864\r\nFE-UN ONLY-;S3-4;P1-2\r\nFM*M*5\r\nFPCASH\r\nFVUN;S3-4;P1-2\r\nTKOK03DEC/DOKC32530\r\nI-002;02SEK/RUSLAN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K670-3544667865\r\nFE-UN ONLY-;S3-4;P1-2\r\nFM*M*5\r\nFPCASH\r\nFVUN;S3-4;P1-2\r\nTKOK03DEC/DOKC32530\r\nRIZTICKET FARE UAH 2533.00\r\nRIZTICKET TAX UAH 787.00\r\nRIZTICKET TTL UAH 3320.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 3440.00\r\nENDX\r\n	1	2011-12-05 09:56:24: Билет 670-3544667864 успешно импортирован\r\nБилет 670-3544667865 успешно импортирован\r\n	0	\N	\N	\N	\N
4a8d0a75dc144b21842bb84435109b30	Air	2	SYSTEM	2011-12-05 09:56:24	SYSTEM	2011-12-05 09:56:24	AIR_20111205073837.15264.PDT	2011-12-05 09:38:00	AIR-BLK207;7A;;247;0500010285;1A1336371;001001\r\nAMD 0501326053;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZWY3YM013;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZWY3YM\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-7777SVSU-M-E--\r\nD-111203;111205;111205\r\nG-   ;;DOKDOK;UA\r\nH-003;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 K K 05DEC1950 2100 05DEC;OK01;HK01;N ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nH-004;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0093 K K 08DEC0700 0820 08DEC;OK01;HK01;N ;0;733;;;20K;B ;;ET;0120 ;N;347;UA;UA;  \r\nK-FUSD141.00     ;UAH1127       ;;;;;;;;;;;UAH1565       ;7.9899     ;;   \r\nKFTF; UAH245      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH245      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-KSS1M    KK6   ;KSS1M    KK6   \r\nN-USD70.50;70.50\r\nO-05DEC05DEC;08DEC08DEC\r\nQ-DOK VV IEV70.50VV DOK70.50USD141.00END XT96YQ56YK33UA;FXP/ZO-6P*KK6\r\nI-001;01VERBITSKA/IRYNA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667866\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGREEMENT 01-851 /VAT UAH 260.83\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK04DEC/IEVVV0009\r\nRIZTICKET FARE UAH 1127.00\r\nRIZTICKET TAX UAH 438.00\r\nRIZTICKET TTL UAH 1565.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1625.00\r\nENDX\r\n	1	2011-12-05 09:56:24: Билет 870-3544667866 успешно импортирован\r\n	0	\N	\N	\N	\N
d7eb3c35cb9c414998cbdf21d2f242c4	Air	2	SYSTEM	2011-12-05 12:27:39	SYSTEM	2011-12-05 12:27:39	AIR_20111205102727.17044.PDT	2011-12-05 12:27:00	AIR-BLK207;7A;;247;0500035678;1A1336371;001001\r\nAMD 0501326391;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A Z9T4GO005;0303;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;SU PALIYP\r\nA-AEROFLOT;SU 5552\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111205;111205;111205\r\nG-X  ;;DOKDOK;E1\r\nH-001;004ODOK;DONETSK          ;SVO;MOSCOW SVO       ;SU    4139 Q Q 26DEC0740 1135 26DEC;OK03;HK03;L ;0;735;;;20K;;;ET;0155 ; ;545;UA;RU;C \r\nX-001;004ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0301 Q Q 26DEC0740 1135 26DEC;OK03;HK03;L ;0;735;;;20K;;;ET;0155 ; ;C \r\nH-002;005XSVO;MOSCOW SVO       ;LED;ST PETERSBURG    ;SU    0845 Q Q 26DEC1400 1530 26DEC;OK03;HK03;L ;0;320;;;20K;D ;;ET;0130 ; ;386;RU;RU;1 \r\nH-003;006OLED;ST PETERSBURG    ;SVO;MOSCOW SVO       ;SU    0852 Q Q 04JAN1925 2045 04JAN;OK03;HK03;L ;0;320;;;20K;1 ;;ET;0120 ; ;386;RU;RU;D \r\nH-004;007XSVO;MOSCOW SVO       ;DOK;DONETSK          ;SU    4140 Q Q 04JAN2245 2230 04JAN;OK03;HK03;L ;0;735;;;20K;C ;;ET;0145 ; ;545;RU;UA;  \r\nX-004;007XSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0306 Q Q 04JAN2245 2230 04JAN;OK03;HK03;L ;0;735;;;20K;C ;;ET;0145 ; ;  \r\nK-FUSD215.00     ;UAH1718       ;;;;;;;;;;;UAH2040       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH144      YR VB; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH186      XT ;\r\nL-\r\nM-QPX            ;QPX            ;QPX            ;QPX            \r\nN-NUC;107.50;;107.50\r\nO-26DEC26DEC;26DEC26DEC;04JAN04JAN;04JAN04JAN\r\nQ-DOK SU X/MOW SU LED107.50SU X/MOW SU DOK107.50NUC215.00END ROE1.000000XT144YR42UA;FXB\r\nI-001;01BEZKOROVAYNYY/MYKOLA MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K555-3544667871\r\nFEVALID ON SU/FARE RESTR APL;S4-7;P1-3\r\nFM*M*5\r\nFPCASH\r\nFVSU;S4-7;P1-3\r\nTKOK05DEC/DOKC32530\r\nI-002;02SHKODA/OLGA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR DOCS SU  HK1/P/UKR/BE111371/UKR/14JUL66/F/27JUN21/SHKODA/OLGA/H;P2\r\nT-K555-3544667872\r\nFEVALID ON SU/FARE RESTR APL;S4-7;P1-3\r\nFM*M*5\r\nFPCASH\r\nFVSU;S4-7;P1-3\r\nTKOK05DEC/DOKC32530\r\nI-003;03SHKODA/VLADA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K555-3544667873\r\nFEVALID ON SU/FARE RESTR APL;S4-7;P1-3\r\nFM*M*5\r\nFPCASH\r\nFVSU;S4-7;P1-3\r\nTKOK05DEC/DOKC32530\r\nRIZTICKET FARE UAH 1718.00\r\nRIZTICKET TAX UAH 322.00\r\nRIZTICKET TTL UAH 2040.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 2160.00\r\nENDX\r\n	1	2011-12-05 12:27:39: Билет 555-3544667871 успешно импортирован\r\nБилет 555-3544667872 успешно импортирован\r\nБилет 555-3544667873 успешно импортирован\r\n	0	\N	\N	\N	\N
acde7fcfe39447be9e6fdcd11ce146cc	Air	2	SYSTEM	2011-12-05 10:29:09	SYSTEM	2011-12-05 10:29:09	AIR_20111205082756.15726.PDT	2011-12-05 10:27:00	AIR-BLK207;7A;;249;0500014250;1A1336371;001001\r\nAMD 0501326137;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A ZM8QMJ029;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;LH ZM8QMJ;LO ZM8QMJ\r\nA-LUFTHANSA;LH 2203\r\nB-TTP/RT\r\nC-7906/ 0001AASU-7777SVSU-I-0--\r\nD-111202;111205;111205\r\nG-X  ;;DOKDOK;E1\r\nH-007;002ODOK;DONETSK          ;WAW;WARSAW           ;LO    0758 Z Z 06DEC0435 0605 06DEC;OK01;HK01;M ;0;E70;;;30K;;;ET;0230 ;N;795;UA;PL;A \r\nH-008;003XWAW;WARSAW           ;LHR;LONDON LHR       ;LO    0281 Z Z 06DEC0745 0935 06DEC;OK01;HK01;M ;0;734;;;30K;A ;;ET;0250 ;N;923;PL;GB;1 \r\nH-009;004OLHR;LONDON LHR       ;MUC;MUNICH           ;LH    2483 D D 12DEC0710 1005 12DEC;OK01;HK01;S ;0;321;;;2PC;1 ;0640 ;ET;0155 ; ;594;GB;DE;2 \r\nH-010;005XMUC;MUNICH           ;DOK;DONETSK          ;LH    2542 D D 12DEC1100 1500 12DEC;OK01;HK01;M ;0;CR9;;LUFTHANSA CITYLINE;2PC;2 ;1030 ;ET;0300 ; ;1193;DE;UA;  \r\nY-010;005MUC;MUNICH;DOK;DONETSK;CL;LUFTHANSA CITYLINE;;;;;CR9;N\r\nK-FUSD1162.00    ;UAH9285       ;;;;;;;;;;;UAH11596      ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH464      YQ AD; UAH778      YQ AC; UAH42       UA SE; UAH2        ND AD; UAH141      XW AE; UAH298      GB AD; UAH275      UB AS; UAH175      RA EB;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH2175     XT ;\r\nL-*LH *\r\nM-ZRT24          ;ZRT24          ;DFF65D1        ;DFF65D1        \r\nN-NUC;499.50;;662.50\r\nO-XXXX;XXXX;07DECXX;07DECXX\r\nQ-DOK LO X/WAW LO LON499.50LH X/MUC LH DOK662.50NUC1162.00END ROE1.000000XT464YQ778YQ42UA2ND141XW298GB275UB175RA;FXP/S2-5/R,VC-LH\r\nI-001;01SUMMERFIELD/DAVID MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nSSR OTHS 1A  /PLEASE INSERT TKT NUMBER ASAP\r\nOSI LH  DS/EXP/UA23969\r\nT-K220-3544667867\r\nFELO ONLY/NON ENDO;S2-5;P1\r\nFM*M*5\r\nFPCASH\r\nFTUA0299\r\nFVLH;S2-5;P1\r\nFZP\r\nTKOK02DEC/DOKC32530\r\nRM PISHEM SEYCHAS///BRDS///MVZ7010///\r\nENDX\r\n	1	2011-12-05 10:29:09: Билет 220-3544667867 успешно импортирован\r\n	0	\N	\N	\N	\N
dd08ca4681934e789428678abfc13831	Air	2	SYSTEM	2011-12-05 11:01:10	SYSTEM	2011-12-05 11:01:10	AIR_20111205090001.16078.PDT	2011-12-05 11:00:00	AIR-BLK207;7A;;247;0500018846;1A1336371;001001\r\nAMD 0501326219;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Z863UH009;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Z863UH\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111205;111205;111205\r\nG-X  ;;DOKTBS;E1\r\nH-003;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0092 B B 06DEC0920 1045 06DEC;OK01;HK01;N ;0;735;;;20K;;;ET;0125 ;N;347;UA;UA;B \r\nH-004;003XKBP;KIEV BORYSPIL    ;TBS;TBILISI          ;VV    0417 B B 06DEC1255 1730 06DEC;OK01;HK01;;0;E95;;;20K;B ;;ET;0235 ;N;901;UA;GE;  \r\nK-FUSD198.00     ;UAH1582       ;;;;;;;;;;;UAH2075       ;7.9899     ;;   \r\nKFTF; UAH172      YK AE; UAH20       UD DP; UAH248      YQ AC; UAH53       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH172      YK ;UAH20       UD ;UAH301      XT ;\r\nL-\r\nM-BOW6M5         ;BOW6M5         \r\nN-NUC;198.00\r\nO-06DEC06DEC;06DEC06DEC\r\nQ-DOK VV X/IEV VV TBS198.00NUC198.00END ROE1.000000XT248YQ53UA;FXB\r\nI-001;01GOGNADZE/KETEVAN MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667868\r\nFE.-44877533.-/NON ENDO/REBOOK RESTR;S2-3;P1\r\nFM*M*1\r\nFPCASH\r\nFVVV;S2-3;P1\r\nTKOK05DEC/DOKC32530\r\nRIZTICKET FARE UAH 1582.00\r\nRIZTICKET TAX UAH 493.00\r\nRIZTICKET TTL UAH 2075.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 2195.00\r\nENDX\r\n	1	2011-12-05 11:01:10: Билет 870-3544667868 успешно импортирован\r\n	0	\N	\N	\N	\N
8f3f10fc5b964c7abd43c6ecd2c24897	Air	2	SYSTEM	2011-12-05 11:11:37	SYSTEM	2011-12-05 11:11:37	AIR_20111205091030.16183.PDT	2011-12-05 11:10:00	AIR-BLK207;7A;;247;0500013200;1A1336371;001001\r\nAMD 0501326243;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A ZWZDQ6012;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV ZWZDQ6\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111203;111205;111205\r\nG-   ;;DOKDOK;UA\r\nH-002;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0092 C C 06DEC0920 1045 06DEC;OK01;HK01;S ;0;735;;;30K;;;ET;0125 ;N;347;UA;UA;B \r\nH-003;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0091 D D 08DEC0910 1025 08DEC;OK01;HK01;S ;0;735;;;30K;B ;;ET;0115 ;N;347;UA;UA;  \r\nK-FUSD590.00     ;UAH4714       ;;;;;;;;;;;UAH5870       ;7.9899     ;;   \r\nKFTF; UAH963      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH963      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-CRT5           ;DEE6M5         \r\nN-USD335.00;255.00\r\nO-06DEC06DEC;08DEC08DEC\r\nQ-DOK VV IEV335.00VV DOK255.00USD590.00END XT96YQ56YK33UA;FXP\r\nI-001;01EPEL/MICHAEL MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV87000038589;S2;P1\r\nFQV VV  FQTV-VV87000038589;S3;P1\r\nT-K870-3544667869\r\nFE.-44877533.-/NONENDO/REFRESTR/REB20USD /VAT UAH 978.33;S2-3;P1\r\nFM*M*1A\r\nFPCASH\r\nFVVV;S2-3;P1\r\nTKOK04DEC/IEVVV0009\r\nRIZTICKET FARE UAH 4714.00\r\nRIZTICKET TAX UAH 1156.00\r\nRIZTICKET TTL UAH 5870.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 5930.00\r\nENDX\r\n	1	2011-12-05 11:11:37: Билет 870-3544667869 успешно импортирован\r\n	0	\N	\N	\N	\N
da0914cf2bfe4668b6aa055bd86d2074	Air	2	SYSTEM	2011-12-05 11:25:54	SYSTEM	2011-12-05 11:25:54	AIR_20111205092546.16328.PDT	2011-12-05 11:25:00	AIR-BLK207;7M;;247;0500019690;1A1336371;001001\r\nAMD 0501326267;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 4UZZPY039;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 4UZZPY\r\nB-TTM/L9\r\nC-7906/ 2222MVSU-7777SVSU-M---\r\nD-111116;111205;111205\r\nU-009X;007OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0095 K K 10DEC1345 1500 10DEC;OK01;HK01;N ;0;735;;;;B ;;ET;0115 ;N;;347;;UA;UA;  \r\nMCO277;009VV;8702;AEROSVIT AIRLINES;KBP;TO-AEROSVIT AIRLINES;AT-KIEV;10DEC;M;A;PENALTY FOR CHANGE;**-;-PENALTY FOR CHANGE;F;UAH        240;N;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;UAH        240;;;870-3544598447;;;;UAH        240;;;; ;P1\r\nI-001;01FESHCHENKO/PAVLO MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nTMCM870-9074146871;;L9\r\nMFM*M*0;L9\r\nMFPCASH;L9\r\nENDX\r\n	1	2011-12-05 11:25:54: MCO 870-9074146871 успешно импортирован\r\n	0	\N	\N	\N	\N
9bbd539773d3495196118bd83625e138	Air	2	SYSTEM	2011-12-05 11:52:23	SYSTEM	2011-12-05 11:52:23	AIR_20111205095204.16642.PDT	2011-12-05 11:52:00	AIR-BLK207;RF;;190;0500019812;1A1336371;001001\r\nAMD 0501326315;1/1;    05DEC;SVSU\r\nGW3670821;1A1336371\r\nMUC1A          ;  01;DOKC32530;72320942;;;;;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;\r\nB-TRFP\r\nC-7906/ 7777SVSU-7777SVSU-M---\r\nD-111205;111205;111205\r\nRFDF;01DEC11;I;UAH1574;0;1574;;;;;;XT178;1752;04DEC11\r\nKRF ;QUAH120      YK   ;QUAH16       UD   ;QUAH42       UA   ;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nI-001;01RYPALOV/VOLODYMYR MR;;;;\r\nT-E870-3544667778\r\nTBS1-1200\r\nR-870-3544667778;05DEC11\r\nSAC870AJN08I041U\r\nFM5.00P\r\nFPCASH/UAH1752\r\nFTITAGVV01-801\r\nENDX\r\n	1	2011-12-05 11:52:23: Возврат 870-3544667778 успешно импортирован\r\n	0	\N	\N	\N	\N
c52fe5c9fa2548babb2cc128f1032bc9	Air	2	SYSTEM	2011-12-05 12:31:53	SYSTEM	2011-12-05 12:31:53	AIR_20111205103145.17113.PDT	2011-12-05 12:31:00	AIR-BLK207;7A;;257;0500026626;1A1336371;001001\r\nAMD 0501326406;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 2ASQ9P011;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;B2 NWYDRF;PS RJ1QL \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/T4/RT\r\nC-7906/ 7777SVSU-7777SVSU-M-E--\r\nD-111205;111205;111205\r\nG-   ;;DOKDOK;UA\r\nH-007;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    0048 V V 06DEC0720 0830 06DEC;OK01;HK01;S ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nH-006;005OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 W W 09DEC1950 2100 09DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nU-008X;003OKBP;KIEV BORYSPIL    ;MSQ;MINSK MINSK 2 INT;B2    0846 V V 06DEC1130 1335 06DEC;OK01;HK01;;0;CRJ;;;;B ;;ET;0105 ; ;;283;;UA;BY;  \r\nU-009X;004OMSQ;MINSK MINSK 2 INT;KBP;KIEV BORYSPIL    ;B2    0845 V V 09DEC1730 1735 09DEC;OK01;HK01;;0;735;;;;;;ET;0105 ; ;;283;;BY;UA;B \r\nK-FUSD243.00     ;UAH1942       ;;;;;;;;;;;UAH2623       ;7.9899     ;;   \r\nKFTF; UAH408      HF GO; UAH8        UD DP; UAH32       YQ AD; UAH144      YR VA; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH408      HF ;UAH8        UD ;UAH265      XT ;\r\nL-\r\nM-VFLYDOM  KK10  ;WFLYDOM  KK10  \r\nN-USD101.25;141.75\r\nO-06DEC06DEC;09DEC09DEC\r\nQ-DOK PS IEV101.25PS DOK141.75USD243.00END XT32YQ144YR56YK33UA;FXP/ZO-10P*KK10/S2,5\r\nI-001;01SINELNIKOV/VOLODYMYR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667875\r\nFENON END/REF REST/RBK 10USD;S2,5;P1\r\nFM*M*1;S2,5;P1\r\nFPCASH\r\nFTKK4704/11;S2,5;P1\r\nFVPS;S2,5;P1\r\nTKOK05DEC/DOKC32530\r\nENDX\r\n	1	2011-12-05 12:31:53: Билет 566-3544667875 успешно импортирован\r\n	0	\N	\N	\N	\N
69da27d4663949b1bb88c1d32d1818ae	Air	2	SYSTEM	2011-12-05 12:35:45	SYSTEM	2011-12-05 12:35:45	AIR_20111205103535.17149.PDT	2011-12-05 12:35:00	AIR-BLK207;MA;;233;0500019741;1A1336371;001001\r\nAMD 0501326411;1/1;VOID05DEC;MVSU\r\nGW3671291;1A1336371\r\nMUC1A Z9T4GO005;0301;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;SU PALIYP\r\nI-001;01BEZKOROVAYNYY/MYKOLA MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K555-3544667871\r\nFPCASH\r\nENDX\r\n	1	2011-12-05 12:35:45: Void 555-3544667871 успешно импортирован\r\n	0	\N	\N	\N	\N
fb673ec7ea8445969b6deaf189f2a055	Air	2	SYSTEM	2011-12-05 12:36:07	SYSTEM	2011-12-05 12:36:07	AIR_20111205103547.17152.PDT	2011-12-05 12:35:00	AIR-BLK207;MA;;233;0500036701;1A1336371;001001\r\nAMD 0501326412;1/1;VOID05DEC;MVSU\r\nGW3671291;1A1336371\r\nMUC1A Z9T4GO005;0301;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;SU PALIYP\r\nI-002;02SHKODA/OLGA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K555-3544667872\r\nFPCASH\r\nENDX\r\n	1	2011-12-05 12:36:07: Void 555-3544667872 успешно импортирован\r\n	0	\N	\N	\N	\N
cb099697ba1b4cd0bd080292f4b09891	Air	2	SYSTEM	2011-12-05 12:36:08	SYSTEM	2011-12-05 12:36:08	AIR_20111205103555.17155.PDT	2011-12-05 12:35:00	AIR-BLK207;MA;;233;0500021610;1A1336371;001001\r\nAMD 0501326413;1/1;VOID05DEC;MVSU\r\nGW3671291;1A1336371\r\nMUC1A Z9T4GO005;0301;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;SU PALIYP\r\nI-003;03SHKODA/VLADA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K555-3544667873\r\nFPCASH\r\nENDX\r\n	1	2011-12-05 12:36:08: Void 555-3544667873 успешно импортирован\r\n	0	\N	\N	\N	\N
3078b5db078d417e9b894eecde4b940f	Air	2	SYSTEM	2011-12-05 12:36:30	SYSTEM	2011-12-05 12:36:31	AIR_20111205103616.17161.PDT	2011-12-05 12:36:00	AIR-BLK207;7A;;247;0500036762;1A1336371;001001\r\nAMD 0501326414;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A Z9T4GO013;0303;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;SU PALIYP\r\nA-AEROFLOT;SU 5552\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111205;111205;111205\r\nG-X  ;;DOKDOK;E1\r\nH-001;004ODOK;DONETSK          ;SVO;MOSCOW SVO       ;SU    4139 Q Q 26DEC0740 1135 26DEC;OK03;HK03;L ;0;735;;;20K;;;ET;0155 ; ;545;UA;RU;C \r\nX-001;004ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0301 Q Q 26DEC0740 1135 26DEC;OK03;HK03;L ;0;735;;;20K;;;ET;0155 ; ;C \r\nH-002;005XSVO;MOSCOW SVO       ;LED;ST PETERSBURG    ;SU    0845 Q Q 26DEC1400 1530 26DEC;OK03;HK03;L ;0;320;;;20K;D ;;ET;0130 ; ;386;RU;RU;1 \r\nH-007;006OLED;ST PETERSBURG    ;SVO;MOSCOW SVO       ;SU    0830 Q Q 06JAN0855 1020 06JAN;OK03;HK03;L ;0;SU9;;;20K;1 ;;ET;0125 ; ;386;RU;RU;D \r\nH-008;007XSVO;MOSCOW SVO       ;DOK;DONETSK          ;SU    4138 Q Q 06JAN1235 1210 06JAN;OK03;HK03;L ;0;735;;;20K;C ;;ET;0135 ; ;545;RU;UA;  \r\nX-008;007XSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 Q Q 06JAN1235 1210 06JAN;OK03;HK03;L ;0;735;;;20K;C ;;ET;0135 ; ;  \r\nK-FUSD215.00     ;UAH1718       ;;;;;;;;;;;UAH2040       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH144      YR VB; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH186      XT ;\r\nL-\r\nM-QPX            ;QPX            ;QPX            ;QPX            \r\nN-NUC;107.50;;107.50\r\nO-26DEC26DEC;26DEC26DEC;06JAN06JAN;06JAN06JAN\r\nQ-DOK SU X/MOW SU LED107.50SU X/MOW SU DOK107.50NUC215.00END ROE1.000000XT144YR42UA;FXB\r\nI-001;01BEZKOROVAYNYY/MYKOLA MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K555-3544667876\r\nFEVALID ON SU/FARE RESTR APL;S4-7;P1-3\r\nFM*M*5\r\nFPCASH\r\nFVSU;S4-7;P1-3\r\nTKOK05DEC/DOKC32530//ETSU\r\nI-002;02SHKODA/OLGA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR DOCS SU  HK1/P/UKR/BE111371/UKR/14JUL66/F/27JUN21/SHKODA/OLGA/H;P2\r\nT-K555-3544667877\r\nFEVALID ON SU/FARE RESTR APL;S4-7;P1-3\r\nFM*M*5\r\nFPCASH\r\nFVSU;S4-7;P1-3\r\nTKOK05DEC/DOKC32530//ETSU\r\nI-003;03SHKODA/VLADA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K555-3544667878\r\nFEVALID ON SU/FARE RESTR APL;S4-7;P1-3\r\nFM*M*5\r\nFPCASH\r\nFVSU;S4-7;P1-3\r\nTKOK05DEC/DOKC32530//ETSU\r\nRIZTICKET FARE UAH 1718.00\r\nRIZTICKET TAX UAH 322.00\r\nRIZTICKET TTL UAH 2040.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 2160.00\r\nENDX\r\n	1	2011-12-05 12:36:31: Билет 555-3544667876 успешно импортирован\r\nБилет 555-3544667877 успешно импортирован\r\nБилет 555-3544667878 успешно импортирован\r\n	0	\N	\N	\N	\N
c2140eef655744dcad9360125fec06ba	Air	2	SYSTEM	2011-12-05 13:02:15	SYSTEM	2011-12-05 13:02:15	AIR_20111205110206.17470.PDT	2011-12-05 13:02:00	AIR-BLK207;7A;;247;0500023063;1A1336371;001001\r\nAMD 0501326460;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A ZXJ9X7022;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS RFNNF \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111203;111205;111205\r\nG-   ;;DOKDOK;UA\r\nH-012;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    0048 N N 06DEC0720 0830 06DEC;OK01;HK01;S ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nH-011;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 N N 06DEC1950 2100 06DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD145.00     ;UAH1159       ;;;;;;;;;;;UAH1685       ;7.9899     ;;   \r\nKFTF; UAH253      HF GO; UAH8        UD DP; UAH32       YQ AD; UAH144      YR VA; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH253      HF ;UAH8        UD ;UAH265      XT ;\r\nL-\r\nM-NFLYDOM        ;NFLYDOM        \r\nN-USD72.50;72.50\r\nO-06DEC06DEC;06DEC06DEC\r\nQ-DOK PS IEV72.50PS DOK72.50USD145.00END XT32YQ144YR56YK33UA;FXB\r\nI-001;01EPEL/OLEKSANDR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667879\r\nFENON END/NO REF/RBK 20USD;S2-3;P1\r\nFM*M*1\r\nFPCASH\r\nFVPS;S2-3;P1\r\nTKOK03DEC/DOKC32530\r\nRIZTICKET FARE UAH 1159.00\r\nRIZTICKET TAX UAH 526.00\r\nRIZTICKET TTL UAH 1685.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1745.00\r\nENDX\r\n	1	2011-12-05 13:02:15: Билет 566-3544667879 успешно импортирован\r\n	0	\N	\N	\N	\N
8b766a5fa1cc4ad49b38160e6e717771	Air	2	SYSTEM	2011-12-05 14:51:15	SYSTEM	2011-12-05 14:51:15	AIR_20111205125054.18609.PDT	2011-12-05 14:50:00	AIR-BLK207;7A;;247;0500051488;1A1336371;001001\r\nAMD 0501326629;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A 2EL7BH004;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 2EL7BH\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111205;111205;111205\r\nG-   ;;DOKDOK;UA\r\nH-003;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    4094 K K 06DEC0720 0830 06DEC;OK01;HK01;S ;0;737;;;20K;;;ET;0110 ; ;347;UA;UA;B \r\nX-003;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    0048 K K 06DEC0720 0830 06DEC;OK01;HK01;S ;0;737;;;20K;;;ET;0110 ; ;B \r\nH-002;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 K K 06DEC2130 2240 06DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD150.00     ;UAH1199       ;;;;;;;;;;;UAH1653       ;7.9899     ;;   \r\nKFTF; UAH261      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH261      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-KSS1M          ;KSS1M          \r\nN-USD75.00;75.00\r\nO-06DEC06DEC;06DEC06DEC\r\nQ-DOK VV IEV75.00VV DOK75.00USD150.00END XT96YQ56YK33UA;FXB\r\nI-001;01SHVETSOV/ROMAN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667882\r\nFE.-44877533.-/NONENDO/REFRESTR/RBK USD30 /VAT UAH 275.50;S2-3;P1\r\nFM*M*1A\r\nFPCASH\r\nFVVV;S2-3;P1\r\nTKOK05DEC/DOKC32530\r\nRIZTICKET FARE UAH 1199.00\r\nRIZTICKET TAX UAH 454.00\r\nRIZTICKET TTL UAH 1653.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1713.00\r\nENDX\r\n	1	2011-12-05 14:51:15: Билет 870-3544667882 успешно импортирован\r\n	0	\N	\N	\N	\N
8c69fbf5ed12486ea3739b00bd7ac256	Air	2	SYSTEM	2011-12-05 14:52:21	SYSTEM	2011-12-05 14:52:21	AIR_20111205125215.18620.PDT	2011-12-05 14:52:00	AIR-BLK207;7M;;247;0500049455;1A1336371;001001\r\nAMD 0501326631;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A Y7DFFD017;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS R3ZP7 \r\nB-TTM\r\nC-7906/ 7777SVSU-7777SVSU-M---\r\nD-111201;111205;111205\r\nU-001X;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    0048 N N 05DEC0720 0830 05DEC;OK01;HK01;S ;0;735;;;;;;ET;0110 ;N;;347;;UA;UA;B \r\nU-003X;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 N N 05DEC1950 2100 05DEC;OK01;HK01;S ;0;735;;;;B ;;ET;0110 ;N;;347;;UA;UA;  \r\nMCO104;004PS;5666;UKRAINE INTL AIRLINES;KBP;TO-UKRAINE INTL AIRLINES;AT-KIEV;05DEC;M;A;Q;**-;;F;UAH        160;N;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;UAH        160;;;;;;;UAH        160;;;; ;P1\r\nI-001;01COCKUN/HUSAIN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nTMCM566-9074146873;;L4\r\nMFM*M*0;L4\r\nMFPCASH;L4\r\nTKOK01DEC/DOKC32530//ETPS\r\nRIZTICKET FARE UAH 1159.00\r\nRIZTICKET TAX UAH 494.00\r\nRIZTICKET TTL UAH 1653.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1713.00\r\nENDX\r\n	1	2011-12-05 14:52:21: MCO 566-9074146873 успешно импортирован\r\n	0	\N	\N	\N	\N
2d6ff0e00af94b02bc0afd13d1fdd4b4	Air	2	SYSTEM	2011-12-05 13:06:27	SYSTEM	2011-12-05 13:06:27	AIR_20111205110616.17503.PDT	2011-12-05 13:06:00	AIR-BLK207;7A;;247;0500023414;1A1336371;001001\r\nAMD 0501326466;1/1;              \r\nGW3671291;1A1336371;IEV1A098A;AIR\r\nMUC1A Z9VPIO010;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Z9VPIO\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 2222MVSU-2222MVSU-I-0--\r\nD-111205;111205;111205\r\nG-   ;;IEVODS;UA\r\nH-009;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 K K 06DEC2130 2240 06DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nH-010;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0096 S S 08DEC1700 1820 08DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nH-011;004XKBP;KIEV BORYSPIL    ;ODS;ODESA            ;VV    4017 S S 08DEC2115 2215 08DEC;OK01;HK01;N ;0;735;;;20K;B ;;ET;0100 ; ;266;UA;UA;  \r\nX-011;004XKBP;KIEV BORYSPIL    ;ODS;ODESA            ;PS    0055 S S 08DEC2115 2215 08DEC;OK01;HK01;N ;0;735;;;20K;B ;;ET;0100 ; ;  \r\nK-FUSD260.00     ;UAH2078       ;;;;;;;;;;;UAH2799       ;7.9899     ;;   \r\nKFTF; UAH444      HF GO; UAH12       UD DP; UAH144      YQ AC; UAH76       YK AE; UAH45       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH444      HF ;UAH12       UD ;UAH265      XT ;\r\nL-\r\nM-KSS1M          ;S1RT           ;S1RT           \r\nN-USD75.00;;185.00\r\nO-06DEC06DEC;08DEC08DEC;08DEC08DEC\r\nQ-IEV VV DOK75.00VV X/IEV VV ODS185.00USD260.00END XT144YQ76YK45UA;FXB\r\nI-001;01ALLAKHVERDIEVA/ZAMINA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667880\r\nFE.-44877533.-/NONENDO/REFRESTR/RBK USD30 /VAT UAH 466.50;S2-4;P1\r\nFM*M*1A\r\nFPCASH\r\nFVVV;S2-4;P1\r\nTKOK05DEC/DOKC32530\r\nRIZTICKET FARE UAH 2078.00\r\nRIZTICKET TAX UAH 721.00\r\nRIZTICKET TTL UAH 2799.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2859.00\r\nENDX\r\n	1	2011-12-05 13:06:27: Билет 870-3544667880 успешно импортирован\r\n	0	\N	\N	\N	\N
2f87b37dd2794014bf517e6058c74f5e	Air	2	SYSTEM	2011-12-05 14:23:44	SYSTEM	2011-12-05 14:23:44	AIR_20111205122331.18354.PDT	2011-12-05 14:23:00	AIR-BLK207;7A;;247;0500028420;1A1336371;001001\r\nAMD 0501326583;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A 2AAUQP010;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS RJ0B2 \r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/ITR-EML-EKA@CAMBIO.DN.UA\r\nC-7906/ 0001AASU-0001AASU-I-0--\r\nD-111205;111205;111205\r\nG-   ;;DOKDOK;UA\r\nH-006;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    1490 N N 06DEC0700 0820 06DEC;OK01;HK01;M ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nX-006;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0090 N N 06DEC0700 0820 06DEC;OK01;HK01;M ;0;320;;;20K;;;ET;0120 ;N;B \r\nH-007;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 N N 06DEC1950 2100 06DEC;OK01;HK01;S ;0;735;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nK-FUSD145.00     ;UAH1159       ;;;;;;;;;;;UAH1685       ;7.9899     ;;   \r\nKFTF; UAH253      HF GO; UAH8        UD DP; UAH32       YQ AD; UAH144      YR VA; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH253      HF ;UAH8        UD ;UAH265      XT ;\r\nL-\r\nM-NFLYDOM        ;NFLYDOM        \r\nN-USD72.50;72.50\r\nO-06DEC06DEC;06DEC06DEC\r\nQ-DOK PS IEV72.50PS DOK72.50USD145.00END XT32YQ144YR56YK33UA;FXB\r\nI-001;01CHAUSOVSKA/KATERYNA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K566-3544667881\r\nFENON END/NO REF/RBK 20USD;S2-3;P1\r\nFM*M*1A\r\nFPCASH\r\nFVPS;S2-3;P1\r\nTKOK05DEC/DOKC32530\r\nRIZTICKET FARE UAH 1159.00\r\nRIZTICKET TAX UAH 526.00\r\nRIZTICKET TTL UAH 1685.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1745.00\r\nENDX\r\n	1	2011-12-05 14:23:44: Билет 566-3544667881 успешно импортирован\r\n	0	\N	\N	\N	\N
a74327095790432ab685b984654e2836	Air	2	SYSTEM	2011-12-05 15:06:59	SYSTEM	2011-12-05 15:06:59	AIR_20111205130645.18753.PDT	2011-12-05 15:06:00	AIR-BLK207;7A;;257;0500040570;1A1336371;001001\r\nAMD 0501326660;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 2D26OX009;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS RJ64S ;VV 2D26OX\r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/S2/RT\r\nC-7906/ 7777SVSU-7777SVSU-M-E--\r\nD-111205;111205;111205\r\nG-   ;;DOKIEV;UA\r\nH-001;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    0048 H H 06DEC0720 0830 06DEC;OK01;HK01;S ;0;735;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nU-002X;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0095 L L 07DEC1345 1500 07DEC;OK01;HK01;N ;0;735;;;;B ;;ET;0115 ;N;;347;;UA;UA;  \r\nK-FUSD150.00     ;UAH1199       ;;;;;;;;;;;UAH1601       ;7.9899     ;;   \r\nKFTF; UAH253      HF GO; UAH4        UD DP; UAH16       YQ AD; UAH72       YR VA; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH253      HF ;UAH4        UD ;UAH145      XT ;\r\nL-\r\nM-HOWDOM   KK10  \r\nN-USD150.30\r\nO-06DEC06DEC\r\nQ-DOK PS IEV150.30USD150.30END XT16YQ72YR36YK21UA;FXP/ZO-10P*KK10/S2\r\nI-001;01MASLOV/IGOR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV PS  FQTV-PS4060972;S2;P1\r\nT-K566-3544667883\r\nFENON END/REF REST/RBK 10USD;S2;P1\r\nFM*M*1;S2;P1\r\nFPCASH\r\nFTKK4704/11;S2;P1\r\nFVPS;S2;P1\r\nTKOK05DEC/DOKC32530\r\nENDX\r\n	1	2011-12-05 15:06:59: Билет 566-3544667883 успешно импортирован\r\n	0	\N	\N	\N	\N
8947597771fd4e118f9e466cb317b90b	Air	2	SYSTEM	2011-12-05 15:07:00	SYSTEM	2011-12-05 15:07:00	AIR_20111205130651.18754.PDT	2011-12-05 15:06:00	AIR-BLK207;7A;;257;0500042419;1A1336371;001001\r\nAMD 0501326661;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 2D26OX010;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS RJ64S ;VV 2D26OX\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/S3/RT\r\nC-7906/ 7777SVSU-7777SVSU-M-E--\r\nD-111205;111205;111205\r\nG-   ;;IEVDOK;UA\r\nH-002;003OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0095 L L 07DEC1345 1500 07DEC;OK01;HK01;N ;0;735;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nU-001X;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    0048 H H 06DEC0720 0830 06DEC;OK01;HK01;S ;0;735;;;;;;ET;0110 ;N;;347;;UA;UA;B \r\nK-FUSD113.00     ;UAH903        ;;;;;;;;;;;UAH1174       ;7.9899     ;;   \r\nKFTF; UAH187      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH20       YK AE; UAH12       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH187      HF ;UAH4        UD ;UAH80       XT ;\r\nL-\r\nM-LOW1M5   KK10  \r\nN-USD112.50\r\nO-07DEC07DEC\r\nQ-IEV VV DOK112.50USD112.50END XT48YQ20YK12UA;FXP/ZO-10P*KK10/S3\r\nI-001;01MASLOV/IGOR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nFQV VV  FQTV-VV870000616483;S3;P1\r\nT-K870-3544667884\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 195.67;S3;P1\r\nFM*M*1A;S3;P1\r\nFPCASH\r\nFTITAGVV01-851;S3;P1\r\nFVVV;S3;P1\r\nTKOK05DEC/DOKC32530\r\nENDX\r\n	1	2011-12-05 15:07:00: Билет 870-3544667884 успешно импортирован\r\n	0	\N	\N	\N	\N
ad7e3c6c96dd4582a1693c196e1f37e8	Air	2	SYSTEM	2011-12-05 16:19:42	SYSTEM	2011-12-05 16:19:42	AIR_20111205141556.19520.PDT	2011-12-05 16:15:00	AIR-BLK207;7A;;249;0500049461;1A1336371;001001\r\nAMD 0501326825;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 7CQPVX028;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS RJ6XP ;VV 7CQPVX\r\nA-UKRAINE INTL AIRLINES;PS 5666\r\nB-TTP/S3/RT\r\nC-7906/ 0001AASU-7777SVSU-M-E--\r\nD-111123;111205;111205\r\nG-X  ;;MOWIEV;E1\r\nH-006;003ODME;MOSCOW DME       ;KBP;KIEV BORYSPIL    ;PS    0574 Z Z 09DEC2310 2235 09DEC;OK01;HK01;S ;0;735;;;40K;;;ET;0125 ;N;454;RU;UA;F \r\nU-003X;002ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0301 D D 09DEC0740 1135 09DEC;OK01;HK01;L ;0;735;;;;;;ET;0155 ;N;;545;;UA;RU;C \r\nK-FEUR351.00     ;UAH3789       ;;;;;;;;;;;UAH4068       ;10.79515   ;;   \r\nKFTF; UAH16       YQ AD; UAH144      YR VA; UAH68       RI DP; UAH51       UH SE;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH16       YQ ;UAH144      YR ;UAH119      XT ;\r\nL-\r\nM-ZPSUAOW  KK10  \r\nN-NUC492.29\r\nO-XXXX\r\nQ-MOW PS IEV492.29NUC492.29END ROE0.712992XT68RI51UH;FXP/ZO-10P*KK10/S3\r\nI-001;01IGNATOVSKYY/SERGIY MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nSSR OTHS 1A  /XXLD AS NO TKT INFO\r\nT-K566-3544667897\r\nFENON END/REF AND CHNG RESTR;S3;P1\r\nFM*M*3;S3;P1\r\nFPCASH\r\nFTKK4715/11;S3;P1\r\nFVPS;S3;P1\r\nTKOK23NOV/DOKC32530\r\nRM PISHEM SEYCHAS//BRDS///MVZ1200///\r\nRM PISHEM SEYCHAS//BRDS///MVZ1200///\r\nENDX\r\n	1	2011-12-05 16:19:42: Билет 566-3544667897 успешно импортирован\r\n	0	\N	\N	\N	\N
e734b38e867c44198b016c7517c1aef9	Air	2	SYSTEM	2011-12-05 15:25:50	SYSTEM	2011-12-05 15:25:51	AIR_20111205132532.18920.PDT	2011-12-05 15:25:00	AIR-BLK207;7A;;239;0500023476;1A1336371;001001\r\nAMD 0501326704;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A 2DSR3G007;0606;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 2DSR3G\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/ITR-EML-OLESYA.KRIVORUCHKO@METINVESTHOLDING.COM\r\nC-7906/ 0001AASU-0001AASU-M-E--\r\nD-111205;111205;111205\r\nG-X  ;;DOKDOK;E1\r\nH-001;007ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0305 K K 11DEC1805 2155 11DEC;OK06;HK06;L ;0;735;;;20K;;;ET;0150 ;N;545;UA;RU;C \r\nH-002;008OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 K K 14DEC1235 1210 14DEC;OK06;HK06;L ;0;735;;;20K;C ;;ET;0135 ;N;545;RU;UA;  \r\nK-FUSD197.00     ;UAH1574       ;;;;;;;;;;;UAH1752       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-KPX3M5   KK6   ;KPX3M5   KK6   \r\nN-NUC98.70;98.70\r\nO-11DEC11DEC;14DEC14DEC\r\nQ-DOK VV MOW98.70VV DOK98.70NUC197.40END ROE1.000000;FXP/ZO-6P*KK6\r\nI-001;01GURTOVAYA/NATALIYA MRS;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667889\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S7-8;P1-6\r\nTKOK05DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///MVZ4000////\r\nI-002;02IGNATUSHA/ANATOLIY MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667890\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S7-8;P1-6\r\nTKOK05DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///MVZ4000////\r\nI-003;03KALNITSKIY/VITALIY MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667891\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S7-8;P1-6\r\nTKOK05DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///MVZ4000////\r\nI-004;04MARIN/OLEG MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667892\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S7-8;P1-6\r\nTKOK05DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///MVZ4000////\r\nI-006;05PODKORYTOV/OLEKSANDR MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667893\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S7-8;P1-6\r\nTKOK05DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///MVZ4000////\r\nI-005;06PYLYPENKO/OLENA MRS;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667894\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S7-8;P1-6\r\nTKOK05DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///MVZ4000////\r\nRIZTICKET FARE UAH 1574.00\r\nRIZTICKET TAX UAH 178.00\r\nRIZTICKET TTL UAH 1752.00\r\nRIZSERVICE FEE UAH 80.00+VAT 16.00\r\nRIZGRAND TOTAL UAH 1848.00\r\nENDX\r\n	1	2011-12-05 15:25:51: Билет 870-3544667889 успешно импортирован\r\nБилет 870-3544667890 успешно импортирован\r\nБилет 870-3544667891 успешно импортирован\r\nБилет 870-3544667892 успешно импортирован\r\nБилет 870-3544667893 успешно импортирован\r\nБилет 870-3544667894 успешно импортирован\r\n	0	\N	\N	\N	\N
780211c03d8f44f9bc4bd1552ef81f9a	Air	2	SYSTEM	2011-12-05 16:19:39	SYSTEM	2011-12-05 16:19:40	AIR_20111205141105.19466.PDT	2011-12-05 16:11:00	AIR-BLK207;7A;;247;0500034798;1A1336371;001001\r\nAMD 0501326818;1/1;              \r\nGW3670821;1A1336371;IEV1A098A;AIR\r\nMUC1A 2C6A32009;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;UN PJPHG \r\nA-TRANSAERO AIRLINES;UN 6705\r\nB-TTP\r\nC-7906/ 2222MVSU-7777SVSU-I-0--\r\nD-111205;111205;111205\r\nG-X  ;;DOKDOK;E1\r\nH-003;002ODOK;DONETSK          ;DME;MOSCOW DME       ;UN    0244 W W 06DEC1150 1530 06DEC;OK01;HK01;S ;0;735;;;20K;;;ET;0140 ;N;506;UA;RU;  \r\nH-004;003ODME;MOSCOW DME       ;DOK;DONETSK          ;UN    0243 W W 09DEC1640 1620 09DEC;OK01;HK01;S ;0;735;;;20K;;;ET;0140 ;N;506;RU;UA;  \r\nK-FUSD100.00     ;UAH799        ;;;;;;;;;;;UAH1625       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH324      YQ AC; UAH324      YR VB; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH690      XT ;\r\nL-\r\nM-WPR2M          ;WPR2M          \r\nN-NUC50.00;50.00\r\nO-06DEC06DEC;09DEC09DEC\r\nQ-DOK UN MOW50.00UN DOK50.00NUC100.00END ROE1.000000XT324YQ324YR42UA;FXB\r\nI-001;01SYMONOVICH/VALENTIN MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nSSR OTHS 1A  /ADTK IN SSR TKNE TILL 1751/DOK/05DEC OR PNR WILL BE CXLD\r\nT-K670-3544667895\r\nFM*M*5\r\nFPCASH\r\nFVUN;S2-3;P1\r\nTKOK05DEC/DOKC32530\r\nRIZTICKET FARE UAH 799.00\r\nRIZTICKET TAX UAH 826.00\r\nRIZTICKET TTL UAH 1625.00\r\nRIZSERVICE FEE UAH 100.00+VAT 20.00\r\nRIZGRAND TOTAL UAH 1745.00\r\nENDX\r\n	1	2011-12-05 16:19:40: Билет 670-3544667895 успешно импортирован\r\n	0	\N	\N	\N	\N
500b2251de5041cfb6a2c17a07ef8540	Air	2	SYSTEM	2011-12-05 16:19:41	SYSTEM	2011-12-05 16:19:41	AIR_20111205141549.19518.PDT	2011-12-05 16:15:00	AIR-BLK207;7A;;249;0500047393;1A1336371;001001\r\nAMD 0501326824;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 7CQPVX027;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;PS RJ6XP ;VV 7CQPVX\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/S2/RT\r\nC-7906/ 0001AASU-7777SVSU-M-E--\r\nD-111123;111205;111205\r\nG-X  ;;DOKMOW;E1\r\nH-003;002ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0301 D D 09DEC0740 1135 09DEC;OK01;HK01;L ;0;735;;;30K;;;ET;0155 ;N;545;UA;RU;C \r\nU-006X;003ODME;MOSCOW DME       ;KBP;KIEV BORYSPIL    ;PS    0574 Z Z 09DEC2310 2235 09DEC;OK01;HK01;S ;0;735;;;;;;ET;0125 ;N;;454;;RU;UA;F \r\nK-FUSD270.00     ;UAH2158       ;;;;;;;;;;;UAH2336       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-DOW1     KK10  \r\nN-NUC270.00\r\nO-XXXX\r\nQ-DOK VV MOW270.00NUC270.00END ROE1.000000;FXP/S2/ZO-10P*KK10\r\nI-001;01IGNATOVSKYY/SERGIY MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nSSR OTHS 1A  /XXLD AS NO TKT INFO\r\nT-K870-3544667896\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-801;S2;P1\r\nFM*M*5;S2;P1\r\nFPCASH\r\nFTITAGVV01-801;S2;P1\r\nFVVV;S2;P1\r\nTKOK23NOV/DOKC32530\r\nRM PISHEM SEYCHAS//BRDS///MVZ1200///\r\nRM PISHEM SEYCHAS//BRDS///MVZ1200///\r\nENDX\r\n	1	2011-12-05 16:19:41: Билет 870-3544667896 успешно импортирован\r\n	0	\N	\N	\N	\N
fac7f255a29540e69138ecae5de7fc23	Air	2	SYSTEM	2011-12-05 17:11:51	SYSTEM	2011-12-05 17:11:51	AIR_20111205151139.20077.PDT	2011-12-05 17:11:00	AIR-BLK207;7A;;247;0500055547;1A1336371;001001\r\nAMD 0501326940;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 2FEPVR007;0303;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 2FEPVR\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-7777SVSU-M-E--\r\nD-111205;111205;111205\r\nG-   ;;DOKIEV;UA\r\nH-001;004ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0090 L L 06DEC0700 0820 06DEC;OK03;HK03;N ;0;320;;;20K;;;ET;0120 ;N;347;UA;UA;B \r\nK-FUSD113.00     ;UAH903        ;;;;;;;;;;;UAH1204       ;7.9899     ;;   \r\nKFTF; UAH192      HF GO; UAH4        UD DP; UAH48       YQ AC; UAH36       YK AE; UAH21       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH192      HF ;UAH4        UD ;UAH105      XT ;\r\nL-\r\nM-LOW1M5   KK10  \r\nN-USD112.50\r\nO-06DEC06DEC\r\nQ-DOK VV IEV112.50USD112.50END XT48YQ36YK21UA;FXP/ZO-10P*KK10\r\nI-003;01DMITROV/GENNADIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667899\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 200.67\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S4;P1-3\r\nTKOK05DEC/DOKC32530\r\nI-001;02KARYMOV/SERGIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667900\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 200.67\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S4;P1-3\r\nTKOK05DEC/DOKC32530\r\nI-002;03LESIV/VOLODYMYR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667901\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 200.67\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S4;P1-3\r\nTKOK05DEC/DOKC32530\r\nRIZTICKET FARE UAH 903.00\r\nRIZTICKET TAX UAH 301.00\r\nRIZTICKET TTL UAH 1204.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1264.00\r\nENDX\r\n	1	2011-12-05 17:11:51: Билет 870-3544667899 успешно импортирован\r\nБилет 870-3544667900 успешно импортирован\r\nБилет 870-3544667901 успешно импортирован\r\n	0	\N	\N	\N	\N
3c3a8e6ff58e4a86a2c1bd9604f52c4a	Air	2	SYSTEM	2011-12-05 18:16:58	SYSTEM	2011-12-05 18:16:58	AIR_20111205161647.20538.PDT	2011-12-05 18:16:00	AIR-BLK207;MA;;233;0500071901;1A1336371;001001\r\nAMD 0501327076;1/1;VOID05DEC;SVSU\r\nGW3718884;1A1336371\r\nMUC1A 2FEPVR007;0301;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 2FEPVR\r\nI-003;01DMITROV/GENNADIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667899\r\nFPCASH\r\nENDX\r\n	1	2011-12-05 18:16:58: Void 870-3544667899 успешно импортирован\r\n	0	\N	\N	\N	\N
903fa08427b146d1bc787829653c3151	Air	2	SYSTEM	2011-12-05 18:17:22	SYSTEM	2011-12-05 18:17:22	AIR_20111205161703.20541.PDT	2011-12-05 18:17:00	AIR-BLK207;MA;;233;0500044789;1A1336371;001001\r\nAMD 0501327078;1/1;VOID05DEC;SVSU\r\nGW3718884;1A1336371\r\nMUC1A 2FEPVR007;0301;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 2FEPVR\r\nI-001;02KARYMOV/SERGIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667900\r\nFPCASH\r\nENDX\r\n	1	2011-12-05 18:17:22: Void 870-3544667900 успешно импортирован\r\n	0	\N	\N	\N	\N
ff723ffd383a40f6b7bf4881048d8def	Air	3	SYSTEM	2011-12-05 17:05:51	viarosh	2011-12-07 13:25:15	AIR_20111205150532.20023.PDT	2011-12-05 17:05:00	AIR-BLK207;7A;;247;0500054933;1A1336371;001001\r\nAMD 0501326927;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A Z86YH7007;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV Z86YH7\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-7777SVSU-M-E--\r\nD-111205;111205;111205\r\nG-   ;;IEVIEV;UA\r\nH-001;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0091 T T 06DEC0910 1025 06DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0115 ;N;347;UA;UA;  \r\nH-002;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 Q Q 06DEC1950 2100 06DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nK-FUSD207.00     ;UAH1654       ;;;;;;;;;;;UAH2198       ;7.9899     ;;   \r\nKFTF; UAH351      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH351      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-TPX2M5   KK10  ;QPX1M5   KK10  \r\nN-USD121.50;85.50\r\nO-06DEC06DEC;06DEC06DEC\r\nQ-IEV VV DOK121.50VV IEV85.50USD207.00END XT96YQ56YK33UA;FXP/ZO-10P*KK10\r\nI-001;01SIZONENKO/SERGIY MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667898\r\nFE.-44877533.-/VVONLY/NON ENDO KK10 AGREEMENT 01-851 /VAT UAH 366.33\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK05DEC/DOKC32530\r\nRIZTICKET FARE UAH 1654.00\r\nRIZTICKET TAX UAH 544.00\r\nRIZTICKET TTL UAH 2198.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 2258.00\r\nENDX\r\n	1	2011-12-07 13:25:15: Билет 870-3544667898 успешно импортирован\r\n\n\n2011-12-05 17:05:51: Билет 870-3544667898 успешно импортирован\r\n	0	\N	\N	\N	\N
c2d8db6b7c694595badf500cc4efbf17	Air	3	SYSTEM	2011-12-05 15:24:02	viarosh	2011-12-07 14:07:09	AIR_20111205132355.18899.PDT	2011-12-05 15:23:00	AIR-BLK207;7A;;239;0500023589;1A1336371;001001\r\nAMD 0501326695;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A 2DSNA9007;0303;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 2DSNA9\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/ITR-EML-OLESYA.KRIVORUCHKO@METINVESTHOLDING.COM\r\nC-7906/ 0001AASU-0001AASU-M-E--\r\nD-111205;111205;111205\r\nG-X  ;;DOKDOK;E1\r\nH-001;004ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0305 K K 11DEC1805 2155 11DEC;OK03;HK03;L ;0;735;;;20K;;;ET;0150 ;N;545;UA;RU;C \r\nH-002;005OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0302 K K 14DEC1235 1210 14DEC;OK03;HK03;L ;0;735;;;20K;C ;;ET;0135 ;N;545;RU;UA;  \r\nK-FUSD197.00     ;UAH1574       ;;;;;;;;;;;UAH1752       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-KPX3M5   KK6   ;KPX3M5   KK6   \r\nN-NUC98.70;98.70\r\nO-11DEC11DEC;14DEC14DEC\r\nQ-DOK VV MOW98.70VV DOK98.70NUC197.40END ROE1.000000;FXP/ZO-6P*KK6\r\nI-001;01GAVRILOV/YURIY MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667885\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S4-5;P1-3\r\nTKOK05DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///MVZ4000////\r\nI-002;02GAVSHIN/VASYL MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667886\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S4-5;P1-3\r\nTKOK05DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///MVZ4000////\r\nI-003;03GOLOVKO/KOSTYANTYN MR;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667887\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S4-5;P1-3\r\nTKOK05DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS///MVZ4000////\r\nRIZTICKET FARE UAH 1574.00\r\nRIZTICKET TAX UAH 178.00\r\nRIZTICKET TTL UAH 1752.00\r\nRIZSERVICE FEE UAH 80.00+VAT 16.00\r\nRIZGRAND TOTAL UAH 1848.00\r\nENDX\r\n	3	2011-12-07 14:07:09: Билет 870-3544667885 успешно импортирован\r\nПредупреждение - Документ с номером 870-3544667886 уже существует.\r\nПредупреждение - Документ с номером 870-3544667887 уже существует.\r\n\n\n2011-12-05 15:24:03: Билет 870-3544667885 успешно импортирован\r\nБилет 870-3544667886 успешно импортирован\r\nБилет 870-3544667887 успешно импортирован\r\n	0	\N	\N	\N	\N
d99cac5edcd6466aa63b60f6f79d67f0	Air	3	SYSTEM	2011-12-05 15:25:27	viarosh	2011-12-12 17:18:28	AIR_20111205132516.18914.PDT	2011-12-05 15:25:00	AIR-BLK207;7A;;247;0500042165;1A1336371;001001\r\nAMD 0501326702;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 2ASY5V008;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 2ASY5V\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/RT\r\nC-7906/ 7777SVSU-7777SVSU-M-E--\r\nD-111205;111205;111205\r\nG-   ;;IEVIEV;UA\r\nH-003;002OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;VV    0099 K K 05DEC2130 2240 05DEC;OK01;HK01;N ;0;320;;;20K;B ;;ET;0110 ;N;347;UA;UA;  \r\nH-004;003ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;VV    0098 Q Q 06DEC1950 2100 06DEC;OK01;HK01;N ;0;320;;;20K;;;ET;0110 ;N;347;UA;UA;B \r\nK-FUSD160.00     ;UAH1279       ;;;;;;;;;;;UAH1747       ;7.9899     ;;   \r\nKFTF; UAH275      HF GO; UAH8        UD DP; UAH96       YQ AC; UAH56       YK AE; UAH33       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH275      HF ;UAH8        UD ;UAH185      XT ;\r\nL-\r\nM-KSS1M    KK6   ;QPX1M5   KK6   \r\nN-USD70.50;89.30\r\nO-05DEC05DEC;06DEC06DEC\r\nQ-IEV VV DOK70.50VV IEV89.30USD159.80END XT96YQ56YK33UA;FXP/ZO-6P*KK6\r\nI-001;01SEMYKINA/OLENA MRS;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K870-3544667888\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGREEMENT 01-851 /VAT UAH 291.17\r\nFM*M*1A\r\nFPCASH\r\nFTITAGVV01-851\r\nFVVV;S2-3;P1\r\nTKOK05DEC/DOKC32530\r\nRIZTICKET FARE UAH 1279.00\r\nRIZTICKET TAX UAH 468.00\r\nRIZTICKET TTL UAH 1747.00\r\nRIZSERVICE FEE UAH 50.00+VAT 10.00\r\nRIZGRAND TOTAL UAH 1807.00\r\nENDX\r\n	1	2011-12-12 17:18:28: Билет 870-3544667888 успешно импортирован\r\n\n\n2011-12-05 15:25:28: Билет 870-3544667888 успешно импортирован\r\n	0	\N	\N	\N	\N
ea5520a0cb5b4691b996d77a539943e8	Air	16	SYSTEM	2011-12-05 18:27:35	test	2012-03-06 21:27:58	AIR_20111205162729.20587.PDT	2011-12-05 18:27:00	AIR-BLK207;7A;;239;0500032925;1A1336371;001001\r\nAMD 0501327085;1/1;              \r\nGW3671292;1A1336371;IEV1A098A;AIR\r\nMUC1A 2E6LAN008;0101;DOKU23199;;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;VV 2E6LAN\r\nA-AEROSVIT AIRLINES;VV 8702\r\nB-TTP/ITR-EML-OLESYA.KRIVORUCHKO@METINVESTHOLDING.COM\r\nC-7906/ 0001AASU-0001AASU-M-E--\r\nD-111205;111205;111205\r\nG-X  ;;DOKDOK;E1\r\nH-001;002ODOK;DONETSK          ;SVO;MOSCOW SVO       ;VV    0305 K K 08DEC1805 2155 08DEC;OK01;HK01;L ;0;735;;;20K;;;ET;0150 ;N;545;UA;RU;C \r\nH-002;003OSVO;MOSCOW SVO       ;DOK;DONETSK          ;VV    0306 K K 12DEC2245 2230 12DEC;OK01;HK01;L ;0;735;;;20K;C ;;ET;0145 ;N;545;RU;UA;  \r\nK-FUSD197.00     ;UAH1574       ;;;;;;;;;;;UAH1752       ;7.9899     ;;   \r\nKFTF; UAH120      YK AE; UAH16       UD DP; UAH42       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH120      YK ;UAH16       UD ;UAH42       UA ;\r\nL-\r\nM-KPX3M5   KK6   ;KPX3M5   KK6   \r\nN-NUC98.70;98.70\r\nO-08DEC08DEC;12DEC12DEC\r\nQ-DOK VV MOW98.70VV DOK98.70NUC197.40END ROE1.000000;FXP/ZO-6P*KK6\r\nI-001;01KHMELEVA/NADEZHDA MRS;;APDOK +380 62 3881760 - METINVEST - A;;\r\nT-K870-3544667902\r\nFE.-44877533.-/VVONLY/NON ENDO KK6 AGR01-801\r\nFM*M*5\r\nFPCASH\r\nFTITAGVV01-801\r\nFVVV;S2-3;P1\r\nTKOK05DEC/DOKC32530\r\nRM PISHEM SEYCHAS////BRDS//MVZ4000///\r\nRIZTICKET FARE UAH 1574.00\r\nRIZTICKET TAX UAH 178.00\r\nRIZTICKET TTL UAH 1752.00\r\nRIZSERVICE FEE UAH 80.00+VAT 16.00\r\nRIZGRAND TOTAL UAH 1848.00\r\nENDX\r\n	3	2012-03-06 21:27:58: Предупреждение - Документ с номером 870-3544667902 уже существует.\r\n\n\n2012-03-06 21:25:45: Предупреждение - Документ с номером 870-3544667902 уже существует.\r\n\n\n2012-03-06 21:22:07: Билет 870-3544667902 успешно импортирован\r\n\n\n2012-03-06 21:03:36: Билет 870-3544667902 успешно импортирован\r\n\n\n2012-03-06 20:20:16: Предупреждение - Документ с номером 870-3544667902 уже существует.\r\n\n\n2012-03-06 20:11:02: Предупреждение - Документ с номером 870-3544667902 уже существует.\r\n\n\n2012-03-06 20:08:50: Предупреждение - Документ с номером 870-3544667902 уже существует.\r\n\n\n2012-03-06 20:05:43: Билет 870-3544667902 успешно импортирован\r\n\n\n2012-02-16 16:06:54: Предупреждение - Документ с номером 870-3544667902 уже существует.\r\n\n\n2012-02-16 16:01:43: Предупреждение - Документ с номером 870-3544667902 уже существует.\r\n\n\n2012-02-16 15:55:09: Предупреждение - Документ с номером 870-3544667902 уже существует.\r\n\n\n2012-02-16 15:49:56: Предупреждение - Документ с номером 870-3544667902 уже существует.\r\n\n\n2012-02-16 15:44:17: Предупреждение - Документ с номером 870-3544667902 уже существует.\r\n\n\n2012-02-16 15:39:35: Билет 870-3544667902 успешно импортирован\r\n\n\n2011-12-05 18:27:36: Билет 870-3544667902 успешно импортирован\r\n	0	\N	\N	\N	\N
777367f7c2b143de8a91214a2ca881a6	Air	3	SYSTEM	2011-12-05 12:31:52	admin	2012-05-15 15:56:50	AIR_20111205103135.17110.PDT	2011-12-05 12:31:00	AIR-BLK207;7A;;257;0500026611;1A1336371;001001\r\nAMD 0501326405;1/1;              \r\nGW3718884;1A1336371;IEV1A098A;AIR\r\nMUC1A 2ASQ9P010;0101;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;DOKC32530;72320942;;;;;;;;;;;;;;;;;;;;;;B2 NWYDRF;PS RJ1QL \r\nA-BELAVIA;B2 6285\r\nB-TTP/T3/RT\r\nC-7906/ 7777SVSU-7777SVSU-I-0--\r\nD-111205;111205;111205\r\nG-X  ;;IEVIEV;E1\r\nH-008;003OKBP;KIEV BORYSPIL    ;MSQ;MINSK MINSK 2 INT;B2    0846 V V 06DEC1130 1335 06DEC;OK01;HK01;;0;CRJ;;;20K;B ;;ET;0105 ; ;283;UA;BY;  \r\nH-009;004OMSQ;MINSK MINSK 2 INT;KBP;KIEV BORYSPIL    ;B2    0845 V V 09DEC1730 1735 09DEC;OK01;HK01;;0;735;;;20K;;;ET;0105 ; ;283;BY;UA;B \r\nU-007X;002ODOK;DONETSK          ;KBP;KIEV BORYSPIL    ;PS    0048 V V 06DEC0720 0830 06DEC;OK01;HK01;S ;0;735;;;;;;ET;0110 ;N;;347;;UA;UA;B \r\nU-006X;005OKBP;KIEV BORYSPIL    ;DOK;DONETSK          ;PS    0047 W W 09DEC1950 2100 09DEC;OK01;HK01;S ;0;735;;;;B ;;ET;0110 ;N;;347;;UA;UA;  \r\nK-FUSD185.00     ;UAH1479       ;;;;;;;;;;;UAH1663       ;7.9899     ;;   \r\nKFTF; UAH136      YK AE; UAH16       UD DP; UAH32       UA SE;;;;;;;;;;;;;;;;;;;;;;;;;;;\r\nTAX-UAH136      YK ;UAH16       UD ;UAH32       UA ;\r\nL-\r\nM-VSR            ;VSR            \r\nN-NUC92.50;92.50\r\nO-06DEC06DEC;09DEC09DEC\r\nQ-IEV B2 MSQ92.50B2 IEV92.50NUC185.00END ROE1.000000;FXB/S3-4\r\nI-001;01SINELNIKOV/VOLODYMYR MR;;APDOK 380 62 3343222 - TRAVEL SHOP "test" - A;;\r\nT-K628-3544667874\r\nFECHANGING/PENALTIES APPLIES TKT RESTR APPLY;S3-4;P1\r\nFM*M*1A;S3-4;P1\r\nFPCASH\r\nFVB2;S3-4;P1\r\nTKOK05DEC/DOKC32530\r\nENDX\r\n	1	2012-05-15 15:56:50: Билет 628-3544667874 успешно импортирован\r\n\n\n2011-12-05 12:31:52: Билет 628-3544667874 успешно импортирован\r\n	0	\N	\N	\N	\N
\.


--
-- Data for Name: lt_identity; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_identity (id, version, name) FROM stdin;
042863cee672491ebc1c21e7b57e5bb6	2	valentina.miroshnichenko
51e57a1d645b43c2b12d17c424a86500	2	svetlana.vasilkova
6dec9784a3ef4ff29a847c8394b0fc65	2	tatyana.shevchenko
c471b0d3372711dfab3816a0e28d1ada	1	SYSTEM
3953546ed10742dd8aa28ee378a28892	2	admin
\.


--
-- Data for Name: lt_internal_identity; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_internal_identity (id, description) FROM stdin;
c471b0d3372711dfab3816a0e28d1ada	\N
\.


--
-- Data for Name: lt_internal_transfer; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_internal_transfer (id, version, createdby, createdon, modifiedby, modifiedon, number_, date_, fromparty, fromorder, toparty, toorder, amount) FROM stdin;
\.


--
-- Data for Name: lt_invoice; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_invoice (id, version, number_, agreement, type, issuedate, "timestamp", content, order_, issuedby, total_amount, total_currency) FROM stdin;
\.


--
-- Data for Name: lt_issued_consignment; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_issued_consignment (id, number_, "timestamp", content, consignment, issuedby) FROM stdin;
\.


--
-- Data for Name: lt_miles_card; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_miles_card (id, version, createdby, createdon, modifiedby, modifiedon, number_, owner, organization) FROM stdin;
9cba385523584542a878236b6fc1aaa0	1	svetlana.vasilkova	2012-05-30 23:55:33	\N	\N	123	d49b37163e334b94aa4590104d603de2	\N
\.


--
-- Data for Name: lt_modification; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_modification (id, "timestamp", author, type, comment_, instancetype, instanceid, instancestring) FROM stdin;
306630	2012-05-30 20:37:22		1	\N	User	51e57a1d645b43c2b12d17c424a86500	svetlana.vasilkova
306631	2012-05-30 20:38:49	svetlana.vasilkova	1	\N	Airport	36eb900275a0490f81b46340962fe6d4	DLT - Аэропорт для удаления
306632	2012-05-30 23:48:38		1	\N	User	51e57a1d645b43c2b12d17c424a86500	svetlana.vasilkova
306633	2012-05-30 23:48:48	svetlana.vasilkova	2	\N	Organization	13522d8134e0464e887b1cd1316a0b2a	Новая организация
306634	2012-05-30 23:53:19		1	\N	User	51e57a1d645b43c2b12d17c424a86500	svetlana.vasilkova
306635	2012-05-30 23:55:33	svetlana.vasilkova	2	\N	Passport	46c565fe64d94dbfa4e9e6503a8a5144	
306636	2012-05-30 23:55:33	svetlana.vasilkova	2	\N	MilesCard	7cbbca0e9f2f4994bbba4cf8c7807c4a	
306637	2012-05-30 23:55:33	svetlana.vasilkova	1	\N	Person	d49b37163e334b94aa4590104d603de2	Пользователь для удаления
306638	2012-05-31 00:02:44		1	\N	User	51e57a1d645b43c2b12d17c424a86500	svetlana.vasilkova
306639	2012-05-31 00:05:51	svetlana.vasilkova	1	\N	Organization	3e7f951952194f35af1f7fff90ee9b70	Организация для удаления
306640	2012-05-31 00:13:21		1	\N	User	51e57a1d645b43c2b12d17c424a86500	svetlana.vasilkova
306641	2012-05-31 00:13:59	svetlana.vasilkova	1	\N	Country	c5c461808de44e2f85e2305102a3356b	Страна для удаления
306642	2012-05-31 00:22:58		1	\N	User	51e57a1d645b43c2b12d17c424a86500	svetlana.vasilkova
306643	2012-05-31 00:23:36	svetlana.vasilkova	1	\N	Currency	f42bfd6a66094743b9a842a8847662a1	DLT
\.


--
-- Data for Name: lt_modification_items; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_modification_items (property, modification, oldvalue) FROM stdin;
SessionId	306630	c627138f02154aa0a8587896dc903fca
Code	306631	TST
Settlement	306631	Los Testos
LocalizedSettlement	306631	Лос Тестос
Name	306631	Новый аэропорт
SessionId	306632	11c1720905fb425bb216e7515c796087
Phone2	306633	\N
Email2	306633	\N
IsCustomer	306633	False
IsSupplier	306633	False
ReportsTo	306633	\N
SessionId	306634	6772ecdeb25f4907b200fc5db4d7cba5
Owner	306635	d49b37163e334b94aa4590104d603de2
Citizenship	306635	\N
Birthday	306635	11/2/1971 12:00:00 AM
Gender	306635	\N
IssuedBy	306635	\N
ExpiredOn	306635	11/2/2020 12:00:00 AM
Owner	306636	d49b37163e334b94aa4590104d603de2
Organization	306636	\N
Name	306637	Новый пользователь
LegalName	306637	Иванов Иван Иванович
Phone1	306637	444-44-44
Fax	306637	555-55-55
Email1	306637	test@google.com
WebAddress	306637	www.test.com.ua
Note	306637	Тестовый пользователь\n
SessionId	306638	4cf36e9848a54878886a36d2fddae7d0
Name	306639	Новая организация
LegalName	306639	New Company
Phone1	306639	444-44-44
Fax	306639	555-55-55
Email1	306639	test@google.com
WebAddress	306639	www.test.com.ua
Note	306639	Тестовый пользователь
Code	306639	ЕГРПОУ 22
SessionId	306640	98e2c68f5bd04219a208c052ab6a900d
Name	306641	Новая страна
TwoCharCode	306641	NС
ThreeCharCode	306641	NСT
SessionId	306642	1ef88174d6824ad8b7e045783e0f4d0e
Code	306643	TPD
NumericCode	306643	99999
CyrillicCode	306643	ТПД
Name	306643	Новая валюта
\.


--
-- Data for Name: lt_opening_balance; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_opening_balance (id, version, createdby, createdon, modifiedby, modifiedon, number_, date_, balance, party) FROM stdin;
\.


--
-- Data for Name: lt_order; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_order (id, version, createdby, createdon, modifiedby, modifiedon, number_, issuedate, status, note, customer, shipto, billto, assignedto, owner, discount_amount, discount_currency, vat_amount, vat_currency, total_amount, total_currency, paid_amount, paid_currency, totaldue_amount, totaldue_currency, vatdue_amount, vatdue_currency, ispublic, issubjectofpaymentscontrol) FROM stdin;
\.


--
-- Data for Name: lt_order_item; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_order_item (id, version, createdby, createdon, modifiedby, modifiedon, "position", text, quantity, hasvat, order_, consignment, price_amount, price_currency, discount_amount, discount_currency, grandtotal_amount, grandtotal_currency, givenvat_amount, givenvat_currency, taxedtotal_amount, taxedtotal_currency) FROM stdin;
\.


--
-- Data for Name: lt_order_item_avia_link; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_order_item_avia_link (id, linktype, document) FROM stdin;
\.


--
-- Data for Name: lt_order_item_source_link; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_order_item_source_link (id) FROM stdin;
\.


--
-- Data for Name: lt_order_status_record; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_order_status_record (id, status, "timestamp", changedby, order_) FROM stdin;
\.


--
-- Data for Name: lt_organization; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_organization (id, code) FROM stdin;
a354460c7cfc42d0bdf1e7624018905a	31762080
4e176acf95ea41ff81b48389917e1740	\N
3e7f951952194f35af1f7fff90ee9b70	DELETE 22
\.


--
-- Data for Name: lt_party; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_party (id, version, createdby, createdon, modifiedby, modifiedon, name, legalname, phone1, phone2, fax, email1, email2, webaddress, iscustomer, issupplier, legaladdress, actualaddress, note, reportsto) FROM stdin;
5cbde1ec38d94566adec7f5a3a0d7cc1	2	SYSTEM	2010-03-24 00:00:00	sergey.buturlakin	2011-10-18 15:56:38	Шевченко Татьяна	\N	\N	\N	\N	\N	\N	\N	t	f	\N	\N	\N	\N
9c47021f082c4e5b9e38ee71d6dd8fa3	2	SYSTEM	2010-03-24 00:00:00	sergey.buturlakin	2011-10-18 15:56:47	Мирошниченко Валентина	\N	\N	\N	\N	\N	\N	\N	t	f	\N	\N	\N	\N
d52cb22e2e754178afb8184957fbaba8	2	SYSTEM	2010-03-24 00:00:00	sergey.buturlakin	2011-10-18 15:56:56	Василькова Светлана	\N	\N	\N	\N	\N	\N	\N	t	f	\N	\N	\N	\N
a354460c7cfc42d0bdf1e7624018905a	5	SYSTEM	2010-03-24 00:00:00	sergey.buturlakin	2011-10-18 15:58:08	ТОВ "Магазин путешествий "Мерси"	\N	+38 (062) 334-32-22	+38 (062) 334-14-18	\N	avia@test.travel	\N	\N	t	f	83001\nпр.Гурова,13	\N	\N	\N
c7dac0f7c017496892ceb3390db7fc3e	5	oleksii.dubrovskyi	2011-04-18 16:09:26	viarosh	2011-12-07 13:46:13	Сергей Бутурлакин	\N	654654	\N	\N	\N	\N	\N	f	f	\N	\N	\N	\N
4e176acf95ea41ff81b48389917e1740	2	viarosh	2011-12-07 13:45:24	\N	\N	Луксена	ТОВ "Луксена Софт"	\N	\N	\N	\N	\N	luxena.com	f	f	\N	Глубочицкая 17д\nКиев	\N	\N
8bd45630832e411d9a7cda47d018a251	2	viarosh	2012-03-03 14:14:16	test	2012-04-18 15:31:00	Администратор	\N	\N	\N	\N	\N	\N	\N	f	f	\N	\N	\N	\N
737a9b5c972e4407b91c03d20e3dac13	1	admin	2012-05-15 16:39:15	\N	\N	Конечный покупатель	\N	\N	\N	\N	\N	\N	\N	f	f	\N	\N	\N	\N
d49b37163e334b94aa4590104d603de2	2	svetlana.vasilkova	2012-05-30 23:53:29	svetlana.vasilkova	2012-05-30 23:55:33	Пользователь для удаления	Петров Петр Петрович	111-22-33	\N	643-43-44	delete@google.com	\N	www.delete.com.ua	f	f	Юридический адрес\n	Фактический адрес\n	Тестовый пользователь для удаления\n	\N
3e7f951952194f35af1f7fff90ee9b70	2	svetlana.vasilkova	2012-05-31 00:02:50	svetlana.vasilkova	2012-05-31 00:05:51	Организация для удаления	Company for deletion	112-43-22	\N	132-44-55	delete@google.com	\N	www.delete.com.ua	f	f	Юридический адрес	Фактический адрес	Тестовая организация для удаления	\N
\.


--
-- Data for Name: lt_passport; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_passport (id, version, createdby, createdon, modifiedby, modifiedon, number_, firstname, middlename, lastname, birthday, gender, expiredon, note, owner, citizenship, issuedby) FROM stdin;
9c265117cbb34b48a13f1d2cec85e1ce	1	svetlana.vasilkova	2012-05-30 23:55:33	\N	\N	111	Петр	Петрович	Петров	2012-05-01	0	2014-05-16	Для удаления	d49b37163e334b94aa4590104d603de2	\N	\N
\.


--
-- Data for Name: lt_payment; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_payment (id, class, version, createdby, createdon, modifiedby, modifiedon, number_, documentnumber, documentuniquecode, paymentform, receivedfrom, note, isvoid, date_, postedon, printeddocument, payer, assignedto, registeredby, order_, invoice, owner, amount_amount, amount_currency, vat_amount, vat_currency, authorizationcode, paymentsystem) FROM stdin;
\.


--
-- Data for Name: lt_payment_system; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_payment_system (id, version, createdby, createdon, modifiedby, modifiedon, name) FROM stdin;
\.


--
-- Data for Name: lt_penalize_operation; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_penalize_operation (id, version, createdby, createdon, modifiedby, modifiedon, type, status, description, ticket) FROM stdin;
\.


--
-- Data for Name: lt_person; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_person (id, birthday, milescardsstring, title, organization) FROM stdin;
5cbde1ec38d94566adec7f5a3a0d7cc1	\N	\N		a354460c7cfc42d0bdf1e7624018905a
9c47021f082c4e5b9e38ee71d6dd8fa3	\N	\N		a354460c7cfc42d0bdf1e7624018905a
d52cb22e2e754178afb8184957fbaba8	\N	\N		a354460c7cfc42d0bdf1e7624018905a
c7dac0f7c017496892ceb3390db7fc3e	\N	\N		4e176acf95ea41ff81b48389917e1740
8bd45630832e411d9a7cda47d018a251	\N	\N		\N
737a9b5c972e4407b91c03d20e3dac13	\N	\N		\N
d49b37163e334b94aa4590104d603de2	2012-03-28	123		\N
\.


--
-- Data for Name: lt_preferences; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_preferences (id, version, createdby, createdon, modifiedby, modifiedon, identity) FROM stdin;
\.


--
-- Data for Name: lt_sequence; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_sequence (id, name, discriminator, format, "timestamp", current) FROM stdin;
0	Task	\N	T.{0:yy}-{1}	2001-01-01 00:00:00	0
1	Invoice	\N	I.{0:yy}-{1:00000}	2001-01-01 00:00:00	0
2	Payment	\N	P.{0:yy}-{1:00000}	2001-01-01 00:00:00	0
3	CashInOrderPayment	\N	C.{0:yy}-{1:00000}	2001-01-01 00:00:00	0
4	Consignment	\N	{1:00000}	2001-01-01 00:00:00	0
5	Order	\N	O.{0:yy}-{1:00000}	2001-01-01 00:00:00	0
6	Receipt	\N	R.{0:yy}-{1:00000}	2001-01-01 00:00:00	0
7	OpeningBalance	\N	OB.{0:yy}-{1:00000}	2001-01-01 00:00:00	0
8	InternalTransfer	\N	IT.{0:yy}-{1:00000}	2001-01-01 00:00:00	0
\.


--
-- Data for Name: lt_system_configuration; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_system_configuration (id, version, modifiedby, modifiedon, companydetails, amadeusrizusingmode, vatrate, usedefaultcurrencyforinput, ispassengerpassportrequired, aviaorderitemgenerationoption, separatedocumentaccess, allowagentsetordervat, useaviadocumentvatinorder, aviadocumentvatoptions, incomingcashordercorrespondentaccount, accountantdisplaystring, useaviahandling, isorganizationcoderequired, daysbeforedeparture, company, birthdaytaskresponsible, defaultcurrency, country, isorderrequiredforprocesseddocument) FROM stdin;
c48dd52e372711dfab3816a0e28d1ada	6	sergey.buturlakin	2011-07-16 12:47:41	\N	0	20.00000	f	f	2	f	t	t	1	3613.1	Сапа В.В.	f	f	3	a354460c7cfc42d0bdf1e7624018905a	\N	af496e4b4de647afa7b61eacfd1a6f4f	c6597a0f59204b00b63fe1c0f121df7e	t
\.


--
-- Data for Name: lt_system_shutdown; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_system_shutdown (id, createdby, createdon, note, launchplannedon) FROM stdin;
\.


--
-- Data for Name: lt_system_variables; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_system_variables (id, version, modifiedby, modifiedon, birthdaytasktimestamp) FROM stdin;
c9581a4a372711dfab3816a0e28d1ada	52	SYSTEM	2012-03-06 21:18:42	2012-03-06 21:18:42
\.


--
-- Data for Name: lt_task; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_task (id, version, createdby, createdon, modifiedby, modifiedon, number_, subject, description, status, duedate, relatedto, order_, assignedto) FROM stdin;
\.


--
-- Data for Name: lt_task_comment; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_task_comment (id, version, createdby, createdon, modifiedby, modifiedon, text, task) FROM stdin;
\.


--
-- Data for Name: lt_user; Type: TABLE DATA; Schema: test; Owner: -
--

COPY lt_user (id, createdby, createdon, modifiedby, modifiedon, password, active, isadministrator, issupervisor, isagent, isanalyst, iscashier, issubagent, person, sessionid) FROM stdin;
042863cee672491ebc1c21e7b57e5bb6	yulia.sosnina	2010-04-16 10:12:27		2011-12-05 09:56:23	EC3464B4A46C762AB9FBBC82749E5F71	t	f	f	t	f	f	f	9c47021f082c4e5b9e38ee71d6dd8fa3	57d6280527e54c9c8b9e16c1be351e62
6dec9784a3ef4ff29a847c8394b0fc65	yulia.sosnina	2010-03-24 12:01:15		2011-12-05 15:32:46	4BA51C045A51BE6783A4570D786D5D92	t	f	t	t	f	f	f	5cbde1ec38d94566adec7f5a3a0d7cc1	01fddd9addf5426fbfe628308ecbcb98
3953546ed10742dd8aa28ee378a28892	viarosh	2012-03-03 14:14:33	svetlana.vasilkova	2012-05-15 16:39:04	21232F297A57A5A743894A0E4A801FC3	t	t	f	f	f	f	f	8bd45630832e411d9a7cda47d018a251	4dc8dd4f34af4d779dc1a0ac702ea2c2
51e57a1d645b43c2b12d17c424a86500	yulia.sosnina	2010-04-16 10:14:15		2012-05-31 00:22:58	BE526C9A0A18DFE30F79F913E23E97AB	t	f	f	t	f	f	f	d52cb22e2e754178afb8184957fbaba8	d6118580f44347709e5b9f9932c6553d
\.


--
-- Name: lt_airline_commission_percents_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_airline_commission_percents
    ADD CONSTRAINT lt_airline_commission_percents_pkey PRIMARY KEY (id);


--
-- Name: lt_airline_iatacode_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_airline
    ADD CONSTRAINT lt_airline_iatacode_key UNIQUE (iatacode);


--
-- Name: lt_airline_name_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_airline
    ADD CONSTRAINT lt_airline_name_key UNIQUE (name);


--
-- Name: lt_airline_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_airline
    ADD CONSTRAINT lt_airline_pkey PRIMARY KEY (id);


--
-- Name: lt_airline_prefixcode_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_airline
    ADD CONSTRAINT lt_airline_prefixcode_key UNIQUE (prefixcode);


--
-- Name: lt_airline_service_class_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_airline_service_class
    ADD CONSTRAINT lt_airline_service_class_pkey PRIMARY KEY (id);


--
-- Name: lt_airport_code_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_airport
    ADD CONSTRAINT lt_airport_code_key UNIQUE (code);


--
-- Name: lt_airport_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_airport
    ADD CONSTRAINT lt_airport_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_document_fee_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_avia_document_fee
    ADD CONSTRAINT lt_avia_document_fee_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_document_number__type_airlineprefixcode_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT lt_avia_document_number__type_airlineprefixcode_key UNIQUE (number_, type, airlineprefixcode);


--
-- Name: lt_avia_document_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT lt_avia_document_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_document_voiding_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_avia_document_voiding
    ADD CONSTRAINT lt_avia_document_voiding_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_mco_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_avia_mco
    ADD CONSTRAINT lt_avia_mco_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_refund_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT lt_avia_refund_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_ticket_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_avia_ticket
    ADD CONSTRAINT lt_avia_ticket_pkey PRIMARY KEY (id);


--
-- Name: lt_closed_period_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_closed_period
    ADD CONSTRAINT lt_closed_period_pkey PRIMARY KEY (id);


--
-- Name: lt_consignment_number__key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT lt_consignment_number__key UNIQUE (number_);


--
-- Name: lt_consignment_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT lt_consignment_pkey PRIMARY KEY (id);


--
-- Name: lt_country_name_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_country
    ADD CONSTRAINT lt_country_name_key UNIQUE (name);


--
-- Name: lt_country_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_country
    ADD CONSTRAINT lt_country_pkey PRIMARY KEY (id);


--
-- Name: lt_country_threecharcode_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_country
    ADD CONSTRAINT lt_country_threecharcode_key UNIQUE (threecharcode);


--
-- Name: lt_country_twocharcode_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_country
    ADD CONSTRAINT lt_country_twocharcode_key UNIQUE (twocharcode);


--
-- Name: lt_currency_code_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_currency
    ADD CONSTRAINT lt_currency_code_key UNIQUE (code);


--
-- Name: lt_currency_cyrilliccode_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_currency
    ADD CONSTRAINT lt_currency_cyrilliccode_key UNIQUE (cyrilliccode);


--
-- Name: lt_currency_numericcode_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_currency
    ADD CONSTRAINT lt_currency_numericcode_key UNIQUE (numericcode);


--
-- Name: lt_currency_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_currency
    ADD CONSTRAINT lt_currency_pkey PRIMARY KEY (id);


--
-- Name: lt_department_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_department
    ADD CONSTRAINT lt_department_pkey PRIMARY KEY (id);


--
-- Name: lt_document_access_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_document_access
    ADD CONSTRAINT lt_document_access_pkey PRIMARY KEY (id);


--
-- Name: lt_document_owner_owner_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_document_owner
    ADD CONSTRAINT lt_document_owner_owner_key UNIQUE (owner);


--
-- Name: lt_document_owner_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_document_owner
    ADD CONSTRAINT lt_document_owner_pkey PRIMARY KEY (id);


--
-- Name: lt_file_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_file
    ADD CONSTRAINT lt_file_pkey PRIMARY KEY (id);


--
-- Name: lt_flight_segment_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT lt_flight_segment_pkey PRIMARY KEY (id);


--
-- Name: lt_gds_agent_origin_code_officecode_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_gds_agent
    ADD CONSTRAINT lt_gds_agent_origin_code_officecode_key UNIQUE (origin, code, officecode);


--
-- Name: lt_gds_agent_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_gds_agent
    ADD CONSTRAINT lt_gds_agent_pkey PRIMARY KEY (id);


--
-- Name: lt_gds_file_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_gds_file
    ADD CONSTRAINT lt_gds_file_pkey PRIMARY KEY (id);


--
-- Name: lt_identity_name_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_identity
    ADD CONSTRAINT lt_identity_name_key UNIQUE (name);


--
-- Name: lt_identity_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_identity
    ADD CONSTRAINT lt_identity_pkey PRIMARY KEY (id);


--
-- Name: lt_internal_identity_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_internal_identity
    ADD CONSTRAINT lt_internal_identity_pkey PRIMARY KEY (id);


--
-- Name: lt_internal_transfer_number__key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT lt_internal_transfer_number__key UNIQUE (number_);


--
-- Name: lt_internal_transfer_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT lt_internal_transfer_pkey PRIMARY KEY (id);


--
-- Name: lt_invoice_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_invoice
    ADD CONSTRAINT lt_invoice_pkey PRIMARY KEY (id);


--
-- Name: lt_issued_consignment_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_issued_consignment
    ADD CONSTRAINT lt_issued_consignment_pkey PRIMARY KEY (id);


--
-- Name: lt_miles_card_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_miles_card
    ADD CONSTRAINT lt_miles_card_pkey PRIMARY KEY (id);


--
-- Name: lt_modification_items_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_modification_items
    ADD CONSTRAINT lt_modification_items_pkey PRIMARY KEY (modification, property);


--
-- Name: lt_modification_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_modification
    ADD CONSTRAINT lt_modification_pkey PRIMARY KEY (id);


--
-- Name: lt_opening_balance_date__party_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_opening_balance
    ADD CONSTRAINT lt_opening_balance_date__party_key UNIQUE (date_, party);


--
-- Name: lt_opening_balance_number__key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_opening_balance
    ADD CONSTRAINT lt_opening_balance_number__key UNIQUE (number_);


--
-- Name: lt_opening_balance_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_opening_balance
    ADD CONSTRAINT lt_opening_balance_pkey PRIMARY KEY (id);


--
-- Name: lt_order_item_avia_link_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_order_item_avia_link
    ADD CONSTRAINT lt_order_item_avia_link_pkey PRIMARY KEY (id);


--
-- Name: lt_order_item_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT lt_order_item_pkey PRIMARY KEY (id);


--
-- Name: lt_order_item_source_link_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_order_item_source_link
    ADD CONSTRAINT lt_order_item_source_link_pkey PRIMARY KEY (id);


--
-- Name: lt_order_number__key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT lt_order_number__key UNIQUE (number_);


--
-- Name: lt_order_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT lt_order_pkey PRIMARY KEY (id);


--
-- Name: lt_order_status_record_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_order_status_record
    ADD CONSTRAINT lt_order_status_record_pkey PRIMARY KEY (id);


--
-- Name: lt_organization_code_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_organization
    ADD CONSTRAINT lt_organization_code_key UNIQUE (code);


--
-- Name: lt_organization_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_organization
    ADD CONSTRAINT lt_organization_pkey PRIMARY KEY (id);


--
-- Name: lt_party_name_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_party
    ADD CONSTRAINT lt_party_name_key UNIQUE (name);


--
-- Name: lt_party_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_party
    ADD CONSTRAINT lt_party_pkey PRIMARY KEY (id);


--
-- Name: lt_passport_number__issuedby_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_passport
    ADD CONSTRAINT lt_passport_number__issuedby_key UNIQUE (number_, issuedby);


--
-- Name: lt_passport_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_passport
    ADD CONSTRAINT lt_passport_pkey PRIMARY KEY (id);


--
-- Name: lt_payment_documentuniquecode_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT lt_payment_documentuniquecode_key UNIQUE (documentuniquecode);


--
-- Name: lt_payment_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT lt_payment_pkey PRIMARY KEY (id);


--
-- Name: lt_payment_system_name_key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_payment_system
    ADD CONSTRAINT lt_payment_system_name_key UNIQUE (name);


--
-- Name: lt_payment_system_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_payment_system
    ADD CONSTRAINT lt_payment_system_pkey PRIMARY KEY (id);


--
-- Name: lt_penalize_operation_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_penalize_operation
    ADD CONSTRAINT lt_penalize_operation_pkey PRIMARY KEY (id);


--
-- Name: lt_person_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_person
    ADD CONSTRAINT lt_person_pkey PRIMARY KEY (id);


--
-- Name: lt_preferences_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_preferences
    ADD CONSTRAINT lt_preferences_pkey PRIMARY KEY (id);


--
-- Name: lt_sequence_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_sequence
    ADD CONSTRAINT lt_sequence_pkey PRIMARY KEY (id);


--
-- Name: lt_system_configuration_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_system_configuration
    ADD CONSTRAINT lt_system_configuration_pkey PRIMARY KEY (id);


--
-- Name: lt_system_shutdown_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_system_shutdown
    ADD CONSTRAINT lt_system_shutdown_pkey PRIMARY KEY (id);


--
-- Name: lt_system_variables_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_system_variables
    ADD CONSTRAINT lt_system_variables_pkey PRIMARY KEY (id);


--
-- Name: lt_task_comment_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_task_comment
    ADD CONSTRAINT lt_task_comment_pkey PRIMARY KEY (id);


--
-- Name: lt_task_number__key; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_task
    ADD CONSTRAINT lt_task_number__key UNIQUE (number_);


--
-- Name: lt_task_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_task
    ADD CONSTRAINT lt_task_pkey PRIMARY KEY (id);


--
-- Name: lt_user_pkey; Type: CONSTRAINT; Schema: test; Owner: -; Tablespace: 
--

ALTER TABLE ONLY lt_user
    ADD CONSTRAINT lt_user_pkey PRIMARY KEY (id);


--
-- Name: airline_organization_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX airline_organization_idx ON lt_airline USING btree (organization);


--
-- Name: airlinecommissionpercents_airline_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX airlinecommissionpercents_airline_idx ON lt_airline_commission_percents USING btree (airline);


--
-- Name: airlineserviceclass_airline_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX airlineserviceclass_airline_idx ON lt_airline_service_class USING btree (airline);


--
-- Name: airport_country_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX airport_country_idx ON lt_airport USING btree (country);


--
-- Name: aviadocument_airline_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_airline_idx ON lt_avia_document USING btree (airline);


--
-- Name: aviadocument_booker_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_booker_idx ON lt_avia_document USING btree (booker);


--
-- Name: aviadocument_commission_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_commission_currency_idx ON lt_avia_document USING btree (commission_currency);


--
-- Name: aviadocument_commissiondiscount_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_commissiondiscount_currency_idx ON lt_avia_document USING btree (commissiondiscount_currency);


--
-- Name: aviadocument_customer_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_customer_idx ON lt_avia_document USING btree (customer);


--
-- Name: aviadocument_discount_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_discount_currency_idx ON lt_avia_document USING btree (discount_currency);


--
-- Name: aviadocument_equalfare_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_equalfare_currency_idx ON lt_avia_document USING btree (equalfare_currency);


--
-- Name: aviadocument_fare_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_fare_currency_idx ON lt_avia_document USING btree (fare_currency);


--
-- Name: aviadocument_feestotal_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_feestotal_currency_idx ON lt_avia_document USING btree (feestotal_currency);


--
-- Name: aviadocument_grandtotal_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_grandtotal_currency_idx ON lt_avia_document USING btree (grandtotal_currency);


--
-- Name: aviadocument_handling_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_handling_currency_idx ON lt_avia_document USING btree (handling_currency);


--
-- Name: aviadocument_intermediary_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_intermediary_idx ON lt_avia_document USING btree (intermediary);


--
-- Name: aviadocument_issuedate_displaystring_owner_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_issuedate_displaystring_owner_idx ON lt_avia_document USING btree (issuedate DESC, displaystring DESC, owner DESC);


--
-- Name: aviadocument_order_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_order_idx ON lt_avia_document USING btree (order_);


--
-- Name: aviadocument_originaldocument_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_originaldocument_idx ON lt_avia_document USING btree (originaldocument);


--
-- Name: aviadocument_owner_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_owner_idx ON lt_avia_document USING btree (owner);


--
-- Name: aviadocument_owner_issuedate_displaystring_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_owner_issuedate_displaystring_idx ON lt_avia_document USING btree (owner DESC, issuedate DESC, displaystring DESC);


--
-- Name: aviadocument_passenger_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_passenger_idx ON lt_avia_document USING btree (passenger);


--
-- Name: aviadocument_seller_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_seller_idx ON lt_avia_document USING btree (seller);


--
-- Name: aviadocument_servicefee_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_servicefee_currency_idx ON lt_avia_document USING btree (servicefee_currency);


--
-- Name: aviadocument_ticketer_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_ticketer_idx ON lt_avia_document USING btree (ticketer);


--
-- Name: aviadocument_total_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_total_currency_idx ON lt_avia_document USING btree (total_currency);


--
-- Name: aviadocument_vat_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocument_vat_currency_idx ON lt_avia_document USING btree (vat_currency);


--
-- Name: aviadocumentfee_amount_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocumentfee_amount_currency_idx ON lt_avia_document_fee USING btree (amount_currency);


--
-- Name: aviadocumentfee_document_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocumentfee_document_idx ON lt_avia_document_fee USING btree (document);


--
-- Name: aviadocumentvoiding_agent_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocumentvoiding_agent_idx ON lt_avia_document_voiding USING btree (agent);


--
-- Name: aviadocumentvoiding_document_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviadocumentvoiding_document_idx ON lt_avia_document_voiding USING btree (document);


--
-- Name: aviamco_reissuefor_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviamco_reissuefor_idx ON lt_avia_mco USING btree (reissuefor);


--
-- Name: aviarefund_cancelcommission_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviarefund_cancelcommission_currency_idx ON lt_avia_refund USING btree (cancelcommission_currency);


--
-- Name: aviarefund_cancelfee_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviarefund_cancelfee_currency_idx ON lt_avia_refund USING btree (cancelfee_currency);


--
-- Name: aviarefund_refundeddocument_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviarefund_refundeddocument_idx ON lt_avia_refund USING btree (refundeddocument);


--
-- Name: aviarefund_refundservicefee_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviarefund_refundservicefee_currency_idx ON lt_avia_refund USING btree (refundservicefee_currency);


--
-- Name: aviarefund_servicefeepenalty_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviarefund_servicefeepenalty_currency_idx ON lt_avia_refund USING btree (servicefeepenalty_currency);


--
-- Name: aviaticket_reissuefor_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX aviaticket_reissuefor_idx ON lt_avia_ticket USING btree (reissuefor);


--
-- Name: consignment_acquirer_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX consignment_acquirer_idx ON lt_consignment USING btree (acquirer);


--
-- Name: consignment_discount_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX consignment_discount_currency_idx ON lt_consignment USING btree (discount_currency);


--
-- Name: consignment_grandtotal_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX consignment_grandtotal_currency_idx ON lt_consignment USING btree (grandtotal_currency);


--
-- Name: consignment_supplier_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX consignment_supplier_idx ON lt_consignment USING btree (supplier);


--
-- Name: consignment_vat_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX consignment_vat_currency_idx ON lt_consignment USING btree (vat_currency);


--
-- Name: department_organization_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX department_organization_idx ON lt_department USING btree (organization);


--
-- Name: documentaccess_owner_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX documentaccess_owner_idx ON lt_document_access USING btree (owner);


--
-- Name: documentaccess_person_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX documentaccess_person_idx ON lt_document_access USING btree (person);


--
-- Name: documentowner_owner_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX documentowner_owner_idx ON lt_document_owner USING btree (owner);


--
-- Name: electronicpayment_paymentsystem_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX electronicpayment_paymentsystem_idx ON lt_payment USING btree (paymentsystem);


--
-- Name: file_party_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX file_party_idx ON lt_file USING btree (party);


--
-- Name: file_uploadedby_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX file_uploadedby_idx ON lt_file USING btree (uploadedby);


--
-- Name: flightsegment_carrier_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX flightsegment_carrier_idx ON lt_flight_segment USING btree (carrier);


--
-- Name: flightsegment_fromairport_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX flightsegment_fromairport_idx ON lt_flight_segment USING btree (fromairport);


--
-- Name: flightsegment_ticket_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX flightsegment_ticket_idx ON lt_flight_segment USING btree (ticket);


--
-- Name: flightsegment_toairport_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX flightsegment_toairport_idx ON lt_flight_segment USING btree (toairport);


--
-- Name: gdsagent_office_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX gdsagent_office_idx ON lt_gds_agent USING btree (office);


--
-- Name: gdsagent_person_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX gdsagent_person_idx ON lt_gds_agent USING btree (person);


--
-- Name: gdsfile_timestamp_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX gdsfile_timestamp_idx ON lt_gds_file USING btree ("timestamp" DESC);


--
-- Name: internaltransfer_fromorder_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX internaltransfer_fromorder_idx ON lt_internal_transfer USING btree (fromorder);


--
-- Name: internaltransfer_fromparty_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX internaltransfer_fromparty_idx ON lt_internal_transfer USING btree (fromparty);


--
-- Name: internaltransfer_toorder_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX internaltransfer_toorder_idx ON lt_internal_transfer USING btree (toorder);


--
-- Name: internaltransfer_toparty_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX internaltransfer_toparty_idx ON lt_internal_transfer USING btree (toparty);


--
-- Name: invoice_issuedby_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX invoice_issuedby_idx ON lt_invoice USING btree (issuedby);


--
-- Name: invoice_order_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX invoice_order_idx ON lt_invoice USING btree (order_);


--
-- Name: invoice_total_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX invoice_total_currency_idx ON lt_invoice USING btree (total_currency);


--
-- Name: issuedconsignment_consignment_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX issuedconsignment_consignment_idx ON lt_issued_consignment USING btree (consignment);


--
-- Name: issuedconsignment_issuedby_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX issuedconsignment_issuedby_idx ON lt_issued_consignment USING btree (issuedby);


--
-- Name: milescard_organization_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX milescard_organization_idx ON lt_miles_card USING btree (organization);


--
-- Name: milescard_owner_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX milescard_owner_idx ON lt_miles_card USING btree (owner);


--
-- Name: modification_author_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX modification_author_idx ON lt_modification USING btree (author);


--
-- Name: modification_instanceid_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX modification_instanceid_idx ON lt_modification USING btree (instanceid);


--
-- Name: modification_instancestring_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX modification_instancestring_idx ON lt_modification USING btree (instancestring);


--
-- Name: modification_instancetype_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX modification_instancetype_idx ON lt_modification USING btree (instancetype);


--
-- Name: modification_timestamp_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX modification_timestamp_idx ON lt_modification USING btree ("timestamp");


--
-- Name: openingbalance_party_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX openingbalance_party_idx ON lt_opening_balance USING btree (party);


--
-- Name: order_assignedto_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_assignedto_idx ON lt_order USING btree (assignedto);


--
-- Name: order_billto_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_billto_idx ON lt_order USING btree (billto);


--
-- Name: order_customer_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_customer_idx ON lt_order USING btree (customer);


--
-- Name: order_discount_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_discount_currency_idx ON lt_order USING btree (discount_currency);


--
-- Name: order_owner_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_owner_idx ON lt_order USING btree (owner);


--
-- Name: order_paid_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_paid_currency_idx ON lt_order USING btree (paid_currency);


--
-- Name: order_shipto_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_shipto_idx ON lt_order USING btree (shipto);


--
-- Name: order_total_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_total_currency_idx ON lt_order USING btree (total_currency);


--
-- Name: order_totaldue_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_totaldue_currency_idx ON lt_order USING btree (totaldue_currency);


--
-- Name: order_vat_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_vat_currency_idx ON lt_order USING btree (vat_currency);


--
-- Name: order_vatdue_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX order_vatdue_currency_idx ON lt_order USING btree (vatdue_currency);


--
-- Name: orderitem_consignment_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX orderitem_consignment_idx ON lt_order_item USING btree (consignment);


--
-- Name: orderitem_discount_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX orderitem_discount_currency_idx ON lt_order_item USING btree (discount_currency);


--
-- Name: orderitem_givenvat_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX orderitem_givenvat_currency_idx ON lt_order_item USING btree (givenvat_currency);


--
-- Name: orderitem_grandtotal_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX orderitem_grandtotal_currency_idx ON lt_order_item USING btree (grandtotal_currency);


--
-- Name: orderitem_order_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX orderitem_order_idx ON lt_order_item USING btree (order_);


--
-- Name: orderitem_price_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX orderitem_price_currency_idx ON lt_order_item USING btree (price_currency);


--
-- Name: orderitem_taxedtotal_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX orderitem_taxedtotal_currency_idx ON lt_order_item USING btree (taxedtotal_currency);


--
-- Name: orderitemavialink_document_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX orderitemavialink_document_idx ON lt_order_item_avia_link USING btree (document);


--
-- Name: orderstatusrecord_changedby_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX orderstatusrecord_changedby_idx ON lt_order_status_record USING btree (changedby);


--
-- Name: orderstatusrecord_order_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX orderstatusrecord_order_idx ON lt_order_status_record USING btree (order_);


--
-- Name: party_reportsto_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX party_reportsto_idx ON lt_party USING btree (reportsto);


--
-- Name: passport_citizenship_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX passport_citizenship_idx ON lt_passport USING btree (citizenship);


--
-- Name: passport_issuedby_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX passport_issuedby_idx ON lt_passport USING btree (issuedby);


--
-- Name: passport_owner_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX passport_owner_idx ON lt_passport USING btree (owner);


--
-- Name: payment_amount_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_amount_currency_idx ON lt_payment USING btree (amount_currency);


--
-- Name: payment_assignedto_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_assignedto_idx ON lt_payment USING btree (assignedto);


--
-- Name: payment_date_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_date_idx ON lt_payment USING btree (date_);


--
-- Name: payment_documentnumber_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_documentnumber_idx ON lt_payment USING btree (documentnumber);


--
-- Name: payment_invoice_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_invoice_idx ON lt_payment USING btree (invoice);


--
-- Name: payment_number_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_number_idx ON lt_payment USING btree (number_);


--
-- Name: payment_order_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_order_idx ON lt_payment USING btree (order_);


--
-- Name: payment_owner_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_owner_idx ON lt_payment USING btree (owner);


--
-- Name: payment_payer_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_payer_idx ON lt_payment USING btree (payer);


--
-- Name: payment_receivedfrom_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_receivedfrom_idx ON lt_payment USING btree (receivedfrom);


--
-- Name: payment_registeredby_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_registeredby_idx ON lt_payment USING btree (registeredby);


--
-- Name: payment_vat_currency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX payment_vat_currency_idx ON lt_payment USING btree (vat_currency);


--
-- Name: penalizeoperation_ticket_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX penalizeoperation_ticket_idx ON lt_penalize_operation USING btree (ticket);


--
-- Name: person_organization_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX person_organization_idx ON lt_person USING btree (organization);


--
-- Name: preferences_identity_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX preferences_identity_idx ON lt_preferences USING btree (identity);


--
-- Name: systemconfiguration_birthdaytaskresponsible_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX systemconfiguration_birthdaytaskresponsible_idx ON lt_system_configuration USING btree (birthdaytaskresponsible);


--
-- Name: systemconfiguration_company_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX systemconfiguration_company_idx ON lt_system_configuration USING btree (company);


--
-- Name: systemconfiguration_country_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX systemconfiguration_country_idx ON lt_system_configuration USING btree (country);


--
-- Name: systemconfiguration_defaultcurrency_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX systemconfiguration_defaultcurrency_idx ON lt_system_configuration USING btree (defaultcurrency);


--
-- Name: task_assignedto_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX task_assignedto_idx ON lt_task USING btree (assignedto);


--
-- Name: task_order_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX task_order_idx ON lt_task USING btree (order_);


--
-- Name: task_relatedto_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX task_relatedto_idx ON lt_task USING btree (relatedto);


--
-- Name: taskcomment_task_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX taskcomment_task_idx ON lt_task_comment USING btree (task);


--
-- Name: user_person_idx; Type: INDEX; Schema: test; Owner: -; Tablespace: 
--

CREATE INDEX user_person_idx ON lt_user USING btree (person);


--
-- Name: airline_organization_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_airline
    ADD CONSTRAINT airline_organization_fkey FOREIGN KEY (organization) REFERENCES lt_organization(id);


--
-- Name: airlinecommissionpercents_airline_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_airline_commission_percents
    ADD CONSTRAINT airlinecommissionpercents_airline_fkey FOREIGN KEY (airline) REFERENCES lt_airline(id);


--
-- Name: airlineserviceclass_airline_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_airline_service_class
    ADD CONSTRAINT airlineserviceclass_airline_fkey FOREIGN KEY (airline) REFERENCES lt_airline(id);


--
-- Name: airport_country_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_airport
    ADD CONSTRAINT airport_country_fkey FOREIGN KEY (country) REFERENCES lt_country(id);


--
-- Name: aviadocument_airline_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_airline_fkey FOREIGN KEY (airline) REFERENCES lt_airline(id);


--
-- Name: aviadocument_booker_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_booker_fkey FOREIGN KEY (booker) REFERENCES lt_person(id);


--
-- Name: aviadocument_commission_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_commission_currency_fk FOREIGN KEY (commission_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_commissiondiscount_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_commissiondiscount_currency_fk FOREIGN KEY (commissiondiscount_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_customer_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_customer_fkey FOREIGN KEY (customer) REFERENCES lt_party(id);


--
-- Name: aviadocument_discount_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_discount_currency_fk FOREIGN KEY (discount_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_equalfare_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_equalfare_currency_fk FOREIGN KEY (equalfare_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_fare_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_fare_currency_fk FOREIGN KEY (fare_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_feestotal_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_feestotal_currency_fk FOREIGN KEY (feestotal_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_grandtotal_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_grandtotal_currency_fk FOREIGN KEY (grandtotal_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_handling_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_handling_currency_fk FOREIGN KEY (handling_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_intermediary_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_intermediary_fkey FOREIGN KEY (intermediary) REFERENCES lt_party(id);


--
-- Name: aviadocument_order_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: aviadocument_originaldocument_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_originaldocument_fkey FOREIGN KEY (originaldocument) REFERENCES lt_gds_file(id);


--
-- Name: aviadocument_owner_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_owner_fkey FOREIGN KEY (owner) REFERENCES lt_party(id);


--
-- Name: aviadocument_passenger_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_passenger_fkey FOREIGN KEY (passenger) REFERENCES lt_person(id);


--
-- Name: aviadocument_seller_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_seller_fkey FOREIGN KEY (seller) REFERENCES lt_person(id);


--
-- Name: aviadocument_servicefee_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_servicefee_currency_fk FOREIGN KEY (servicefee_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_ticketer_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_ticketer_fkey FOREIGN KEY (ticketer) REFERENCES lt_person(id);


--
-- Name: aviadocument_total_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_total_currency_fk FOREIGN KEY (total_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_vat_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_vat_currency_fk FOREIGN KEY (vat_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocumentfee_amount_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document_fee
    ADD CONSTRAINT aviadocumentfee_amount_currency_fk FOREIGN KEY (amount_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocumentfee_document_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document_fee
    ADD CONSTRAINT aviadocumentfee_document_fkey FOREIGN KEY (document) REFERENCES lt_avia_document(id);


--
-- Name: aviadocumentvoiding_agent_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document_voiding
    ADD CONSTRAINT aviadocumentvoiding_agent_fkey FOREIGN KEY (agent) REFERENCES lt_person(id);


--
-- Name: aviadocumentvoiding_document_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_document_voiding
    ADD CONSTRAINT aviadocumentvoiding_document_fkey FOREIGN KEY (document) REFERENCES lt_avia_document(id);


--
-- Name: aviamco_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_mco
    ADD CONSTRAINT aviamco_fkey FOREIGN KEY (id) REFERENCES lt_avia_document(id);


--
-- Name: aviamco_reissuefor_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_mco
    ADD CONSTRAINT aviamco_reissuefor_fkey FOREIGN KEY (reissuefor) REFERENCES lt_avia_mco(id);


--
-- Name: aviarefund_cancelcommission_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_cancelcommission_currency_fk FOREIGN KEY (cancelcommission_currency) REFERENCES lt_currency(id);


--
-- Name: aviarefund_cancelfee_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_cancelfee_currency_fk FOREIGN KEY (cancelfee_currency) REFERENCES lt_currency(id);


--
-- Name: aviarefund_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_fkey FOREIGN KEY (id) REFERENCES lt_avia_document(id);


--
-- Name: aviarefund_refundeddocument_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_refundeddocument_fkey FOREIGN KEY (refundeddocument) REFERENCES lt_avia_document(id);


--
-- Name: aviarefund_refundservicefee_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_refundservicefee_currency_fk FOREIGN KEY (refundservicefee_currency) REFERENCES lt_currency(id);


--
-- Name: aviarefund_servicefeepenalty_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_servicefeepenalty_currency_fk FOREIGN KEY (servicefeepenalty_currency) REFERENCES lt_currency(id);


--
-- Name: aviaticket_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_ticket
    ADD CONSTRAINT aviaticket_fkey FOREIGN KEY (id) REFERENCES lt_avia_document(id);


--
-- Name: aviaticket_reissuefor_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_avia_ticket
    ADD CONSTRAINT aviaticket_reissuefor_fkey FOREIGN KEY (reissuefor) REFERENCES lt_avia_ticket(id);


--
-- Name: consignment_acquirer_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT consignment_acquirer_fkey FOREIGN KEY (acquirer) REFERENCES lt_party(id);


--
-- Name: consignment_discount_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT consignment_discount_currency_fk FOREIGN KEY (discount_currency) REFERENCES lt_currency(id);


--
-- Name: consignment_grandtotal_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT consignment_grandtotal_currency_fk FOREIGN KEY (grandtotal_currency) REFERENCES lt_currency(id);


--
-- Name: consignment_supplier_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT consignment_supplier_fkey FOREIGN KEY (supplier) REFERENCES lt_party(id);


--
-- Name: consignment_vat_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT consignment_vat_currency_fk FOREIGN KEY (vat_currency) REFERENCES lt_currency(id);


--
-- Name: department_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_department
    ADD CONSTRAINT department_fkey FOREIGN KEY (id) REFERENCES lt_party(id);


--
-- Name: department_organization_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_department
    ADD CONSTRAINT department_organization_fkey FOREIGN KEY (organization) REFERENCES lt_organization(id);


--
-- Name: documentaccess_owner_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_document_access
    ADD CONSTRAINT documentaccess_owner_fkey FOREIGN KEY (owner) REFERENCES lt_party(id);


--
-- Name: documentaccess_person_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_document_access
    ADD CONSTRAINT documentaccess_person_fkey FOREIGN KEY (person) REFERENCES lt_person(id);


--
-- Name: documentowner_owner_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_document_owner
    ADD CONSTRAINT documentowner_owner_fkey FOREIGN KEY (owner) REFERENCES lt_party(id);


--
-- Name: electronicpayment_paymentsystem_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT electronicpayment_paymentsystem_fkey FOREIGN KEY (paymentsystem) REFERENCES lt_payment_system(id);


--
-- Name: file_party_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_file
    ADD CONSTRAINT file_party_fkey FOREIGN KEY (party) REFERENCES lt_party(id);


--
-- Name: file_uploadedby_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_file
    ADD CONSTRAINT file_uploadedby_fkey FOREIGN KEY (uploadedby) REFERENCES lt_person(id);


--
-- Name: flightsegment_carrier_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT flightsegment_carrier_fkey FOREIGN KEY (carrier) REFERENCES lt_airline(id);


--
-- Name: flightsegment_fromairport_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT flightsegment_fromairport_fkey FOREIGN KEY (fromairport) REFERENCES lt_airport(id);


--
-- Name: flightsegment_ticket_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT flightsegment_ticket_fkey FOREIGN KEY (ticket) REFERENCES lt_avia_ticket(id);


--
-- Name: flightsegment_toairport_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT flightsegment_toairport_fkey FOREIGN KEY (toairport) REFERENCES lt_airport(id);


--
-- Name: gdsagent_office_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_gds_agent
    ADD CONSTRAINT gdsagent_office_fkey FOREIGN KEY (office) REFERENCES lt_party(id);


--
-- Name: gdsagent_person_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_gds_agent
    ADD CONSTRAINT gdsagent_person_fkey FOREIGN KEY (person) REFERENCES lt_person(id);


--
-- Name: internalidentity_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_internal_identity
    ADD CONSTRAINT internalidentity_fkey FOREIGN KEY (id) REFERENCES lt_identity(id);


--
-- Name: internaltransfer_fromorder_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT internaltransfer_fromorder_fkey FOREIGN KEY (fromorder) REFERENCES lt_order(id);


--
-- Name: internaltransfer_fromparty_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT internaltransfer_fromparty_fkey FOREIGN KEY (fromparty) REFERENCES lt_party(id);


--
-- Name: internaltransfer_toorder_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT internaltransfer_toorder_fkey FOREIGN KEY (toorder) REFERENCES lt_order(id);


--
-- Name: internaltransfer_toparty_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT internaltransfer_toparty_fkey FOREIGN KEY (toparty) REFERENCES lt_party(id);


--
-- Name: invoice_issuedby_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_invoice
    ADD CONSTRAINT invoice_issuedby_fkey FOREIGN KEY (issuedby) REFERENCES lt_person(id);


--
-- Name: invoice_order_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_invoice
    ADD CONSTRAINT invoice_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: invoice_total_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_invoice
    ADD CONSTRAINT invoice_total_currency_fk FOREIGN KEY (total_currency) REFERENCES lt_currency(id);


--
-- Name: issuedconsignment_consignment_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_issued_consignment
    ADD CONSTRAINT issuedconsignment_consignment_fkey FOREIGN KEY (consignment) REFERENCES lt_consignment(id);


--
-- Name: issuedconsignment_issuedby_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_issued_consignment
    ADD CONSTRAINT issuedconsignment_issuedby_fkey FOREIGN KEY (issuedby) REFERENCES lt_person(id);


--
-- Name: milescard_organization_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_miles_card
    ADD CONSTRAINT milescard_organization_fkey FOREIGN KEY (organization) REFERENCES lt_organization(id);


--
-- Name: milescard_owner_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_miles_card
    ADD CONSTRAINT milescard_owner_fkey FOREIGN KEY (owner) REFERENCES lt_person(id);


--
-- Name: openingbalance_party_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_opening_balance
    ADD CONSTRAINT openingbalance_party_fkey FOREIGN KEY (party) REFERENCES lt_party(id);


--
-- Name: order_assignedto_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_assignedto_fkey FOREIGN KEY (assignedto) REFERENCES lt_person(id);


--
-- Name: order_billto_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_billto_fkey FOREIGN KEY (billto) REFERENCES lt_party(id);


--
-- Name: order_customer_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_customer_fkey FOREIGN KEY (customer) REFERENCES lt_party(id);


--
-- Name: order_discount_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_discount_currency_fk FOREIGN KEY (discount_currency) REFERENCES lt_currency(id);


--
-- Name: order_owner_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_owner_fkey FOREIGN KEY (owner) REFERENCES lt_party(id);


--
-- Name: order_paid_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_paid_currency_fk FOREIGN KEY (paid_currency) REFERENCES lt_currency(id);


--
-- Name: order_shipto_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_shipto_fkey FOREIGN KEY (shipto) REFERENCES lt_party(id);


--
-- Name: order_total_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_total_currency_fk FOREIGN KEY (total_currency) REFERENCES lt_currency(id);


--
-- Name: order_totaldue_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_totaldue_currency_fk FOREIGN KEY (totaldue_currency) REFERENCES lt_currency(id);


--
-- Name: order_vat_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_vat_currency_fk FOREIGN KEY (vat_currency) REFERENCES lt_currency(id);


--
-- Name: order_vatdue_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_vatdue_currency_fk FOREIGN KEY (vatdue_currency) REFERENCES lt_currency(id);


--
-- Name: orderitem_consignment_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_consignment_fkey FOREIGN KEY (consignment) REFERENCES lt_consignment(id);


--
-- Name: orderitem_discount_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_discount_currency_fk FOREIGN KEY (discount_currency) REFERENCES lt_currency(id);


--
-- Name: orderitem_givenvat_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_givenvat_currency_fk FOREIGN KEY (givenvat_currency) REFERENCES lt_currency(id);


--
-- Name: orderitem_grandtotal_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_grandtotal_currency_fk FOREIGN KEY (grandtotal_currency) REFERENCES lt_currency(id);


--
-- Name: orderitem_order_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: orderitem_price_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_price_currency_fk FOREIGN KEY (price_currency) REFERENCES lt_currency(id);


--
-- Name: orderitem_taxedtotal_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_taxedtotal_currency_fk FOREIGN KEY (taxedtotal_currency) REFERENCES lt_currency(id);


--
-- Name: orderitemavialink_document_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_item_avia_link
    ADD CONSTRAINT orderitemavialink_document_fkey FOREIGN KEY (document) REFERENCES lt_avia_document(id);


--
-- Name: orderitemavialink_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_item_avia_link
    ADD CONSTRAINT orderitemavialink_fkey FOREIGN KEY (id) REFERENCES lt_order_item_source_link(id);


--
-- Name: orderitemsourcelink_orderitem_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_item_source_link
    ADD CONSTRAINT orderitemsourcelink_orderitem_fkey FOREIGN KEY (id) REFERENCES lt_order_item(id);


--
-- Name: orderstatusrecord_changedby_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_status_record
    ADD CONSTRAINT orderstatusrecord_changedby_fkey FOREIGN KEY (changedby) REFERENCES lt_person(id);


--
-- Name: orderstatusrecord_order_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_order_status_record
    ADD CONSTRAINT orderstatusrecord_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: organization_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_organization
    ADD CONSTRAINT organization_fkey FOREIGN KEY (id) REFERENCES lt_party(id);


--
-- Name: party_reportsto_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_party
    ADD CONSTRAINT party_reportsto_fkey FOREIGN KEY (reportsto) REFERENCES lt_party(id);


--
-- Name: passport_citizenship_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_passport
    ADD CONSTRAINT passport_citizenship_fkey FOREIGN KEY (citizenship) REFERENCES lt_country(id);


--
-- Name: passport_issuedby_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_passport
    ADD CONSTRAINT passport_issuedby_fkey FOREIGN KEY (issuedby) REFERENCES lt_country(id);


--
-- Name: passport_owner_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_passport
    ADD CONSTRAINT passport_owner_fkey FOREIGN KEY (owner) REFERENCES lt_person(id);


--
-- Name: payment_amount_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_amount_currency_fk FOREIGN KEY (amount_currency) REFERENCES lt_currency(id);


--
-- Name: payment_assignedto_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_assignedto_fkey FOREIGN KEY (assignedto) REFERENCES lt_person(id);


--
-- Name: payment_invoice_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_invoice_fkey FOREIGN KEY (invoice) REFERENCES lt_invoice(id);


--
-- Name: payment_order_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: payment_owner_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_owner_fkey FOREIGN KEY (owner) REFERENCES lt_party(id);


--
-- Name: payment_payer_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_payer_fkey FOREIGN KEY (payer) REFERENCES lt_party(id);


--
-- Name: payment_registeredby_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_registeredby_fkey FOREIGN KEY (registeredby) REFERENCES lt_person(id);


--
-- Name: payment_vat_currency_fk; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_vat_currency_fk FOREIGN KEY (vat_currency) REFERENCES lt_currency(id);


--
-- Name: penalizeoperation_ticket_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_penalize_operation
    ADD CONSTRAINT penalizeoperation_ticket_fkey FOREIGN KEY (ticket) REFERENCES lt_avia_ticket(id);


--
-- Name: person_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_person
    ADD CONSTRAINT person_fkey FOREIGN KEY (id) REFERENCES lt_party(id);


--
-- Name: person_organization_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_person
    ADD CONSTRAINT person_organization_fkey FOREIGN KEY (organization) REFERENCES lt_organization(id);


--
-- Name: preferences_identity_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_preferences
    ADD CONSTRAINT preferences_identity_fkey FOREIGN KEY (identity) REFERENCES lt_identity(id);


--
-- Name: systemconfiguration_birthdaytaskresponsible_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_system_configuration
    ADD CONSTRAINT systemconfiguration_birthdaytaskresponsible_fkey FOREIGN KEY (birthdaytaskresponsible) REFERENCES lt_person(id);


--
-- Name: systemconfiguration_company_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_system_configuration
    ADD CONSTRAINT systemconfiguration_company_fkey FOREIGN KEY (company) REFERENCES lt_organization(id);


--
-- Name: systemconfiguration_country_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_system_configuration
    ADD CONSTRAINT systemconfiguration_country_fkey FOREIGN KEY (country) REFERENCES lt_country(id);


--
-- Name: systemconfiguration_defaultcurrency_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_system_configuration
    ADD CONSTRAINT systemconfiguration_defaultcurrency_fkey FOREIGN KEY (defaultcurrency) REFERENCES lt_currency(id);


--
-- Name: task_assignedto_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_task
    ADD CONSTRAINT task_assignedto_fkey FOREIGN KEY (assignedto) REFERENCES lt_party(id);


--
-- Name: task_order_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_task
    ADD CONSTRAINT task_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: task_relatedto_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_task
    ADD CONSTRAINT task_relatedto_fkey FOREIGN KEY (relatedto) REFERENCES lt_party(id);


--
-- Name: taskcomment_task_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_task_comment
    ADD CONSTRAINT taskcomment_task_fkey FOREIGN KEY (task) REFERENCES lt_task(id);


--
-- Name: user_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_user
    ADD CONSTRAINT user_fkey FOREIGN KEY (id) REFERENCES lt_identity(id);


--
-- Name: user_person_fkey; Type: FK CONSTRAINT; Schema: test; Owner: -
--

ALTER TABLE ONLY lt_user
    ADD CONSTRAINT user_person_fkey FOREIGN KEY (person) REFERENCES lt_person(id);


--
-- PostgreSQL database dump complete
--

