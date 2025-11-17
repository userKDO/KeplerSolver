using System;
using PublicVariables;
using SatelliteMath;

namespace GUI
{
	public static class UserGUI
	{
		public static void MainMenu()
		{
			while (true)
			{
				Console.WriteLine($"===Welcome to my 'program' KeplerSolver!===\nSelect a function by writing its number");
				Console.WriteLine("6.Delete satellite");
				Console.WriteLine("5.List all satellites  ");
				Console.WriteLine("4.Select existing satellite");
				Console.WriteLine("3.Create new satellite");
				Console.WriteLine("2.Calculate the orbital velocity");
				Console.WriteLine("1.Calculate the orbital period via height");
				Console.WriteLine("0.exit");
				Console.Write("Your choice: ");

				var choice = Console.ReadLine();
				switch (choice)
				{
					case "0":
						return;
					case "1":
						CalculOrbPeriodViaHeightGUI();
						break;
					case "2":
						CalculateOrbVelocity();
						break;
					default:
						Console.WriteLine("{choice} is probably not a command. If u think that command is correct, problem might be on program's side");
						break;
				}
			}
		}
		static void CalculOrbPeriodViaHeightGUI()
		{
			PlanetVariables ChosenPlanet = AskPlanet();
			string UserChosenName = AskSatteliteName();
			double UserChosenAltitude = AskAltitude();
			double UserChosenInclination = AskInclination();
			double UserChosenCurrentAnomaly = AskCurrentAnomaly();

			var iss = new Satellite
			{
				Name = UserChosenName,
				Altitude = UserChosenAltitude,
				Inclination = UserChosenInclination,
				CurrentAnomaly = UserChosenCurrentAnomaly
			};

			double periodseconds = SatelliteMath.OrbitalCalculator.OrbitalPeriodviaHeight(iss, ChosenPlanet);
			iss.OrbitalPeriod = periodseconds ; // iss.OrbitalPeriod can be used later
			double periodminutes = periodseconds * Constants.MinSecs;
			Console.WriteLine($"Orbital period for {iss.Name}: {periodminutes:F2} minutes\n");
		}

		static void CalculateOrbVelocity()
		{
			PlanetVariables ChosenPlanet = AskPlanet();
			string UserChosenName = AskSatteliteName();
			double UserChosenAltitude = AskAltitude();

			var iss = new Satellite
			{
				Name = UserChosenName,
				Altitude = UserChosenAltitude,
			};

			double orbVelocity = SatelliteMath.OrbitalCalculator.OrbitalVelocity(iss, ChosenPlanet);
			iss.OrbitalVelocity = orbVelocity;
			Console.WriteLine($"Orbital velocity for {iss.Name}: {orbVelocity:F2} meters per sec\n");
		}

		static void CalculateAngularVelocity()
        {
			PlanetVariables ChosenPlanet = AskPlanet();
			string UserChosenName = AskSatteliteName();


        } // NOT ENDED WORK

		static PlanetVariables AskPlanet()
		{
			PlanetVariables ChosenPlanet = PlanetVariables.Earth(); // Earth as default

			Console.Write("Pls enter your planet(Earth,Mars.Moon): ");
			string InputChosenPlanet = Console.ReadLine() ?? "Earth";
			switch (InputChosenPlanet.ToLower())
			{
				case "earth":
					ChosenPlanet = PlanetVariables.Earth();
					break;
				case "mars":
					ChosenPlanet = PlanetVariables.Mars();
					break;
				case "moon":
					ChosenPlanet = PlanetVariables.Moon();
					break;
				default:
					Console.WriteLine($"Unknown planet: {InputChosenPlanet}. Using Earth as default.");
					ChosenPlanet = PlanetVariables.Earth();
					break;
			}
			return ChosenPlanet;
		}

		// ask methods

		static string AskSatteliteName()
		{
			Console.Write($"Please enter Name of sattelite: ");
			string UserChosenName = Console.ReadLine() ?? "Unnamed";
			return UserChosenName;
		}

		static double AskAltitude()
		{
			Console.Write("Please enter Altitude: ");
			double UserChosenAltitude = SafeParseDouble(Console.ReadLine());
			return UserChosenAltitude;
		}

		static double AskInclination()
		{
			Console.Write("Please enter Inclination: ");
			double UserChosenInclination = SafeParseDouble(Console.ReadLine());
			return UserChosenInclination;
		}

		static double AskCurrentAnomaly()
		{
			Console.Write("Please enter Current anomaly(if you don't know just type 0): ");
			double UserChosenCurrentAnomaly = SafeParseDouble(Console.ReadLine());
			return UserChosenCurrentAnomaly;
		}

		private static double SafeParseDouble(string? input)
		{
			return double.TryParse(input, out double result) ? result : 0;
		}
	}
}