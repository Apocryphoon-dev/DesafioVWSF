namespace DesafioVWFS.Application.DTOs
{
    public class RegistrarPagamentoRequest
    {
        public decimal ValorPago { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string? Observacoes { get; set; }
    }

    public class PagamentoResponse
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

    public class ListaPagamentosResponse
    {
        public Guid Id { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string StatusPagamento { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
    }
}
