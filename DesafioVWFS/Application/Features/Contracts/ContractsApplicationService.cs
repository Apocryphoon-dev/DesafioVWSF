using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Shared.Domain.Services;

namespace DesafioVWFS.Application.Features.Contracts;

public interface IContractsApplicationService
{
    Task<ContratoResponse?> GetContractAsync(Guid id);
    Task<List<ListaContratoResponse>> ListContractsAsync(int page = 1, int pageSize = 10, bool ordenarDescendente = false);
    Task<List<ContratoResponse>> GetContractsByCustomerAsync(string cpfCnpj);
    Task<ContratoResponse> CreateContractAsync(CriarContratoRequest request);
    Task<bool> DeleteContractAsync(Guid id);
}

public class ContractsApplicationService : IContractsApplicationService
{
    private readonly IContratoService _contratoService;

    public ContractsApplicationService(IContratoService contratoService)
    {
        _contratoService = contratoService;
    }

    public Task<ContratoResponse?> GetContractAsync(Guid id) => _contratoService.ObterPorIdAsync(id);

    public Task<List<ListaContratoResponse>> ListContractsAsync(int page = 1, int pageSize = 10, bool ordenarDescendente = false)
        => _contratoService.ListarTodosAsync(page, pageSize, ordenarDescendente);

    public Task<List<ContratoResponse>> GetContractsByCustomerAsync(string cpfCnpj)
        => _contratoService.ObterPorClienteAsync(cpfCnpj);

    public Task<ContratoResponse> CreateContractAsync(CriarContratoRequest request)
        => _contratoService.CriarAsync(request);

    public Task<bool> DeleteContractAsync(Guid id) => _contratoService.DeletarAsync(id);
}
