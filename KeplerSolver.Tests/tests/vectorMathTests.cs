using System;
using static MathStructs.VectorMath;
using logger;

namespace VectorMathTests
{
	public static class VectorTests
	{
		private const double Tolerance = 1e-9;
		private const double Deg2Rad = Math.PI / 180.0;
			
		public static void EquatorialOrbit_ZeroZ()
		{
			double xOrb = 700000;
			double yOrb = 300000;
				
			double inclination = 0;
			double omega = 0;
			double Omega = 0;
				
			var result = Vector3d.FromOrbitalPlane(xOrb, yOrb, inclination,Omega, omega);
				
			if (Math.Abs(result.Z) < Tolerance)
			{
				Console.WriteLine($"[PASSED] EquatorialOrbit_ZeroZ(got: {result.Z})");
				var logger = new SimpleLogger();
				logger.logTestPassed($"EquatorialOrbit_ZeroZ(got: {result.Z})");
			}
			else
			{
				Console.WriteLine($"[FAILED] EquatorialOrbit_ZeroZ: Expected Z ~ 0, got: {result.Z}");
				var logger = new SimpleLogger();
				logger.logTestFailed($"EquatorialOrbit_ZeroZ: Expected Z ~ 0, got: {result.Z}");
			}
		}
		
		public static void PolarOrbit_RotateToZAxis()
		{
			double xOrb = 0;
			double yOrb = 650000;
			double inclination = Math.PI / 2;
			double omega = 0;
			double Omega = 0;
			
			var result = Vector3d.FromOrbitalPlane(xOrb, yOrb, inclination,Omega, omega);
			
			bool isXZero = Math.Abs(result.X) < Tolerance;
			bool isYZero = Math.Abs(result.Y) < Tolerance;
			bool isZCorrect = Math.Abs(result.Z - yOrb) < Tolerance;
			
			if (isXZero && isYZero && isZCorrect)
			{
				Console.WriteLine($"[PASSED] PolarOrbit_RotateToZAxis(got: {result.X}, {result.Y}, {result.Z})");
				var logger = new SimpleLogger();
				logger.logTestPassed($"PolarOrbit_RotateToZAxis(got: {result.X}, {result.Y}, {result.Z})");
			}
			else
			{
				Console.WriteLine($"[FAILED] PolarOrbit_RotateToZAxis: Expected (0,0, {yOrb}), got {result.X}, {result.Y}, {result.Z}");
				var logger = new SimpleLogger();
				logger.logTestFailed($"PolarOrbit_RotateToZAxis: Expected (0,0, {yOrb}), got {result.X}, {result.Y}, {result.Z}");
			}
		}
	}
}
