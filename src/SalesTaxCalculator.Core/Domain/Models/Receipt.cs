using SalesTaxCalculator.Core.Domain.ValueObjects;

namespace SalesTaxCalculator.Core.Domain.Models;

public class Receipt
{
    private readonly List<LineItem> _lineItems;

    public Receipt()
    {
        _lineItems = new List<LineItem>();
    }

    public IReadOnlyList<LineItem> LineItems => _lineItems.AsReadOnly();

    public Money TotalTax => _lineItems.Aggregate(
        Money.Zero,
        (total, item) => total + (item.Tax * item.Quantity));

    public Money Total => _lineItems.Aggregate(
        Money.Zero,
        (total, item) => total + item.TotalPrice);

    public void AddLineItem(LineItem lineItem)
    {
        if (lineItem == null)
            throw new ArgumentNullException(nameof(lineItem));
        
        _lineItems.Add(lineItem);
    }

    public void AddLineItems(IEnumerable<LineItem> lineItems)
    {
        if (lineItems == null)
            throw new ArgumentNullException(nameof(lineItems));
        
        foreach (var item in lineItems)
        {
            AddLineItem(item);
        }
    }
}