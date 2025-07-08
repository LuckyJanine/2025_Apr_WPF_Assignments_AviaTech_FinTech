using FlowLedger.Utils;
using Newtonsoft.Json;

namespace FlowLedger.Models
{
    internal class AccountStatus
    {
        public decimal CurrentBalance { get; init; }

        [JsonConverter(typeof(DictionaryDTOConverter<YearMonth, MonthTransactions>))]
        public Dictionary<YearMonth, MonthTransactions> MonthlyTransactions { get; init; }

        public AccountStatus(decimal balance, Dictionary<YearMonth, MonthTransactions> transactions)
        {
            CurrentBalance = balance;
            MonthlyTransactions = transactions;
        }
    }
}
