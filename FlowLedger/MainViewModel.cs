using FlowLedger.ViewModels;
using System.ComponentModel;

namespace FlowLedger
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private CustomerViewModel _currentCustomer;

        public MainViewModel()
        {
            _currentCustomer = new CustomerViewModel();
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
