using System.Text.RegularExpressions;
using SalesTaxCalculator.Core.Domain.Models;

namespace SalesTaxCalculator.Core.Services;

public interface IProductCategoryService
{
    ProductCategory DetermineCategory(string productDescription);
}

public class ProductCategoryService : IProductCategoryService
{
    private static readonly Dictionary<string, ProductCategory> CategoryKeywords = new()
    {
        { "book", ProductCategory.Book },
        
        // Food items
        { "chocolate", ProductCategory.Food },
        { "chocolates", ProductCategory.Food },
        { "bread", ProductCategory.Food },
        { "milk", ProductCategory.Food },
        { "cheese", ProductCategory.Food },
        { "eggs", ProductCategory.Food },
        { "egg", ProductCategory.Food },
        { "meat", ProductCategory.Food },
        { "beef", ProductCategory.Food },
        { "chicken", ProductCategory.Food },
        { "pork", ProductCategory.Food },
        { "fish", ProductCategory.Food },
        { "salmon", ProductCategory.Food },
        { "tuna", ProductCategory.Food },
        { "apple", ProductCategory.Food },
        { "apples", ProductCategory.Food },
        { "banana", ProductCategory.Food },
        { "bananas", ProductCategory.Food },
        { "orange", ProductCategory.Food },
        { "oranges", ProductCategory.Food },
        { "grape", ProductCategory.Food },
        { "grapes", ProductCategory.Food },
        { "strawberry", ProductCategory.Food },
        { "strawberries", ProductCategory.Food },
        { "carrot", ProductCategory.Food },
        { "carrots", ProductCategory.Food },
        { "potato", ProductCategory.Food },
        { "potatoes", ProductCategory.Food },
        { "tomato", ProductCategory.Food },
        { "tomatoes", ProductCategory.Food },
        { "onion", ProductCategory.Food },
        { "onions", ProductCategory.Food },
        { "rice", ProductCategory.Food },
        { "pasta", ProductCategory.Food },
        { "cereal", ProductCategory.Food },
        { "oats", ProductCategory.Food },
        { "flour", ProductCategory.Food },
        { "sugar", ProductCategory.Food },
        { "salt", ProductCategory.Food },
        { "pepper", ProductCategory.Food },
        { "oil", ProductCategory.Food },
        { "butter", ProductCategory.Food },
        { "yogurt", ProductCategory.Food },
        { "juice", ProductCategory.Food },
        { "coffee", ProductCategory.Food },
        { "tea", ProductCategory.Food },
        { "water", ProductCategory.Food },
        { "soda", ProductCategory.Food },
        { "chips", ProductCategory.Food },
        { "crackers", ProductCategory.Food },
        { "cookies", ProductCategory.Food },
        { "cookie", ProductCategory.Food },
        { "nuts", ProductCategory.Food },
        { "almonds", ProductCategory.Food },
        { "peanuts", ProductCategory.Food },
        { "cake", ProductCategory.Food },
        { "pie", ProductCategory.Food },
        { "candy", ProductCategory.Food },
        { "gum", ProductCategory.Food },
        { "ice cream", ProductCategory.Food },
        { "pizza", ProductCategory.Food },
        { "sandwich", ProductCategory.Food },
        { "soup", ProductCategory.Food },
        { "salad", ProductCategory.Food },
        
        // Medical items
        { "pills", ProductCategory.Medical },
        { "pill", ProductCategory.Medical },
        { "headache", ProductCategory.Medical },
        { "aspirin", ProductCategory.Medical },
        { "medicine", ProductCategory.Medical },
        { "medication", ProductCategory.Medical },
        { "drug", ProductCategory.Medical },
        { "tablet", ProductCategory.Medical },
        { "tablets", ProductCategory.Medical },
        { "capsule", ProductCategory.Medical },
        { "capsules", ProductCategory.Medical },
        { "bandage", ProductCategory.Medical },
        { "bandages", ProductCategory.Medical },
        { "antibiotic", ProductCategory.Medical },
        { "antibiotics", ProductCategory.Medical },
        { "vitamin", ProductCategory.Medical },
        { "vitamins", ProductCategory.Medical },
        { "painkiller", ProductCategory.Medical },
        { "painkillers", ProductCategory.Medical },
        { "prescription", ProductCategory.Medical },
        { "ibuprofen", ProductCategory.Medical },
        { "acetaminophen", ProductCategory.Medical },
        { "tylenol", ProductCategory.Medical },
        { "advil", ProductCategory.Medical },
        { "antacid", ProductCategory.Medical },
        { "cough syrup", ProductCategory.Medical },
        { "cough", ProductCategory.Medical },
        { "syrup", ProductCategory.Medical },
        { "thermometer", ProductCategory.Medical },
        { "inhaler", ProductCategory.Medical },
        { "insulin", ProductCategory.Medical }
    };

    public ProductCategory DetermineCategory(string productDescription)
    {
        if (string.IsNullOrWhiteSpace(productDescription))
            return ProductCategory.General;

        var lowerDescription = productDescription.ToLowerInvariant();
        
        foreach (var kvp in CategoryKeywords)
        {
            // Use word boundary matching to match whole words only
            var pattern = $@"\b{Regex.Escape(kvp.Key)}\b";
            if (Regex.IsMatch(lowerDescription, pattern))
            {
                return kvp.Value;
            }
        }

        return ProductCategory.General;
    }
}