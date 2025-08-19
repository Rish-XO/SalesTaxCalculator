using SalesTaxCalculator.Core.Domain.Models;

namespace SalesTaxCalculator.Core.Services;

public interface IReceiptService
{
    Receipt GenerateReceipt(IEnumerable<(Product Product, int Quantity)> items);
}