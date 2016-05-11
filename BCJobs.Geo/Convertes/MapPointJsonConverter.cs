using Newtonsoft.Json;
using System;

namespace BCJobs.Analytics.Geocoding.Convertes
{
    class MapPointJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MapPoint);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var text = reader.ReadAsString();
            if (string.IsNullOrEmpty(text))
                return null;
            else
                return MapPoint.Parse(text);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value?.ToString());
        }
    }
}
