using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Strategies;

public class ImportTaxStrategy : ITaxStrategy
{
    private readonly decimal _importDutyRate;
    private const decimal RoundingFactor = 0.05m;

    public ImportTaxStrategy(decimal importDutyRate = 0.05m)
    {
        if (importDutyRate < 0 || importDutyRate > 1)
            throw new ArgumentException("Import duty rate must be between 0 and 1", nameof(importDutyRate));
        
        _importDutyRate = importDutyRate;
    }

    public Money CalculateTax(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        if (!IsApplicable(product))
            return Money.Zero;
        
        var tax = product.BasePrice * _importDutyRate;
        return tax.RoundUpToNearest(RoundingFactor);
    }

    public bool IsApplicable(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        return product.IsImported;
    }
}