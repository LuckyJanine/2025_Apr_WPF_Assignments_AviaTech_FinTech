using System.ComponentModel;

namespace FlowLedger.ViewModels
{
    internal class CustomerViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public string Error => null;

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _id;
        private string _lastName;
        private string _firstName;

        public string ID
        {
            get => _id; 
            set
            {
                _id = value; 
                OnPropertyChanged(nameof(ID));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
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
