using DesafioVWFS.Application.Shared.Core;

namespace DesafioVWFS.Application.Features.Contracts.GetContract.Models;

public class GetContractInput : IRequest<GetContractOutput>
{
    public Guid Id { get; set; }
    public Guid CorrelationId { get; set; }
}
