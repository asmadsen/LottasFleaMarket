using System;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Models.Enums;

namespace LottasFleaMarket.Decorators {
    public abstract class AbstractItemDecorator : IItem {
        protected IItem _item;

        public AbstractItemDecorator(IItem item) {
            _item = item;
        }

        public Guid Id => _item.Id;
        public int SellerItemId => _item.SellerItemId;
        public decimal Price => _item.Price;
        public string Description => _item.Description;
        public Category Category => _item.Category;
    }
}