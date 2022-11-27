using Common.Entities;
using FluentValidation;

namespace Common.Validators;

public class CustomerValidator:AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.Address).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Status).NotNull();
        RuleFor(x => x.EmailAddress).NotEmpty().EmailAddress();
        RuleFor(x => x.DateOfBirth).NotEmpty().LessThanOrEqualTo(DateTime.Now);
        RuleForEach(x => x.FinancialProducts).SetValidator(new FinancialProductValidator());
    }
}