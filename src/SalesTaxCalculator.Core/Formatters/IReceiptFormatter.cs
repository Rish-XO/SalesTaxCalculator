using SalesTaxCalculator.Core.Domain.Models;

namespace SalesTaxCalculator.Core.Formatters;

public interface IReceiptFormatter
{
    string Format(Receipt receipt);
}