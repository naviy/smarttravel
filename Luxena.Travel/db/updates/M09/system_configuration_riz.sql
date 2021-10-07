alter table lt_system_configuration
	add `amadeusrizusingmode` int(11) default NULL after `birthdaytaskresposible`;

update lt_system_configuration
	set amadeusrizusingmode = 1;

alter table lt_system_configuration
	change `amadeusrizusingmode` `amadeusrizusingmode` int(11) not NULL;