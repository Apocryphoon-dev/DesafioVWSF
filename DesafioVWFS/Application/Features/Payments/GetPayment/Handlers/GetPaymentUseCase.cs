using DesafioVWFS.Application.Features.Payments.GetPayment.Models;
using DesafioVWFS.Application.Shared.Core;
using DesafioVWFS.Application.Shared.Repository;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DesafioVWFS.Application.Features.Payments.GetPayment.Handlers;

public class GetPaymentUseCase : UseCaseHandlerBase<GetPaymentInput, GetPaymentOutput>
{
    private readonly IPagamentoRepository _pagamentoRepository;

    public GetPaymentUseCase(
        ILogger<UseCaseHandlerBase<GetPaymentInput, GetPaymentOutput>> logger,
        IPagamentoRepository pagamentoRepository,
        IValidator<GetPaymentInput>? validator = null)
        : base(logger, validator)
    {
        _pagamentoRepository = pagamentoRepository;
    }

    protected override async Task<Output<GetPaymentOutput>> HandleAsync(GetPaymentInput input, CancellationToken cancellationToken)
    {
        var output = new Output<GetPaymentOutput>();

        var pagamento = await _pagamentoRepository.ObterPorIdAsync(input.PagamentoId);
        if (pagamento is null)
        {
            output.AddErrorMessage("Pagamento não encontrado");
            return output;
        }

        output.AddResult(new GetPaymentOutput
        {
            Id = pagamento.Id,
            ContratoId = pagamento.ContratoId,
            ValorPago = pagamento.ValorParcela,
            DataVencimento = pagamento.DataVencimento,
            DataPagamento = pagamento.DataPagamento,
            StatusPagamento = pagamento.StatusPagamento.ToString(),
            Observacoes = pagamento.Observacoes,
            DataCriacao = pagamento.DataCriacao
        });

        return output;
    }
}
