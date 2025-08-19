namespace SalesTaxCalculator.Core.Configuration;

public class TaxConfiguration
{
    public decimal BasicTaxRate { get; set; } = 0.10m;
    public decimal ImportDutyRate { get; set; } = 0.05m;
    public decimal RoundingFactor { get; set; } = 0.05m;
}