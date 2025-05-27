using System.ComponentModel;

namespace FlowLedger.ViewModels
{
    internal class TransactionViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string Error => null;

        private DateTime _creationDate;

        private string _txtAmount;
        private decimal _amount;

        private string _currency;

        private string _description;

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

        public string TxtAmount
        {
            get => _txtAmount;
            set
            {
                _txtAmount = value;
                OnPropertyChanged(nameof(TxtAmount));
            }
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

        public string this[string columnName]
        {
            get 
            {
                return null;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
