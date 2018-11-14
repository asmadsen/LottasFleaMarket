using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using Bogus;
using LottasFleaMarket.Models;
using LottasFleaMarket.Models.Enums;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket {
    class Program {
   
        static void Main(string[] args) {
            
            var numberOfBuyers = 10;
            var numberOfSellers = 10;
            var numberOfSellerBelongings = 20;

            var sellers = new List<Seller>();
            var buyers = new List<Buyer>();

            PopulateSimulationWithSellersAndBuyers(numberOfBuyers, buyers, numberOfSellers, numberOfSellerBelongings, sellers);

            StartLoppemarked(sellers);

            printStatistics(sellers, buyers);
        }

        private static void PopulateSimulationWithSellersAndBuyers(int numberOfBuyers, List<Buyer> buyers, int numberOfSellers,
            int numberOfSellerBelongings, List<Seller> sellers)
        {
            Console.WriteLine("Buyers:");

            for (var i = 0; i < numberOfBuyers; i++)
            {
                var buyer = new Buyer(1000);
                buyers.Add(buyer);
            }

            for (var i = 0; i < numberOfSellers; i++)
            {
                Seller seller = new Seller(numberOfSellerBelongings);
                sellers.Add(seller);
            }
        }

        private static void StartLoppemarked(List<Seller> sellers)
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
            Console.WriteLine();
            
            Console.WriteLine("Sellers:");
            Console.WriteLine("");
            Console.WriteLine("{0, -10} {1, -8} {2, -13} {3, -13} {4, -10}", "Name", "Smart","Items left", "Items Sold", "Total income");
            Console.WriteLine("--------------------------------------------------------------");
            sellers.ForEach(s =>
            {
                Console.WriteLine("{0, -10} {1, -12} {2, -13} {3, -13} ${4, -10}", s.Name , s.IsSmart, s.NumberOfBelongings, s._NumberOfItemsSellerStartWith-s.NumberOfBelongings ,s.AmountSoldFor);
                
            });
            
            Console.WriteLine();
            
            Console.WriteLine("Buyers:");
            Console.WriteLine();
            Console.WriteLine("{0, -10} {1, -8} {2, -13} {3, -13} {4, -10}", "Name", "Smart","Items Bought", "Money spent", "Bank Saldo");
            Console.WriteLine("--------------------------------------------------------------");
            buyers.ForEach(b =>
            {
                Console.WriteLine("{0, -10} {1, -12} {2,-12} ${3, -13} ${4, -10}", b.Name, b.IsSmart, b.NumberOfBelongings, b.AmountUsed, b.Saldo);
                
            });
            
        }
    }
}