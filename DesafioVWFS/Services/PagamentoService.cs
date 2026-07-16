using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Shared.Domain.Services;

namespace DesafioVWFS.Services
{
    public class PagamentoService : IPagamentoService
    {
        public Task<PagamentoResponse> RegistrarPagamentoAsync(Guid contratoId, RegistrarPagamentoRequest request)
            => throw new NotSupportedException("Use a implementação em Application/Services.");

        public Task<List<ListaPagamentosResponse>> ListarPagamentosAsync(Guid contratoId)
            => throw new NotSupportedException("Use a implementação em Application/Services.");

        public Task<PagamentoResponse?> ObterPorIdAsync(Guid pagamentoId)
            => throw new NotSupportedException("Use a implementação em Application/Services.");
    }
}
