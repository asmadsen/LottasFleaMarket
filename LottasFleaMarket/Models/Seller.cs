using System;
using System.Collections.Generic;
using System.Linq;
using LottasFleaMarket.Decorators;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Models.Enums;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket.Models {
    public class Seller : Person {
        private ISet<IItem> _ItemsListedForSale = new HashSet<IItem>();

        public Boolean HasMoreNotListedItems => Belongings.Count > 0;
        public decimal AmountSoldFor { get; protected set; }
        public int NumberOfItemsSold { get; set; }
        public decimal InitialValueOfItems { get; set; }
        public int InitialBelongingsCount;
     
        public Seller(ISet<IItem> belongings, decimal startBalance, string name) : base(startBalance, name, belongings)
        {
            InitialBelongingsCount = belongings.Count;
            InitialValueOfItems = Belongings.ToList().Sum(s => s.Price);
        }

        public void SellItems(int numberOfItemsToSell) {
            if (numberOfItemsToSell > Belongings.Count) {
                numberOfItemsToSell = Belongings.Count;
            }

            numberOfItemsToSell--;
            var random = new ThreadSafeRandom();

            var array = Belongings.ToArray();
            while (numberOfItemsToSell >= 0 && array.Length > 0) {
                var item = array[random.Next(0, numberOfItemsToSell)];
                Belongings.Remove(item);
                _ItemsListedForSale.Add(item);
                Market.GetInstance().PublishItem(this, item);
                numberOfItemsToSell--;
                array = Belongings.ToArray();
            }
        }

        public bool BuyItem(IItem item) {
            lock (item) {
                if (!_ItemsListedForSale.Contains(item)) return false;
                Market.GetInstance().UnPublishItem(this, item);
                _ItemsListedForSale.Remove(item);
                AmountSoldFor += item.Price;
                NumberOfItemsSold++;

                if (_ItemsListedForSale.Count == 0 && Belongings.Count == 0) {
                    Console.WriteLine($"{Name} has sold all their items");
                }

                return true;
            }
        }

     
        public override Report GenerateReport()
        {
            return new SellerReport(this, NumberOfItemsSold, AmountSoldFor, InitialValueOfItems);
        }
    }
}