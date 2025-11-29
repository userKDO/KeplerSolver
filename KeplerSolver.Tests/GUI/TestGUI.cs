namespace TEST_GUI
{
    class TestGUI
    {
        public static void MainMenu_TESTS()
        {
            while(true)
            {
                Console.WriteLine("Welcome to tester mode in my program KeplerSolver");
                Console.WriteLine("1. Test orbital period via height calculation");
                Console.WriteLine("0. exit");

                var choice = Console.ReadLine();
                switch(choice)
                {
                    case "0":
                        return;
                    case "1":
                        MathTests.OrbitalTests.TestOrbitalPeriodViaHeight();
                        break;
                }
            }
        }
    }
}