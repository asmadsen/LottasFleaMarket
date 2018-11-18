using System;
using System.Collections.Generic;
using LottasFleaMarket.Interfaces.Decorators;
using LottasFleaMarket.Models.Enums;
using LottasFleaMarket.Utils;

namespace LottasFleaMarket.Models
{

    public abstract class Person
    {
        public readonly Guid Id;
        public readonly string Name;
        public ISet<IItem> Belongings = new HashSet<IItem>();
        public int NumberOfBelongings;
        public decimal Balance;
        public decimal InitialBalance;

        protected Person(decimal balance, string name, ISet<IItem> belongings = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Balance = balance;
            InitialBalance = balance;
        
            if (belongings != null)
            {
                Belongings = belongings;
            }
        }
        public abstract Report GenerateReport();
    }
}