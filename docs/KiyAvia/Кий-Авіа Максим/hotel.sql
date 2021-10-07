--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.4
-- Dumped by pg_dump version 9.4.4
-- Started on 2015-11-25 12:26:26

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 36 (class 2615 OID 19553)
-- Name: hotel; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA hotel;


--
-- TOC entry 2849 (class 0 OID 0)
-- Dependencies: 36
-- Name: SCHEMA hotel; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA hotel IS 'The schema for information of hotel business';


SET search_path = hotel, pg_catalog;

--
-- TOC entry 465 (class 1255 OID 19593)
-- Name: add_service_amount(bigint, numeric, numeric); Type: FUNCTION; Schema: hotel; Owner: -
--

CREATE FUNCTION add_service_amount(bigint, numeric, numeric) RETURNS void
    LANGUAGE sql
    AS $_$
	update hotel.hotel_order 
	set 
		base_amount = base_amount + $2,
		amount = amount + $3
	where id = $1;
$_$;


--
-- TOC entry 2850 (class 0 OID 0)
-- Dependencies: 465
-- Name: FUNCTION add_service_amount(bigint, numeric, numeric); Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON FUNCTION add_service_amount(bigint, numeric, numeric) IS 'Adds base_amount and amount of service to hotel_order';


--
-- TOC entry 469 (class 1255 OID 19594)
-- Name: add_tax_amount(bigint, bigint, numeric); Type: FUNCTION; Schema: hotel; Owner: -
--

CREATE FUNCTION add_tax_amount(bigint, bigint, numeric) RETURNS void
    LANGUAGE plpgsql
    AS $_$
begin
	if ($1 < 50) then
		update hotel.service set amount = amount + $3 where id = $2;
	else 
		update hotel.service set base_amount = base_amount + $3 where id = $2;
	end if;
end $_$;


--
-- TOC entry 2851 (class 0 OID 0)
-- Dependencies: 469
-- Name: FUNCTION add_tax_amount(bigint, bigint, numeric); Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON FUNCTION add_tax_amount(bigint, bigint, numeric) IS 'Adds amount of tax to service amount';


--
-- TOC entry 470 (class 1255 OID 19595)
-- Name: before_service_change(); Type: FUNCTION; Schema: hotel; Owner: -
--

CREATE FUNCTION before_service_change() RETURNS trigger
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
-- TOC entry 2852 (class 0 OID 0)
-- Dependencies: 470
-- Name: FUNCTION before_service_change(); Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON FUNCTION before_service_change() IS 'Updates amount in service before service is changed';


--
-- TOC entry 466 (class 1255 OID 19596)
-- Name: rate_by_order(bigint, timestamp without time zone); Type: FUNCTION; Schema: hotel; Owner: -
--

CREATE FUNCTION rate_by_order(bigint, timestamp without time zone) RETURNS numeric
    LANGUAGE plpgsql
    AS $_$
declare fin_rate numeric;
begin
	select financial.rate(currency, $2) into fin_rate from hotel.hotel_order where id = $1;
	return coalesce(fin_rate, 1);
end $_$;


--
-- TOC entry 2853 (class 0 OID 0)
-- Dependencies: 466
-- Name: FUNCTION rate_by_order(bigint, timestamp without time zone); Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON FUNCTION rate_by_order(bigint, timestamp without time zone) IS 'Calculate currency exchange rate by hotel_order_id';


--
-- TOC entry 467 (class 1255 OID 19597)
-- Name: rate_by_service(bigint); Type: FUNCTION; Schema: hotel; Owner: -
--

CREATE FUNCTION rate_by_service(bigint) RETURNS numeric
    LANGUAGE plpgsql
    AS $_$
declare fin_rate numeric;
begin
	select hotel.rate_by_order(hotel_order_id, created) into fin_rate from hotel.service where id = $1;
	return coalesce(fin_rate, 1);
end $_$;


--
-- TOC entry 2854 (class 0 OID 0)
-- Dependencies: 467
-- Name: FUNCTION rate_by_service(bigint); Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON FUNCTION rate_by_service(bigint) IS 'Calculate currency exchange rate by service_id';


--
-- TOC entry 472 (class 1255 OID 19598)
-- Name: service_change(); Type: FUNCTION; Schema: hotel; Owner: -
--

CREATE FUNCTION service_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'DELETE') then
		perform hotel.add_service_amount(OLD.hotel_order_id, -1 * OLD.base_amount, -1 * OLD.amount);
	elsif (TG_OP = 'INSERT') then
		perform hotel.add_service_amount(NEW.hotel_order_id, NEW.base_amount, NEW.amount);
	elsif (TG_OP = 'UPDATE') then
		if (OLD.hotel_order_id != NEW.hotel_order_id) then
			perform hotel.add_service_amount(OLD.hotel_order_id, -1 * OLD.base_amount, -1 * OLD.amount);
			perform hotel.add_service_amount(NEW.hotel_order_id, NEW.base_amount, NEW.amount);
		elsif (OLD.base_amount != NEW.base_amount or OLD.amount != NEW.amount) then
			perform hotel.add_service_amount(NEW.hotel_order_id, NEW.base_amount - OLD.base_amount, NEW.amount - OLD.amount);
		end if;
	end if;
	return null;
end $$;


--
-- TOC entry 2855 (class 0 OID 0)
-- Dependencies: 472
-- Name: FUNCTION service_change(); Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON FUNCTION service_change() IS 'Updates amount in railway_order when service is changed';


--
-- TOC entry 473 (class 1255 OID 19599)
-- Name: tax_amount_change(); Type: FUNCTION; Schema: hotel; Owner: -
--

CREATE FUNCTION tax_amount_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'DELETE') then
		perform hotel.add_tax_amount(OLD.tax_id, OLD.service_id, -1 * OLD.amount);
	elsif (TG_OP = 'INSERT') then
		perform hotel.add_tax_amount(NEW.tax_id, NEW.service_id, NEW.amount);
	elsif (TG_OP = 'UPDATE') then
		if (OLD.service_id != NEW.service_id or OLD.tax_id != NEW.tax_id) then
			perform hotel.add_tax_amount(OLD.tax_id, OLD.service_id, -1 * OLD.amount);
			perform hotel.add_tax_amount(NEW.tax_id, NEW.service_id, NEW.amount);
		elsif (OLD.amount != NEW.amount) then
			perform hotel.add_tax_amount(NEW.tax_id, NEW.service_id, NEW.amount - OLD.amount);
		end if;
	end if;
	return null;
end $$;


--
-- TOC entry 2856 (class 0 OID 0)
-- Dependencies: 473
-- Name: FUNCTION tax_amount_change(); Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON FUNCTION tax_amount_change() IS 'Updates amount in service when tax_amount is changed';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 314 (class 1259 OID 20045)
-- Name: hotel; Type: TABLE; Schema: hotel; Owner: -; Tablespace: 
--

CREATE TABLE hotel (
    id bigint NOT NULL,
    city_id bigint NOT NULL,
    start_date date DEFAULT now() NOT NULL,
    end_date date
);


--
-- TOC entry 2857 (class 0 OID 0)
-- Dependencies: 314
-- Name: TABLE hotel; Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON TABLE hotel IS 'The hotels';


--
-- TOC entry 315 (class 1259 OID 20049)
-- Name: hotel_id_seq; Type: SEQUENCE; Schema: hotel; Owner: -
--

CREATE SEQUENCE hotel_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2858 (class 0 OID 0)
-- Dependencies: 315
-- Name: hotel_id_seq; Type: SEQUENCE OWNED BY; Schema: hotel; Owner: -
--

ALTER SEQUENCE hotel_id_seq OWNED BY hotel.id;


--
-- TOC entry 316 (class 1259 OID 20051)
-- Name: hotel_order; Type: TABLE; Schema: hotel; Owner: -; Tablespace: 
--

CREATE TABLE hotel_order (
    id bigint NOT NULL,
    sector_id bigint DEFAULT 41 NOT NULL,
    customer_order_id bigint,
    doc_number character varying(50) NOT NULL,
    state character varying(100) NOT NULL,
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
-- TOC entry 2859 (class 0 OID 0)
-- Dependencies: 316
-- Name: TABLE hotel_order; Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON TABLE hotel_order IS 'The orders for hotels';


--
-- TOC entry 317 (class 1259 OID 20062)
-- Name: hotel_order_id_seq; Type: SEQUENCE; Schema: hotel; Owner: -
--

CREATE SEQUENCE hotel_order_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2860 (class 0 OID 0)
-- Dependencies: 317
-- Name: hotel_order_id_seq; Type: SEQUENCE OWNED BY; Schema: hotel; Owner: -
--

ALTER SEQUENCE hotel_order_id_seq OWNED BY hotel_order.id;


--
-- TOC entry 318 (class 1259 OID 20064)
-- Name: hotel_property; Type: TABLE; Schema: hotel; Owner: -; Tablespace: 
--

CREATE TABLE hotel_property (
    hotel_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


--
-- TOC entry 2861 (class 0 OID 0)
-- Dependencies: 318
-- Name: TABLE hotel_property; Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON TABLE hotel_property IS 'Some properties of hotel';


--
-- TOC entry 319 (class 1259 OID 20070)
-- Name: service; Type: TABLE; Schema: hotel; Owner: -; Tablespace: 
--

CREATE TABLE service (
    id bigint NOT NULL,
    hotel_order_id bigint NOT NULL,
    consumer_id bigint NOT NULL,
    hotel_id bigint NOT NULL,
    state public.property_type,
    check_in date NOT NULL,
    check_out date NOT NULL,
    doc_number character varying(255),
    base_amount numeric DEFAULT 0 NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    show_vat boolean DEFAULT false NOT NULL,
    created timestamp without time zone DEFAULT now() NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL
);


--
-- TOC entry 2862 (class 0 OID 0)
-- Dependencies: 319
-- Name: TABLE service; Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON TABLE service IS 'The hotel service data info';


--
-- TOC entry 320 (class 1259 OID 20081)
-- Name: service_id_seq; Type: SEQUENCE; Schema: hotel; Owner: -
--

CREATE SEQUENCE service_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2863 (class 0 OID 0)
-- Dependencies: 320
-- Name: service_id_seq; Type: SEQUENCE OWNED BY; Schema: hotel; Owner: -
--

ALTER SEQUENCE service_id_seq OWNED BY service.id;


--
-- TOC entry 321 (class 1259 OID 20083)
-- Name: tax_amount; Type: TABLE; Schema: hotel; Owner: -; Tablespace: 
--

CREATE TABLE tax_amount (
    id bigint NOT NULL,
    service_id bigint NOT NULL,
    tax_id bigint NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    created timestamp without time zone NOT NULL
);


--
-- TOC entry 2864 (class 0 OID 0)
-- Dependencies: 321
-- Name: TABLE tax_amount; Type: COMMENT; Schema: hotel; Owner: -
--

COMMENT ON TABLE tax_amount IS 'The taxes of hotel service';


--
-- TOC entry 322 (class 1259 OID 20090)
-- Name: tax_amount_id_seq; Type: SEQUENCE; Schema: hotel; Owner: -
--

CREATE SEQUENCE tax_amount_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2865 (class 0 OID 0)
-- Dependencies: 322
-- Name: tax_amount_id_seq; Type: SEQUENCE OWNED BY; Schema: hotel; Owner: -
--

ALTER SEQUENCE tax_amount_id_seq OWNED BY tax_amount.id;


--
-- TOC entry 2674 (class 2604 OID 20633)
-- Name: id; Type: DEFAULT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY hotel ALTER COLUMN id SET DEFAULT nextval('hotel_id_seq'::regclass);


--
-- TOC entry 2681 (class 2604 OID 20634)
-- Name: id; Type: DEFAULT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY hotel_order ALTER COLUMN id SET DEFAULT nextval('hotel_order_id_seq'::regclass);


--
-- TOC entry 2687 (class 2604 OID 20635)
-- Name: id; Type: DEFAULT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY service ALTER COLUMN id SET DEFAULT nextval('service_id_seq'::regclass);


--
-- TOC entry 2688 (class 2604 OID 20636)
-- Name: id; Type: DEFAULT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY tax_amount ALTER COLUMN id SET DEFAULT nextval('tax_amount_id_seq'::regclass);


--
-- TOC entry 2700 (class 2606 OID 20769)
-- Name: hotel_order_pkey; Type: CONSTRAINT; Schema: hotel; Owner: -; Tablespace: 
--

ALTER TABLE ONLY hotel_order
    ADD CONSTRAINT hotel_order_pkey PRIMARY KEY (id);


--
-- TOC entry 2694 (class 2606 OID 20771)
-- Name: hotel_pkey; Type: CONSTRAINT; Schema: hotel; Owner: -; Tablespace: 
--

ALTER TABLE ONLY hotel
    ADD CONSTRAINT hotel_pkey PRIMARY KEY (id);


--
-- TOC entry 2711 (class 2606 OID 20773)
-- Name: service_doc_number_key; Type: CONSTRAINT; Schema: hotel; Owner: -; Tablespace: 
--

ALTER TABLE ONLY service
    ADD CONSTRAINT service_doc_number_key UNIQUE (doc_number);


--
-- TOC entry 2714 (class 2606 OID 20775)
-- Name: service_pkey; Type: CONSTRAINT; Schema: hotel; Owner: -; Tablespace: 
--

ALTER TABLE ONLY service
    ADD CONSTRAINT service_pkey PRIMARY KEY (id);


--
-- TOC entry 2720 (class 2606 OID 20777)
-- Name: tax_amount_pkey; Type: CONSTRAINT; Schema: hotel; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_pkey PRIMARY KEY (id);


--
-- TOC entry 2690 (class 1259 OID 21053)
-- Name: hotel_city_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_city_idx ON hotel USING btree (city_id);


--
-- TOC entry 2691 (class 1259 OID 21054)
-- Name: hotel_city_period_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_city_period_idx ON hotel USING btree (city_id, start_date, end_date NULLS FIRST);


--
-- TOC entry 2695 (class 1259 OID 21055)
-- Name: hotel_order_base_currency_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_order_base_currency_idx ON hotel_order USING btree (base_currency);


--
-- TOC entry 2696 (class 1259 OID 21056)
-- Name: hotel_order_created_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_order_created_idx ON hotel_order USING btree (created);


--
-- TOC entry 2697 (class 1259 OID 21057)
-- Name: hotel_order_currency_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_order_currency_idx ON hotel_order USING btree (currency);


--
-- TOC entry 2698 (class 1259 OID 21058)
-- Name: hotel_order_customer_order_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_order_customer_order_idx ON hotel_order USING btree (customer_order_id NULLS FIRST);


--
-- TOC entry 2701 (class 1259 OID 21059)
-- Name: hotel_order_sector_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_order_sector_idx ON hotel_order USING btree (sector_id);


--
-- TOC entry 2702 (class 1259 OID 21060)
-- Name: hotel_order_state_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_order_state_idx ON hotel_order USING btree (state);


--
-- TOC entry 2703 (class 1259 OID 21061)
-- Name: hotel_order_valid_until_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_order_valid_until_idx ON hotel_order USING btree (valid_until);


--
-- TOC entry 2692 (class 1259 OID 21062)
-- Name: hotel_period_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_period_idx ON hotel USING btree (start_date, end_date NULLS FIRST);


--
-- TOC entry 2704 (class 1259 OID 21063)
-- Name: hotel_property_hotel_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_property_hotel_idx ON hotel_property USING btree (hotel_id);


--
-- TOC entry 2705 (class 1259 OID 21064)
-- Name: hotel_property_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX hotel_property_idx ON hotel_property USING btree (hotel_id, type);


--
-- TOC entry 2706 (class 1259 OID 21065)
-- Name: hotel_property_type_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_property_type_idx ON hotel_property USING btree (type);


--
-- TOC entry 2707 (class 1259 OID 21066)
-- Name: hotel_property_value_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_property_value_idx ON hotel_property USING btree (value);


--
-- TOC entry 2716 (class 1259 OID 30777)
-- Name: hotel_tax_amount_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_tax_amount_idx ON tax_amount USING btree (service_id, tax_id);


--
-- TOC entry 2717 (class 1259 OID 21068)
-- Name: hotel_tax_amount_service_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_tax_amount_service_idx ON tax_amount USING btree (service_id);


--
-- TOC entry 2718 (class 1259 OID 21069)
-- Name: hotel_tax_amount_tax_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX hotel_tax_amount_tax_idx ON tax_amount USING btree (tax_id);


--
-- TOC entry 2708 (class 1259 OID 21070)
-- Name: service_consumer_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX service_consumer_idx ON service USING btree (consumer_id);


--
-- TOC entry 2709 (class 1259 OID 21071)
-- Name: service_created_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX service_created_idx ON service USING btree (created);


--
-- TOC entry 2712 (class 1259 OID 21072)
-- Name: service_hotel_order_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX service_hotel_order_idx ON service USING btree (hotel_order_id);


--
-- TOC entry 2715 (class 1259 OID 21073)
-- Name: service_state_idx; Type: INDEX; Schema: hotel; Owner: -; Tablespace: 
--

CREATE INDEX service_state_idx ON service USING btree (state);


--
-- TOC entry 2733 (class 2620 OID 21314)
-- Name: before_service_change; Type: TRIGGER; Schema: hotel; Owner: -
--

CREATE TRIGGER before_service_change BEFORE INSERT OR UPDATE OF hotel_order_id, base_amount ON service FOR EACH ROW EXECUTE PROCEDURE before_service_change();


--
-- TOC entry 2732 (class 2620 OID 21315)
-- Name: hotel_order_change; Type: TRIGGER; Schema: hotel; Owner: -
--

CREATE TRIGGER hotel_order_change AFTER INSERT OR DELETE OR UPDATE OF customer_order_id, amount ON hotel_order FOR EACH ROW EXECUTE PROCEDURE financial.order_change();


--
-- TOC entry 2734 (class 2620 OID 21316)
-- Name: service_change; Type: TRIGGER; Schema: hotel; Owner: -
--

CREATE TRIGGER service_change AFTER INSERT OR DELETE OR UPDATE OF hotel_order_id, base_amount, amount ON service FOR EACH ROW EXECUTE PROCEDURE service_change();


--
-- TOC entry 2735 (class 2620 OID 21317)
-- Name: tax_amount_change; Type: TRIGGER; Schema: hotel; Owner: -
--

CREATE TRIGGER tax_amount_change AFTER INSERT OR DELETE OR UPDATE OF service_id, tax_id, amount ON tax_amount FOR EACH ROW EXECUTE PROCEDURE tax_amount_change();


--
-- TOC entry 2721 (class 2606 OID 21732)
-- Name: hotel_city_id_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY hotel
    ADD CONSTRAINT hotel_city_id_fkey FOREIGN KEY (city_id) REFERENCES dictionary.city(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2722 (class 2606 OID 21737)
-- Name: hotel_order_base_currency_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY hotel_order
    ADD CONSTRAINT hotel_order_base_currency_fkey FOREIGN KEY (base_currency) REFERENCES dictionary.currency(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2723 (class 2606 OID 21742)
-- Name: hotel_order_currency_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY hotel_order
    ADD CONSTRAINT hotel_order_currency_fkey FOREIGN KEY (currency) REFERENCES dictionary.currency(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2724 (class 2606 OID 21747)
-- Name: hotel_order_customer_order_id_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY hotel_order
    ADD CONSTRAINT hotel_order_customer_order_id_fkey FOREIGN KEY (customer_order_id) REFERENCES financial.customer_order(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2725 (class 2606 OID 21752)
-- Name: hotel_order_sector_id_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY hotel_order
    ADD CONSTRAINT hotel_order_sector_id_fkey FOREIGN KEY (sector_id) REFERENCES org_chart.sector(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2726 (class 2606 OID 21757)
-- Name: hotel_property_hotel_id_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY hotel_property
    ADD CONSTRAINT hotel_property_hotel_id_fkey FOREIGN KEY (hotel_id) REFERENCES hotel(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2727 (class 2606 OID 21762)
-- Name: service_consumer_id_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY service
    ADD CONSTRAINT service_consumer_id_fkey FOREIGN KEY (consumer_id) REFERENCES dictionary.consumer(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2728 (class 2606 OID 21767)
-- Name: service_hotel_id_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY service
    ADD CONSTRAINT service_hotel_id_fkey FOREIGN KEY (hotel_id) REFERENCES hotel(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2729 (class 2606 OID 21772)
-- Name: service_hotel_order_id_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY service
    ADD CONSTRAINT service_hotel_order_id_fkey FOREIGN KEY (hotel_order_id) REFERENCES hotel_order(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2730 (class 2606 OID 21777)
-- Name: tax_amount_service_id_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_service_id_fkey FOREIGN KEY (service_id) REFERENCES service(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2731 (class 2606 OID 21782)
-- Name: tax_amount_tax_id_fkey; Type: FK CONSTRAINT; Schema: hotel; Owner: -
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_tax_id_fkey FOREIGN KEY (tax_id) REFERENCES dictionary.tax(id) ON UPDATE CASCADE ON DELETE RESTRICT;


-- Completed on 2015-11-25 12:26:26

--
-- PostgreSQL database dump complete
--

