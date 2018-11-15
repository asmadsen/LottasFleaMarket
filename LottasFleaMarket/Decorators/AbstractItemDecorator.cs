using System;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Models.Enums;

namespace LottasFleaMarket.Decorators {
    public abstract class AbstractItemDecorator : IItem {
        protected readonly IItem _item;
        public readonly IItem _parent;

        protected AbstractItemDecorator(IItem item) {
            _item = item;
            _parent = item is AbstractItemDecorator ? ((AbstractItemDecorator) _item)._parent : _item;
        }

        public Guid Id => _item.Id;
        public int SellerItemId => _item.SellerItemId;
        public decimal Price => _item.Price;
        public string Description => _item.Description;
        public Category Category => _item.Category;
        public Condition Condition => _item.Condition;
    }
}