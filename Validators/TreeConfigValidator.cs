using FluentValidation;
using TraverseXml.Models;

namespace TraverseXml.Validators;

public class TreeConfigValidator: AbstractValidator<TreeConfig>
{
    public TreeConfigValidator()
    {
        RuleFor(x=>x.Export).Must(x=> {
            var options = new List<string>(){
                "unique","equal","all"
            };
            return options.Contains(x);
        }).WithMessage("Export must be: unique, equal, or all");

        RuleFor(x=>x.Output).Must(x=> x=="console" || x=="excel")
        .WithMessage("Output must be: console or excel");

        RuleFor(x=>x.XML).NotNull().WithMessage("XML file cannot be null");

        RuleFor(x=>x.Config).SetValidator(new CompareScriptValidator());        
    }
}