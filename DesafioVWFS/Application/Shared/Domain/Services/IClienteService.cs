using DesafioVWFS.Application.DTOs;

namespace DesafioVWFS.Application.Shared.Domain.Services;

public interface IClienteService
{
    Task<ResumoClienteResponse> ObterResumoAsync(string cpfCnpj);
}
