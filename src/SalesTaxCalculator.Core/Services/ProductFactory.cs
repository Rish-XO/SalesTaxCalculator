using System.Text.RegularExpressions;
using SalesTaxCalculator.Core.Domain.Models;
using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Services;

public interface IProductFactory
{
    Product CreateProduct(string description, decimal price);
}

public class ProductFactory : IProductFactory
{
    private readonly IProductCategoryService _categoryService;

    public ProductFactory(IProductCategoryService categoryService)
    {
        _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
    }

    public Product CreateProduct(string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Product description cannot be empty", nameof(description));
        
        if (price < 0)
            throw new ArgumentException("Price cannot be negative", nameof(price));

        var isImported = description.Contains("imported", StringComparison.OrdinalIgnoreCase);
        var category = _categoryService.DetermineCategory(description);
        var name = CleanProductName(description);

        return new Product(name, new Money(price), category, isImported);
    }

    private string CleanProductName(string description)
    {
        var name = description;
        
        // Remove "imported" from the name
        name = Regex.Replace(name, @"\bimported\b", "", RegexOptions.IgnoreCase).Trim();
        
        // Clean up multiple spaces
        name = Regex.Replace(name, @"\s+", " ");
        
        return name.Trim();
    }
}