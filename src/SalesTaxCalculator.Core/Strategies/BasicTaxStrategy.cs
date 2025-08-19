using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Strategies;

public class BasicTaxStrategy : ITaxStrategy
{
    private readonly decimal _taxRate;
    private const decimal RoundingFactor = 0.05m;

    public BasicTaxStrategy(decimal taxRate = 0.10m)
    {
        if (taxRate < 0 || taxRate > 1)
            throw new ArgumentException("Tax rate must be between 0 and 1", nameof(taxRate));
        
        _taxRate = taxRate;
    }

    public Money CalculateTax(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        if (!IsApplicable(product))
            return Money.Zero;
        
        var tax = product.BasePrice * _taxRate;
        return tax.RoundUpToNearest(RoundingFactor);
    }

    public bool IsApplicable(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        return !product.IsExemptFromBasicTax;
    }
}