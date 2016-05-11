using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCJobs.Analytics.Geocoding
{
    public struct MapPoint : IEquatable<MapPoint>
    {
        public static implicit operator DbGeography(MapPoint? point)
        {
            if (point == null)
                return null;

            return DbGeography.PointFromText($"POINT({point.Value.Longitude} {point.Value.Latitude})", 4326);
        }

        public static explicit operator MapPoint? (DbGeography geo)
        {
            if (geo == null)
                return null;

            return (MapPoint)geo;
        }

        public static explicit operator MapPoint(DbGeography geo)
        {
            if (geo == null)
                throw new InvalidCastException("DbGeography is required.");

            if (geo.Latitude == null || geo.Longitude == null)
                throw new InvalidCastException("DbGeography does not represent point.");

            return new MapPoint(geo.Latitude.Value, geo.Longitude.Value);
        }

        public static MapPoint Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new FormatException("Malformed Point.");

            var xy = text
                .Trim()
                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (xy.Length != 2)
                throw new FormatException("Malformed Point.");

            double latitude, longitude;
            if (!double.TryParse(xy[0], out latitude) || !double.TryParse(xy[1], out longitude))
                throw new FormatException("Malformed Point.");

            return new MapPoint(latitude, longitude);
        }

        public MapPoint(double latitude, double longitude)
            : this()
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }

        public override string ToString()
        {
            return $"{Latitude} {Longitude}";
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj != null &&
                Equals((MapPoint)obj);
        }

        public bool Equals(MapPoint other)
        {
            return
                Latitude == other.Latitude &&
                Longitude == other.Longitude;
        }

        public static bool operator ==(MapPoint left, MapPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MapPoint left, MapPoint right)
        {
            return !left.Equals(right);
        }
    }
}