using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Strategies;

public interface ITaxStrategy
{
    Money CalculateTax(Product product);
    bool IsApplicable(Product product);
}