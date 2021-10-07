@echo off

set schema=%1


set Today=%Date: =0%
set yy=%Today:~-4%
set mm=%Today:~-10,2%
set dd=%Today:~-7,2%

set Now=%Time: =0%
set hh=%Now:~0,2%
set ii=%Now:~3,2%


set fn=%schema%_%yy%-%mm%-%dd%_%hh%-%ii%

@echo Beginning backup of %schema%...

"C:\Program Files\PostgreSQL\9.5\bin\pg_dump" -h localhost -O -U postgres -n %schema% -w -f %fn%.sql travel

"C:\Program Files\7-Zip\7z.exe" a -tzip %fn%.zip %fn%.sql

del %fn%.sql

@echo New File: %fn%.zip