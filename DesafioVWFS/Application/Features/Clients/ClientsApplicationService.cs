using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Features.Clients.GetSummary.Handlers;
using DesafioVWFS.Application.Features.Clients.GetSummary.Models;

namespace DesafioVWFS.Application.Features.Clients;

public interface IClientsApplicationService
{
    Task<ResumoClienteResponse> GetCustomerSummaryAsync(string cpfCnpj);
}

public class ClientsApplicationService : IClientsApplicationService
{
    private readonly GetSummaryUseCase _getSummaryUseCase;

    public ClientsApplicationService(GetSummaryUseCase getSummaryUseCase)
    {
        _getSummaryUseCase = getSummaryUseCase;
    }

    public async Task<ResumoClienteResponse> GetCustomerSummaryAsync(string cpfCnpj)
    {
        var output = await _getSummaryUseCase.ExecuteAsync(new GetSummaryInput { CpfCnpj = cpfCnpj }, CancellationToken.None);

        if (!output.IsValid)
        {
            var message = output.ErrorMessages.FirstOrDefault() ?? "Não foi possível obter o resumo do cliente";
            if (message.Contains("inválido", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException(message);

            throw new InvalidOperationException(message);
        }

        var result = output.GetResult();
        return new ResumoClienteResponse
        {
            ClienteCpfCnpj = result.ClienteCpfCnpj,
            ContratosAtivos = result.ContratosAtivos,
            TotalParcelas = result.TotalParcelas,
            ParcelasPagas = result.ParcelasPagas,
            ParcelasAtrasadas = result.ParcelasAtrasadas,
            ParcelasAVencer = result.ParcelasAVencer,
            PercentualParcelasPagasEmDia = result.PercentualParcelasPagasEmDia,
            SaldoDevedorConsolidado = result.SaldoDevedorConsolidado
        };
    }
}
