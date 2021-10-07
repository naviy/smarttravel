@echo off
for /f "tokens=1-4 delims=. " %%a in ('date/t') do (
set dd=%%a
set mm=%%b
set yy=%%c
)
for /F "tokens=5-8 delims=:. " %%i in ('echo.^| time ^| find "current" ') do (
set hh=%%i
set ii=%%j
)

set backupdir=%~dp0.
set postgresdir="C:\Program Files\PostgreSQL\9.5\bin"
set dbname=%1

set fn=%yy%-%mm%-%dd%_%hh%-%ii%_%dbname%

@echo Beginning backup of %dbname%...
%postgresdir%\pg_dump -h localhost -O -U postgres -n %dbname% -w -f %backupdir%\%fn%.sql travel
@echo Done!
@echo New File: %fn%.sql

pause