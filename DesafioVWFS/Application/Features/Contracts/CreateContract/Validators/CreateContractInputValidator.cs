using DesafioVWFS.Application.Features.Contracts.CreateContract.Models;
using FluentValidation;

namespace DesafioVWFS.Application.Features.Contracts.CreateContract.Validators;

public class CreateContractInputValidator : AbstractValidator<CreateContractInput>
{
    public CreateContractInputValidator()
    {
        RuleFor(x => x.ClienteCpfCnpj)
            .NotEmpty().WithMessage("CPF/CNPJ é obrigatório")
            .MaximumLength(18).WithMessage("CPF/CNPJ não pode exceder 18 caracteres");

        RuleFor(x => x.ValorTotal)
            .GreaterThan(0).WithMessage("Valor total deve ser maior que zero");

        RuleFor(x => x.TaxaMensal)
            .GreaterThan(0).WithMessage("Taxa mensal deve ser maior que zero");

        RuleFor(x => x.PrazoMeses)
            .GreaterThan(0).WithMessage("Prazo em meses deve ser maior que zero");
    }
}
