using AirportSimulator.Enums;

namespace AirportSimulator
{
    class AirplaneEventArgs : EventArgs
    {
        public Guid Tracker { get; }
        public AirplaneEventType AirplaneEventType { get; }

        public AirplaneEventArgs(Guid tracker, AirplaneEventType eventType)
        {
            Tracker = tracker;
            AirplaneEventType = eventType;
        }
    }
}
