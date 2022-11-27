using Common.Entities;
using FluentValidation;

namespace Common.Validators;

public class FinancialProductValidator:AbstractValidator<FinancialProduct>
{
    public FinancialProductValidator()
    {
        RuleFor(x => x.Balance).GreaterThan(0);
        RuleFor(x => x.ProductCode).NotEmpty();
        RuleFor(x => x.InterestRate).NotNull();
    }
}