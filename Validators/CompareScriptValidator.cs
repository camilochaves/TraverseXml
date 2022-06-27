using FluentValidation;

namespace Validators;

public class CompareScriptValidator: AbstractValidator<CompareScript>
{
    public CompareScriptValidator()
    {
        RuleFor(x=>x.Tag).NotNull().WithMessage("Tag cannot be null!");        
    }
}