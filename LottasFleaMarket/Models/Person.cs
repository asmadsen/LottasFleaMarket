using System;
using System.Collections.Generic;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket.Models
{

    public abstract class Person
    {
        public readonly Guid Id;
        public readonly string Name;
        public ISet<IItem> Belongings;
        public int NumberOfBelongings => Belongings.Count;
        public decimal Balance;
        public bool IsSmart;
        public bool IsGreedy;

        protected Person(decimal balance)
        {
            Id = Guid.NewGuid();
            Name = PersonGeneratorUtil.UniqueName();
            IsSmart = PersonGeneratorUtil.IsSmart();
            IsGreedy = PersonGeneratorUtil.IsGreedy();
            Belongings = new HashSet<IItem>();
            Balance = balance;
        }
    }
}