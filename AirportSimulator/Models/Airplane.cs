using AirportSimulator.Enums;

namespace AirportSimulator.Models
{
    class Airplane
    {
        public Guid TrackerId { get; private set; }

        public string Name { get; }
        public string FlightId { get; }
        public string Destination { get; }
        public double FlightDuration { get; }

        public FlightStatus FlightStatus  { get; private set; }
        public bool CanLand { get; private set; }

        public Airplane(string name, string flightId, string destination, double flightDuration)
        {
            TrackerId = Guid.NewGuid();
            Name = name;
            FlightId = flightId;
            Destination = destination;
            FlightDuration = flightDuration;
            FlightStatus = FlightStatus.Scheduled;
        }
    }
}
