using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Strategies;

public class CompositeTaxStrategy : ITaxStrategy
{
    private readonly List<ITaxStrategy> _strategies;

    public CompositeTaxStrategy(params ITaxStrategy[] strategies)
    {
        _strategies = strategies?.ToList() ?? new List<ITaxStrategy>();
    }

    public void AddStrategy(ITaxStrategy strategy)
    {
        if (strategy == null)
            throw new ArgumentNullException(nameof(strategy));
        
        _strategies.Add(strategy);
    }

    public Money CalculateTax(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        return _strategies
            .Where(s => s.IsApplicable(product))
            .Aggregate(Money.Zero, (total, strategy) => total + strategy.CalculateTax(product));
    }

    public bool IsApplicable(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        return _strategies.Any(s => s.IsApplicable(product));
    }
}