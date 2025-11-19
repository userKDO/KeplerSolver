using System;
using PublicVariables;
using SatelliteMath;
using DataBases;

namespace GUI
{
	public static class UserGUI
	{
		public static void MainMenu()
		{
			while (true)
			{
				Console.WriteLine($"===Welcome to my 'program' KeplerSolver!===\nSelect a function by writing its number");
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
						Console.WriteLine("{choice} is probably not a command. If u think that command is correct, problem might be on program's side");
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

			 if (satellite.Altitude == 0)
			{
				Console.WriteLine("Satellite altitude not set.");
				satellite.Altitude = AskAltitude();
			}
			
			if (satellite.Inclination == 0)
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

			 if (satellite.Altitude == 0)
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

			if (satellite.OrbitalPeriod == 0)
            {
                PlanetVariables planet = AskPlanet();
        		satellite.OrbitalPeriod = OrbitalCalculator.OrbitalPeriodviaHeight(satellite, planet);

				double periodMinutes = satellite.OrbitalPeriod / 60;
        		Console.WriteLine($"Calculated orbital period: {periodMinutes:F2} minutes");
            }

			double angularVelocity = SatelliteMath.OrbitalCalculator.AngularVelocity(satellite);
			satellite.AngularVelocity = angularVelocity;
			Console.WriteLine($"Angular velocity: {angularVelocity:F6} °/sec");
			Console.WriteLine($"This is {angularVelocity * 60:F4} °/min");
			DataBases.SatelliteDataBase.UpdateSatellite(satellite);
        } // NOT ENDED WORK

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
						var satellite = new Satellite 
						{ 
							Name = UserChosenName,
							Altitude = AskAltitude(),
							Inclination = AskInclination(),
							CurrentAnomaly = AskCurrentAnomaly()
						};

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
				Console.WriteLine($" - {sat.Name} (Alt: {sat.Altitude}km, Inc: {sat.Inclination}°)");
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
			//Satellite? satellite = DataBases.SatelliteDataBase.GetSatellite(UserChosenName);
        }
	}
}