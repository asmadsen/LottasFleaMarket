using System;
using System.Collections.Generic;
using System.Linq;

namespace LottasFleaMarket.Models {
    public class Seller : Person {
        private ISet<Item> _upForSale = new HashSet<Item>();
        
        public Boolean HasMoreItems => Belongings.Count > 0;

        public Seller(string name, int numberOfBelongings = -1) : base(name, 0) {
            var random = new Random();
            if (numberOfBelongings == -1) {
                numberOfBelongings = random.Next(10, 30);
            }
            for (int i = 0; i < numberOfBelongings; i++) {
                var price = new decimal(random.NextDouble()) * (10 * random.Next(1, 8));
                Belongings.Add(new Item(i + 1, price));
            }
        }

        public void SellItems(int numberOfItemsToSell) {
            if (numberOfItemsToSell > Belongings.Count) {
                numberOfItemsToSell = Belongings.Count;
            }

            numberOfItemsToSell--;
            var random = new Random();

            while (numberOfItemsToSell >= 0) {
                var item = Belongings.ToArray()[random.Next(0, numberOfItemsToSell--)];
                Belongings.Remove(item);
                _upForSale.Add(item);
                Market.GetInstance().PublishItem(this, item);
            }
        }

        public bool BuyItem(Item item) {
            lock (item) {
                if (!_upForSale.Contains(item)) return false;
                Market.GetInstance().UnPublishItem(this, item);
                _upForSale.Remove(item);
                if (_upForSale.Count == 0 && Belongings.Count == 0) {
                    Console.WriteLine($"{Name} has sold all their items");
                }
                return true;
            }
        }
    }
}