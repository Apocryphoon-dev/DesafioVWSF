using DesafioVWFS.Application.Shared.Core;

namespace DesafioVWFS.Application.Features.Payments.GetPayment.Models;

public class GetPaymentInput : IRequest<GetPaymentOutput>
{
    public Guid PagamentoId { get; set; }
}
