--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- Name: travel; Type: SCHEMA; Schema: -; Owner: travel
--

CREATE SCHEMA travel;


ALTER SCHEMA travel OWNER TO travel;

SET search_path = travel, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: lt_airline; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_airline (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    name public.citext2 NOT NULL,
    iatacode public.citext2,
    prefixcode public.citext2,
    organization character varying(32),
    passportrequirement integer NOT NULL
);


ALTER TABLE travel.lt_airline OWNER TO travel;

--
-- Name: lt_airline_commission_percents; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_airline_commission_percents (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    airline character varying(32),
    domestic numeric(19,5) NOT NULL,
    international numeric(19,5) NOT NULL,
    interlinedomestic numeric(19,5) NOT NULL,
    interlineinternational numeric(19,5) NOT NULL
);


ALTER TABLE travel.lt_airline_commission_percents OWNER TO travel;

--
-- Name: lt_airline_service_class; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_airline_service_class (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    airline character varying(32) NOT NULL,
    code public.citext2 NOT NULL,
    serviceclass integer NOT NULL
);


ALTER TABLE travel.lt_airline_service_class OWNER TO travel;

--
-- Name: lt_airport; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_airport (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    code public.citext2 NOT NULL,
    country character varying(32),
    settlement public.citext2,
    localizedsettlement public.citext2,
    name public.citext2,
    latitude double precision,
    longitude double precision
);


ALTER TABLE travel.lt_airport OWNER TO travel;

--
-- Name: lt_avia_document; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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
    airline character varying(32),
    number_ bigint,
    conjunctionnumbers public.citext2,
    isprocessed boolean NOT NULL,
    isvoid boolean NOT NULL,
    requiresprocessing boolean NOT NULL,
    passengername public.citext2,
    passenger character varying(32),
    gdspassportstatus integer NOT NULL,
    gdspassport public.citext2,
    itinerary public.citext2,
    fare_currency character varying(32),
    fare_amount numeric(19,5),
    equalfare_currency character varying(32),
    equalfare_amount numeric(19,5),
    commissionpercent numeric(19,5),
    commission_currency character varying(32),
    commission_amount numeric(19,5),
    commissiondiscount_currency character varying(32),
    commissiondiscount_amount numeric(19,5),
    feestotal_currency character varying(32),
    feestotal_amount numeric(19,5),
    vat_currency character varying(32),
    vat_amount numeric(19,5),
    total_currency character varying(32),
    total_amount numeric(19,5),
    servicefee_currency character varying(32),
    servicefee_amount numeric(19,5),
    handling_currency character varying(32),
    handling_amount numeric(19,5),
    discount_currency character varying(32),
    discount_amount numeric(19,5),
    grandtotal_currency character varying(32),
    grandtotal_amount numeric(19,5),
    paymentform public.citext2,
    paymenttype integer NOT NULL,
    paymentdetails public.citext2,
    bookeroffice public.citext2,
    bookercode public.citext2,
    booker character varying(32),
    ticketeroffice public.citext2,
    ticketercode public.citext2,
    ticketer character varying(32),
    ticketingiataoffice public.citext2,
    isticketerrobot boolean,
    seller character varying(32),
    owner character varying(32),
    intermediary character varying(32),
    customer character varying(32),
    order_ character varying(32),
    originator integer NOT NULL,
    origin integer NOT NULL,
    originaldocument character varying(32),
    pnrcode public.citext2,
    airlinepnrcode public.citext2,
    tourcode public.citext2,
    note public.citext2,
    remarks public.citext2,
    displaystring public.citext2
);


ALTER TABLE travel.lt_avia_document OWNER TO travel;

--
-- Name: lt_avia_document_fee; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_avia_document_fee (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    document character varying(32),
    code public.citext2 NOT NULL,
    amount_currency character varying(32),
    amount_amount numeric(19,5),
    createdon timestamp without time zone NOT NULL,
    createdby public.citext2 NOT NULL,
    modifiedon timestamp without time zone,
    modifiedby public.citext2
);


ALTER TABLE travel.lt_avia_document_fee OWNER TO travel;

--
-- Name: lt_avia_document_voiding; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_avia_document_voiding (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    document character varying(32) NOT NULL,
    isvoid boolean NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    agentoffice public.citext2,
    agentcode public.citext2,
    agent character varying(32),
    iataoffice public.citext2,
    createdon timestamp without time zone NOT NULL,
    createdby public.citext2 NOT NULL,
    modifiedon timestamp without time zone,
    modifiedby public.citext2
);


ALTER TABLE travel.lt_avia_document_voiding OWNER TO travel;

--
-- Name: lt_avia_mco; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_avia_mco (
    id character varying(32) NOT NULL,
    description public.citext2,
    reissuefor character varying(32),
    inconnectionwith character varying(32)
);


ALTER TABLE travel.lt_avia_mco OWNER TO travel;

--
-- Name: lt_avia_refund; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_avia_refund (
    id character varying(32) NOT NULL,
    refundeddocument character varying(32),
    cancelfee_currency character varying(32),
    cancelfee_amount numeric(19,5),
    cancelcommissionpercent numeric(19,5),
    cancelcommission_currency character varying(32),
    cancelcommission_amount numeric(19,5),
    servicefeepenalty_currency character varying(32),
    servicefeepenalty_amount numeric(19,5),
    refundservicefee_currency character varying(32),
    refundservicefee_amount numeric(19,5)
);


ALTER TABLE travel.lt_avia_refund OWNER TO travel;

--
-- Name: lt_avia_ticket; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_avia_ticket (
    id character varying(32) NOT NULL,
    departure timestamp without time zone,
    domestic boolean NOT NULL,
    interline boolean NOT NULL,
    segmentclasses public.citext2,
    reissuefor character varying(32),
    endorsement public.citext2,
    faretotal_currency character varying(32),
    faretotal_amount numeric(19,5)
);


ALTER TABLE travel.lt_avia_ticket OWNER TO travel;

--
-- Name: lt_closed_period; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_closed_period (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    datefrom date NOT NULL,
    dateto date NOT NULL,
    periodstate integer NOT NULL
);


ALTER TABLE travel.lt_closed_period OWNER TO travel;

--
-- Name: lt_consignment; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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
    supplier character varying(32),
    acquirer character varying(32) NOT NULL,
    grandtotal_currency character varying(32),
    grandtotal_amount numeric(19,5),
    vat_currency character varying(32),
    vat_amount numeric(19,5),
    discount_currency character varying(32),
    discount_amount numeric(19,5),
    totalsupplied public.citext2
);


ALTER TABLE travel.lt_consignment OWNER TO travel;

--
-- Name: lt_country; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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


ALTER TABLE travel.lt_country OWNER TO travel;

--
-- Name: lt_currency; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_currency (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    code public.citext2 NOT NULL,
    numericcode integer NOT NULL,
    cyrilliccode public.citext2,
    name public.citext2
);


ALTER TABLE travel.lt_currency OWNER TO travel;

--
-- Name: lt_department; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_department (
    id character varying(32) NOT NULL,
    organization character varying(32) NOT NULL
);


ALTER TABLE travel.lt_department OWNER TO travel;

--
-- Name: lt_document_access; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_document_access (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    person character varying(32) NOT NULL,
    owner character varying(32),
    fulldocumentcontrol boolean NOT NULL
);


ALTER TABLE travel.lt_document_access OWNER TO travel;

--
-- Name: lt_document_owner; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_document_owner (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    owner character varying(32) NOT NULL,
    isactive boolean NOT NULL
);


ALTER TABLE travel.lt_document_owner OWNER TO travel;

--
-- Name: lt_file; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_file (
    id character varying(32) NOT NULL,
    filename public.citext2 NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    content bytea NOT NULL,
    uploadedby character varying(32),
    party character varying(32)
);


ALTER TABLE travel.lt_file OWNER TO travel;

--
-- Name: lt_flight_segment; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_flight_segment (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    ticket character varying(32) NOT NULL,
    "position" integer NOT NULL,
    type integer NOT NULL,
    fromairportcode public.citext2,
    fromairportname public.citext2,
    fromairport character varying(32),
    toairportcode public.citext2,
    toairportname public.citext2,
    toairport character varying(32),
    carrieriatacode public.citext2,
    carrierprefixcode public.citext2,
    carriername public.citext2,
    carrier character varying(32),
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
    surcharges numeric(19,5),
    isinclusive boolean NOT NULL,
    fare numeric(19,5),
    stopoverortransfercharge numeric(19,5),
    issidetrip boolean NOT NULL,
    distance double precision NOT NULL,
    amount_currency character varying(32),
    amount_amount numeric(19,5),
    createdon timestamp without time zone NOT NULL,
    createdby public.citext2 NOT NULL,
    modifiedon timestamp without time zone,
    modifiedby public.citext2
);


ALTER TABLE travel.lt_flight_segment OWNER TO travel;

--
-- Name: lt_gds_agent; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_gds_agent (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    person character varying(32) NOT NULL,
    origin integer NOT NULL,
    code public.citext2 NOT NULL,
    officecode public.citext2 NOT NULL,
    office character varying(32)
);


ALTER TABLE travel.lt_gds_agent OWNER TO travel;

--
-- Name: lt_gds_file; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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
    filetype integer NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    content public.citext2 NOT NULL,
    importresult integer NOT NULL,
    importoutput public.citext2,
    officecode public.citext2,
    officeiata public.citext2,
    filepath public.citext2,
    username public.citext2,
    office public.citext2
);


ALTER TABLE travel.lt_gds_file OWNER TO travel;

--
-- Name: lt_identity; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_identity (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    name public.citext2 NOT NULL
);


ALTER TABLE travel.lt_identity OWNER TO travel;

--
-- Name: lt_internal_identity; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_internal_identity (
    id character varying(32) NOT NULL,
    description public.citext2
);


ALTER TABLE travel.lt_internal_identity OWNER TO travel;

--
-- Name: lt_internal_transfer; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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


ALTER TABLE travel.lt_internal_transfer OWNER TO travel;

--
-- Name: lt_invoice; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_invoice (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    number_ public.citext2 NOT NULL,
    agreement public.citext2,
    issuedate date NOT NULL,
    "timestamp" timestamp without time zone NOT NULL,
    type integer NOT NULL,
    content bytea NOT NULL,
    order_ character varying(32),
    issuedby character varying(32),
    total_currency character varying(32),
    total_amount numeric(19,5)
);


ALTER TABLE travel.lt_invoice OWNER TO travel;

--
-- Name: lt_issued_consignment; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_issued_consignment (
    id character varying(32) NOT NULL,
    number_ public.citext2,
    "timestamp" timestamp without time zone NOT NULL,
    content bytea NOT NULL,
    consignment character varying(32),
    issuedby character varying(32)
);


ALTER TABLE travel.lt_issued_consignment OWNER TO travel;

--
-- Name: lt_miles_card; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_miles_card (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    owner character varying(32) NOT NULL,
    number_ public.citext2 NOT NULL,
    organization character varying(32)
);


ALTER TABLE travel.lt_miles_card OWNER TO travel;

--
-- Name: lt_modification; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_modification (
    id integer NOT NULL,
    "timestamp" timestamp without time zone,
    author public.citext2,
    type integer,
    instancetype public.citext2,
    instanceid character varying(32),
    instancestring public.citext2,
    comment_ public.citext2
);


ALTER TABLE travel.lt_modification OWNER TO travel;

--
-- Name: lt_modification_id_seq; Type: SEQUENCE; Schema: travel; Owner: travel
--

CREATE SEQUENCE lt_modification_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE travel.lt_modification_id_seq OWNER TO travel;

--
-- Name: lt_modification_id_seq; Type: SEQUENCE OWNED BY; Schema: travel; Owner: travel
--

ALTER SEQUENCE lt_modification_id_seq OWNED BY lt_modification.id;


--
-- Name: lt_modification_items; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_modification_items (
    modification integer NOT NULL,
    oldvalue character varying(255),
    property character varying(255) NOT NULL
);


ALTER TABLE travel.lt_modification_items OWNER TO travel;

--
-- Name: lt_opening_balance; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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
    party character varying(32) NOT NULL,
    balance numeric(19,5) NOT NULL
);


ALTER TABLE travel.lt_opening_balance OWNER TO travel;

--
-- Name: lt_order; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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
    isvoid boolean NOT NULL,
    customer character varying(32) NOT NULL,
    billto character varying(32),
    shipto character varying(32),
    discount_currency character varying(32),
    discount_amount numeric(19,5),
    vat_currency character varying(32),
    vat_amount numeric(19,5),
    total_currency character varying(32),
    total_amount numeric(19,5),
    paid_currency character varying(32),
    paid_amount numeric(19,5),
    totaldue_currency character varying(32),
    totaldue_amount numeric(19,5),
    ispaid boolean NOT NULL,
    vatdue_currency character varying(32),
    vatdue_amount numeric(19,5),
    deliverybalance numeric(19,5) NOT NULL,
    assignedto character varying(32),
    owner character varying(32),
    note public.citext2,
    ispublic boolean NOT NULL,
    issubjectofpaymentscontrol boolean NOT NULL
);


ALTER TABLE travel.lt_order OWNER TO travel;

--
-- Name: lt_order_item; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_order_item (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    order_ character varying(32) NOT NULL,
    "position" integer NOT NULL,
    text public.citext2 NOT NULL,
    price_currency character varying(32),
    price_amount numeric(19,5),
    quantity integer NOT NULL,
    discount_currency character varying(32),
    discount_amount numeric(19,5),
    grandtotal_currency character varying(32),
    grandtotal_amount numeric(19,5),
    givenvat_currency character varying(32),
    givenvat_amount numeric(19,5),
    taxedtotal_currency character varying(32),
    taxedtotal_amount numeric(19,5),
    hasvat boolean NOT NULL,
    consignment character varying(32),
    createdon timestamp without time zone NOT NULL,
    createdby public.citext2 NOT NULL,
    modifiedon timestamp without time zone,
    modifiedby public.citext2
);


ALTER TABLE travel.lt_order_item OWNER TO travel;

--
-- Name: lt_order_item_avia_link; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_order_item_avia_link (
    id character varying(32) NOT NULL,
    linktype integer NOT NULL,
    document character varying(32)
);


ALTER TABLE travel.lt_order_item_avia_link OWNER TO travel;

--
-- Name: lt_order_item_source_link; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_order_item_source_link (
    id character varying(32) NOT NULL
);


ALTER TABLE travel.lt_order_item_source_link OWNER TO travel;

--
-- Name: lt_organization; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_organization (
    id character varying(32) NOT NULL,
    code public.citext2
);


ALTER TABLE travel.lt_organization OWNER TO travel;

--
-- Name: lt_party; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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
    reportsto character varying(32),
    legaladdress public.citext2,
    actualaddress public.citext2,
    note public.citext2
);


ALTER TABLE travel.lt_party OWNER TO travel;

--
-- Name: lt_passport; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_passport (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    owner character varying(32) NOT NULL,
    number_ public.citext2 NOT NULL,
    firstname public.citext2,
    middlename public.citext2,
    lastname public.citext2,
    citizenship character varying(32),
    birthday date,
    gender integer,
    issuedby character varying(32),
    expiredon date,
    note public.citext2
);


ALTER TABLE travel.lt_passport OWNER TO travel;

--
-- Name: lt_payment; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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
    date_ date NOT NULL,
    paymentform integer NOT NULL,
    documentnumber public.citext2,
    documentuniquecode public.citext2,
    payer character varying(32) NOT NULL,
    order_ character varying(32),
    invoice character varying(32),
    amount_currency character varying(32),
    amount_amount numeric(19,5),
    vat_currency character varying(32),
    vat_amount numeric(19,5),
    receivedfrom public.citext2,
    postedon timestamp without time zone,
    note public.citext2,
    isvoid boolean NOT NULL,
    assignedto character varying(32) NOT NULL,
    registeredby character varying(32) NOT NULL,
    owner character varying(32),
    printeddocument bytea,
    authorizationcode public.citext2,
    paymentsystem character varying(32)
);


ALTER TABLE travel.lt_payment OWNER TO travel;

--
-- Name: lt_payment_system; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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


ALTER TABLE travel.lt_payment_system OWNER TO travel;

--
-- Name: lt_penalize_operation; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_penalize_operation (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    modifiedby public.citext2,
    modifiedon timestamp without time zone,
    ticket character varying(32),
    type integer,
    status integer,
    description public.citext2
);


ALTER TABLE travel.lt_penalize_operation OWNER TO travel;

--
-- Name: lt_person; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_person (
    id character varying(32) NOT NULL,
    milescardsstring public.citext2,
    birthday date,
    organization character varying(32),
    title public.citext2
);


ALTER TABLE travel.lt_person OWNER TO travel;

--
-- Name: lt_preferences; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_preferences (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    identity character varying(32) NOT NULL,
    createdon timestamp without time zone NOT NULL,
    createdby public.citext2 NOT NULL,
    modifiedon timestamp without time zone,
    modifiedby public.citext2
);


ALTER TABLE travel.lt_preferences OWNER TO travel;

--
-- Name: lt_sequence; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_sequence (
    id integer NOT NULL,
    name public.citext2 NOT NULL,
    discriminator public.citext2,
    current bigint NOT NULL,
    format public.citext2 NOT NULL,
    "timestamp" timestamp without time zone NOT NULL
);


ALTER TABLE travel.lt_sequence OWNER TO travel;

--
-- Name: lt_system_configuration; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_system_configuration (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    modifiedon timestamp without time zone,
    modifiedby public.citext2,
    company character varying(32),
    companydetails public.citext2,
    country character varying(32),
    defaultcurrency character varying(32) NOT NULL,
    usedefaultcurrencyforinput boolean NOT NULL,
    vatrate numeric(19,5) NOT NULL,
    amadeusrizusingmode integer NOT NULL,
    ispassengerpassportrequired boolean NOT NULL,
    aviaorderitemgenerationoption integer NOT NULL,
    allowagentsetordervat boolean NOT NULL,
    useaviadocumentvatinorder boolean NOT NULL,
    aviadocumentvatoptions integer NOT NULL,
    accountantdisplaystring public.citext2,
    incomingcashordercorrespondentaccount public.citext2 NOT NULL,
    separatedocumentaccess boolean NOT NULL,
    isorganizationcoderequired boolean NOT NULL,
    useaviahandling boolean NOT NULL,
    daysbeforedeparture integer NOT NULL,
    birthdaytaskresponsible character varying(32),
    isorderrequiredforprocesseddocument boolean NOT NULL,
    metricsfromdate date,
    reservationsinofficemetrics boolean NOT NULL,
    mcorequiresdescription boolean NOT NULL
);


ALTER TABLE travel.lt_system_configuration OWNER TO travel;

--
-- Name: lt_system_shutdown; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_system_shutdown (
    id character varying(32) NOT NULL,
    createdby public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    launchplannedon timestamp without time zone NOT NULL,
    note public.citext2 NOT NULL
);


ALTER TABLE travel.lt_system_shutdown OWNER TO travel;

--
-- Name: lt_system_variables; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_system_variables (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    modifiedon timestamp without time zone,
    modifiedby public.citext2,
    birthdaytasktimestamp timestamp without time zone NOT NULL
);


ALTER TABLE travel.lt_system_variables OWNER TO travel;

--
-- Name: lt_task; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
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
    relatedto character varying(32),
    order_ character varying(32),
    assignedto character varying(32),
    status integer NOT NULL,
    duedate date
);


ALTER TABLE travel.lt_task OWNER TO travel;

--
-- Name: lt_task_comment; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_task_comment (
    id character varying(32) NOT NULL,
    version integer NOT NULL,
    task character varying(32) NOT NULL,
    text public.citext2 NOT NULL,
    createdon timestamp without time zone NOT NULL,
    createdby public.citext2 NOT NULL,
    modifiedon timestamp without time zone,
    modifiedby public.citext2
);


ALTER TABLE travel.lt_task_comment OWNER TO travel;

--
-- Name: lt_user; Type: TABLE; Schema: travel; Owner: travel; Tablespace: 
--

CREATE TABLE lt_user (
    id character varying(32) NOT NULL,
    createdon timestamp without time zone NOT NULL,
    createdby public.citext2 NOT NULL,
    modifiedon timestamp without time zone,
    modifiedby public.citext2,
    person character varying(32) NOT NULL,
    password public.citext2 NOT NULL,
    active boolean NOT NULL,
    issupervisor boolean NOT NULL,
    isadministrator boolean NOT NULL,
    isagent boolean NOT NULL,
    iscashier boolean NOT NULL,
    isanalyst boolean NOT NULL,
    issubagent boolean NOT NULL,
    sessionid public.citext2
);


ALTER TABLE travel.lt_user OWNER TO travel;

--
-- Name: id; Type: DEFAULT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_modification ALTER COLUMN id SET DEFAULT nextval('lt_modification_id_seq'::regclass);


--
-- Name: lt_airline_commission_percents_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_airline_commission_percents
    ADD CONSTRAINT lt_airline_commission_percents_pkey PRIMARY KEY (id);


--
-- Name: lt_airline_iatacode_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_airline
    ADD CONSTRAINT lt_airline_iatacode_key UNIQUE (iatacode);


--
-- Name: lt_airline_name_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_airline
    ADD CONSTRAINT lt_airline_name_key UNIQUE (name);


--
-- Name: lt_airline_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_airline
    ADD CONSTRAINT lt_airline_pkey PRIMARY KEY (id);


--
-- Name: lt_airline_prefixcode_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_airline
    ADD CONSTRAINT lt_airline_prefixcode_key UNIQUE (prefixcode);


--
-- Name: lt_airline_service_class_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_airline_service_class
    ADD CONSTRAINT lt_airline_service_class_pkey PRIMARY KEY (id);


--
-- Name: lt_airport_code_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_airport
    ADD CONSTRAINT lt_airport_code_key UNIQUE (code);


--
-- Name: lt_airport_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_airport
    ADD CONSTRAINT lt_airport_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_document_fee_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_avia_document_fee
    ADD CONSTRAINT lt_avia_document_fee_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_document_number__type_airlineprefixcode_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT lt_avia_document_number__type_airlineprefixcode_key UNIQUE (number_, type, airlineprefixcode);


--
-- Name: lt_avia_document_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT lt_avia_document_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_document_voiding_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_avia_document_voiding
    ADD CONSTRAINT lt_avia_document_voiding_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_mco_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_avia_mco
    ADD CONSTRAINT lt_avia_mco_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_refund_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT lt_avia_refund_pkey PRIMARY KEY (id);


--
-- Name: lt_avia_ticket_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_avia_ticket
    ADD CONSTRAINT lt_avia_ticket_pkey PRIMARY KEY (id);


--
-- Name: lt_closed_period_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_closed_period
    ADD CONSTRAINT lt_closed_period_pkey PRIMARY KEY (id);


--
-- Name: lt_consignment_number__key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT lt_consignment_number__key UNIQUE (number_);


--
-- Name: lt_consignment_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT lt_consignment_pkey PRIMARY KEY (id);


--
-- Name: lt_country_name_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_country
    ADD CONSTRAINT lt_country_name_key UNIQUE (name);


--
-- Name: lt_country_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_country
    ADD CONSTRAINT lt_country_pkey PRIMARY KEY (id);


--
-- Name: lt_country_threecharcode_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_country
    ADD CONSTRAINT lt_country_threecharcode_key UNIQUE (threecharcode);


--
-- Name: lt_country_twocharcode_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_country
    ADD CONSTRAINT lt_country_twocharcode_key UNIQUE (twocharcode);


--
-- Name: lt_currency_code_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_currency
    ADD CONSTRAINT lt_currency_code_key UNIQUE (code);


--
-- Name: lt_currency_cyrilliccode_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_currency
    ADD CONSTRAINT lt_currency_cyrilliccode_key UNIQUE (cyrilliccode);


--
-- Name: lt_currency_numericcode_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_currency
    ADD CONSTRAINT lt_currency_numericcode_key UNIQUE (numericcode);


--
-- Name: lt_currency_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_currency
    ADD CONSTRAINT lt_currency_pkey PRIMARY KEY (id);


--
-- Name: lt_department_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_department
    ADD CONSTRAINT lt_department_pkey PRIMARY KEY (id);


--
-- Name: lt_document_access_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_document_access
    ADD CONSTRAINT lt_document_access_pkey PRIMARY KEY (id);


--
-- Name: lt_document_owner_owner_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_document_owner
    ADD CONSTRAINT lt_document_owner_owner_key UNIQUE (owner);


--
-- Name: lt_document_owner_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_document_owner
    ADD CONSTRAINT lt_document_owner_pkey PRIMARY KEY (id);


--
-- Name: lt_file_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_file
    ADD CONSTRAINT lt_file_pkey PRIMARY KEY (id);


--
-- Name: lt_flight_segment_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT lt_flight_segment_pkey PRIMARY KEY (id);


--
-- Name: lt_gds_agent_origin_code_officecode_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_gds_agent
    ADD CONSTRAINT lt_gds_agent_origin_code_officecode_key UNIQUE (origin, code, officecode);


--
-- Name: lt_gds_agent_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_gds_agent
    ADD CONSTRAINT lt_gds_agent_pkey PRIMARY KEY (id);


--
-- Name: lt_gds_file_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_gds_file
    ADD CONSTRAINT lt_gds_file_pkey PRIMARY KEY (id);


--
-- Name: lt_identity_name_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_identity
    ADD CONSTRAINT lt_identity_name_key UNIQUE (name);


--
-- Name: lt_identity_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_identity
    ADD CONSTRAINT lt_identity_pkey PRIMARY KEY (id);


--
-- Name: lt_internal_identity_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_internal_identity
    ADD CONSTRAINT lt_internal_identity_pkey PRIMARY KEY (id);


--
-- Name: lt_internal_transfer_number__key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT lt_internal_transfer_number__key UNIQUE (number_);


--
-- Name: lt_internal_transfer_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT lt_internal_transfer_pkey PRIMARY KEY (id);


--
-- Name: lt_invoice_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_invoice
    ADD CONSTRAINT lt_invoice_pkey PRIMARY KEY (id);


--
-- Name: lt_issued_consignment_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_issued_consignment
    ADD CONSTRAINT lt_issued_consignment_pkey PRIMARY KEY (id);


--
-- Name: lt_miles_card_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_miles_card
    ADD CONSTRAINT lt_miles_card_pkey PRIMARY KEY (id);


--
-- Name: lt_modification_items_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_modification_items
    ADD CONSTRAINT lt_modification_items_pkey PRIMARY KEY (modification, property);


--
-- Name: lt_modification_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_modification
    ADD CONSTRAINT lt_modification_pkey PRIMARY KEY (id);


--
-- Name: lt_opening_balance_date__party_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_opening_balance
    ADD CONSTRAINT lt_opening_balance_date__party_key UNIQUE (date_, party);


--
-- Name: lt_opening_balance_number__key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_opening_balance
    ADD CONSTRAINT lt_opening_balance_number__key UNIQUE (number_);


--
-- Name: lt_opening_balance_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_opening_balance
    ADD CONSTRAINT lt_opening_balance_pkey PRIMARY KEY (id);


--
-- Name: lt_order_item_avia_link_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_order_item_avia_link
    ADD CONSTRAINT lt_order_item_avia_link_pkey PRIMARY KEY (id);


--
-- Name: lt_order_item_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT lt_order_item_pkey PRIMARY KEY (id);


--
-- Name: lt_order_item_source_link_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_order_item_source_link
    ADD CONSTRAINT lt_order_item_source_link_pkey PRIMARY KEY (id);


--
-- Name: lt_order_number__key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT lt_order_number__key UNIQUE (number_);


--
-- Name: lt_order_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT lt_order_pkey PRIMARY KEY (id);


--
-- Name: lt_organization_code_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_organization
    ADD CONSTRAINT lt_organization_code_key UNIQUE (code);


--
-- Name: lt_organization_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_organization
    ADD CONSTRAINT lt_organization_pkey PRIMARY KEY (id);


--
-- Name: lt_party_name_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_party
    ADD CONSTRAINT lt_party_name_key UNIQUE (name);


--
-- Name: lt_party_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_party
    ADD CONSTRAINT lt_party_pkey PRIMARY KEY (id);


--
-- Name: lt_passport_number__issuedby_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_passport
    ADD CONSTRAINT lt_passport_number__issuedby_key UNIQUE (number_, issuedby);


--
-- Name: lt_passport_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_passport
    ADD CONSTRAINT lt_passport_pkey PRIMARY KEY (id);


--
-- Name: lt_payment_documentuniquecode_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT lt_payment_documentuniquecode_key UNIQUE (documentuniquecode);


--
-- Name: lt_payment_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT lt_payment_pkey PRIMARY KEY (id);


--
-- Name: lt_payment_system_name_key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_payment_system
    ADD CONSTRAINT lt_payment_system_name_key UNIQUE (name);


--
-- Name: lt_payment_system_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_payment_system
    ADD CONSTRAINT lt_payment_system_pkey PRIMARY KEY (id);


--
-- Name: lt_penalize_operation_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_penalize_operation
    ADD CONSTRAINT lt_penalize_operation_pkey PRIMARY KEY (id);


--
-- Name: lt_person_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_person
    ADD CONSTRAINT lt_person_pkey PRIMARY KEY (id);


--
-- Name: lt_preferences_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_preferences
    ADD CONSTRAINT lt_preferences_pkey PRIMARY KEY (id);


--
-- Name: lt_sequence_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_sequence
    ADD CONSTRAINT lt_sequence_pkey PRIMARY KEY (id);


--
-- Name: lt_system_configuration_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_system_configuration
    ADD CONSTRAINT lt_system_configuration_pkey PRIMARY KEY (id);


--
-- Name: lt_system_shutdown_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_system_shutdown
    ADD CONSTRAINT lt_system_shutdown_pkey PRIMARY KEY (id);


--
-- Name: lt_system_variables_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_system_variables
    ADD CONSTRAINT lt_system_variables_pkey PRIMARY KEY (id);


--
-- Name: lt_task_comment_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_task_comment
    ADD CONSTRAINT lt_task_comment_pkey PRIMARY KEY (id);


--
-- Name: lt_task_number__key; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_task
    ADD CONSTRAINT lt_task_number__key UNIQUE (number_);


--
-- Name: lt_task_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_task
    ADD CONSTRAINT lt_task_pkey PRIMARY KEY (id);


--
-- Name: lt_user_pkey; Type: CONSTRAINT; Schema: travel; Owner: travel; Tablespace: 
--

ALTER TABLE ONLY lt_user
    ADD CONSTRAINT lt_user_pkey PRIMARY KEY (id);


--
-- Name: airline_organization_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX airline_organization_idx ON lt_airline USING btree (organization);


--
-- Name: airlinecommissionpercents_airline_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX airlinecommissionpercents_airline_idx ON lt_airline_commission_percents USING btree (airline);


--
-- Name: airlineserviceclass_airline_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX airlineserviceclass_airline_idx ON lt_airline_service_class USING btree (airline);


--
-- Name: airport_country_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX airport_country_idx ON lt_airport USING btree (country);


--
-- Name: aviadocument_airline_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_airline_idx ON lt_avia_document USING btree (airline);


--
-- Name: aviadocument_booker_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_booker_idx ON lt_avia_document USING btree (booker);


--
-- Name: aviadocument_commission_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_commission_currency_idx ON lt_avia_document USING btree (commission_currency);


--
-- Name: aviadocument_commissiondiscount_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_commissiondiscount_currency_idx ON lt_avia_document USING btree (commissiondiscount_currency);


--
-- Name: aviadocument_customer_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_customer_idx ON lt_avia_document USING btree (customer);


--
-- Name: aviadocument_discount_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_discount_currency_idx ON lt_avia_document USING btree (discount_currency);


--
-- Name: aviadocument_equalfare_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_equalfare_currency_idx ON lt_avia_document USING btree (equalfare_currency);


--
-- Name: aviadocument_fare_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_fare_currency_idx ON lt_avia_document USING btree (fare_currency);


--
-- Name: aviadocument_feestotal_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_feestotal_currency_idx ON lt_avia_document USING btree (feestotal_currency);


--
-- Name: aviadocument_grandtotal_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_grandtotal_currency_idx ON lt_avia_document USING btree (grandtotal_currency);


--
-- Name: aviadocument_handling_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_handling_currency_idx ON lt_avia_document USING btree (handling_currency);


--
-- Name: aviadocument_intermediary_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_intermediary_idx ON lt_avia_document USING btree (intermediary);


--
-- Name: aviadocument_issuedate_displaystring_owner_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_issuedate_displaystring_owner_idx ON lt_avia_document USING btree (issuedate DESC, displaystring DESC, owner DESC);


--
-- Name: aviadocument_order_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_order_idx ON lt_avia_document USING btree (order_);


--
-- Name: aviadocument_originaldocument_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_originaldocument_idx ON lt_avia_document USING btree (originaldocument);


--
-- Name: aviadocument_owner_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_owner_idx ON lt_avia_document USING btree (owner);


--
-- Name: aviadocument_owner_issuedate_displaystring_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_owner_issuedate_displaystring_idx ON lt_avia_document USING btree (owner DESC, issuedate DESC, displaystring DESC);


--
-- Name: aviadocument_passenger_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_passenger_idx ON lt_avia_document USING btree (passenger);


--
-- Name: aviadocument_seller_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_seller_idx ON lt_avia_document USING btree (seller);


--
-- Name: aviadocument_servicefee_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_servicefee_currency_idx ON lt_avia_document USING btree (servicefee_currency);


--
-- Name: aviadocument_ticketer_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_ticketer_idx ON lt_avia_document USING btree (ticketer);


--
-- Name: aviadocument_total_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_total_currency_idx ON lt_avia_document USING btree (total_currency);


--
-- Name: aviadocument_vat_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocument_vat_currency_idx ON lt_avia_document USING btree (vat_currency);


--
-- Name: aviadocumentfee_amount_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocumentfee_amount_currency_idx ON lt_avia_document_fee USING btree (amount_currency);


--
-- Name: aviadocumentfee_document_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocumentfee_document_idx ON lt_avia_document_fee USING btree (document);


--
-- Name: aviadocumentvoiding_agent_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocumentvoiding_agent_idx ON lt_avia_document_voiding USING btree (agent);


--
-- Name: aviadocumentvoiding_document_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviadocumentvoiding_document_idx ON lt_avia_document_voiding USING btree (document);


--
-- Name: aviamco_inconnectionwith_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviamco_inconnectionwith_idx ON lt_avia_mco USING btree (inconnectionwith);


--
-- Name: aviamco_reissuefor_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviamco_reissuefor_idx ON lt_avia_mco USING btree (reissuefor);


--
-- Name: aviarefund_cancelcommission_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviarefund_cancelcommission_currency_idx ON lt_avia_refund USING btree (cancelcommission_currency);


--
-- Name: aviarefund_cancelfee_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviarefund_cancelfee_currency_idx ON lt_avia_refund USING btree (cancelfee_currency);


--
-- Name: aviarefund_refundeddocument_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviarefund_refundeddocument_idx ON lt_avia_refund USING btree (refundeddocument);


--
-- Name: aviarefund_refundservicefee_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviarefund_refundservicefee_currency_idx ON lt_avia_refund USING btree (refundservicefee_currency);


--
-- Name: aviarefund_servicefeepenalty_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviarefund_servicefeepenalty_currency_idx ON lt_avia_refund USING btree (servicefeepenalty_currency);


--
-- Name: aviaticket_faretotal_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviaticket_faretotal_currency_idx ON lt_avia_ticket USING btree (faretotal_currency);


--
-- Name: aviaticket_reissuefor_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX aviaticket_reissuefor_idx ON lt_avia_ticket USING btree (reissuefor);


--
-- Name: consignment_acquirer_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX consignment_acquirer_idx ON lt_consignment USING btree (acquirer);


--
-- Name: consignment_discount_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX consignment_discount_currency_idx ON lt_consignment USING btree (discount_currency);


--
-- Name: consignment_grandtotal_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX consignment_grandtotal_currency_idx ON lt_consignment USING btree (grandtotal_currency);


--
-- Name: consignment_supplier_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX consignment_supplier_idx ON lt_consignment USING btree (supplier);


--
-- Name: consignment_vat_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX consignment_vat_currency_idx ON lt_consignment USING btree (vat_currency);


--
-- Name: department_organization_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX department_organization_idx ON lt_department USING btree (organization);


--
-- Name: documentaccess_owner_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX documentaccess_owner_idx ON lt_document_access USING btree (owner);


--
-- Name: documentaccess_person_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX documentaccess_person_idx ON lt_document_access USING btree (person);


--
-- Name: documentowner_owner_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX documentowner_owner_idx ON lt_document_owner USING btree (owner);


--
-- Name: electronicpayment_paymentsystem_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX electronicpayment_paymentsystem_idx ON lt_payment USING btree (paymentsystem);


--
-- Name: file_party_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX file_party_idx ON lt_file USING btree (party);


--
-- Name: file_uploadedby_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX file_uploadedby_idx ON lt_file USING btree (uploadedby);


--
-- Name: flightsegment_amount_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX flightsegment_amount_currency_idx ON lt_flight_segment USING btree (amount_currency);


--
-- Name: flightsegment_carrier_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX flightsegment_carrier_idx ON lt_flight_segment USING btree (carrier);


--
-- Name: flightsegment_fromairport_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX flightsegment_fromairport_idx ON lt_flight_segment USING btree (fromairport);


--
-- Name: flightsegment_ticket_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX flightsegment_ticket_idx ON lt_flight_segment USING btree (ticket);


--
-- Name: flightsegment_toairport_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX flightsegment_toairport_idx ON lt_flight_segment USING btree (toairport);


--
-- Name: gdsagent_office_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX gdsagent_office_idx ON lt_gds_agent USING btree (office);


--
-- Name: gdsagent_person_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX gdsagent_person_idx ON lt_gds_agent USING btree (person);


--
-- Name: gdsfile_timestamp_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX gdsfile_timestamp_idx ON lt_gds_file USING btree ("timestamp" DESC);


--
-- Name: internaltransfer_fromorder_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX internaltransfer_fromorder_idx ON lt_internal_transfer USING btree (fromorder);


--
-- Name: internaltransfer_fromparty_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX internaltransfer_fromparty_idx ON lt_internal_transfer USING btree (fromparty);


--
-- Name: internaltransfer_toorder_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX internaltransfer_toorder_idx ON lt_internal_transfer USING btree (toorder);


--
-- Name: internaltransfer_toparty_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX internaltransfer_toparty_idx ON lt_internal_transfer USING btree (toparty);


--
-- Name: invoice_issuedby_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX invoice_issuedby_idx ON lt_invoice USING btree (issuedby);


--
-- Name: invoice_order_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX invoice_order_idx ON lt_invoice USING btree (order_);


--
-- Name: invoice_total_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX invoice_total_currency_idx ON lt_invoice USING btree (total_currency);


--
-- Name: issuedconsignment_consignment_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX issuedconsignment_consignment_idx ON lt_issued_consignment USING btree (consignment);


--
-- Name: issuedconsignment_issuedby_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX issuedconsignment_issuedby_idx ON lt_issued_consignment USING btree (issuedby);


--
-- Name: milescard_organization_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX milescard_organization_idx ON lt_miles_card USING btree (organization);


--
-- Name: milescard_owner_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX milescard_owner_idx ON lt_miles_card USING btree (owner);


--
-- Name: modification_author_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX modification_author_idx ON lt_modification USING btree (author);


--
-- Name: modification_instanceid_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX modification_instanceid_idx ON lt_modification USING btree (instanceid);


--
-- Name: modification_instancestring_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX modification_instancestring_idx ON lt_modification USING btree (instancestring);


--
-- Name: modification_instancetype_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX modification_instancetype_idx ON lt_modification USING btree (instancetype);


--
-- Name: modification_timestamp_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX modification_timestamp_idx ON lt_modification USING btree ("timestamp");


--
-- Name: openingbalance_party_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX openingbalance_party_idx ON lt_opening_balance USING btree (party);


--
-- Name: order_assignedto_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_assignedto_idx ON lt_order USING btree (assignedto);


--
-- Name: order_billto_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_billto_idx ON lt_order USING btree (billto);


--
-- Name: order_customer_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_customer_idx ON lt_order USING btree (customer);


--
-- Name: order_discount_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_discount_currency_idx ON lt_order USING btree (discount_currency);


--
-- Name: order_owner_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_owner_idx ON lt_order USING btree (owner);


--
-- Name: order_paid_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_paid_currency_idx ON lt_order USING btree (paid_currency);


--
-- Name: order_shipto_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_shipto_idx ON lt_order USING btree (shipto);


--
-- Name: order_total_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_total_currency_idx ON lt_order USING btree (total_currency);


--
-- Name: order_totaldue_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_totaldue_currency_idx ON lt_order USING btree (totaldue_currency);


--
-- Name: order_vat_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_vat_currency_idx ON lt_order USING btree (vat_currency);


--
-- Name: order_vatdue_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX order_vatdue_currency_idx ON lt_order USING btree (vatdue_currency);


--
-- Name: orderitem_consignment_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX orderitem_consignment_idx ON lt_order_item USING btree (consignment);


--
-- Name: orderitem_discount_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX orderitem_discount_currency_idx ON lt_order_item USING btree (discount_currency);


--
-- Name: orderitem_givenvat_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX orderitem_givenvat_currency_idx ON lt_order_item USING btree (givenvat_currency);


--
-- Name: orderitem_grandtotal_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX orderitem_grandtotal_currency_idx ON lt_order_item USING btree (grandtotal_currency);


--
-- Name: orderitem_order_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX orderitem_order_idx ON lt_order_item USING btree (order_);


--
-- Name: orderitem_price_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX orderitem_price_currency_idx ON lt_order_item USING btree (price_currency);


--
-- Name: orderitem_taxedtotal_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX orderitem_taxedtotal_currency_idx ON lt_order_item USING btree (taxedtotal_currency);


--
-- Name: orderitemavialink_document_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX orderitemavialink_document_idx ON lt_order_item_avia_link USING btree (document);


--
-- Name: party_reportsto_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX party_reportsto_idx ON lt_party USING btree (reportsto);


--
-- Name: passport_citizenship_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX passport_citizenship_idx ON lt_passport USING btree (citizenship);


--
-- Name: passport_issuedby_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX passport_issuedby_idx ON lt_passport USING btree (issuedby);


--
-- Name: passport_owner_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX passport_owner_idx ON lt_passport USING btree (owner);


--
-- Name: payment_amount_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_amount_currency_idx ON lt_payment USING btree (amount_currency);


--
-- Name: payment_assignedto_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_assignedto_idx ON lt_payment USING btree (assignedto);


--
-- Name: payment_date_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_date_idx ON lt_payment USING btree (date_);


--
-- Name: payment_documentnumber_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_documentnumber_idx ON lt_payment USING btree (documentnumber);


--
-- Name: payment_invoice_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_invoice_idx ON lt_payment USING btree (invoice);


--
-- Name: payment_number_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_number_idx ON lt_payment USING btree (number_);


--
-- Name: payment_order_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_order_idx ON lt_payment USING btree (order_);


--
-- Name: payment_owner_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_owner_idx ON lt_payment USING btree (owner);


--
-- Name: payment_payer_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_payer_idx ON lt_payment USING btree (payer);


--
-- Name: payment_receivedfrom_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_receivedfrom_idx ON lt_payment USING btree (receivedfrom);


--
-- Name: payment_registeredby_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_registeredby_idx ON lt_payment USING btree (registeredby);


--
-- Name: payment_vat_currency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX payment_vat_currency_idx ON lt_payment USING btree (vat_currency);


--
-- Name: penalizeoperation_ticket_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX penalizeoperation_ticket_idx ON lt_penalize_operation USING btree (ticket);


--
-- Name: person_organization_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX person_organization_idx ON lt_person USING btree (organization);


--
-- Name: preferences_identity_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX preferences_identity_idx ON lt_preferences USING btree (identity);


--
-- Name: systemconfiguration_birthdaytaskresponsible_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX systemconfiguration_birthdaytaskresponsible_idx ON lt_system_configuration USING btree (birthdaytaskresponsible);


--
-- Name: systemconfiguration_company_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX systemconfiguration_company_idx ON lt_system_configuration USING btree (company);


--
-- Name: systemconfiguration_country_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX systemconfiguration_country_idx ON lt_system_configuration USING btree (country);


--
-- Name: systemconfiguration_defaultcurrency_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX systemconfiguration_defaultcurrency_idx ON lt_system_configuration USING btree (defaultcurrency);


--
-- Name: task_assignedto_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX task_assignedto_idx ON lt_task USING btree (assignedto);


--
-- Name: task_order_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX task_order_idx ON lt_task USING btree (order_);


--
-- Name: task_relatedto_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX task_relatedto_idx ON lt_task USING btree (relatedto);


--
-- Name: taskcomment_task_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX taskcomment_task_idx ON lt_task_comment USING btree (task);


--
-- Name: user_person_idx; Type: INDEX; Schema: travel; Owner: travel; Tablespace: 
--

CREATE INDEX user_person_idx ON lt_user USING btree (person);


--
-- Name: airline_organization_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_airline
    ADD CONSTRAINT airline_organization_fkey FOREIGN KEY (organization) REFERENCES lt_organization(id);


--
-- Name: airlinecommissionpercents_airline_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_airline_commission_percents
    ADD CONSTRAINT airlinecommissionpercents_airline_fkey FOREIGN KEY (airline) REFERENCES lt_airline(id);


--
-- Name: airlineserviceclass_airline_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_airline_service_class
    ADD CONSTRAINT airlineserviceclass_airline_fkey FOREIGN KEY (airline) REFERENCES lt_airline(id);


--
-- Name: airport_country_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_airport
    ADD CONSTRAINT airport_country_fkey FOREIGN KEY (country) REFERENCES lt_country(id);


--
-- Name: aviadocument_airline_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_airline_fkey FOREIGN KEY (airline) REFERENCES lt_airline(id);


--
-- Name: aviadocument_booker_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_booker_fkey FOREIGN KEY (booker) REFERENCES lt_person(id);


--
-- Name: aviadocument_commission_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_commission_currency_fk FOREIGN KEY (commission_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_commissiondiscount_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_commissiondiscount_currency_fk FOREIGN KEY (commissiondiscount_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_customer_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_customer_fkey FOREIGN KEY (customer) REFERENCES lt_party(id);


--
-- Name: aviadocument_discount_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_discount_currency_fk FOREIGN KEY (discount_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_equalfare_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_equalfare_currency_fk FOREIGN KEY (equalfare_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_fare_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_fare_currency_fk FOREIGN KEY (fare_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_feestotal_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_feestotal_currency_fk FOREIGN KEY (feestotal_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_grandtotal_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_grandtotal_currency_fk FOREIGN KEY (grandtotal_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_handling_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_handling_currency_fk FOREIGN KEY (handling_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_intermediary_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_intermediary_fkey FOREIGN KEY (intermediary) REFERENCES lt_party(id);


--
-- Name: aviadocument_order_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: aviadocument_originaldocument_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_originaldocument_fkey FOREIGN KEY (originaldocument) REFERENCES lt_gds_file(id);


--
-- Name: aviadocument_owner_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_owner_fkey FOREIGN KEY (owner) REFERENCES lt_party(id);


--
-- Name: aviadocument_passenger_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_passenger_fkey FOREIGN KEY (passenger) REFERENCES lt_person(id);


--
-- Name: aviadocument_seller_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_seller_fkey FOREIGN KEY (seller) REFERENCES lt_person(id);


--
-- Name: aviadocument_servicefee_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_servicefee_currency_fk FOREIGN KEY (servicefee_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_ticketer_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_ticketer_fkey FOREIGN KEY (ticketer) REFERENCES lt_person(id);


--
-- Name: aviadocument_total_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_total_currency_fk FOREIGN KEY (total_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocument_vat_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document
    ADD CONSTRAINT aviadocument_vat_currency_fk FOREIGN KEY (vat_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocumentfee_amount_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document_fee
    ADD CONSTRAINT aviadocumentfee_amount_currency_fk FOREIGN KEY (amount_currency) REFERENCES lt_currency(id);


--
-- Name: aviadocumentfee_document_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document_fee
    ADD CONSTRAINT aviadocumentfee_document_fkey FOREIGN KEY (document) REFERENCES lt_avia_document(id);


--
-- Name: aviadocumentvoiding_agent_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document_voiding
    ADD CONSTRAINT aviadocumentvoiding_agent_fkey FOREIGN KEY (agent) REFERENCES lt_person(id);


--
-- Name: aviadocumentvoiding_document_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_document_voiding
    ADD CONSTRAINT aviadocumentvoiding_document_fkey FOREIGN KEY (document) REFERENCES lt_avia_document(id);


--
-- Name: aviamco_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_mco
    ADD CONSTRAINT aviamco_fkey FOREIGN KEY (id) REFERENCES lt_avia_document(id);


--
-- Name: aviamco_inconnectionwith_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_mco
    ADD CONSTRAINT aviamco_inconnectionwith_fkey FOREIGN KEY (inconnectionwith) REFERENCES lt_avia_document(id);


--
-- Name: aviamco_reissuefor_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_mco
    ADD CONSTRAINT aviamco_reissuefor_fkey FOREIGN KEY (reissuefor) REFERENCES lt_avia_mco(id);


--
-- Name: aviarefund_cancelcommission_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_cancelcommission_currency_fk FOREIGN KEY (cancelcommission_currency) REFERENCES lt_currency(id);


--
-- Name: aviarefund_cancelfee_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_cancelfee_currency_fk FOREIGN KEY (cancelfee_currency) REFERENCES lt_currency(id);


--
-- Name: aviarefund_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_fkey FOREIGN KEY (id) REFERENCES lt_avia_document(id);


--
-- Name: aviarefund_refundeddocument_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_refundeddocument_fkey FOREIGN KEY (refundeddocument) REFERENCES lt_avia_document(id);


--
-- Name: aviarefund_refundservicefee_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_refundservicefee_currency_fk FOREIGN KEY (refundservicefee_currency) REFERENCES lt_currency(id);


--
-- Name: aviarefund_servicefeepenalty_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_refund
    ADD CONSTRAINT aviarefund_servicefeepenalty_currency_fk FOREIGN KEY (servicefeepenalty_currency) REFERENCES lt_currency(id);


--
-- Name: aviaticket_faretotal_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_ticket
    ADD CONSTRAINT aviaticket_faretotal_currency_fk FOREIGN KEY (faretotal_currency) REFERENCES lt_currency(id);


--
-- Name: aviaticket_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_ticket
    ADD CONSTRAINT aviaticket_fkey FOREIGN KEY (id) REFERENCES lt_avia_document(id);


--
-- Name: aviaticket_reissuefor_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_avia_ticket
    ADD CONSTRAINT aviaticket_reissuefor_fkey FOREIGN KEY (reissuefor) REFERENCES lt_avia_ticket(id);


--
-- Name: consignment_acquirer_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT consignment_acquirer_fkey FOREIGN KEY (acquirer) REFERENCES lt_party(id);


--
-- Name: consignment_discount_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT consignment_discount_currency_fk FOREIGN KEY (discount_currency) REFERENCES lt_currency(id);


--
-- Name: consignment_grandtotal_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT consignment_grandtotal_currency_fk FOREIGN KEY (grandtotal_currency) REFERENCES lt_currency(id);


--
-- Name: consignment_supplier_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT consignment_supplier_fkey FOREIGN KEY (supplier) REFERENCES lt_party(id);


--
-- Name: consignment_vat_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_consignment
    ADD CONSTRAINT consignment_vat_currency_fk FOREIGN KEY (vat_currency) REFERENCES lt_currency(id);


--
-- Name: department_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_department
    ADD CONSTRAINT department_fkey FOREIGN KEY (id) REFERENCES lt_party(id);


--
-- Name: department_organization_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_department
    ADD CONSTRAINT department_organization_fkey FOREIGN KEY (organization) REFERENCES lt_organization(id);


--
-- Name: documentaccess_owner_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_document_access
    ADD CONSTRAINT documentaccess_owner_fkey FOREIGN KEY (owner) REFERENCES lt_party(id);


--
-- Name: documentaccess_person_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_document_access
    ADD CONSTRAINT documentaccess_person_fkey FOREIGN KEY (person) REFERENCES lt_person(id);


--
-- Name: documentowner_owner_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_document_owner
    ADD CONSTRAINT documentowner_owner_fkey FOREIGN KEY (owner) REFERENCES lt_party(id);


--
-- Name: electronicpayment_paymentsystem_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT electronicpayment_paymentsystem_fkey FOREIGN KEY (paymentsystem) REFERENCES lt_payment_system(id);


--
-- Name: file_party_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_file
    ADD CONSTRAINT file_party_fkey FOREIGN KEY (party) REFERENCES lt_party(id);


--
-- Name: file_uploadedby_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_file
    ADD CONSTRAINT file_uploadedby_fkey FOREIGN KEY (uploadedby) REFERENCES lt_person(id);


--
-- Name: flightsegment_amount_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT flightsegment_amount_currency_fk FOREIGN KEY (amount_currency) REFERENCES lt_currency(id);


--
-- Name: flightsegment_carrier_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT flightsegment_carrier_fkey FOREIGN KEY (carrier) REFERENCES lt_airline(id);


--
-- Name: flightsegment_fromairport_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT flightsegment_fromairport_fkey FOREIGN KEY (fromairport) REFERENCES lt_airport(id);


--
-- Name: flightsegment_ticket_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT flightsegment_ticket_fkey FOREIGN KEY (ticket) REFERENCES lt_avia_ticket(id);


--
-- Name: flightsegment_toairport_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_flight_segment
    ADD CONSTRAINT flightsegment_toairport_fkey FOREIGN KEY (toairport) REFERENCES lt_airport(id);


--
-- Name: gdsagent_office_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_gds_agent
    ADD CONSTRAINT gdsagent_office_fkey FOREIGN KEY (office) REFERENCES lt_party(id);


--
-- Name: gdsagent_person_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_gds_agent
    ADD CONSTRAINT gdsagent_person_fkey FOREIGN KEY (person) REFERENCES lt_person(id);


--
-- Name: internalidentity_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_internal_identity
    ADD CONSTRAINT internalidentity_fkey FOREIGN KEY (id) REFERENCES lt_identity(id);


--
-- Name: internaltransfer_fromorder_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT internaltransfer_fromorder_fkey FOREIGN KEY (fromorder) REFERENCES lt_order(id);


--
-- Name: internaltransfer_fromparty_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT internaltransfer_fromparty_fkey FOREIGN KEY (fromparty) REFERENCES lt_party(id);


--
-- Name: internaltransfer_toorder_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT internaltransfer_toorder_fkey FOREIGN KEY (toorder) REFERENCES lt_order(id);


--
-- Name: internaltransfer_toparty_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_internal_transfer
    ADD CONSTRAINT internaltransfer_toparty_fkey FOREIGN KEY (toparty) REFERENCES lt_party(id);


--
-- Name: invoice_issuedby_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_invoice
    ADD CONSTRAINT invoice_issuedby_fkey FOREIGN KEY (issuedby) REFERENCES lt_person(id);


--
-- Name: invoice_order_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_invoice
    ADD CONSTRAINT invoice_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: invoice_total_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_invoice
    ADD CONSTRAINT invoice_total_currency_fk FOREIGN KEY (total_currency) REFERENCES lt_currency(id);


--
-- Name: issuedconsignment_consignment_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_issued_consignment
    ADD CONSTRAINT issuedconsignment_consignment_fkey FOREIGN KEY (consignment) REFERENCES lt_consignment(id);


--
-- Name: issuedconsignment_issuedby_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_issued_consignment
    ADD CONSTRAINT issuedconsignment_issuedby_fkey FOREIGN KEY (issuedby) REFERENCES lt_person(id);


--
-- Name: milescard_organization_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_miles_card
    ADD CONSTRAINT milescard_organization_fkey FOREIGN KEY (organization) REFERENCES lt_organization(id);


--
-- Name: milescard_owner_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_miles_card
    ADD CONSTRAINT milescard_owner_fkey FOREIGN KEY (owner) REFERENCES lt_person(id);


--
-- Name: openingbalance_party_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_opening_balance
    ADD CONSTRAINT openingbalance_party_fkey FOREIGN KEY (party) REFERENCES lt_party(id);


--
-- Name: order_assignedto_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_assignedto_fkey FOREIGN KEY (assignedto) REFERENCES lt_person(id);


--
-- Name: order_billto_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_billto_fkey FOREIGN KEY (billto) REFERENCES lt_party(id);


--
-- Name: order_customer_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_customer_fkey FOREIGN KEY (customer) REFERENCES lt_party(id);


--
-- Name: order_discount_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_discount_currency_fk FOREIGN KEY (discount_currency) REFERENCES lt_currency(id);


--
-- Name: order_owner_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_owner_fkey FOREIGN KEY (owner) REFERENCES lt_party(id);


--
-- Name: order_paid_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_paid_currency_fk FOREIGN KEY (paid_currency) REFERENCES lt_currency(id);


--
-- Name: order_shipto_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_shipto_fkey FOREIGN KEY (shipto) REFERENCES lt_party(id);


--
-- Name: order_total_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_total_currency_fk FOREIGN KEY (total_currency) REFERENCES lt_currency(id);


--
-- Name: order_totaldue_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_totaldue_currency_fk FOREIGN KEY (totaldue_currency) REFERENCES lt_currency(id);


--
-- Name: order_vat_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_vat_currency_fk FOREIGN KEY (vat_currency) REFERENCES lt_currency(id);


--
-- Name: order_vatdue_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order
    ADD CONSTRAINT order_vatdue_currency_fk FOREIGN KEY (vatdue_currency) REFERENCES lt_currency(id);


--
-- Name: orderitem_consignment_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_consignment_fkey FOREIGN KEY (consignment) REFERENCES lt_consignment(id);


--
-- Name: orderitem_discount_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_discount_currency_fk FOREIGN KEY (discount_currency) REFERENCES lt_currency(id);


--
-- Name: orderitem_givenvat_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_givenvat_currency_fk FOREIGN KEY (givenvat_currency) REFERENCES lt_currency(id);


--
-- Name: orderitem_grandtotal_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_grandtotal_currency_fk FOREIGN KEY (grandtotal_currency) REFERENCES lt_currency(id);


--
-- Name: orderitem_order_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: orderitem_price_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_price_currency_fk FOREIGN KEY (price_currency) REFERENCES lt_currency(id);


--
-- Name: orderitem_taxedtotal_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order_item
    ADD CONSTRAINT orderitem_taxedtotal_currency_fk FOREIGN KEY (taxedtotal_currency) REFERENCES lt_currency(id);


--
-- Name: orderitemavialink_document_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order_item_avia_link
    ADD CONSTRAINT orderitemavialink_document_fkey FOREIGN KEY (document) REFERENCES lt_avia_document(id);


--
-- Name: orderitemavialink_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order_item_avia_link
    ADD CONSTRAINT orderitemavialink_fkey FOREIGN KEY (id) REFERENCES lt_order_item_source_link(id);


--
-- Name: orderitemsourcelink_orderitem_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_order_item_source_link
    ADD CONSTRAINT orderitemsourcelink_orderitem_fkey FOREIGN KEY (id) REFERENCES lt_order_item(id);


--
-- Name: organization_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_organization
    ADD CONSTRAINT organization_fkey FOREIGN KEY (id) REFERENCES lt_party(id);


--
-- Name: party_reportsto_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_party
    ADD CONSTRAINT party_reportsto_fkey FOREIGN KEY (reportsto) REFERENCES lt_party(id);


--
-- Name: passport_citizenship_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_passport
    ADD CONSTRAINT passport_citizenship_fkey FOREIGN KEY (citizenship) REFERENCES lt_country(id);


--
-- Name: passport_issuedby_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_passport
    ADD CONSTRAINT passport_issuedby_fkey FOREIGN KEY (issuedby) REFERENCES lt_country(id);


--
-- Name: passport_owner_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_passport
    ADD CONSTRAINT passport_owner_fkey FOREIGN KEY (owner) REFERENCES lt_person(id);


--
-- Name: payment_amount_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_amount_currency_fk FOREIGN KEY (amount_currency) REFERENCES lt_currency(id);


--
-- Name: payment_assignedto_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_assignedto_fkey FOREIGN KEY (assignedto) REFERENCES lt_person(id);


--
-- Name: payment_invoice_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_invoice_fkey FOREIGN KEY (invoice) REFERENCES lt_invoice(id);


--
-- Name: payment_order_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: payment_owner_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_owner_fkey FOREIGN KEY (owner) REFERENCES lt_party(id);


--
-- Name: payment_payer_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_payer_fkey FOREIGN KEY (payer) REFERENCES lt_party(id);


--
-- Name: payment_registeredby_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_registeredby_fkey FOREIGN KEY (registeredby) REFERENCES lt_person(id);


--
-- Name: payment_vat_currency_fk; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_payment
    ADD CONSTRAINT payment_vat_currency_fk FOREIGN KEY (vat_currency) REFERENCES lt_currency(id);


--
-- Name: penalizeoperation_ticket_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_penalize_operation
    ADD CONSTRAINT penalizeoperation_ticket_fkey FOREIGN KEY (ticket) REFERENCES lt_avia_ticket(id);


--
-- Name: person_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_person
    ADD CONSTRAINT person_fkey FOREIGN KEY (id) REFERENCES lt_party(id);


--
-- Name: person_organization_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_person
    ADD CONSTRAINT person_organization_fkey FOREIGN KEY (organization) REFERENCES lt_organization(id);


--
-- Name: preferences_identity_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_preferences
    ADD CONSTRAINT preferences_identity_fkey FOREIGN KEY (identity) REFERENCES lt_identity(id);


--
-- Name: systemconfiguration_birthdaytaskresponsible_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_system_configuration
    ADD CONSTRAINT systemconfiguration_birthdaytaskresponsible_fkey FOREIGN KEY (birthdaytaskresponsible) REFERENCES lt_person(id);


--
-- Name: systemconfiguration_company_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_system_configuration
    ADD CONSTRAINT systemconfiguration_company_fkey FOREIGN KEY (company) REFERENCES lt_organization(id);


--
-- Name: systemconfiguration_country_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_system_configuration
    ADD CONSTRAINT systemconfiguration_country_fkey FOREIGN KEY (country) REFERENCES lt_country(id);


--
-- Name: systemconfiguration_defaultcurrency_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_system_configuration
    ADD CONSTRAINT systemconfiguration_defaultcurrency_fkey FOREIGN KEY (defaultcurrency) REFERENCES lt_currency(id);


--
-- Name: task_assignedto_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_task
    ADD CONSTRAINT task_assignedto_fkey FOREIGN KEY (assignedto) REFERENCES lt_party(id);


--
-- Name: task_order_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_task
    ADD CONSTRAINT task_order_fkey FOREIGN KEY (order_) REFERENCES lt_order(id);


--
-- Name: task_relatedto_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_task
    ADD CONSTRAINT task_relatedto_fkey FOREIGN KEY (relatedto) REFERENCES lt_party(id);


--
-- Name: taskcomment_task_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_task_comment
    ADD CONSTRAINT taskcomment_task_fkey FOREIGN KEY (task) REFERENCES lt_task(id);


--
-- Name: user_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_user
    ADD CONSTRAINT user_fkey FOREIGN KEY (id) REFERENCES lt_identity(id);


--
-- Name: user_person_fkey; Type: FK CONSTRAINT; Schema: travel; Owner: travel
--

ALTER TABLE ONLY lt_user
    ADD CONSTRAINT user_person_fkey FOREIGN KEY (person) REFERENCES lt_person(id);


--
-- PostgreSQL database dump complete
--

