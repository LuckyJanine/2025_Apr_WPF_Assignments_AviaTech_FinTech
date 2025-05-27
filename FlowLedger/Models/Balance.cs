namespace FlowLedger.Models
{
    internal record Balance
    {
        public decimal InitialBalance { get; init; }
        public string CurrencyCode {  get; init; }

        public decimal CurrentBalance { get; private set; }

        public Balance(decimal initialBalance, string currency)
        {
            InitialBalance = initialBalance;
            CurrencyCode = currency;
            CurrentBalance = initialBalance;
        }

        public void ConfirmTransaction(decimal amount, string currency)
        {
            if (currency != CurrencyCode)
            {
                throw new InvalidOperationException("Currency mismatch.");
            }
            CurrentBalance += amount;
            //return this with { CurrentBalance = CurrentBalance + amount };
        }
    }
}
