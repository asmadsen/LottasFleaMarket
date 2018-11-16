using System.Collections.Generic;
using LottasFleaMarket;
using LottasFleaMarket.Models;
using Xunit;

namespace LottasFleaMarketTest

{
    public class ProgramTest
    {
        [Fact]
        public void numberOfBelongingsHasNotChangedAfterDuringSimulation()
        {
            var numberOfBuyers = 50;
            var numberOfSellers = 5;
            var numberOfSellerBelongings = 20;
            var startBalanceBuyers = 10000;
            
            var sellers = new List<Seller>();
            var buyers = new List<Buyer>();
            
            Program.PopulateSimulationWithSellersAndBuyers(
                numberOfBuyers, buyers, numberOfSellers, numberOfSellerBelongings, sellers, startBalanceBuyers);
            Program.StartLoppemarked(sellers);

            var numberOfBelongingsAfterSaleEnd = 0;
            buyers.ForEach(b => numberOfBelongingsAfterSaleEnd += b.Belongings.Count);
            sellers.ForEach(b => numberOfBelongingsAfterSaleEnd += b.Belongings.Count);

            Assert.Equal(numberOfSellerBelongings*numberOfSellers, numberOfBelongingsAfterSaleEnd);

        }
    }
}