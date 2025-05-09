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
    }
}
