using System;
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
        public void ShouldFindItemInteresting()
        {
            var item = new Item(1);
            var buyer = PersonFactory.BuyerBuilder().WithStartBalance(400).Build();
            var collectorsDecoratorItem = new CollectorsDecorator(item);

            Assert.True(buyer.IsInteresting(collectorsDecoratorItem));

        }

        [Fact]
        public void ShouldHavePriceOfBoughtItemLessInBalance()
        {
            var startBalance = 100;
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().Build();
            var buyer = PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(startBalance).Build();

            IItem item = new Item(1);
            var itemPrice = item.Price;
            
            buyer.BuyItem(item, seller);
            
            Assert.Equal(startBalance-itemPrice, buyer.Balance);
        }
        
        [Fact]
        public void CannotBuyItemThatCostsMoreThanBalance()
        {
            IItem item = new Item(1);
            var startBalance = (int)item.Price - 1;
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().Build();
            var buyer = PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(startBalance).Build();
            
            if(buyer.IsInteresting(item))  buyer.BuyItem(item, seller);
            
            Assert.Equal(0, buyer.NumberOfItemsBought);
        }
        
        [Fact]
        public void CanBuyItemThatCostsSameAsBalance()
        {
            IItem item = new Item(1);
            var startBalance = (int)item.Price;
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().Build();
            var buyer = PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(startBalance).Build();
            
            if(buyer.IsInteresting(item))  buyer.BuyItem(item, seller);
            
            Assert.Equal(1, buyer.NumberOfItemsBought);
        }
        
        [Fact]
        public void ShouldFindItemNotInterestingBecauseOfLackOfMoney()
        {
            var item = new Item(1);
            var buyer = PersonFactory.BuyerBuilder().WithStartBalance(10).Build();
            var collectorsDecoratorItem = new CollectorsDecorator(item);

            Assert.False(buyer.IsInteresting(collectorsDecoratorItem));

        }
        
        [Fact]
        public void ShouldHaveOneMoreItemAfterBuying()
        {
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().Build();
            var buyer = PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(100).Build();

            IItem item = new Item(1);
            
            buyer.BuyItem(item, seller);
            
            Assert.Equal(1, buyer.Belongings.Count);
        }
        
        [Fact]
        public void NoBuyersIsEqualToEachOther()
        {
            var listOfBuyers = new List<Buyer>();

            for (var i = 0; i < 100; i++)
            {
                listOfBuyers.Add(PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(100).Build());    
            }

            var num = listOfBuyers.Distinct().Count();
            
            Assert.Equal(100, num);
        }
        
        [Fact]
        public void BuyersIsDuplicate()
        {
            var listOfBuyers = new List<Buyer>();
            var buyer = PersonFactory.BuyerBuilder().WithNumberOfBelongings(0).WithStartBalance(100).Build();

            for (var i = 0; i < 100; i++)
            {
                listOfBuyers.Add(buyer);    
            }

            var num = listOfBuyers.Distinct().Count();
            
            Assert.Equal(1, num);
        }
    }
}