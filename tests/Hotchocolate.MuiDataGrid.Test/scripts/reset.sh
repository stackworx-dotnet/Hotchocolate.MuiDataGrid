#!/bin/bash
set -e
set -x

rm -rf ./Migrations || 0

dotnet ef database drop --force
dotnet ef migrations add InitialCreate
dotnet ef database update