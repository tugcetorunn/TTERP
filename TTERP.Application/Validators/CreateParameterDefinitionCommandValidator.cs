using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTERP.Application.CQRS.ParameterDefinitions.Commands;

namespace TTERP.Application.Validators
{
    public class CreateParameterDefinitionCommandValidator : AbstractValidator<CreateParameterDefinitionCommand>
    {
        public CreateParameterDefinitionCommandValidator()
        {
            RuleFor(x => x.ParamType)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(200);

            RuleFor(x => x.DataType)
                .MaximumLength(15);

            RuleFor(x => x.DefaultValue)
                .MaximumLength(100);

            //RuleForEach(x => x.ParameterValues)
                //.SetValidator(new CreateParameterValueCommandValidator());

            RuleFor(x => x.ParameterValues)
                .Must(values =>
                {
                    if (values == null || !values.Any())
                        return true;

                    return !values
                        .GroupBy(x => new { x.ParamCode, x.LanguageId })
                        .Any(g => g.Count() > 1);
                })
                .WithMessage("Aynı dil için aynı değer kodu birden fazla kez gönderilemez.");
        }
    }
}
