using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;
using SalesTaxCalculator.Core.Strategies;

namespace SalesTaxCalculator.Core.Services;

public class TaxCalculator : ITaxCalculator
{
    private readonly ITaxStrategy _taxStrategy;

    public TaxCalculator(ITaxStrategy taxStrategy)
    {
        _taxStrategy = taxStrategy ?? throw new ArgumentNullException(nameof(taxStrategy));
    }

    public Money CalculateTax(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        return _taxStrategy.CalculateTax(product);
    }
}