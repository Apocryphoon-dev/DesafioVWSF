using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Features.Payments.GetPayment.Handlers;
using DesafioVWFS.Application.Features.Payments.GetPayment.Models;
using DesafioVWFS.Application.Features.Payments.InsertPayment.Handlers;
using DesafioVWFS.Application.Features.Payments.InsertPayment.Models;
using DesafioVWFS.Application.Features.Payments.ListPayments.Handlers;
using DesafioVWFS.Application.Features.Payments.ListPayments.Models;

namespace DesafioVWFS.Application.Features.Payments;

public interface IPaymentsApplicationService
{
    Task<PagamentoResponse> RegisterPaymentAsync(Guid contratoId, RegistrarPagamentoRequest request);
    Task<List<ListaPagamentosResponse>> ListPaymentsAsync(Guid contratoId);
    Task<PagamentoResponse?> GetPaymentAsync(Guid pagamentoId);
}

public class PaymentsApplicationService : IPaymentsApplicationService
{
    private readonly InsertPaymentUseCase _insertPaymentUseCase;
    private readonly ListPaymentsUseCase _listPaymentsUseCase;
    private readonly GetPaymentUseCase _getPaymentUseCase;

    public PaymentsApplicationService(
        InsertPaymentUseCase insertPaymentUseCase,
        ListPaymentsUseCase listPaymentsUseCase,
        GetPaymentUseCase getPaymentUseCase)
    {
        _insertPaymentUseCase = insertPaymentUseCase;
        _listPaymentsUseCase = listPaymentsUseCase;
        _getPaymentUseCase = getPaymentUseCase;
    }

    public async Task<PagamentoResponse> RegisterPaymentAsync(Guid contratoId, RegistrarPagamentoRequest request)
    {
        var output = await _insertPaymentUseCase.ExecuteAsync(new InsertPaymentInput
        {
            ContratoId = contratoId,
            ValorPago = request.ValorPago,
            DataVencimento = request.DataVencimento,
            DataPagamento = request.DataPagamento,
            Observacoes = request.Observacoes
        }, CancellationToken.None);

        if (!output.IsValid)
            throw new InvalidOperationException(string.Join("; ", output.ErrorMessages));

        var result = output.GetResult();
        return new PagamentoResponse
        {
            Id = result.Id,
            ContratoId = result.ContratoId,
            ValorPago = result.ValorPago,
            DataVencimento = result.DataVencimento,
            DataPagamento = result.DataPagamento,
            StatusPagamento = result.StatusPagamento,
            Observacoes = result.Observacoes,
            DataCriacao = result.DataCriacao
        };
    }

    public async Task<List<ListaPagamentosResponse>> ListPaymentsAsync(Guid contratoId)
    {
        var output = await _listPaymentsUseCase.ExecuteAsync(new ListPaymentsInput { ContratoId = contratoId }, CancellationToken.None);

        var result = output.GetResult();
        return result.Pagamentos.Select(p => new ListaPagamentosResponse
        {
            Id = p.Id,
            ValorPago = p.ValorPago,
            DataVencimento = p.DataVencimento,
            DataPagamento = p.DataPagamento,
            StatusPagamento = p.StatusPagamento,
            DataCriacao = p.DataCriacao
        }).ToList();
    }

    public async Task<PagamentoResponse?> GetPaymentAsync(Guid pagamentoId)
    {
        var output = await _getPaymentUseCase.ExecuteAsync(new GetPaymentInput { PagamentoId = pagamentoId }, CancellationToken.None);
        if (!output.IsValid)
            return null;

        var result = output.GetResult();
        return new PagamentoResponse
        {
            Id = result.Id,
            ContratoId = result.ContratoId,
            ValorPago = result.ValorPago,
            DataVencimento = result.DataVencimento,
            DataPagamento = result.DataPagamento,
            StatusPagamento = result.StatusPagamento,
            Observacoes = result.Observacoes,
            DataCriacao = result.DataCriacao
        };
    }
}
