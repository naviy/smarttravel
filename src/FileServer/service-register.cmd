@echo off

"%~dp0bin\nssm.exe" install luxena-file-server "%~dp0bin\node.exe" "%~dp0lib\server.js"

net start luxena-file-server