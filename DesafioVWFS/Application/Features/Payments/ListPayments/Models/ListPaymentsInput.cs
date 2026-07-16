using DesafioVWFS.Application.Shared.Core;

namespace DesafioVWFS.Application.Features.Payments.ListPayments.Models;

public class ListPaymentsInput : IRequest<ListPaymentsOutput>
{
    public Guid ContratoId { get; set; }
}
