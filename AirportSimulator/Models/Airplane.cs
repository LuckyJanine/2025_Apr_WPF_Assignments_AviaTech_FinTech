using AirportSimulator.Enums;
using System.ComponentModel;

namespace AirportSimulator.Models
{
    internal class Airplane : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string Error => null;

        private string _name;
        private bool _isNameValid = true;

        private string _flightId;
        private bool _isFlightIdValid = true;

        private string _destination;
        private bool _isDestinationValid = true;

        private double _flightDuration;
        private FlightStatus _flightStatus = FlightStatus.Scheduled;
        private bool _canLand = false;
        public Airplane()
        {
            
        }
        public Airplane(string name, string flightId, string destination, double flightDuration)
        {
            _name = name;
            _flightId = flightId;
            _destination = destination;
            _flightDuration = flightDuration;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                ValidateName(value);
            }
        }

        public string FlightId
        {
            get => _flightId;
            set
            {
                _flightId = value;
                OnPropertyChanged(nameof(FlightId));
                ValidateFlightId(value);
            }
        }

        public string Destination
        {
            get => _destination;
            set
            {
                value.Trim();
                _destination = value;
                OnPropertyChanged(nameof(Destination));
                ValidateDestination(value);
            }
        }

        public double FlightDuration
        {
            get => _flightDuration;
            set
            {
                _flightDuration = value;
                OnPropertyChanged(nameof(FlightDuration));
            }
        }

        public bool CanLand
        {
            get => _canLand;
        }

        public virtual string this[string propName]
        {
            get
            {
                switch (propName)
                {
                    case nameof(Name):
                        if (!_isNameValid)
                        {
                            return "Please provide Name of the aircraft";
                        }
                        break;
                    case nameof(FlightId):
                        if (!_isFlightIdValid)
                        {
                            return "ID of the Flight is required";
                        }
                        break;
                    case nameof(Destination):
                        if (!_isDestinationValid)
                        {
                            return "Please specify the Destination";
                        }
                        break;
                }
                return null;
            }
        }

        private void ValidateName(string name)
        {
            _isNameValid = !string.IsNullOrEmpty(name);
        }

        private void ValidateFlightId(string flightId)
        {
            _isFlightIdValid = !string.IsNullOrEmpty(flightId);
        }

        private void ValidateDestination(string destination)
        {
            _isDestinationValid = !string.IsNullOrEmpty(destination);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
