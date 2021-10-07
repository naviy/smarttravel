--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.4
-- Dumped by pg_dump version 9.4.4
-- Started on 2015-11-25 13:07:36

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 19 (class 2615 OID 19551)
-- Name: dictionary; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA dictionary;


--
-- TOC entry 2959 (class 0 OID 0)
-- Dependencies: 19
-- Name: SCHEMA dictionary; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA dictionary IS 'The shared dictionaries of the system';


SET search_path = dictionary, pg_catalog;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 269 (class 1259 OID 19814)
-- Name: bank; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE bank (
    id bigint NOT NULL,
    name public.property_value NOT NULL,
    code public.bank_code
);


--
-- TOC entry 2960 (class 0 OID 0)
-- Dependencies: 269
-- Name: TABLE bank; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE bank IS 'The banks dictionary';


--
-- TOC entry 270 (class 1259 OID 19820)
-- Name: bank_id_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE bank_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2961 (class 0 OID 0)
-- Dependencies: 270
-- Name: bank_id_seq; Type: SEQUENCE OWNED BY; Schema: dictionary; Owner: -
--

ALTER SEQUENCE bank_id_seq OWNED BY bank.id;


--
-- TOC entry 271 (class 1259 OID 19822)
-- Name: city; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE city (
    id bigint NOT NULL,
    country_id bigint NOT NULL,
    start_date date DEFAULT now() NOT NULL,
    end_date date
);


--
-- TOC entry 2962 (class 0 OID 0)
-- Dependencies: 271
-- Name: TABLE city; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE city IS 'The cities dictionary';


--
-- TOC entry 272 (class 1259 OID 19826)
-- Name: city_id_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE city_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2963 (class 0 OID 0)
-- Dependencies: 272
-- Name: city_id_seq; Type: SEQUENCE OWNED BY; Schema: dictionary; Owner: -
--

ALTER SEQUENCE city_id_seq OWNED BY city.id;


--
-- TOC entry 2964 (class 0 OID 0)
-- Dependencies: 272
-- Name: SEQUENCE city_id_seq; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON SEQUENCE city_id_seq IS 'The sequence for ID of cities';


--
-- TOC entry 273 (class 1259 OID 19828)
-- Name: city_property; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE city_property (
    city_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


--
-- TOC entry 2965 (class 0 OID 0)
-- Dependencies: 273
-- Name: TABLE city_property; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE city_property IS 'Some properties of city';


--
-- TOC entry 274 (class 1259 OID 19834)
-- Name: company; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE company (
    id bigint NOT NULL,
    start_date date DEFAULT now() NOT NULL,
    end_date date,
    contract_deadline date,
    parent_id bigint,
    country_id bigint NOT NULL,
    unit_id bigint
);


--
-- TOC entry 2966 (class 0 OID 0)
-- Dependencies: 274
-- Name: TABLE company; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE company IS 'The companies dictionary';


--
-- TOC entry 275 (class 1259 OID 19838)
-- Name: company_account; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE company_account (
    id bigint NOT NULL,
    company_id bigint NOT NULL,
    bank_id bigint NOT NULL,
    name public.short_name NOT NULL,
    code public.bank_account
);


--
-- TOC entry 2967 (class 0 OID 0)
-- Dependencies: 275
-- Name: TABLE company_account; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE company_account IS 'The bank accounts of company';


--
-- TOC entry 276 (class 1259 OID 19844)
-- Name: company_account_id_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE company_account_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2968 (class 0 OID 0)
-- Dependencies: 276
-- Name: company_account_id_seq; Type: SEQUENCE OWNED BY; Schema: dictionary; Owner: -
--

ALTER SEQUENCE company_account_id_seq OWNED BY company_account.id;


--
-- TOC entry 277 (class 1259 OID 19846)
-- Name: company_date; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE company_date (
    company_id bigint NOT NULL,
    description public.property_value NOT NULL,
    date date NOT NULL
);


--
-- TOC entry 2969 (class 0 OID 0)
-- Dependencies: 277
-- Name: TABLE company_date; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE company_date IS 'Some important dates of company';


--
-- TOC entry 278 (class 1259 OID 19852)
-- Name: company_id_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE company_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2970 (class 0 OID 0)
-- Dependencies: 278
-- Name: company_id_seq; Type: SEQUENCE OWNED BY; Schema: dictionary; Owner: -
--

ALTER SEQUENCE company_id_seq OWNED BY company.id;


--
-- TOC entry 2971 (class 0 OID 0)
-- Dependencies: 278
-- Name: SEQUENCE company_id_seq; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON SEQUENCE company_id_seq IS 'The sequence for ID of companies';


--
-- TOC entry 279 (class 1259 OID 19854)
-- Name: company_property; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE company_property (
    company_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


--
-- TOC entry 2972 (class 0 OID 0)
-- Dependencies: 279
-- Name: TABLE company_property; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE company_property IS 'Some properties of company';


--
-- TOC entry 280 (class 1259 OID 19860)
-- Name: competitor; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE competitor (
    id bigint NOT NULL,
    name public.short_name NOT NULL
);


--
-- TOC entry 2973 (class 0 OID 0)
-- Dependencies: 280
-- Name: TABLE competitor; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE competitor IS 'The competitors dictionary';


--
-- TOC entry 281 (class 1259 OID 19866)
-- Name: competitor_id_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE competitor_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2974 (class 0 OID 0)
-- Dependencies: 281
-- Name: competitor_id_seq; Type: SEQUENCE OWNED BY; Schema: dictionary; Owner: -
--

ALTER SEQUENCE competitor_id_seq OWNED BY competitor.id;


--
-- TOC entry 2975 (class 0 OID 0)
-- Dependencies: 281
-- Name: SEQUENCE competitor_id_seq; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON SEQUENCE competitor_id_seq IS 'The sequence for ID of competitors';


--
-- TOC entry 282 (class 1259 OID 19868)
-- Name: main_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE main_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2976 (class 0 OID 0)
-- Dependencies: 282
-- Name: SEQUENCE main_seq; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON SEQUENCE main_seq IS 'The main sequence of this schema';


--
-- TOC entry 283 (class 1259 OID 19870)
-- Name: competitor_sku; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE competitor_sku (
    id bigint DEFAULT nextval('main_seq'::regclass) NOT NULL,
    competitor_id bigint NOT NULL,
    sku_id bigint NOT NULL,
    name public.short_name NOT NULL,
    start_date date DEFAULT now() NOT NULL,
    end_date date
);


--
-- TOC entry 2977 (class 0 OID 0)
-- Dependencies: 283
-- Name: TABLE competitor_sku; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE competitor_sku IS 'The dictionary for skus of competitors';


--
-- TOC entry 284 (class 1259 OID 19878)
-- Name: consumer; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE consumer (
    id bigint NOT NULL,
    first_name public.short_name NOT NULL,
    last_name public.short_name NOT NULL,
    middle_name public.short_name,
    birthday date,
    passport public.short_name,
    zip_code public.zip_code,
    address character varying,
    email public.short_name,
    phone public.phone_number
);


--
-- TOC entry 2978 (class 0 OID 0)
-- Dependencies: 284
-- Name: TABLE consumer; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE consumer IS 'The consumers dictionary';


--
-- TOC entry 285 (class 1259 OID 19884)
-- Name: consumer_id_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE consumer_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2979 (class 0 OID 0)
-- Dependencies: 285
-- Name: consumer_id_seq; Type: SEQUENCE OWNED BY; Schema: dictionary; Owner: -
--

ALTER SEQUENCE consumer_id_seq OWNED BY consumer.id;


--
-- TOC entry 2980 (class 0 OID 0)
-- Dependencies: 285
-- Name: SEQUENCE consumer_id_seq; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON SEQUENCE consumer_id_seq IS 'The sequence for ID of consumers';


--
-- TOC entry 286 (class 1259 OID 19886)
-- Name: country; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE country (
    id bigint NOT NULL,
    start_date date DEFAULT now() NOT NULL,
    end_date date
);


--
-- TOC entry 2981 (class 0 OID 0)
-- Dependencies: 286
-- Name: TABLE country; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE country IS 'The countries dictionary';


--
-- TOC entry 287 (class 1259 OID 19890)
-- Name: country_id_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE country_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2982 (class 0 OID 0)
-- Dependencies: 287
-- Name: country_id_seq; Type: SEQUENCE OWNED BY; Schema: dictionary; Owner: -
--

ALTER SEQUENCE country_id_seq OWNED BY country.id;


--
-- TOC entry 2983 (class 0 OID 0)
-- Dependencies: 287
-- Name: SEQUENCE country_id_seq; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON SEQUENCE country_id_seq IS 'The sequence for ID of countries';


--
-- TOC entry 288 (class 1259 OID 19892)
-- Name: country_property; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE country_property (
    country_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


--
-- TOC entry 2984 (class 0 OID 0)
-- Dependencies: 288
-- Name: TABLE country_property; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE country_property IS 'Some properties of country';


--
-- TOC entry 289 (class 1259 OID 19898)
-- Name: currency; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE currency (
    id bigint NOT NULL,
    code public.iata_3 NOT NULL,
    name public.short_name
);


--
-- TOC entry 2985 (class 0 OID 0)
-- Dependencies: 289
-- Name: TABLE currency; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE currency IS 'The currencies dictionary';


--
-- TOC entry 290 (class 1259 OID 19904)
-- Name: currency_id_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE currency_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2986 (class 0 OID 0)
-- Dependencies: 290
-- Name: currency_id_seq; Type: SEQUENCE OWNED BY; Schema: dictionary; Owner: -
--

ALTER SEQUENCE currency_id_seq OWNED BY currency.id;


--
-- TOC entry 2987 (class 0 OID 0)
-- Dependencies: 290
-- Name: SEQUENCE currency_id_seq; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON SEQUENCE currency_id_seq IS 'The sequence for ID of currencies';


--
-- TOC entry 291 (class 1259 OID 19906)
-- Name: customer; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE customer (
    id bigint NOT NULL,
    company_id bigint,
    password character varying,
    email public.short_name,
    phone public.phone_number,
    created timestamp without time zone,
    updated timestamp without time zone,
    last_login timestamp without time zone,
    gender_type public.property_type DEFAULT 'MALE'::character varying NOT NULL,
    birthday date,
    address character varying,
    unit_id bigint
);


--
-- TOC entry 2988 (class 0 OID 0)
-- Dependencies: 291
-- Name: TABLE customer; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE customer IS 'The customers dictionary who made any reserve request';


--
-- TOC entry 292 (class 1259 OID 19913)
-- Name: customer_date; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE customer_date (
    customer_id bigint NOT NULL,
    description public.property_value NOT NULL,
    date date NOT NULL
);


--
-- TOC entry 2989 (class 0 OID 0)
-- Dependencies: 292
-- Name: TABLE customer_date; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE customer_date IS 'Some important dates of customer';


--
-- TOC entry 293 (class 1259 OID 19919)
-- Name: customer_document; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE customer_document (
    id public.short_name NOT NULL,
    customer_id bigint NOT NULL,
    country_id bigint NOT NULL,
    type smallint NOT NULL,
    internal boolean DEFAULT true NOT NULL,
    validity date NOT NULL,
    description character varying
);


--
-- TOC entry 2990 (class 0 OID 0)
-- Dependencies: 293
-- Name: TABLE customer_document; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE customer_document IS 'Some identification documents of customer';


--
-- TOC entry 294 (class 1259 OID 19926)
-- Name: customer_id_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE customer_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2991 (class 0 OID 0)
-- Dependencies: 294
-- Name: customer_id_seq; Type: SEQUENCE OWNED BY; Schema: dictionary; Owner: -
--

ALTER SEQUENCE customer_id_seq OWNED BY customer.id;


--
-- TOC entry 2992 (class 0 OID 0)
-- Dependencies: 294
-- Name: SEQUENCE customer_id_seq; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON SEQUENCE customer_id_seq IS 'The sequence for ID of customers';


--
-- TOC entry 295 (class 1259 OID 19928)
-- Name: customer_property; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE customer_property (
    customer_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


--
-- TOC entry 2993 (class 0 OID 0)
-- Dependencies: 295
-- Name: TABLE customer_property; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE customer_property IS 'Some properties of customer';


--
-- TOC entry 296 (class 1259 OID 19934)
-- Name: network; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE network (
    id bigint DEFAULT nextval('main_seq'::regclass) NOT NULL,
    company_id bigint NOT NULL,
    name public.short_name NOT NULL
);


--
-- TOC entry 2994 (class 0 OID 0)
-- Dependencies: 296
-- Name: TABLE network; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE network IS 'The dictionary for the trade networks';


--
-- TOC entry 297 (class 1259 OID 19941)
-- Name: shop; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE shop (
    id bigint DEFAULT nextval('main_seq'::regclass) NOT NULL,
    network_id bigint NOT NULL,
    city_id bigint NOT NULL,
    shop_format_id bigint NOT NULL,
    trade_channel_id bigint NOT NULL,
    name public.short_name NOT NULL,
    nielsen_coverage boolean DEFAULT false NOT NULL
);


--
-- TOC entry 2995 (class 0 OID 0)
-- Dependencies: 297
-- Name: TABLE shop; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE shop IS 'The dictionary of the shops';


--
-- TOC entry 298 (class 1259 OID 19949)
-- Name: shop_format; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE shop_format (
    id bigint DEFAULT nextval('main_seq'::regclass) NOT NULL,
    name public.short_name NOT NULL
);


--
-- TOC entry 2996 (class 0 OID 0)
-- Dependencies: 298
-- Name: TABLE shop_format; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE shop_format IS 'The dictionary for format of the shops';


--
-- TOC entry 299 (class 1259 OID 19956)
-- Name: sku; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE sku (
    id bigint DEFAULT nextval('main_seq'::regclass) NOT NULL,
    sector_id bigint NOT NULL,
    article character varying(20) NOT NULL,
    name public.short_name NOT NULL
);


--
-- TOC entry 2997 (class 0 OID 0)
-- Dependencies: 299
-- Name: TABLE sku; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE sku IS 'The sku''s dictionary';


--
-- TOC entry 300 (class 1259 OID 19963)
-- Name: tax; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE tax (
    id bigint NOT NULL,
    code public.short_name NOT NULL,
    name public.full_name NOT NULL
);


--
-- TOC entry 2998 (class 0 OID 0)
-- Dependencies: 300
-- Name: TABLE tax; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE tax IS 'The dictionary of taxes';


--
-- TOC entry 301 (class 1259 OID 19969)
-- Name: tax_id_seq; Type: SEQUENCE; Schema: dictionary; Owner: -
--

CREATE SEQUENCE tax_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2999 (class 0 OID 0)
-- Dependencies: 301
-- Name: tax_id_seq; Type: SEQUENCE OWNED BY; Schema: dictionary; Owner: -
--

ALTER SEQUENCE tax_id_seq OWNED BY tax.id;


--
-- TOC entry 3000 (class 0 OID 0)
-- Dependencies: 301
-- Name: SEQUENCE tax_id_seq; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON SEQUENCE tax_id_seq IS 'The sequence for ID of taxes';


--
-- TOC entry 302 (class 1259 OID 19971)
-- Name: trade_channel; Type: TABLE; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE TABLE trade_channel (
    id bigint DEFAULT nextval('main_seq'::regclass) NOT NULL,
    name public.short_name NOT NULL
);


--
-- TOC entry 3001 (class 0 OID 0)
-- Dependencies: 302
-- Name: TABLE trade_channel; Type: COMMENT; Schema: dictionary; Owner: -
--

COMMENT ON TABLE trade_channel IS 'The trade chanels dictionary';


--
-- TOC entry 2692 (class 2604 OID 20618)
-- Name: id; Type: DEFAULT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY bank ALTER COLUMN id SET DEFAULT nextval('bank_id_seq'::regclass);


--
-- TOC entry 2693 (class 2604 OID 20619)
-- Name: id; Type: DEFAULT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY city ALTER COLUMN id SET DEFAULT nextval('city_id_seq'::regclass);


--
-- TOC entry 2695 (class 2604 OID 20620)
-- Name: id; Type: DEFAULT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY company ALTER COLUMN id SET DEFAULT nextval('company_id_seq'::regclass);


--
-- TOC entry 2697 (class 2604 OID 20621)
-- Name: id; Type: DEFAULT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY company_account ALTER COLUMN id SET DEFAULT nextval('company_account_id_seq'::regclass);


--
-- TOC entry 2698 (class 2604 OID 20622)
-- Name: id; Type: DEFAULT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY competitor ALTER COLUMN id SET DEFAULT nextval('competitor_id_seq'::regclass);


--
-- TOC entry 2701 (class 2604 OID 20623)
-- Name: id; Type: DEFAULT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY consumer ALTER COLUMN id SET DEFAULT nextval('consumer_id_seq'::regclass);


--
-- TOC entry 2702 (class 2604 OID 20624)
-- Name: id; Type: DEFAULT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY country ALTER COLUMN id SET DEFAULT nextval('country_id_seq'::regclass);


--
-- TOC entry 2704 (class 2604 OID 20625)
-- Name: id; Type: DEFAULT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY currency ALTER COLUMN id SET DEFAULT nextval('currency_id_seq'::regclass);


--
-- TOC entry 2705 (class 2604 OID 20626)
-- Name: id; Type: DEFAULT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY customer ALTER COLUMN id SET DEFAULT nextval('customer_id_seq'::regclass);


--
-- TOC entry 2713 (class 2604 OID 20627)
-- Name: id; Type: DEFAULT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY tax ALTER COLUMN id SET DEFAULT nextval('tax_id_seq'::regclass);


--
-- TOC entry 2716 (class 2606 OID 20707)
-- Name: bank_code_key; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY bank
    ADD CONSTRAINT bank_code_key UNIQUE (code);


--
-- TOC entry 2718 (class 2606 OID 20709)
-- Name: bank_name_key; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY bank
    ADD CONSTRAINT bank_name_key UNIQUE (name);


--
-- TOC entry 2720 (class 2606 OID 20711)
-- Name: bank_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY bank
    ADD CONSTRAINT bank_pkey PRIMARY KEY (id);


--
-- TOC entry 2725 (class 2606 OID 20713)
-- Name: city_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY city
    ADD CONSTRAINT city_pkey PRIMARY KEY (id);


--
-- TOC entry 2740 (class 2606 OID 20715)
-- Name: company_account_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY company_account
    ADD CONSTRAINT company_account_pkey PRIMARY KEY (id);


--
-- TOC entry 2734 (class 2606 OID 20717)
-- Name: company_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY company
    ADD CONSTRAINT company_pkey PRIMARY KEY (id);


--
-- TOC entry 2750 (class 2606 OID 20719)
-- Name: competitor_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY competitor
    ADD CONSTRAINT competitor_pkey PRIMARY KEY (id);


--
-- TOC entry 2756 (class 2606 OID 20721)
-- Name: competitor_sku_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY competitor_sku
    ADD CONSTRAINT competitor_sku_pkey PRIMARY KEY (id);


--
-- TOC entry 2760 (class 2606 OID 20723)
-- Name: consumer_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY consumer
    ADD CONSTRAINT consumer_pkey PRIMARY KEY (id);


--
-- TOC entry 2763 (class 2606 OID 20725)
-- Name: country_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY country
    ADD CONSTRAINT country_pkey PRIMARY KEY (id);


--
-- TOC entry 2769 (class 2606 OID 20727)
-- Name: currency_code_key; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY currency
    ADD CONSTRAINT currency_code_key UNIQUE (code);


--
-- TOC entry 2772 (class 2606 OID 20729)
-- Name: currency_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY currency
    ADD CONSTRAINT currency_pkey PRIMARY KEY (id);


--
-- TOC entry 2785 (class 2606 OID 20731)
-- Name: customer_document_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY customer_document
    ADD CONSTRAINT customer_document_pkey PRIMARY KEY (id);


--
-- TOC entry 2777 (class 2606 OID 20733)
-- Name: customer_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY customer
    ADD CONSTRAINT customer_pkey PRIMARY KEY (id);


--
-- TOC entry 2795 (class 2606 OID 20735)
-- Name: network_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY network
    ADD CONSTRAINT network_pkey PRIMARY KEY (id);


--
-- TOC entry 2804 (class 2606 OID 20737)
-- Name: shop_format_name_key; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY shop_format
    ADD CONSTRAINT shop_format_name_key UNIQUE (name);


--
-- TOC entry 2806 (class 2606 OID 20739)
-- Name: shop_format_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY shop_format
    ADD CONSTRAINT shop_format_pkey PRIMARY KEY (id);


--
-- TOC entry 2800 (class 2606 OID 20741)
-- Name: shop_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY shop
    ADD CONSTRAINT shop_pkey PRIMARY KEY (id);


--
-- TOC entry 2808 (class 2606 OID 20743)
-- Name: sku_article_key; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY sku
    ADD CONSTRAINT sku_article_key UNIQUE (article);


--
-- TOC entry 2811 (class 2606 OID 20745)
-- Name: sku_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY sku
    ADD CONSTRAINT sku_pkey PRIMARY KEY (id);


--
-- TOC entry 2814 (class 2606 OID 20747)
-- Name: tax_code_key; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tax
    ADD CONSTRAINT tax_code_key UNIQUE (code);


--
-- TOC entry 2817 (class 2606 OID 20749)
-- Name: tax_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tax
    ADD CONSTRAINT tax_pkey PRIMARY KEY (id);


--
-- TOC entry 2819 (class 2606 OID 20751)
-- Name: trade_channel_name_key; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY trade_channel
    ADD CONSTRAINT trade_channel_name_key UNIQUE (name);


--
-- TOC entry 2821 (class 2606 OID 20753)
-- Name: trade_channel_pkey; Type: CONSTRAINT; Schema: dictionary; Owner: -; Tablespace: 
--

ALTER TABLE ONLY trade_channel
    ADD CONSTRAINT trade_channel_pkey PRIMARY KEY (id);


--
-- TOC entry 2721 (class 1259 OID 20969)
-- Name: city_country_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX city_country_idx ON city USING btree (country_id);


--
-- TOC entry 2722 (class 1259 OID 20970)
-- Name: city_full_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX city_full_idx ON city USING btree (country_id, start_date, end_date NULLS FIRST);


--
-- TOC entry 2723 (class 1259 OID 20971)
-- Name: city_period_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX city_period_idx ON city USING btree (start_date, end_date NULLS FIRST);


--
-- TOC entry 2726 (class 1259 OID 20972)
-- Name: city_property_city_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX city_property_city_idx ON city_property USING btree (city_id);


--
-- TOC entry 2727 (class 1259 OID 20973)
-- Name: city_property_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX city_property_idx ON city_property USING btree (city_id, type);


--
-- TOC entry 2728 (class 1259 OID 20974)
-- Name: city_property_type_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX city_property_type_idx ON city_property USING btree (type);


--
-- TOC entry 2729 (class 1259 OID 20975)
-- Name: city_property_value_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX city_property_value_idx ON city_property USING btree (value);


--
-- TOC entry 2736 (class 1259 OID 20976)
-- Name: company_account_bank_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_account_bank_idx ON company_account USING btree (bank_id);


--
-- TOC entry 2737 (class 1259 OID 20977)
-- Name: company_account_code_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_account_code_idx ON company_account USING btree (code);


--
-- TOC entry 2738 (class 1259 OID 20978)
-- Name: company_account_company_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_account_company_idx ON company_account USING btree (company_id);


--
-- TOC entry 2730 (class 1259 OID 20979)
-- Name: company_country_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_country_idx ON company USING btree (country_id);


--
-- TOC entry 2741 (class 1259 OID 20980)
-- Name: company_date_company_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_date_company_idx ON company_date USING btree (company_id);


--
-- TOC entry 2742 (class 1259 OID 20981)
-- Name: company_date_date_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_date_date_idx ON company_date USING btree (date);


--
-- TOC entry 2743 (class 1259 OID 20982)
-- Name: company_date_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX company_date_idx ON company_date USING btree (company_id, date);


--
-- TOC entry 2731 (class 1259 OID 20983)
-- Name: company_parent_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_parent_idx ON company USING btree (parent_id NULLS FIRST);


--
-- TOC entry 2732 (class 1259 OID 20984)
-- Name: company_period_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_period_idx ON company USING btree (start_date, end_date NULLS FIRST);


--
-- TOC entry 2744 (class 1259 OID 20985)
-- Name: company_property_company_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_property_company_idx ON company_property USING btree (company_id);


--
-- TOC entry 2745 (class 1259 OID 20986)
-- Name: company_property_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX company_property_idx ON company_property USING btree (company_id, type);


--
-- TOC entry 2746 (class 1259 OID 20987)
-- Name: company_property_type_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_property_type_idx ON company_property USING btree (type);


--
-- TOC entry 2747 (class 1259 OID 20988)
-- Name: company_property_value_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_property_value_idx ON company_property USING btree (value);


--
-- TOC entry 2735 (class 1259 OID 20989)
-- Name: company_unit_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX company_unit_idx ON company USING btree (unit_id NULLS FIRST);


--
-- TOC entry 2748 (class 1259 OID 20990)
-- Name: competitor_name_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX competitor_name_idx ON competitor USING btree (name);


--
-- TOC entry 2751 (class 1259 OID 20991)
-- Name: competitor_sku_competitor_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX competitor_sku_competitor_idx ON competitor_sku USING btree (competitor_id);


--
-- TOC entry 2752 (class 1259 OID 20992)
-- Name: competitor_sku_full_search_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX competitor_sku_full_search_idx ON competitor_sku USING btree (sku_id, competitor_id, name, start_date, end_date NULLS FIRST);


--
-- TOC entry 2753 (class 1259 OID 20993)
-- Name: competitor_sku_name_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX competitor_sku_name_idx ON competitor_sku USING btree (name);


--
-- TOC entry 2754 (class 1259 OID 20994)
-- Name: competitor_sku_period_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX competitor_sku_period_idx ON competitor_sku USING btree (competitor_id, name, start_date, end_date NULLS FIRST);


--
-- TOC entry 2757 (class 1259 OID 20995)
-- Name: competitor_sku_sku_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX competitor_sku_sku_idx ON competitor_sku USING btree (sku_id);


--
-- TOC entry 2758 (class 1259 OID 20996)
-- Name: consumer_name_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX consumer_name_idx ON consumer USING btree (first_name, last_name, middle_name NULLS FIRST);


--
-- TOC entry 2761 (class 1259 OID 20997)
-- Name: country_period_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX country_period_idx ON country USING btree (start_date, end_date NULLS FIRST);


--
-- TOC entry 2764 (class 1259 OID 20998)
-- Name: country_property_country_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX country_property_country_idx ON country_property USING btree (country_id);


--
-- TOC entry 2765 (class 1259 OID 20999)
-- Name: country_property_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX country_property_idx ON country_property USING btree (country_id, type);


--
-- TOC entry 2766 (class 1259 OID 21000)
-- Name: country_property_type_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX country_property_type_idx ON country_property USING btree (type);


--
-- TOC entry 2767 (class 1259 OID 21001)
-- Name: country_property_value_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX country_property_value_idx ON country_property USING btree (value);


--
-- TOC entry 2770 (class 1259 OID 21002)
-- Name: currency_name_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX currency_name_idx ON currency USING btree (name NULLS FIRST);


--
-- TOC entry 2773 (class 1259 OID 21003)
-- Name: customer_birthday_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_birthday_idx ON customer USING btree (birthday);


--
-- TOC entry 2774 (class 1259 OID 21004)
-- Name: customer_company_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_company_idx ON customer USING btree (company_id);


--
-- TOC entry 2778 (class 1259 OID 21005)
-- Name: customer_date_customer_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_date_customer_idx ON customer_date USING btree (customer_id);


--
-- TOC entry 2779 (class 1259 OID 21006)
-- Name: customer_date_date_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_date_date_idx ON customer_date USING btree (date);


--
-- TOC entry 2780 (class 1259 OID 21007)
-- Name: customer_date_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX customer_date_idx ON customer_date USING btree (customer_id, date);


--
-- TOC entry 2781 (class 1259 OID 21008)
-- Name: customer_document_country_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_document_country_idx ON customer_document USING btree (country_id);


--
-- TOC entry 2782 (class 1259 OID 21009)
-- Name: customer_document_customer_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_document_customer_idx ON customer_document USING btree (customer_id);


--
-- TOC entry 2783 (class 1259 OID 21010)
-- Name: customer_document_internal_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_document_internal_idx ON customer_document USING btree (internal);


--
-- TOC entry 2786 (class 1259 OID 21011)
-- Name: customer_document_type_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_document_type_idx ON customer_document USING btree (type);


--
-- TOC entry 2787 (class 1259 OID 21012)
-- Name: customer_document_validity_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_document_validity_idx ON customer_document USING btree (validity);


--
-- TOC entry 2775 (class 1259 OID 21013)
-- Name: customer_login_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_login_idx ON customer USING btree (email, password);


--
-- TOC entry 2788 (class 1259 OID 21014)
-- Name: customer_property_customer_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_property_customer_idx ON customer_property USING btree (customer_id);


--
-- TOC entry 2789 (class 1259 OID 21015)
-- Name: customer_property_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX customer_property_idx ON customer_property USING btree (customer_id, type);


--
-- TOC entry 2790 (class 1259 OID 21016)
-- Name: customer_property_type_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_property_type_idx ON customer_property USING btree (type);


--
-- TOC entry 2791 (class 1259 OID 21017)
-- Name: customer_property_value_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX customer_property_value_idx ON customer_property USING btree (value);


--
-- TOC entry 2792 (class 1259 OID 21018)
-- Name: network_comapny_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX network_comapny_idx ON network USING btree (company_id);


--
-- TOC entry 2793 (class 1259 OID 21019)
-- Name: network_name_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX network_name_idx ON network USING btree (name);


--
-- TOC entry 2796 (class 1259 OID 21020)
-- Name: shop_city_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX shop_city_idx ON shop USING btree (city_id);


--
-- TOC entry 2797 (class 1259 OID 21021)
-- Name: shop_name_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX shop_name_idx ON shop USING btree (name);


--
-- TOC entry 2798 (class 1259 OID 21022)
-- Name: shop_network_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX shop_network_idx ON shop USING btree (network_id);


--
-- TOC entry 2801 (class 1259 OID 21023)
-- Name: shop_shop_format_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX shop_shop_format_idx ON shop USING btree (shop_format_id);


--
-- TOC entry 2802 (class 1259 OID 21024)
-- Name: shop_trade_channel_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX shop_trade_channel_idx ON shop USING btree (trade_channel_id);


--
-- TOC entry 2809 (class 1259 OID 21025)
-- Name: sku_name_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX sku_name_idx ON sku USING btree (name);


--
-- TOC entry 2812 (class 1259 OID 21026)
-- Name: sku_sector_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX sku_sector_idx ON sku USING btree (sector_id);


--
-- TOC entry 2815 (class 1259 OID 21027)
-- Name: tax_name_idx; Type: INDEX; Schema: dictionary; Owner: -; Tablespace: 
--

CREATE INDEX tax_name_idx ON tax USING btree (name);


--
-- TOC entry 2822 (class 2606 OID 21552)
-- Name: city_country_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY city
    ADD CONSTRAINT city_country_id_fkey FOREIGN KEY (country_id) REFERENCES country(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2823 (class 2606 OID 21557)
-- Name: city_property_city_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY city_property
    ADD CONSTRAINT city_property_city_id_fkey FOREIGN KEY (city_id) REFERENCES city(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2827 (class 2606 OID 21562)
-- Name: company_account_bank_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY company_account
    ADD CONSTRAINT company_account_bank_id_fkey FOREIGN KEY (bank_id) REFERENCES bank(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2828 (class 2606 OID 21567)
-- Name: company_account_company_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY company_account
    ADD CONSTRAINT company_account_company_id_fkey FOREIGN KEY (company_id) REFERENCES company(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2824 (class 2606 OID 21572)
-- Name: company_country_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY company
    ADD CONSTRAINT company_country_id_fkey FOREIGN KEY (country_id) REFERENCES country(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2829 (class 2606 OID 21577)
-- Name: company_date_company_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY company_date
    ADD CONSTRAINT company_date_company_id_fkey FOREIGN KEY (company_id) REFERENCES company(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2825 (class 2606 OID 21582)
-- Name: company_parent_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY company
    ADD CONSTRAINT company_parent_id_fkey FOREIGN KEY (parent_id) REFERENCES company(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2830 (class 2606 OID 21587)
-- Name: company_property_company_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY company_property
    ADD CONSTRAINT company_property_company_id_fkey FOREIGN KEY (company_id) REFERENCES company(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2826 (class 2606 OID 21592)
-- Name: company_unit_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY company
    ADD CONSTRAINT company_unit_id_fkey FOREIGN KEY (unit_id) REFERENCES org_chart.unit(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2831 (class 2606 OID 21597)
-- Name: competitor_sku_competitor_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY competitor_sku
    ADD CONSTRAINT competitor_sku_competitor_id_fkey FOREIGN KEY (competitor_id) REFERENCES competitor(id) ON UPDATE CASCADE;


--
-- TOC entry 2832 (class 2606 OID 21602)
-- Name: competitor_sku_sku_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY competitor_sku
    ADD CONSTRAINT competitor_sku_sku_id_fkey FOREIGN KEY (sku_id) REFERENCES sku(id) ON UPDATE CASCADE;


--
-- TOC entry 2833 (class 2606 OID 21607)
-- Name: country_property_country_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY country_property
    ADD CONSTRAINT country_property_country_id_fkey FOREIGN KEY (country_id) REFERENCES country(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2834 (class 2606 OID 21612)
-- Name: customer_company_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY customer
    ADD CONSTRAINT customer_company_id_fkey FOREIGN KEY (company_id) REFERENCES company(id) ON UPDATE CASCADE ON DELETE SET NULL;


--
-- TOC entry 2836 (class 2606 OID 21617)
-- Name: customer_date_customer_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY customer_date
    ADD CONSTRAINT customer_date_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES customer(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2837 (class 2606 OID 21622)
-- Name: customer_document_country_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY customer_document
    ADD CONSTRAINT customer_document_country_id_fkey FOREIGN KEY (country_id) REFERENCES country(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2838 (class 2606 OID 21627)
-- Name: customer_document_customer_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY customer_document
    ADD CONSTRAINT customer_document_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES customer(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2839 (class 2606 OID 21632)
-- Name: customer_property_customer_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY customer_property
    ADD CONSTRAINT customer_property_customer_id_fkey FOREIGN KEY (customer_id) REFERENCES customer(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2835 (class 2606 OID 21637)
-- Name: customer_unit_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY customer
    ADD CONSTRAINT customer_unit_id_fkey FOREIGN KEY (unit_id) REFERENCES org_chart.unit(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2840 (class 2606 OID 21642)
-- Name: network_company_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY network
    ADD CONSTRAINT network_company_id_fkey FOREIGN KEY (company_id) REFERENCES company(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2841 (class 2606 OID 21647)
-- Name: shop_city_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY shop
    ADD CONSTRAINT shop_city_id_fkey FOREIGN KEY (city_id) REFERENCES city(id) ON UPDATE CASCADE;


--
-- TOC entry 2842 (class 2606 OID 21652)
-- Name: shop_network_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY shop
    ADD CONSTRAINT shop_network_id_fkey FOREIGN KEY (network_id) REFERENCES network(id) ON UPDATE CASCADE;


--
-- TOC entry 2843 (class 2606 OID 21657)
-- Name: shop_shop_format_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY shop
    ADD CONSTRAINT shop_shop_format_id_fkey FOREIGN KEY (shop_format_id) REFERENCES shop_format(id) ON UPDATE CASCADE;


--
-- TOC entry 2844 (class 2606 OID 21662)
-- Name: shop_trade_channel_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY shop
    ADD CONSTRAINT shop_trade_channel_id_fkey FOREIGN KEY (trade_channel_id) REFERENCES trade_channel(id) ON UPDATE CASCADE;


--
-- TOC entry 2845 (class 2606 OID 21667)
-- Name: sku_sector_id_fkey; Type: FK CONSTRAINT; Schema: dictionary; Owner: -
--

ALTER TABLE ONLY sku
    ADD CONSTRAINT sku_sector_id_fkey FOREIGN KEY (sector_id) REFERENCES org_chart.sector(id) ON UPDATE CASCADE;


-- Completed on 2015-11-25 13:07:37

--
-- PostgreSQL database dump complete
--

