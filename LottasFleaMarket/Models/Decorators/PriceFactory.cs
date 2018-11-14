using System;
using System.Collections.Generic;
using LottasFleaMarket.Models;
using LottasFleaMarket.Models.Enums;

namespace LottasFleaMarket.Utils
{
    public abstract class PriceFactory
    {
        
        public decimal GeneratePriceBasedOnSeller(Seller seller)
        {              
            var priceFactorBecauseOfPersonIsSmart = seller.IsSmart ? (decimal) 1.10 : (decimal) 0.3;
            var priceFactorBecauseOfPersonIsGreedy = seller.IsGreedy ? (decimal) 1.50 : (decimal) 0.3;

            return priceFactorBecauseOfPersonIsGreedy * priceFactorBecauseOfPersonIsGreedy;
        }

        public decimal GeneratePriceBasedOnItemCategory(Category category)
        {
            return category.Price;
        }

        public decimal getCalculatedPrice(Category category, Seller seller)
        {
            return GeneratePriceBasedOnSeller(seller) * GeneratePriceBasedOnItemCategory(category);
        }
    }
}