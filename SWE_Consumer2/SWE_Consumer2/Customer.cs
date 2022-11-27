using Newtonsoft.Json;

namespace SWE_Consumer2;

[JsonConverter(typeof(CustomerConverter))]
public class Customer
{
    public string Name { get; set; }
    public double AggregatedBalance { get; set; }
}