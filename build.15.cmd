@echo off

set NANT="tools\nant\NAnt.exe"

%NANT% %* -buildfile:Luxena.Travel.15.build

pause