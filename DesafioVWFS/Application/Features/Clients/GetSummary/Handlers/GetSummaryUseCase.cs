using DesafioVWFS.Application.Features.Clients.GetSummary.Models;
using DesafioVWFS.Application.Shared.Core;
using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Domain.Enums;
using DesafioVWFS.Application.Shared.Repository;
using DesafioVWFS.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DesafioVWFS.Application.Features.Clients.GetSummary.Handlers;

public class GetSummaryUseCase : UseCaseHandlerBase<GetSummaryInput, GetSummaryOutput>
{
    private readonly IContratoRepository _contratoRepository;

    public GetSummaryUseCase(
        ILogger<UseCaseHandlerBase<GetSummaryInput, GetSummaryOutput>> logger,
        IContratoRepository contratoRepository,
        IValidator<GetSummaryInput>? validator = null)
        : base(logger, validator)
    {
        _contratoRepository = contratoRepository;
    }

    protected override async Task<Output<GetSummaryOutput>> HandleAsync(GetSummaryInput input, CancellationToken cancellationToken)
    {
        var output = new Output<GetSummaryOutput>();

        if (!CpfCnpjValidator.ValidarCpfCnpj(input.CpfCnpj))
        {
            output.AddErrorMessage("CPF/CNPJ inválido");
            return output;
        }

        var contratos = await _contratoRepository.ObterPorClienteAsync(input.CpfCnpj);

        if (!contratos.Any())
        {
            output.AddErrorMessage("Nenhum contrato encontrado para este cliente");
            return output;
        }

        var contratosAtivos = contratos.Count(c => c.Ativo);
        var totalParcelas = contratos.Sum(c => c.PrazoMeses);
        var totalPagamentos = contratos.SelectMany(c => c.Pagamentos).ToList();
        var parcelasPagas = totalPagamentos.Count(p => p.DataPagamento.HasValue);
        var parcelasAtrasadas = totalPagamentos.Count(p => p.StatusPagamento == StatusPagamento.ATRASADO && !p.DataPagamento.HasValue);
        var parcelasAVencer = totalParcelas - parcelasPagas - parcelasAtrasadas;
        var parcelasPagasEmDia = totalPagamentos.Count(p => p.DataPagamento.HasValue && p.StatusPagamento == StatusPagamento.EMDIA);
        var percentualParcelasPagasEmDia = totalParcelas > 0 ? (decimal)parcelasPagasEmDia / totalParcelas * 100m : 0m;
        var saldoDevedorConsolidado = contratos.Sum(c =>
        {
            var ultimoPagamento = c.Pagamentos.OrderByDescending(p => p.DataCriacao).FirstOrDefault();
            return ultimoPagamento?.SaldoDevedor ?? c.ValorTotal;
        });

        output.AddResult(new GetSummaryOutput
        {
            ClienteCpfCnpj = input.CpfCnpj,
            ContratosAtivos = contratosAtivos,
            TotalParcelas = totalParcelas,
            ParcelasPagas = parcelasPagas,
            ParcelasAtrasadas = parcelasAtrasadas,
            ParcelasAVencer = parcelasAVencer,
            PercentualParcelasPagasEmDia = Math.Round(percentualParcelasPagasEmDia, 2),
            SaldoDevedorConsolidado = Math.Round(saldoDevedorConsolidado, 2)
        });

        return output;
    }
}
