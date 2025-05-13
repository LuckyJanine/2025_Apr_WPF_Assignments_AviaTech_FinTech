using AirportSimulator.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AirportSimulator
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Airplane> _airplanes = new ObservableCollection<Airplane>();
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

        public ObservableCollection<Airplane> Airplanes
        {
            get => _airplanes;
            set
            {
                _airplanes = value;
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
                LoadAirplanes();
            }
            return (ok, err);
        }

        private void LoadAirplanes()
        {
            _airplanes.Clear();
            foreach (var airplane in _controlTower.Airplanes)
            {
                _airplanes.Add( airplane );
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
