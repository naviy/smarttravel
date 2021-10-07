alter table lt_avia_ticket
	drop constraint aviaticket_faretotal_currency_fkey;

alter table lt_flight_segment
	drop constraint flightsegment_amount_currency_fkey;

alter table lt_system_configuration
	alter column reservationsinofficemetrics set not null;

alter table lt_user
	alter column password type citext2;

alter table lt_avia_ticket
	add constraint aviaticket_faretotal_currency_fk foreign key (faretotal_currency) references lt_currency(id);

alter table lt_flight_segment
	add constraint flightsegment_amount_currency_fk foreign key (amount_currency) references lt_currency(id);
