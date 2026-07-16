using DesafioVWFS.Application.Shared.Core;

namespace DesafioVWFS.Application.Features.Contracts.DeleteContract.Models;

public class DeleteContractInput : IRequest<DeleteContractOutput>
{
    public Guid Id { get; set; }
    public Guid CorrelationId { get; set; }
}
