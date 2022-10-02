#!/bin/bash

echo "Packing...."
echo $GITHUB_TAG

dotnet pack --no-restore --no-build \
    -o /PackOutputs \
    -c Release \
    -p:Version=$GITHUB_TAG \
    -p:IncludeSymbols=true \
    -p:SymbolPackageFormat=snupkg

rm -f /PackOutputs/*.nupkg

dotnet pack --no-restore --no-build \
    -o /PackOutputs \
    -c Release \
    -p:Version=$GITHUB_TAG \
    -p:PackageReadmeFile=README.md
