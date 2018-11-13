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
            
            var buyer1 = new Buyer(UniqueName(), 400);
            var buyer2 = new Buyer(UniqueName(), 400);
            var buyer3 = new Buyer(UniqueName(), 400);
            var buyer4 = new Buyer(UniqueName(), 400);
            var buyer5 = new Buyer(UniqueName(), 400);

            var sellers = new List<Seller> {
                new Seller(UniqueName()),
                new Seller(UniqueName()),
                new Seller(UniqueName()),
                new Seller(UniqueName()),
                new Seller(UniqueName())
            };

            while (sellers.Any(seller => seller.HasMoreItems)) {
                foreach (var seller in sellers.FindAll(seller => seller.HasMoreItems).OrderBy(s => Guid.NewGuid())) {
                    new Thread(() => {
                        var threadSafeRandom = new ThreadSafeRandom();
                        seller.SellItems(threadSafeRandom.Next(1, 10));
                    }).Start();
                }
            }
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