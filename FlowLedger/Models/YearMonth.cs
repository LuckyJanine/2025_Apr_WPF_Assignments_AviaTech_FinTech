using FlowLedger.Enums;

namespace FlowLedger.Models
{
    internal struct YearMonth : IEquatable<YearMonth>
    {
        public int Year { get; }
        public int Month { get; }

        public YearMonth(int year, int month)
        {
            Year = year;
            Month = month;
        }
        public bool Equals(YearMonth other)
        {
            return Year == other.Year && Month == other.Month;
        }

        public override string ToString()
        {
            var monthOutput = (Month)Month;
            return $"{monthOutput}, {Year}";
        }
    }
}
