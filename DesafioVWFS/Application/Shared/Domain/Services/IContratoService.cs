using DesafioVWFS.Application.DTOs;

namespace DesafioVWFS.Application.Shared.Domain.Services;

public interface IContratoService
{
    Task<ContratoResponse?> ObterPorIdAsync(Guid id);
    Task<List<ListaContratoResponse>> ListarTodosAsync(int page = 1, int pageSize = 10, bool ordenarDescendente = false);
    Task<List<ContratoResponse>> ObterPorClienteAsync(string cpfCnpj);
    Task<ContratoResponse> CriarAsync(CriarContratoRequest request);
    Task<bool> DeletarAsync(Guid id);
}
