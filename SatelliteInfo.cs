using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Satellite_Tracker {
    struct SatelliteInfo {
        public string name;

        public int catalogNumber;
        public float inclination;
        public float raan;
        public float ecc;
        public float argument;
        public float meanAnomaly;
        public float meanMotion;

        public DateTime time;

        public static SatelliteInfo Parse(string name, string line1, string line2) {
            SatelliteInfo info = new SatelliteInfo();

            info.name = name;

            // 1st line
            info.catalogNumber = int.Parse(line1.Substring(2, 5));
            int epochYear = int.Parse(line1.Substring(18, 2));
            float epochDay = float.Parse(line1.Substring(20, 12));

            if (epochYear < 57)
                epochYear += 2000;
            else epochYear += 1900;

            info.time = new DateTime(epochYear, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            info.time = info.time.AddDays(epochDay - 1.0f);

            // 2nd line
            info.inclination = float.Parse(line2.Substring(8, 8)) * MathF.PI / 180;
            info.raan = float.Parse(line2.Substring(17, 8)) * MathF.PI / 180;
            info.ecc = float.Parse("0." + line2.Substring(26, 7));
            info.argument = float.Parse(line2.Substring(34, 8)) * MathF.PI / 180;
            info.meanAnomaly = float.Parse(line2.Substring(43, 8)) * MathF.PI / 180;
            info.meanMotion = float.Parse(line2.Substring(52, 11)) * MathF.Tau;

            return info;
        }

        public void Print() {
            Console.WriteLine($"{name} [{catalogNumber}] ({time.ToUniversalTime()} UTC)");
            Console.WriteLine($"Inclination:  {inclination * 180.0f / MathF.PI} deg");
            Console.WriteLine($"RAAN:         {raan * 180.0f / MathF.PI} deg");
            Console.WriteLine($"Eccentricity: {ecc}");
            Console.WriteLine($"AoP:          {argument * 180.0f / MathF.PI} deg");
            Console.WriteLine($"Mean anomaly: {meanAnomaly * 180.0f / MathF.PI} deg");
            Console.WriteLine($"Mean motion:  {meanMotion * 180.0f / MathF.PI} deg/day");
        }

        public override string ToString()
        {
            return $"{name} [{catalogNumber}] ({time})";
        }
    }
}
