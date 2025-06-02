namespace FlowLedger.Models
{
    internal class AccountInfo
    {
        public decimal CurrentBalance { get; init; }
        public Dictionary<string, MonthTransactions> MonthlyTransactions { get; init; }
        public AccountInfo(decimal balance, Dictionary<string, MonthTransactions> transactions)
        {
            CurrentBalance = balance;
            MonthlyTransactions = transactions;
        }
    }
}
