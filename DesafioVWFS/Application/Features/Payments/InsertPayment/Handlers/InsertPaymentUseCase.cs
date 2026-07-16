using DesafioVWFS.Application.Features.Payments.InsertPayment.Models;
using DesafioVWFS.Application.Shared.Core;
using DesafioVWFS.Application.Shared.Domain.Repositories;
using DesafioVWFS.Application.Shared.Domain.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DesafioVWFS.Application.Features.Payments.InsertPayment.Handlers;

public class InsertPaymentUseCase : UseCaseHandlerBase<InsertPaymentInput, InsertPaymentOutput>
{
    private readonly IContratoRepository _contratoRepository;
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly IPagamentoCalculoService _calculoService;

    public InsertPaymentUseCase(
        ILogger<UseCaseHandlerBase<InsertPaymentInput, InsertPaymentOutput>> logger,
        IContratoRepository contratoRepository,
        IPagamentoRepository pagamentoRepository,
        IPagamentoCalculoService calculoService,
        IValidator<InsertPaymentInput>? validator = null)
        : base(logger, validator)
    {
        _contratoRepository = contratoRepository;
        _pagamentoRepository = pagamentoRepository;
        _calculoService = calculoService;
    }

    protected override async Task<Output<InsertPaymentOutput>> HandleAsync(InsertPaymentInput input, CancellationToken cancellationToken)
    {
        var output = new Output<InsertPaymentOutput>();
        var contrato = await _contratoRepository.ObterPorIdAsync(input.ContratoId);

        if (contrato is null)
        {
            output.AddErrorMessage("Contrato não encontrado");
            return output;
        }

        var pagamentosExistentes = (await _pagamentoRepository.ListarPorContratoAsync(input.ContratoId))
            .OrderBy(p => p.DataCriacao)
            .ToList();

        DateTime dataVencimentoAnterior = pagamentosExistentes.Any()
            ? pagamentosExistentes.Last().DataVencimento
            : contrato.DataVencimentoPrimeiraParcela;

        var pagamento = _calculoService.CalcularParcela(contrato, pagamentosExistentes, dataVencimentoAnterior);

        if (input.DataVencimento != default)
            pagamento.DataVencimento = input.DataVencimento;

        if (input.DataPagamento.HasValue)
            pagamento.DataPagamento = input.DataPagamento;

        if (!string.IsNullOrWhiteSpace(input.Observacoes))
            pagamento.Observacoes = input.Observacoes;

        pagamento.CalcularStatus();
        var saved = await _pagamentoRepository.CriarAsync(pagamento);

        output.AddResult(new InsertPaymentOutput
        {
            Id = saved.Id,
            ContratoId = saved.ContratoId,
            ValorPago = saved.ValorParcela,
            DataVencimento = saved.DataVencimento,
            DataPagamento = saved.DataPagamento,
            StatusPagamento = saved.StatusPagamento.ToString(),
            Observacoes = saved.Observacoes,
            DataCriacao = saved.DataCriacao
        });

        return output;
    }
}
