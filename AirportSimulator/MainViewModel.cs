using AirportSimulator.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Media3D;

namespace AirportSimulator
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Airplane> _airplaneInQueue = new ObservableCollection<Airplane>();
        private ObservableCollection<Airplane> _flights = new ObservableCollection<Airplane>();
        private ControlTower _controlTower = new ControlTower();
        public AirplaneViewModel _airplaneToQueue = new AirplaneViewModel();

        public AirplaneViewModel AirplaneToQueue
        {
            get { return _airplaneToQueue; }
            set 
            { 
                _airplaneToQueue = value;
                OnPropertyChanged(nameof(AirplaneToQueue));
            }
        }

        public ObservableCollection<Airplane> AirplanesInQueue
        {
            get => _airplaneInQueue;
            set
            {
                _airplaneInQueue = value;
            }
        }

        public ObservableCollection<Airplane> Flights
        {
            get => _flights;
            set
            {
                _flights = value;
            }
        }

        public (bool, string) QueueAirplaneForTakeoff()
        {
            var airplaneToQueue = new Airplane(
                AirplaneToQueue.Name, 
                AirplaneToQueue.FlightId, 
                AirplaneToQueue.Destination, 
                AirplaneToQueue.FlightDuration
                );
            var (ok, err) = _controlTower.TryAddAirplane(airplaneToQueue);
            if (ok && string.IsNullOrEmpty(err))
            {
                AirplaneToQueue.Reset();
                airplaneToQueue.FlightStatusUpdated += OnFlightStatusUpdate;
                LoadAirplanes();
            }
            return (ok, err);
        }

        public void TakeOff(Guid trackerId)
        {
            _controlTower.TakeOff(trackerId);
        }

        private void LoadAirplanes()
        {
            AirplanesInQueue.Clear();
            Flights.Clear();
            foreach (var airplane in _controlTower.Airplanes)
            {
                if (airplane.FlightStatus == Enums.FlightStatus.QueuedForTakeoff)
                {
                    AirplanesInQueue.Add(airplane);
                } else
                {
                    Flights.Add(airplane);
                }
            }
        }

        private void OnFlightStatusUpdate(object? sender, AirplaneEventArgs e)
        {
            LoadAirplanes();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
