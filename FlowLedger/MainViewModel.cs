using FlowLedger.Enums;
using FlowLedger.ViewModels;
using System.Collections.ObjectModel;
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

        private HashSet<string> _categoryNames = new HashSet<string>
        {
            "Uncategorized",
            "Salary",
            "Food",
            "Rent"
        };
        public ObservableCollection<string> CategoryNamesView { get; } = new();

        private TransactionType _selectedTransactionType = TransactionType.Spend;
        private string _newCategoryToAdd;


        public event PropertyChangedEventHandler? PropertyChanged;

        private string _selectedCategoryName;

        private TransactionViewModel _transactionVM;

        public MainViewModel()
        {
            CategoryNamesView = new ObservableCollection<string>(_categoryNames);
            SelectedCategoryName = _categoryNames.First();
            _transactionVM = new TransactionViewModel();
        }

        public string NewCategoryToAdd
        {
            get => _newCategoryToAdd;
            set
            {
                _newCategoryToAdd = value;
                OnPropertyChanged(nameof(NewCategoryToAdd));
            }
        }

        public IEnumerable<string> CategoryNames
        {
            get => _categoryNames;
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

        public (bool, string) AddNewCategory(string newCategory)
        {
            bool ok = _categoryNames.Add(newCategory);
            string errorMsg = string.Empty;
            if (ok)
            {
                CategoryNamesView.Clear();
                foreach (var category in _categoryNames)
                {
                    CategoryNamesView.Add(category);
                }
                SelectedCategoryName = _categoryNames.First();
                return (ok, errorMsg);
            }
            else 
            {
                errorMsg = "Can't add. Already exist.";
            }
            return (ok, errorMsg);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
