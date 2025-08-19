using SalesTaxCalculator.Core.Domain.Models;

namespace SalesTaxCalculator.Core.Parsers;

public interface IInputParser
{
    IEnumerable<(Product Product, int Quantity)> ParseShoppingBasket(string[] lines);
}