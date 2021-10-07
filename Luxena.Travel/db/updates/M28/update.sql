--begin work;

alter table lt_order add column billtoname citext2;

--rollback;

