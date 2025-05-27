using System.ComponentModel;
using System.Globalization;

namespace FlowLedger.ViewModels
{
    internal class TransactionViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string Error => null;

        private DateTime _creationDate;

        private string _txtIncomeAmount;
        private string _txtExpenseAmount;
        private bool _isAmountConvertable;
        private bool _isAmountValid;
        private decimal _amount;

        private string _currency;

        private string _description;

        private bool _canProceed;

        public TransactionViewModel()
        {
            CurrencyCode = "SEK";
            TransactionDate = DateTime.Now;
        }

        public DateTime TransactionDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                OnPropertyChanged(nameof(TransactionDate));
            }
        }

        public string TxtIncomeAmount
        {
            get => _txtIncomeAmount;
            set
            {
                _txtIncomeAmount = value;
                OnPropertyChanged(nameof(TxtIncomeAmount));
                _isAmountConvertable = false;
                _isAmountValid = false;
                decimal? amount = ConvertTransactionAmount(value);
                if (amount != null)
                {
                    ValidateTransactionAmount(amount.Value);
                }
            }
        }

        public string TxtExpenseAmount
        {
            get => _txtExpenseAmount;
            set
            {
                _txtExpenseAmount = value;
                OnPropertyChanged(nameof(TxtExpenseAmount));
                _isAmountConvertable = false;
                _isAmountValid = false;
                decimal? amount = ConvertTransactionAmount(value);
                if (amount != null)
                {
                    ValidateTransactionAmount(amount.Value);
                }
            }
        }

        public decimal Amount
        {
            get => _amount;
        }

        public string CurrencyCode
        {
            get => _currency;
            set 
            {
                _currency = value;
                OnPropertyChanged(nameof(CurrencyCode));
            }
        }

        public string Description
        {
            get => _description;
            set 
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public bool CanProceed
        {
            get => _canProceed;
            set
            {
                _canProceed = value;
                OnPropertyChanged(nameof(CanProceed));
            }
        }

        private decimal? ConvertTransactionAmount(string transactionAmount)
        {
            transactionAmount = transactionAmount.Trim();
            transactionAmount = transactionAmount.Replace(',', '.');
            if (decimal.TryParse(transactionAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
            {
                _isAmountConvertable = true;
                return amount;
            }
            return null;
        }

        private void ValidateTransactionAmount(decimal transAmount)
        {
            if (transAmount <= 0 || transAmount > 200000)
            {
                _isAmountValid = false;
            } else
            {
                _isAmountValid = true;
                _amount = transAmount;
            }
        }

        public string this[string columnName]
        {
            get 
            {
                switch (columnName)
                {
                    case nameof(TxtIncomeAmount):
                    case nameof(TxtExpenseAmount):
                        if (!_isAmountConvertable)
                        {
                            return "value not convertable.";
                        }
                        if (!_isAmountValid)
                        {
                            return "0 < Amount <= 200000.";
                        }
                        break;
                }
                return null;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
