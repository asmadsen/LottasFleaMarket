using System;

namespace LottasFleaMarket.Models {
    public class Item {
        public readonly Guid Id;
        public readonly int SellerItemId;
        private decimal _price;

        public decimal Price {
            get { return _price; }
            private set => _price = value;
        }

        public Item(int sellerItemId, decimal price) {
            Id = Guid.NewGuid();
            SellerItemId = sellerItemId;
            Price = price;
        }
    }
}