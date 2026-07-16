using DesafioVWFS.Application.Features.Clients.GetSummary.Handlers;
using DesafioVWFS.Application.Features.Clients.GetSummary.Models;
using DesafioVWFS.Application.Shared.Core;
using DesafioVWFS.Application.Shared.Domain.Enums;
using DesafioVWFS.Application.Shared.Repository;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DesafioVWFS.Tests.UseCases;

public class GetSummaryUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_CpfInvalido_DeveRetornarErro()
    {
        var repoMock = new Mock<IContratoRepository>(MockBehavior.Strict);
        var logger = NullLogger<UseCaseHandlerBase<GetSummaryInput, GetSummaryOutput>>.Instance;
        var useCase = new GetSummaryUseCase(logger, repoMock.Object);

        var output = await useCase.ExecuteAsync(new GetSummaryInput { CpfCnpj = "123" }, CancellationToken.None);

        Assert.False(output.IsValid);
        Assert.Contains("CPF/CNPJ inválido", output.ErrorMessages);
        repoMock.Verify(r => r.ObterPorClienteAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_ComContratos_DeveConsolidarResumoCorretamente()
    {
        var cpf = "12345678909";
        var contrato1 = DefaultFixture.CreateContrato(cpf, ativo: true, prazoMeses: 3, valorTotal: 10000m);
        var contrato2 = DefaultFixture.CreateContrato(cpf, ativo: false, prazoMeses: 2, valorTotal: 5000m);

        contrato1.Pagamentos.Add(DefaultFixture.CreatePagamento(
            contrato1.Id,
            StatusPagamento.EMDIA,
            DateTime.UtcNow.Date.AddDays(-5),
            saldoDevedor: 9000m,
            dataCriacao: DateTime.UtcNow.AddMinutes(-10)));

        contrato1.Pagamentos.Add(DefaultFixture.CreatePagamento(
            contrato1.Id,
            StatusPagamento.ATRASADO,
            dataPagamento: null,
            saldoDevedor: 8000m,
            dataCriacao: DateTime.UtcNow));

        var repoMock = new Mock<IContratoRepository>();
        repoMock.Setup(r => r.ObterPorClienteAsync(cpf)).ReturnsAsync(new List<DesafioVWFS.Application.Shared.Domain.Entities.Contrato> { contrato1, contrato2 });

        var logger = NullLogger<UseCaseHandlerBase<GetSummaryInput, GetSummaryOutput>>.Instance;
        var useCase = new GetSummaryUseCase(logger, repoMock.Object);

        var output = await useCase.ExecuteAsync(new GetSummaryInput { CpfCnpj = cpf }, CancellationToken.None);
        var result = output.GetResult();

        Assert.True(output.IsValid);
        Assert.Equal(1, result.ContratosAtivos);
        Assert.Equal(5, result.TotalParcelas);
        Assert.Equal(1, result.ParcelasPagas);
        Assert.Equal(1, result.ParcelasAtrasadas);
        Assert.Equal(3, result.ParcelasAVencer);
        Assert.Equal(20m, result.PercentualParcelasPagasEmDia);
        Assert.Equal(13000m, result.SaldoDevedorConsolidado);
    }
}
