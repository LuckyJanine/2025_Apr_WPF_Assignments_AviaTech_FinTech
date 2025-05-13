using AirportSimulator.Models;
using System.ComponentModel;

namespace AirportSimulator
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        //public ObservableCollection<Airplane> Airplanes
        //{
        //    get => _controlTower.Airplanes;
        //    set
        //    {

        //    }
        //}

        public (bool, string) QueueAirplaneForTakeoff()
        {
            var airplaneToQueue = new Airplane(
                AirplaneToQueue.Name, 
                AirplaneToQueue.FlightId, 
                AirplaneToQueue.Destination, 
                AirplaneToQueue.FlightDuration
                );
            return _controlTower.TryAddAirplane(airplaneToQueue);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
