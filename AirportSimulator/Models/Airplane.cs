using AirportSimulator.Enums;
using System.ComponentModel;

namespace AirportSimulator.Models
{
    internal class Airplane : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string Error => null;

        private string _name;
        private bool _isNameValid;

        private string _flightId;
        private bool _isFlightIdValid;

        private string _destination;
        private bool _isDestinationValid;

        private string _txtFlightDuration;
        private double _flightDuration;
        private bool _isDurationConvertable;
        private bool _isFlightDurationValid;

        private bool _canAddFlight;

        private FlightStatus _flightStatus = FlightStatus.Scheduled;
        private bool _canLand;

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
                _isNameValid = false;
                ValidateName(value);
                ValidateAll();
            }
        }

        public string FlightId
        {
            get => _flightId;
            set
            {
                _flightId = value;
                OnPropertyChanged(nameof(FlightId));
                _isFlightIdValid = false;
                ValidateFlightId(value);
                ValidateAll();
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
                _isDestinationValid = false;
                ValidateDestination(value);
                ValidateAll();
            }
        }

        public string TxtFlightDuration
        {
            get => _txtFlightDuration;
            set
            {
                _txtFlightDuration = value;
                OnPropertyChanged(nameof(TxtFlightDuration));
                _isDurationConvertable = false;
                _isFlightDurationValid = false;
                double? duration = ConvertFlightDuration(value);
                if (duration != null)
                {
                    ValidateFlightDuration(duration.Value);
                    ValidateAll();
                }
            }
        }


        public bool CanAddAirplane
        {
            get => _canAddFlight;
            set
            {
                _canAddFlight = value;
                OnPropertyChanged(nameof(CanAddAirplane));
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
                    //case nameof(Name):
                    //    if (!_isNameValid)
                    //    {
                    //        return "Please provide Name of the aircraft";
                    //    }
                    //    break;
                    //case nameof(FlightId):
                    //    if (!_isFlightIdValid)
                    //    {
                    //        return "ID of the Flight is required";
                    //    }
                    //    break;
                    //case nameof(Destination):
                    //    if (!_isDestinationValid)
                    //    {
                    //        return "Please specify the Destination";
                    //    }
                    //    break;
                    case nameof(Name):
                        if (!_isNameValid)
                        {
                            return "less than 50 characters.";
                        }
                        break;
                    case nameof(TxtFlightDuration):
                        if (!_isDurationConvertable)
                        {
                            return "value not convertable.";
                        }
                        if (!_isFlightDurationValid)
                        {
                            return "3mins <= duration <= 24hrs.";
                        }
                        break;
                }
                return null;
            }
        }

        private void ValidateName(string name)
        {
            if (name.Length <= 50)
            {
                _isNameValid = true;
            }
        }

        private void ValidateFlightId(string flightId)
        {
            _isFlightIdValid = !string.IsNullOrEmpty(flightId);
        }

        private void ValidateDestination(string destination)
        {
            _isDestinationValid = !string.IsNullOrEmpty(destination);
        }

        private double? ConvertFlightDuration(string duration)
        {
            if (double.TryParse(duration, out double flightDuration))
            {
                _isDurationConvertable = true;
                return flightDuration;
            }
            return null;
        }
        
        private void ValidateFlightDuration(double flightDuration)
        {
            // input validation [assume]: no flight lasts longer than 24 hours or less then 3 mins
            if (flightDuration > 24.0 || flightDuration < 0.05)
            {
                _isFlightDurationValid = false;
            } else
            {
                _isFlightDurationValid = true;
                _flightDuration = flightDuration;
            }
        }

        private void ValidateAll()
        {
            CanAddAirplane = !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(FlightId)
                && !string.IsNullOrWhiteSpace(Destination) 
                && _isDurationConvertable && _isFlightDurationValid;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
