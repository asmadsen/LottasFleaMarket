using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Models.Enums;

namespace LottasFleaMarket.Decorators {
    public class BadConditionDecorator : AbstractItemDecorator {
        public BadConditionDecorator(IItem item) : base(item) {
        }
        
        public new decimal Price => _item.Price * (decimal) 0.50;
        public new Condition Condition => Condition.Bad;
    }
}