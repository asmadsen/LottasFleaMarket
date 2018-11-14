using System;
using System.Collections.Generic;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket.Models {
    public abstract class Person  : PersonFactory {
        public readonly Guid Id;
        public readonly string Name;
        public ISet<Item> Belongings;
        public int NumberOfBelongings => Belongings.Count;
        protected decimal Money;
        public bool IsSmart;
        public bool IsGreedy;

        protected Person(decimal money) {
            Id = Guid.NewGuid();
            Name = UniqueName();
            IsSmart = IsSmart();
            IsGreedy = IsGreedy();
            Belongings = new HashSet<Item>();
            Money = money;
        }
    }
}