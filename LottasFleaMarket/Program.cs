using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using Bogus;
using LottasFleaMarket.Models;
using LottasFleaMarket.Models.Enums;
using LottasFleaMarket.Models.Factories;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket {
    public class Program {
   
        static void Main(string[] args) {
            
            var numberOfBuyers = 50;
            var numberOfSellers = 50;
            var numberOfSellerBelongings = 20;
            var startBalanceBuyers = 10000;

            var sellers = new List<Seller>();
            var buyers = new List<Buyer>();

            PopulateSimulationWithSellersAndBuyers(numberOfBuyers, buyers, numberOfSellers, numberOfSellerBelongings, sellers, startBalanceBuyers);

            StartLoppemarked(sellers);

            printStatistics(sellers, buyers);
        }

        public static void PopulateSimulationWithSellersAndBuyers(int numberOfBuyers, List<Buyer> buyers, int numberOfSellers,
            int numberOfSellerBelongings, List<Seller> sellers, int startBalanceBuyers)
        {
            Console.WriteLine("Buyers:");
            Console.WriteLine();
            for (var i = 0; i < numberOfBuyers; i++)
            {

                buyers.Add(PersonFactory.BuyerBuilder()
                    .WithStartBalance(startBalanceBuyers)
                    .Build());
            }
            
            Console.WriteLine();
            Console.WriteLine();
            
            Console.WriteLine("Sellers:");
            Console.WriteLine();

            for (var i = 0; i < numberOfSellers; i++)
            {
                sellers.Add(PersonFactory.SellerBuilder(false)
                    .WithNumberOfBelongings(numberOfSellerBelongings)
                    .Build());
            }
        }

        public static void StartLoppemarked(List<Seller> sellers)
        {
            var loopCount = 1;
            while (sellers.Any(seller => seller.HasMoreNotListedItems))
            {
                var waitHandles = new List<WaitHandle>();
                foreach (var seller in sellers.FindAll(seller => seller.HasMoreNotListedItems).OrderBy(s => Guid.NewGuid()))
                {
                    var waitHandle = new ManualResetEvent(false);
                    waitHandles.Add(waitHandle);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(Object state)
                    {
                        ManualResetEvent are = (ManualResetEvent) state;
                        var threadSafeRandom = new ThreadSafeRandom();
                        seller.SellItems(threadSafeRandom.Next(1, 50));
                        are.Set();
                    }), waitHandle);
                }

                WaitHandle.WaitAll(waitHandles.ToArray());
                Console.WriteLine($"All sellers have now sold it's #{loopCount++} batch");
            }
            
            Console.WriteLine("Everything is sold");
        }

        private static void printStatistics(List<Seller> sellers, List<Buyer> buyers)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Statistic:");
            
            Console.WriteLine("Number of sellers: {0, -3}", sellers.Count);
            Console.WriteLine("Number of buyers:  {0, -3}", buyers.Count);
                
            var sellerReports = new List<SellerReport>();
            var buyerReports = new List<BuyerReport>();

            buyers.ForEach(b => buyerReports.Add(b.GenerateReport() as BuyerReport));
            sellers.ForEach(s => sellerReports.Add(s.GenerateReport() as SellerReport));
            
            Console.WriteLine();
            Console.WriteLine();
            
            Console.WriteLine("Number of seller reports: {0, -3}", sellerReports.Count);
            Console.WriteLine("Number of buyer reports:  {0, -3}", buyerReports.Count);
            
            Console.WriteLine();
            Console.WriteLine();
            
            Console.WriteLine("Sellers:");
            Console.WriteLine("");
            Console.WriteLine("{0, -10} {1, -8} {2, -13} {3, -13} {4, -13} ", "Name", "Items left", "Items Sold", "Initial Value", "Total income");
            Console.WriteLine("--------------------------------------------------------------");
            sellerReports.ForEach(s =>
            {
                Console.WriteLine("{0, -14} {1, -10} {2, -13} ${3, -10} ${4 ,-13}", s.Owner.Name , s.Owner.NumberOfBelongings, s.ItemSold, s.InitialItemValue, s.MoneyMade );
                
            });
            
            Console.WriteLine();
            
            Console.WriteLine("Buyers:");
            Console.WriteLine();
            Console.WriteLine("{0, -10} {1, -13} {2, -13} {3, -13} {4, -13} ", "Name","Items Bought", "Initial Saldo", "Money spent", "Bank Saldo");
            Console.WriteLine("--------------------------------------------------------------");
            buyerReports.ForEach(b =>
            {
                Console.WriteLine("{0, -15} {1,-12} {2, -10} ${3, -10} ${4, -13} ", b.Owner.Name, b.StartBalance, b.ItemsBougth, b.MoneySpent, b.EndBalance);
                
            });
            
        }
    }
}