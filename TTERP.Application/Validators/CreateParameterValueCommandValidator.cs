using FluentValidation;
using TTERP.Application.CQRS.ParameterValues.Commands;
using TTERP.Domain.Interfaces;

public class CreateParameterValueCommandValidator
    : AbstractValidator<CreateParameterValueCommand>
{
    private readonly IParameterValueRepository _parameterValueRepository;

    public CreateParameterValueCommandValidator(
        IParameterValueRepository parameterValueRepository)
    {
        _parameterValueRepository = parameterValueRepository;

        RuleFor(x => x.ParamCode)
            .GreaterThan(0);

        RuleFor(x => x.ParamValue)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(200);

        RuleFor(x => x.LanguageId)
            .Must(x => x == 1 || x == 2)
            .WithMessage("LanguageId sadece 1 veya 2 olabilir.");

        //RuleFor(x => x)
        //    .MustAsync(async (command, cancellationToken) =>
        //    {
        //        return !await _parameterValueRepository.ExistsAsync(
        //            command.ParameterDefinitionId,
        //            command.ParamCode,
        //            command.LanguageId,
        //            cancellationToken);
        //    })
        //    .WithMessage("Bu ParameterDefinitionId, ParamCode ve LanguageId kombinasyonu zaten kayıtlı.");
    }
}