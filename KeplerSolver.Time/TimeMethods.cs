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

		public static void Step(ref State s, double dt, double mu) // mu is a grav param, dt is a delta time, s is a object for State
		{
			Vector3d a = -mu * s.r / Math.Pow(s.r.Length(), 3); // acceleration
			s.v += a * dt; // updated speed
			s.r += s.v * dt; // updated position
		}
    }
}
