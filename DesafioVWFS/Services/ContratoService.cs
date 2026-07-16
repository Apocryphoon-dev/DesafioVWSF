using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Shared.Domain.Services;

namespace DesafioVWFS.Services
{
    public class ContratoService : IContratoService
    {
        public Task<ContratoResponse?> ObterPorIdAsync(Guid id)
            => throw new NotSupportedException("Use a implementação em Application/Services.");

        public Task<List<ListaContratoResponse>> ListarTodosAsync(int page = 1, int pageSize = 10, bool ordenarDescendente = false)
            => throw new NotSupportedException("Use a implementação em Application/Services.");

        public Task<List<ContratoResponse>> ObterPorClienteAsync(string cpfCnpj)
            => throw new NotSupportedException("Use a implementação em Application/Services.");

        public Task<ContratoResponse> CriarAsync(CriarContratoRequest request)
            => throw new NotSupportedException("Use a implementação em Application/Services.");

        public Task<bool> DeletarAsync(Guid id)
            => throw new NotSupportedException("Use a implementação em Application/Services.");
    }
}
