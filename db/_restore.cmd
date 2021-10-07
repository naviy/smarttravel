@echo off

set file=%1
set host=%2

if "%file%" == "" goto usage


for /f "delims=/_" %%a in ("%file%") do (
	set schema=%%a
	goto schema
)
:schema
echo Schema: "%schema%"
if "%schema%" == "" goto usage

if "%host%" == "" set host=localhost


::set PGCLIENTENCODING=WIN1251
set PGCLIENTENCODING=Unicode

set psql="c:\Program Files\PostgreSQL\9.6\bin\psql.exe"
set PGUSER=%schema%


set PGPASSWORD=1
%psql% -h %host% -U postgres -c "DROP SCHEMA IF EXISTS %schema% CASCADE; CREATE SCHEMA %schema% AUTHORIZATION %schema%" travel


@echo Executing %file%...
set PGPASSWORD=%schema%
%psql% -h %host% -U %PGUSER% -f %file% travel
@echo Done.


goto exit

:usage
echo Usage: run ^<script file^> [^<host^>]


:exit
pause