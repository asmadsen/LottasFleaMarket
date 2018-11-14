using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace LottasFleaMarket.Models.Enums
{
    public class Category
    {
        public static readonly Category Fishing = new Category("Fishing", 40);
        public static readonly Category Shoes = new Category("Shoes", 20);
        public static readonly Category Nature = new Category("Nature", 50);

        public string Name { get; }
        public decimal Price { get; }

        private Category(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public static Category GetCategory(string name)
        {
            var fields = typeof(Category).GetFields(BindingFlags.Static | BindingFlags.Public);
            var field = fields.First(entry => String.Equals(entry.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (field == null) {
                return null;
            }
            return field.GetValue(null) as Category;
        }

        public static Category RandomCategory() {
            var fields = typeof(Category).GetFields(BindingFlags.Static | BindingFlags.Public);
            var index = new Random().Next(0, fields.Length - 1);
            return fields[index].GetValue(null) as Category;
        }
    }
}