using System.Collections.Generic;
using Bogus;
using LottasFleaMarket.Decorators;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket.Models.Factories
{
    public class PersonFactory
    {
        public static PersonBuilder<Seller> SellerBuilder(bool generateSubSellers)
        {
            return new PersonBuilder<Seller>(0, 0, generateSubSellers);
        }
        
        public static PersonBuilder<Buyer> BuyerBuilder()
        {
            return new PersonBuilder<Buyer>(0, 0);
        }

        public class PersonBuilder<T> where T : Person
        {
            private bool _generateSubSellers;
            private static Faker _faker = new Faker();
            private static ISet<string> _usedNames = new HashSet<string>();
            private int _numberOfBelongingsToGenerate;
            private decimal _decimalStartBalance;
            private ThreadSafeRandom _random = new ThreadSafeRandom();

            internal PersonBuilder(int numberOfBelongingsToGenerate, decimal decimalStartBalance, bool generateSubSellers = false)
            {
                _generateSubSellers = generateSubSellers;
                _numberOfBelongingsToGenerate = numberOfBelongingsToGenerate;
                _decimalStartBalance = decimalStartBalance;
            }
    
            
            
            public PersonBuilder<T> WithRandomBalance()
            {
                _decimalStartBalance = _random.Next(50, 1000);
                return this;
            }
            
            public PersonBuilder<T> WithStartBalance(decimal balance)
            {
                _decimalStartBalance = balance;
                return this;
            }
            
            public PersonBuilder<T> WithRandomOfBelongings()
            {
                _numberOfBelongingsToGenerate = _random.Next(10, 100);
                return this;
            }
            
            public PersonBuilder<T> WithNumberOfBelongings(int numberOfBelongings)
            {
                _numberOfBelongingsToGenerate = numberOfBelongings;
                return this;
            }
    
            public T Build()
            {
                if (typeof(T) == typeof(Seller))
                {
                    return new Seller(MakeBelongings(_numberOfBelongingsToGenerate), _decimalStartBalance, GenerateUniqueName()) as T;
                }
                return new Buyer(_decimalStartBalance, GenerateUniqueName()) as T;
            }
            
            private string GenerateUniqueName()
            {
                
                lock (_usedNames)
                {
                    string name;
                    do {
                        name = _faker.Name.FirstName();
                    } while (_usedNames.Contains(name));
    
                    _usedNames.Add(name);
                    return name;
                }
            }
    
            private ISet<IItem> MakeBelongings(int numberOfBelongingsToGenerate)
            {
                ISet<IItem> set = new HashSet<IItem>();
                
                for (var i = 0; i < numberOfBelongingsToGenerate; i++)
                {
                    IItem item = new Item(i+1);
                    var rand = _random.Next(0, 100);
                    
                    {
                        switch (rand)
                        {
                            case int n when (rand >= 85):
                                item = new CollectorsDecorator(item);
                                break;
                            case int n when (84 > rand):
                                item = new BadConditionDecorator(item);
                                break;
                        }
                    }
                    set.Add(item);
                }
                return set;
            }
        }
    }
}