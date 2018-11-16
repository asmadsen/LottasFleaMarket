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
        public void shouldFindItemNOTInterestingBecauseOfLackOfMoney()
        {
            Item item = new Item(1);
            var Buyer = PersonFactory.BuyerBuilder().WithStartBalance(10).Build();
            var CollectorsDecoratorItem = new CollectorsDecorator(item);

            Assert.False(Buyer.IsInteresting(CollectorsDecoratorItem));

        }
    }
}