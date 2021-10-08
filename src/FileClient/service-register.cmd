@echo off

"%~dp0bin\nssm.exe" install luxena-file-client "%~dp0bin\node.exe" "%~dp0lib\client.js"

net start luxena-file-client