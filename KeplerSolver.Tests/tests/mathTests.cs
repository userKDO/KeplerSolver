using PublicVariables;
using SatelliteMath;

namespace MathTests
{
    class OrbitalTests
    {
        public static void TestOrbitalPeriodViaHeight()
        {
            Console.WriteLine("Testing OrbitalPeriodViaHeight:");
            try
            {
                var satellite = new Satellite("TestCircular", 400, 51.6, 0, 0, OrbitType.Circular);
                var period = OrbitalCalculator.OrbitalPeriodviaHeight(satellite, PlanetVariables.Earth());
                //Console.WriteLine($"Circular 400km: {period/60:F1} min (expected: ~92.5 min)");
                var expectedPeriod = 92.5 * 60;
                var tolerance = 60;
                Console.WriteLine($"Real period: {period/60:F1} minutes");
                if (Math.Abs(period - expectedPeriod) < tolerance)
                {
                    Console.WriteLine($"1st test: Passed (period: {period/60:F1} diapason: 91.5 - 93.5 minutes)");
                }
                else
                {
                    Console.WriteLine("1st test: Failed(period not in allowed diapason)");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"1st test: Failed({ex.Message})");
            }

            try
            {
                var satellite = new Satellite("TestElliptical", 0, 45, 0.1, 0, OrbitType.Elliptical, 7571);
                var period = OrbitalCalculator.OrbitalPeriodviaHeight(satellite, PlanetVariables.Earth());
                var expectedPeriod = 109.3*60;
                var tolerance = 5;
                Console.WriteLine($"Real period: {period/60:F1} minutes");
                if (Math.Abs(period - expectedPeriod) < tolerance)
                {
                    Console.WriteLine($"2nd test: Passed(period: {period/60:F1}; diapason: {expectedPeriod/60-0.5:F1} - {expectedPeriod/60+0.5:F1} minutes)");
                }
                else
                {
                    Console.WriteLine($"2nd test: Failed(period: {period/60:F1} not in allowed diapason {expectedPeriod/60-0.5:F1}-{expectedPeriod/60+0.5:F1} minutes)");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"2nd test: Failed({ex.Message})");
            }

            try
            {
                var satellite = new Satellite("TestZero", 0, 0, 0, 0, OrbitType.Circular);
                var period = OrbitalCalculator.OrbitalPeriodviaHeight(satellite, PlanetVariables.Earth());
                Console.WriteLine("3rd test: Failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"3rd test: Passed({ex.Message})");
            }

            try
            {
                var satellite = new Satellite("TestError", 0, 0, 0.5, 0, OrbitType.Elliptical);
                var period = OrbitalCalculator.OrbitalPeriodviaHeight(satellite, PlanetVariables.Earth());
                Console.WriteLine("4th test: Failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"4th test: Passed({ex.Message})");
            }
        }

        public static void TestOrbitalVelocity()
        {
            Console.WriteLine("Testing Orbital Velocity:");

            try
            {
                var satellite = new Satellite("TestCircular", 400, 51.6, 0, 0, OrbitType.Circular);
                var velocity = OrbitalCalculator.OrbitalVelocity(satellite, PlanetVariables.Earth());
                var expectedVelocity = PlanetVariables.Earth().Radius + satellite.Altitude;
                if (velocity <= expectedVelocity + 50 || velocity >= expectedVelocity - 50)
                {
                    Console.WriteLine($"1st test: Passed(velocity = {velocity}, expectedVelocity = {expectedVelocity} +- 50)");
                }
                else
                {
                    Console.WriteLine($"1st test: Failed(velocity = {velocity}, expectedVelocity = {expectedVelocity} +- 50)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"1st test: Failed({ex.Message})");
            }

            /*try
            {
                // Orbit with apoapsis 1000.0 kilometers and periapsis 400.0 kilometers
                double apoapsisAlt = 1000.0; // kilometers
                double periapsisAlt = 400.0; //kilometers
                // calculates orbit parameters
                double a = OrbitalCalculator.CalculateSemiMajorAxis(periapsisAlt, apoapsisAlt, PlanetVariables.Earth().Radius);
                double e = OrbitalCalculator.CalculateEccentricity(periapsisAlt, apoapsisAlt, PlanetVariables.Earth().Radius);
                var satellite = new Satellite("TestEllipticalOrbit", 0, 51.6, e, 0, OrbitType.Elliptical, a);
                var PeriapsisVelocity = OrbitalCalculator.OrbitalVelocity(satellite, PlanetVariables.Earth(), 0);
                var velocityApoapsis = OrbitalCalculator.OrbitalVelocity(satellite, PlanetVariables.Earth(),0);

                var ExpectedVelocityPeriapsis = ;
                var ExpectedVelocityApoapsis = ;

                if ()
                {
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"2nd test: Failed({ex.Message})");
            }*/
        }
    }
}