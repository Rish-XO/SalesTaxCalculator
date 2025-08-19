using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Domain.Models;

public class LineItem
{
    public LineItem(Product product, int quantity, Money tax)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));
        Tax = tax ?? throw new ArgumentNullException(nameof(tax));
        
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        
        Quantity = quantity;
    }

    public Product Product { get; }
    public int Quantity { get; }
    public Money Tax { get; }
    
    public Money TotalPrice => (Product.BasePrice + Tax) * Quantity;
    public Money PriceWithTax => Product.BasePrice + Tax;

    public string GetFormattedName()
    {
        return Product.ToString();
    }

    public override string ToString()
    {
        return $"{Quantity} {GetFormattedName()}: {PriceWithTax:F2}";
    }
}