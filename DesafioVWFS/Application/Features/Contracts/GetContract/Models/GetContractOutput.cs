namespace DesafioVWFS.Application.Features.Contracts.GetContract.Models;

public class GetContractOutput
{
    public Guid Id { get; set; }
    public string ClienteCpfCnpj { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public decimal TaxaMensal { get; set; }
    public int PrazoMeses { get; set; }
    public DateTime DataVencimentoPrimeiraParcela { get; set; }
    public string TipoVeiculo { get; set; } = string.Empty;
    public string CondicaoVeiculo { get; set; } = string.Empty;
    public decimal SaldoDevedor { get; set; }
    public int TotalParcelas { get; set; }
    public int ParcelasPagas { get; set; }
    public int ParcelasAtrasadas { get; set; }
    public int ParcelasAVencer { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
