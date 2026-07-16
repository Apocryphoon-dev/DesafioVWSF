using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Domain.Enums;

namespace DesafioVWFS.Application.Shared.Services
{
    public class PagamentoCalculoService : IPagamentoCalculoService
    {
        public Pagamento CalcularParcela(Contrato contrato, List<Pagamento> pagamentosExistentes, DateTime dataVencimentoAnterior)
        {
            decimal valor;
            int mesesRestantes;

            if (pagamentosExistentes.Count == 0)
            {
                valor = contrato.ValorTotal;
                mesesRestantes = contrato.PrazoMeses;
            }
            else
            {
                var ultimoPagamento = pagamentosExistentes.OrderByDescending(p => p.DataCriacao).First();
                valor = ultimoPagamento.SaldoDevedor;
                mesesRestantes = contrato.PrazoMeses - pagamentosExistentes.Count;
            }

            decimal jurosMensal = contrato.TaxaMensal / 100m;
            decimal fator = (decimal)Math.Pow((double)(1 + jurosMensal), mesesRestantes);
            decimal parcelaFixa = valor * jurosMensal * fator / (fator - 1);
            decimal juros = valor * jurosMensal;
            decimal amortizacao = parcelaFixa - juros;
            decimal saldoDevedor = Math.Round(valor - amortizacao, 2);
            DateTime dataVencimento = dataVencimentoAnterior.AddDays(30);

            return new Pagamento
            {
                Id = Guid.NewGuid(),
                ContratoId = contrato.Id,
                ValorParcela = Math.Round(parcelaFixa, 2),
                Juros = Math.Round(juros, 2),
                Amortizacao = Math.Round(amortizacao, 2),
                SaldoDevedor = saldoDevedor,
                DataVencimento = dataVencimento,
                DataPagamento = null,
                StatusPagamento = StatusPagamento.EMDIA,
                DataCriacao = DateTime.UtcNow
            };
        }
    }
}
