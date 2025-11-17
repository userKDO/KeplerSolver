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
		public string? Name;// Satellite's name

		public double Altitude; // km above Earth (will determine the future flight speed - the higher the slower, the period of revolution around the earth and the field of view)

		// Working with degrees
		public double Inclination; // degrees (0 - 180, where 0° = equatorial orbit, 90° = polar orbit, 180° = retrograde orbit)
		public double CurrentAnomaly; // degrees (0 - 360, shows where the satellite is currently in its orbit)

		public double OrbitalPeriod; // seconds - the time of a full revolution around the Planet
		public double OrbitalVelocity; // meters per second

		public double AngularVelocity; // degres/second
	}
}