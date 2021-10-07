--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.4
-- Dumped by pg_dump version 9.4.4
-- Started on 2015-11-25 12:25:13

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 32 (class 2615 OID 19549)
-- Name: bus; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA bus;


--
-- TOC entry 2861 (class 0 OID 0)
-- Dependencies: 32
-- Name: SCHEMA bus; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA bus IS 'The schema for information of bus business';


SET search_path = bus, pg_catalog;

--
-- TOC entry 458 (class 1255 OID 19581)
-- Name: add_tax_amount(bigint, bigint, numeric); Type: FUNCTION; Schema: bus; Owner: -
--

CREATE FUNCTION add_tax_amount(bigint, bigint, numeric) RETURNS void
    LANGUAGE plpgsql
    AS $_$
begin
	if ($1 < 50) then
		update bus.ticket set amount = amount + $3 where id = $2;
	else 
		update bus.ticket set base_amount = base_amount + $3 where id = $2;
	end if;
end $_$;


--
-- TOC entry 2862 (class 0 OID 0)
-- Dependencies: 458
-- Name: FUNCTION add_tax_amount(bigint, bigint, numeric); Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON FUNCTION add_tax_amount(bigint, bigint, numeric) IS 'Adds amount of tax to ticket amount';


--
-- TOC entry 453 (class 1255 OID 19582)
-- Name: add_ticket_amount(bigint, numeric, numeric); Type: FUNCTION; Schema: bus; Owner: -
--

CREATE FUNCTION add_ticket_amount(bigint, numeric, numeric) RETURNS void
    LANGUAGE sql
    AS $_$
	update bus.bus_order 
	set 
		base_amount = base_amount + $2,
		amount = amount + $3
	where id = (select bus_order_id from bus.route where id = $1);
$_$;


--
-- TOC entry 2863 (class 0 OID 0)
-- Dependencies: 453
-- Name: FUNCTION add_ticket_amount(bigint, numeric, numeric); Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON FUNCTION add_ticket_amount(bigint, numeric, numeric) IS 'Adds base_amount and amount of ticket to bus_order';


--
-- TOC entry 459 (class 1255 OID 19583)
-- Name: before_ticket_change(); Type: FUNCTION; Schema: bus; Owner: -
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
-- TOC entry 2864 (class 0 OID 0)
-- Dependencies: 459
-- Name: FUNCTION before_ticket_change(); Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON FUNCTION before_ticket_change() IS 'Updates amount in ticket before ticket is changed';


--
-- TOC entry 454 (class 1255 OID 19584)
-- Name: rate_by_route(bigint, timestamp without time zone); Type: FUNCTION; Schema: bus; Owner: -
--

CREATE FUNCTION rate_by_route(bigint, timestamp without time zone) RETURNS numeric
    LANGUAGE plpgsql
    AS $_$
declare fin_rate numeric;
begin
	select financial.rate(o.base_currency, $2) into fin_rate from bus.route r
	left join bus.bus_order o on o.id = r.bus_order_id
	where r.id = $1;
	return coalesce(fin_rate, 1);
end $_$;


--
-- TOC entry 2865 (class 0 OID 0)
-- Dependencies: 454
-- Name: FUNCTION rate_by_route(bigint, timestamp without time zone); Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON FUNCTION rate_by_route(bigint, timestamp without time zone) IS 'Calculate currency exchange rate by route_id';


--
-- TOC entry 455 (class 1255 OID 19585)
-- Name: rate_by_ticket(bigint); Type: FUNCTION; Schema: bus; Owner: -
--

CREATE FUNCTION rate_by_ticket(bigint) RETURNS numeric
    LANGUAGE plpgsql
    AS $_$
declare fin_rate numeric;
begin
	select bus.rate_by_route(route_id, created) into fin_rate from bus.ticket where id = $1;
	return coalesce(fin_rate, 1);
end $_$;


--
-- TOC entry 2866 (class 0 OID 0)
-- Dependencies: 455
-- Name: FUNCTION rate_by_ticket(bigint); Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON FUNCTION rate_by_ticket(bigint) IS 'Calculate currency exchange rate by ticket_id';


--
-- TOC entry 456 (class 1255 OID 19586)
-- Name: tax_amount_change(); Type: FUNCTION; Schema: bus; Owner: -
--

CREATE FUNCTION tax_amount_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'DELETE') then
		perform bus.add_tax_amount(OLD.tax_id, OLD.ticket_id, -1 * OLD.amount);
	elsif (TG_OP = 'INSERT') then
		perform bus.add_tax_amount(NEW.tax_id, NEW.ticket_id, NEW.amount);
	elsif (TG_OP = 'UPDATE') then
		if (OLD.ticket_id != NEW.ticket_id or OLD.tax_id != NEW.tax_id) then
			perform bus.add_tax_amount(OLD.tax_id, OLD.ticket_id, -1 * OLD.amount);
			perform bus.add_tax_amount(NEW.tax_id, NEW.ticket_id, NEW.amount);
		elsif (OLD.amount != NEW.amount) then
			perform bus.add_tax_amount(NEW.tax_id, NEW.ticket_id, NEW.amount - OLD.amount);
		end if;
	end if;
	return null;
end $$;


--
-- TOC entry 2867 (class 0 OID 0)
-- Dependencies: 456
-- Name: FUNCTION tax_amount_change(); Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON FUNCTION tax_amount_change() IS 'Updates amount in ticket when tax_amount is changed';


--
-- TOC entry 457 (class 1255 OID 19587)
-- Name: ticket_change(); Type: FUNCTION; Schema: bus; Owner: -
--

CREATE FUNCTION ticket_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'DELETE') then
		perform bus.add_ticket_amount(OLD.route_id, -1 * OLD.base_amount, -1 * OLD.amount);
	elsif (TG_OP = 'INSERT') then
		perform bus.add_ticket_amount(NEW.route_id, NEW.base_amount, NEW.amount);
	elsif (TG_OP = 'UPDATE') then
		if (OLD.route_id != NEW.route_id) then
			perform bus.add_ticket_amount(OLD.route_id, -1 * OLD.base_amount, -1 * OLD.amount);
			perform bus.add_ticket_amount(NEW.route_id, NEW.base_amount, NEW.amount);
		elsif (OLD.base_amount != NEW.base_amount or OLD.amount != NEW.amount) then
			perform bus.add_ticket_amount(NEW.route_id, NEW.base_amount - OLD.base_amount, NEW.amount - OLD.amount);
		end if;
	end if;
	return null;
end $$;


--
-- TOC entry 2868 (class 0 OID 0)
-- Dependencies: 457
-- Name: FUNCTION ticket_change(); Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON FUNCTION ticket_change() IS 'Updates amount in bus_order when ticket is changed';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 246 (class 1259 OID 19695)
-- Name: bus_order; Type: TABLE; Schema: bus; Owner: -; Tablespace: 
--

CREATE TABLE bus_order (
    id bigint NOT NULL,
    sector_id bigint DEFAULT 31 NOT NULL,
    customer_order_id bigint,
    company_id bigint NOT NULL,
    doc_number character varying NOT NULL,
    state public.property_type,
    base_currency bigint NOT NULL,
    base_amount numeric DEFAULT 0 NOT NULL,
    currency bigint NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    description character varying,
    valid_until timestamp without time zone NOT NULL,
    created timestamp without time zone DEFAULT now() NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL
);


--
-- TOC entry 2869 (class 0 OID 0)
-- Dependencies: 246
-- Name: TABLE bus_order; Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON TABLE bus_order IS 'The orders for buses';


--
-- TOC entry 247 (class 1259 OID 19706)
-- Name: bus_order_id_seq; Type: SEQUENCE; Schema: bus; Owner: -
--

CREATE SEQUENCE bus_order_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2870 (class 0 OID 0)
-- Dependencies: 247
-- Name: bus_order_id_seq; Type: SEQUENCE OWNED BY; Schema: bus; Owner: -
--

ALTER SEQUENCE bus_order_id_seq OWNED BY bus_order.id;


--
-- TOC entry 248 (class 1259 OID 19708)
-- Name: route; Type: TABLE; Schema: bus; Owner: -; Tablespace: 
--

CREATE TABLE route (
    id bigint NOT NULL,
    bus_order_id bigint NOT NULL,
    trip_number character varying,
    station_from_id bigint NOT NULL,
    departure_date timestamp without time zone NOT NULL,
    station_to_id bigint NOT NULL,
    arrival_date timestamp without time zone NOT NULL,
    created timestamp without time zone DEFAULT now() NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL
);


--
-- TOC entry 2871 (class 0 OID 0)
-- Dependencies: 248
-- Name: TABLE route; Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON TABLE route IS 'The bus routes';


--
-- TOC entry 249 (class 1259 OID 19716)
-- Name: route_id_seq; Type: SEQUENCE; Schema: bus; Owner: -
--

CREATE SEQUENCE route_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2872 (class 0 OID 0)
-- Dependencies: 249
-- Name: route_id_seq; Type: SEQUENCE OWNED BY; Schema: bus; Owner: -
--

ALTER SEQUENCE route_id_seq OWNED BY route.id;


--
-- TOC entry 250 (class 1259 OID 19718)
-- Name: station; Type: TABLE; Schema: bus; Owner: -; Tablespace: 
--

CREATE TABLE station (
    id bigint NOT NULL,
    city_id bigint,
    bus_code character varying(10) NOT NULL,
    start_date date DEFAULT now() NOT NULL,
    end_date date
);


--
-- TOC entry 2873 (class 0 OID 0)
-- Dependencies: 250
-- Name: TABLE station; Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON TABLE station IS 'The stations dictionary';


--
-- TOC entry 251 (class 1259 OID 19722)
-- Name: station_id_seq; Type: SEQUENCE; Schema: bus; Owner: -
--

CREATE SEQUENCE station_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2874 (class 0 OID 0)
-- Dependencies: 251
-- Name: station_id_seq; Type: SEQUENCE OWNED BY; Schema: bus; Owner: -
--

ALTER SEQUENCE station_id_seq OWNED BY station.id;


--
-- TOC entry 252 (class 1259 OID 19724)
-- Name: station_property; Type: TABLE; Schema: bus; Owner: -; Tablespace: 
--

CREATE TABLE station_property (
    station_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


--
-- TOC entry 2875 (class 0 OID 0)
-- Dependencies: 252
-- Name: TABLE station_property; Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON TABLE station_property IS 'Some properties of station';


--
-- TOC entry 253 (class 1259 OID 19730)
-- Name: tax_amount; Type: TABLE; Schema: bus; Owner: -; Tablespace: 
--

CREATE TABLE tax_amount (
    id bigint NOT NULL,
    ticket_id bigint NOT NULL,
    tax_id bigint NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    created timestamp without time zone NOT NULL
);


--
-- TOC entry 2876 (class 0 OID 0)
-- Dependencies: 253
-- Name: TABLE tax_amount; Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON TABLE tax_amount IS 'The taxes of bus ticket';


--
-- TOC entry 254 (class 1259 OID 19737)
-- Name: tax_amount_id_seq; Type: SEQUENCE; Schema: bus; Owner: -
--

CREATE SEQUENCE tax_amount_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2877 (class 0 OID 0)
-- Dependencies: 254
-- Name: tax_amount_id_seq; Type: SEQUENCE OWNED BY; Schema: bus; Owner: -
--

ALTER SEQUENCE tax_amount_id_seq OWNED BY tax_amount.id;


--
-- TOC entry 255 (class 1259 OID 19739)
-- Name: ticket; Type: TABLE; Schema: bus; Owner: -; Tablespace: 
--

CREATE TABLE ticket (
    id bigint NOT NULL,
    route_id bigint NOT NULL,
    consumer_id bigint NOT NULL,
    state public.property_type,
    doc_number character varying(255) NOT NULL,
    seat_num character varying(10) NOT NULL,
    base_amount numeric DEFAULT 0 NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    tariff character varying NOT NULL,
    created timestamp without time zone DEFAULT now() NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL
);


--
-- TOC entry 2878 (class 0 OID 0)
-- Dependencies: 255
-- Name: TABLE ticket; Type: COMMENT; Schema: bus; Owner: -
--

COMMENT ON TABLE ticket IS 'The bus ticket';


--
-- TOC entry 256 (class 1259 OID 19749)
-- Name: ticket_id_seq; Type: SEQUENCE; Schema: bus; Owner: -
--

CREATE SEQUENCE ticket_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2879 (class 0 OID 0)
-- Dependencies: 256
-- Name: ticket_id_seq; Type: SEQUENCE OWNED BY; Schema: bus; Owner: -
--

ALTER SEQUENCE ticket_id_seq OWNED BY ticket.id;


--
-- TOC entry 2680 (class 2604 OID 20609)
-- Name: id; Type: DEFAULT; Schema: bus; Owner: -
--

ALTER TABLE ONLY bus_order ALTER COLUMN id SET DEFAULT nextval('bus_order_id_seq'::regclass);


--
-- TOC entry 2683 (class 2604 OID 20610)
-- Name: id; Type: DEFAULT; Schema: bus; Owner: -
--

ALTER TABLE ONLY route ALTER COLUMN id SET DEFAULT nextval('route_id_seq'::regclass);


--
-- TOC entry 2684 (class 2604 OID 20611)
-- Name: id; Type: DEFAULT; Schema: bus; Owner: -
--

ALTER TABLE ONLY station ALTER COLUMN id SET DEFAULT nextval('station_id_seq'::regclass);


--
-- TOC entry 2686 (class 2604 OID 20612)
-- Name: id; Type: DEFAULT; Schema: bus; Owner: -
--

ALTER TABLE ONLY tax_amount ALTER COLUMN id SET DEFAULT nextval('tax_amount_id_seq'::regclass);


--
-- TOC entry 2692 (class 2604 OID 20613)
-- Name: id; Type: DEFAULT; Schema: bus; Owner: -
--

ALTER TABLE ONLY ticket ALTER COLUMN id SET DEFAULT nextval('ticket_id_seq'::regclass);


--
-- TOC entry 2699 (class 2606 OID 20687)
-- Name: bus_order_pkey; Type: CONSTRAINT; Schema: bus; Owner: -; Tablespace: 
--

ALTER TABLE ONLY bus_order
    ADD CONSTRAINT bus_order_pkey PRIMARY KEY (id);


--
-- TOC entry 2706 (class 2606 OID 20689)
-- Name: route_pkey; Type: CONSTRAINT; Schema: bus; Owner: -; Tablespace: 
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_pkey PRIMARY KEY (id);


--
-- TOC entry 2709 (class 2606 OID 20691)
-- Name: station_bus_code_key; Type: CONSTRAINT; Schema: bus; Owner: -; Tablespace: 
--

ALTER TABLE ONLY station
    ADD CONSTRAINT station_bus_code_key UNIQUE (bus_code);


--
-- TOC entry 2714 (class 2606 OID 20693)
-- Name: station_pkey; Type: CONSTRAINT; Schema: bus; Owner: -; Tablespace: 
--

ALTER TABLE ONLY station
    ADD CONSTRAINT station_pkey PRIMARY KEY (id);


--
-- TOC entry 2723 (class 2606 OID 20695)
-- Name: tax_amount_pkey; Type: CONSTRAINT; Schema: bus; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_pkey PRIMARY KEY (id);


--
-- TOC entry 2727 (class 2606 OID 20697)
-- Name: ticket_pkey; Type: CONSTRAINT; Schema: bus; Owner: -; Tablespace: 
--

ALTER TABLE ONLY ticket
    ADD CONSTRAINT ticket_pkey PRIMARY KEY (id);


--
-- TOC entry 2693 (class 1259 OID 20918)
-- Name: bus_order_base_currency_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_order_base_currency_idx ON bus_order USING btree (base_currency);


--
-- TOC entry 2694 (class 1259 OID 20919)
-- Name: bus_order_company_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_order_company_idx ON bus_order USING btree (company_id);


--
-- TOC entry 2695 (class 1259 OID 20920)
-- Name: bus_order_created_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_order_created_idx ON bus_order USING btree (created);


--
-- TOC entry 2696 (class 1259 OID 20921)
-- Name: bus_order_currency_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_order_currency_idx ON bus_order USING btree (currency);


--
-- TOC entry 2697 (class 1259 OID 20922)
-- Name: bus_order_customer_order_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_order_customer_order_idx ON bus_order USING btree (customer_order_id NULLS FIRST);


--
-- TOC entry 2700 (class 1259 OID 20923)
-- Name: bus_order_sector_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_order_sector_idx ON bus_order USING btree (sector_id);


--
-- TOC entry 2701 (class 1259 OID 20924)
-- Name: bus_order_state_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_order_state_idx ON bus_order USING btree (state);


--
-- TOC entry 2702 (class 1259 OID 20925)
-- Name: bus_order_valid_until_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_order_valid_until_idx ON bus_order USING btree (valid_until);


--
-- TOC entry 2719 (class 1259 OID 30754)
-- Name: bus_tax_amount_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_tax_amount_idx ON tax_amount USING btree (ticket_id, tax_id);


--
-- TOC entry 2720 (class 1259 OID 20927)
-- Name: bus_tax_amount_tax_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_tax_amount_tax_idx ON tax_amount USING btree (tax_id);


--
-- TOC entry 2721 (class 1259 OID 20928)
-- Name: bus_tax_amount_ticket_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX bus_tax_amount_ticket_idx ON tax_amount USING btree (ticket_id);


--
-- TOC entry 2703 (class 1259 OID 20929)
-- Name: route_bus_order_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX route_bus_order_idx ON route USING btree (bus_order_id);


--
-- TOC entry 2704 (class 1259 OID 20930)
-- Name: route_date_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX route_date_idx ON route USING btree (departure_date, arrival_date);


--
-- TOC entry 2707 (class 1259 OID 20931)
-- Name: route_point_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX route_point_idx ON route USING btree (station_from_id, station_to_id);


--
-- TOC entry 2710 (class 1259 OID 20932)
-- Name: station_city_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX station_city_idx ON station USING btree (city_id NULLS FIRST);


--
-- TOC entry 2711 (class 1259 OID 20933)
-- Name: station_full_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX station_full_idx ON station USING btree (city_id, start_date, end_date NULLS FIRST);


--
-- TOC entry 2712 (class 1259 OID 20934)
-- Name: station_period_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX station_period_idx ON station USING btree (start_date, end_date NULLS FIRST);


--
-- TOC entry 2715 (class 1259 OID 20935)
-- Name: station_property_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX station_property_idx ON station_property USING btree (station_id, type);


--
-- TOC entry 2716 (class 1259 OID 20936)
-- Name: station_property_station_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX station_property_station_idx ON station_property USING btree (station_id);


--
-- TOC entry 2717 (class 1259 OID 20937)
-- Name: station_property_type_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX station_property_type_idx ON station_property USING btree (type);


--
-- TOC entry 2718 (class 1259 OID 20938)
-- Name: station_property_value_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX station_property_value_idx ON station_property USING btree (value);


--
-- TOC entry 2724 (class 1259 OID 20939)
-- Name: ticket_consumer_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX ticket_consumer_idx ON ticket USING btree (consumer_id);


--
-- TOC entry 2725 (class 1259 OID 20940)
-- Name: ticket_document_number_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX ticket_document_number_idx ON ticket USING btree (doc_number);


--
-- TOC entry 2728 (class 1259 OID 20941)
-- Name: ticket_route_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX ticket_route_idx ON ticket USING btree (route_id);


--
-- TOC entry 2729 (class 1259 OID 20942)
-- Name: ticket_state_idx; Type: INDEX; Schema: bus; Owner: -; Tablespace: 
--

CREATE INDEX ticket_state_idx ON ticket USING btree (state);


--
-- TOC entry 2746 (class 2620 OID 21307)
-- Name: before_ticket_change; Type: TRIGGER; Schema: bus; Owner: -
--

CREATE TRIGGER before_ticket_change BEFORE INSERT OR UPDATE OF route_id, base_amount ON ticket FOR EACH ROW EXECUTE PROCEDURE before_ticket_change();


--
-- TOC entry 2744 (class 2620 OID 21308)
-- Name: bus_order_change; Type: TRIGGER; Schema: bus; Owner: -
--

CREATE TRIGGER bus_order_change AFTER INSERT OR DELETE OR UPDATE OF customer_order_id, amount ON bus_order FOR EACH ROW EXECUTE PROCEDURE financial.order_change();


--
-- TOC entry 2745 (class 2620 OID 21309)
-- Name: tax_amount_change; Type: TRIGGER; Schema: bus; Owner: -
--

CREATE TRIGGER tax_amount_change AFTER INSERT OR DELETE OR UPDATE OF ticket_id, tax_id, amount ON tax_amount FOR EACH ROW EXECUTE PROCEDURE tax_amount_change();


--
-- TOC entry 2747 (class 2620 OID 21310)
-- Name: ticket_change; Type: TRIGGER; Schema: bus; Owner: -
--

CREATE TRIGGER ticket_change AFTER INSERT OR DELETE OR UPDATE OF route_id, base_amount, amount ON ticket FOR EACH ROW EXECUTE PROCEDURE ticket_change();


--
-- TOC entry 2730 (class 2606 OID 21427)
-- Name: bus_order_base_currency_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY bus_order
    ADD CONSTRAINT bus_order_base_currency_fkey FOREIGN KEY (base_currency) REFERENCES dictionary.currency(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2731 (class 2606 OID 21432)
-- Name: bus_order_company_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY bus_order
    ADD CONSTRAINT bus_order_company_id_fkey FOREIGN KEY (company_id) REFERENCES dictionary.company(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2732 (class 2606 OID 21437)
-- Name: bus_order_currency_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY bus_order
    ADD CONSTRAINT bus_order_currency_fkey FOREIGN KEY (currency) REFERENCES dictionary.currency(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2733 (class 2606 OID 21442)
-- Name: bus_order_customer_order_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY bus_order
    ADD CONSTRAINT bus_order_customer_order_id_fkey FOREIGN KEY (customer_order_id) REFERENCES financial.customer_order(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2734 (class 2606 OID 21447)
-- Name: bus_order_sector_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY bus_order
    ADD CONSTRAINT bus_order_sector_id_fkey FOREIGN KEY (sector_id) REFERENCES org_chart.sector(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2735 (class 2606 OID 21452)
-- Name: route_bus_order_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_bus_order_id_fkey FOREIGN KEY (bus_order_id) REFERENCES bus_order(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2736 (class 2606 OID 21457)
-- Name: route_station_from_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_station_from_id_fkey FOREIGN KEY (station_from_id) REFERENCES station(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2737 (class 2606 OID 21462)
-- Name: route_station_to_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY route
    ADD CONSTRAINT route_station_to_id_fkey FOREIGN KEY (station_to_id) REFERENCES station(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2738 (class 2606 OID 21467)
-- Name: station_city_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY station
    ADD CONSTRAINT station_city_id_fkey FOREIGN KEY (city_id) REFERENCES dictionary.city(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2739 (class 2606 OID 21472)
-- Name: station_property_station_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY station_property
    ADD CONSTRAINT station_property_station_id_fkey FOREIGN KEY (station_id) REFERENCES station(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2740 (class 2606 OID 21477)
-- Name: tax_amount_tax_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_tax_id_fkey FOREIGN KEY (tax_id) REFERENCES dictionary.tax(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2741 (class 2606 OID 21482)
-- Name: tax_amount_ticket_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_ticket_id_fkey FOREIGN KEY (ticket_id) REFERENCES ticket(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2742 (class 2606 OID 21487)
-- Name: ticket_consumer_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY ticket
    ADD CONSTRAINT ticket_consumer_id_fkey FOREIGN KEY (consumer_id) REFERENCES dictionary.consumer(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2743 (class 2606 OID 21492)
-- Name: ticket_route_id_fkey; Type: FK CONSTRAINT; Schema: bus; Owner: -
--

ALTER TABLE ONLY ticket
    ADD CONSTRAINT ticket_route_id_fkey FOREIGN KEY (route_id) REFERENCES route(id) ON UPDATE CASCADE ON DELETE RESTRICT;


-- Completed on 2015-11-25 12:25:13

--
-- PostgreSQL database dump complete
--

