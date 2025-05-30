using FlowLedger.Enums;
using FlowLedger.Models;
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

        public IEnumerable<Month> Months { get; } = Enum.GetValues(typeof(Month)).Cast<Month>();

        private HashSet<string> _categoryNames = new HashSet<string>
        {
            "Uncategorized",
            "Salary",
            "Food",
            "Rent"
        };

        private Balance _currentBalance;
        private Month _selectedMonth = Month.NotSelected;

        private bool _isOverviewVisible = false;

        private ObservableCollection<TransactionDetail> _transactions;
        private Dictionary<string, MonthTransactions> _monthlyTransactions;

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
            _currentBalance = new Balance(0, "SEK");
            _transactions = new ObservableCollection<TransactionDetail>();
            _monthlyTransactions = new Dictionary<string, MonthTransactions>();
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

        public Balance CurrentBalance
        {
            get => _currentBalance;
            set
            {
                _currentBalance = value;
                OnPropertyChanged(nameof(CurrentBalance));
            }
        }

        public Month SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged(nameof(SelectedMonth));
                UpdateMonthlyOverview();
            }
        }

        public bool IsOverviewVisible
        {
            get => _isOverviewVisible;
            set
            {
                _isOverviewVisible = value;
                OnPropertyChanged(nameof(IsOverviewVisible));
            }
        }

        public ObservableCollection<TransactionDetail> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                OnPropertyChanged(nameof(Transactions));
            }
        }

        private void UpdateMonthlyOverview()
        {
            if (SelectedMonth != Month.NotSelected)
            {
                IsOverviewVisible = true;
            }
            else
            {
                IsOverviewVisible = false;
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

        public void ConfirmTransaction()
        {
            if (_transactionVM != null) 
            {
                var category = new TransactionCategory(SelectedCategoryName, SelectedTransactionType);
                var transaction = new TransactionDetail(_transactionVM.Amount,
                    _transactionVM.CurrencyCode,
                    category,
                    _transactionVM.Description,
                    _transactionVM.TransactionDate);
                var amount = transaction.Amount;
                switch (SelectedTransactionType)
                {
                    case TransactionType.Spend:
                        amount = amount * -1;
                        break;
                }
                CurrentBalance.ConfirmTransaction(amount, transaction.Currency);
                OnPropertyChanged(nameof(CurrentBalance));
                _transactions.Add(transaction);
                SyncListTransactionsAndDicTransactions(transaction);
                ResetTransaction();
            }
        }

        private void ResetTransaction()
        {
            TransactionVM = new TransactionViewModel();
        }

        // not sure why use two collections for Transactions
        // but required by 5.3
        private void SyncListTransactionsAndDicTransactions(TransactionDetail transaction)
        {
            if (transaction.CreationDate != null)
            {
                string month = transaction.CreationDate.Value.ToString("MMMM");
                if (!_monthlyTransactions.ContainsKey(month))
                {
                    var monthTransactions = new MonthTransactions();
                    monthTransactions.Add(transaction);
                    _monthlyTransactions.Add(month, monthTransactions);
                } else if (_monthlyTransactions[month] != null)
                {
                    _monthlyTransactions[month].Add(transaction);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
