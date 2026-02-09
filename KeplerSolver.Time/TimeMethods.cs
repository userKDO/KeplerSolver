using System;
using SimulationTimeVariables;
using VectorStructs;

namespace GameTime
{
    public class Time
    {
        public static void TimeIncrease(double amount)
        {
            SimTime += amount;
        }

		public static void Step(ref State s, double dt, double mu)
		{
			Vector3d a = -mu * s.r / Math.Pow(s.r.Length(), 3);
			s.v += a * dt;
			s.r += s.v * dt;
		}
    }
}
