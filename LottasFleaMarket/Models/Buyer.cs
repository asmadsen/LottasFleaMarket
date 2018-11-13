using System;
using System.Threading;
using LottasFleaMarket.Interfaces;

namespace LottasFleaMarket.Models {
    class Buyer : Person, IMarketObserver {
        
        public Buyer(string name, decimal money) : base(name, money) {
            Market.GetInstance().Observe(this);
        }

        public void OnCompleted() {
            throw new NotImplementedException();
        }

        public void OnNext(Seller seller, Item item) {
            
            if (!IsInteresting(item)) return;
            
            Thread.Sleep(new Random().Next(100, 500));
            
            if (seller.BuyItem(item)) {
                var tabs = "                    ";
                Console.WriteLine($"{tabs}{Name} bought {seller.Name}'s #{item.SellerItemId} for ${String.Format("{0:0.00}", item.Price)}");
                Belongings.Add(item);
            }
        }

        private Boolean IsInteresting(Item value) {
            return true;
        }

        public override bool Equals(object obj) {
            return obj is Buyer && ((Buyer) obj).Id.Equals(Id);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}