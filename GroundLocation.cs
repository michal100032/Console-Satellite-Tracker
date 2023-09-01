using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Satellite_Tracker
{
    internal class GroundLocation {
        public static readonly GroundLocation ORIGIN = new GroundLocation(0.0f, 0.0f);

        static readonly DateTime VERNAL_EQUINOX = new DateTime(2023, 3, 20, 21, 24, 0, DateTimeKind.Utc);
        static readonly TimeSpan VERNAL_EQUINOX_DIFF
            = VERNAL_EQUINOX - new DateTime(VERNAL_EQUINOX.Year, VERNAL_EQUINOX.Month, VERNAL_EQUINOX.Day, 0, 0, 0, DateTimeKind.Utc);
        static readonly TimeSpan ROTATION_PERIOD = new TimeSpan(0, 23, 56, 4, 90);
        static readonly float VERNAL_ANGLE = (float)VERNAL_EQUINOX_DIFF.TotalSeconds / (float)ROTATION_PERIOD.TotalSeconds * MathF.Tau - 2.5f / 180.0f * MathF.PI;
        
        float latitude, longitude;
        
        public GroundLocation(float latitude, float longitude) {
            this.latitude = latitude * MathF.PI / 180; this.longitude = longitude * MathF.PI / 180;
        }

        public Coordinates GetCoordinates()
        {
            return new Coordinates(latitude, longitude);
        }

        public Coordinates GetCelestialCoordinates(DateTime time) {
            float cLongitude = 
                (float)(time - VERNAL_EQUINOX).TotalSeconds / (float)ROTATION_PERIOD.TotalSeconds * MathF.Tau + VERNAL_ANGLE;
            while (cLongitude > MathF.PI) {
                cLongitude -= MathF.Tau;
            }
            while (cLongitude < -MathF.PI) {
                cLongitude += MathF.Tau;
            }

            return new Coordinates(latitude, cLongitude);
        }
    }
}
