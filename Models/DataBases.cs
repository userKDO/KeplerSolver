using System;
using PublicVariables;

namespace DataBases
{
    /// <summary>
    /// Class for database of satellites
    /// </summary>
    public static class SatelliteDataBase
    {
        /// <summary>
        /// List of satellites. Main database
        /// </summary>
        private static List<Satellite> _satellites = new();

        /// <summary>
        /// Method for adding satellites to the list
        /// </summary>
        /// <param name="satellite">Name of the satellite</param>
        public static void AddSatellite(Satellite satellite) // added to gui
        {
            _satellites.Add(satellite);
        }

        /// <summary>
        /// Method for searching satellite by name and getting his parameters
        /// </summary>
        /// <param name="name">Name of the satellite</param>
        /// <returns>Returns all parameters of the satellite</returns>
        public static Satellite? GetSatellite(string name) // added to gui
        {
            return _satellites.Find(s => s.Name == name);
        }

        /// <summary>
        /// "Method" for getting all satellites in list and all of their parameteres
        /// </summary>
        /// <returns>Returns copy of the main _satellites list</returns>
        public static List<Satellite> GetAllSatellites() // added to gui
        {
            return new List<Satellite>(_satellites); // returning copy
        }

        /// <summary>
        /// "Method" for deleting satellites from the database
        /// </summary>
        /// <param name="name">Name of the satellite</param>
        /// <returns>Returns false if satellite == null and deletes satellite from list if satellite != null</returns>
        public static bool RemoveSatellite(string name) // added to gui
        {
            var satellite = GetSatellite(name);
            if (satellite != null)
            {
                return _satellites.Remove(satellite);
            }
            return false;
        }

        /// <summary>
        /// "Method" for check if satellite exists in database
        /// </summary>
        /// <param name="name">Name of the satellite</param>
        /// <returns>Returns true if the list has one or more elements with entered name, and false if it is empty</returns>
        public static bool SatelliteExists(string name) // added to gui
        {
            return _satellites.Any(s=> s.Name == name);
        }   

        /// <summary>
        /// Method for updating data of the satellite
        /// </summary>
        /// <remarks>The most important method in this class. Do not forget to update your satellite's params in database if you changed something</remarks>
        /// <param name="updatedSatellite">Satellite object with updated parameters</param>
        public static void UpdateSatellite(Satellite updatedSatellite)
        {
            if (updatedSatellite?.Name == null) // null check
            {
                Console.WriteLine("Cannot update satellite: name is null");
                return;
            }

            var existing = GetSatellite(updatedSatellite.Name);
            if (existing != null)
            {
                if (updatedSatellite.Eccentricity >= 0 && updatedSatellite.Eccentricity < 1)
                {
                    existing.Eccentricity = updatedSatellite.Eccentricity;
                }
                
                if (updatedSatellite.Inclination >= 0 && updatedSatellite.Inclination <= 180)
                {
                    existing.Inclination = updatedSatellite.Inclination;
                }

                existing.Altitude = updatedSatellite.Altitude;
                existing.Inclination = updatedSatellite.Inclination;
                existing.OrbitalPeriod = updatedSatellite.OrbitalPeriod;
                existing.OrbitalVelocity = updatedSatellite.OrbitalVelocity;
                existing.AngularVelocity = updatedSatellite.AngularVelocity;
            }
        }
    }
}