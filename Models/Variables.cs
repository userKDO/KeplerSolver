namespace PublicVariables
{
	public static class Constants // Class for Constants
	{
		public const double MetersInKilometer = 1000;
		public const double MinSecs = 60;
	}

	public class PlanetVariables // Class for Planet's variables
	{
		public string Name { get; set; }
		public double Radius { get; set; } // km
		public double Mass { get; set; } // kg
		public double GravitationalParameter { get; set; } // meters^3/seconds^2

		// Constructor for custom planets
		public PlanetVariables(string name, double radius, double mass, double gravParam)
		{
			Name = name;
			Radius = radius;
			Mass = mass;
			GravitationalParameter = gravParam;
		}

		// Методы пресеты
		public static PlanetVariables Earth() => new("Earth", 6371.0, 5.9722e24, 3.986004418e14);
		public static PlanetVariables Mars() => new("Mars", 3389.5, 6.4171e23, 4.282837e13);
		public static PlanetVariables Moon() => new("Moon", 1737.4, 7.342e22, 4.9048695e12);

		public static double CalculateGravParam(double mass)
		{
			const double G = 6.67430e-11; // gravitational constant
			return G * mass;
		}
	}

	public class Satellite // Class for satellite's variables
	{
		private double _eccentricity;
		private double _inclination;

		public string? Name { get; set; } // Satellite's name
		public double Altitude { get; set; } // km above Earth (will determine the future flight speed - the higher the slower, the period of revolution around the earth and the field of view)

		// Working with degrees
		public double Inclination
        {
            get => _inclination;
			set => _inclination = value >= 0 && value <= 180 ? value: throw new ArgumentException("Inclination must be 0-180°");
        } 	// degrees (0 - 180, where 0° = equatorial orbit, 90° = polar orbit, 180° = retrograde orbit)
		public double Eccentricity
        {
            get => _eccentricity;
        	set => _eccentricity = value >= 0 && value < 1 ? value : throw new ArgumentException("Eccentricity must be 0-1");
        } 	// (0-1) - 0=circular, 0-1=elliptical, 1=parabolic

		public double CurrentAnomaly; // degrees (0 - 360, shows where the satellite is currently in its orbit)

		public double OrbitalPeriod; // seconds - the time of a full revolution around the Planet
		public double OrbitalVelocity; // meters per second

		public double AngularVelocity; // degrees/second

		
		public double SemiMajorAxis; // km - half of longest diameter of ellipse
		public double ArgumentOfPeriapsis; // degrees (0-360) - orientation of ellipse in orbital plane

		public OrbitType OrbitType { get; set; }
	}

	public enum OrbitType
    {
        Circular,
		Elliptical,
		Geostationary,
		Polar,
		Molniya
    }
}