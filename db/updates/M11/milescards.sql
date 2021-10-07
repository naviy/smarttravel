alter table lt_person
	add column milescardsstring varchar(200) default null after birthday;
	
update lt_person as p
set p.milescardsstring = (
	select GROUP_CONCAT(c.number_ SEPARATOR ', ')
	from lt_miles_card as c 
	where c.owner = p.id
	group by c.owner
);