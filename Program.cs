using System;
using System.Globalization;

namespace Console_Satellite_Tracker {
    class Program {
        static void Main(string[] args) {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            var satellites = DataLoader.Load();

            /*    EXAMPLE NORAD CODES
             *    ISS - 25544
             *    NOAA 15 - 25338
             * 
             */

            // ISS
            SatelliteInfo issInfo = satellites[25544];
            Orbit issOrbit = new Orbit(issInfo);
            
            issInfo.Print();

            // Pass prediction                        |     COORDINATES      |
            GroundLocation ground = new GroundLocation(52.242734f, 21.014953f);
            
            DateTime closestTime = getClosestPassTime(ground, issOrbit);
            Console.WriteLine($"Closest pass in the next 24 hours: {closestTime} (UTC)");
        }


        private static DateTime getClosestPassTime(GroundLocation ground, Orbit satellite)
        {
            DateTime now = DateTime.UtcNow;
            float smallestDist = float.MaxValue;
            DateTime smallestDistTime = now;

            for (float t = .0f; t < 24 * 60 * 60; t += 10.0f) {
                DateTime time = now.AddSeconds(t);

                Coordinates groundPos = ground.GetCoordinates();
                Coordinates satPos = satellite.GetCoordinates(time);

                float distance = MathF.Pow(groundPos.latitude - satPos.latitude, 2) + MathF.Pow(groundPos.longitude - satPos.longitude, 2);
                if (distance<smallestDist) {
                    smallestDist = distance;
                    smallestDistTime = time;
                }
            }
            return smallestDistTime;
        }
    }
}
