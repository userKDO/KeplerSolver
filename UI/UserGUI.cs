using System;
using PublicVariables;
using SatelliteMath;
using DataBases;
using System.Security.AccessControl;

namespace GUI
{
	public static class UserGUI
	{
		public static void MainMenu()
		{
			while (true)
			{
				Console.WriteLine("===Welcome to my 'program' KeplerSolver!===\nSelect a function by writing its number");
				Console.WriteLine("7.Delete satellite");
				Console.WriteLine("6.List all satellites");
				Console.WriteLine("5.Select existing satellite");
				Console.WriteLine("4.Create new satellite");
				Console.WriteLine("3.Calculate the angular velocity");
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
					case "3":
						CalculateAngularVelocity();
						break;
					case "4":
						GUI_CreateSatellite();
						break;
					case "5":
						GUI_GetSatellite();
						break;
					case "6":
						GUI_GetAllSatellites();
						break;
					case "7":
						GUI_DeleteSatellite();
						break;
					default:
						Console.WriteLine($"{choice} is probably not a command. If u think that command is correct, problem might be on program's side, idk, my code is shit dude");
						break;
				}
			}
		}
		// Orbital stuff
		static void CalculOrbPeriodViaHeightGUI()
		{
			Satellite? satellite = ChooseSatellite();
			if (satellite == null)
            {
                Console.WriteLine("No satellite selected");
				return;
            }

			 if (satellite.OrbitType == OrbitType.Circular && satellite.Altitude == 0)
			{
				Console.WriteLine("Satellite altitude not set.");
				satellite.Altitude = AskAltitude();
			}
			
			if (satellite.OrbitType == OrbitType.Circular && satellite.Inclination == 0 )
			{
				Console.WriteLine("Satellite inclination not set.");  
				satellite.Inclination = AskInclination();
			}

			PlanetVariables planet = AskPlanet();
			double periodSeconds = SatelliteMath.OrbitalCalculator.OrbitalPeriodviaHeight(satellite, planet);
			satellite.OrbitalPeriod = periodSeconds;
			
			double periodMinutes = periodSeconds / 60;
			Console.WriteLine($"Orbital period for {satellite.Name}: {periodMinutes:F2} minutes\n");

			DataBases.SatelliteDataBase.UpdateSatellite(satellite);
		}

		static void CalculateOrbVelocity()
		{
			Satellite? satellite = ChooseSatellite();

			if (satellite == null)
            {
                Console.WriteLine("No satellite selected");
				return;
            }

			 if (satellite.OrbitType == OrbitType.Circular && satellite.Altitude == 0)
			{
				Console.WriteLine("Satellite altitude not set.");
				satellite.Altitude = AskAltitude();
				DataBases.SatelliteDataBase.UpdateSatellite(satellite);
			}

			PlanetVariables planet = AskPlanet();
			double orbVelocity = SatelliteMath.OrbitalCalculator.OrbitalVelocity(satellite, planet);
			satellite.OrbitalVelocity = orbVelocity;
			
			Console.WriteLine($"Orbital velocity for {satellite.Name}: {orbVelocity:F2} m/s");
			Console.WriteLine($"This is {orbVelocity * 3.6:F2} km/h\n");

			DataBases.SatelliteDataBase.UpdateSatellite(satellite);
		}

		static void CalculateAngularVelocity()
        {
			Satellite? satellite = ChooseSatellite();
			if (satellite == null)
            {
                Console.WriteLine("No satellite selected.");
        		return;
            }

			PlanetVariables planet = AskPlanet();
			Console.WriteLine("Calculate: 1. Mean angular velocity  2. Instantaneous angular velocity");
			var choice = Console.ReadLine();

			double angularVelocity;

			if (choice == "2")
			{
				Console.Write("Enter true anomaly (degrees): ");
				double trueAnomaly = SafeParseDouble(Console.ReadLine());
				angularVelocity = OrbitalCalculator.InstantaneousAngularVelocity(satellite, planet, trueAnomaly);
				Console.WriteLine($"Instantaneous angular velocity: {angularVelocity:F6} °/sec");
			}
			else
			{
				if (satellite.OrbitalPeriod == 0)
				{
					satellite.OrbitalPeriod = OrbitalCalculator.OrbitalPeriodviaHeight(satellite, planet);
				}
				angularVelocity = OrbitalCalculator.AngularVelocity(satellite);
				Console.WriteLine($"Mean angular velocity: {angularVelocity:F6} °/sec");
			}
			
			satellite.AngularVelocity = angularVelocity;
			SatelliteDataBase.UpdateSatellite(satellite);
        }
		
		// Data bases

		// SatelliteDataBase:
		static Satellite GUI_CreateSatellite()
		{
			while (true)
			{
				string UserChosenName = AskSatelliteName();
				Console.WriteLine($"Your name of satellite is: {UserChosenName}, right?");
				Console.WriteLine("1.Yes/2.No: ");
				var choice = Console.ReadLine();
				switch (choice)
				{
					case "1":
						OrbitType orbitType = AskOrbitType();
						
						var satellite = new Satellite 
						{
							Name = UserChosenName,
							//Altitude = AskAltitude(),
							//Inclination = AskInclination(),
							//CurrentAnomaly = AskCurrentAnomaly(),
							OrbitType = orbitType
						};

						switch (orbitType)
                        {
                            case OrbitType.Circular:
								satellite.Altitude = AskAltitude();
								satellite.Inclination = AskInclination();
								satellite.CurrentAnomaly = AskCurrentAnomaly();
								satellite.Eccentricity = 0;
								break;
							case OrbitType.Elliptical:
								satellite.Inclination = AskInclination();
                        		satellite.CurrentAnomaly = AskCurrentAnomaly();

								double periapsis = AskPeriapsis();
								double apoapsis = AskApoapsis();
								satellite.ArgumentOfPeriapsis = AskArgumentOfPeriapsis();

								PlanetVariables planet = AskPlanet();
								satellite.SemiMajorAxis = SatelliteMath.OrbitalCalculator.CalculateSemiMajorAxis(periapsis, apoapsis, planet.Radius);
								satellite.Eccentricity = SatelliteMath.OrbitalCalculator.CalculateEccentricity(periapsis, apoapsis, planet.Radius);

								satellite.OrbitalPeriod = OrbitalCalculator.OrbitalPeriodviaHeight(satellite, planet);
								break;
							case OrbitType.Geostationary:
								satellite.Altitude = 35786; // Geostationary's altitude
								satellite.Inclination = 0;
								satellite.Eccentricity = 0;
								satellite.CurrentAnomaly = 0;
								Console.WriteLine("Geostationary orbit parameters set automatically");
								break;
                        }

						DataBases.SatelliteDataBase.AddSatellite(satellite); // RETURNING OBJECT, NOT A STRING
						Console.WriteLine($"{UserChosenName} added into a database");
                		return satellite;
					case "2":
						break;
					default:
						Console.WriteLine($"Wrong answer: {choice}");
						break;
				}
			}
		}

		static void GUI_GetAllSatellites()
		{
			var satellites = DataBases.SatelliteDataBase.GetAllSatellites();
			foreach (var sat in satellites)
			{
				Console.WriteLine($"\n=== {sat.Name} ===");
				Console.WriteLine($"Orbit type: {sat.OrbitType}");
				
				if (sat.OrbitType == OrbitType.Circular || sat.OrbitType == OrbitType.Geostationary)
				{
					Console.WriteLine($"Altitude: {sat.Altitude} km");
				}
				
				if (sat.OrbitType == OrbitType.Elliptical)
				{
					Console.WriteLine($"Semi-major axis: {sat.SemiMajorAxis:F0} km");
					Console.WriteLine($"Eccentricity: {sat.Eccentricity:F3}");
					Console.WriteLine($"Argument of periapsis: {sat.ArgumentOfPeriapsis}°");
				}
				
				Console.WriteLine($"Inclination: {sat.Inclination}°");
				Console.WriteLine($"Current anomaly: {sat.CurrentAnomaly}°");
				
				// Рассчитанные параметры (если есть)
				if (sat.OrbitalPeriod > 0)
					Console.WriteLine($"Orbital period: {sat.OrbitalPeriod / 60:F1} min");
				if (sat.OrbitalVelocity > 0)
					Console.WriteLine($"Orbital velocity: {sat.OrbitalVelocity:F0} m/s");
				if (sat.AngularVelocity > 0)
					Console.WriteLine($"Angular velocity: {sat.AngularVelocity:F4} °/s");
				
				Console.WriteLine("---");
			}
			
			if (satellites.Count == 0)
			{
				Console.WriteLine("No satellites in database");
			}
		}

		static void GUI_GetSatellite()
		{
			while (true)
			{
				string UserChosenName = AskSatelliteName();
				Console.WriteLine($"Your name of satellite is: {UserChosenName}, right?");
				Console.WriteLine("1.Yes/2.No: ");
				var choice = Console.ReadLine();
				switch (choice)
				{
					case "1":
						Satellite? foundSatellite = DataBases.SatelliteDataBase.GetSatellite(UserChosenName);

						if (foundSatellite != null)
						{
							Console.WriteLine($"Satellite found: {foundSatellite.Name}");
							Console.WriteLine($"Altitude: {foundSatellite.Altitude}");
							Console.WriteLine($"Inclination: {foundSatellite.Inclination}");
							Console.WriteLine($"CurrentAnomaly: {foundSatellite.CurrentAnomaly}");
							Console.WriteLine($"OrbitalPeriod: {foundSatellite.OrbitalPeriod}");
							Console.WriteLine($"OrbitalVelocity: {foundSatellite.OrbitalVelocity}");
							Console.WriteLine($"AngularVelocity: {foundSatellite.AngularVelocity}");
						}
						else
                		{
                    		Console.WriteLine($"Satellite '{UserChosenName}' not found in database");
                		}
						return;
					case "2":
						break;
					default:
						Console.WriteLine($"Wrong answer: {choice}");
						break;
				}
			}
		}

		static Satellite? GUI_WORK_GetSatellite()
        {
            while (true)
			{
				string UserChosenName = AskSatelliteName();
				Console.WriteLine($"Your name of satellite is: {UserChosenName}, right?");
				Console.WriteLine("1.Yes/2.No:");
				var choice = Console.ReadLine();
				switch (choice)
				{
					case "1":
						Satellite? foundSatellite = DataBases.SatelliteDataBase.GetSatellite(UserChosenName);

						if (foundSatellite != null)
						{
							return foundSatellite;
						}
						else
                		{
                    		Console.WriteLine($"Satellite '{UserChosenName}' not found in database");
                		}
						break;
					case "2":
						break;
					default:
						Console.WriteLine($"Wrong answer: {choice}");
						break;
				}

				Console.WriteLine("Continue searching? (1.Yes/2.No)");
        		var continueChoice = Console.ReadLine();
        		if (continueChoice == "2") return null;
			}
        }

		static void GUI_DeleteSatellite()
		{
			string UserChosenName = AskSatelliteName();
			while (true)
			{
				Console.WriteLine($"Delete satellite: {UserChosenName}?");
				Console.WriteLine("1.Yes/2.No: ");
				var choice = Console.ReadLine();
				switch (choice)
				{
					case "1":
						bool removed = DataBases.SatelliteDataBase.RemoveSatellite(UserChosenName);
						
						if (removed)
						{
							Console.WriteLine($"Satellite '{UserChosenName}' successfully deleted");
						}
						else
						{
							Console.WriteLine($"Satellite '{UserChosenName}' not found in database");
						}
						return;
						
					case "2":
						return;
					default:
						Console.WriteLine($"Wrong answer: {choice}");
						break;
				}
			}
		}

		// ask methods

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

		static string AskSatelliteName()
		{
			Console.Write($"Please enter Name of satellite: ");
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

		static Satellite? ChooseSatellite()
        {
			while (true)
            {
                Console.WriteLine("You want to create new satellite or use existing?");
        		Console.WriteLine("1. Create new satellite");
        		Console.WriteLine("2. Use existing satellite"); 
        		Console.WriteLine("3. List all satellites");
        		Console.WriteLine("0. Back to menu");

				var choice = Console.ReadLine();
				switch (choice)
                {
                    case "1":
						return GUI_CreateSatellite();
					case "2":
						return GUI_WORK_GetSatellite();
					case "3":
						GUI_GetAllSatellites();
                		break;
					case "0":
						return null;
					default:
                		Console.WriteLine($"Invalid choice: {choice}");
                		break;
                }
            }
        }

		static OrbitType AskOrbitType()
        {
            while (true)
            {
                Console.WriteLine("Select orbit type:");
				Console.WriteLine("1. Circullar orbit");
				Console.WriteLine("2. Elliptical orbit");
				Console.WriteLine("3. Geostationary orbit");
				Console.WriteLine("4. Polar orbit");

				var choice = Console.ReadLine();
				switch (choice)
                {
                    case "1":
						return OrbitType.Circular;
					case "2":
						return OrbitType.Elliptical;
					case "3":
						return OrbitType.Geostationary;
					case "4":
						return OrbitType.Polar;
					default:
						Console.WriteLine($"Wrong answer: {choice}");
						break;
                }
            }
        }

		static double AskPeriapsis()
        {
            Console.WriteLine("Please, enter Periapsis altitude (km): ");
			return SafeParseDouble(Console.ReadLine());
        }

		static double AskApoapsis()
        {
            Console.WriteLine("Please, enter Apoapsis altitude (km): ");
			return SafeParseDouble(Console.ReadLine());
        }

		static double AskArgumentOfPeriapsis()
        {
            Console.WriteLine("Please enter Argument of periapsis (degrees): ");
			return SafeParseDouble(Console.ReadLine());
        }
	}
}