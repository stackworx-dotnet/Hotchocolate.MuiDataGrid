#!/bin/bash
set -e

dotnet test ./tests/Hotchocolate.MuiDataGrid.Test --configuration=Release --logger "trx;LogFileName=TestResults.Integration.trx"