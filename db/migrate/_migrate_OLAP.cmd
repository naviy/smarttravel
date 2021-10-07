@echo off

set schema=%1

set host=localhost
set timeout=1200


set connectionString=Server=%host%;Database=travel;User ID=%schema%;Password=%schema%;CommandTimeout=%timeout%;ConnectionLifetime=%timeout%;
set assembly=..\..\tools\FluentMigrator\Luxena.Travel.DbMigrator.dll


::@echo %schema%
::@echo %connectionString%
::@echo %assembly%

..\..\tools\FluentMigrator\Migrate.exe /conn "%connectionString%" /provider postgres /assembly %assembly% /verbose=false /profile OLAP /namespace Luxena.FluentMigrator.Olap