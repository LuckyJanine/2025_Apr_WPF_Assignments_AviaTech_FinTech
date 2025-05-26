using FlowLedger.Enums;

namespace FlowLedger.Models
{
    internal record TransactionCategory
    {
        public string Name { get; init; }
        public TransactionType TransactionType { get; init; }

        public TransactionCategory(string name, TransactionType type)
        {
            Name = name;
            TransactionType = type;
        }
    }
}
