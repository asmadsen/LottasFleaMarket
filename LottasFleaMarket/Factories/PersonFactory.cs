using System.Collections.Generic;
using Bogus;

namespace LottasFleaMarket.Utils
{
    public class PersonUtil
    {
        private static Faker _faker = new Faker();
        private static ISet<string> _names = new HashSet<string>();
        
        private static void UniqueName(Person p) {
            string name;
            do {
                name = _faker.Name.FirstName();
            } while (_names.Contains(name));

            _names.Add(name);
            p.FirstName = name;
        }
    }
}