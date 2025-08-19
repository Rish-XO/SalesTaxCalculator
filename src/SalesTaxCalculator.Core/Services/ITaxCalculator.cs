using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Services;

public interface ITaxCalculator
{
    Money CalculateTax(Product product);
}