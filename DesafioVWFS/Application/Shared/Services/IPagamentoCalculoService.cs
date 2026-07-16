using DesafioVWFS.Application.Shared.Domain.Entities;

namespace DesafioVWFS.Application.Shared.Services;

public interface IPagamentoCalculoService
{
    Pagamento CalcularParcela(Contrato contrato, List<Pagamento> pagamentosExistentes, DateTime dataVencimentoAnterior);
}
