using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using Bogus;
using LottasFleaMarket.Models;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket {
    class Program {
        private static Faker _faker = new Faker();
        private static ISet<string> _names = new HashSet<string>();

        static void Main(string[] args) {
            var numberOfBuyers = 500;
            var numberOfSellers = 64;
            var numberOfSellerBelongings = 30;

            var sellers = new List<Seller>();


            for (var i = 0; i < numberOfBuyers; i++) {
                var buyer = new Buyer(UniqueName(), 400);
            }

            for (var i = 0; i < numberOfSellers; i++) {
                sellers.Add(new Seller(UniqueName(), numberOfSellerBelongings));
            }

            //sellers.ForEach(s => s.SellItems(10));

            var loopCount = 1;
            while (sellers.Any(seller => seller.HasMoreItems)) {
                var waitHandles = new List<WaitHandle>();
                foreach (var seller in sellers.FindAll(seller => seller.HasMoreItems).OrderBy(s => Guid.NewGuid())) {
                    var waitHandle = new ManualResetEvent(false);
                    waitHandles.Add(waitHandle);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(Object state) {
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

        private static string UniqueName() {
            string name;
            do {
                name = _faker.Name.FirstName();
            } while (_names.Contains(name));

            _names.Add(name);
            return name;
        }
    }
}