using DesafioVWFS.Application.Shared.Domain.Enums;

namespace DesafioVWFS.Application.Shared.Domain.Entities;

public class Contrato
{
    public Guid Id { get; set; }
    public string ClienteCpfCnpj { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public decimal TaxaMensal { get; set; }
    public int PrazoMeses { get; set; }
    public DateTime DataVencimentoPrimeiraParcela { get; set; }
    public TipoVeiculo TipoVeiculo { get; set; }
    public CondicaoVeiculo CondicaoVeiculo { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
    public bool Ativo { get; set; } = true;
    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
