namespace FlowLedger.Models
{
    internal record TransactionDetail
    {
        public Guid Id { get; init; }
        public DateTime? CreationDate { get; init; }
        public decimal Amount { get; init; }
        public string Currency { get; init; }

        public TransactionCategory Category { get; init; }
        public string Description { get; init; }

        public TransactionDetail(decimal amount, string currency,
            TransactionCategory categroy, string description,
            DateTime? transactionDate = null)
        {
            Id = Guid.NewGuid();
            Amount = amount;
            Currency = currency;
            Category = categroy;
            Description = description;
            CreationDate = transactionDate;
            if (transactionDate != null)
            {
                CreationDate = transactionDate.Value;
            } else
            {
                CreationDate = DateTime.UtcNow;
            }
        }
    }
}
