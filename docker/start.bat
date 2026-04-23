@echo off
cd /d "%~dp0"

REM Generate certificates if they don't exist
if not exist "certs\aspnetapp.pfx" (
    echo Generating SSL certificates...
    call generate-certs.bat
)

REM Clean up previous containers and volumes
echo Stopping and removing previous containers...
docker compose down -v

REM Build and start
echo Building and starting services...
docker compose up --build
