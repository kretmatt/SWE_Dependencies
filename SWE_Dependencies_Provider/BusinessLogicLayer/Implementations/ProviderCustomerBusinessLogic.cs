using BusinessLogicLayer.Interfaces;
using Common.Entities;
using DataAccessLayer.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Implementations;

public class ProviderCustomerBusinessLogic:IProviderBusinessLogic<Customer>
{
    private IDALRepository<Customer> _customerRepository;
    private ILogger<ProviderCustomerBusinessLogic> _logger;
    private IValidator<Customer> _customerValidator;

    public ProviderCustomerBusinessLogic(IDALRepository<Customer> customerRepository, ILogger<ProviderCustomerBusinessLogic> logger, IValidator<Customer> customerValidator)
    {
        _customerRepository = customerRepository;
        _customerValidator = customerValidator;
        _logger = logger;
    }
    
    public IEnumerable<Customer> ReadAll()
    {
        IEnumerable<Customer> customers = null;

        try
        {
            _logger.LogInformation("Attempting to retrieve all customers");
            customers = _customerRepository.Get(null, null, "FinancialProducts");
        }
        catch (Exception e)
        {
            string errorMessage = "An error occurred while attempting to retrieve all customers!";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }

        return customers;
    }

    public void Create(Customer entity)
    {
        string errorMessage;
        
        _logger.LogInformation("Validating the new customer information");
        var validationResult = _customerValidator.Validate(entity);

        if (!validationResult.IsValid)
        {
            errorMessage = "Validation of the customer did not succeed!";
            _logger.LogError(errorMessage);
            _logger.LogError(string.Join(",",validationResult.Errors.Select(x=>x.ErrorMessage)));
            throw new ValidationException(errorMessage, validationResult.Errors);
        }

        try
        {
            _logger.LogInformation("Attempting to persist the new customer!");
            _customerRepository.Insert(entity);
        }
        catch (Exception e)
        {
            errorMessage = "An error occurred while attempting to persist the new customer!";
            _logger.LogError(errorMessage);
            throw new Exception(errorMessage, e);
        }
        
        _logger.LogInformation("The customer was persisted!");
    }
}