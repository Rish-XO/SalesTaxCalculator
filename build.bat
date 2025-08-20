@echo off
echo Sales Tax Calculator - Build and Test Script
echo.

echo Checking .NET SDK...
dotnet --version
if %errorlevel% neq 0 (
    echo ERROR: .NET SDK not found. Please install .NET 8 SDK.
    echo Download from: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo.
echo Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo Restore failed!
    pause
    exit /b %errorlevel%
)

echo.
echo Building Sales Tax Calculator...
dotnet build --no-restore
if %errorlevel% neq 0 (
    echo Build failed!
    echo.
    echo Troubleshooting:
    echo 1. Check that all .csproj files exist
    echo 2. Verify project references are correct
    echo 3. Run 'dotnet clean' then try again
    pause
    exit /b %errorlevel%
)

echo.
echo Running tests...
dotnet test --no-build --verbosity normal
if %errorlevel% neq 0 (
    echo Tests failed!
    pause
    exit /b %errorlevel%
)

echo.
echo ==============================================
echo All tests passed! Build successful!
echo ==============================================
echo.
echo Running application...
dotnet run --project src/SalesTaxCalculator.App --no-build