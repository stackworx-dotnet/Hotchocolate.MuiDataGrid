#!/bin/bash
set -e

dotnet test ./tests/Hotchocolate.MuiDataGrid.Test --configuration=Release --no-build --logger "trx;LogFileName=TestResults.Integration.trx"