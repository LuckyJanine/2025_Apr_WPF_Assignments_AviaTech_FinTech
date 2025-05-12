using AirportSimulator.Models;
using System.ComponentModel;

namespace AirportSimulator
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ControlTower _controlTower;
        public Airplane _airplaneToQueue = new Airplane();

        public Airplane AirplaneToQueue
        {
            get { return _airplaneToQueue; }
            set 
            { 
                _airplaneToQueue = value;
                OnPropertyChanged(nameof(AirplaneToQueue));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
