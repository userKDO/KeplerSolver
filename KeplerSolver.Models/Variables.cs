using System.Numerics;

namespace PublicVariables
{
	/// <summary>
    /// Class containing constants
    /// </summary>
	public static class Constants // Class for Constants
	{
		/// <summary>
        /// Constant for converting meters to kilometers and vice versa
        /// </summary>
		public const double MetersInKilometer = 1000;

		/// <summary>
        /// Constant for converting Minutes to seconds and vice versa
        /// </summary>
		public const double MinSecs = 60;
	}

	/// <summary>
    /// Class containing planet parameters
    /// </summary>
	public class PlanetVariables // Class for Planet's variables
	{
		/// <summary>
        /// Just the name of the planet
        /// </summary>
		/// <value>Any string value</value>
		public string Name { get; set; }

		/// <summary>
        /// Radius of the planet
        /// </summary>
		/// <value>Any value in kilometers</value>
		public double Radius { get; set; } // km

		/// <summary>
        /// Mass of the planet
        /// </summary>
		/// <value>Any value in killograms</value>
		public double Mass { get; set; } // kg

		/// <summary>
        /// Gravitational parameter of the planet
        /// </summary>
		/// <value>Any double value in meters^3/seconds^2</value>
		public double GravitationalParameter { get; set; } // meters^3/seconds^2

		// Constructor for custom planets

		/// <summary>
        /// Constructor for the custom planets. Structure: (name, radius, mass, gravParam)
        /// </summary>
        /// <param name="name">Name of the planet(any string value)</param>
        /// <param name="radius">Radius of the planet(any double value in kilometers)</param>
        /// <param name="mass">Mass of the planet(any double value in killograms)</param>
        /// <param name="gravParam">Gravitational parameter of the planet(any double value in m^3/s^2)</param>
		public PlanetVariables(string name, double radius, double mass, double gravParam)
		{
			Name = name;
			Radius = radius;
			Mass = mass;
			GravitationalParameter = gravParam;
		}

		// Methods-presets

		/// <summary>
        /// Preset for the planet Earth
        /// </summary>
        /// <returns>Returns parameters of Earth</returns>
		public static PlanetVariables Earth() => new("Earth", 6371.0, 5.9722e24, 3.986004418e14);

		/// <summary>
        /// Preset for the planet Mars
        /// </summary>
        /// <returns>Returns parameters of Mars</returns>
		public static PlanetVariables Mars() => new("Mars", 3389.5, 6.4171e23, 4.282837e13);

		/// <summary>
        /// Preset for the planet Moon
        /// </summary>
        /// <returns>Returns parameters of Moon</returns>
		public static PlanetVariables Moon() => new("Moon", 1737.4, 7.342e22, 4.9048695e12);

		/// <summary>
        /// Calculates the gravitational parameter via mass. Formula: G * mass, where G is 6.67430e-11(gravitational constant)
        /// </summary>
        /// <param name="mass">Mass of the planet</param>
        /// <returns>Returns the gravitational parameter in meter^3/seconds^2</returns>
		public static double CalculateGravParam(double mass)
		{
			const double G = 6.67430e-11; // gravitational constant
			return G * mass;
		}
	}

	/// <summary>
    /// Class containing satellite parameters
    /// </summary>
	public class Satellite // Class for satellite's variables
	{
		private double _eccentricity;
		private double _inclination;

		/// <summary>
        /// Just the name of the satellite
        /// </summary>
		/// <value>Any name in string</value>
		/// <remarks>If Name is null => Name = 'Unnamed' by default, but it works only if you use AskSatelliteName() method</remarks>
		public string? Name { get; set; } // Satellite's name

		/// <summary>
        /// Altitude of the satellite above the planet
        /// </summary>
		/// <remarks>will determine the future flight speed - the higher the slower, the period of revolution around the earth and the field of view</remarks>
		/// <value>Any value in kilometers</value>
		public double Altitude { get; set; } // km above Earth (will determine the future flight speed - the higher the slower, the period of revolution around the earth and the field of view)

		// Working with degrees

		/// <summary>
        /// This is the slope of satellite(0-180, where 0° = equatorial orbit, 90° = polar orbit, 180° = retrograde orbit)
        /// </summary>
		/// <value>double value between 0 and 180</value>
		/// <exception cref="ArgumentException">Thrown when value is not in range 0-180 degrees</exception>
		public double Inclination
        {
            get => _inclination;
			set => _inclination = value >= 0 && value <= 180 ? value: throw new ArgumentException("Inclination must be 0-180°");
        } 	// degrees (0 - 180, where 0° = equatorial orbit, 90° = polar orbit, 180° = retrograde orbit)

		/// <summary>
        /// Orbital eccentricity (0 = circular, 0-1 = elliptical, 1 = parabolic)
        /// </summary>
		/// <value>Double value between 0 and 1</value>
		/// <exception cref="ArgumentException">Thrown when value is not in range [0,1)</exception>
		public double Eccentricity
        {
            get => _eccentricity;
        	set => _eccentricity = value >= 0 && value < 1 ? value : throw new ArgumentException("Eccentricity must be 0-1");
        } 	// (0-1) - 0=circular, 0-1=elliptical, 1=parabolic

		/// <summary>
        /// Anomaly of satellite in degrees. Shows where the satellite is currently in its orbit
        /// </summary>
		/// <value>Any value within 360 degrees</value>
		public double CurrentAnomaly; // degrees (0 - 360, shows where the satellite is currently in its orbit)

		/// <summary>
        /// The time of a full revolution around the Planet
        /// </summary>
		/// <value>Any value in seconds</value>
		public double OrbitalPeriod; // seconds - the time of a full revolution around the Planet

		/// <summary>
        /// Orbital velocity of the satellite
        /// </summary>
		/// <value>Any value in meters per second</value>
		public double OrbitalVelocity; // meters per second

		/// <summary>
        /// Angular velocity of the satellite
        /// </summary>
		/// <value>Any value in degrees per second</value>
		public double AngularVelocity; // degrees/second

		/// <summary>
        /// SemiMajorAxis - half of longest diameter of ellipse
        /// </summary>
		/// <value>Any value in kilometers</value>
		public double SemiMajorAxis; // km - half of longest diameter of ellipse

		/// <summary>
        /// ArgumentOfPeriapsis - orientation of ellipse in orbital plane
        /// </summary>
		/// <value>Any value within 360 degrees</value>
		public double ArgumentOfPeriapsis; // degrees (0-360) - orientation of ellipse in orbital plane

		/// <summary>
        /// The object of the class OrbitType
        /// </summary>
		/// <value>Circular, Elliptical, Geostationary, Polar, Molniya</value>
		public OrbitType OrbitType { get; set; }

		/// <summary>
        /// Constructor for Satellite object. Very useful for test in my opinion
        /// </summary>
		/// <remarks>You can use this constructor for your own testing if you want to add some features yourself.</remarks>
		/// <example>
		/// <code>
		/// // Create a circular orbit satellite for your test
		/// var satellite = new Satellite(
		///     name: "ISS",
		///     altitude: 400,
		///     inclination: 51.6,
		///     eccentricity: 0,
		///     currentAnomaly: 0,
		///     orbitType: OrbitType.Circular
		/// );
		/// </code>
		/// </example>
        /// <param name="name"></param>
        /// <param name="altitude"></param>
        /// <param name="inclination"></param>
        /// <param name="eccentricity"></param>
        /// <param name="currentAnomaly"></param>
        /// <param name="orbitType"></param>
        /// <param name="semiMajorAxis"></param>
        /// <param name="argumentOfPeriapsis"></param>
        /// <param name="orbitalPeriod"></param>
        /// <param name="orbitalVelocity"></param>
        /// <param name="angularVelocity"></param>
		public Satellite(string? name, double altitude, double inclination, 
                double eccentricity, double currentAnomaly, OrbitType orbitType,
                double semiMajorAxis = 0, double argumentOfPeriapsis = 0,
                double orbitalPeriod = 0, double orbitalVelocity = 0, 
                double angularVelocity = 0) // Ayo, thats looks like shit
        {
            Name = name;
			Altitude = altitude;
			Inclination = inclination;
			Eccentricity = eccentricity;
			CurrentAnomaly = currentAnomaly;
			OrbitType = orbitType;
			SemiMajorAxis = semiMajorAxis;
			ArgumentOfPeriapsis = argumentOfPeriapsis;
			OrbitalPeriod = orbitalPeriod;
			OrbitalVelocity = orbitalVelocity;
			AngularVelocity = angularVelocity;
        }

		/// <summary>
        /// Empty constructor of Satellite object to solve some problems in UserGUI
        /// </summary>
		/// <remarks>With this counstructor you can create empty objects and fill'em step by step</remarks>
		/// <example>
		/// <code>
		/// // Create empty satellite and fill properties gradually
		/// var satellite = new Satellite();
		/// satellite.Name = "Hubble";
		/// satellite.Altitude = 547;
		/// satellite.Inclination = 28.5;
		/// </code>
		/// </example>
		public Satellite()
        {
            
        }
	}

	/// <summary>
    /// enum of orbit types
    /// </summary>
	/// <remarks>Contains: Circular, Elliptical, Geostationary, Polar, Molniya orbits</remarks>
	public enum OrbitType
    {
		/// <summary>Circular orbit with constant altitude</summary>
        Circular,
		/// <summary>Elliptical orbit with varying altitude</summary>
		Elliptical,
		/// <summary>Geostationary orbit at 35,786 km altitude</summary>
		Geostationary,
		/// <summary>Polar orbit with 90° inclination</summary>
		Polar,
		/// <summary>Molniya highly elliptical orbit</summary>
		Molniya
    }

	/// <summary>
    /// Structure for all orbital states
    /// </summary>
	public struct OrbitalState
    {
		/// <summary>Vector for position</summary>
		/// <value>Meters</value>
        public Vector3 Position;

		/// <summary>Vector for velocity</summary>
		/// <value>Meters per second</value>
		public Vector3 Velocity;

		/// <summary>True anomaly in OrbitalState</summary>
		/// <value>Degrees</value>
		public double TrueAnomaly;

		/// <summary>Time in OrbitalState</summary>
		/// <value>Seconds</value>
		public double Time;

		//Constructors

		/// <summary>
        /// Constructor for OrbitalState class
		/// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="trueAnomaly"></param>
        /// <param name="time"></param>
		public OrbitalState(Vector3 position, Vector3 velocity, double trueAnomaly, double time)
        {
            Position = position;
			Velocity = velocity;
			TrueAnomaly = trueAnomaly;
			Time = time;
        }
    }
}