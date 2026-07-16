namespace DesafioVWFS.Application.Features.Payments.GetPayment.Models;

public class GetPaymentOutput
{
    public Guid Id { get; set; }
    public Guid ContratoId { get; set; }
    public decimal ValorPago { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string StatusPagamento { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; }
}
