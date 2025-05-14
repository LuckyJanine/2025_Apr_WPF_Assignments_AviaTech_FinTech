using AirportSimulator.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

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
                OnPropertyChanged(nameof(AirplanesInQueue));
            }
        }

        public ObservableCollection<Airplane> Flights
        {
            get => _flights;
            set
            {
                _flights = value;
                OnPropertyChanged(nameof(Flights));
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
                LoadAirplaneQueue();
            }
            return (ok, err);
        }

        public void TakeOff(Airplane airplane)
        {
            AirplanesInQueue.Remove(airplane);
            _controlTower.TakeOff(airplane.TrackerId);
        }

        private void LoadAirplaneQueue()
        {
            AirplanesInQueue.Clear();
            foreach (var airplane in _controlTower.Airplanes)
            {
                if (airplane.FlightStatus == Enums.FlightStatus.QueuedForTakeoff)
                {
                    AirplanesInQueue.Add(airplane);
                }
            }
        }

        private void RefreshFlightStatus()
        {
            Flights.Clear();
            foreach (var flight in _controlTower.Airplanes)
            {
                if (flight.FlightStatus != Enums.FlightStatus.QueuedForTakeoff)
                {
                    Flights.Add(flight);
                }
            }
        }

        private void OnFlightStatusUpdate(object? sender, AirplaneEventArgs e)
        {
            if (((Airplane)sender).TrackerId == e.Tracker)
            {
                RefreshFlightStatus();
                if (e.AirplaneEventType == Enums.AirplaneEventType.Landed)
                {
                    ((Airplane)sender).FlightStatusUpdated -= OnFlightStatusUpdate;
                }
            } else
            {
                MessageBox.Show("Error with tracking.");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
