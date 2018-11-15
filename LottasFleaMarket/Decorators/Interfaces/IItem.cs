using System;
using LottasFleaMarket.Models.Enums;

namespace LottasFleaMarket.Interfaces.Decorators {
    public interface IItem {
        Guid Id { get; }
        int SellerItemId { get; }
        decimal Price { get; }
        string Description { get; }
        Category Category { get; }
        Condition Condition { get; }
    }
}