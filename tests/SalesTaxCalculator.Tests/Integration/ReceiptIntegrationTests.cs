using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;
using SalesTaxCalculator.Core.Formatters;
using SalesTaxCalculator.Core.Parsers;
using SalesTaxCalculator.Core.Services;
using SalesTaxCalculator.Core.Strategies;
using Xunit;

namespace SalesTaxCalculator.Tests.Integration;

public class ReceiptIntegrationTests
{
    private readonly IReceiptService _receiptService;
    private readonly IInputParser _parser;
    private readonly IReceiptFormatter _formatter;

    public ReceiptIntegrationTests()
    {
        var basicTaxStrategy = new BasicTaxStrategy(0.10m);
        var importTaxStrategy = new ImportTaxStrategy(0.05m);
        var compositeTaxStrategy = new CompositeTaxStrategy(basicTaxStrategy, importTaxStrategy);
        var taxCalculator = new TaxCalculator(compositeTaxStrategy);
        
        _receiptService = new ReceiptService(taxCalculator);
        _parser = new ShoppingBasketParser();
        _formatter = new ConsoleReceiptFormatter();
    }

    [Fact]
    public void TestCase1_ProducesCorrectOutput()
    {
        var input = new[]
        {
            "1 book at 12.49",
            "1 music CD at 14.99",
            "1 chocolate bar at 0.85"
        };

        var items = _parser.ParseShoppingBasket(input);
        var receipt = _receiptService.GenerateReceipt(items);

        Assert.Equal(1.50m, receipt.TotalTax.Amount);
        Assert.Equal(29.83m, receipt.Total.Amount);

        Assert.Collection(receipt.LineItems,
            item => {
                Assert.Equal("book", item.Product.Name);
                Assert.Equal(12.49m, item.PriceWithTax.Amount);
            },
            item => {
                Assert.Equal("music CD", item.Product.Name);
                Assert.Equal(16.49m, item.PriceWithTax.Amount);
            },
            item => {
                Assert.Equal("chocolate bar", item.Product.Name);
                Assert.Equal(0.85m, item.PriceWithTax.Amount);
            }
        );
    }

    [Fact]
    public void TestCase2_ProducesCorrectOutput()
    {
        var input = new[]
        {
            "1 imported box of chocolates at 10.00",
            "1 imported bottle of perfume at 47.50"
        };

        var items = _parser.ParseShoppingBasket(input);
        var receipt = _receiptService.GenerateReceipt(items);

        Assert.Equal(7.65m, receipt.TotalTax.Amount);
        Assert.Equal(65.15m, receipt.Total.Amount);

        Assert.Collection(receipt.LineItems,
            item => {
                Assert.Equal("box of chocolates", item.Product.Name);
                Assert.Equal(10.50m, item.PriceWithTax.Amount);
            },
            item => {
                Assert.Equal("bottle of perfume", item.Product.Name);
                Assert.Equal(54.65m, item.PriceWithTax.Amount);
            }
        );
    }

    [Fact]
    public void TestCase3_ProducesCorrectOutput()
    {
        var input = new[]
        {
            "1 imported bottle of perfume at 27.99",
            "1 bottle of perfume at 18.99",
            "1 packet of headache pills at 9.75",
            "1 box of imported chocolates at 11.25"
        };

        var items = _parser.ParseShoppingBasket(input);
        var receipt = _receiptService.GenerateReceipt(items);

        Assert.Equal(6.70m, receipt.TotalTax.Amount);
        Assert.Equal(74.68m, receipt.Total.Amount);

        Assert.Collection(receipt.LineItems,
            item => {
                Assert.Equal("bottle of perfume", item.Product.Name);
                Assert.True(item.Product.IsImported);
                Assert.Equal(32.19m, item.PriceWithTax.Amount);
            },
            item => {
                Assert.Equal("bottle of perfume", item.Product.Name);
                Assert.False(item.Product.IsImported);
                Assert.Equal(20.89m, item.PriceWithTax.Amount);
            },
            item => {
                Assert.Equal("packet of headache pills", item.Product.Name);
                Assert.Equal(9.75m, item.PriceWithTax.Amount);
            },
            item => {
                Assert.Equal("box of chocolates", item.Product.Name);
                Assert.True(item.Product.IsImported);
                Assert.Equal(11.85m, item.PriceWithTax.Amount);
            }
        );
    }

    [Fact]
    public void FormatterOutput_MatchesExpectedFormat()
    {
        var input = new[]
        {
            "1 book at 12.49",
            "1 music CD at 14.99",
            "1 chocolate bar at 0.85"
        };

        var items = _parser.ParseShoppingBasket(input);
        var receipt = _receiptService.GenerateReceipt(items);
        var output = _formatter.Format(receipt);

        var expectedLines = new[]
        {
            "1 book: 12.49",
            "1 music CD: 16.49",
            "1 chocolate bar: 0.85",
            "Sales Taxes: 1.50",
            "Total: 29.83"
        };

        var actualLines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.Equal(expectedLines, actualLines);
    }
}