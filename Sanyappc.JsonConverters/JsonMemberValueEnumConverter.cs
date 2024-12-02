using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sanyappc.JsonConverters
{
    public class JsonMemberValueEnumConverter<TEnum> : JsonConverter<TEnum>
        where TEnum : struct, Enum
    {
        private static readonly Dictionary<TEnum, string> enumToMemberValues;
        private static readonly Dictionary<string, TEnum> memberValueToEnums;

        static JsonMemberValueEnumConverter()
        {
            TEnum[] values = Enum.GetValues<TEnum>();

            enumToMemberValues = new(values.Length);
            memberValueToEnums = new(values.Length);

            foreach (TEnum value in values)
            {
                string enumMemverValue = GetMemberValue(value);

                enumToMemberValues.Add(value, enumMemverValue);
                memberValueToEnums.Add(enumMemverValue, value);
            }
        }

        private static string GetMemberValue(TEnum value)
        {
            EnumMemberAttribute? enumMemberAttribute = typeof(TEnum)
                .GetField(value.ToString())
                ?.GetCustomAttribute<EnumMemberAttribute>();

            string? enumMemverValue = enumMemberAttribute?.Value;
            if (enumMemverValue != null)
                return enumMemverValue;

            throw new NotSupportedException();
        }

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? memberValue = reader.GetString();
            if (memberValue != null && memberValueToEnums.TryGetValue(memberValue, out var value))
                return value;

            throw new FormatException { Source = "System.Text.Json.Rethrowable" };
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(enumToMemberValues[value]);
        }
    }
}
