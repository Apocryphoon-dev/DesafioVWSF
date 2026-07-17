using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Features.Contracts.CreateContract.Handlers;
using DesafioVWFS.Application.Features.Contracts.CreateContract.Models;
using DesafioVWFS.Application.Features.Contracts.DeleteContract.Handlers;
using DesafioVWFS.Application.Features.Contracts.DeleteContract.Models;
using DesafioVWFS.Application.Features.Contracts.GetContract.Models;
using DesafioVWFS.Application.Features.Contracts.GetContract.UseCase;
using DesafioVWFS.Application.Features.Contracts.ListContracts.Handlers;
using DesafioVWFS.Application.Features.Contracts.ListContracts.Models;
using DesafioVWFS.Application.Validators;

namespace DesafioVWFS.Application.Features.Contracts;

public interface IContractsApplicationService
{
    Task<ContratoResponse?> GetContractAsync(Guid id);
    Task<List<ListaContratoResponse>> ListContractsAsync(int page = 1, int pageSize = 10, bool ordenarDescendente = false);
    Task<ContratoResponse> CreateContractAsync(CriarContratoRequest request);
    Task<bool> DeleteContractAsync(Guid id);
}

public class ContractsApplicationService : IContractsApplicationService
{
    private readonly CreateContractUseCase _createContractUseCase;
    private readonly GetContractUseCase _getContractUseCase;
    private readonly ListContractsUseCase _listContractsUseCase;
    private readonly DeleteContractUseCase _deleteContractUseCase;

    public ContractsApplicationService(
        CreateContractUseCase createContractUseCase,
        GetContractUseCase getContractUseCase,
        ListContractsUseCase listContractsUseCase,
        DeleteContractUseCase deleteContractUseCase)
    {
        _createContractUseCase = createContractUseCase;
        _getContractUseCase = getContractUseCase;
        _listContractsUseCase = listContractsUseCase;
        _deleteContractUseCase = deleteContractUseCase;
    }

    public async Task<ContratoResponse?> GetContractAsync(Guid id)
    {
        var output = await _getContractUseCase.ExecuteAsync(new GetContractInput { Id = id }, CancellationToken.None);
        if (!output.IsValid)
            return null;

        var result = output.GetResult();
        return new ContratoResponse
        {
            Id = result.Id,
            ClienteCpfCnpj = CpfCnpjValidator.FormatarCpfCnpj(result.ClienteCpfCnpj),
            ValorTotal = result.ValorTotal,
            TaxaMensal = result.TaxaMensal,
            PrazoMeses = result.PrazoMeses,
            DataVencimentoPrimeiraParcela = result.DataVencimentoPrimeiraParcela,
            TipoVeiculo = result.TipoVeiculo,
            CondicaoVeiculo = result.CondicaoVeiculo,
            SaldoDevedor = result.SaldoDevedor,
            TotalParcelas = result.TotalParcelas,
            ParcelasPagas = result.ParcelasPagas,
            ParcelasAtrasadas = result.ParcelasAtrasadas,
            ParcelasAVencer = result.ParcelasAVencer,
            Ativo = result.Ativo,
            DataCriacao = result.DataCriacao
        };
    }

    public async Task<List<ListaContratoResponse>> ListContractsAsync(int page = 1, int pageSize = 10, bool ordenarDescendente = false)
    {
        var output = await _listContractsUseCase.ExecuteAsync(
            new ListContractsInput { Page = page, PageSize = pageSize, OrdenarDescendente = ordenarDescendente },
            CancellationToken.None);

        var result = output.GetResult();
        return result.Contratos.Select(c => new ListaContratoResponse
        {
            Id = c.Id,
            ClienteCpfCnpj = CpfCnpjValidator.FormatarCpfCnpj(c.ClienteCpfCnpj),
            ValorTotal = c.ValorTotal,
            SaldoDevedor = c.SaldoDevedor,
            Ativo = c.Ativo,
            DataCriacao = c.DataCriacao
        }).ToList();
    }

    public async Task<ContratoResponse> CreateContractAsync(CriarContratoRequest request)
    {
        var output = await _createContractUseCase.ExecuteAsync(new CreateContractInput
        {
            ClienteCpfCnpj = request.ClienteCpfCnpj,
            ValorTotal = request.ValorTotal,
            TaxaMensal = request.TaxaMensal,
            PrazoMeses = request.PrazoMeses,
            DataVencimentoPrimeiraParcela = request.DataVencimentoPrimeiraParcela,
            TipoVeiculo = request.TipoVeiculo,
            CondicaoVeiculo = request.CondicaoVeiculo
        }, CancellationToken.None);

        if (!output.IsValid)
            throw new ArgumentException(string.Join("; ", output.ErrorMessages));

        var result = output.GetResult();
        return new ContratoResponse
        {
            Id = result.Id,
            ClienteCpfCnpj = CpfCnpjValidator.FormatarCpfCnpj(result.ClienteCpfCnpj),
            ValorTotal = result.ValorTotal,
            TaxaMensal = result.TaxaMensal,
            PrazoMeses = result.PrazoMeses,
            DataVencimentoPrimeiraParcela = result.DataVencimentoPrimeiraParcela,
            TipoVeiculo = result.TipoVeiculo,
            CondicaoVeiculo = result.CondicaoVeiculo,
            Ativo = result.Ativo
        };
    }

    public async Task<bool> DeleteContractAsync(Guid id)
    {
        var output = await _deleteContractUseCase.ExecuteAsync(new DeleteContractInput { Id = id }, CancellationToken.None);
        return output.IsValid && output.GetResult().Success;
    }
}
