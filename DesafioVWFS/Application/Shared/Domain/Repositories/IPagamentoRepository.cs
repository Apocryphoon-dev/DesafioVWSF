using DesafioVWFS.Application.Shared.Domain.Entities;

namespace DesafioVWFS.Application.Shared.Domain.Repositories;

public interface IPagamentoRepository
{
    Task<Pagamento?> ObterPorIdAsync(Guid id);
    Task<List<Pagamento>> ListarPorContratoAsync(Guid contratoId);
    Task<Pagamento?> ObterUltimoPorContratoAsync(Guid contratoId);
    Task<Pagamento> CriarAsync(Pagamento pagamento);
    Task<Pagamento> AtualizarAsync(Pagamento pagamento);
    Task<bool> DeletarAsync(Guid id);
}
