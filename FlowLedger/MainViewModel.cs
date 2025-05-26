using FlowLedger.Enums;
using FlowLedger.ViewModels;
using System.ComponentModel;

namespace FlowLedger
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public readonly IEnumerable<TransactionType> CategoryTypes 
            = Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>();
        private TransactionType _selectedTransactionType = TransactionType.Spend;

        public HashSet<string> CategoryNames { get; set; } = new HashSet<string>
        {
            "Uncategorized",
            "Salary",
            "Food",
            "Rent"
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _selectedCategoryName;
        private DateTime _transactionDate;

        public MainViewModel()
        {
            SelectedCategoryName = CategoryNames.First();
            TransactionDate = DateTime.Now.Date;
        }

        public TransactionType SelectedTransactionType
        {
            get { return _selectedTransactionType; }
            set 
            { 
                _selectedTransactionType = value; 
                OnPropertyChanged(nameof(SelectedTransactionType));
            }
        }

        public string SelectedCategoryName
        {
            get => _selectedCategoryName;
            set
            {
                _selectedCategoryName = value;
                OnPropertyChanged(nameof(SelectedCategoryName));
            }
        }

        public DateTime TransactionDate
        {
            get => _transactionDate;
            set
            {
                _transactionDate = value;
                OnPropertyChanged(nameof(TransactionDate));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
