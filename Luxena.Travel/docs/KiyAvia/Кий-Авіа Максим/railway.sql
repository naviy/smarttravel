--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.4
-- Dumped by pg_dump version 9.4.4
-- Started on 2015-11-25 12:28:04

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 11 (class 2615 OID 19558)
-- Name: railway; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA railway;


--
-- TOC entry 2889 (class 0 OID 0)
-- Dependencies: 11
-- Name: SCHEMA railway; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA railway IS 'The schema for information of railway business';


SET search_path = railway, pg_catalog;

--
-- TOC entry 471 (class 1255 OID 19609)
-- Name: add_tax_amount(bigint, bigint, numeric); Type: FUNCTION; Schema: railway; Owner: -
--

CREATE FUNCTION add_tax_amount(bigint, bigint, numeric) RETURNS void
    LANGUAGE plpgsql
    AS $_$
begin
	if ($1 < 50) then
		update railway.ticket set amount = amount + $3 where id = $2;
	else 
		update railway.ticket set base_amount = base_amount + $3 where id = $2;
	end if;
end $_$;


--
-- TOC entry 2890 (class 0 OID 0)
-- Dependencies: 471
-- Name: FUNCTION add_tax_amount(bigint, bigint, numeric); Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON FUNCTION add_tax_amount(bigint, bigint, numeric) IS 'Adds amount of tax to ticket amount';


--
-- TOC entry 468 (class 1255 OID 19610)
-- Name: add_ticket_amount(bigint, numeric, numeric); Type: FUNCTION; Schema: railway; Owner: -
--

CREATE FUNCTION add_ticket_amount(bigint, numeric, numeric) RETURNS void
    LANGUAGE sql
    AS $_$
	update railway.railway_order 
	set 
		base_amount = base_amount + $2,
		amount = amount + $3
	where id = (select railway_order_id from railway.route where id = $1);
$_$;


--
-- TOC entry 2891 (class 0 OID 0)
-- Dependencies: 468
-- Name: FUNCTION add_ticket_amount(bigint, numeric, numeric); Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON FUNCTION add_ticket_amount(bigint, numeric, numeric) IS 'Adds base_amount and amount of ticket to railway_order';


--
-- TOC entry 487 (class 1255 OID 19611)
-- Name: before_ticket_change(); Type: FUNCTION; Schema: railway; Owner: -
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


--
-- TOC entry 2892 (class 0 OID 0)
-- Dependencies: 487
-- Name: FUNCTION before_ticket_change(); Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON FUNCTION before_ticket_change() IS 'Updates amount in ticket before ticket is changed';


--
-- TOC entry 483 (class 1255 OID 19612)
-- Name: rate_by_route(bigint, timestamp without time zone); Type: FUNCTION; Schema: railway; Owner: -
--

CREATE FUNCTION rate_by_route(bigint, timestamp without time zone) RETURNS numeric
    LANGUAGE plpgsql
    AS $_$
declare fin_rate numeric;
begin
	select financial.rate(o.base_currency, $2) into fin_rate from railway.route r
	left join railway.railway_order o on o.id = r.railway_order_id
	where r.id = $1;
	return coalesce(fin_rate, 1);
end $_$;


--
-- TOC entry 2893 (class 0 OID 0)
-- Dependencies: 483
-- Name: FUNCTION rate_by_route(bigint, timestamp without time zone); Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON FUNCTION rate_by_route(bigint, timestamp without time zone) IS 'Calculate currency exchange rate by route_id';


--
-- TOC entry 484 (class 1255 OID 19613)
-- Name: rate_by_ticket(bigint); Type: FUNCTION; Schema: railway; Owner: -
--

CREATE FUNCTION rate_by_ticket(bigint) RETURNS numeric
    LANGUAGE plpgsql
    AS $_$
declare fin_rate numeric;
begin
	select railway.rate_by_route(route_id, created) into fin_rate from railway.ticket where id = $1;
	return coalesce(fin_rate, 1);
end $_$;


--
-- TOC entry 2894 (class 0 OID 0)
-- Dependencies: 484
-- Name: FUNCTION rate_by_ticket(bigint); Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON FUNCTION rate_by_ticket(bigint) IS 'Calculate currency exchange rate by ticket_id';


--
-- TOC entry 485 (class 1255 OID 19614)
-- Name: tax_amount_change(); Type: FUNCTION; Schema: railway; Owner: -
--

CREATE FUNCTION tax_amount_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'DELETE') then
		perform railway.add_tax_amount(OLD.tax_id, OLD.ticket_id, -1 * OLD.amount);
	elsif (TG_OP = 'INSERT') then
		perform railway.add_tax_amount(NEW.tax_id, NEW.ticket_id, NEW.amount);
	elsif (TG_OP = 'UPDATE') then
		if (OLD.ticket_id != NEW.ticket_id or OLD.tax_id != NEW.tax_id) then
			perform railway.add_tax_amount(OLD.tax_id, OLD.ticket_id, -1 * OLD.amount);
			perform railway.add_tax_amount(NEW.tax_id, NEW.ticket_id, NEW.amount);
		elsif (OLD.amount != NEW.amount) then
			perform railway.add_tax_amount(NEW.tax_id, NEW.ticket_id, NEW.amount - OLD.amount);
		end if;
	end if;
	return null;
end $$;


--
-- TOC entry 2895 (class 0 OID 0)
-- Dependencies: 485
-- Name: FUNCTION tax_amount_change(); Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON FUNCTION tax_amount_change() IS 'Updates amount in ticket when tax_amount is changed';


--
-- TOC entry 486 (class 1255 OID 19615)
-- Name: ticket_change(); Type: FUNCTION; Schema: railway; Owner: -
--

CREATE FUNCTION ticket_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'DELETE') then
		perform railway.add_ticket_amount(OLD.route_id, -1 * OLD.base_amount, -1 * OLD.amount);
	elsif (TG_OP = 'INSERT') then
		perform railway.add_ticket_amount(NEW.route_id, NEW.base_amount, NEW.amount);
	elsif (TG_OP = 'UPDATE') then
		if (OLD.route_id != NEW.route_id) then
			perform railway.add_ticket_amount(OLD.route_id, -1 * OLD.base_amount, -1 * OLD.amount);
			perform railway.add_ticket_amount(NEW.route_id, NEW.base_amount, NEW.amount);
		elsif (OLD.base_amount != NEW.base_amount or OLD.amount != NEW.amount) then
			perform railway.add_ticket_amount(NEW.route_id, NEW.base_amount - OLD.base_amount, NEW.amount - OLD.amount);
		end if;
	end if;
	return null;
end $$;


--
-- TOC entry 2896 (class 0 OID 0)
-- Dependencies: 486
-- Name: FUNCTION ticket_change(); Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON FUNCTION ticket_change() IS 'Updates amount in railway_order when ticket is changed';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 383 (class 1259 OID 20407)
-- Name: railway; Type: TABLE; Schema: railway; Owner: -; Tablespace: 
--

CREATE TABLE railway (
    id bigint NOT NULL,
    country_id bigint NOT NULL,
    start_date date DEFAULT now() NOT NULL,
    end_date date,
    company_id bigint NOT NULL,
    railway_code character varying(10) NOT NULL
);


--
-- TOC entry 2897 (class 0 OID 0)
-- Dependencies: 383
-- Name: TABLE railway; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON TABLE railway IS 'The railways dictionary';


--
-- TOC entry 384 (class 1259 OID 20411)
-- Name: railway_id_seq; Type: SEQUENCE; Schema: railway; Owner: -
--

CREATE SEQUENCE railway_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2898 (class 0 OID 0)
-- Dependencies: 384
-- Name: railway_id_seq; Type: SEQUENCE OWNED BY; Schema: railway; Owner: -
--

ALTER SEQUENCE railway_id_seq OWNED BY railway.id;


--
-- TOC entry 2899 (class 0 OID 0)
-- Dependencies: 384
-- Name: SEQUENCE railway_id_seq; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON SEQUENCE railway_id_seq IS 'The sequence for ID of railways';


--
-- TOC entry 385 (class 1259 OID 20413)
-- Name: railway_order; Type: TABLE; Schema: railway; Owner: -; Tablespace: 
--

CREATE TABLE railway_order (
    id bigint NOT NULL,
    customer_order_id bigint,
    company_id bigint NOT NULL,
    doc_number character varying NOT NULL,
    state public.property_type,
    currency bigint NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    description character varying,
    valid_until timestamp without time zone NOT NULL,
    created timestamp without time zone DEFAULT now() NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL,
    sector_id bigint DEFAULT 21 NOT NULL,
    base_currency bigint NOT NULL,
    base_amount numeric DEFAULT 0 NOT NULL
);


--
-- TOC entry 2900 (class 0 OID 0)
-- Dependencies: 385
-- Name: TABLE railway_order; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON TABLE railway_order IS 'The orders for railways';


--
-- TOC entry 386 (class 1259 OID 20424)
-- Name: railway_order_id_seq; Type: SEQUENCE; Schema: railway; Owner: -
--

CREATE SEQUENCE railway_order_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2901 (class 0 OID 0)
-- Dependencies: 386
-- Name: railway_order_id_seq; Type: SEQUENCE OWNED BY; Schema: railway; Owner: -
--

ALTER SEQUENCE railway_order_id_seq OWNED BY railway_order.id;


--
-- TOC entry 387 (class 1259 OID 20426)
-- Name: railway_property; Type: TABLE; Schema: railway; Owner: -; Tablespace: 
--

CREATE TABLE railway_property (
    railway_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


--
-- TOC entry 2902 (class 0 OID 0)
-- Dependencies: 387
-- Name: TABLE railway_property; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON TABLE railway_property IS 'Some properties of railway';


--
-- TOC entry 388 (class 1259 OID 20432)
-- Name: route; Type: TABLE; Schema: railway; Owner: -; Tablespace: 
--

CREATE TABLE route (
    id bigint NOT NULL,
    railway_order_id bigint NOT NULL,
    doc_number character varying NOT NULL,
    train_number character varying(20) NOT NULL,
    station_from_id bigint NOT NULL,
    departure_date timestamp without time zone NOT NULL,
    station_to_id bigint NOT NULL,
    arrival_date timestamp without time zone NOT NULL,
    electronic boolean DEFAULT false NOT NULL,
    created timestamp without time zone DEFAULT now() NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL
);


--
-- TOC entry 2903 (class 0 OID 0)
-- Dependencies: 388
-- Name: TABLE route; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON TABLE route IS 'The railway routes';


--
-- TOC entry 389 (class 1259 OID 20441)
-- Name: route_id_seq; Type: SEQUENCE; Schema: railway; Owner: -
--

CREATE SEQUENCE route_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2904 (class 0 OID 0)
-- Dependencies: 389
-- Name: route_id_seq; Type: SEQUENCE OWNED BY; Schema: railway; Owner: -
--

ALTER SEQUENCE route_id_seq OWNED BY route.id;


--
-- TOC entry 390 (class 1259 OID 20443)
-- Name: station; Type: TABLE; Schema: railway; Owner: -; Tablespace: 
--

CREATE TABLE station (
    id bigint NOT NULL,
    city_id bigint,
    start_date date DEFAULT now() NOT NULL,
    end_date date,
    railway_id bigint NOT NULL,
    railway_code character varying(10) NOT NULL
);


--
-- TOC entry 2905 (class 0 OID 0)
-- Dependencies: 390
-- Name: TABLE station; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON TABLE station IS 'The stations dictionary';


--
-- TOC entry 391 (class 1259 OID 20447)
-- Name: station_id_seq; Type: SEQUENCE; Schema: railway; Owner: -
--

CREATE SEQUENCE station_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2906 (class 0 OID 0)
-- Dependencies: 391
-- Name: station_id_seq; Type: SEQUENCE OWNED BY; Schema: railway; Owner: -
--

ALTER SEQUENCE station_id_seq OWNED BY station.id;


--
-- TOC entry 2907 (class 0 OID 0)
-- Dependencies: 391
-- Name: SEQUENCE station_id_seq; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON SEQUENCE station_id_seq IS 'The sequence for ID of stations';


--
-- TOC entry 392 (class 1259 OID 20449)
-- Name: station_property; Type: TABLE; Schema: railway; Owner: -; Tablespace: 
--

CREATE TABLE station_property (
    station_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


--
-- TOC entry 2908 (class 0 OID 0)
-- Dependencies: 392
-- Name: TABLE station_property; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON TABLE station_property IS 'Some properties of station';


--
-- TOC entry 393 (class 1259 OID 20455)
-- Name: tax_amount; Type: TABLE; Schema: railway; Owner: -; Tablespace: 
--

CREATE TABLE tax_amount (
    id bigint NOT NULL,
    ticket_id bigint NOT NULL,
    tax_id bigint NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    created timestamp without time zone NOT NULL
);


--
-- TOC entry 2909 (class 0 OID 0)
-- Dependencies: 393
-- Name: TABLE tax_amount; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON TABLE tax_amount IS 'The taxes of railway ticket';


--
-- TOC entry 394 (class 1259 OID 20462)
-- Name: tax_amount_id_seq; Type: SEQUENCE; Schema: railway; Owner: -
--

CREATE SEQUENCE tax_amount_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2910 (class 0 OID 0)
-- Dependencies: 394
-- Name: tax_amount_id_seq; Type: SEQUENCE OWNED BY; Schema: railway; Owner: -
--

ALTER SEQUENCE tax_amount_id_seq OWNED BY tax_amount.id;


--
-- TOC entry 395 (class 1259 OID 20464)
-- Name: ticket; Type: TABLE; Schema: railway; Owner: -; Tablespace: 
--

CREATE TABLE ticket (
    id bigint NOT NULL,
    route_id bigint NOT NULL,
    consumer_id bigint NOT NULL,
    doc_number character varying(255) NOT NULL,
    doc_type public.property_type,
    wagon_number character varying(10) NOT NULL,
    seat_number character varying(255) NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    original_data character varying,
    created timestamp without time zone NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL,
    base_amount numeric DEFAULT 0 NOT NULL,
    state public.property_type DEFAULT 'SALE'::character varying
);


--
-- TOC entry 2911 (class 0 OID 0)
-- Dependencies: 395
-- Name: TABLE ticket; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON TABLE ticket IS 'The railway ticket';


--
-- TOC entry 396 (class 1259 OID 20474)
-- Name: ticket_id_seq; Type: SEQUENCE; Schema: railway; Owner: -
--

CREATE SEQUENCE ticket_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2912 (class 0 OID 0)
-- Dependencies: 396
-- Name: ticket_id_seq; Type: SEQUENCE OWNED BY; Schema: railway; Owner: -
--

ALTER SEQUENCE ticket_id_seq OWNED BY ticket.id;


--
-- TOC entry 397 (class 1259 OID 20476)
-- Name: ticket_property; Type: TABLE; Schema: railway; Owner: -; Tablespace: 
--

CREATE TABLE ticket_property (
    ticket_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


--
-- TOC entry 2913 (class 0 OID 0)
-- Dependencies: 397
-- Name: TABLE ticket_property; Type: COMMENT; Schema: railway; Owner: -
--

COMMENT ON TABLE ticket_property IS 'Some properties of railway ticket';


--
-- TOC entry 2678 (class 2604 OID 20654)
-- Name: id; Type: DEFAULT; Schema: railway; Owner: -
--

ALTER TABLE ONLY railway ALTER COLUMN id SET DEFAULT nextval('railway_id_seq'::regclass);


--
-- TOC entry 2685 (class 2604 OID 20655)
-- Name: id; Type: DEFAULT; Schema: railway; Owner: -
--

ALTER TABLE ONLY railway_order ALTER COLUMN id SET DEFAULT nextval('railway_order_id_seq'::regclass);


--
-- TOC entry 2689 (class 2604 OID 20656)
-- Name: id; Type: DEFAULT; Schema: railway; Owner: -
--

ALTER TABLE ONLY route ALTER COLUMN id SET DEFAULT nextval('route_id_seq'::regclass);


--
-- TOC entry 2690 (class 2604 OID 20657)
-- Name: id; Type: DEFAULT; Schema: railway; Owner: -
--

ALTER TABLE ONLY station ALTER COLUMN id SET DEFAULT nextval('station_id_seq'::regclass);


--
-- TOC entry 2692 (class 2604 OID 20658)
-- Name: id; Type: DEFAULT; Schema: railway; Owner: -
--

ALTER TABLE ONLY tax_amount ALTER COLUMN id SET DEFAULT nextval('tax_amount_id_seq'::regclass);


--
-- TOC entry 2698 (class 2604 OID 20659)
-- Name: id; Type: DEFAULT; Schema: railway; Owner: -
--

ALTER TABLE ONLY ticket ALTER COLUMN id SET DEFAULT nextval('ticket_id_seq'::regclass);


--
-- TOC entry 2713 (class 2606 OID 20849)
-- Name: railway_order_pkey; Type: CONSTRAINT; Schema: railway; Owner: -; Tablespace: 
--

ALTER TABLE ONLY railway_order
    ADD CONSTRAINT railway_order_pkey PRIMARY KEY (id);


--
-- TOC entry 2704 (class 2606 OID 20851)
-- Name: railway_pkey; Type: CONSTRAINT; Schema: railway; Owner: -; Tablespace: 
--

ALTER TABLE ONLY railway
    ADD CONSTRAINT railway_pkey PRIMARY KEY (id);


--
-- TOC entry 2706 (class 2606 OID 20853)
-- Name: railway_railway_code_key; Type: CONSTRAINT; Schema: railway; Owner: -; Tablespace: 
--

ALTER TABLE ONLY railway
    ADD CONSTRAINT railway_railway_code_key UNIQUE (railway_code);


--
-- TOC entry 2723 (class 2606 OID 20855)
-- Name: route_pkey; Type: CONSTRAINT; Schema: railway; Owner: -; Tablespace: 
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_pkey PRIMARY KEY (id);


--
-- TOC entry 2730 (class 2606 OID 20857)
-- Name: station_pkey; Type: CONSTRAINT; Schema: railway; Owner: -; Tablespace: 
--

ALTER TABLE ONLY station
    ADD CONSTRAINT station_pkey PRIMARY KEY (id);


--
-- TOC entry 2732 (class 2606 OID 20859)
-- Name: station_railway_code_key; Type: CONSTRAINT; Schema: railway; Owner: -; Tablespace: 
--

ALTER TABLE ONLY station
    ADD CONSTRAINT station_railway_code_key UNIQUE (railway_code);


--
-- TOC entry 2742 (class 2606 OID 20861)
-- Name: tax_amount_pkey; Type: CONSTRAINT; Schema: railway; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_pkey PRIMARY KEY (id);


--
-- TOC entry 2747 (class 2606 OID 20863)
-- Name: ticket_pkey; Type: CONSTRAINT; Schema: railway; Owner: -; Tablespace: 
--

ALTER TABLE ONLY ticket
    ADD CONSTRAINT ticket_pkey PRIMARY KEY (id);


--
-- TOC entry 2699 (class 1259 OID 21213)
-- Name: railway_company_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_company_idx ON railway USING btree (company_id);


--
-- TOC entry 2700 (class 1259 OID 21214)
-- Name: railway_country_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_country_idx ON railway USING btree (country_id);


--
-- TOC entry 2701 (class 1259 OID 21215)
-- Name: railway_full_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_full_idx ON railway USING btree (country_id, start_date, end_date NULLS FIRST);


--
-- TOC entry 2707 (class 1259 OID 21216)
-- Name: railway_order_base_currency_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_order_base_currency_idx ON railway_order USING btree (base_currency);


--
-- TOC entry 2708 (class 1259 OID 21217)
-- Name: railway_order_company_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_order_company_idx ON railway_order USING btree (company_id);


--
-- TOC entry 2709 (class 1259 OID 21218)
-- Name: railway_order_created_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_order_created_idx ON railway_order USING btree (created);


--
-- TOC entry 2710 (class 1259 OID 21219)
-- Name: railway_order_currency_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_order_currency_idx ON railway_order USING btree (currency);


--
-- TOC entry 2711 (class 1259 OID 21220)
-- Name: railway_order_customer_order_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_order_customer_order_idx ON railway_order USING btree (customer_order_id NULLS FIRST);


--
-- TOC entry 2714 (class 1259 OID 21221)
-- Name: railway_order_sector_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_order_sector_idx ON railway_order USING btree (sector_id);


--
-- TOC entry 2715 (class 1259 OID 21222)
-- Name: railway_order_state_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_order_state_idx ON railway_order USING btree (state);


--
-- TOC entry 2716 (class 1259 OID 21223)
-- Name: railway_order_valid_until_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_order_valid_until_idx ON railway_order USING btree (valid_until);


--
-- TOC entry 2702 (class 1259 OID 21224)
-- Name: railway_period_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_period_idx ON railway USING btree (start_date, end_date NULLS FIRST);


--
-- TOC entry 2717 (class 1259 OID 21225)
-- Name: railway_property_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX railway_property_idx ON railway_property USING btree (railway_id, type);


--
-- TOC entry 2718 (class 1259 OID 21226)
-- Name: railway_property_railway_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_property_railway_idx ON railway_property USING btree (railway_id);


--
-- TOC entry 2719 (class 1259 OID 21227)
-- Name: railway_property_type_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_property_type_idx ON railway_property USING btree (type);


--
-- TOC entry 2720 (class 1259 OID 21228)
-- Name: railway_property_value_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_property_value_idx ON railway_property USING btree (value);


--
-- TOC entry 2738 (class 1259 OID 30801)
-- Name: railway_tax_amount_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_tax_amount_idx ON tax_amount USING btree (ticket_id, tax_id);


--
-- TOC entry 2739 (class 1259 OID 21230)
-- Name: railway_tax_amount_tax_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_tax_amount_tax_idx ON tax_amount USING btree (tax_id);


--
-- TOC entry 2740 (class 1259 OID 21231)
-- Name: railway_tax_amount_ticket_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX railway_tax_amount_ticket_idx ON tax_amount USING btree (ticket_id);


--
-- TOC entry 2721 (class 1259 OID 21232)
-- Name: route_date_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX route_date_idx ON route USING btree (departure_date, arrival_date);


--
-- TOC entry 2724 (class 1259 OID 21233)
-- Name: route_point_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX route_point_idx ON route USING btree (station_from_id, station_to_id);


--
-- TOC entry 2725 (class 1259 OID 21234)
-- Name: route_railway_order_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX route_railway_order_idx ON route USING btree (railway_order_id);


--
-- TOC entry 2726 (class 1259 OID 21235)
-- Name: station_city_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX station_city_idx ON station USING btree (city_id);


--
-- TOC entry 2727 (class 1259 OID 21236)
-- Name: station_full_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX station_full_idx ON station USING btree (city_id, start_date, end_date NULLS FIRST);


--
-- TOC entry 2728 (class 1259 OID 21237)
-- Name: station_period_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX station_period_idx ON station USING btree (start_date, end_date NULLS FIRST);


--
-- TOC entry 2734 (class 1259 OID 21238)
-- Name: station_property_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX station_property_idx ON station_property USING btree (station_id, type);


--
-- TOC entry 2735 (class 1259 OID 21239)
-- Name: station_property_station_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX station_property_station_idx ON station_property USING btree (station_id);


--
-- TOC entry 2736 (class 1259 OID 21240)
-- Name: station_property_type_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX station_property_type_idx ON station_property USING btree (type);


--
-- TOC entry 2737 (class 1259 OID 21241)
-- Name: station_property_value_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX station_property_value_idx ON station_property USING btree (value);


--
-- TOC entry 2733 (class 1259 OID 21242)
-- Name: station_railway_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX station_railway_idx ON station USING btree (railway_id);


--
-- TOC entry 2743 (class 1259 OID 21243)
-- Name: ticket_consumer_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX ticket_consumer_idx ON ticket USING btree (consumer_id);


--
-- TOC entry 2744 (class 1259 OID 21244)
-- Name: ticket_document_number_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX ticket_document_number_idx ON ticket USING btree (doc_number);


--
-- TOC entry 2745 (class 1259 OID 21245)
-- Name: ticket_document_type_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX ticket_document_type_idx ON ticket USING btree (doc_type);


--
-- TOC entry 2749 (class 1259 OID 21246)
-- Name: ticket_property_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX ticket_property_idx ON ticket_property USING btree (ticket_id, type);


--
-- TOC entry 2750 (class 1259 OID 21247)
-- Name: ticket_property_ticket_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX ticket_property_ticket_idx ON ticket_property USING btree (ticket_id);


--
-- TOC entry 2751 (class 1259 OID 21248)
-- Name: ticket_property_type_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX ticket_property_type_idx ON ticket_property USING btree (type);


--
-- TOC entry 2752 (class 1259 OID 21249)
-- Name: ticket_property_value_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX ticket_property_value_idx ON ticket_property USING btree (value);


--
-- TOC entry 2748 (class 1259 OID 21250)
-- Name: ticket_route_idx; Type: INDEX; Schema: railway; Owner: -; Tablespace: 
--

CREATE INDEX ticket_route_idx ON ticket USING btree (route_id);


--
-- TOC entry 2774 (class 2620 OID 21322)
-- Name: before_ticket_change; Type: TRIGGER; Schema: railway; Owner: -
--

CREATE TRIGGER before_ticket_change BEFORE INSERT OR UPDATE OF route_id, base_amount ON ticket FOR EACH ROW EXECUTE PROCEDURE before_ticket_change();


--
-- TOC entry 2772 (class 2620 OID 21323)
-- Name: railway_order_change; Type: TRIGGER; Schema: railway; Owner: -
--

CREATE TRIGGER railway_order_change AFTER INSERT OR DELETE OR UPDATE OF customer_order_id, amount ON railway_order FOR EACH ROW EXECUTE PROCEDURE financial.order_change();


--
-- TOC entry 2773 (class 2620 OID 21324)
-- Name: tax_amount_change; Type: TRIGGER; Schema: railway; Owner: -
--

CREATE TRIGGER tax_amount_change AFTER INSERT OR DELETE OR UPDATE OF ticket_id, tax_id, amount ON tax_amount FOR EACH ROW EXECUTE PROCEDURE tax_amount_change();


--
-- TOC entry 2775 (class 2620 OID 21325)
-- Name: ticket_change; Type: TRIGGER; Schema: railway; Owner: -
--

CREATE TRIGGER ticket_change AFTER INSERT OR DELETE OR UPDATE OF route_id, base_amount, amount ON ticket FOR EACH ROW EXECUTE PROCEDURE ticket_change();


--
-- TOC entry 2753 (class 2606 OID 22042)
-- Name: railway_company_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY railway
    ADD CONSTRAINT railway_company_id_fkey FOREIGN KEY (company_id) REFERENCES dictionary.company(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2754 (class 2606 OID 22047)
-- Name: railway_country_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY railway
    ADD CONSTRAINT railway_country_id_fkey FOREIGN KEY (country_id) REFERENCES dictionary.country(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2755 (class 2606 OID 22052)
-- Name: railway_order_base_currency_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY railway_order
    ADD CONSTRAINT railway_order_base_currency_fkey FOREIGN KEY (base_currency) REFERENCES dictionary.currency(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2756 (class 2606 OID 22057)
-- Name: railway_order_company_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY railway_order
    ADD CONSTRAINT railway_order_company_id_fkey FOREIGN KEY (company_id) REFERENCES dictionary.company(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2757 (class 2606 OID 22062)
-- Name: railway_order_currency_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY railway_order
    ADD CONSTRAINT railway_order_currency_fkey FOREIGN KEY (currency) REFERENCES dictionary.currency(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2758 (class 2606 OID 22067)
-- Name: railway_order_customer_order_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY railway_order
    ADD CONSTRAINT railway_order_customer_order_id_fkey FOREIGN KEY (customer_order_id) REFERENCES financial.customer_order(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2759 (class 2606 OID 22072)
-- Name: railway_order_sector_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY railway_order
    ADD CONSTRAINT railway_order_sector_id_fkey FOREIGN KEY (sector_id) REFERENCES org_chart.sector(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2760 (class 2606 OID 22077)
-- Name: railway_property_railway_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY railway_property
    ADD CONSTRAINT railway_property_railway_id_fkey FOREIGN KEY (railway_id) REFERENCES railway(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2761 (class 2606 OID 22082)
-- Name: route_railway_order_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_railway_order_id_fkey FOREIGN KEY (railway_order_id) REFERENCES railway_order(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2762 (class 2606 OID 22087)
-- Name: route_station_from_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_station_from_id_fkey FOREIGN KEY (station_from_id) REFERENCES station(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2763 (class 2606 OID 22092)
-- Name: route_station_to_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_station_to_id_fkey FOREIGN KEY (station_to_id) REFERENCES station(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2764 (class 2606 OID 22097)
-- Name: station_city_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY station
    ADD CONSTRAINT station_city_id_fkey FOREIGN KEY (city_id) REFERENCES dictionary.city(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2766 (class 2606 OID 22102)
-- Name: station_property_station_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY station_property
    ADD CONSTRAINT station_property_station_id_fkey FOREIGN KEY (station_id) REFERENCES station(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2765 (class 2606 OID 22107)
-- Name: station_railway_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY station
    ADD CONSTRAINT station_railway_id_fkey FOREIGN KEY (railway_id) REFERENCES railway(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2767 (class 2606 OID 22112)
-- Name: tax_amount_tax_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_tax_id_fkey FOREIGN KEY (tax_id) REFERENCES dictionary.tax(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2768 (class 2606 OID 22117)
-- Name: tax_amount_ticket_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_ticket_id_fkey FOREIGN KEY (ticket_id) REFERENCES ticket(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2769 (class 2606 OID 22122)
-- Name: ticket_consumer_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY ticket
    ADD CONSTRAINT ticket_consumer_id_fkey FOREIGN KEY (consumer_id) REFERENCES dictionary.consumer(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2771 (class 2606 OID 22127)
-- Name: ticket_property_ticket_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY ticket_property
    ADD CONSTRAINT ticket_property_ticket_id_fkey FOREIGN KEY (ticket_id) REFERENCES ticket(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2770 (class 2606 OID 22132)
-- Name: ticket_route_id_fkey; Type: FK CONSTRAINT; Schema: railway; Owner: -
--

ALTER TABLE ONLY ticket
    ADD CONSTRAINT ticket_route_id_fkey FOREIGN KEY (route_id) REFERENCES route(id) ON UPDATE CASCADE ON DELETE RESTRICT;


-- Completed on 2015-11-25 12:28:05

--
-- PostgreSQL database dump complete
--

