using System;
using System.Threading;
using LottasFleaMarket.Interfaces;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Models.Enums;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket.Models {
    public class Buyer : Person, IMarketObserver {
        private readonly Action _unSubscribe;
        public decimal AmountUsed { get; protected set; }
        public int NumberOfItemsBought { get; protected set; }
       
        public Buyer(decimal startBalance, string name) : base(startBalance, name) {
            _unSubscribe = Market.GetInstance().Subscribe(this);
        }

        public void OnNext(Seller seller, IItem item) {
         
            if (!IsInteresting(item)) return;
            
            lock (this)
            {
            if (!seller.BuyItem(item)) return;
                
                Thread.Sleep(new ThreadSafeRandom().Next(100, 500));
                BuyItem(item, seller);
            }

            if (Balance == 0) {
                _unSubscribe();
            }
            
        }

        public void BuyItem(IItem item, Seller seller)
        {
            const string tabs = "                    ";
            Console.WriteLine($"{tabs}{Name} bought {seller.Name}'s #{item.SellerItemId} for ${String.Format("{0:0.00}", item.Price)}");
            Belongings.Add(item);
            NumberOfItemsBought++;
            Balance -= item.Price;
            AmountUsed += item.Price;
            
        }

        public bool IsInteresting(IItem item)
        {
            /*decimal listingPrice = item.Category.Price;*/
            
            lock (this) {
                if (Balance < item.Price) return false;
                
                //if (!(item.Price < listingPrice)) return false;
                
            }
            return true;
        }

        public override bool Equals(object obj) {
            return obj is Buyer && ((Buyer) obj).Id.Equals(Id);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }

        public override Report GenerateReport()
        {
            return new BuyerReport(this, NumberOfItemsBought, AmountUsed);
        }
    }
}