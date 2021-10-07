--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.4
-- Dumped by pg_dump version 9.4.4
-- Started on 2015-11-25 12:26:54

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 45 (class 2615 OID 19554)
-- Name: insurance; Type: SCHEMA; Schema: -; Owner: -
--

CREATE SCHEMA insurance;


--
-- TOC entry 2847 (class 0 OID 0)
-- Dependencies: 45
-- Name: SCHEMA insurance; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON SCHEMA insurance IS 'The schema for information of insurance business';


SET search_path = insurance, pg_catalog;

--
-- TOC entry 474 (class 1255 OID 19600)
-- Name: add_service_amount(bigint, numeric, numeric); Type: FUNCTION; Schema: insurance; Owner: -
--

CREATE FUNCTION add_service_amount(bigint, numeric, numeric) RETURNS void
    LANGUAGE sql
    AS $_$
	update insurance.insurance_order 
	set 
		base_amount = base_amount + $2,
		amount = amount + $3
	where id = $1;
$_$;


--
-- TOC entry 2848 (class 0 OID 0)
-- Dependencies: 474
-- Name: FUNCTION add_service_amount(bigint, numeric, numeric); Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON FUNCTION add_service_amount(bigint, numeric, numeric) IS 'Adds base_amount and amount of service to insurance_order';


--
-- TOC entry 481 (class 1255 OID 19601)
-- Name: add_tax_amount(bigint, bigint, numeric); Type: FUNCTION; Schema: insurance; Owner: -
--

CREATE FUNCTION add_tax_amount(bigint, bigint, numeric) RETURNS void
    LANGUAGE plpgsql
    AS $_$
begin
	if ($1 < 50) then
		update insurance.service set amount = amount + $3 where id = $2;
	else 
		update insurance.service set base_amount = base_amount + $3 where id = $2;
	end if;
end $_$;


--
-- TOC entry 2849 (class 0 OID 0)
-- Dependencies: 481
-- Name: FUNCTION add_tax_amount(bigint, bigint, numeric); Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON FUNCTION add_tax_amount(bigint, bigint, numeric) IS 'Adds amount of tax to service amount';


--
-- TOC entry 482 (class 1255 OID 19602)
-- Name: before_service_change(); Type: FUNCTION; Schema: insurance; Owner: -
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
-- TOC entry 2850 (class 0 OID 0)
-- Dependencies: 482
-- Name: FUNCTION before_service_change(); Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON FUNCTION before_service_change() IS 'Updates amount in service before service is changed';


--
-- TOC entry 475 (class 1255 OID 19603)
-- Name: rate_by_order(bigint, timestamp without time zone); Type: FUNCTION; Schema: insurance; Owner: -
--

CREATE FUNCTION rate_by_order(bigint, timestamp without time zone) RETURNS numeric
    LANGUAGE plpgsql
    AS $_$
declare fin_rate numeric;
begin
	select financial.rate(currency, $2) into fin_rate from insurance.insurance_order where id = $1;
	return coalesce(fin_rate, 1);
end $_$;


--
-- TOC entry 2851 (class 0 OID 0)
-- Dependencies: 475
-- Name: FUNCTION rate_by_order(bigint, timestamp without time zone); Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON FUNCTION rate_by_order(bigint, timestamp without time zone) IS 'Calculate currency exchange rate by insurance_order_id';


--
-- TOC entry 476 (class 1255 OID 19604)
-- Name: rate_by_service(bigint); Type: FUNCTION; Schema: insurance; Owner: -
--

CREATE FUNCTION rate_by_service(bigint) RETURNS numeric
    LANGUAGE plpgsql
    AS $_$
declare fin_rate numeric;
begin
	select financial.rate(o.currency, s.created) into fin_rate from insurance.service s
	left join insurance.insurance_order o on o.id = s.insurance_order_id where s.id = $1;
	return coalesce(fin_rate, 1);
end $_$;


--
-- TOC entry 2852 (class 0 OID 0)
-- Dependencies: 476
-- Name: FUNCTION rate_by_service(bigint); Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON FUNCTION rate_by_service(bigint) IS 'Calculate currency exchange rate by service_id';


--
-- TOC entry 477 (class 1255 OID 19605)
-- Name: service_change(); Type: FUNCTION; Schema: insurance; Owner: -
--

CREATE FUNCTION service_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'DELETE') then
		perform insurance.add_service_amount(OLD.insurance_order_id, -1 * OLD.base_amount, -1 * OLD.amount);
	elsif (TG_OP = 'INSERT') then
		perform insurance.add_service_amount(NEW.insurance_order_id, NEW.base_amount, NEW.amount);
	elsif (TG_OP = 'UPDATE') then
		if (OLD.insurance_order_id != NEW.insurance_order_id) then
			perform insurance.add_service_amount(OLD.insurance_order_id, -1 * OLD.base_amount, -1 * OLD.amount);
			perform insurance.add_service_amount(NEW.insurance_order_id, NEW.base_amount, NEW.amount);
		elsif (OLD.base_amount != NEW.base_amount or OLD.amount != NEW.amount) then
			perform insurance.add_service_amount(NEW.insurance_order_id, NEW.base_amount - OLD.base_amount, NEW.amount - OLD.amount);
		end if;
	end if;
	return null;
end $$;


--
-- TOC entry 2853 (class 0 OID 0)
-- Dependencies: 477
-- Name: FUNCTION service_change(); Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON FUNCTION service_change() IS 'Updates amount in insurance_order when service is changed';


--
-- TOC entry 478 (class 1255 OID 19606)
-- Name: tax_amount_change(); Type: FUNCTION; Schema: insurance; Owner: -
--

CREATE FUNCTION tax_amount_change() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
begin
	if (TG_OP = 'DELETE') then
		perform insurance.add_tax_amount(OLD.tax_id, OLD.service_id, -1 * OLD.amount);
	elsif (TG_OP = 'INSERT') then
		perform insurance.add_tax_amount(NEW.tax_id, NEW.service_id, NEW.amount);
	elsif (TG_OP = 'UPDATE') then
		if (OLD.service_id != NEW.service_id or OLD.tax_id != NEW.tax_id) then
			perform insurance.add_tax_amount(OLD.tax_id, OLD.service_id, -1 * OLD.amount);
			perform insurance.add_tax_amount(NEW.tax_id, NEW.service_id, NEW.amount);
		elsif (OLD.amount != NEW.amount) then
			perform insurance.add_tax_amount(NEW.tax_id, NEW.service_id, NEW.amount - OLD.amount);
		end if;
	end if;
	return null;
end $$;


--
-- TOC entry 2854 (class 0 OID 0)
-- Dependencies: 478
-- Name: FUNCTION tax_amount_change(); Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON FUNCTION tax_amount_change() IS 'Updates amount in service when tax_amount is changed';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- TOC entry 323 (class 1259 OID 20092)
-- Name: insurance_order; Type: TABLE; Schema: insurance; Owner: -; Tablespace: 
--

CREATE TABLE insurance_order (
    id bigint NOT NULL,
    sector_id bigint DEFAULT 41 NOT NULL,
    customer_order_id bigint,
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
-- TOC entry 2855 (class 0 OID 0)
-- Dependencies: 323
-- Name: TABLE insurance_order; Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON TABLE insurance_order IS 'The orders for insurance';


--
-- TOC entry 324 (class 1259 OID 20103)
-- Name: insurance_order_id_seq; Type: SEQUENCE; Schema: insurance; Owner: -
--

CREATE SEQUENCE insurance_order_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2856 (class 0 OID 0)
-- Dependencies: 324
-- Name: insurance_order_id_seq; Type: SEQUENCE OWNED BY; Schema: insurance; Owner: -
--

ALTER SEQUENCE insurance_order_id_seq OWNED BY insurance_order.id;


--
-- TOC entry 325 (class 1259 OID 20105)
-- Name: program; Type: TABLE; Schema: insurance; Owner: -; Tablespace: 
--

CREATE TABLE program (
    id bigint NOT NULL,
    start_date date DEFAULT now() NOT NULL,
    end_date date
);


--
-- TOC entry 2857 (class 0 OID 0)
-- Dependencies: 325
-- Name: TABLE program; Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON TABLE program IS 'The insurance program dictionary';


--
-- TOC entry 326 (class 1259 OID 20109)
-- Name: program_id_seq; Type: SEQUENCE; Schema: insurance; Owner: -
--

CREATE SEQUENCE program_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2858 (class 0 OID 0)
-- Dependencies: 326
-- Name: program_id_seq; Type: SEQUENCE OWNED BY; Schema: insurance; Owner: -
--

ALTER SEQUENCE program_id_seq OWNED BY program.id;


--
-- TOC entry 327 (class 1259 OID 20111)
-- Name: program_property; Type: TABLE; Schema: insurance; Owner: -; Tablespace: 
--

CREATE TABLE program_property (
    program_id bigint NOT NULL,
    type public.property_type,
    value public.property_value
);


--
-- TOC entry 2859 (class 0 OID 0)
-- Dependencies: 327
-- Name: TABLE program_property; Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON TABLE program_property IS 'Some properties of insurance program';


--
-- TOC entry 328 (class 1259 OID 20117)
-- Name: service; Type: TABLE; Schema: insurance; Owner: -; Tablespace: 
--

CREATE TABLE service (
    id bigint NOT NULL,
    insurance_order_id bigint NOT NULL,
    program_id bigint NOT NULL,
    consumer_id bigint NOT NULL,
    state public.property_type,
    doc_number character varying(255),
    date_from date NOT NULL,
    date_to date NOT NULL,
    base_amount numeric DEFAULT 0 NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    show_vat boolean DEFAULT false NOT NULL,
    created timestamp without time zone DEFAULT now() NOT NULL,
    updated timestamp without time zone DEFAULT now() NOT NULL
);


--
-- TOC entry 2860 (class 0 OID 0)
-- Dependencies: 328
-- Name: TABLE service; Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON TABLE service IS 'The service data info';


--
-- TOC entry 329 (class 1259 OID 20128)
-- Name: service_id_seq; Type: SEQUENCE; Schema: insurance; Owner: -
--

CREATE SEQUENCE service_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2861 (class 0 OID 0)
-- Dependencies: 329
-- Name: service_id_seq; Type: SEQUENCE OWNED BY; Schema: insurance; Owner: -
--

ALTER SEQUENCE service_id_seq OWNED BY service.id;


--
-- TOC entry 330 (class 1259 OID 20130)
-- Name: tax_amount; Type: TABLE; Schema: insurance; Owner: -; Tablespace: 
--

CREATE TABLE tax_amount (
    id bigint NOT NULL,
    service_id bigint NOT NULL,
    tax_id bigint NOT NULL,
    amount numeric DEFAULT 0 NOT NULL,
    created timestamp without time zone NOT NULL
);


--
-- TOC entry 2862 (class 0 OID 0)
-- Dependencies: 330
-- Name: TABLE tax_amount; Type: COMMENT; Schema: insurance; Owner: -
--

COMMENT ON TABLE tax_amount IS 'The taxes of insurance service';


--
-- TOC entry 331 (class 1259 OID 20137)
-- Name: tax_amount_id_seq; Type: SEQUENCE; Schema: insurance; Owner: -
--

CREATE SEQUENCE tax_amount_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2863 (class 0 OID 0)
-- Dependencies: 331
-- Name: tax_amount_id_seq; Type: SEQUENCE OWNED BY; Schema: insurance; Owner: -
--

ALTER SEQUENCE tax_amount_id_seq OWNED BY tax_amount.id;


--
-- TOC entry 2679 (class 2604 OID 20637)
-- Name: id; Type: DEFAULT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY insurance_order ALTER COLUMN id SET DEFAULT nextval('insurance_order_id_seq'::regclass);


--
-- TOC entry 2680 (class 2604 OID 20638)
-- Name: id; Type: DEFAULT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY program ALTER COLUMN id SET DEFAULT nextval('program_id_seq'::regclass);


--
-- TOC entry 2687 (class 2604 OID 20639)
-- Name: id; Type: DEFAULT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY service ALTER COLUMN id SET DEFAULT nextval('service_id_seq'::regclass);


--
-- TOC entry 2688 (class 2604 OID 20640)
-- Name: id; Type: DEFAULT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY tax_amount ALTER COLUMN id SET DEFAULT nextval('tax_amount_id_seq'::regclass);


--
-- TOC entry 2695 (class 2606 OID 20779)
-- Name: insurance_order_pkey; Type: CONSTRAINT; Schema: insurance; Owner: -; Tablespace: 
--

ALTER TABLE ONLY insurance_order
    ADD CONSTRAINT insurance_order_pkey PRIMARY KEY (id);


--
-- TOC entry 2701 (class 2606 OID 20781)
-- Name: program_pkey; Type: CONSTRAINT; Schema: insurance; Owner: -; Tablespace: 
--

ALTER TABLE ONLY program
    ADD CONSTRAINT program_pkey PRIMARY KEY (id);


--
-- TOC entry 2709 (class 2606 OID 20783)
-- Name: service_doc_number_key; Type: CONSTRAINT; Schema: insurance; Owner: -; Tablespace: 
--

ALTER TABLE ONLY service
    ADD CONSTRAINT service_doc_number_key UNIQUE (doc_number);


--
-- TOC entry 2712 (class 2606 OID 20785)
-- Name: service_pkey; Type: CONSTRAINT; Schema: insurance; Owner: -; Tablespace: 
--

ALTER TABLE ONLY service
    ADD CONSTRAINT service_pkey PRIMARY KEY (id);


--
-- TOC entry 2719 (class 2606 OID 20787)
-- Name: tax_amount_pkey; Type: CONSTRAINT; Schema: insurance; Owner: -; Tablespace: 
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_pkey PRIMARY KEY (id);


--
-- TOC entry 2690 (class 1259 OID 21074)
-- Name: insurance_order_base_currency_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX insurance_order_base_currency_idx ON insurance_order USING btree (base_currency);


--
-- TOC entry 2691 (class 1259 OID 21075)
-- Name: insurance_order_created_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX insurance_order_created_idx ON insurance_order USING btree (created);


--
-- TOC entry 2692 (class 1259 OID 21076)
-- Name: insurance_order_currency_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX insurance_order_currency_idx ON insurance_order USING btree (currency);


--
-- TOC entry 2693 (class 1259 OID 21077)
-- Name: insurance_order_customer_order_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX insurance_order_customer_order_idx ON insurance_order USING btree (customer_order_id NULLS FIRST);


--
-- TOC entry 2696 (class 1259 OID 21078)
-- Name: insurance_order_sector_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX insurance_order_sector_idx ON insurance_order USING btree (sector_id);


--
-- TOC entry 2697 (class 1259 OID 21079)
-- Name: insurance_order_state_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX insurance_order_state_idx ON insurance_order USING btree (state);


--
-- TOC entry 2698 (class 1259 OID 21080)
-- Name: insurance_order_valid_until_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX insurance_order_valid_until_idx ON insurance_order USING btree (valid_until);


--
-- TOC entry 2715 (class 1259 OID 30789)
-- Name: insurance_tax_amount_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX insurance_tax_amount_idx ON tax_amount USING btree (service_id, tax_id);


--
-- TOC entry 2716 (class 1259 OID 21082)
-- Name: insurance_tax_amount_service_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX insurance_tax_amount_service_idx ON tax_amount USING btree (service_id);


--
-- TOC entry 2717 (class 1259 OID 21083)
-- Name: insurance_tax_amount_tax_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX insurance_tax_amount_tax_idx ON tax_amount USING btree (tax_id);


--
-- TOC entry 2699 (class 1259 OID 21084)
-- Name: program_period_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX program_period_idx ON program USING btree (start_date, end_date NULLS FIRST);


--
-- TOC entry 2702 (class 1259 OID 21085)
-- Name: program_property_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE UNIQUE INDEX program_property_idx ON program_property USING btree (program_id, type);


--
-- TOC entry 2703 (class 1259 OID 21086)
-- Name: program_property_program_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX program_property_program_idx ON program_property USING btree (program_id);


--
-- TOC entry 2704 (class 1259 OID 21087)
-- Name: program_property_type_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX program_property_type_idx ON program_property USING btree (type);


--
-- TOC entry 2705 (class 1259 OID 21088)
-- Name: program_property_value_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX program_property_value_idx ON program_property USING btree (value);


--
-- TOC entry 2706 (class 1259 OID 21089)
-- Name: service_consumer_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX service_consumer_idx ON service USING btree (consumer_id);


--
-- TOC entry 2707 (class 1259 OID 21090)
-- Name: service_created_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX service_created_idx ON service USING btree (created);


--
-- TOC entry 2710 (class 1259 OID 21091)
-- Name: service_insurance_order_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX service_insurance_order_idx ON service USING btree (insurance_order_id);


--
-- TOC entry 2713 (class 1259 OID 21092)
-- Name: service_program_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX service_program_idx ON service USING btree (program_id);


--
-- TOC entry 2714 (class 1259 OID 21093)
-- Name: service_state_idx; Type: INDEX; Schema: insurance; Owner: -; Tablespace: 
--

CREATE INDEX service_state_idx ON service USING btree (state);


--
-- TOC entry 2731 (class 2620 OID 21318)
-- Name: before_service_change; Type: TRIGGER; Schema: insurance; Owner: -
--

CREATE TRIGGER before_service_change BEFORE INSERT OR UPDATE OF insurance_order_id, base_amount ON service FOR EACH ROW EXECUTE PROCEDURE before_service_change();


--
-- TOC entry 2730 (class 2620 OID 21319)
-- Name: insurance_order_change; Type: TRIGGER; Schema: insurance; Owner: -
--

CREATE TRIGGER insurance_order_change AFTER INSERT OR DELETE OR UPDATE OF customer_order_id, amount ON insurance_order FOR EACH ROW EXECUTE PROCEDURE financial.order_change();


--
-- TOC entry 2733 (class 2620 OID 21320)
-- Name: tax_amount_change; Type: TRIGGER; Schema: insurance; Owner: -
--

CREATE TRIGGER tax_amount_change AFTER INSERT OR DELETE OR UPDATE OF service_id, tax_id, amount ON tax_amount FOR EACH ROW EXECUTE PROCEDURE tax_amount_change();


--
-- TOC entry 2732 (class 2620 OID 21321)
-- Name: ticket_change; Type: TRIGGER; Schema: insurance; Owner: -
--

CREATE TRIGGER ticket_change AFTER INSERT OR DELETE OR UPDATE OF insurance_order_id, base_amount, amount ON service FOR EACH ROW EXECUTE PROCEDURE service_change();


--
-- TOC entry 2720 (class 2606 OID 21787)
-- Name: insurance_order_base_currency_fkey; Type: FK CONSTRAINT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY insurance_order
    ADD CONSTRAINT insurance_order_base_currency_fkey FOREIGN KEY (base_currency) REFERENCES dictionary.currency(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2721 (class 2606 OID 21792)
-- Name: insurance_order_currency_fkey; Type: FK CONSTRAINT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY insurance_order
    ADD CONSTRAINT insurance_order_currency_fkey FOREIGN KEY (currency) REFERENCES dictionary.currency(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2722 (class 2606 OID 21797)
-- Name: insurance_order_customer_order_id_fkey; Type: FK CONSTRAINT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY insurance_order
    ADD CONSTRAINT insurance_order_customer_order_id_fkey FOREIGN KEY (customer_order_id) REFERENCES financial.customer_order(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2723 (class 2606 OID 21802)
-- Name: insurance_order_sector_id_fkey; Type: FK CONSTRAINT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY insurance_order
    ADD CONSTRAINT insurance_order_sector_id_fkey FOREIGN KEY (sector_id) REFERENCES org_chart.sector(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2724 (class 2606 OID 21807)
-- Name: program_property_program_id_fkey; Type: FK CONSTRAINT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY program_property
    ADD CONSTRAINT program_property_program_id_fkey FOREIGN KEY (program_id) REFERENCES program(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2725 (class 2606 OID 21812)
-- Name: service_consumer_id_fkey; Type: FK CONSTRAINT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY service
    ADD CONSTRAINT service_consumer_id_fkey FOREIGN KEY (consumer_id) REFERENCES dictionary.consumer(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2726 (class 2606 OID 21817)
-- Name: service_insurance_order_id_fkey; Type: FK CONSTRAINT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY service
    ADD CONSTRAINT service_insurance_order_id_fkey FOREIGN KEY (insurance_order_id) REFERENCES insurance_order(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2727 (class 2606 OID 21822)
-- Name: service_program_id_fkey; Type: FK CONSTRAINT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY service
    ADD CONSTRAINT service_program_id_fkey FOREIGN KEY (program_id) REFERENCES program(id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 2728 (class 2606 OID 21827)
-- Name: tax_amount_service_id_fkey; Type: FK CONSTRAINT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_service_id_fkey FOREIGN KEY (service_id) REFERENCES service(id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 2729 (class 2606 OID 21832)
-- Name: tax_amount_tax_id_fkey; Type: FK CONSTRAINT; Schema: insurance; Owner: -
--

ALTER TABLE ONLY tax_amount
    ADD CONSTRAINT tax_amount_tax_id_fkey FOREIGN KEY (tax_id) REFERENCES dictionary.tax(id) ON UPDATE CASCADE ON DELETE RESTRICT;


-- Completed on 2015-11-25 12:26:54

--
-- PostgreSQL database dump complete
--

