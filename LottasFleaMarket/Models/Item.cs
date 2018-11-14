using System;
using LottasFleaMarket.Models.Enums;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket.Models {
    public class Item : PriceFactory {
        public readonly Guid Id;
        public readonly int SellerItemId;
        private decimal _price;
        private string category;

        public Item(Seller seller)
        {
            int index = new Random().Next(0,2) + 1;
            Category c = Category.getCategory(index);
            category = c.Name;
            _price = getCalculatedPrice(c, seller);
        }

        public decimal Price {
            get { return _price; }
            private set => _price = value;
        }
    }
}