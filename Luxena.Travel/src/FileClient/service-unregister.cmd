@echo off

net stop luxena-file-client

"%~dp0bin\nssm.exe" remove luxena-file-client confirm