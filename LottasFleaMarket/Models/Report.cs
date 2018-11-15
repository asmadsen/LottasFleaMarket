namespace LottasFleaMarket.Models.Enums
{
    public abstract class Report
    {
        protected Report(Person owner)
        {
            Owner = owner;
            EndBalance = owner.Balance;
            StartBalance = owner.InitialBalance;
        }

        public Person Owner;
        public decimal EndBalance;
        public decimal StartBalance;

    }
}