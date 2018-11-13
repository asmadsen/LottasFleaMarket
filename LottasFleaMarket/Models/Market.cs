using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LottasFleaMarket.Interfaces;

namespace LottasFleaMarket.Models {
    public class Market {
        private static readonly object _singletonLock = new object();
        private static Market _market;
        private ISet<IMarketObserver> _observers = new HashSet<IMarketObserver>();
        private Dictionary<Seller, ISet<Item>> _itemsForSale = new Dictionary<Seller, ISet<Item>>();

        private Market() {
        }

        public static Market GetInstance() {
            if (_market != null) {
                return _market;
            }
            lock (_singletonLock) {
                return _market ?? (_market = new Market());
            }
        }

        public void PublishItem(Seller seller, Item item) {
            if (!_itemsForSale.ContainsKey(seller)) {
                _itemsForSale.Add(seller, new HashSet<Item>());
            }
            Console.WriteLine($"{seller.Name} is selling their #{item.SellerItemId} for ${String.Format("{0:0.00}", item.Price)}");

            var items = _itemsForSale.GetValueOrDefault(seller);
            items.Add(item);
            
            _observers.ToList().ForEach(observer => new Thread(() => observer.OnNext(seller, item)).Start());
        }

        public void Observe(IMarketObserver observer) {
            if (_itemsForSale.Count > 0) {
                foreach (var (seller, items) in _itemsForSale) {
                    foreach (var item in items) {
                        observer.OnNext(seller, item);
                    }
                }
            }
            _observers.Add(observer);
        }

        public void UnPublishItem(Seller seller, Item item) {
            if (_itemsForSale.ContainsKey(seller)) {
                var items = _itemsForSale.GetValueOrDefault(seller);
                items.Remove(item);
            }
        }
    }
}