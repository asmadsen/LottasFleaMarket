using LottasFleaMarket.Models.Enums;

namespace LottasFleaMarket.Models
{
    public class BuyerReport : Report
    {
        public int ItemsBought;
        public decimal MoneySpent;

        public BuyerReport(Person owner, int itemsBought, decimal moneySpent) 
            : base(owner)
        {
            ItemsBought = itemsBought;
            MoneySpent = moneySpent;
        }
    }
}