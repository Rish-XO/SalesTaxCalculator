using System.Text;
using SalesTaxCalculator.Core.Domain.Models;

namespace SalesTaxCalculator.Core.Formatters;

public class ConsoleReceiptFormatter : IReceiptFormatter
{
    public string Format(Receipt receipt)
    {
        if (receipt == null)
            throw new ArgumentNullException(nameof(receipt));

        var sb = new StringBuilder();

        foreach (var item in receipt.LineItems)
        {
            sb.AppendLine(item.ToString());
        }

        sb.AppendLine($"Sales Taxes: {receipt.TotalTax:F2}");
        sb.AppendLine($"Total: {receipt.Total:F2}");

        return sb.ToString();
    }
}