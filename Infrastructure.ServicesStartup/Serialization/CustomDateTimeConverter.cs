using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Cmas.Infrastructure.ServicesStartup.Serialization
{
    /// <summary>
    /// Класс для сериализации/десериализации времени 
    /// На данный момент не используется, т.к. хватает средств JsonSerializer (DateFormatString + DateTimeZoneHandling)
    /// </summary>
    public class CustomDateTimeConverter : JsonConverter
    {
        private const string iso8601formatString = "yyyy-MM-ddTHH\\:mm\\:ss.ffffffZ";

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(DateTime))
                return reader.Value;
            else if (reader.ValueType == typeof(string))
                return DateTime.ParseExact((string)reader.Value, iso8601formatString, CultureInfo.InvariantCulture);
            else
                throw new ArgumentException("Wrong ValueType: " + reader.ValueType.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(iso8601formatString, CultureInfo.InvariantCulture));
        }
    }
}
