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
            
            var numberOfBuyers = 100;
            var numberOfSellers = 5;
            var numberOfSellerBelongings = 200;

            var sellers = new List<Seller>();
            var buyers = new List<Buyer>();

            Console.WriteLine("Buyers:");
            
            for (var i = 0; i < numberOfBuyers; i++) {
                var buyer = new Buyer(1000);
                buyers.Add(buyer);
                Console.WriteLine($"{buyer.Name} is smart= {buyer.IsSmart}");
            }

            Console.WriteLine("");
            Console.WriteLine("");
            
            Console.WriteLine("Sellers:");
            
            for (var i = 0; i < numberOfSellers; i++)
            {
                Seller seller = new Seller(numberOfSellerBelongings);
                sellers.Add(seller);
                Console.WriteLine($"{seller.Name} is smart= {seller.IsSmart}");
            }
            
            Console.WriteLine("");
            Console.WriteLine("");

            StartLoppemarked(sellers);

            printStatistics(sellers, buyers);
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
            Console.WriteLine("-----------------");
            sellers.ForEach(s =>
            {
                Console.WriteLine("{0, -9} has {1, -1} items left and sold items for ${2, -4}.", s.Name ,s.NumberOfBelongings, s.AmountSoldFor);
                
            });
            
            Console.WriteLine();
            
            Console.WriteLine("Buyers:");
            Console.WriteLine("-----------------");
            buyers.ForEach(b =>
            {
                Console.WriteLine("{0, -9} bought {1,-3} items for the total value of {2,-4} and has {3,-2} left in the bank", b.Name, b.NumberOfBelongings, b.AmountUsed, b.Saldo);
                
            });
            
        }

        

    }
}