using DesafioVWFS.Application.Features.Contracts.GetContract.Models;
using DesafioVWFS.Application.Shared.Core;
using DesafioVWFS.Application.Shared.Domain.Enums;
using DesafioVWFS.Application.Shared.Repository;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DesafioVWFS.Application.Features.Contracts.GetContract.UseCase;

public class GetContractUseCase : UseCaseHandlerBase<GetContractInput, GetContractOutput>
{
    private readonly IContratoRepository _contratoRepository;

    public GetContractUseCase(
        ILogger<UseCaseHandlerBase<GetContractInput, GetContractOutput>> logger,
        IContratoRepository contratoRepository,
        IValidator<GetContractInput>? validator = null)
        : base(logger, validator)
    {
        _contratoRepository = contratoRepository;
    }

    protected override async Task<Output<GetContractOutput>> HandleAsync(GetContractInput input, CancellationToken cancellationToken)
    {
        var output = new Output<GetContractOutput>();
        var contrato = await _contratoRepository.ObterPorIdAsync(input.Id);

        if (contrato is null)
        {
            output.AddErrorMessage("Contrato não encontrado");
            return output;
        }

        var ultimoPagamento = contrato.Pagamentos.OrderByDescending(p => p.DataCriacao).FirstOrDefault();
        var saldoDevedor = ultimoPagamento?.SaldoDevedor ?? contrato.ValorTotal;
        var parcelasPagas = contrato.Pagamentos.Count(p => p.DataPagamento.HasValue);
        var parcelasAtrasadas = contrato.Pagamentos.Count(p => p.StatusPagamento == StatusPagamento.ATRASADO && !p.DataPagamento.HasValue);
        var parcelasAVencer = contrato.PrazoMeses - parcelasPagas - parcelasAtrasadas;

        output.AddResult(new GetContractOutput
        {
            Id = contrato.Id,
            ClienteCpfCnpj = contrato.ClienteCpfCnpj,
            ValorTotal = contrato.ValorTotal,
            TaxaMensal = contrato.TaxaMensal,
            PrazoMeses = contrato.PrazoMeses,
            DataVencimentoPrimeiraParcela = contrato.DataVencimentoPrimeiraParcela,
            TipoVeiculo = contrato.TipoVeiculo.ToString(),
            CondicaoVeiculo = contrato.CondicaoVeiculo.ToString(),
            SaldoDevedor = saldoDevedor,
            TotalParcelas = contrato.PrazoMeses,
            ParcelasPagas = parcelasPagas,
            ParcelasAtrasadas = parcelasAtrasadas,
            ParcelasAVencer = parcelasAVencer,
            Ativo = contrato.Ativo,
            DataCriacao = contrato.DataCriacao
        });

        return output;
    }
}
