using PublicVariables;

namespace SatelliteMath
{
    class OrbitalCalculator
    {
        public static double OrbitalPeriodviaHeight(Satellite satellite, PlanetVariables planet)
        {
            double semiMajorAxis = satellite.SemiMajorAxis;
            double semiMajorAxisMeters;
            if (satellite.SemiMajorAxis > 0)
            {
                semiMajorAxisMeters = satellite.SemiMajorAxis * Constants.MetersInKilometer;
            }
            else if (satellite.Altitude > 0 && satellite.Eccentricity == 0)
            {
                semiMajorAxisMeters = (planet.Radius + satellite.Altitude) * Constants.MetersInKilometer;
            }
            else
            {
                throw new ArgumentException("Cannot calculate orbital period: insufficient orbital parameters");
            }
            double periodSeconds = 2 * Math.PI * Math.Sqrt(Math.Pow(semiMajorAxisMeters, 3) / planet.GravitationalParameter);
            return periodSeconds;
        }

        public static double CalculateSemiMajorAxis(double periapsis, double apoapsis)
        {
            return (periapsis + apoapsis) / 2;
        }

        private static double CalculateCurrentDistance(Satellite satellite, PlanetVariables planet, double trueAnomalyDegrees)
        {
             if (satellite.Eccentricity == 0)
            {
                return (planet.Radius + satellite.Altitude) * Constants.MetersInKilometer;
            }
            
            double a = satellite.SemiMajorAxis * Constants.MetersInKilometer;
            double e = satellite.Eccentricity;
            double theta = trueAnomalyDegrees * Math.PI / 180; // radians
            
            return a * (1 - e * e) / (1 + e * Math.Cos(theta));
        }

        public static double OrbitalVelocity(Satellite satellite, PlanetVariables planet, double trueAnomaly = 0)
        {
            double r;
    
            if (satellite.Eccentricity > 0)
            {
                // v = √[μ(2/r - 1/a)]
                double semiMajorAxisMeters = satellite.SemiMajorAxis * Constants.MetersInKilometer;
                double currentDistance = CalculateCurrentDistance(satellite, planet, trueAnomaly);
                r = currentDistance;
                
                return Math.Sqrt(planet.GravitationalParameter * (2/r - 1/semiMajorAxisMeters));
            }
            else
            {
                // old formula
                r = (planet.Radius + satellite.Altitude) * Constants.MetersInKilometer;
                return Math.Sqrt(planet.GravitationalParameter / r);
            }
        }

        public static double AngularVelocity(Satellite satellite)
        { 
            return 360.0 / satellite.OrbitalPeriod; // deegrees per second
            // ω = 360° / T
        }

        public static double SolveKeplerEquation(double meanAnomaly, double eccentricity, double tolerance = 1e-12) // M = E - e·sin(E)
        {
            double M = meanAnomaly * Math.PI / 180.0;

            double E = M;
            for (int i = 0; i < 50; i++) // i < 50, where 50 is a count of iterations
            {
                double f = E - eccentricity * Math.Sin(E) - M;
                double fPrime = 1 - eccentricity * Math.Cos(E);
                double newE = E - f / fPrime;

                if (Math.Abs(newE - E) < tolerance)
                {
                    E = newE;
                    break;
                }

                E = newE;
            }

            return E * 180 / Math.PI;
        }

        public static double TrueAnomalyFromEccentric(double eccentricAnomaly, double eccentricity) // θ = 2 * atan( √((1+e)/(1-e)) * tan(E/2) )
        {
            double E = eccentricAnomaly * Math.PI / 180.0;

            double term = Math.Sqrt((1 + eccentricity) / (1 - eccentricity)) * Math.Tan(E/2);
            double trueAnomalyRad = 2 * Math.Atan(term);

            return trueAnomalyRad * 180 / Math.PI;
        }

        public static (double x, double y) CalculateEllipticalPosition(Satellite satellite, double trueAnomaly)
        {
            // r = a * (1 - e²) / (1 + e * cos(θ))
            // x = r * cos(θ)  
            // y = r * sin(θ)

            double theta = trueAnomaly * Math.PI / 180;

            double r = satellite.SemiMajorAxis * (1 - satellite.Eccentricity * satellite.Eccentricity) / (1 + satellite.Eccentricity * Math.Cos(theta));

            double x = r * Math.Cos(theta);
            double y = r * Math.Sin(theta);

            return (x, y);
        }

        public static double CalculateEccentricity(double periapsisAltitude, double apoapsisAltitude, double planetRadius)
        {
            double r_peri = periapsisAltitude + planetRadius;  // radius pericenter
            double r_apo = apoapsisAltitude + planetRadius;    // radius apocenter
            return (r_apo - r_peri) / (r_apo + r_peri);
        }
    }
}