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
                double R = PlanetVariables.Earth().Radius * Constants.MetersInKilometer;
                double h = satellite.Altitude * Constants.MetersInKilometer;
                double r = R + h;
                double μ = PlanetVariables.Earth().GravitationalParameter;
                double expectedVelocity = Math.Sqrt(μ / r); // v = √(μ/r)

                double tolerance = 1.0; // m/s
                double difference = Math.Abs(velocity - expectedVelocity);

                if (difference < tolerance)
                {
                    Console.WriteLine($"1st test: Passed");
                }
                else
                {
                    Console.WriteLine($"1st test: Failed (outside tolerance of {tolerance} m/s)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"1st test: Failed({ex.Message})");
            }

            try
            {
                // Orbit with apoapsis 1000.0 kilometers and periapsis 400.0 kilometers
                double apoapsisAlt = 1000.0; // kilometers
                double periapsisAlt = 400.0; //kilometers
                // calculates orbit parameters
                double a = OrbitalCalculator.CalculateSemiMajorAxis(periapsisAlt, apoapsisAlt, PlanetVariables.Earth().Radius);
                double e = OrbitalCalculator.CalculateEccentricity(periapsisAlt, apoapsisAlt, PlanetVariables.Earth().Radius);
                var satellite = new Satellite("TestEllipticalOrbit", 0, 51.6, e, 0, OrbitType.Elliptical, a);

                var PeriapsisVelocity = OrbitalCalculator.OrbitalVelocity(satellite, PlanetVariables.Earth(), 0);
                var ApoapsisVelocity = OrbitalCalculator.OrbitalVelocity(satellite, PlanetVariables.Earth(),180);

                double R = PlanetVariables.Earth().Radius * Constants.MetersInKilometer;
                double r_peri = R + periapsisAlt * Constants.MetersInKilometer;
                double r_apo = R + apoapsisAlt * Constants.MetersInKilometer;
                double a_m = a * Constants.MetersInKilometer;

                // Use vis-viva formula: v = √[μ(2/r - 1/a)]
                double expectedPeriApsisVel = Math.Sqrt(PlanetVariables.Earth().GravitationalParameter * (2.0 / r_peri - 1.0 / a_m));
                double expectedApoApsisVel = Math.Sqrt(PlanetVariables.Earth().GravitationalParameter * (2.0 / r_apo - 1.0 / a_m));

                double tolerance = 1.0;

                bool periCorrect = Math.Abs(PeriapsisVelocity - expectedPeriApsisVel) < tolerance;
                bool apoCorrect = Math.Abs(ApoapsisVelocity - expectedApoApsisVel) < tolerance;
                bool periFasterThanApo = PeriapsisVelocity > ApoapsisVelocity; // Should be true

                if (periCorrect && apoCorrect && periFasterThanApo)
                {
                    Console.WriteLine($"2nd test: Passed");
                }
                else
                {
                    Console.WriteLine($"2nd test: FAILED - Issues:");
                    if (!periCorrect) Console.WriteLine($"Periapsis velocity mismatch");
                    if (!apoCorrect) Console.WriteLine($"Apoapsis velocity mismatch");
                    if (!periFasterThanApo) Console.WriteLine($"Periapsis not faster than apoapsis");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"2nd test: Failed({ex.Message})");
            }
            try
            {
                var satellite = new Satellite("TestLessThanZeroAltitude", -1, 51.6, 0, 0, OrbitType.Circular); 
                var velocity = OrbitalCalculator.OrbitalVelocity(satellite, PlanetVariables.Earth());
                Console.WriteLine($"3rd test: Failed(velocity: {velocity}. How?)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"3rd test: Passed({ex.Message})");
            }
        }
    }
}