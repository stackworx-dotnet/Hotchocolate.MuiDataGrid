#!/bin/bash
set -e

if [ -z "$VERSION" ]; then
  echo "VERSION is required (example: VERSION=2.0.0)"
  exit 1
fi

if [ -z "$NUGET_API_KEY" ]; then
  echo "NUGET_API_KEY is required"
  exit 1
fi

dotnet restore
dotnet build --configuration Release --no-restore
dotnet pack ./src/Hotchocolate.MuiDataGrid/Hotchocolate.MuiDataGrid.csproj --configuration Release --output out --no-build --include-symbols --include-source -p:SymbolPackageFormat=snupkg -p:PackageVersion="$VERSION"
dotnet pack ./src/Hotchocolate.MudDataGrid/Hotchocolate.MudDataGrid.csproj --configuration Release --output out --no-build --include-symbols --include-source -p:SymbolPackageFormat=snupkg -p:PackageVersion="$VERSION"
dotnet nuget push "./out/*.nupkg" --api-key "$NUGET_API_KEY" --source https://api.nuget.org/v3/index.json --skip-duplicate
