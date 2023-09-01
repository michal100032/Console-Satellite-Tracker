using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Console_Satellite_Tracker
{
    struct Coordinates {

        public float latitude, longitude;

        public Coordinates(float latitude, float longitude) {
            this.latitude = latitude; this.longitude = longitude;
        }

        public override string ToString()
        {
            return $"lat: {(latitude * 180.0f / MathF.PI).ToString("0.00")} long: {(longitude * 180.0f / MathF.PI).ToString("0.00")}";
        }
    }

    internal class Orbit {
        DateTime anomalyTime;
        float meanAnomaly;
        float meanMotion;

        float inclination;
        float raan;

        float aop;

        public Orbit(SatelliteInfo info) {
            this.meanAnomaly = info.meanAnomaly - MathF.PI / 2 + info.argument;
            this.meanMotion = info.meanMotion;
            this.aop = info.argument;
            this.anomalyTime = info.time;
            this.inclination = info.inclination;
            this.raan = info.raan;
        } 

        public Orbit(float ecc, float aop, float raan, float inc, float meanMotion, float meanAnomaly, DateTime anomalyTime) {
            this.meanAnomaly = - MathF.PI / 2 + aop + meanAnomaly;
            this.meanMotion = meanMotion;
            this.aop = aop;
            this.anomalyTime = anomalyTime;
            this.inclination = inc;
            this.raan = raan;
        }

        public Coordinates GetCelestialCoordinates(DateTime time) {
            float mean = (float)(time - anomalyTime).TotalDays * meanMotion + meanAnomaly;
            float latitude = MathF.Asin(MathF.Cos(mean) * MathF.Sin(inclination));

            float longitude = MathF.Atan2(MathF.Sin(mean), MathF.Cos(inclination) * MathF.Cos(mean)) + raan - 0.5f * MathF.PI;

            return new Coordinates(latitude, longitude);
        }

        public Coordinates GetCoordinates(DateTime time)
        {
            Coordinates celestial = GetCelestialCoordinates(time);
            Coordinates originCelestial = GroundLocation.ORIGIN.GetCelestialCoordinates(time);
            float longitude = celestial.longitude - originCelestial.longitude;
            while (longitude > MathF.PI)
            {
                longitude -= MathF.Tau;
            }
            while (longitude < -MathF.PI)
            {
                longitude += MathF.Tau;
            }
            return new Coordinates(celestial.latitude, longitude);
        }
    }
}
