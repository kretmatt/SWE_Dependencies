using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SWE_Consumer1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController:ControllerBase
{
    private ILogger<CustomerController> _logger;
    private HttpClient _client;
    public CustomerController(ILogger<CustomerController> logger)
    {
        _logger = logger;

        _client = new HttpClient();
        _client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("BaseUrl"));
    }

    [HttpGet]
    public IActionResult ReadAll()
    {
        try
        {
            _logger.LogInformation("Requesting customer information from provider!");
            var customersResponseMessage = _client.GetAsync("api/Customer").Result;
            var customersResponseContent = customersResponseMessage.Content.ReadAsStringAsync().Result;
            var customers = JsonConvert.DeserializeObject<IList<Customer>>(customersResponseContent);
            _logger.LogInformation("Retrieved the customer information!");
            return Ok(customers);
        }
        catch (Exception e)
        {
            _logger.LogError("An error occurred while retrieving customer information!",e);
            return StatusCode(500);
        }
    }
}