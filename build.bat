@echo off
echo Building Sales Tax Calculator...
dotnet build
if %errorlevel% neq 0 (
    echo Build failed!
    exit /b %errorlevel%
)
echo.
echo Running tests...
dotnet test
if %errorlevel% neq 0 (
    echo Tests failed!
    exit /b %errorlevel%
)
echo.
echo All tests passed!
echo.
echo Running application...
dotnet run --project src/SalesTaxCalculator.App