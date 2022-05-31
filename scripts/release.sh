#!/bin/bash
set -e

prefix="v"
VERSION=${CI_COMMIT_REF_NAME#"$prefix"}

dotnet restore
dotnet pack --output out -p:SymbolPackageFormat=snupkg --no-restore --include-symbols --include-source /p:Version=$VERSION
dotnet nuget add source "${CI_API_V4_URL}/projects/${CI_PROJECT_ID}/packages/nuget/index.json" --name gitlab --username gitlab-ci-token --password $CI_JOB_TOKEN --store-password-in-clear-text
dotnet nuget push "./out/*.nupkg" --source gitlab