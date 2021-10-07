@echo off


set NANT="tools\nant\NAnt.exe"

%NANT% %* -buildfile:Luxena.Travel.build

pause