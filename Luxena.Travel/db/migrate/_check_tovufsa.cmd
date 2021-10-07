@echo off

set psql="c:\Program Files\PostgreSQL\9.6\bin\psql.exe"
set PGUSER=utb_ufsa
set PGPASSWORD=utb_ufsa
set PGCLIENTENCODING=WIN1251

%psql% -h srv03.luxena.com -U utb_ufsa -d travel -c "select pt.name from (select distinct p1.seller from ufsa.lt_product p1 inner join tovufsa.lt_product p2 on p2.seller = p1.seller) as p inner join tovufsa.lt_party pt on p.seller = pt.id"

pause