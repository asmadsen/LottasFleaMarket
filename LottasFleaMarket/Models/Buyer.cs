using System;
using System.Threading;
using LottasFleaMarket.Interfaces;

namespace LottasFleaMarket.Models {
    class Buyer : Person, IMarketObserver {
        public Buyer(decimal money) : base(money) {
            Market.GetInstance().Observe(this);
        }

        public void OnNext(Seller seller, Item item) {
            if (!IsInteresting(item)) return;
            if (!seller.BuyItem(item)) return;
            const string tabs = "                    ";
            Console.WriteLine(
                $"{tabs}{Name} bought {seller.Name}'s #{item.SellerItemId} for ${String.Format("{0:0.00}", item.Price)}");
            lock (this) {
                Belongings.Add(item);
                Money -= item.Price;
            }
        }

        private bool IsInteresting(Item value) {
            lock (this) {
                if (Money < value.Price) return false;
            }

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