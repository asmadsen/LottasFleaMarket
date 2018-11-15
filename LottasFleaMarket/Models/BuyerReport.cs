using LottasFleaMarket.Models.Enums;

namespace LottasFleaMarket.Models
{
    public class BuyerReport : Report
    {
        public int ItemsBougth;
        public decimal MoneySpent;

        public BuyerReport(Person owner, int itemsBougth, decimal moneySpent) 
            : base(owner)
        {
            ItemsBougth = itemsBougth;
            MoneySpent = moneySpent;
        }
    }
}