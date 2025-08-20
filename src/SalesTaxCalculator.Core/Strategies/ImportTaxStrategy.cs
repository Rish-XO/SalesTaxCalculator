using SalesTaxCalculator.Core.Configuration;
using SalesTaxCalculator.Core.Domain.Models;

namespace SalesTaxCalculator.Core.Strategies;

public class ImportTaxStrategy : BaseTaxStrategy
{
    public ImportTaxStrategy(TaxConfiguration configuration)
        : base(
            configuration?.ImportDutyRate ?? throw new ArgumentNullException(nameof(configuration)),
            configuration.RoundingFactor)
    {
    }

    public override bool IsApplicable(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        return product.IsImported;
    }
}