using PublicVariables;

namespace SatelliteMath
{
    public class OrbitalCalculator
    {
        /// <summary>
        /// Calculates orbital period using Kepler's third law: T = 2π * √(a³ / μ)
        /// </summary>
		/// <param name="satellite">Satellite object with orbital parameters</param>
		/// <param name="planet">Celestial body with gravitational parameter</param>
		/// <returns>Orbital period in seconds</returns>
		/// <exception cref="ArgumentException">Thrown when semi-major axis cannot be determined from provided parameters</exception>
		/// <example>
		/// <code>
		/// var satellite = new Satellite { Altitude = 400, Eccentricity = 0};
		/// var period = OrbitalCalculator.OrbitalPeriodViaHeight(satellite, Planet.Earth);
		/// // period ~ 5550 seconds (92.5 minutes)
		/// </code>
		/// </example>
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

        /// <summary>
        /// Necessary for calculating the semi-major axis of an elliptical orbit. Formula: a = (r_peri + r_apo) / 2
        /// </summary>
        /// <remarks>
        /// At the entrance it receives ALTITUDES above the planet, not distances from the center!
        /// </remarks>
        /// <param name="periapsisAltitude">The height of the periapsis (perigee) above the surface of the planet in kilometers</param>
        /// <param name="apoapsisAltitude">The height of the apocenter (apogee) above the surface of the planet in kilometers</param>
        /// <param name="planetRadius">Radius of the planet in kilometers</param>
        /// <returns>Semimajor axis in kilometers</returns>
        public static double CalculateSemiMajorAxis(double periapsisAltitude, double apoapsisAltitude, double planetRadius)
        {
            double periapsisDistance = periapsisAltitude + planetRadius; // range from the center
            double apoapsisDistance = apoapsisAltitude + planetRadius;   // tange from the center
            return (periapsisDistance + apoapsisDistance) / 2;
        }

        /// <summary>
        /// Auxiliary method for calculating the current distance.
        /// For circular orbits: constant distance = semi-major axis
        /// </summary>
        /// <remarks>
        /// Used in: OrbitalVelocity() and InstantaneousAngularVelocity().
        /// </remarks>
        /// <param name="satellite">The object of the Satellite shall contain: semi-major axis in kilometres (priority) OR Altitude and Eccentricity = 0 - for circular orbit</param>
        /// <param name="planet">The object of the planet shall contain Radius - is the radius of the planet in kilometers</param>
        /// <param name="trueAnomalyDegrees">True anomaly in degrees</param>
        /// <returns>Current distance from planet center in meters</returns>
        /// <exception cref="ArgumentException">Cannot calculate current distance: insufficient orbital parameters</exception>
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
        /// <summary>
        /// Calculates orbital velocity at specific true anomaly using vis-viva equation: v = √[μ(2/r - 1/a)]
        /// </summary>
		/// <param name="satellite">Satellite object with orbital parameters</param>
		/// <param name="planet">Celestial body with gravitational parameters</param>
		/// <param name="trueAnomaly">True anomaly in degrees(0-360). For circular orbits this parameter is ignored</param>
		/// <returns>Orbital velocity in meters per second</returns>
		/// <exception cref="ArgumentException">Thrown when orbital parameters are insufficient</exception>
		/// <remarks>
		/// For circular orbits, uses simplified formula. For elliptical orbits, uses universal vis-viva equation.
		/// </remarks>
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

        /// <summary>
        /// Calculates AVERAGE angular velocity. Formula: 360 / T.
        /// </summary>
        /// <param name="satellite">Satellite object with orbital period</param>
        /// <returns> Returns an average angular velocity in degrees per second</returns>
        /// <remarks> The method is accurate ONLY FOR CIRCULAR ORBITS. For an ellipse - the average value </remarks>
        public static double AngularVelocity(Satellite satellite)
        { 
            return 360.0 / satellite.OrbitalPeriod; // deegrees per second
            // ω = 360° / T
        }

        /// <summary>
        /// Calculates INSTANTANEOUS angular velocity for elleptic orbits. Formula: ω = h / r, where h = √[μ * a * (1 - e²)]
        /// </summary>
        /// <param name="satellite">Satellite object with SemiMajorAxis and Eccentricity</param>
        /// <param name="planet">Celestial body with GravitationalParameter</param>
        /// <param name="trueAnomaly">The angular position of the satellite in orbit in degrees (0-360)</param>
        /// <remarks>At perigee the speed is maximum, at apogee it is minimum</remarks>
        /// <returns>Returns instantaneous angular velocity in degrees per second</returns>
        public static double InstantaneousAngularVelocity(Satellite satellite, PlanetVariables planet, double trueAnomaly)
        {
            // Instaneous anagular velocity: ω = h / r²
            double h = Math.Sqrt(planet.GravitationalParameter * satellite.SemiMajorAxis * Constants.MetersInKilometer * (1 - satellite.Eccentricity * satellite.Eccentricity));
            double r = CalculateCurrentDistance(satellite, planet, trueAnomaly);
            return (h / (r * r)) * (180 / Math.PI); // rad/s → °/s
        }

        /// <summary>
        /// Solves Kepler's equation: M = E - e * sin(E) (Newton's Iterative Method)(Solves a nonlinear equation through iterations)
        /// </summary>
        /// <param name="meanAnomaly">The average anomaly in degrees (0-360) is the angle that the satellite would travel with uniform motion</param>
        /// <param name="eccentricity">Orbital eccentricity (0-1)</param>
        /// <param name="tolerance">Equation solution accuracy (default 1e-12)</param>
        /// <returns>Returns eccentric anomaly in degrees</returns>
        /// <exception cref="ArgumentException">Only elliptical orbits supported</exception>
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
        /// <summary>
        /// Converts an eccentric anomaly into a true one. Formula: θ = 2 * atan( √((1+e)/(1-e)) * tan(E/2) )
        /// </summary>
        /// <param name="eccentricAnomaly">Eccentric anomaly in degrees (0-360) - auxiliary angle</param>
        /// <param name="eccentricity">Orbital eccentricity (0-1)</param>
        /// <remarks>Requires e lower than 1 (does not work for parabolic orbits)</remarks>
        /// <returns>Returns the True Anomaly in degrees</returns>        
        /// <exception cref="ArgumentException">Parabolic orbits not supported</exception>
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

        /// <summary>
        /// Calculates the coordinates of the satellite in the orbital plane. 
        /// </summary>
        /// <remarks>
        /// Formulas: 
        /// r = a * (1 - e²) / (1 + e * cos(θ))
        /// x = r * cos(θ)
        /// y = r * sin(θ)
        /// </remarks>
        /// <param name="satellite">Satellite object with: semi-major axis in kilometers, orbital eccentricity (0-1)</param>
        /// <param name="trueAnomaly">The true anomaly in degrees (0-360) is the real angular position</param>
        /// <returns>Returns the coordinates (x, y) in meters</returns>
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

        /// <summary>
        /// Calculates orbital eccentricity from periapsis and apoapsis altitudes. Formula: e = (r_apo - r_peri) / (r_apo + r_peri)
        /// </summary>
        /// <param name="periapsisAltitude">Periapsis altitude above planet surface in kilometers</param>
        /// <param name="apoapsisAltitude">Apoapsis altitude above planet surface in kilometers</param>
        /// <param name="planetRadius">Planet radius in kilometers</param>
        /// <returns>Orbital eccentricity (0-1)</returns>
        /// <exception cref="ArgumentException">Thrown when altitudes are negative or apoapsis is less than periapsis</exception>
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

    /// <summary>
    /// Class that counts evolution of orbit in real time
    /// </summary>
	/// <remarks>calculates position and other stuff after N seconds</remarks>
	public class OrbitalPropagator
    {
        
    }
}