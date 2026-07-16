using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Shared.Domain.Repositories;
using DesafioVWFS.Application.Shared.Domain.Services;

namespace DesafioVWFS.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IContratoRepository _contratoRepository;

        public ClienteService(IContratoRepository contratoRepository)
        {
            _contratoRepository = contratoRepository;
        }

        public Task<ResumoClienteResponse> ObterResumoAsync(string cpfCnpj)
        {
            throw new NotSupportedException("Use a implementação em Application/Services.");
        }
    }
}
