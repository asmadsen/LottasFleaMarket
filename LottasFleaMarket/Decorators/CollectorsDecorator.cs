using LottasFleaMarket.Interfaces.Decorators;

namespace LottasFleaMarket.Decorators {
    public class CollectorsDecorator : AbstractItemDecorator {
        public CollectorsDecorator(IItem item) : base(item) {
        }

        public new decimal Price => _item.Price * (decimal) 1.25;
    }
}