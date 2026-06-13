using logger;

namespace TEST_GUI
{
    public static class TestGUI
    {
        public static void MainMenu_TESTS()
        {
            while(true)
            {
                Console.WriteLine("Welcome to tester mode in my program KeplerSolver");
                Console.WriteLine("9.Read logs");
                Console.WriteLine("4.Test Polar Orbit");
                Console.WriteLine("3.Test Equatorial Orbit");
                Console.WriteLine("2.Test orbital velocity");
                Console.WriteLine("1.Test orbital period via height calculation");
                Console.WriteLine("0. exit");
                Console.Write("Your choice: ");

                var choice = Console.ReadLine();
                switch(choice)
                {
                    case "0":
                        return;
                    case "1":
                        MathTests.OrbitalTests.TestOrbitalPeriodViaHeight();
                        break;
                    case "2":
                        MathTests.OrbitalTests.TestOrbitalVelocity();
                        break;
                    case "3":
						VectorMathTests.VectorTests.EquatorialOrbit_ZeroZ();
						break;
					case "4":
						VectorMathTests.VectorTests.PolarOrbit_RotateToZAxis();
						break;
						
					case "9":
						var logger = new SimpleLogger();
						logger.LogRead();
						break;
                }
            }
        }
    }
}
