using DesafioVWFS.Application.Shared.Domain.Services;
using DesafioVWFS.Application.DTOs;

namespace DesafioVWFS.Application.Features.Payments;

public interface IPaymentsApplicationService
{
    Task<PagamentoResponse> RegisterPaymentAsync(Guid contratoId, RegistrarPagamentoRequest request);
    Task<List<ListaPagamentosResponse>> ListPaymentsAsync(Guid contratoId);
    Task<PagamentoResponse?> GetPaymentAsync(Guid pagamentoId);
}

public class PaymentsApplicationService : IPaymentsApplicationService
{
    private readonly IPagamentoService _pagamentoService;

    public PaymentsApplicationService(IPagamentoService pagamentoService)
    {
        _pagamentoService = pagamentoService;
    }

    public Task<PagamentoResponse> RegisterPaymentAsync(Guid contratoId, RegistrarPagamentoRequest request)
        => _pagamentoService.RegistrarPagamentoAsync(contratoId, request);

    public Task<List<ListaPagamentosResponse>> ListPaymentsAsync(Guid contratoId)
        => _pagamentoService.ListarPagamentosAsync(contratoId);

    public Task<PagamentoResponse?> GetPaymentAsync(Guid pagamentoId)
        => _pagamentoService.ObterPorIdAsync(pagamentoId);
}
