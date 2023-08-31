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
            float epochDay = float.Parse(line1.Substring(20, 12), CultureInfo.InvariantCulture);

            if (epochYear < 57)
                epochYear += 2000;
            else epochYear += 1900;

            info.time = new DateTime(epochYear, 1, 1, 0, 0, 0);
            info.time.AddDays(epochDay);

            // 2nd line
            info.inclination = float.Parse(line2.Substring(8, 8), CultureInfo.InvariantCulture);
            info.raan = float.Parse(line2.Substring(17, 8), CultureInfo.InvariantCulture);
            info.ecc = float.Parse("0." + line2.Substring(26, 7), CultureInfo.InvariantCulture);
            info.argument = float.Parse(line2.Substring(34, 8), CultureInfo.InvariantCulture);
            info.meanAnomaly = float.Parse(line2.Substring(43, 8), CultureInfo.InvariantCulture);
            info.meanMotion = float.Parse(line2.Substring(52, 11), CultureInfo.InvariantCulture);

            return info;
        }
    }
}
