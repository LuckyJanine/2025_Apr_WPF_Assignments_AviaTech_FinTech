using System.Collections.ObjectModel;

namespace AirportSimulator.Models
{
    internal class ControlTower
    {
        protected ObservableCollection<Airplane> airplanes;
        public ControlTower()
        {
            airplanes = new ObservableCollection<Airplane>();
        }

        public ObservableCollection<Airplane> Airplanes
        {
            get => airplanes;
        }

        public bool TryAddAirplane(Airplane airplaneToQueue)
        {
            airplanes.Add(airplaneToQueue);
            return false;
        }
    }
}
