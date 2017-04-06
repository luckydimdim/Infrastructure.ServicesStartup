using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cmas.Infrastructure.ServicesStartup.Serialization
{
    public class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFK";
            DateTimeZoneHandling = DateTimeZoneHandling.Utc;

            ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            // Раскомментировать, если используется особый конвертер DateTime 
            //Converters.Add(new CustomDateTimeConverter());
        }
    }
}
