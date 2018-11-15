using System;
using System.Collections.Generic;
using System.Linq;
using LottasFleaMarket.Decorators;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket.Models {
    public class Seller : Person {
        private ISet<IItem> _ItemsListedForSale = new HashSet<IItem>();

        public Boolean HasMoreNotListedItems => Belongings.Count > 0;
        public int _NumberOfItemsSellerStartWith { get; set; }
        public decimal AmountSoldFor { get; protected set; }
        public decimal InitialValueOfItems { get; set; }

        public Seller(int NumberOfItemsSellerStartWith = -1) : base(0)
        {
            _NumberOfItemsSellerStartWith = NumberOfItemsSellerStartWith;
            MakeSellersItemsReadyForSale(NumberOfItemsSellerStartWith);
        }

        private void MakeSellersItemsReadyForSale(int NumberOfItemsSellerStartWith)
        {
            var random = new Random();

            if (NumberOfItemsSellerStartWith == -1)
            {
                NumberOfItemsSellerStartWith = random.Next(10, 30);
            }

            for (int i = 0; i < NumberOfItemsSellerStartWith; i++)
            {
                var ItemId = i + 1;
                IItem item = new Item(ItemId);
                item = new CollectorsDecorator(item);
                

                Belongings.Add(item);
                InitialValueOfItems += item.Category.Price;
            }
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

                if (_ItemsListedForSale.Count == 0 && Belongings.Count == 0) {
                    Console.WriteLine($"{Name} has sold all their items");
                }

                return true;
            }
        }
    }
}