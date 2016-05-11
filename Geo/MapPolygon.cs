using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCJobs.Analytics.Geocoding
{
    public struct MapPolygon : IEnumerable<MapPoint>, IEquatable<MapPolygon>
    {
        static readonly MapPolygon Empty = new MapPolygon();

        public static implicit operator DbGeography(MapPolygon? polygon)
        {
            if (polygon == null || polygon == Empty)
                return null;

            var points = string.Join(",", from p in polygon.Value
                                          select $"{p.Longitude} {p.Latitude}");

            return DbGeography.PolygonFromText($"POLYGON(({points}))", 4326);
        }

        public static explicit operator MapPolygon? (DbGeography geo)
        {
            if (geo == null)
                return null;

            return (MapPolygon)geo;
        }

        public static explicit operator MapPolygon(DbGeography geo)
        {
            if (geo == null)
                throw new InvalidCastException("DbGeography is required.");

            if (geo.PointCount == null)
                throw new InvalidCastException("DbGeography does not represent polygon.");

            return new MapPolygon(from i in Enumerable.Range(1, geo.PointCount.Value)
                               select (MapPoint)geo.PointAt(i));
        }

        public static MapPolygon Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new FormatException("Malformed Polygon.");

            var points = text
                .Trim()
                .Split(',');

            return new MapPolygon(from p in points
                               select MapPoint.Parse(p));
        }

        public static MapPolygon Rectangle(MapPoint southWest, MapPoint northEast)
        {
            var southEast = new MapPoint(southWest.Latitude, northEast.Longitude);
            var northWest = new MapPoint(northEast.Latitude, southWest.Longitude);
            return new MapPolygon(new[]
            {
                southWest, southEast,
                northEast, northWest
            });
        }

        public MapPolygon(IEnumerable<MapPoint> points)
            : this()
        {
            var list = points.ToList();
            if (list.Count < 3)
                throw new ArgumentException("Not enough points in polygon.");

            if (list.First() != list.Last())
                list.Add(list.First());

            Points = list;
        }

        IEnumerable<MapPoint> Points { get; }

        public override string ToString()
        {
            return String.Join(",", Points);
        }

        public IEnumerator<MapPoint> GetEnumerator()
        {
            return (Points ?? new MapPoint[0]).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override int GetHashCode()
        {
            return Points.Aggregate(0, (hc, p) => hc ^ p.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return obj != null &&
                Equals((MapPolygon)obj);
        }

        public bool Equals(MapPolygon other)
        {
            return this.SequenceEqual(other);
        }

        public static bool operator ==(MapPolygon left, MapPolygon right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MapPolygon left, MapPolygon right)
        {
            return !left.Equals(right);
        }
    }
}
