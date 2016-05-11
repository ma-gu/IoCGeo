namespace BCJobs.Analytics.Geocoding
{
    public interface IGeocoder
    {
        Coordinates Resolve(string location);
    }
}
