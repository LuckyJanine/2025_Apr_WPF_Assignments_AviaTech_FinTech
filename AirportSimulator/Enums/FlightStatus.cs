namespace AirportSimulator.Enums
{
    internal enum FlightStatus
    {
        // Scheduled,
        // TaxiingOut - to the runway
        QueuedForTakeoff, // on the runway
        // ClearForTakeoff,
        Takeoff, // on the runway and accelarating
        // Climbing, // Airborne
        Cruising,
        // Landing, // descending and touching down
        Landed
        // TaxiingIn - to the gate
        // ArrivedAtGate
    }
}
