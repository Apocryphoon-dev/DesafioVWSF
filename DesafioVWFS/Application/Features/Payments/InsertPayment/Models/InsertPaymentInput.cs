using DesafioVWFS.Application.Shared.Core;

namespace DesafioVWFS.Application.Features.Payments.InsertPayment.Models;

public class InsertPaymentInput : IRequest<InsertPaymentOutput>
{
    public Guid ContratoId { get; set; }
    public decimal ValorPago { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string? Observacoes { get; set; }
    public Guid CorrelationId { get; set; }
}
