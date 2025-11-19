using System;
using PublicVariables;

namespace DataBases
{
    public static class SatelliteDataBase
    {
        private static List<Satellite> _satellites = new();

        public static void AddSatellite(Satellite satellite) // added to gui
        {
            _satellites.Add(satellite);
        }

        public static Satellite? GetSatellite(string name) // added to gui
        {
            return _satellites.Find(s => s.Name == name);
        }

        public static List<Satellite> GetAllSatellites() // added to gui
        {
            return new List<Satellite>(_satellites); // returning copy
        }

        public static bool RemoveSatellite(string name) // added to gui
        {
            var satellite = GetSatellite(name);
            if (satellite != null)
            {
                return _satellites.Remove(satellite);
            }
            return false;
        }

        public static bool SatelliteExists(string name) // added to gui
        {
            return _satellites.Any(s=> s.Name == name);
        }   

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
                existing.Altitude = updatedSatellite.Altitude;
                existing.Inclination = updatedSatellite.Inclination;
                existing.OrbitalPeriod = updatedSatellite.OrbitalPeriod;
                existing.OrbitalVelocity = updatedSatellite.OrbitalVelocity;
                existing.AngularVelocity = updatedSatellite.AngularVelocity;
            }
        }
    }
}