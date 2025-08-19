using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Domain.Models;

public class Product
{
    public Product(string name, Money basePrice, ProductCategory category, bool isImported)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty", nameof(name));
        
        Name = name.Trim();
        BasePrice = basePrice ?? throw new ArgumentNullException(nameof(basePrice));
        Category = category;
        IsImported = isImported;
    }

    public string Name { get; }
    public Money BasePrice { get; }
    public ProductCategory Category { get; }
    public bool IsImported { get; }

    public bool IsExemptFromBasicTax =>
        Category == ProductCategory.Book ||
        Category == ProductCategory.Food ||
        Category == ProductCategory.Medical;

    public override string ToString()
    {
        var importedPrefix = IsImported ? "imported " : "";
        return $"{importedPrefix}{Name}";
    }
}