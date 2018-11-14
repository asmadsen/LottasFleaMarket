using System;
using System.Collections.Generic;
using Bogus;

namespace LottasFleaMarket.Utils
{
    public class PersonGeneratorUtil
    {
        private static Faker _faker = new Faker();
        private static ISet<string> _names = new HashSet<string>();
        
        public static string UniqueName() {
            string name;
            do {
                name = _faker.Name.FirstName();
            } while (_names.Contains(name));

            _names.Add(name);
            return name;
        }

        public static bool IsSmart()
        {
            ThreadSafeRandom r = new ThreadSafeRandom();
            int iq = r.Next(0, 100);

            return iq > 85;
        }
        
        public static bool IsGreedy()
        {
            ThreadSafeRandom r = new ThreadSafeRandom();
            int greedyNess = r.Next(0, 100);

            return greedyNess > 70;
        }
    }
}