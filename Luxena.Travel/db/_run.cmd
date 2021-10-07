@echo off

set schema=%1
set file=%2
set host=%3

if "%schema%" == "" goto usage
if "%file%" == "" goto usage

if "%host%" == "" set host=localhost

set psql="c:\Program Files\PostgreSQL\13\bin\psql.exe"
set PGUSER=%schema%
set PGPASSWORD=%schema%
set PGCLIENTENCODING=UTF8

@echo Executing %file%...
%psql% -h %host% -U %PGUSER% -f %file% travel
@echo Done.

goto exit

:usage
echo Usage: run ^<schema^> ^<script file^>


:exit
pause