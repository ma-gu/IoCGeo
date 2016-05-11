using System;
using System.ComponentModel;
using System.Globalization;

namespace BCJobs.Analytics.Geocoding.Convertes
{
    public class MapPointTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return
                sourceType == typeof(string) ||
                base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
                return null;

            if (value is string)
                if (string.IsNullOrWhiteSpace(value as string))
                    return null;
                else
                    return MapPoint.Parse(value as string);

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return
                destinationType == typeof(string) ||
                base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return value?.ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
