using System.Collections.Generic;
using System.Linq;
using LottasFleaMarket.Models;
using LottasFleaMarket.Models.Factories;
using Xunit;

namespace LottasFleaMarketTest.Models
{
    public class SellerTest
    {
        [Fact]
        public void shouldHaveOneLessItemAfterSellingOne()
        {
            
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().WithNumberOfBelongings(10).Build();
            var Buyer = PersonFactory.BuyerBuilder().WithStartBalance(100).Build();

            var items = seller.Belongings.ToList();
            seller.SellItems(1);
            
            items.ForEach(i => seller.BuyItem(i));
            
            Assert.Equal(9, seller.Belongings.Count);
        }
        
        [Fact]
        public void shouldHaveThreeLessItemAfterSellingThree()
        {
            
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().WithNumberOfBelongings(10).Build();
            var Buyer = PersonFactory.BuyerBuilder().WithStartBalance(100).Build();

            var items = seller.Belongings.ToList();
            seller.SellItems(3);
            
            items.ForEach(i => seller.BuyItem(i));
            
            Assert.Equal(7, seller.Belongings.Count);
        }
        
        [Fact]
        public void cannotSellBecauseIsNotListedForSale()
        {
            
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().WithNumberOfBelongings(10).Build();
            var Buyer = PersonFactory.BuyerBuilder().WithStartBalance(100).Build();

            var items = seller.Belongings.ToList();
            
            items.ForEach(i => seller.BuyItem(i));
            
            Assert.Equal(10, seller.Belongings.Count);
        }
        
        [Fact]
        public void cannotSellAnItemTwice()
        {
            
            var seller = PersonFactory.SellerBuilder(false).WithRandomBalance().WithNumberOfBelongings(10).Build();
            var Buyer = PersonFactory.BuyerBuilder().WithStartBalance(100).Build();

            var items = seller.Belongings.ToList();

            seller.SellItems(1);
            
            items.ForEach(i => seller.BuyItem(i));
            items.ForEach(i => seller.BuyItem(i));
            
            Assert.Equal(9, seller.Belongings.Count);
        }
        
        [Fact]
        public void noSellersIsEqualToEachOther()
        {
            var listOfSellers = new List<Seller>();

            for (int i = 0; i < 100; i++)
            {
                listOfSellers.Add(PersonFactory.SellerBuilder(false).WithNumberOfBelongings(0).WithStartBalance(100).Build());    
            }

            var num = listOfSellers.Distinct().Count();
            
            Assert.Equal(100, num);
        }
        
        [Fact]
        public void SellersisDuplicate()
        {
            var listOfSellers = new List<Seller>();
            Seller seller = PersonFactory.SellerBuilder(false).WithNumberOfBelongings(0).WithStartBalance(100).Build();

            for (int i = 0; i < 100; i++)
            {
                listOfSellers.Add(seller);    
            }

            var num = listOfSellers.Distinct().Count();
            
            Assert.Equal(1, num);
        }
    }
}