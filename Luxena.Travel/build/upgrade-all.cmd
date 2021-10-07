@echo off

setlocal EnableDelayedExpansion

for /R "inbox/_last" %%I in ("*_web-app.zip") do (
	set filename=%%~nxI
	set schema=!filename:~24,-12!
	echo "!filename! => !schema!"
	"C:\Program Files\7-Zip\7z.exe" x -y -o"apps\!schema!" "inbox\_last\!filename!" web
	if not exist inbox\!schema! mkdir inbox\!schema!
	move "inbox\_last\!filename!" "inbox\!schema!\"
)
	



