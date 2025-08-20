using SalesTaxCalculator.Core.Domain.Models;

namespace SalesTaxCalculator.Core.Services;

public interface IProductCategoryService
{
    ProductCategory DetermineCategory(string productDescription);
}

public class ProductCategoryService : IProductCategoryService
{
    private static readonly Dictionary<string, ProductCategory> CategoryKeywords = new()
    {
        { "book", ProductCategory.Book },
        { "chocolate", ProductCategory.Food },
        { "chocolates", ProductCategory.Food },
        { "pills", ProductCategory.Medical },
        { "pill", ProductCategory.Medical },
        { "headache", ProductCategory.Medical }
    };

    public ProductCategory DetermineCategory(string productDescription)
    {
        if (string.IsNullOrWhiteSpace(productDescription))
            return ProductCategory.General;

        var lowerDescription = productDescription.ToLowerInvariant();
        
        foreach (var kvp in CategoryKeywords)
        {
            if (lowerDescription.Contains(kvp.Key))
            {
                return kvp.Value;
            }
        }

        return ProductCategory.General;
    }
}