using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LottasFleaMarket.Models.Enums
{
    public class Category
    {
        public static readonly Category Fishing = new Category("Fishing", 40);
        public static readonly Category Shoes = new Category("Shoes", 40);
        public static readonly Category Nature = new Category("Nature", 30);
        private static List<Category> categories = new List<Category>();
        

        public string Name { get; }
        public decimal Price { get; }

        private Category(string name, decimal price)
        {
            Name = name;
            Price = price;
            
            categories.Add(Fishing);
            categories.Add(Shoes);
            categories.Add(Nature);
        }

        public static Category getCategory(int index)
        {
            return categories[index];
        }
    }
}