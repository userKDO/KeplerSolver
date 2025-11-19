using PublicVariables;

namespace SatelliteMath
{
    class OrbitalCalculator
    {
        public static double OrbitalPeriodviaHeight(Satellite satellite, PlanetVariables planet)
        {
            double semiMajorAxis = (planet.Radius + satellite.Altitude) * Constants.MetersInKilometer; // semi-major axis in meters
            double periodSeconds = 2 * Math.PI * Math.Sqrt(Math.Pow(semiMajorAxis,3) / planet.GravitationalParameter);

            return periodSeconds;
        }
        public static double OrbitalVelocity(Satellite satellite, PlanetVariables planet)
        {
            double r = (planet.Radius + satellite.Altitude) * Constants.MetersInKilometer;
            double velocityMs = Math.Sqrt(planet.GravitationalParameter / r);
            return velocityMs;
            // Formula: v = √(μ / r)
            // where r = planet.Radius + satellite.Altitude
        }

        public static double AngularVelocity(Satellite satellite)
        { 
            return 360.0 / satellite.OrbitalPeriod; // deegrees per second
            // ω = 360° / T
        }
    }
}