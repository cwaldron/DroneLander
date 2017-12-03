using DroneLander.Common;

namespace DroneLander.Models
{
    public enum LandingResultType { Landed, Kaboom, }
    public class LandingParameters
    {
        public LandingParameters()
        {
            Altitude = CoreConstants.StartingAltitude;
            Velocity = CoreConstants.StartingVelocity;
            Fuel = CoreConstants.StartingFuel;
            Thrust = CoreConstants.StartingThrust;
        }

        public double Altitude;
        public double Velocity;
        public double Fuel;
        public double Thrust;
    }
}