using DesafioVWFS.Application.Shared.Core;

namespace DesafioVWFS.Application.Features.Contracts.ListContracts.Models;

public class ListContractsInput : IRequest<ListContractsOutput>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool OrdenarDescendente { get; set; }
}
