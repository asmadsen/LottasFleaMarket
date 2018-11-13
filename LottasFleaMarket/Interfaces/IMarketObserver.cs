using LottasFleaMarket.Models;

namespace LottasFleaMarket.Interfaces {
    public interface IMarketObserver {
        void OnNext(Seller seller, Item item);

        void OnCompleted();
    }
}