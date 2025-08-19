using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;
using SalesTaxCalculator.Core.Strategies;
using Xunit;

namespace SalesTaxCalculator.Tests.Strategies;

public class TaxStrategyTests
{
    [Fact]
    public void BasicTaxStrategy_AppliesTaxToNonExemptProducts()
    {
        var strategy = new BasicTaxStrategy(0.10m);
        var product = new Product("music CD", new Money(14.99m), ProductCategory.General, false);
        
        var tax = strategy.CalculateTax(product);
        
        Assert.Equal(1.50m, tax.Amount);
    }

    [Fact]
    public void BasicTaxStrategy_NoTaxForExemptProducts()
    {
        var strategy = new BasicTaxStrategy(0.10m);
        var product = new Product("book", new Money(12.49m), ProductCategory.Book, false);
        
        var tax = strategy.CalculateTax(product);
        
        Assert.Equal(0m, tax.Amount);
    }

    [Fact]
    public void ImportTaxStrategy_AppliesTaxToImportedProducts()
    {
        var strategy = new ImportTaxStrategy(0.05m);
        var product = new Product("imported chocolates", new Money(10.00m), ProductCategory.Food, true);
        
        var tax = strategy.CalculateTax(product);
        
        Assert.Equal(0.50m, tax.Amount);
    }

    [Fact]
    public void ImportTaxStrategy_NoTaxForDomesticProducts()
    {
        var strategy = new ImportTaxStrategy(0.05m);
        var product = new Product("chocolates", new Money(10.00m), ProductCategory.Food, false);
        
        var tax = strategy.CalculateTax(product);
        
        Assert.Equal(0m, tax.Amount);
    }

    [Fact]
    public void CompositeTaxStrategy_CombinesMultipleStrategies()
    {
        var basicStrategy = new BasicTaxStrategy(0.10m);
        var importStrategy = new ImportTaxStrategy(0.05m);
        var compositeStrategy = new CompositeTaxStrategy(basicStrategy, importStrategy);
        
        var product = new Product("imported perfume", new Money(47.50m), ProductCategory.General, true);
        
        var tax = compositeStrategy.CalculateTax(product);
        
        Assert.Equal(7.15m, tax.Amount);
    }

    [Theory]
    [InlineData(14.99, 1.50)]
    [InlineData(27.99, 2.80)]
    [InlineData(18.99, 1.90)]
    public void BasicTaxStrategy_RoundsToNearestFiveCents(decimal price, decimal expectedTax)
    {
        var strategy = new BasicTaxStrategy(0.10m);
        var product = new Product("item", new Money(price), ProductCategory.General, false);
        
        var tax = strategy.CalculateTax(product);
        
        Assert.Equal(expectedTax, tax.Amount);
    }
}