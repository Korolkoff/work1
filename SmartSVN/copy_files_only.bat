rem @echo off
chcp 1251 > nul
rem if exist %temp%\~copy~files~only~.tmp exit
for /f "delims=" %%a in (%1) do dir /a:-d /b /s "%%a" | find /V ".svn" > %temp%\~copy~files~only~.tmp
for /f "delims=" %%b in (%temp%\~copy~files~only~.tmp) do copy /b /y "%%b" "%2"
del %temp%\~copy~files~only~.tmp