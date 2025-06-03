namespace FlowLedger.Models
{
    internal class AccountStatus
    {
        public decimal CurrentBalance { get; init; }
        public Dictionary<string, MonthTransactions> MonthlyTransactions { get; init; }
        public AccountStatus(decimal balance, Dictionary<string, MonthTransactions> transactions)
        {
            CurrentBalance = balance;
            MonthlyTransactions = transactions;
        }
    }
}
