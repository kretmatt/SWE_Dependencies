using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SWE_Consumer2;

public class CustomerConverter:JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        var jObject = JObject.Load(reader);
        
        Customer customer = new Customer()
        {
            Name = (string)jObject["name"],
            AggregatedBalance = jObject["financialProducts"].Sum(x=>(double)x["balance"])
        };

        return customer;
    }

    public override bool CanConvert(Type objectType)
    {
        return true;
    }
}