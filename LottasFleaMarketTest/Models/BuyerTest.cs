using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

using LottasFleaMarket.Models;
using LottasFleaMarket.Decorators;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Models.Factories;

namespace LottasFleaMarketTest.Models
{
    public class BuyerTest
    {
        [Fact]
        public void shouldFindItemIntersting()
        {
            Item item = new Item(1);
            var Buyer = PersonFactory.BuyerBuilder().WithStartBalance(400).Build();
            var CollectorsDecoratorItem = new CollectorsDecorator(item);

            Assert.True(Buyer.IsInteresting(CollectorsDecoratorItem));

        }

        [Fact]
        public void shouldHavePriceOfBougthItemLessInBalance()
        {
            int startBalance = 100;
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().Build();
            var Buyer = PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(startBalance).Build();

            IItem item = new Item(1);
            decimal itemPrice = item.Price;
            
            Buyer.BuyItem(item, seller);
            
            Assert.Equal(startBalance-itemPrice, Buyer.Balance);
        }
        
        [Fact]
        public void cannotBuyItemThatCostsMoreThanBalance()
        {
            IItem item = new Item(1);
            int startBalance = (int)item.Price - 1;
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().Build();
            var Buyer = PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(startBalance).Build();
            
            if(Buyer.IsInteresting(item))  Buyer.BuyItem(item, seller);
            
            Assert.Equal(0, Buyer.NumberOfItemsBought);
        }
        
        [Fact]
        public void canBuyItemThatCostsSameAsBalance()
        {
            IItem item = new Item(1);
            int startBalance = (int)item.Price;
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().Build();
            var Buyer = PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(startBalance).Build();
            
            if(Buyer.IsInteresting(item))  Buyer.BuyItem(item, seller);
            
            Assert.Equal(1, Buyer.NumberOfItemsBought);
        }
        
        [Fact]
        public void shouldFindItemNOTInterestingBecauseOfLackOfMoney()
        {
            Item item = new Item(1);
            var Buyer = PersonFactory.BuyerBuilder().WithStartBalance(10).Build();
            var CollectorsDecoratorItem = new CollectorsDecorator(item);

            Assert.False(Buyer.IsInteresting(CollectorsDecoratorItem));

        }
        
        [Fact]
        public void shouldHaveOneMoreItemAfterBuying()
        {
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().Build();
            var Buyer = PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(100).Build();

            IItem item = new Item(1);
            
            Buyer.BuyItem(item, seller);
            
            Assert.Equal(1, Buyer.Belongings.Count);
        }
        
        [Fact]
        public void noBuyersIsEqualToEachOther()
        {
            var listOfBuyers = new List<Buyer>();

            for (int i = 0; i < 100; i++)
            {
                listOfBuyers.Add(PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(100).Build());    
            }

            var num = listOfBuyers.Distinct().Count();
            
            Assert.Equal(100, num);
        }
        
        [Fact]
        public void BuyersisDuplicate()
        {
            var listOfBuyers = new List<Buyer>();
            Buyer buyer = PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(100).Build();

            for (int i = 0; i < 100; i++)
            {
                listOfBuyers.Add(buyer);    
            }

            var num = listOfBuyers.Distinct().Count();
            
            Assert.Equal(1, num);
        }
    }
}