
set client_encoding to 'win1251';

alter table lt_flight_segment
	alter column isinclusive drop default,
	alter column issidetrip drop default,
	alter column distance drop default;
