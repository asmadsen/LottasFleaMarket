using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LottasFleaMarket.Interfaces;
using LottasFleaMarket.Interfaces.Decorators;

namespace LottasFleaMarket.Models {
    public class Market {
        private static readonly object _singletonLock = new object();
        private static Market _market;
        private ISet<IMarketObserver> _observers = new HashSet<IMarketObserver>();
        private Dictionary<Seller, ISet<IItem>> _itemsForSale = new Dictionary<Seller, ISet<IItem>>();

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

        public void PublishItem(Seller seller, IItem item) {
            
            lock (this) {
                if (!_itemsForSale.ContainsKey(seller)) {
                    _itemsForSale.Add(seller, new HashSet<IItem>());
                }
            }
            
            Console.WriteLine("{0,-8} is selling item nr {1,2} in category {2,8} for {3,3}", seller.Name, item.SellerItemId, item.Category.Name.ToLower(), item.Price);

            var items = _itemsForSale.GetValueOrDefault(seller);
            items.Add(item);
            
            _observers.ToList().ForEach(observer => new Thread(() => {
                observer.OnNext(seller, item);
            }).Start());
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

        public void UnPublishItem(Seller seller, IItem item) {
            if (_itemsForSale.ContainsKey(seller)) {
                var items = _itemsForSale.GetValueOrDefault(seller);
                items.Remove(item);
            }
        }
    }
}