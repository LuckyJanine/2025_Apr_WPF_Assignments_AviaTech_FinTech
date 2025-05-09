using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.Enums
{
    internal enum FlightStatus
    {
        Scheduled,
        // TaxiingOut - to the runway
        QueuedForTakeoff, // on the runway
        // ClearForTakeoff,
        Takeoff, // on the runway and accelarating
        Climbing, // Airborne
        Cruising,
        Landing, // descending and touching down
        // TaxiingIn - to the gate
        ArrivedAtGate
    }
}
