using System;
using PublicVariables;

namespace DataBases
{
    public static class SatelliteDataBase
    {
        private static List<Satellite> _satellites = new();

        public static void AddSatellite(Satellite satellite)
        {
            _satellites.Add(satellite);
        }

        public static Satellite GetSatellite(string name)
        {
            return _satellites.Find(s => s.Name == name);
        }

        public static List<Satellite> GetAllSatellites()
        {
            return new List<Satellite>(_satellites); // returning copy
        }

        public static bool RemoveSatellite(string name)
        {
            var satellite = GetSatellite(name);
            if (satellite != null)
            {
                return _satellites.Remove(satellite);
            }
            return false;
        }

        public static bool SatelliteExists(string name)
        {
            return _satellites.Any(s=> s.Name == name);
        }   
    }
}