# Sales Tax Calculator

A production-ready sales tax calculation system built with C# and .NET 8, demonstrating clean architecture, SOLID principles, and comprehensive testing.

## Problem Statement

This application calculates sales tax for shopping baskets according to the following rules:
- **Basic sales tax**: 10% on all goods except books, food, and medical products (exempt)
- **Import duty**: Additional 5% on all imported goods (no exemptions)
- **Tax rounding**: Rounded up to the nearest 0.05

## Architecture

### Design Patterns Used
- **Strategy Pattern**: For flexible tax calculation rules
- **Builder Pattern**: For constructing receipts
- **Repository Pattern**: For product categorization
- **Domain-Driven Design**: Clear separation of concerns

### Project Structure
```
SalesTaxCalculator/
├── src/
│   ├── SalesTaxCalculator.App/        # Console application
│   └── SalesTaxCalculator.Core/       # Business logic library
│       ├── Domain/                    # Domain models and value objects
│       ├── Services/                  # Business services
│       ├── Strategies/                # Tax calculation strategies
│       ├── Parsers/                   # Input parsing
│       └── Formatters/                # Output formatting
└── tests/
    └── SalesTaxCalculator.Tests/      # Unit and integration tests
```

## Key Components

### Domain Models
- **Product**: Core entity representing a product with price, category, and import status
- **LineItem**: Represents a product with quantity in a shopping basket
- **Receipt**: Aggregate root containing line items and totals
- **Money**: Value object ensuring proper decimal handling and rounding

### Tax Strategies
- **BasicTaxStrategy**: Applies 10% tax to non-exempt products
- **ImportTaxStrategy**: Applies 5% import duty to imported products
- **CompositeTaxStrategy**: Combines multiple tax strategies

### Services
- **TaxCalculator**: Orchestrates tax calculation using strategies
- **ReceiptService**: Generates receipts from shopping baskets
- **ShoppingBasketParser**: Parses input strings to domain models

## Getting Started

### Prerequisites
- .NET 8 SDK or later
- Git
- Visual Studio 2022, VS Code, or any text editor (optional)

### Clone and Setup
```bash
# Clone the repository
git clone <repository-url>
cd SalesTaxCalculator

# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build
```

### Alternative Quick Setup
**Windows:**
```batch
build.bat
```

**Linux/Mac:**
```bash
chmod +x build.sh
./build.sh
```

### Running Tests
```bash
dotnet test
```

### Running the Application
```bash
dotnet run --project src/SalesTaxCalculator.App
```

### Troubleshooting

**Build Fails?**
1. Ensure .NET 8 SDK is installed: `dotnet --version`
2. Run `dotnet restore` first
3. Check all .csproj files are present
4. Try `dotnet clean` then `dotnet build`

**Missing Project Files?**
Ensure these files exist:
- `SalesTaxCalculator.sln`
- `src/SalesTaxCalculator.App/SalesTaxCalculator.App.csproj`
- `src/SalesTaxCalculator.Core/SalesTaxCalculator.Core.csproj` 
- `tests/SalesTaxCalculator.Tests/SalesTaxCalculator.Tests.csproj`

## Test Cases

The application includes all three required test cases:

### Input 1
```
1 book at 12.49
1 music CD at 14.99
1 chocolate bar at 0.85
```
**Output**: Sales Taxes: 1.50, Total: 29.83

### Input 2
```
1 imported box of chocolates at 10.00
1 imported bottle of perfume at 47.50
```
**Output**: Sales Taxes: 7.65, Total: 65.15

### Input 3
```
1 imported bottle of perfume at 27.99
1 bottle of perfume at 18.99
1 packet of headache pills at 9.75
1 box of imported chocolates at 11.25
```
**Output**: Sales Taxes: 6.70, Total: 74.68

## Code Quality

### SOLID Principles
- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Tax strategies can be extended without modifying existing code
- **Liskov Substitution**: All strategies implement ITaxStrategy interface
- **Interface Segregation**: Small, focused interfaces
- **Dependency Inversion**: Depends on abstractions, not concretions

### Testing
- Unit tests for all components
- Integration tests for end-to-end scenarios
- Test coverage for all business logic
- Edge cases for rounding and tax exemptions

### Extensibility
- Easy to add new tax rules by implementing ITaxStrategy
- New product categories can be added to the enum
- Parser can be extended for different input formats
- Formatter can be swapped for different output formats

## Future Enhancements
- Configuration file support for tax rates
- Database persistence
- Web API interface
- Bulk processing capabilities
- Internationalization support

## Author
Developed as a technical assessment for Makkajai