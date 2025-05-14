namespace AirportSimulator.Models
{
    internal class ControlTower
    {
        private readonly int _maxCapacity = 100;
        private List<Airplane> _airplanes;
        public ControlTower()
        {
            _airplanes = new List<Airplane>();
        }

        public List<Airplane> Airplanes
        {
            get => _airplanes;
        }

        public (bool, string) TryAddAirplane(Airplane airplaneToQueue)
        {
            string err = string.Empty;
            if (airplaneToQueue.TrackerId == Guid.Empty)
            {
                err = "Tracker Id assignment failure.";
                return (false, err);
            } else if(_airplanes.Count >= _maxCapacity)
            {
                err = "Maximial number of flights queued up.";
                return (false, err);
            } else
            {
                _airplanes.Add(airplaneToQueue);
                return (true, err);
            }   
        }

        public void TakeOff(Guid tracker)
        {
            var flight = _airplanes.FirstOrDefault(a => a.TrackerId == tracker);
            if (flight != null) 
            {
                flight.Takeoff();
            }
        }
    }
}
