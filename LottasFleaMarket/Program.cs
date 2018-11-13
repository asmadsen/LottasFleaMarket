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
        
        
        static void Main(string[] args)
        {

            int numberOfBuyers = 10;
            int numberOfSellers = 10;
            int numberOfSellerBelongings = 10;

            List<Seller> sellers = new List<Seller>();

            for (int i = 0; i < numberOfBuyers; i++)
            {
                var buyer1 = new Buyer(UniqueName(), 400);
            }

            for (int i = 0; i < numberOfSellers; i++)
            {
                sellers.Add(new Seller(UniqueName(), numberOfSellerBelongings));

            };
            


             
            
            //sellers.ForEach(s => s.SellItems(10));
            
            

            while (sellers.Any(seller => seller.HasMoreItems)) {
                foreach (var seller in sellers.FindAll(seller => seller.HasMoreItems).OrderBy(s => Guid.NewGuid())) {
                    var threadSafeRandom = new ThreadSafeRandom();
                    seller.SellItems(threadSafeRandom.Next(1, 10));
                    
                    /*new Thread(() => {
                        
                    }).Start();*/
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