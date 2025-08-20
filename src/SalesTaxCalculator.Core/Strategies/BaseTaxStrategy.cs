using SalesTaxCalculator.Core.Configuration;
using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Strategies;

public abstract class BaseTaxStrategy : ITaxStrategy
{
    protected readonly decimal TaxRate;
    protected readonly decimal RoundingFactor;

    protected BaseTaxStrategy(decimal taxRate, decimal roundingFactor)
    {
        if (taxRate < 0 || taxRate > 1)
            throw new ArgumentException("Tax rate must be between 0 and 1", nameof(taxRate));
        
        if (roundingFactor <= 0)
            throw new ArgumentException("Rounding factor must be positive", nameof(roundingFactor));
        
        TaxRate = taxRate;
        RoundingFactor = roundingFactor;
    }

    public virtual Money CalculateTax(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        if (!IsApplicable(product))
            return Money.Zero;
        
        var tax = product.BasePrice * TaxRate;
        return tax.RoundUpToNearest(RoundingFactor);
    }

    public abstract bool IsApplicable(Product product);
}