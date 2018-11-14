using System;
using System.Collections.Generic;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket.Models {
    public abstract class Person {
        public readonly Guid Id;
        public readonly string Name;
        public ISet<IItem> ItemsNotYetListedForSale;
        public int NumberOfBelongings => ItemsNotYetListedForSale.Count;
        public decimal Saldo;
        public bool IsSmart;
        public bool IsGreedy;

        protected Person(decimal saldo) {
            Id = Guid.NewGuid();
            Name = PersonGeneratorUtil.UniqueName();
            IsSmart = PersonGeneratorUtil.IsSmart();
            IsGreedy = PersonGeneratorUtil.IsGreedy();
            ItemsNotYetListedForSale = new HashSet<IItem>();
            Saldo = saldo;
        }
    }
}