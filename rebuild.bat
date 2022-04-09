@echo off
net stop WinBot
dotnet build -c Release
net start WinBot
REM echo Press RETURN to exit...
REM set /p input=
exit