using FluentValidation;
using TraverseXml.Models;

namespace TraverseXml.Validators;

public class CompareScriptValidator: AbstractValidator<TreeStructure>
{
    public CompareScriptValidator()
    {
        RuleFor(x=>x.Tag).NotNull().WithMessage("Tag cannot be null!");        
    }
}