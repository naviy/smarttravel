Server

1. Install Microsoft Analysis Services
2. Install Npgsql .NET provider
3. Run copy-pg.cmd (additional directories could be required to be added to the command file)
4. Apply changes to machine.config from machine_config_update.config
5. Добавить в GAC Npgsql и Mono.Security

Client

1. Install
     "Поставщик OLE DB служб Microsoft® Analysis Services для Microsoft® SQL Server® 2008 R2"
     http://www.microsoft.com/downloads/ru-ru/details.aspx?familyid=ceb4346f-657f-4d28-83f5-aae0c5c83d52&displaylang=ru
   for the appropriative platform (x86 or x64)
