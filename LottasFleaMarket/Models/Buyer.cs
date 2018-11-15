using System;
using System.Threading;
using LottasFleaMarket.Interfaces;
using LottasFleaMarket.Interfaces.Decorators;

namespace LottasFleaMarket.Models {
    public class Buyer : Person, IMarketObserver {
        private readonly Action _unSubscribe;
        public decimal AmountUsed { get; protected set; }


        public Buyer(decimal saldo) : base(saldo) {
            _unSubscribe = Market.GetInstance().Subscribe(this);
        }

        public void OnNext(Seller seller, IItem item) {
         
            if (!IsInteresting(item)) return;
            if (!seller.BuyItem(item)) return;
            
            lock (this)
            {
                //Thread.Sleep(new Random().Next(100, 200));
                BuyItem(item, seller);
            }

            if (Saldo == 0) {
                _unSubscribe();
            }
        }

        private void BuyItem(IItem item, Seller seller)
        {
            const string tabs = "                    ";
            Console.WriteLine($"{tabs}{Name} bought {seller.Name}'s #{item.SellerItemId} for ${String.Format("{0:0.00}", item.Price)}");
            ItemsNotYetListedForSale.Add(item);
            Saldo -= item.Price;
            AmountUsed += item.Price;
            
        }

        public bool IsInteresting(IItem item)
        {
            decimal listingPrice = item.Category.Price;
            
            lock (this) {
                if (Saldo < item.Price) return false;
                
                if (!this.IsSmart)
                {
                    return true;
                }

                if (!(item.Price < listingPrice)) return false;
                
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