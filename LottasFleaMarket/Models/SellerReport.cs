using LottasFleaMarket.Models.Enums;

namespace LottasFleaMarket.Models
{
    public class SellerReport : Report
    {

        public decimal InitialItemValue;
        public int ItemSold;
        public decimal MoneyMade;

        public SellerReport(Person owner, int itemSold, decimal moneyMade, decimal initialItemValue) : base(owner)
        {
            ItemSold = itemSold;
            MoneyMade = moneyMade;
            InitialItemValue = initialItemValue;
        }
    }
}