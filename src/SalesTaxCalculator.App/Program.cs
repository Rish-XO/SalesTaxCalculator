using SalesTaxCalculator.Core.Configuration;
using SalesTaxCalculator.Core.Formatters;
using SalesTaxCalculator.Core.Parsers;
using SalesTaxCalculator.Core.Services;
using SalesTaxCalculator.Core.Strategies;

namespace SalesTaxCalculator.App;

class Program
{
    static void Main(string[] args)
    {
        // Setup configuration
        var taxConfig = new TaxConfiguration
        {
            BasicTaxRate = 0.10m,
            ImportDutyRate = 0.05m,
            RoundingFactor = 0.05m
        };

        // Setup services
        var categoryService = new ProductCategoryService();
        var productFactory = new ProductFactory(categoryService);
        
        // Setup tax strategies with configuration
        var basicTaxStrategy = new BasicTaxStrategy(taxConfig);
        var importTaxStrategy = new ImportTaxStrategy(taxConfig);
        var compositeTaxStrategy = new CompositeTaxStrategy(basicTaxStrategy, importTaxStrategy);
        
        // Setup application services
        var taxCalculator = new TaxCalculator(compositeTaxStrategy);
        var receiptService = new ReceiptService(taxCalculator);
        var parser = new ShoppingBasketParser(productFactory);
        var formatter = new ConsoleReceiptFormatter();

        Console.WriteLine("=== Sales Tax Calculator ===");
        Console.WriteLine();
        Console.WriteLine("Enter your shopping items (format: 'quantity item at price')");
        Console.WriteLine("Examples:");
        Console.WriteLine("  1 book at 12.49");
        Console.WriteLine("  2 imported chocolate bars at 5.00");
        Console.WriteLine("  1 packet of headache pills at 9.75");
        Console.WriteLine();
        Console.WriteLine("Press Enter on an empty line when finished:");
        Console.WriteLine();
        
        var userItems = new List<string>();
        string line;
        
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            userItems.Add(line.Trim());
        }
        
        if (userItems.Count == 0)
        {
            Console.WriteLine("No items entered. Goodbye!");
            return;
        }
        
        try
        {
            Console.WriteLine("\n=== Your Receipt ===");
            var items = parser.ParseShoppingBasket(userItems.ToArray());
            var receipt = receiptService.GenerateReceipt(items);
            var output = formatter.Format(receipt);
            
            Console.Write(output);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing your input: {ex.Message}");
            Console.WriteLine("Please check the format and try again.");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}