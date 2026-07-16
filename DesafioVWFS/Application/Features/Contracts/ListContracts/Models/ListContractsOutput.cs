namespace DesafioVWFS.Application.Features.Contracts.ListContracts.Models;

public class ListContractsOutput
{
    public List<ListContractsItem> Contratos { get; set; } = new();
}

public class ListContractsItem
{
    public Guid Id { get; set; }
    public string ClienteCpfCnpj { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public decimal SaldoDevedor { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
