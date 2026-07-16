using DesafioVWFS.Application.Features.Contracts.ListContracts.Models;
using DesafioVWFS.Application.Shared.Core;
using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Repository;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DesafioVWFS.Application.Features.Contracts.ListContracts.Handlers;

public class ListContractsUseCase : UseCaseHandlerBase<ListContractsInput, ListContractsOutput>
{
    private readonly IContratoRepository _contratoRepository;

    public ListContractsUseCase(
        ILogger<UseCaseHandlerBase<ListContractsInput, ListContractsOutput>> logger,
        IContratoRepository contratoRepository,
        IValidator<ListContractsInput>? validator = null)
        : base(logger, validator)
    {
        _contratoRepository = contratoRepository;
    }

    protected override async Task<Output<ListContractsOutput>> HandleAsync(ListContractsInput input, CancellationToken cancellationToken)
    {
        var output = new Output<ListContractsOutput>();

        var contratos = await _contratoRepository.ListarTodosAsync(input.Page, input.PageSize, input.OrdenarDescendente);

        output.AddResult(new ListContractsOutput
        {
            Contratos = contratos.Select(MapToItem).ToList()
        });

        return output;
    }

    private static ListContractsItem MapToItem(Contrato contrato)
    {
        var ultimoPagamento = contrato.Pagamentos.OrderByDescending(p => p.DataCriacao).FirstOrDefault();

        return new ListContractsItem
        {
            Id = contrato.Id,
            ClienteCpfCnpj = contrato.ClienteCpfCnpj,
            ValorTotal = contrato.ValorTotal,
            SaldoDevedor = ultimoPagamento?.SaldoDevedor ?? contrato.ValorTotal,
            Ativo = contrato.Ativo,
            DataCriacao = contrato.DataCriacao
        };
    }
}
