using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NationalBank.BackEnd.Extensions
{
    public class IdToStringConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jt = JValue.ReadFrom(reader);

            return jt.Value<long>();
        }

        public override bool CanConvert(Type objectType) => typeof(long).Equals(objectType);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => serializer.Serialize(writer, value.ToString());


    }
}
