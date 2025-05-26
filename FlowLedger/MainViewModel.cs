using FlowLedger.ViewModels;
using System.ComponentModel;

namespace FlowLedger
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private CustomerViewModel _currentCustomer;
        private DateTime _transactionDate;

        public MainViewModel()
        {
            _currentCustomer = new CustomerViewModel();
            _transactionDate = DateTime.Now.Date;
        }

        public CustomerViewModel CurrentCustomer
        {
            get { return _currentCustomer; }
            set 
            { 
                _currentCustomer = value;
                OnPropertyChanged(nameof(CurrentCustomer));
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
