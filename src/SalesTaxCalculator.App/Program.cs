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

        var testCases = new Dictionary<string, string[]>
        {
            {
                "Input 1", new[]
                {
                    "1 book at 12.49",
                    "1 music CD at 14.99",
                    "1 chocolate bar at 0.85"
                }
            },
            {
                "Input 2", new[]
                {
                    "1 imported box of chocolates at 10.00",
                    "1 imported bottle of perfume at 47.50"
                }
            },
            {
                "Input 3", new[]
                {
                    "1 imported bottle of perfume at 27.99",
                    "1 bottle of perfume at 18.99",
                    "1 packet of headache pills at 9.75",
                    "1 box of imported chocolates at 11.25"
                }
            }
        };

        foreach (var testCase in testCases)
        {
            Console.WriteLine($"\n{testCase.Key}:");
            foreach (var line in testCase.Value)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine($"\nOutput {testCase.Key.Replace("Input", "")}:");
            
            var items = parser.ParseShoppingBasket(testCase.Value);
            var receipt = receiptService.GenerateReceipt(items);
            var output = formatter.Format(receipt);
            
            Console.Write(output);
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
