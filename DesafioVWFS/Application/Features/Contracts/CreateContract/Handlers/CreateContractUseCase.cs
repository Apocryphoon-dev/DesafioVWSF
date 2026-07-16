using DesafioVWFS.Application.Features.Contracts.CreateContract.Models;
using DesafioVWFS.Application.Shared.Core;
using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Domain.Enums;
using DesafioVWFS.Application.Shared.Domain.Repositories;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DesafioVWFS.Application.Features.Contracts.CreateContract.Handlers;

public class CreateContractUseCase : UseCaseHandlerBase<CreateContractInput, CreateContractOutput>
{
    private readonly IContratoRepository _contratoRepository;

    public CreateContractUseCase(
        ILogger<UseCaseHandlerBase<CreateContractInput, CreateContractOutput>> logger,
        IContratoRepository contratoRepository,
        IValidator<CreateContractInput>? validator = null)
        : base(logger, validator)
    {
        _contratoRepository = contratoRepository;
    }

    protected override async Task<Output<CreateContractOutput>> HandleAsync(CreateContractInput input, CancellationToken cancellationToken)
    {
        var output = new Output<CreateContractOutput>();

        var entity = new Contrato
        {
            Id = Guid.NewGuid(),
            ClienteCpfCnpj = input.ClienteCpfCnpj,
            ValorTotal = input.ValorTotal,
            TaxaMensal = input.TaxaMensal,
            PrazoMeses = input.PrazoMeses,
            DataVencimentoPrimeiraParcela = input.DataVencimentoPrimeiraParcela,
            TipoVeiculo = Enum.Parse<TipoVeiculo>(input.TipoVeiculo, true),
            CondicaoVeiculo = Enum.Parse<CondicaoVeiculo>(input.CondicaoVeiculo, true),
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        var created = await _contratoRepository.CriarAsync(entity);
        output.AddResult(new CreateContractOutput
        {
            Id = created.Id,
            ClienteCpfCnpj = created.ClienteCpfCnpj,
            ValorTotal = created.ValorTotal,
            TaxaMensal = created.TaxaMensal,
            PrazoMeses = created.PrazoMeses,
            DataVencimentoPrimeiraParcela = created.DataVencimentoPrimeiraParcela,
            TipoVeiculo = created.TipoVeiculo.ToString(),
            CondicaoVeiculo = created.CondicaoVeiculo.ToString(),
            Ativo = created.Ativo
        });

        return output;
    }
}
