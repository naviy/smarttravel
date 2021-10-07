@echo off

set schema=%1
set dump=%2

set psql="c:\Program Files\PostgreSQL\9.2\bin\psql.exe"
set PGUSER=%schema%
set PGPASSWORD=%schema%
set PGCLIENTENCODING=UTF8

@echo Beginning restore %schema% schema...
%psql% -h srv03.luxena.com -U %PGUSER% -f %dump% travel
@echo Restore file %dump% into %schema% schema finished. 

pause