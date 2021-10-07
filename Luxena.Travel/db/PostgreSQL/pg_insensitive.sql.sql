drop domain if exists public.citext2 cascade;

create domain public.citext2 as citext;


create or replace function public.citext2_like(citext2, citext2) returns boolean
as $$
   select lower($1) like lower($2);
$$
language sql;

drop operator if exists ~~ (citext2, citext2);

create operator ~~(procedure = citext2_like, leftarg = citext2, rightarg = citext2, restrict = likesel, join = likejoinsel);


create or replace function public.citext2_equals(citext2, text) returns boolean
as $$
   select $1 = $2::citext2;
$$
language sql;

drop operator if exists = (citext2, text);

create operator =(procedure = citext2_equals, leftarg = citext2, rightarg = text);


create or replace function public.citext2_equals(text, citext2) returns boolean
as $$
   select $1::citext2 = $2;
$$
language sql;

drop operator if exists = (text, citext2);

create operator =(procedure = citext2_equals, leftarg = text, rightarg = citext2);




create or replace function public.citext2_equals(citext2, bigint) returns boolean
as $$
   select $1 = $2::citext2;
$$
language sql;

drop operator if exists = (citext2, bigint);

create operator =(procedure = citext2_equals, leftarg = citext2, rightarg = bigint);


create or replace function public.citext2_equals(bigint, citext2) returns boolean
as $$
   select $1::citext2 = $2;
$$
language sql;

drop operator if exists = (bigint, citext2);

create operator =(procedure = citext2_equals, leftarg = bigint, rightarg = citext2);


create or replace function public.citext2_equals(citext2, int) returns boolean
as $$
   select $1 = $2::citext2;
$$
language sql;

drop operator if exists = (citext2, int);

create operator =(procedure = citext2_equals, leftarg = citext2, rightarg = int);


create or replace function public.citext2_equals(int, citext2) returns boolean
as $$
   select $1::citext2 = $2;
$$
language sql;

drop operator if exists = (int, citext2);

create operator =(procedure = citext2_equals, leftarg = int, rightarg = citext2);

