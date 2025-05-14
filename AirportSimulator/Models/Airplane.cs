using AirportSimulator.Enums;
using System.Windows.Threading;

namespace AirportSimulator.Models
{
    class Airplane
    {
        private readonly TimeSpan _takeoffDuration = TimeSpan.FromSeconds(0.05);

        public Guid TrackerId { get; private set; }

        public string Name { get; }
        public string FlightId { get; }
        public string Destination { get; set; }
        public double FlightDuration { get; }

        public FlightStatus FlightStatus { get; private set; }
        public bool CanReschedule { get; private set; }

        public event EventHandler<AirplaneEventArgs>? FlightStatusUpdated;

        private DispatcherTimer _dispatcherTimer;
        public TimeOnly TakeoffAt { get; private set; }
        public TimeOnly LandAt { get; private set; }

        public Airplane(string name, string flightId, string destination, double flightDuration)
        {
            TrackerId = Guid.NewGuid();
            Name = name;
            FlightId = flightId;
            Destination = destination;
            FlightDuration = flightDuration;
            FlightStatus = FlightStatus.QueuedForTakeoff;
            _dispatcherTimer = new DispatcherTimer();
        }

        public void Takeoff()
        {
            FlightStatus = FlightStatus.Takeoff;
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(0.05);
            TakeoffAt = TimeOnly.FromDateTime(DateTime.Now);
            _dispatcherTimer.Tick += _dispatcherTimer_Tick;
            _dispatcherTimer.Start();
        }

        private void _dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            if (((TimeOnly.FromDateTime(DateTime.Now) - TakeoffAt) >= _takeoffDuration) && // Takeoff complete
                ((TimeOnly.FromDateTime(DateTime.Now) - TakeoffAt) < TimeSpan.FromSeconds(FlightDuration))) 
            {
                FlightStatus = FlightStatus.Cruising;
                FlightStatusUpdated?.Invoke(this, new AirplaneEventArgs(TrackerId, AirplaneEventType.TakeoffComplete));
            } else if (((TimeOnly.FromDateTime(DateTime.Now) - TakeoffAt) >= TimeSpan.FromSeconds(FlightDuration))) // Landed
            {
                _dispatcherTimer.Stop();
                _dispatcherTimer.Tick -= _dispatcherTimer_Tick;

                LandAt = TimeOnly.FromDateTime(DateTime.Now);
                FlightStatus = FlightStatus.Landed;
                Destination = "Home";
                CanReschedule = true;
                FlightStatusUpdated?.Invoke(this, new AirplaneEventArgs(TrackerId, AirplaneEventType.Landed));
            }
        }
    }
}
