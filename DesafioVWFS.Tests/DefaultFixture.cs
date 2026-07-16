using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Domain.Enums;
using DesafioVWFS.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioVWFS.Tests;

public static class DefaultFixture
{
    public static DesafioDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<DesafioDbContext>()
            .UseInMemoryDatabase(databaseName: $"desafiovwfs-tests-{Guid.NewGuid():N}")
            .Options;

        return new DesafioDbContext(options);
    }

    public static Contrato CreateContrato(
        string cpfCnpj,
        bool ativo = true,
        int prazoMeses = 12,
        decimal valorTotal = 10000m)
    {
        return new Contrato
        {
            Id = Guid.NewGuid(),
            ClienteCpfCnpj = cpfCnpj,
            ValorTotal = valorTotal,
            TaxaMensal = 2m,
            PrazoMeses = prazoMeses,
            DataVencimentoPrimeiraParcela = DateTime.UtcNow.Date.AddDays(30),
            TipoVeiculo = TipoVeiculo.AUTOMOVEL,
            CondicaoVeiculo = CondicaoVeiculo.USADO,
            Ativo = ativo,
            DataCriacao = DateTime.UtcNow,
            Pagamentos = new List<Pagamento>()
        };
    }

    public static Pagamento CreatePagamento(
        Guid contratoId,
        StatusPagamento status,
        DateTime? dataPagamento,
        decimal saldoDevedor,
        DateTime? dataCriacao = null)
    {
        return new Pagamento
        {
            Id = Guid.NewGuid(),
            ContratoId = contratoId,
            ValorParcela = 1000m,
            Juros = 100m,
            Amortizacao = 900m,
            SaldoDevedor = saldoDevedor,
            DataVencimento = DateTime.UtcNow.Date,
            DataPagamento = dataPagamento,
            StatusPagamento = status,
            DataCriacao = dataCriacao ?? DateTime.UtcNow
        };
    }
}
