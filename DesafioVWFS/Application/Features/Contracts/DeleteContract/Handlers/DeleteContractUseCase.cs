using DesafioVWFS.Application.Features.Contracts.DeleteContract.Models;
using DesafioVWFS.Application.Shared.Core;
using DesafioVWFS.Application.Shared.Repository;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DesafioVWFS.Application.Features.Contracts.DeleteContract.Handlers;

public class DeleteContractUseCase : UseCaseHandlerBase<DeleteContractInput, DeleteContractOutput>
{
    private readonly IContratoRepository _contratoRepository;

    public DeleteContractUseCase(
        ILogger<UseCaseHandlerBase<DeleteContractInput, DeleteContractOutput>> logger,
        IContratoRepository contratoRepository,
        IValidator<DeleteContractInput>? validator = null)
        : base(logger, validator)
    {
        _contratoRepository = contratoRepository;
    }

    protected override async Task<Output<DeleteContractOutput>> HandleAsync(DeleteContractInput input, CancellationToken cancellationToken)
    {
        var output = new Output<DeleteContractOutput>();
        var deleted = await _contratoRepository.DeletarAsync(input.Id);
        output.AddResult(new DeleteContractOutput { Success = deleted });
        return output;
    }
}
