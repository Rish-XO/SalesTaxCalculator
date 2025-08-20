#!/bin/bash

echo "Sales Tax Calculator - Build and Test Script"
echo

echo "Checking .NET SDK..."
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET SDK not found. Please install .NET 8 SDK."
    echo "Download from: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

dotnet --version

echo
echo "Restoring NuGet packages..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "Restore failed!"
    exit 1
fi

echo
echo "Building Sales Tax Calculator..."
dotnet build --no-restore
if [ $? -ne 0 ]; then
    echo "Build failed!"
    echo
    echo "Troubleshooting:"
    echo "1. Check that all .csproj files exist"
    echo "2. Verify project references are correct" 
    echo "3. Run 'dotnet clean' then try again"
    exit 1
fi

echo
echo "Running tests..."
dotnet test --no-build --verbosity normal
if [ $? -ne 0 ]; then
    echo "Tests failed!"
    exit 1
fi

echo
echo "=============================================="
echo "All tests passed! Build successful!"
echo "=============================================="
echo
echo "Running application..."
dotnet run --project src/SalesTaxCalculator.App --no-build