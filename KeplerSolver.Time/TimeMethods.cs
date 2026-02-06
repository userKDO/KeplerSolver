using System;
using SimulationTimeVariables;

namespace GameTime
{
    public class Time
    {
        public static void TimeIncrease(double amount)
        {
            SimTime += amount;
        }
    }
}