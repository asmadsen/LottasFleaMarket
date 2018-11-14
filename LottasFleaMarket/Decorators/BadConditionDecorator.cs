using LottasFleaMarket.Interfaces.Decorators;

namespace LottasFleaMarket.Decorators {
    public class BadConditionDecorator : AbstractItemDecorator {
        public BadConditionDecorator(IItem item) : base(item) {
        }
        
        public new decimal Price => _item.Price * (decimal) 0.50;
    }
}