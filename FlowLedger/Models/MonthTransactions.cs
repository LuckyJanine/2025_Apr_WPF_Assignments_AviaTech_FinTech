namespace FlowLedger.Models
{
    internal class MonthTransactions
    {
        private List<TransactionDetail> _transactions = new List<TransactionDetail>();

        private decimal _totalRevenue;
        private decimal _totalExpense;
        private decimal _monthlyNet;

        public decimal TotalRevenue => _totalRevenue;
        public decimal TotalExpense => _totalExpense;
        public decimal MonthlyNet => _monthlyNet;

        public void Add(TransactionDetail transaction)
        {
            _transactions.Add(transaction);
            UpdateOverview(transaction);
        }

        private void UpdateOverview(TransactionDetail transaction)
        {
            //_totalRevenue = _transactions
            //    .Where(t => t.Category.TransactionType == Enums.TransactionType.Revenue)
            //    .Sum(t => t.Amount);
            if (transaction.Category.TransactionType == Enums.TransactionType.Revenue)
            {
                _totalRevenue += transaction.Amount;
            } else if (transaction.Category.TransactionType == Enums.TransactionType.Spend)
            {
                _totalExpense += transaction.Amount;
            } else
            {
                throw new InvalidOperationException();
            }
            _monthlyNet = _totalRevenue - _totalExpense;
        }
    }
}
