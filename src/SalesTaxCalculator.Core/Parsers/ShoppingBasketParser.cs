using System.Text.RegularExpressions;
using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Parsers;

public class ShoppingBasketParser : IInputParser
{
    private static readonly Regex InputPattern = new Regex(
        @"^(\d+)\s+(.+?)\s+at\s+([\d.]+)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Dictionary<string, ProductCategory> CategoryKeywords = new()
    {
        { "book", ProductCategory.Book },
        { "chocolate", ProductCategory.Food },
        { "chocolates", ProductCategory.Food },
        { "pills", ProductCategory.Medical },
        { "pill", ProductCategory.Medical },
        { "headache", ProductCategory.Medical }
    };

    public IEnumerable<(Product Product, int Quantity)> ParseShoppingBasket(string[] lines)
    {
        if (lines == null)
            throw new ArgumentNullException(nameof(lines));

        var results = new List<(Product Product, int Quantity)>();

        foreach (var line in lines.Where(l => !string.IsNullOrWhiteSpace(l)))
        {
            var parsed = ParseLine(line.Trim());
            if (parsed.HasValue)
            {
                results.Add(parsed.Value);
            }
        }

        return results;
    }

    private (Product Product, int Quantity)? ParseLine(string line)
    {
        var match = InputPattern.Match(line);
        if (!match.Success)
            return null;

        var quantity = int.Parse(match.Groups[1].Value);
        var description = match.Groups[2].Value;
        var price = decimal.Parse(match.Groups[3].Value);

        var isImported = description.Contains("imported", StringComparison.OrdinalIgnoreCase);
        var category = DetermineCategory(description);
        var name = CleanProductName(description);

        var product = new Product(name, new Money(price), category, isImported);
        return (product, quantity);
    }

    private ProductCategory DetermineCategory(string description)
    {
        var lowerDescription = description.ToLowerInvariant();
        
        foreach (var kvp in CategoryKeywords)
        {
            if (lowerDescription.Contains(kvp.Key))
            {
                return kvp.Value;
            }
        }

        return ProductCategory.General;
    }

    private string CleanProductName(string description)
    {
        var name = description;
        
        name = Regex.Replace(name, @"\bimported\b", "", RegexOptions.IgnoreCase).Trim();
        
        name = Regex.Replace(name, @"\s+", " ");
        
        return name.Trim();
    }
}