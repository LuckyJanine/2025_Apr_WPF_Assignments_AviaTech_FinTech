using AirportSimulator.Models;

namespace AirportSimulator
{
    internal class MainViewModel
    {
        private ControlTower _controlTower;
        private Airplane _airplaneToQueue = new Airplane();

        public Airplane AirplaneToQueue
        {
            get => _airplaneToQueue;
            set => _airplaneToQueue = value;
        }
    }
}
