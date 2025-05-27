using FlowLedger.Enums;
using FlowLedger.ViewModels;
using System.ComponentModel;

namespace FlowLedger
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public HashSet<string> CURRENCIES { get; } = new HashSet<string> 
        {
            "SEK", 
            "EUR" 
        };

        public HashSet<string> CategoryNames { get; set; } = new HashSet<string>
        {
            "Uncategorized",
            "Salary",
            "Food",
            "Rent"
        };

        private TransactionType _selectedTransactionType = TransactionType.Spend;


        public event PropertyChangedEventHandler? PropertyChanged;

        private string _selectedCategoryName;

        private TransactionViewModel _transactionVM;

        public MainViewModel()
        {
            SelectedCategoryName = CategoryNames.First();
            _transactionVM = new TransactionViewModel();
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

        public TransactionViewModel TransactionVM
        {
            get => _transactionVM;
            set
            {
                _transactionVM = value;
                OnPropertyChanged(nameof(TransactionVM));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
