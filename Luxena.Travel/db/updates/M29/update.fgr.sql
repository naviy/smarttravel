alter table lt_user
    alter column password type citext2;

alter table lt_avia_document cluster on aviadocument_issuedate_displaystring_owner_idx;