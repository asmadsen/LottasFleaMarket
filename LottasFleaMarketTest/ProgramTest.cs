using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LottasFleaMarket;
using LottasFleaMarket.Models;
using Xunit;

namespace LottasFleaMarketTest

{
    public class ProgramTest
    {

        [Fact]
        public void noPersonHasTheSameName()
        {
            var numberOfBuyers = 1500;
            var numberOfSellers = 1500;
            var numberOfSellerBelongings = 20;
            var startBalanceBuyers = 100;
            
            var sellers = new List<Seller>();
            var buyers = new List<Buyer>();
            
            Program.PopulateSimulationWithSellersAndBuyers(
                numberOfBuyers, buyers, numberOfSellers, numberOfSellerBelongings, sellers, startBalanceBuyers);

            var numberOfPersonWithDistinct =
                (from seller in sellers select seller.Name).Distinct().Count()
                +
                (from buyer in buyers select buyer.Name).Distinct().Count();
            
            Assert.Equal(numberOfBuyers+numberOfSellers, numberOfPersonWithDistinct);
        }
        
        
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
        
        [Fact]
        public void noBuyersHasNegativeBalance()
        {
            var numberOfBuyers = 10;
            var numberOfSellers = 30;
            var numberOfSellerBelongings = 20;
            var startBalanceBuyers = 100;
            
            var sellers = new List<Seller>();
            var buyers = new List<Buyer>();
            
            Program.PopulateSimulationWithSellersAndBuyers(
                numberOfBuyers, buyers, numberOfSellers, numberOfSellerBelongings, sellers, startBalanceBuyers);
            Program.StartLoppemarked(sellers);

            var numberOfBuyersThatIsInNegativeBalance = (from buyer in buyers where buyer.Balance < 0 select buyer).Count();

            Assert.Equal(0, numberOfBuyersThatIsInNegativeBalance);

        }
    }
}