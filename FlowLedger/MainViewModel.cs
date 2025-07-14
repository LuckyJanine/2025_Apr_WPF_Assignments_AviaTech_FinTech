using FlowLedger.Enums;
using FlowLedger.Models;
using FlowLedger.Utils;
using FlowLedger.ViewModels;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

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
        public ObservableCollection<int> Years { get; } = new ObservableCollection<int> { 0 };

        private HashSet<string> _categoryNames = new HashSet<string>
        {
            "Uncategorized",
            "Salary",
            "Food",
            "Rent"
        };

        private string _filePath;

        private Balance _currentBalance;
        private Month _selectedMonth = Month.NotSelected;
        private int _selectedYear = 0;

        private bool _isOverviewVisible = false;

        private decimal _monthlyRevenue;
        private decimal _monthlyExpense;
        private decimal _monthlyTotalNet;
        private bool _isDeficit;

        private ObservableCollection<TransactionDetail> _transactions;
        private Dictionary<YearMonth, MonthTransactions> _transactionsByYearmonth;

        public ObservableCollection<string> CategoryNamesView { get; } = new();

        private TransactionType _selectedTransactionType = TransactionType.Spend;

        private string _newCategoryToAdd;
        private string _searchQuery;

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
            _transactionsByYearmonth = new Dictionary<YearMonth, MonthTransactions>();
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
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

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
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
                PopulateMonthlyTransactions();
            }
        }

        public int SelectedYear
        {
            get => _selectedYear;
            set 
            {
                _selectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
                PopulateMonthlyTransactions();
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

        public decimal MonthlyRevenue
        {
            get => _monthlyRevenue;
            set
            {
                _monthlyRevenue = value;
                OnPropertyChanged(nameof(MonthlyRevenue));
            }
        }

        public decimal MonthlyExpense
        {
            get => _monthlyExpense;
            set
            {
                _monthlyExpense = value;
                OnPropertyChanged(nameof(MonthlyExpense));
            }
        }

        public decimal MonthlyTotalNet
        {
            get => _monthlyTotalNet;
            set
            {
                _monthlyTotalNet = value;
                OnPropertyChanged(nameof(MonthlyTotalNet));
            }
        }

        public bool IsDeficit
        {
            get => _isDeficit;
            set 
            {
                _isDeficit = value;
                OnPropertyChanged(nameof(IsDeficit));
            }
        }

        public List<KeyValuePair<YearMonth, MonthTransactions>> MonthTransactions
        {
            get
            {
                // include monthtransactions from likely more than one year.
                var selectedMonth = (int)SelectedMonth;
                var transactions = _transactionsByYearmonth
                    .Where(kvp => kvp.Key.Month == selectedMonth)
                    .ToList();
                return transactions;
            }
        }

        private void InitializeAvailableYearsWithTransactions()
        {
            var availableYears = _transactionsByYearmonth.Keys
                .Select(ym => ym.Year)
                .Distinct()
                .ToList();
            foreach (var year in availableYears) 
            {
                Years.Add(year);
            }
        }

        private void UpdateYearsWithTransactions(int year)
        {
            if (!Years.Contains(year))
            {
                int index = 0;
                while (index < Years.Count && Years[index] < year)
                {
                    index++;
                }
                Years.Insert(index, year);
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
            // make this work only when Year is defined.
            if (SelectedMonth != Month.NotSelected && SelectedYear != 0)
            {
                IsOverviewVisible = true;
                var month = (int)SelectedMonth;
                var year = SelectedYear;
                var yearmonth = new YearMonth(year, month);
                if (_transactionsByYearmonth.ContainsKey(yearmonth))
                {
                    MonthlyRevenue = _transactionsByYearmonth[yearmonth].TotalRevenue;
                    MonthlyExpense = _transactionsByYearmonth[yearmonth].TotalExpense;
                    MonthlyTotalNet = _transactionsByYearmonth[yearmonth].MonthlyNet;
                    IsDeficit = _transactionsByYearmonth[yearmonth].IsDeficit;
                } else
                {
                    MonthlyRevenue = decimal.Zero;
                    MonthlyExpense = decimal.Zero;
                    MonthlyTotalNet = decimal.Zero;
                    IsDeficit = false;
                }
            }
            else
            {
                IsOverviewVisible = false;
            }
        }

        private void PopulateMonthlyTransactions()
        {
            var month = (int)SelectedMonth;
            var year = SelectedYear;
            var yearmonth = new YearMonth(year, month);
            if (SelectedMonth != Month.NotSelected && SelectedYear != 0)
            {
                if (_transactionsByYearmonth.TryGetValue(yearmonth, out var monthTransactions))
                {
                    Transactions = new ObservableCollection<TransactionDetail>(monthTransactions.Transactions);
                }
                else
                {
                    Transactions = new ObservableCollection<TransactionDetail>();
                }
            }
            else if (SelectedMonth != Month.NotSelected && SelectedYear == 0) // a specific month from all years with transactions
            {
                var monthlyTransactionsNoYearFilter = _transactionsByYearmonth
                    .Where(kvp => kvp.Key.Month == month)
                    .Select(p => p.Value)
                    .SelectMany(t => t.Transactions);
                Transactions = new ObservableCollection<TransactionDetail>(monthlyTransactionsNoYearFilter);
            }
            else if (SelectedMonth == Month.NotSelected && SelectedYear != 0) // a specific year with all transactions
            {
                var yearlyTransactions = _transactionsByYearmonth
                    .Where(kvp => kvp.Key.Year == year)
                    .Select(p => p.Value)
                    .SelectMany(t => t.Transactions);
                Transactions = new ObservableCollection<TransactionDetail>(yearlyTransactions);
            }
            else
            {
                Transactions =
                    new ObservableCollection<TransactionDetail>(_transactionsByYearmonth.Values.SelectMany(t => t.Transactions));
            }
        }

        public (bool, string) Save()
        {
            return FileHelper.RunWithFile(FilePath, SaveTransactions);
        }

        private bool SaveTransactions(string file)
        {
            try
            {
                var accountStatusToSave = new AccountStatus(CurrentBalance.CurrentBalance, _transactionsByYearmonth);
                string jsontransactions = JsonConvert.SerializeObject(accountStatusToSave, Formatting.Indented, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                File.WriteAllText(file, jsontransactions);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public (bool, string) OpenTransactions()
        {
            try
            {
                var (json, error) = FileHelper.RunWithFile(FilePath, File.ReadAllText);
                if (json != null && string.IsNullOrEmpty(error))
                {
                    Transactions.Clear();
                    _transactionsByYearmonth.Clear();
                    var deserialized = JsonConvert.DeserializeObject<AccountStatus>(json, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                    });
                    _transactionsByYearmonth = deserialized.MonthlyTransactions;
                    _currentBalance = new Balance(deserialized.CurrentBalance, "SEK");
                    OnPropertyChanged(nameof(CurrentBalance));
                    InitializeAvailableYearsWithTransactions();
                    PopulateMonthlyTransactions();
                    // todo:
                    // reset Filter and Search
                    return (true, string.Empty);
                } else
                {
                    return (false, error);
                }
            }
            catch (Exception ex) 
            {
                return (false, ex.Message);
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
                AddTransactionToDictionaryCollection(transaction);
                UpdateMonthlyOverview();
                PopulateMonthlyTransactions();
                ResetTransaction();
            }
        }

        private void ResetTransaction()
        {
            TransactionVM = new TransactionViewModel();
        }

        public void SearchTransactions()
        {
            // implement a simple case-insensitive substring match search
            // another way for fuzzysearch is to use NuGet pkg - FuzzySharp
            var searchQuery = SearchQuery.ToLower();
            var searchResult = _transactionsByYearmonth
                .SelectMany(monthTrans => monthTrans.Value.Transactions)
                .Where(tran => 
                        ((tran.Category != null && !string.IsNullOrEmpty(tran.Category.Name)) &&
                          tran.Category.Name.ToLower().Contains(searchQuery)) ||
                        (!string.IsNullOrEmpty(tran.Description)) &&
                          (tran.Description.ToLower().Contains(searchQuery))
                )
                .ToList();
            Transactions = new ObservableCollection<TransactionDetail>(searchResult);
        }

        // Don't feel comfortable with keeping two collections as requested 3.2 in assignment doc
        // use Dictionary as single point of truth.
        // omit List<Transaction> on purpose.
        private void AddTransactionToDictionaryCollection(TransactionDetail transaction)
        {
            if (transaction.CreationDate != null)
            {
                int year = transaction.CreationDate.Value.Year;
                int month = transaction.CreationDate.Value.Month;
                var yearmonth = new YearMonth(year, month);
                if (!_transactionsByYearmonth.ContainsKey(yearmonth))
                {
                    var monthTransactions = new MonthTransactions();
                    monthTransactions.Add(transaction);
                    _transactionsByYearmonth.Add(yearmonth, monthTransactions);
                    UpdateYearsWithTransactions(year);
                }
                else if (_transactionsByYearmonth[yearmonth] != null)
                {
                    _transactionsByYearmonth[yearmonth].Add(transaction);
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
