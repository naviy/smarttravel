@if "%~1" == "" goto exit

@"c:\Program Files\PostgreSQL\9.2\bin\pg_dump.exe" -h localhost -U postgres -n %1 -s -f %1.sql travel

:exit