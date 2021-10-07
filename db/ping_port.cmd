@echo off

::Must be without quotes. If you intend to use command line arguments use %~1
set "host=smartikum.com"
set /a port=5432

for /f %%a in ('powershell "$t = New-Object Net.Sockets.TcpClient;try{$t.Connect("""%host%""", %port%)}catch{};$t.Connected"') do set "open=%%a"
:: or simply
:: powershell "$t = New-Object Net.Sockets.TcpClient;try{$t.Connect("""%host%""", %port%)}catch{};$t.Connected"
echo Host: %host%
echo Port: %port%
echo Open: %open%
