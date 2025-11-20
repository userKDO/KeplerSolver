using PublicVariables;

namespace SatelliteMath
{
    class OrbitalCalculator
    {
        public static double OrbitalPeriodviaHeight(Satellite satellite, PlanetVariables planet)
        {
            double semiMajorAxis = satellite.SemiMajorAxis;
            double semiMajorAxisMeters;
            double periodSeconds;

            if (satellite.SemiMajorAxis > 0)
            {
                semiMajorAxisMeters = satellite.SemiMajorAxis * Constants.MetersInKilometer;
            }
            else if (satellite.Altitude > 0 && Math.Abs(satellite.Eccentricity) < 1e-10)
            {
                semiMajorAxisMeters = (planet.Radius + satellite.Altitude) * Constants.MetersInKilometer;
            }
            else
            {
                throw new ArgumentException($"Cannot calculate orbital period: insufficient parameters. SMA: {satellite.SemiMajorAxis}, Alt: {satellite.Altitude}, Ecc: {satellite.Eccentricity}");
            }
            if (semiMajorAxisMeters <= 0)
            {
                throw new ArgumentException("Semi-major axis must be positive");
            }
            periodSeconds = 2 * Math.PI * Math.Sqrt(Math.Pow(semiMajorAxisMeters, 3) / planet.GravitationalParameter);
            return periodSeconds;
        }

        public static double CalculateSemiMajorAxis(double periapsisAltitude, double apoapsisAltitude, double planetRadius)
        {
            double periapsisDistance = periapsisAltitude + planetRadius; // range from the center
            double apoapsisDistance = apoapsisAltitude + planetRadius;   // tange from the center
            return (periapsisDistance + apoapsisDistance) / 2;
        }

        private static double CalculateCurrentDistance(Satellite satellite, PlanetVariables planet, double trueAnomalyDegrees)
        {
            double a;
            if (satellite.SemiMajorAxis > 0)
            {
                a = satellite.SemiMajorAxis * Constants.MetersInKilometer;
            }
            else if (satellite.Eccentricity == 0 && satellite.Altitude > 0)
            {
                a = (planet.Radius + satellite.Altitude) * Constants.MetersInKilometer;
            }
            else
            {
                throw new ArgumentException("Cannot calculate current distance: insufficient orbital parameters");
            }

            if (satellite.Eccentricity == 0)
            {
                return a;
            }
            else
            {
                double e = satellite.Eccentricity;
                double theta = trueAnomalyDegrees * Math.PI / 180; // radians
                return a * (1 - e * e) / (1 + e * Math.Cos(theta));
            }
        }

        public static double OrbitalVelocity(Satellite satellite, PlanetVariables planet, double trueAnomaly = 0)
        {
            double semiMajorAxisMeters;
    
            if (satellite.Eccentricity > 0)
            {
                // v = √[μ(2/r - 1/a)]
                semiMajorAxisMeters = satellite.SemiMajorAxis * Constants.MetersInKilometer;
            }
            else if (satellite.Altitude > 0 && satellite.Eccentricity == 0)
            {
                // old formula
                semiMajorAxisMeters = (planet.Radius + satellite.Altitude) * Constants.MetersInKilometer;
            }
            else
            {
                throw new ArgumentException("Cannot calculate orbital velocity: insufficient orbital parameters");
            }

            double currentDistance = CalculateCurrentDistance(satellite, planet, trueAnomaly);
            return Math.Sqrt(planet.GravitationalParameter * (2/currentDistance - 1/semiMajorAxisMeters));
        }

        public static double AngularVelocity(Satellite satellite)
        { 
            return 360.0 / satellite.OrbitalPeriod; // deegrees per second
            // ω = 360° / T
        }

        public static double InstantaneousAngularVelocity(Satellite satellite, PlanetVariables planet, double trueAnomaly)
        {
            // Instaneous anagular velocity: ω = h / r²
            double h = Math.Sqrt(planet.GravitationalParameter * satellite.SemiMajorAxis * Constants.MetersInKilometer * (1 - satellite.Eccentricity * satellite.Eccentricity));
            double r = CalculateCurrentDistance(satellite, planet, trueAnomaly);
            return (h / (r * r)) * (180 / Math.PI); // rad/s → °/s
        }

        public static double SolveKeplerEquation(double meanAnomaly, double eccentricity, double tolerance = 1e-12) // M = E - e·sin(E)
        {
            if (Math.Abs(eccentricity) < 1e-10)
            {
                return meanAnomaly; // Для круговой орбиты E = M
            }
            
            if (eccentricity >= 1)
            {
                throw new ArgumentException("Only elliptical orbits supported");
            }

            double M = meanAnomaly * Math.PI / 180.0;

            M = M % (2 * Math.PI);
            if (M < 0) M += 2 * Math.PI;    //  M in range [0, 2π)
            
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
            if (Math.Abs(eccentricity - 1) < 1e-10)
            {
                throw new ArgumentException("Parabolic orbits not supported");
            }
            
            if (Math.Abs(eccentricity) < 1e-10)
            {
                return eccentricAnomaly; // for circular orbit E = θ
            }
            
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

            double semiMajorAxisMeters = satellite.SemiMajorAxis * Constants.MetersInKilometer;
    
            double r = semiMajorAxisMeters * (1 - satellite.Eccentricity * satellite.Eccentricity) / (1 + satellite.Eccentricity * Math.Cos(theta));

            double x = r * Math.Cos(theta);
            double y = r * Math.Sin(theta);

            return (x, y); // now in meters
        }

        public static double CalculateEccentricity(double periapsisAltitude, double apoapsisAltitude, double planetRadius)
        {
            if (periapsisAltitude < 0 || apoapsisAltitude < 0)
            {
                throw new ArgumentException("Altitudes cannot be negative");
            }
            
            if (apoapsisAltitude < periapsisAltitude)
            {
                throw new ArgumentException("Apoapsis must be greater than periapsis");
            }

            double r_peri = periapsisAltitude + planetRadius;  // radius pericenter
            double r_apo = apoapsisAltitude + planetRadius;    // radius apocenter
            return (r_apo - r_peri) / (r_apo + r_peri);
        }
    }
}