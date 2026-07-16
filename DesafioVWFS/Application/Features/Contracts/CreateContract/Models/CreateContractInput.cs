using DesafioVWFS.Application.Shared.Core;

namespace DesafioVWFS.Application.Features.Contracts.CreateContract.Models;

public class CreateContractInput : IRequest<CreateContractOutput>
{
    public Guid Id { get; set; }
    public string ClienteCpfCnpj { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public decimal TaxaMensal { get; set; }
    public int PrazoMeses { get; set; }
    public DateTime DataVencimentoPrimeiraParcela { get; set; }
    public string TipoVeiculo { get; set; } = string.Empty;
    public string CondicaoVeiculo { get; set; } = string.Empty;
    public Guid CorrelationId { get; set; }
}
