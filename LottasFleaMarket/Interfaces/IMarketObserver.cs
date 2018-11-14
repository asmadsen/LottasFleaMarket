using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Models;

namespace LottasFleaMarket.Interfaces {
    public interface IMarketObserver {
        void OnNext(Seller seller, IItem item);
    }
}