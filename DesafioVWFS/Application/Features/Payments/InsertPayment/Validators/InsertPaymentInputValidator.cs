using DesafioVWFS.Application.Features.Payments.InsertPayment.Models;
using FluentValidation;

namespace DesafioVWFS.Application.Features.Payments.InsertPayment.Validators;

public class InsertPaymentInputValidator : AbstractValidator<InsertPaymentInput>
{
    public InsertPaymentInputValidator()
    {
        RuleFor(x => x.ContratoId)
            .NotEmpty().WithMessage("ContratoId é obrigatório");
    }
}
