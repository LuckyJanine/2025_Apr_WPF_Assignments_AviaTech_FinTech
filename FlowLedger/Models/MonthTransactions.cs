namespace FlowLedger.Models
{
    internal class MonthTransactions
    {
        public decimal TotalRevenue;
        public decimal TotalExpense;
        public decimal MonthlyNet;
        public bool IsDeficit;
        public List<TransactionDetail> Transactions = new List<TransactionDetail>();

        public void Add(TransactionDetail transaction)
        {
            Transactions.Add(transaction);
            UpdateOverview(transaction);
        }

        private void UpdateOverview(TransactionDetail transaction)
        {
            //_totalRevenue = _transactions
            //    .Where(t => t.Category.TransactionType == Enums.TransactionType.Revenue)
            //    .Sum(t => t.Amount);
            if (transaction.Category.TransactionType == Enums.TransactionType.Revenue)
            {
                TotalRevenue += transaction.Amount;
            } else if (transaction.Category.TransactionType == Enums.TransactionType.Spend)
            {
                TotalExpense += transaction.Amount;
            } else
            {
                throw new InvalidOperationException();
            }
            MonthlyNet = TotalRevenue - TotalExpense;
            if (MonthlyNet < 0)
            {
                IsDeficit = true;
            }
        }
    }
}
