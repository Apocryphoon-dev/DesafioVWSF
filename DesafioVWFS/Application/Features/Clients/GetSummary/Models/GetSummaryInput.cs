using DesafioVWFS.Application.Shared.Core;

namespace DesafioVWFS.Application.Features.Clients.GetSummary.Models;

public class GetSummaryInput : IRequest<GetSummaryOutput>
{
    public string CpfCnpj { get; set; } = string.Empty;
    public Guid CorrelationId { get; set; }
}
