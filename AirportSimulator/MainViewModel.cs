using AirportSimulator.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AirportSimulator
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ControlTower _controlTower;
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
            get => _controlTower.Airplanes;
            set
            {

            }
        }

        public bool QueueAirplaneForTakeoff()
        {
            
            return false;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
