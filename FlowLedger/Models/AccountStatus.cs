namespace FlowLedger.Models
{
    internal class AccountStatus
    {
        public decimal CurrentBalance { get; init; }
        public Dictionary<YearMonth, MonthTransactions> MonthlyTransactions { get; init; }
        public AccountStatus(decimal balance, Dictionary<YearMonth, MonthTransactions> transactions)
        {
            CurrentBalance = balance;
            MonthlyTransactions = transactions;
        }
    }
}
