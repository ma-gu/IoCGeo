using BCJobs.Analytics.Geocoding;

namespace BCJobs.Analytics.Geocoding
{
    public class Coordinates
    {
        public static readonly Coordinates None = new Coordinates(null, null);

        public Coordinates(MapPoint? centroid, MapPolygon? area)
        {
            Centroid = centroid;
            Area = area;
        }

        public MapPoint? Centroid { get; }
        public MapPolygon? Area { get; }
    }
}