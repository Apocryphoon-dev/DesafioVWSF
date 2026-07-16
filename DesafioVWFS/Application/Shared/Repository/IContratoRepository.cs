using DesafioVWFS.Application.Shared.Domain.Entities;

namespace DesafioVWFS.Application.Shared.Repository;

public interface IContratoRepository
{
    Task<Contrato?> ObterPorIdAsync(Guid id);
    Task<List<Contrato>> ObterPorClienteAsync(string cpfCnpj);
    Task<List<Contrato>> ListarTodosAsync(int page = 1, int pageSize = 10, bool ordenarDescendente = false);
    Task<Contrato> CriarAsync(Contrato contrato);
    Task<Contrato> AtualizarAsync(Contrato contrato);
    Task<bool> DeletarAsync(Guid id);
    Task<int> ContarTotalAsync();
}
