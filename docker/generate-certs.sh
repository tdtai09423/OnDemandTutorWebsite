#!/bin/bash
# Generate self-signed certificates for Docker HTTPS
set -e

CERT_DIR="$(cd "$(dirname "$0")" && pwd)/certs"
mkdir -p "$CERT_DIR"

echo "Generating self-signed certificates..."

# Generate key and certificate for nginx (frontend)
openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
  -keyout "$CERT_DIR/aspnetapp.key" \
  -out "$CERT_DIR/aspnetapp.crt" \
  -subj "/CN=localhost" \
  -addext "subjectAltName=DNS:localhost,DNS:backend,DNS:frontend"

# Generate PFX for ASP.NET Core (backend)
openssl pkcs12 -export -out "$CERT_DIR/aspnetapp.pfx" \
  -inkey "$CERT_DIR/aspnetapp.key" \
  -in "$CERT_DIR/aspnetapp.crt" \
  -password pass:password

echo "Certificates generated in $CERT_DIR"
