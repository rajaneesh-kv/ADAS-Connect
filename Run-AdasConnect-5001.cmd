@echo off
title ADAS Connect — clean run https://localhost:5001
cd /d "%~dp0"

echo Stopping old CvTracker / IIS Express workers...
taskkill /F /IM CvTracker.Web.exe >nul 2>&1
taskkill /F /IM iisexpress.exe >nul 2>&1

echo Cleaning build outputs...
dotnet clean "%~dp0CvTracker.sln" -v q
if exist "%~dp0CvTracker.Web\bin" rmdir /s /q "%~dp0CvTracker.Web\bin"
if exist "%~dp0CvTracker.Web\obj" rmdir /s /q "%~dp0CvTracker.Web\obj"

echo Restore + build...
dotnet restore "%~dp0CvTracker.sln"
dotnet build "%~dp0CvTracker.sln" -v m
if errorlevel 1 (
  echo BUILD FAILED — fix errors above, then run this script again.
  pause
  exit /b 1
)

echo.
echo Starting ADAS Connect. Look for log line: ADAS Connect host: ... ContentRoot=...
echo Then open https://localhost:5001 and press Ctrl+Shift+R hard refresh.
echo Verify response header X-Adas-Ui-Shell = v2-2026  (F12 - Network - document)
echo.
dotnet run --project "%~dp0CvTracker.Web\CvTracker.Web.csproj" --urls "https://localhost:5001;http://localhost:5000"
pause
