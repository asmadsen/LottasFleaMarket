using Moq;
using Xunit;

using LottasFleaMarket.Models;
using LottasFleaMarket.Decorators;
using LottasFleaMarket.Interfaces.Decorators;

namespace LottasFleaMarketTest.Models
{
    public class BuyerTest
    {
        [Fact]
        public void shouldFindItemIntersting()
        {
            Item item = new Item(1);
            var Buyer = new Buyer(400);
            var CollectorsDecoratorItem = new CollectorsDecorator(item);

            var i = (IItem) CollectorsDecoratorItem;

            Assert.Equal(Buyer.IsInteresting(i), true);

        }
        
        [Fact]
        public void shouldFindItemNOTInterestingBecauseOfLackOfMoney()
        {
            Item item = new Item(1);
            var Buyer = new Buyer(1);
            var CollectorsDecoratorItem = new CollectorsDecorator(item);

            var i = (IItem) CollectorsDecoratorItem;

            var isInt = Buyer.IsInteresting(i);

            Assert.Equal(Buyer.IsInteresting(i), false);

        }
        
        [Fact]
        public void shouldFindItemInterstingBecauseNOTSmart()
        {
            Item item = new Item(1);
            var Buyer = new Buyer(100);
            Buyer.IsSmart = false;
            
            var CollectorsDecoratorItem = new CollectorsDecorator(item);

            var i = (IItem) CollectorsDecoratorItem;

            Assert.Equal(Buyer.IsInteresting(i), true);
        }
        
        [Fact]
        public void shouldFindItemNOTInterstingBecauseNOTSmartButNoMoney()
        {
            Item item = new Item(1);
            var Buyer = new Buyer(0);
            Buyer.IsSmart = false;
            
            var CollectorsDecoratorItem = new CollectorsDecorator(item);

            var i = (IItem) CollectorsDecoratorItem;

            Assert.Equal(Buyer.IsInteresting(i), false);
        }
        
        [Fact]
        public void shouldFindItemInterstingBecausePriceIsUnderListingPrice()
        {
            Item item = new Item(1);
            var CollectorsDecoratorItem = new CollectorsDecorator(item);
            decimal listingPrice = CollectorsDecoratorItem.Category.Price;
            decimal price = CollectorsDecoratorItem.Price;
            
            var Buyer = new Buyer(100);
            Buyer.IsSmart = true;
       
            var i = (IItem) CollectorsDecoratorItem;

            Assert.Equal(Buyer.IsInteresting(i), true);
        }
    }
}