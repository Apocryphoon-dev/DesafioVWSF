using DesafioVWFS.Application.Features.Payments.ListPayments.Models;
using DesafioVWFS.Application.Shared.Core;
using DesafioVWFS.Application.Shared.Repository;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DesafioVWFS.Application.Features.Payments.ListPayments.Handlers;

public class ListPaymentsUseCase : UseCaseHandlerBase<ListPaymentsInput, ListPaymentsOutput>
{
    private readonly IPagamentoRepository _pagamentoRepository;

    public ListPaymentsUseCase(
        ILogger<UseCaseHandlerBase<ListPaymentsInput, ListPaymentsOutput>> logger,
        IPagamentoRepository pagamentoRepository,
        IValidator<ListPaymentsInput>? validator = null)
        : base(logger, validator)
    {
        _pagamentoRepository = pagamentoRepository;
    }

    protected override async Task<Output<ListPaymentsOutput>> HandleAsync(ListPaymentsInput input, CancellationToken cancellationToken)
    {
        var output = new Output<ListPaymentsOutput>();

        var pagamentos = await _pagamentoRepository.ListarPorContratoAsync(input.ContratoId);

        output.AddResult(new ListPaymentsOutput
        {
            Pagamentos = pagamentos.Select(p => new ListPaymentsItem
            {
                Id = p.Id,
                ValorPago = p.ValorParcela,
                DataVencimento = p.DataVencimento,
                DataPagamento = p.DataPagamento,
                StatusPagamento = p.StatusPagamento.ToString(),
                DataCriacao = p.DataCriacao
            }).ToList()
        });

        return output;
    }
}
