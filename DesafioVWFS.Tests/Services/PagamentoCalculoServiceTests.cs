using DesafioVWFS.Application.Services;

namespace DesafioVWFS.Tests.Services;

public class PagamentoCalculoServiceTests
{
    [Fact]
    public void CalcularParcela_PrimeiraParcela_DeveGerarDadosBasicosCorretos()
    {
        var service = new PagamentoCalculoService();
        var contrato = DefaultFixture.CreateContrato("12345678909", prazoMeses: 12, valorTotal: 12000m);
        var referenciaVencimento = contrato.DataVencimentoPrimeiraParcela;

        var parcela = service.CalcularParcela(contrato, new List<DesafioVWFS.Application.Shared.Domain.Entities.Pagamento>(), referenciaVencimento);

        Assert.Equal(contrato.Id, parcela.ContratoId);
        Assert.True(parcela.ValorParcela > 0);
        Assert.True(parcela.Juros > 0);
        Assert.True(parcela.Amortizacao > 0);
        Assert.True(parcela.SaldoDevedor < contrato.ValorTotal);
        Assert.Equal(referenciaVencimento.AddDays(30), parcela.DataVencimento);
    }

    [Fact]
    public void CalcularParcela_ComHistorico_DeveUsarSaldoDoUltimoPagamento()
    {
        var service = new PagamentoCalculoService();
        var contrato = DefaultFixture.CreateContrato("12345678909", prazoMeses: 12, valorTotal: 12000m);

        var pagamentos = new List<DesafioVWFS.Application.Shared.Domain.Entities.Pagamento>
        {
            DefaultFixture.CreatePagamento(contrato.Id, DesafioVWFS.Application.Shared.Domain.Enums.StatusPagamento.EMDIA, DateTime.UtcNow.Date.AddDays(-30), 10000m, DateTime.UtcNow.AddDays(-2)),
            DefaultFixture.CreatePagamento(contrato.Id, DesafioVWFS.Application.Shared.Domain.Enums.StatusPagamento.EMDIA, DateTime.UtcNow.Date.AddDays(-1), 8000m, DateTime.UtcNow.AddDays(-1))
        };

        var referenciaVencimento = DateTime.UtcNow.Date;

        var parcela = service.CalcularParcela(contrato, pagamentos, referenciaVencimento);

        Assert.True(parcela.SaldoDevedor < 8000m);
        Assert.Equal(referenciaVencimento.AddDays(30), parcela.DataVencimento);
    }
}
