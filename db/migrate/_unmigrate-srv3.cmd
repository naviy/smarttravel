set schema=%1

set host=srv03.luxena.com
set timeout=1200


set connectionString=Server=%host%;Database=travel;User ID=%schema%;Password=%schema%;CommandTimeout=%timeout%;ConnectionLifetime=%timeout%;
set assembly=D:\data\git\Luxena.Travel\tools\FluentMigrator\Luxena.Travel.DbMigrator.dll


::@echo %schema%
::@echo %connectionString%
::@echo %assembly%

D:\data\git\Luxena.Travel\tools\FluentMigrator\Migrate.exe /conn "%connectionString%" /provider postgres /assembly %assembly% /task rollback /verbose=false