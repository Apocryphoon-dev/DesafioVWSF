namespace DesafioVWFS.Application.DTOs
{
    public class ResumoClienteResponse
    {
        public string ClienteCpfCnpj { get; set; } = string.Empty;
        public int ContratosAtivos { get; set; }
        public int TotalParcelas { get; set; }
        public int ParcelasPagas { get; set; }
        public int ParcelasAtrasadas { get; set; }
        public int ParcelasAVencer { get; set; }
        public decimal PercentualParcelasPagasEmDia { get; set; }
        public decimal SaldoDevedorConsolidado { get; set; }
    }
}
