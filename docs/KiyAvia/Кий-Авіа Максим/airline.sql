--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.4
-- Dumped by pg_dump version 9.4.4
-- Started on 2015-11-25 12:24:22

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 25 (class 2615 OID 19548)
-- Name: airline; Type: SCHEMA; Schema: -; Owner: ics_admin
--

CREATE SCHEMA airline;


ALTER SCHEMA airline OWNER TO ics_admin;

--
-- TOC entry 2890 (class 0 OID 0)
-- Dependencies: 25
-- Name: SCHEMA airline; Type: COMMENT; Schema: -; Owner: ics_admin
--

COMMENT ON SCHEMA airline IS 'The schema for information of airline business';


SET search_path = airline, pg_catalog;

--
-- TOC entry 446 (class 1255 OID 19574)
-- Name: add_tax_amount(bigint, bigint, numeric); Type: FUNCTION; Schema: airline; Owner: ics_admin
--

CREATE FUNCTION add_tax_amount(bigint, bigint, numeric) RETURNS void
    LANGUAGE plpgsql
    AS $_$
begin
	if ($1 < 50) then
		update airline.ticket set amount = amount + $3 where id = $2;
	else 
		update airline.ticket set base_amount = base_amount + $3 where id = $2;
	end if;
end $_$;


ALTER FUNCTION airline.add_tax_amount(bigint, bigint, numeric) OWNER TO ics_admin;

--
-- TOC entry 2892 (class 0 OID 0)
-- Dependencies: 446
-- Name: FUNCTION add_tax_amount(bigint, bigint, numeric); Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON FUNCTION add_tax_amount(bigint, bigint, numeric) IS 'Adds amount of tax to ticket amount';


--
-- TOC entry 447 (class 1255 OID 19575)
-- Name: add_ticket_amount(bigint, numeric, numeric); Type: FUNCTION; Schema: airline; Owner: ics_admin
--

CREATE FUNCTION add_ticket_amount(bigint, numeric, numeric) RETURNS void
    LANGUAGE plpgsql
    AS $_$
begin
	update airline.air_order
	set 
		base_amount = base_amount + $2,
		amount = amount + $3
	where id = (select air_order_id from airline.route where id = $1);
end $_$;


ALTER FUNCTION airline.add_ticket_amount(bigint, numeric, numeric) OWNER TO ics_admin;

--
-- TOC entry 2893 (class 0 OID 0)
-- Dependencies: 447
-- Name: FUNCTION add_ticket_amount(bigint, numeric, numeric); Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON FUNCTION add_ticket_amount(bigint, numeric, numeric) IS 'Adds base_amount and amount of ticket to airline_order';


--
-- TOC entry 448 (class 1255 OID 19576)
-- Name: before_ticket_change(); Type: FUNCTION; Schema: airline; Owner: ics_admin
--

CREATE FUNCTION before_ticket_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'INSERT') then		
		NEW.amount := NEW.base_amount;
	elsif (TG_OP = 'UPDATE') then
		NEW.amount := OLD.amount + NEW.base_amount - OLD.base_amount;
	end if;
	return NEW;
end $$;


ALTER FUNCTION airline.before_ticket_change() OWNER TO ics_admin;

--
-- TOC entry 2894 (class 0 OID 0)
-- Dependencies: 448
-- Name: FUNCTION before_ticket_change(); Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON FUNCTION before_ticket_change() IS 'Updates amount in ticket before ticket is changed';


--
-- TOC entry 449 (class 1255 OID 19577)
-- Name: rate_by_route(bigint, timestamp without time zone); Type: FUNCTION; Schema: airline; Owner: ics_admin
--

CREATE FUNCTION rate_by_route(bigint, timestamp without time zone) RETURNS numeric
    LANGUAGE plpgsql
    AS $_$
declare fin_rate numeric;
begin
	select financial.rate(o.base_currency, $2) into fin_rate from airline.route r
	left join airline.air_order o on o.id = r.air_order_id
	where r.id = $1;
	return coalesce(fin_rate, 1);
end $_$;


ALTER FUNCTION airline.rate_by_route(bigint, timestamp without time zone) OWNER TO ics_admin;

--
-- TOC entry 2895 (class 0 OID 0)
-- Dependencies: 449
-- Name: FUNCTION rate_by_route(bigint, timestamp without time zone); Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON FUNCTION rate_by_route(bigint, timestamp without time zone) IS 'Calculate currency exchange rate by route_id';


--
-- TOC entry 450 (class 1255 OID 19578)
-- Name: rate_by_ticket(bigint); Type: FUNCTION; Schema: airline; Owner: ics_admin
--

CREATE FUNCTION rate_by_ticket(bigint) RETURNS numeric
    LANGUAGE plpgsql
    AS $_$
declare fin_rate numeric;
begin
	select airline.rate_by_route(route_id, created) into fin_rate from airline.ticket where id = $1;
	return coalesce(fin_rate, 1);
end $_$;


ALTER FUNCTION airline.rate_by_ticket(bigint) OWNER TO ics_admin;

--
-- TOC entry 2896 (class 0 OID 0)
-- Dependencies: 450
-- Name: FUNCTION rate_by_ticket(bigint); Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON FUNCTION rate_by_ticket(bigint) IS 'Calculate currency exchange rate by ticket_id';


--
-- TOC entry 451 (class 1255 OID 19579)
-- Name: tax_amount_change(); Type: FUNCTION; Schema: airline; Owner: ics_admin
--

CREATE FUNCTION tax_amount_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'DELETE') then
		perform airline.add_tax_amount(OLD.tax_id, OLD.ticket_id, -1 * OLD.amount);
	elsif (TG_OP = 'INSERT') then
		perform airline.add_tax_amount(NEW.tax_id, NEW.ticket_id, NEW.amount);
	elsif (TG_OP = 'UPDATE') then
		if (OLD.ticket_id != NEW.ticket_id or OLD.tax_id != NEW.tax_id) then
			perform airline.add_tax_amount(OLD.tax_id, OLD.ticket_id, -1 * OLD.amount);
			perform airline.add_tax_amount(NEW.tax_id, NEW.ticket_id, NEW.amount);
		elsif (OLD.amount != NEW.amount) then
			perform airline.add_tax_amount(NEW.tax_id, NEW.ticket_id, NEW.amount - OLD.amount);
		end if;
	end if;
	return null;
end $$;


ALTER FUNCTION airline.tax_amount_change() OWNER TO ics_admin;

--
-- TOC entry 2897 (class 0 OID 0)
-- Dependencies: 451
-- Name: FUNCTION tax_amount_change(); Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON FUNCTION tax_amount_change() IS 'Updates amount in ticket when tax_amount is changed';


--
-- TOC entry 452 (class 1255 OID 19580)
-- Name: ticket_change(); Type: FUNCTION; Schema: airline; Owner: ics_admin
--

CREATE FUNCTION ticket_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'DELETE') then
		perform airline.add_ticket_amount(OLD.route_id, -1 * OLD.base_amount, -1 * OLD.amount);
	elsif (TG_OP = 'INSERT') then
		perform airline.add_ticket_amount(NEW.route_id, NEW.base_amount, NEW.amount);
	elsif (TG_OP = 'UPDATE') then
		if (OLD.route_id != NEW.route_id) then
			perform airline.add_ticket_amount(OLD.route_id, -1 * OLD.base_amount, -1 * OLD.amount);
			perform airline.add_ticket_amount(NEW.route_id, NEW.base_amount, NEW.amount);
		elsif (OLD.base_amount != NEW.base_amount or OLD.amount != NEW.amount) then
			perform airline.add_ticket_amount(NEW.route_id, NEW.base_amount - OLD.base_amount, NEW.amount - OLD.amount);
		end if;
	end if;
	return null;
end $$;


ALTER FUNCTION airline.ticket_change() OWNER TO ics_admin;

--
-- TOC entry 2898 (class 0 OID 0)
-- Dependencies: 452
-- Name: FUNCTION ticket_change(); Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON FUNCTION ticket_change() IS 'Updates amount in airline_order when ticket is changed';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 230 (class 1259 OID 19618)
-- Name: air_order; Type: TABLE; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE TABLE air_order (
    id bigint NOT NULL,
    customer_order_id bigint,
    company_id bigint NOT NULL,
    doc_number character varying(50) NOT NULL,
    state character varying(100) NOT NULL,
    base_currency bigint NOT NULL,
    base_amount numeric DEFAULT 0 NOT NULL,
    currency bigint NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    description character varying,
    valid_until timestamp without time zone NOT NULL,
    created timestamp without time zone DEFAULT now() NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL,
    sector_id bigint DEFAULT 11 NOT NULL
);


ALTER TABLE air_order OWNER TO ics_admin;

--
-- TOC entry 2899 (class 0 OID 0)
-- Dependencies: 230
-- Name: TABLE air_order; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON TABLE air_order IS 'The orders for airlines';


--
-- TOC entry 231 (class 1259 OID 19629)
-- Name: air_order_id_seq; Type: SEQUENCE; Schema: airline; Owner: ics_admin
--

CREATE SEQUENCE air_order_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE air_order_id_seq OWNER TO ics_admin;

--
-- TOC entry 2901 (class 0 OID 0)
-- Dependencies: 231
-- Name: air_order_id_seq; Type: SEQUENCE OWNED BY; Schema: airline; Owner: ics_admin
--

ALTER SEQUENCE air_order_id_seq OWNED BY air_order.id;


--
-- TOC entry 2902 (class 0 OID 0)
-- Dependencies: 231
-- Name: SEQUENCE air_order_id_seq; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON SEQUENCE air_order_id_seq IS 'The sequence for ID of airline orders';


--
-- TOC entry 232 (class 1259 OID 19631)
-- Name: airport; Type: TABLE; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE TABLE airport (
    id bigint NOT NULL,
    city_id bigint NOT NULL,
    iata_code public.iata_3 NOT NULL,
    start_date date DEFAULT now() NOT NULL,
    end_date date
);


ALTER TABLE airport OWNER TO ics_admin;

--
-- TOC entry 2904 (class 0 OID 0)
-- Dependencies: 232
-- Name: TABLE airport; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON TABLE airport IS 'The airports';


--
-- TOC entry 233 (class 1259 OID 19638)
-- Name: airport_id_seq; Type: SEQUENCE; Schema: airline; Owner: ics_admin
--

CREATE SEQUENCE airport_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE airport_id_seq OWNER TO ics_admin;

--
-- TOC entry 2906 (class 0 OID 0)
-- Dependencies: 233
-- Name: airport_id_seq; Type: SEQUENCE OWNED BY; Schema: airline; Owner: ics_admin
--

ALTER SEQUENCE airport_id_seq OWNED BY airport.id;


--
-- TOC entry 2907 (class 0 OID 0)
-- Dependencies: 233
-- Name: SEQUENCE airport_id_seq; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON SEQUENCE airport_id_seq IS 'The sequence for ID of airports';


--
-- TOC entry 234 (class 1259 OID 19640)
-- Name: airport_property; Type: TABLE; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE TABLE airport_property (
    airport_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


ALTER TABLE airport_property OWNER TO ics_admin;

--
-- TOC entry 2909 (class 0 OID 0)
-- Dependencies: 234
-- Name: TABLE airport_property; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON TABLE airport_property IS 'Some properties of airport';


--
-- TOC entry 235 (class 1259 OID 19646)
-- Name: main_seq; Type: SEQUENCE; Schema: airline; Owner: ics_admin
--

CREATE SEQUENCE main_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE main_seq OWNER TO ics_admin;

--
-- TOC entry 2911 (class 0 OID 0)
-- Dependencies: 235
-- Name: SEQUENCE main_seq; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON SEQUENCE main_seq IS 'The main sequence of this schema';


--
-- TOC entry 236 (class 1259 OID 19648)
-- Name: route; Type: TABLE; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE TABLE route (
    id bigint DEFAULT nextval('main_seq'::regclass) NOT NULL,
    air_order_id bigint NOT NULL,
    airport_from_id bigint NOT NULL,
    departure_date timestamp without time zone NOT NULL,
    airport_to_id bigint NOT NULL,
    arrival_date timestamp without time zone NOT NULL,
    created timestamp without time zone DEFAULT now() NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL
);


ALTER TABLE route OWNER TO ics_admin;

--
-- TOC entry 2913 (class 0 OID 0)
-- Dependencies: 236
-- Name: TABLE route; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON TABLE route IS 'The airline routes';


--
-- TOC entry 237 (class 1259 OID 19654)
-- Name: segment; Type: TABLE; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE TABLE segment (
    id bigint DEFAULT nextval('main_seq'::regclass) NOT NULL,
    route_id bigint NOT NULL,
    company_id bigint NOT NULL,
    airport_from_id bigint NOT NULL,
    departure_date timestamp without time zone NOT NULL,
    airport_to_id bigint NOT NULL,
    arrival_date timestamp without time zone NOT NULL,
    trip_num character varying(10) NOT NULL,
    category character varying(10) NOT NULL,
    seat_num character varying(10)
);


ALTER TABLE segment OWNER TO ics_admin;

--
-- TOC entry 2915 (class 0 OID 0)
-- Dependencies: 237
-- Name: TABLE segment; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON TABLE segment IS 'The airline route segments';


--
-- TOC entry 238 (class 1259 OID 19658)
-- Name: tariff; Type: TABLE; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE TABLE tariff (
    id bigint NOT NULL,
    consumer_id bigint NOT NULL,
    segment_id bigint NOT NULL,
    code character varying NOT NULL
);


ALTER TABLE tariff OWNER TO ics_admin;

--
-- TOC entry 2917 (class 0 OID 0)
-- Dependencies: 238
-- Name: TABLE tariff; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON TABLE tariff IS 'The airline tariffs';


--
-- TOC entry 239 (class 1259 OID 19664)
-- Name: tariff_id_seq; Type: SEQUENCE; Schema: airline; Owner: ics_admin
--

CREATE SEQUENCE tariff_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE tariff_id_seq OWNER TO ics_admin;

--
-- TOC entry 2919 (class 0 OID 0)
-- Dependencies: 239
-- Name: tariff_id_seq; Type: SEQUENCE OWNED BY; Schema: airline; Owner: ics_admin
--

ALTER SEQUENCE tariff_id_seq OWNED BY tariff.id;


--
-- TOC entry 240 (class 1259 OID 19666)
-- Name: tax_amount; Type: TABLE; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE TABLE tax_amount (
    id bigint NOT NULL,
    ticket_id bigint NOT NULL,
    tax_id bigint NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    created timestamp without time zone NOT NULL
);


ALTER TABLE tax_amount OWNER TO ics_admin;

--
-- TOC entry 2921 (class 0 OID 0)
-- Dependencies: 240
-- Name: TABLE tax_amount; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON TABLE tax_amount IS 'The taxes of airline ticket';


--
-- TOC entry 241 (class 1259 OID 19673)
-- Name: ticket; Type: TABLE; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE TABLE ticket (
    id bigint NOT NULL,
    consumer_id bigint NOT NULL,
    doc_number character varying(255),
    base_amount numeric NOT NULL,
    amount numeric NOT NULL,
    state public.property_type,
    route_id bigint NOT NULL,
    created timestamp without time zone DEFAULT now() NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL
);


ALTER TABLE ticket OWNER TO ics_admin;

--
-- TOC entry 2923 (class 0 OID 0)
-- Dependencies: 241
-- Name: TABLE ticket; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON TABLE ticket IS 'The airline tickets';


--
-- TOC entry 242 (class 1259 OID 19681)
-- Name: ticket_id_seq; Type: SEQUENCE; Schema: airline; Owner: ics_admin
--

CREATE SEQUENCE ticket_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE ticket_id_seq OWNER TO ics_admin;

--
-- TOC entry 2925 (class 0 OID 0)
-- Dependencies: 242
-- Name: ticket_id_seq; Type: SEQUENCE OWNED BY; Schema: airline; Owner: ics_admin
--

ALTER SEQUENCE ticket_id_seq OWNED BY ticket.id;


--
-- TOC entry 243 (class 1259 OID 19683)
-- Name: ticket_tax_id_seq; Type: SEQUENCE; Schema: airline; Owner: ics_admin
--

CREATE SEQUENCE ticket_tax_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE ticket_tax_id_seq OWNER TO ics_admin;

--
-- TOC entry 2927 (class 0 OID 0)
-- Dependencies: 243
-- Name: ticket_tax_id_seq; Type: SEQUENCE OWNED BY; Schema: airline; Owner: ics_admin
--

ALTER SEQUENCE ticket_tax_id_seq OWNED BY tax_amount.id;


--
-- TOC entry 244 (class 1259 OID 19685)
-- Name: wizzair_rate; Type: TABLE; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE TABLE wizzair_rate (
    id bigint NOT NULL,
    rate numeric DEFAULT 1 NOT NULL,
    relevance date DEFAULT now() NOT NULL
);


ALTER TABLE wizzair_rate OWNER TO ics_admin;

--
-- TOC entry 2929 (class 0 OID 0)
-- Dependencies: 244
-- Name: TABLE wizzair_rate; Type: COMMENT; Schema: airline; Owner: ics_admin
--

COMMENT ON TABLE wizzair_rate IS 'The financial rate for Wizzair';


--
-- TOC entry 245 (class 1259 OID 19693)
-- Name: wizzair_rate_id_seq; Type: SEQUENCE; Schema: airline; Owner: ics_admin
--

CREATE SEQUENCE wizzair_rate_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE wizzair_rate_id_seq OWNER TO ics_admin;

--
-- TOC entry 2931 (class 0 OID 0)
-- Dependencies: 245
-- Name: wizzair_rate_id_seq; Type: SEQUENCE OWNED BY; Schema: airline; Owner: ics_admin
--

ALTER SEQUENCE wizzair_rate_id_seq OWNED BY wizzair_rate.id;


--
-- TOC entry 2683 (class 2604 OID 20603)
-- Name: id; Type: DEFAULT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY air_order ALTER COLUMN id SET DEFAULT nextval('air_order_id_seq'::regclass);


--
-- TOC entry 2684 (class 2604 OID 20604)
-- Name: id; Type: DEFAULT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY airport ALTER COLUMN id SET DEFAULT nextval('airport_id_seq'::regclass);


--
-- TOC entry 2690 (class 2604 OID 20605)
-- Name: id; Type: DEFAULT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY tariff ALTER COLUMN id SET DEFAULT nextval('tariff_id_seq'::regclass);


--
-- TOC entry 2691 (class 2604 OID 20606)
-- Name: id; Type: DEFAULT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY tax_amount ALTER COLUMN id SET DEFAULT nextval('ticket_tax_id_seq'::regclass);


--
-- TOC entry 2695 (class 2604 OID 20607)
-- Name: id; Type: DEFAULT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY ticket ALTER COLUMN id SET DEFAULT nextval('ticket_id_seq'::regclass);


--
-- TOC entry 2698 (class 2604 OID 20608)
-- Name: id; Type: DEFAULT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY wizzair_rate ALTER COLUMN id SET DEFAULT nextval('wizzair_rate_id_seq'::regclass);


--
-- TOC entry 2705 (class 2606 OID 20665)
-- Name: air_order_pkey; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY air_order
    ADD CONSTRAINT air_order_pkey PRIMARY KEY (id);


--
-- TOC entry 2712 (class 2606 OID 20667)
-- Name: airport_iata_code_key; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY airport
    ADD CONSTRAINT airport_iata_code_key UNIQUE (iata_code);


--
-- TOC entry 2716 (class 2606 OID 20669)
-- Name: airport_pkey; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY airport
    ADD CONSTRAINT airport_pkey PRIMARY KEY (id);


--
-- TOC entry 2725 (class 2606 OID 20671)
-- Name: route_pkey; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_pkey PRIMARY KEY (id);


--
-- TOC entry 2731 (class 2606 OID 20673)
-- Name: segment_pkey; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY segment
    ADD CONSTRAINT segment_pkey PRIMARY KEY (id);


--
-- TOC entry 2736 (class 2606 OID 20675)
-- Name: tariff_pkey; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY tariff
    ADD CONSTRAINT tariff_pkey PRIMARY KEY (id);


--
-- TOC entry 2746 (class 2606 OID 20677)
-- Name: ticket_doc_number_key; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY ticket
    ADD CONSTRAINT ticket_doc_number_key UNIQUE (doc_number);


--
-- TOC entry 2748 (class 2606 OID 20679)
-- Name: ticket_pkey; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY ticket
    ADD CONSTRAINT ticket_pkey PRIMARY KEY (id);


--
-- TOC entry 2741 (class 2606 OID 20681)
-- Name: ticket_tax_pkey; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT ticket_tax_pkey PRIMARY KEY (id);


--
-- TOC entry 2750 (class 2606 OID 20683)
-- Name: wizzair_rate_pkey; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY wizzair_rate
    ADD CONSTRAINT wizzair_rate_pkey PRIMARY KEY (id);


--
-- TOC entry 2752 (class 2606 OID 20685)
-- Name: wizzair_rate_relevance_key; Type: CONSTRAINT; Schema: airline; Owner: ics_admin; Tablespace: 
--

ALTER TABLE ONLY wizzair_rate
    ADD CONSTRAINT wizzair_rate_relevance_key UNIQUE (relevance);


--
-- TOC entry 2699 (class 1259 OID 20886)
-- Name: air_order_base_currency_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_order_base_currency_idx ON air_order USING btree (base_currency);


--
-- TOC entry 2700 (class 1259 OID 20887)
-- Name: air_order_company_order_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_order_company_order_idx ON air_order USING btree (company_id);


--
-- TOC entry 2701 (class 1259 OID 20888)
-- Name: air_order_created_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_order_created_idx ON air_order USING btree (created);


--
-- TOC entry 2702 (class 1259 OID 20889)
-- Name: air_order_currency_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_order_currency_idx ON air_order USING btree (currency);


--
-- TOC entry 2703 (class 1259 OID 20890)
-- Name: air_order_customer_order_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_order_customer_order_idx ON air_order USING btree (customer_order_id NULLS FIRST);


--
-- TOC entry 2706 (class 1259 OID 20891)
-- Name: air_order_sector_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_order_sector_idx ON air_order USING btree (sector_id);


--
-- TOC entry 2707 (class 1259 OID 20892)
-- Name: air_order_state_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_order_state_idx ON air_order USING btree (state);


--
-- TOC entry 2708 (class 1259 OID 20893)
-- Name: air_order_valid_until_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_order_valid_until_idx ON air_order USING btree (valid_until);


--
-- TOC entry 2721 (class 1259 OID 20894)
-- Name: air_route_air_order_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_route_air_order_idx ON route USING btree (air_order_id);


--
-- TOC entry 2722 (class 1259 OID 20895)
-- Name: air_route_date_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_route_date_idx ON route USING btree (departure_date, arrival_date);


--
-- TOC entry 2723 (class 1259 OID 20896)
-- Name: air_route_point_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_route_point_idx ON route USING btree (airport_from_id, airport_to_id);


--
-- TOC entry 2726 (class 1259 OID 20897)
-- Name: air_segment_company_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_segment_company_idx ON segment USING btree (company_id);


--
-- TOC entry 2727 (class 1259 OID 20898)
-- Name: air_segment_date_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_segment_date_idx ON segment USING btree (departure_date, arrival_date);


--
-- TOC entry 2728 (class 1259 OID 20899)
-- Name: air_segment_point_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_segment_point_idx ON segment USING btree (airport_from_id, airport_to_id);


--
-- TOC entry 2729 (class 1259 OID 20900)
-- Name: air_segment_route_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_segment_route_idx ON segment USING btree (route_id);


--
-- TOC entry 2732 (class 1259 OID 20901)
-- Name: air_tariff_code_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_tariff_code_idx ON tariff USING btree (code);


--
-- TOC entry 2733 (class 1259 OID 20902)
-- Name: air_tariff_consumer_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_tariff_consumer_idx ON tariff USING btree (consumer_id);


--
-- TOC entry 2734 (class 1259 OID 20903)
-- Name: air_tariff_segment_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_tariff_segment_idx ON tariff USING btree (segment_id);


--
-- TOC entry 2737 (class 1259 OID 20904)
-- Name: air_tax_amount_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_tax_amount_idx ON tax_amount USING btree (ticket_id, tax_id);


--
-- TOC entry 2738 (class 1259 OID 20905)
-- Name: air_tax_amount_tax_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_tax_amount_tax_idx ON tax_amount USING btree (tax_id);


--
-- TOC entry 2739 (class 1259 OID 20906)
-- Name: air_tax_amount_ticket_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_tax_amount_ticket_idx ON tax_amount USING btree (ticket_id);


--
-- TOC entry 2742 (class 1259 OID 20907)
-- Name: air_ticket_consumer_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_ticket_consumer_idx ON ticket USING btree (consumer_id);


--
-- TOC entry 2743 (class 1259 OID 20908)
-- Name: air_ticket_created_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_ticket_created_idx ON ticket USING btree (created);


--
-- TOC entry 2744 (class 1259 OID 20909)
-- Name: air_ticket_state_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX air_ticket_state_idx ON ticket USING btree (state);


--
-- TOC entry 2709 (class 1259 OID 20910)
-- Name: airport_city_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX airport_city_idx ON airport USING btree (city_id);


--
-- TOC entry 2710 (class 1259 OID 20911)
-- Name: airport_city_period_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX airport_city_period_idx ON airport USING btree (city_id, start_date, end_date NULLS FIRST);


--
-- TOC entry 2713 (class 1259 OID 20912)
-- Name: airport_iata_period_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX airport_iata_period_idx ON airport USING btree (iata_code, start_date, end_date NULLS FIRST);


--
-- TOC entry 2714 (class 1259 OID 20913)
-- Name: airport_period_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX airport_period_idx ON airport USING btree (start_date, end_date NULLS FIRST);


--
-- TOC entry 2717 (class 1259 OID 20914)
-- Name: airport_property_airport_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX airport_property_airport_idx ON airport_property USING btree (airport_id);


--
-- TOC entry 2718 (class 1259 OID 20915)
-- Name: airport_property_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE UNIQUE INDEX airport_property_idx ON airport_property USING btree (airport_id, type);


--
-- TOC entry 2719 (class 1259 OID 20916)
-- Name: airport_property_type_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX airport_property_type_idx ON airport_property USING btree (type);


--
-- TOC entry 2720 (class 1259 OID 20917)
-- Name: airport_property_value_idx; Type: INDEX; Schema: airline; Owner: ics_admin; Tablespace: 
--

CREATE INDEX airport_property_value_idx ON airport_property USING btree (value);


--
-- TOC entry 2773 (class 2620 OID 21303)
-- Name: air_order_change; Type: TRIGGER; Schema: airline; Owner: ics_admin
--

CREATE TRIGGER air_order_change AFTER INSERT OR DELETE OR UPDATE OF customer_order_id, amount ON air_order FOR EACH ROW EXECUTE PROCEDURE financial.order_change();


--
-- TOC entry 2775 (class 2620 OID 21304)
-- Name: before_ticket_change; Type: TRIGGER; Schema: airline; Owner: ics_admin
--

CREATE TRIGGER before_ticket_change BEFORE INSERT OR UPDATE OF route_id, base_amount ON ticket FOR EACH ROW EXECUTE PROCEDURE before_ticket_change();


--
-- TOC entry 2774 (class 2620 OID 21305)
-- Name: tax_amount_change; Type: TRIGGER; Schema: airline; Owner: ics_admin
--

CREATE TRIGGER tax_amount_change AFTER INSERT OR DELETE OR UPDATE OF ticket_id, tax_id, amount ON tax_amount FOR EACH ROW EXECUTE PROCEDURE tax_amount_change();


--
-- TOC entry 2776 (class 2620 OID 21306)
-- Name: ticket_change; Type: TRIGGER; Schema: airline; Owner: ics_admin
--

CREATE TRIGGER ticket_change AFTER INSERT OR DELETE OR UPDATE OF route_id, base_amount, amount ON ticket FOR EACH ROW EXECUTE PROCEDURE ticket_change();


--
-- TOC entry 2753 (class 2606 OID 21327)
-- Name: air_order_base_currency_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY air_order
    ADD CONSTRAINT air_order_base_currency_fkey FOREIGN KEY (base_currency) REFERENCES dictionary.currency(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2754 (class 2606 OID 21332)
-- Name: air_order_company_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY air_order
    ADD CONSTRAINT air_order_company_id_fkey FOREIGN KEY (company_id) REFERENCES dictionary.company(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2755 (class 2606 OID 21337)
-- Name: air_order_currency_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY air_order
    ADD CONSTRAINT air_order_currency_fkey FOREIGN KEY (currency) REFERENCES dictionary.currency(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2756 (class 2606 OID 21342)
-- Name: air_order_customer_order_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY air_order
    ADD CONSTRAINT air_order_customer_order_id_fkey FOREIGN KEY (customer_order_id) REFERENCES financial.customer_order(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2757 (class 2606 OID 21347)
-- Name: air_order_sector_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY air_order
    ADD CONSTRAINT air_order_sector_id_fkey FOREIGN KEY (sector_id) REFERENCES org_chart.sector(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2758 (class 2606 OID 21352)
-- Name: airport_city_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY airport
    ADD CONSTRAINT airport_city_id_fkey FOREIGN KEY (city_id) REFERENCES dictionary.city(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2759 (class 2606 OID 21357)
-- Name: airport_property_airport_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY airport_property
    ADD CONSTRAINT airport_property_airport_id_fkey FOREIGN KEY (airport_id) REFERENCES airport(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2760 (class 2606 OID 21362)
-- Name: route_air_order_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_air_order_id_fkey FOREIGN KEY (air_order_id) REFERENCES air_order(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2761 (class 2606 OID 21367)
-- Name: route_airport_from_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_airport_from_id_fkey FOREIGN KEY (airport_from_id) REFERENCES airport(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2762 (class 2606 OID 21372)
-- Name: route_airport_to_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_airport_to_id_fkey FOREIGN KEY (airport_to_id) REFERENCES airport(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2763 (class 2606 OID 21377)
-- Name: segment_airport_from_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY segment
    ADD CONSTRAINT segment_airport_from_id_fkey FOREIGN KEY (airport_from_id) REFERENCES airport(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2764 (class 2606 OID 21382)
-- Name: segment_airport_to_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY segment
    ADD CONSTRAINT segment_airport_to_id_fkey FOREIGN KEY (airport_to_id) REFERENCES airport(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2765 (class 2606 OID 21387)
-- Name: segment_company_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY segment
    ADD CONSTRAINT segment_company_id_fkey FOREIGN KEY (company_id) REFERENCES dictionary.company(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2766 (class 2606 OID 21392)
-- Name: segment_route_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY segment
    ADD CONSTRAINT segment_route_id_fkey FOREIGN KEY (route_id) REFERENCES route(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2767 (class 2606 OID 21397)
-- Name: tariff_consumer_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY tariff
    ADD CONSTRAINT tariff_consumer_id_fkey FOREIGN KEY (consumer_id) REFERENCES dictionary.consumer(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2768 (class 2606 OID 21402)
-- Name: tariff_segment_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY tariff
    ADD CONSTRAINT tariff_segment_id_fkey FOREIGN KEY (segment_id) REFERENCES segment(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2771 (class 2606 OID 21407)
-- Name: ticket_consumer_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY ticket
    ADD CONSTRAINT ticket_consumer_id_fkey FOREIGN KEY (consumer_id) REFERENCES dictionary.consumer(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2772 (class 2606 OID 21412)
-- Name: ticket_route_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY ticket
    ADD CONSTRAINT ticket_route_id_fkey FOREIGN KEY (route_id) REFERENCES route(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2769 (class 2606 OID 21417)
-- Name: ticket_tax_tax_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT ticket_tax_tax_id_fkey FOREIGN KEY (tax_id) REFERENCES dictionary.tax(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2770 (class 2606 OID 21422)
-- Name: ticket_tax_ticket_id_fkey; Type: FK CONSTRAINT; Schema: airline; Owner: ics_admin
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT ticket_tax_ticket_id_fkey FOREIGN KEY (ticket_id) REFERENCES ticket(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2891 (class 0 OID 0)
-- Dependencies: 25
-- Name: airline; Type: ACL; Schema: -; Owner: ics_admin
--

REVOKE ALL ON SCHEMA airline FROM PUBLIC;
REVOKE ALL ON SCHEMA airline FROM ics_admin;
GRANT ALL ON SCHEMA airline TO ics_admin;
GRANT ALL ON SCHEMA airline TO ism_user;
GRANT ALL ON SCHEMA airline TO its_user;
GRANT ALL ON SCHEMA airline TO svd_user;
GRANT ALL ON SCHEMA airline TO tss_user;
GRANT USAGE ON SCHEMA airline TO view_only;


--
-- TOC entry 2900 (class 0 OID 0)
-- Dependencies: 230
-- Name: air_order; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON TABLE air_order FROM PUBLIC;
REVOKE ALL ON TABLE air_order FROM ics_admin;
GRANT ALL ON TABLE air_order TO ics_admin;
GRANT ALL ON TABLE air_order TO ism_user;
GRANT ALL ON TABLE air_order TO its_user;
GRANT ALL ON TABLE air_order TO svd_user;
GRANT ALL ON TABLE air_order TO tss_user;
GRANT SELECT ON TABLE air_order TO view_only;


--
-- TOC entry 2903 (class 0 OID 0)
-- Dependencies: 231
-- Name: air_order_id_seq; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON SEQUENCE air_order_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE air_order_id_seq FROM ics_admin;
GRANT ALL ON SEQUENCE air_order_id_seq TO ics_admin;
GRANT ALL ON SEQUENCE air_order_id_seq TO ism_user;
GRANT ALL ON SEQUENCE air_order_id_seq TO its_user;
GRANT ALL ON SEQUENCE air_order_id_seq TO svd_user;
GRANT ALL ON SEQUENCE air_order_id_seq TO tss_user;
GRANT SELECT ON SEQUENCE air_order_id_seq TO view_only;


--
-- TOC entry 2905 (class 0 OID 0)
-- Dependencies: 232
-- Name: airport; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON TABLE airport FROM PUBLIC;
REVOKE ALL ON TABLE airport FROM ics_admin;
GRANT ALL ON TABLE airport TO ics_admin;
GRANT ALL ON TABLE airport TO ism_user;
GRANT ALL ON TABLE airport TO its_user;
GRANT ALL ON TABLE airport TO svd_user;
GRANT ALL ON TABLE airport TO tss_user;
GRANT SELECT ON TABLE airport TO view_only;


--
-- TOC entry 2908 (class 0 OID 0)
-- Dependencies: 233
-- Name: airport_id_seq; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON SEQUENCE airport_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE airport_id_seq FROM ics_admin;
GRANT ALL ON SEQUENCE airport_id_seq TO ics_admin;
GRANT ALL ON SEQUENCE airport_id_seq TO ism_user;
GRANT ALL ON SEQUENCE airport_id_seq TO its_user;
GRANT ALL ON SEQUENCE airport_id_seq TO svd_user;
GRANT ALL ON SEQUENCE airport_id_seq TO tss_user;
GRANT SELECT ON SEQUENCE airport_id_seq TO view_only;


--
-- TOC entry 2910 (class 0 OID 0)
-- Dependencies: 234
-- Name: airport_property; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON TABLE airport_property FROM PUBLIC;
REVOKE ALL ON TABLE airport_property FROM ics_admin;
GRANT ALL ON TABLE airport_property TO ics_admin;
GRANT ALL ON TABLE airport_property TO ism_user;
GRANT ALL ON TABLE airport_property TO its_user;
GRANT ALL ON TABLE airport_property TO svd_user;
GRANT ALL ON TABLE airport_property TO tss_user;
GRANT SELECT ON TABLE airport_property TO view_only;


--
-- TOC entry 2912 (class 0 OID 0)
-- Dependencies: 235
-- Name: main_seq; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON SEQUENCE main_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE main_seq FROM ics_admin;
GRANT ALL ON SEQUENCE main_seq TO ics_admin;
GRANT ALL ON SEQUENCE main_seq TO ism_user;
GRANT ALL ON SEQUENCE main_seq TO its_user;
GRANT ALL ON SEQUENCE main_seq TO svd_user;
GRANT ALL ON SEQUENCE main_seq TO tss_user;
GRANT SELECT ON SEQUENCE main_seq TO view_only;


--
-- TOC entry 2914 (class 0 OID 0)
-- Dependencies: 236
-- Name: route; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON TABLE route FROM PUBLIC;
REVOKE ALL ON TABLE route FROM ics_admin;
GRANT ALL ON TABLE route TO ics_admin;
GRANT ALL ON TABLE route TO ism_user;
GRANT ALL ON TABLE route TO its_user;
GRANT ALL ON TABLE route TO svd_user;
GRANT ALL ON TABLE route TO tss_user;
GRANT SELECT ON TABLE route TO view_only;


--
-- TOC entry 2916 (class 0 OID 0)
-- Dependencies: 237
-- Name: segment; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON TABLE segment FROM PUBLIC;
REVOKE ALL ON TABLE segment FROM ics_admin;
GRANT ALL ON TABLE segment TO ics_admin;
GRANT ALL ON TABLE segment TO ism_user;
GRANT ALL ON TABLE segment TO its_user;
GRANT ALL ON TABLE segment TO svd_user;
GRANT ALL ON TABLE segment TO tss_user;
GRANT SELECT ON TABLE segment TO view_only;


--
-- TOC entry 2918 (class 0 OID 0)
-- Dependencies: 238
-- Name: tariff; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON TABLE tariff FROM PUBLIC;
REVOKE ALL ON TABLE tariff FROM ics_admin;
GRANT ALL ON TABLE tariff TO ics_admin;
GRANT ALL ON TABLE tariff TO ism_user;
GRANT ALL ON TABLE tariff TO its_user;
GRANT ALL ON TABLE tariff TO svd_user;
GRANT ALL ON TABLE tariff TO tss_user;


--
-- TOC entry 2920 (class 0 OID 0)
-- Dependencies: 239
-- Name: tariff_id_seq; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON SEQUENCE tariff_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE tariff_id_seq FROM ics_admin;
GRANT ALL ON SEQUENCE tariff_id_seq TO ics_admin;
GRANT ALL ON SEQUENCE tariff_id_seq TO ism_user;
GRANT ALL ON SEQUENCE tariff_id_seq TO its_user;
GRANT ALL ON SEQUENCE tariff_id_seq TO svd_user;
GRANT ALL ON SEQUENCE tariff_id_seq TO tss_user;


--
-- TOC entry 2922 (class 0 OID 0)
-- Dependencies: 240
-- Name: tax_amount; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON TABLE tax_amount FROM PUBLIC;
REVOKE ALL ON TABLE tax_amount FROM ics_admin;
GRANT ALL ON TABLE tax_amount TO ics_admin;
GRANT ALL ON TABLE tax_amount TO ism_user;
GRANT ALL ON TABLE tax_amount TO its_user;
GRANT ALL ON TABLE tax_amount TO svd_user;
GRANT ALL ON TABLE tax_amount TO tss_user;


--
-- TOC entry 2924 (class 0 OID 0)
-- Dependencies: 241
-- Name: ticket; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON TABLE ticket FROM PUBLIC;
REVOKE ALL ON TABLE ticket FROM ics_admin;
GRANT ALL ON TABLE ticket TO ics_admin;
GRANT ALL ON TABLE ticket TO ism_user;
GRANT ALL ON TABLE ticket TO its_user;
GRANT ALL ON TABLE ticket TO svd_user;
GRANT ALL ON TABLE ticket TO tss_user;


--
-- TOC entry 2926 (class 0 OID 0)
-- Dependencies: 242
-- Name: ticket_id_seq; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON SEQUENCE ticket_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE ticket_id_seq FROM ics_admin;
GRANT ALL ON SEQUENCE ticket_id_seq TO ics_admin;
GRANT ALL ON SEQUENCE ticket_id_seq TO ism_user;
GRANT ALL ON SEQUENCE ticket_id_seq TO its_user;
GRANT ALL ON SEQUENCE ticket_id_seq TO svd_user;
GRANT ALL ON SEQUENCE ticket_id_seq TO tss_user;


--
-- TOC entry 2928 (class 0 OID 0)
-- Dependencies: 243
-- Name: ticket_tax_id_seq; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON SEQUENCE ticket_tax_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE ticket_tax_id_seq FROM ics_admin;
GRANT ALL ON SEQUENCE ticket_tax_id_seq TO ics_admin;
GRANT ALL ON SEQUENCE ticket_tax_id_seq TO ism_user;
GRANT ALL ON SEQUENCE ticket_tax_id_seq TO its_user;
GRANT ALL ON SEQUENCE ticket_tax_id_seq TO svd_user;
GRANT ALL ON SEQUENCE ticket_tax_id_seq TO tss_user;


--
-- TOC entry 2930 (class 0 OID 0)
-- Dependencies: 244
-- Name: wizzair_rate; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON TABLE wizzair_rate FROM PUBLIC;
REVOKE ALL ON TABLE wizzair_rate FROM ics_admin;
GRANT ALL ON TABLE wizzair_rate TO ics_admin;
GRANT ALL ON TABLE wizzair_rate TO ism_user;
GRANT ALL ON TABLE wizzair_rate TO its_user;
GRANT ALL ON TABLE wizzair_rate TO svd_user;
GRANT ALL ON TABLE wizzair_rate TO tss_user;


--
-- TOC entry 2932 (class 0 OID 0)
-- Dependencies: 245
-- Name: wizzair_rate_id_seq; Type: ACL; Schema: airline; Owner: ics_admin
--

REVOKE ALL ON SEQUENCE wizzair_rate_id_seq FROM PUBLIC;
REVOKE ALL ON SEQUENCE wizzair_rate_id_seq FROM ics_admin;
GRANT ALL ON SEQUENCE wizzair_rate_id_seq TO ics_admin;
GRANT ALL ON SEQUENCE wizzair_rate_id_seq TO ism_user;
GRANT ALL ON SEQUENCE wizzair_rate_id_seq TO its_user;
GRANT ALL ON SEQUENCE wizzair_rate_id_seq TO svd_user;
GRANT ALL ON SEQUENCE wizzair_rate_id_seq TO tss_user;


-- Completed on 2015-11-25 12:24:23

--
-- PostgreSQL database dump complete
--

