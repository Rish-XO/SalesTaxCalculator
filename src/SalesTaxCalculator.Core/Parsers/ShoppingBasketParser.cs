using System.Text.RegularExpressions;
using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Services;

namespace SalesTaxCalculator.Core.Parsers;

public class ShoppingBasketParser : IInputParser
{
    private static readonly Regex InputPattern = new Regex(
        @"^(\d+)\s+(.+?)\s+at\s+([\d.]+)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private readonly IProductFactory _productFactory;

    public ShoppingBasketParser(IProductFactory productFactory)
    {
        _productFactory = productFactory ?? throw new ArgumentNullException(nameof(productFactory));
    }

    public IEnumerable<(Product Product, int Quantity)> ParseShoppingBasket(string[] lines)
    {
        if (lines == null)
            throw new ArgumentNullException(nameof(lines));

        var results = new List<(Product Product, int Quantity)>();
        var lineNumber = 0;

        foreach (var line in lines.Where(l => !string.IsNullOrWhiteSpace(l)))
        {
            lineNumber++;
            try
            {
                var parsed = ParseLine(line.Trim());
                if (parsed.HasValue)
                {
                    results.Add(parsed.Value);
                }
                else
                {
                    throw new FormatException($"Line {lineNumber}: Invalid format. Expected format: 'quantity product at price'");
                }
            }
            catch (FormatException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FormatException($"Line {lineNumber}: Failed to parse '{line}'. {ex.Message}", ex);
            }
        }

        return results;
    }

    private (Product Product, int Quantity)? ParseLine(string line)
    {
        var match = InputPattern.Match(line);
        if (!match.Success)
            return null;

        if (!int.TryParse(match.Groups[1].Value, out var quantity) || quantity <= 0)
            throw new FormatException($"Invalid quantity: {match.Groups[1].Value}");
        
        var description = match.Groups[2].Value;
        
        if (!decimal.TryParse(match.Groups[3].Value, out var price) || price < 0)
            throw new FormatException($"Invalid price: {match.Groups[3].Value}");
        
        var product = _productFactory.CreateProduct(description, price);
        return (product, quantity);
    }
}