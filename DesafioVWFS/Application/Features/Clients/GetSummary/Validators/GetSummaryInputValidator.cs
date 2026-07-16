using DesafioVWFS.Application.Features.Clients.GetSummary.Models;
using FluentValidation;

namespace DesafioVWFS.Application.Features.Clients.GetSummary.Validators;

public class GetSummaryInputValidator : AbstractValidator<GetSummaryInput>
{
    public GetSummaryInputValidator()
    {
        RuleFor(x => x.CpfCnpj)
            .NotEmpty().WithMessage("CPF/CNPJ é obrigatório");
    }
}
