using SalesTaxCalculator.Core.Domain.Models;

namespace SalesTaxCalculator.Core.Services;

public class ReceiptService : IReceiptService
{
    private readonly ITaxCalculator _taxCalculator;

    public ReceiptService(ITaxCalculator taxCalculator)
    {
        _taxCalculator = taxCalculator ?? throw new ArgumentNullException(nameof(taxCalculator));
    }

    public Receipt GenerateReceipt(IEnumerable<(Product Product, int Quantity)> items)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        var receipt = new Receipt();

        foreach (var (product, quantity) in items)
        {
            var tax = _taxCalculator.CalculateTax(product);
            var lineItem = new LineItem(product, quantity, tax);
            receipt.AddLineItem(lineItem);
        }

        return receipt;
    }
}