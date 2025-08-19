using SalesTaxCalculator.Core.Domain.ValueObjects;
using Xunit;

namespace SalesTaxCalculator.Tests.Domain;

public class MoneyTests
{
    [Fact]
    public void Constructor_WithValidAmount_CreatesInstance()
    {
        var money = new Money(10.50m);
        Assert.Equal(10.50m, money.Amount);
    }

    [Fact]
    public void Constructor_WithNegativeAmount_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Money(-5m));
    }

    [Fact]
    public void Add_TwoMoneyValues_ReturnsSum()
    {
        var money1 = new Money(10m);
        var money2 = new Money(5m);
        var result = money1.Add(money2);
        Assert.Equal(15m, result.Amount);
    }

    [Fact]
    public void RoundUpToNearest_WithFiveCents_RoundsCorrectly()
    {
        var testCases = new[]
        {
            (1.00m, 1.00m),
            (1.01m, 1.05m),
            (1.02m, 1.05m),
            (1.03m, 1.05m),
            (1.04m, 1.05m),
            (1.05m, 1.05m),
            (1.06m, 1.10m),
            (1.09m, 1.10m)
        };

        foreach (var (input, expected) in testCases)
        {
            var money = new Money(input);
            var rounded = money.RoundUpToNearest(0.05m);
            Assert.Equal(expected, rounded.Amount);
        }
    }

    [Fact]
    public void Equals_WithSameAmount_ReturnsTrue()
    {
        var money1 = new Money(10.50m);
        var money2 = new Money(10.50m);
        Assert.Equal(money1, money2);
    }

    [Fact]
    public void Operators_WorkCorrectly()
    {
        var money1 = new Money(10m);
        var money2 = new Money(5m);
        
        var sum = money1 + money2;
        Assert.Equal(15m, sum.Amount);
        
        var diff = money1 - money2;
        Assert.Equal(5m, diff.Amount);
        
        var product = money1 * 2;
        Assert.Equal(20m, product.Amount);
    }

    [Fact]
    public void Subtract_WithLargerAmount_ThrowsInvalidOperationException()
    {
        var money1 = new Money(5m);
        var money2 = new Money(10m);
        
        var exception = Assert.Throws<InvalidOperationException>(() => money1.Subtract(money2));
        Assert.Contains("would result in a negative amount", exception.Message);
    }

    [Fact]
    public void Multiply_WithNegativeFactor_HandledByConstructor()
    {
        var money = new Money(10m);
        
        Assert.Throws<ArgumentException>(() => money.Multiply(-2));
    }
}