using System.ComponentModel.DataAnnotations;
using BusinessLogicLayer.Interfaces;
using Common.Entities;
using DataAccessLayer.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace SWE_Dependencies_Provider.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CustomerController:ControllerBase
{
    private ILogger<CustomerController> _logger;
    private IProviderBusinessLogic<Customer> _customerBusinessLogic;
    private ProviderContext _providerContext;

    public CustomerController(IProviderBusinessLogic<Customer> customerBusinessLogic, ILogger<CustomerController> logger, ProviderContext providerContext)
    {
        _logger = logger;
        _customerBusinessLogic = customerBusinessLogic;
        _providerContext = providerContext;
    }

    [HttpPost]
    public IActionResult Create(Customer customer)
    {
        try
        {
            _logger.LogInformation("Received create request for a new customer!");
            _customerBusinessLogic.Create(customer);
        }
        catch (ValidationException ve)
        {
            _logger.LogError($"Customer data could not be validated successfully. Errors:{ve}");
            return BadRequest("Bad customer data");
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Data was validated, but there were other errors. Error:{e}");
            return BadRequest();
        }
        _logger.LogInformation("Customer was created");
        return StatusCode(201, customer);
    }

    [HttpGet]
    public IActionResult ReadAll()
    {
        try
        {
            _logger.LogInformation("Received read request for customers");
            IEnumerable<Customer> customers = _customerBusinessLogic.ReadAll();
            _logger.LogInformation("Customers were retrieved!");

            return Ok(customers);
        }
        catch (Exception e)
        {
            _logger.LogError($"Request can not be fulfilled. Retrieving customers led to an unexpected error. Error:{e}");
            return StatusCode(500);
        }
    }

    [HttpDelete]
    public IActionResult DeleteAll()
    {
        _providerContext.FinancialProducts.RemoveRange(_providerContext.FinancialProducts);
        _providerContext.Customers.RemoveRange(_providerContext.Customers);
        _providerContext.SaveChanges();
        return Ok();
    }
}