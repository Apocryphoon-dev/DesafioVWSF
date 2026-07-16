using DesafioVWFS.Application.Shared.Domain.Services;
using DesafioVWFS.Application.DTOs;

namespace DesafioVWFS.Application.Features.Clients;

public interface IClientsApplicationService
{
    Task<ResumoClienteResponse> GetCustomerSummaryAsync(string cpfCnpj);
}

public class ClientsApplicationService : IClientsApplicationService
{
    private readonly IClienteService _clienteService;

    public ClientsApplicationService(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    public Task<ResumoClienteResponse> GetCustomerSummaryAsync(string cpfCnpj)
        => _clienteService.ObterResumoAsync(cpfCnpj);
}
