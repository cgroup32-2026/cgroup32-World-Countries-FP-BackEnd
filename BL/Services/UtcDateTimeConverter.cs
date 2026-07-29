using System.Text.Json;
using System.Text.Json.Serialization;

namespace CountriesProject.BL.Services
{
    // the ADO.NET always returns DateTime.Kind = Unspecified for sql Server DATETIME columns thats
    // why Without this, dates serialize with no timezone marker, and browsers missinterpret
    // them as local time ,thats why  i was getting the negative "days ago" values in shares page
    //used AI to help me here
    public class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => reader.GetDateTime();

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            DateTime utc = value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(value, DateTimeKind.Utc) : value.ToUniversalTime();
            writer.WriteStringValue(utc);
        }
    }
}