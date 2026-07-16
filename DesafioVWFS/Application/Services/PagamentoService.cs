using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Domain.Repositories;
using DesafioVWFS.Application.Shared.Domain.Services;

namespace DesafioVWFS.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IContratoRepository _contratoRepository;
        private readonly IPagamentoCalculoService _calculoService;

        public PagamentoService(
            IPagamentoRepository pagamentoRepository,
            IContratoRepository contratoRepository,
            IPagamentoCalculoService calculoService)
        {
            _pagamentoRepository = pagamentoRepository;
            _contratoRepository = contratoRepository;
            _calculoService = calculoService;
        }

        public async Task<PagamentoResponse> RegistrarPagamentoAsync(Guid contratoId, RegistrarPagamentoRequest request)
        {
            var contrato = await _contratoRepository.ObterPorIdAsync(contratoId);
            if (contrato == null)
                throw new InvalidOperationException("Contrato não encontrado");

            var pagamentosExistentes = (await _pagamentoRepository.ListarPorContratoAsync(contratoId))
                .OrderBy(p => p.DataCriacao)
                .ToList();

            DateTime dataVencimentoAnterior = pagamentosExistentes.Any()
                ? pagamentosExistentes.Last().DataVencimento
                : contrato.DataVencimentoPrimeiraParcela;

            var pagamento = _calculoService.CalcularParcela(contrato, pagamentosExistentes, dataVencimentoAnterior);

            if (request.DataVencimento != default)
                pagamento.DataVencimento = request.DataVencimento;

            if (request.DataPagamento.HasValue)
                pagamento.DataPagamento = request.DataPagamento;

            if (!string.IsNullOrWhiteSpace(request.Observacoes))
                pagamento.Observacoes = request.Observacoes;

            pagamento.CalcularStatus();

            var pagamentoSalvo = await _pagamentoRepository.CriarAsync(pagamento);
            return MapearParaResponse(pagamentoSalvo);
        }

        public async Task<List<ListaPagamentosResponse>> ListarPagamentosAsync(Guid contratoId)
        {
            var pagamentos = await _pagamentoRepository.ListarPorContratoAsync(contratoId);
            return pagamentos.Select(p => new ListaPagamentosResponse
            {
                Id = p.Id,
                ValorPago = p.ValorParcela,
                DataVencimento = p.DataVencimento,
                DataPagamento = p.DataPagamento,
                StatusPagamento = p.StatusPagamento.ToString(),
                DataCriacao = p.DataCriacao
            }).ToList();
        }

        public async Task<PagamentoResponse?> ObterPorIdAsync(Guid pagamentoId)
        {
            var pagamento = await _pagamentoRepository.ObterPorIdAsync(pagamentoId);
            if (pagamento == null)
                return null;

            return MapearParaResponse(pagamento);
        }

        private PagamentoResponse MapearParaResponse(Pagamento pagamento)
        {
            return new PagamentoResponse
            {
                Id = pagamento.Id,
                ContratoId = pagamento.ContratoId,
                ValorPago = pagamento.ValorParcela,
                DataVencimento = pagamento.DataVencimento,
                DataPagamento = pagamento.DataPagamento,
                StatusPagamento = pagamento.StatusPagamento.ToString(),
                Observacoes = pagamento.Observacoes,
                DataCriacao = pagamento.DataCriacao
            };
        }
    }
}
