using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Domain.Enums;
using DesafioVWFS.Application.Shared.Domain.Services;

namespace DesafioVWFS.Services
{
    public class PagamentoCalculoService : IPagamentoCalculoService
    {
        /// <summary>
        /// Calcula uma parcela usando a fórmula Tabela Price (juros compostos)
        /// </summary>
        public Pagamento CalcularParcela(Contrato contrato, List<Pagamento> pagamentosExistentes, DateTime dataVencimentoAnterior)
        {
            decimal valor;
            int mesesRestantes;

            if (pagamentosExistentes.Count == 0)
            {
                // Primeira parcela
                valor = contrato.ValorTotal;
                mesesRestantes = contrato.PrazoMeses;
            }
            else
            {
                // Parcelas seguintes
                var ultimoPagamento = pagamentosExistentes.OrderByDescending(p => p.DataCriacao).First();
                valor = ultimoPagamento.SaldoDevedor;
                mesesRestantes = contrato.PrazoMeses - pagamentosExistentes.Count;
            }

            // Converter taxa de percentual para decimal (ex: 2% = 0.02)
            decimal jurosMensal = contrato.TaxaMensal / 100m;

            // Calcular fator: (1 + jurosMensal)^mesesRestantes
            decimal fator = (decimal)Math.Pow((double)(1 + jurosMensal), mesesRestantes);

            // Calcular parcela fixa: valor * jurosMensal * fator / (fator - 1)
            decimal parcelaFixa = valor * jurosMensal * fator / (fator - 1);

            // Calcular juros do mês: valor * jurosMensal
            decimal juros = valor * jurosMensal;

            // Calcular amortização: parcelaFixa - juros
            decimal amortizacao = parcelaFixa - juros;

            // Calcular saldo devedor após parcela: valor - amortizacao
            decimal saldoDevedor = Math.Round(valor - amortizacao, 2);

            // Calcular data de vencimento: próxima data + 30 dias
            DateTime dataVencimento = dataVencimentoAnterior.AddDays(30);

            // Criar pagamento
            var pagamento = new Pagamento
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

            return pagamento;
        }
    }
}
