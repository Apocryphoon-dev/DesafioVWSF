using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Domain.Enums;

namespace DesafioVWFS.Application.Shared.Domain.Models;

public class ContratoDomain
{
    public Guid Id { get; set; }
    public string ClienteCpfCnpj { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public decimal TaxaMensal { get; set; }
    public int PrazoMeses { get; set; }
    public DateTime DataVencimentoPrimeiraParcela { get; set; }
    public TipoVeiculo TipoVeiculo { get; set; }
    public CondicaoVeiculo CondicaoVeiculo { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }

    public static ContratoDomain FromEntity(Contrato contrato) => new()
    {
        Id = contrato.Id,
        ClienteCpfCnpj = contrato.ClienteCpfCnpj,
        ValorTotal = contrato.ValorTotal,
        TaxaMensal = contrato.TaxaMensal,
        PrazoMeses = contrato.PrazoMeses,
        DataVencimentoPrimeiraParcela = contrato.DataVencimentoPrimeiraParcela,
        TipoVeiculo = contrato.TipoVeiculo,
        CondicaoVeiculo = contrato.CondicaoVeiculo,
        Ativo = contrato.Ativo,
        DataCriacao = contrato.DataCriacao
    };
}
