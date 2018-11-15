using System;
using System.Threading;
using LottasFleaMarket.Interfaces;
using LottasFleaMarket.Interfaces.Decorators;

namespace LottasFleaMarket.Models {
    public class Buyer : Person, IMarketObserver
    {

        public decimal AmountUsed { get; protected set; }


        public Buyer(decimal balance) : base(balance) {
            Market.GetInstance().Subscribe(this);
        }

        public void OnNext(Seller seller, IItem item) {
         
            if (!IsInteresting(item)) return;
            if (!seller.BuyItem(item)) return;
            
            lock (this)
            {
                //Thread.Sleep(new Random().Next(100, 200));
                BuyItem(item, seller);
            }
        }

        private void BuyItem(IItem item, Seller seller)
        {
            const string tabs = "                    ";
            Console.WriteLine($"{tabs}{Name} bought {seller.Name}'s #{item.SellerItemId} for ${String.Format("{0:0.00}", item.Price)}");
            Belongings.Add(item);
            Balance -= item.Price;
            AmountUsed += item.Price;
            
        }

        public bool IsInteresting(IItem item)
        {
            decimal listingPrice = item.Category.Price;
            
            lock (this) {
                if (Balance < item.Price) return false;
                
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