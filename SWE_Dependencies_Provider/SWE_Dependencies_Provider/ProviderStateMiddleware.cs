using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using Common.Entities;
using Common.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProviderTests;

public class ProviderStateMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDictionary<string, Action> _providerStates;
    private readonly HttpClient _client;

    public ProviderStateMiddleware(RequestDelegate next)
    {
        _next = next;
        _providerStates = new Dictionary<string, Action>()
        {
            {
                "No customer data",
                RemoveAllData
            },
            {
                "There are two customers",
                CreateTwoCustomers
            }
        };

        _client = new HttpClient();
        _client.BaseAddress = new Uri($"http://localhost:8180/");
    }

    private void RemoveAllData()
    {
        var deleteResponse = _client.DeleteAsync("api/Customer").Result;
        deleteResponse.EnsureSuccessStatusCode();
    }

    private void CreateTwoCustomers()
    {
        RemoveAllData();

        var customer1 = new Customer()
        {
            Address = "Straße 1",
            CustomerId = 0,
            DateOfBirth = DateTime.Now,
            EmailAddress = "mika@gmail.com",
            FinancialProducts = new List<FinancialProduct>()
            {
                new FinancialProduct()
                {
                    Balance = 200.0,
                    FinancialProductId = 0,
                    InterestRate = 1,
                    ProductCode = "Code1"
                },
                new FinancialProduct()
                {
                    Balance = 100.0,
                    FinancialProductId = 0,
                    InterestRate = 3,
                    ProductCode = "Code2"
                }
            },
            Name = "Mika Huber",
            Status = ECustomerStatus.ACTIVE
        };
        
        var customer2  = new Customer()
        {
            Address = "Straße 2",
            CustomerId = 0,
            DateOfBirth = DateTime.Now,
            EmailAddress = "kauper@gmail.com",
            FinancialProducts = new List<FinancialProduct>()
            {
                new FinancialProduct()
                {
                    Balance = 250.0,
                    FinancialProductId = 0,
                    InterestRate = 1,
                    ProductCode = "Code3"
                },
                new FinancialProduct()
                {
                    Balance = 99.5,
                    FinancialProductId = 0,
                    InterestRate = 3,
                    ProductCode = "Code4"
                }
            },
            Name = "Alexander Kauper",
            Status = ECustomerStatus.ACTIVE
        };

        HttpContent content =
            new StringContent(JsonConvert.SerializeObject(customer1), Encoding.UTF8, "application/json");

        var customer1Response = _client.PostAsync("/api/Customer", content).Result;
        customer1Response.EnsureSuccessStatusCode();
        
        content = 
            new StringContent(JsonConvert.SerializeObject(customer2), Encoding.UTF8, "application/json");

        var customer2Response = _client.PostAsync("/api/Customer", content).Result;
        customer2Response.EnsureSuccessStatusCode();
    }
    
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.Value == "/provider-states")
        {
            await this.HandleProviderStatesRequestAsync(context);
            await context.Response.WriteAsync(String.Empty);
        }
        else
        {
            await this._next(context);
        }
    }
    
    private async Task HandleProviderStatesRequestAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.OK;

        if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() &&
            context.Request.Body != null)
        {
            string jsonRequestBody = String.Empty;
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                jsonRequestBody = await reader.ReadToEndAsync();
            }

            var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);
            
            if (providerState != null && !String.IsNullOrEmpty(providerState.State))
            {
                _providerStates[providerState.State].Invoke();
            }
        }
    }
}