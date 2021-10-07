@if "%~1" == "" goto exit

@echo off

@if "%~2" == "" (
    set basesql=travel
) else (
    set basesql=%2
)

@"c:\Program Files\PostgreSQL\9.2\bin\pg_dump.exe" -h localhost -U postgres -n %1 -s -f %1.tmp.sql travel
tools\gsar -s"%1" -r"travel" -f %1.tmp.sql sql\%1.sql
del "%1.tmp.sql"

tools/apgdiff "sql\%1.sql" "sql\%basesql%.sql" > "%1 .. %basesql%.sql"

:exit