using System;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Models.Enums;

namespace LottasFleaMarket.Decorators {
    public class RefurbishedDecorator : AbstractItemDecorator {
        public RefurbishedDecorator(IItem item) : base(item) {
        }

        public new decimal Price {
            get {
                switch (_item.Condition) {
                    case Condition.Refurbished:
                        return _item.Price;
                    case Condition.Used:
                        return _item.Price * (decimal) 1.15;
                    case Condition.Bad:
                        return _item.Price * (decimal) 1.75;
                }
                return _item.Price;
            }
        }

        public new Condition Condition => Condition.Refurbished;
    }
}