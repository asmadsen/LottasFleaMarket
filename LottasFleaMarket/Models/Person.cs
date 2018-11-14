using System;
using System.Collections.Generic;

namespace LottasFleaMarket.Models {
    public abstract class Person {
        public readonly Guid Id;
        public readonly string Name;
        public ISet<Item> Belongings;
        public int NumberOfBelongings => Belongings.Count;
        protected decimal Money;

        protected Person(string name, decimal money) {
            Id = Guid.NewGuid();
            Name = name;
            Belongings = new HashSet<Item>();
            Money = money;
        }
    }
}