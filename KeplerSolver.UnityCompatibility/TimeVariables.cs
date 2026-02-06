using System.Numerics;

namespace SimulationTimeVariables
{
    /// <summary>
    /// Class containing GameTime
    /// </summary>
    public class SimulationTime
    {
        private double simTime;

        /// <summary>
        /// Just the time (for library)
        /// </summary>
        public double SimTime
        {
            get => simTime;
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException($"Time cannot be negative: {nameof(value)} = {value}");

                simTime = value;
            }
        } // seconds
    }
}