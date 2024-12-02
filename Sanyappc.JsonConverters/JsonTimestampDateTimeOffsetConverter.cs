using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sanyappc.JsonConverters
{
    public class JsonTimestampDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            long timestamp = reader.GetInt64();

            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToUnixTimeMilliseconds());
        }
    }
}
