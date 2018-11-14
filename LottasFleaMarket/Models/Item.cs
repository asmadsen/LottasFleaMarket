using System;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Models.Enums;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket.Models {
    public class Item : IItem {
        public Item(int sellerItemId) {
            SellerItemId = sellerItemId;
            Id = Guid.NewGuid();
            Category = Category.RandomCategory();
        }

        public Guid Id { get; }
        public int SellerItemId { get; }

        public decimal Price => Category.Price;

        public Category Category { get; }
        public string Description => Category.Name;
    }
}