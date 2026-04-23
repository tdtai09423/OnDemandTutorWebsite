@echo off
cd /d "%~dp0"

set CERT_DIR=%~dp0certs
if not exist "%CERT_DIR%" mkdir "%CERT_DIR%"

echo Generating self-signed certificates...

REM Generate key and certificate
openssl req -x509 -nodes -days 365 -newkey rsa:2048 ^
  -keyout "%CERT_DIR%\aspnetapp.key" ^
  -out "%CERT_DIR%\aspnetapp.crt" ^
  -subj "/CN=localhost" ^
  -addext "subjectAltName=DNS:localhost,DNS:backend,DNS:frontend"

REM Generate PFX for ASP.NET Core
openssl pkcs12 -export -out "%CERT_DIR%\aspnetapp.pfx" ^
  -inkey "%CERT_DIR%\aspnetapp.key" ^
  -in "%CERT_DIR%\aspnetapp.crt" ^
  -password pass:password

echo Certificates generated in %CERT_DIR%
