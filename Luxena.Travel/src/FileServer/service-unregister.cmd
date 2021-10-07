@echo off

net stop luxena-file-server

"%~dp0bin\nssm.exe" remove luxena-file-server confirm