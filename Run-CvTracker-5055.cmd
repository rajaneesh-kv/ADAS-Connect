@echo off
title ADAS Connect (http://localhost:5055)
cd /d "%~dp0"
echo Starting ADAS Connect...
echo Open: http://localhost:5055/Account/Login
echo Press Ctrl+C to stop.
dotnet run --project "%~dp0CvTracker.Web\CvTracker.Web.csproj" --urls "http://localhost:5055"
pause
