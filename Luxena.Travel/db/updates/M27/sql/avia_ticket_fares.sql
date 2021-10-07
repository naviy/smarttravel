
BEGIN WORK;

--drop table if exists lt_fare_segment cascade; 


alter table lt_avia_ticket add faretotal_amount numeric(19,5);
alter table lt_avia_ticket add faretotal_currency character varying(32);

alter table lt_avia_ticket add constraint aviaticket_faretotal_currency_fk 
    foreign key (faretotal_currency) references lt_currency (id);

create index aviaticket_faretotal_currency_idx on lt_avia_ticket using btree (faretotal_currency);


alter table lt_flight_segment add surcharges numeric(19,5);
alter table lt_flight_segment add isinclusive boolean not null default(false);
alter table lt_flight_segment add fare numeric(19,5);
alter table lt_flight_segment add stopoverortransfercharge numeric(19,5);
alter table lt_flight_segment add issidetrip boolean not null default(false);

alter table lt_flight_segment add distance double precision not null default(0);
alter table lt_flight_segment add amount_amount numeric(19,5);
alter table lt_flight_segment add amount_currency character varying(32);

create index flightsegment_amount_currency_idx on lt_flight_segment using btree (amount_currency);

alter table lt_flight_segment add constraint flightsegment_amount_currency_fk 
    foreign key (amount_currency) references lt_currency (id);


alter table lt_flight_segment
    alter column isinclusive drop default,
    alter column issidetrip drop default,
    alter column distance drop default;


insert into lt_currency (id, version, createdby, createdon, code, numericcode, name)
values ('2692cc37551942d385ff67544867eabe', 1, 'SYSTEM', now(), 'NUC', 0, 'Neutral unit of construction');

--ROLLBACK;
COMMIT;