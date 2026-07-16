using DesafioVWFS.Application.Shared.Domain.Enums;

namespace DesafioVWFS.Application.Shared.Domain.Entities;

public class Pagamento
{
    public Guid Id { get; set; }
    public Guid ContratoId { get; set; }
    public decimal ValorParcela { get; set; }
    public decimal Juros { get; set; }
    public decimal Amortizacao { get; set; }
    public decimal SaldoDevedor { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public StatusPagamento StatusPagamento { get; set; }
    public string? Observacoes { get; set; }
    public Contrato? Contrato { get; set; }

    public void CalcularStatus()
    {
        if (DataPagamento == null)
        {
            StatusPagamento = DateTime.UtcNow > DataVencimento ? StatusPagamento.ATRASADO : StatusPagamento.EMDIA;
        }
        else
        {
            if (DataPagamento.Value.Date > DataVencimento.Date)
            {
                StatusPagamento = StatusPagamento.ATRASADO;
            }
            else if (DataPagamento.Value.Date < DataVencimento.Date)
            {
                StatusPagamento = StatusPagamento.ANTECIPADO;
            }
            else
            {
                StatusPagamento = StatusPagamento.EMDIA;
            }
        }
    }
}
