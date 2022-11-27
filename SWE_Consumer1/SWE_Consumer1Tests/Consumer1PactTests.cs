using System.Net;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Matchers;

namespace SWE_Consumer1Tests;

public class Consumer1PactTests
{
    private readonly IPactBuilderV3 _pactBuilderV3;
    private readonly int _port = 9000;
    private readonly HttpClient _client;
    private readonly List<object> customers;

    public Consumer1PactTests()
    {
        customers = new List<object>()
        {
            new { name = "Mika Huber", emailAddress = "mika@gmail.com", status=0},
            new { name = "Alexander Kauper", emailAddress = "kauper@gmail.com", status=0},
        };
        
        var config = new PactConfig()
        {
            PactDir = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName}{Path.DirectorySeparatorChar}pacts",
            DefaultJsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        };
        
        var pact = Pact.V3("Consumer1", "CustomerAPI", config);

        _pactBuilderV3 = pact.WithHttpInteractions(_port);
        _client = new HttpClient();
        _client.BaseAddress = new Uri($"http://localhost:{_port}");
        _client.DefaultRequestHeaders.Add("Accept", "*/*");
    }

    [Fact]
    public async Task ReadAllCustomers_NoCustomersExist_EmptyResponse()
    {
        //arrange
        this._pactBuilderV3
            .UponReceiving("A GET request to retrieve all customer data")
                .Given("No customer data")
                .WithRequest(HttpMethod.Get, "/api/Customer")
                .WithHeader("Accept", "*/*")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new TypeMatcher(new List<object>()));

        await _pactBuilderV3.VerifyAsync(async ctx =>
        {
            // act
            var response = await _client.GetAsync("/api/Customer");
            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        });
    }

    [Fact]
    public async Task ReadAllCustomers_TwoCustomersExist_ReturnsTwoCustomers()
    {
        //arrange
        this._pactBuilderV3
            .UponReceiving("A GET request to retrieve all customer data")
                .Given("There are two customers")
                .WithRequest(HttpMethod.Get, "/api/Customer")
                .WithHeader("Accept", "*/*")
            .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new TypeMatcher(customers));

        await _pactBuilderV3.VerifyAsync(async ctx =>
        {
            // act
            var response = await _client.GetAsync("/api/Customer");
            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        });
    }
}