namespace DesafioVWFS.Application.Features.Payments.ListPayments.Models;

public class ListPaymentsOutput
{
    public List<ListPaymentsItem> Pagamentos { get; set; } = new();
}

public class ListPaymentsItem
{
    public Guid Id { get; set; }
    public decimal ValorPago { get; set; }
    public DateTime DataVencimento { get; set; }
    public DateTime? DataPagamento { get; set; }
    public string StatusPagamento { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}
