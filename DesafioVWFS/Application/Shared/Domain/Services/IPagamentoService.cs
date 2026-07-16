using DesafioVWFS.Application.DTOs;

namespace DesafioVWFS.Application.Shared.Domain.Services;

public interface IPagamentoService
{
    Task<PagamentoResponse> RegistrarPagamentoAsync(Guid contratoId, RegistrarPagamentoRequest request);
    Task<List<ListaPagamentosResponse>> ListarPagamentosAsync(Guid contratoId);
    Task<PagamentoResponse?> ObterPorIdAsync(Guid pagamentoId);
}
