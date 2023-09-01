using System;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;

namespace Console_Satellite_Tracker {
    class DataLoader {
        private const string DATA_FILE_PATH = "data.txt";

        private const string DATA_URI = "https://celestrak.org/NORAD/elements/gp.php?GROUP=active&FORMAT=tle";
        // private const string DATA_URI = "https://celestrak.org/NORAD/elements/gp.php?GROUP=noaa&FORMAT=tle";
        private static readonly TimeSpan DATA_UP_TO_DATE_PERIOD = new TimeSpan(24, 0, 1);



        private static string downloadData() {
            HttpClient client = new HttpClient();
            string data = client.GetStringAsync(DATA_URI).Result;

            return data;
        }

        private static void updateDataFile(string data) {
            using (StreamWriter writer = new StreamWriter(DATA_FILE_PATH)) {
                writer.Write(data);
            }
        }

        private static string readDataFile() {
            using (StreamReader reader = new StreamReader(DATA_FILE_PATH)) {
                return reader.ReadToEnd();
            }
        }

        private static Dictionary<int, SatelliteInfo> parseData(string data) {
            string[] lines = data.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );  

            Dictionary<int, SatelliteInfo> infos = new Dictionary<int, SatelliteInfo>();

            for (int i = 0; i < lines.Length / 3; i++) {
                SatelliteInfo satellite = SatelliteInfo.Parse(lines[3 * i], lines[3 * i + 1], lines[3 * i + 2]);
                infos.Add(satellite.catalogNumber, satellite);
            }

            return infos;
        }

        private static bool isDataUpToDate() {
            return DateTime.Now - File.GetLastWriteTime(DATA_FILE_PATH) < DATA_UP_TO_DATE_PERIOD;
        }

        // Returns Dictionary containing satellite information with the keys being their NORAD codes 
        public static Dictionary<int, SatelliteInfo> Load() {
            string rawData;
            if (!isDataUpToDate()) {
                rawData = downloadData();
                updateDataFile(rawData);
            }
            else rawData = readDataFile();

            return parseData(rawData);
        }
    }
}
