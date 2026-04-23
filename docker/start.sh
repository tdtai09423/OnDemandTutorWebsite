#!/bin/bash
set -e

cd "$(dirname "$0")"

# Generate certificates if they don't exist
if [ ! -f "certs/aspnetapp.pfx" ]; then
    echo "Generating SSL certificates..."
    bash generate-certs.sh
fi

# Clean up previous containers and volumes
echo "Stopping and removing previous containers..."
docker compose down -v

# Build and start
echo "Building and starting services..."
docker compose up --build
