#!/bin/bash

pushd Infrastructure
dotnet restore

# Try to connect to the database every 1 second over 120 seconds
dockerize -wait tcp://postgres-service:5432 -timeout 120s

dotnet ef database update
popd

dotnet watch --project API/API.csproj run -- --urls http://0.0.0.0:5000