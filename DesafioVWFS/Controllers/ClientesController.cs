using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Features.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioVWFS.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClientsApplicationService _clientsApplicationService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClientsApplicationService clientsApplicationService, ILogger<ClientesController> logger)
        {
            _clientsApplicationService = clientsApplicationService;
            _logger = logger;
        }

        /// <summary>
        /// Obter resumo consolidado do cliente
        /// </summary>
        [HttpGet("{cpfCnpj}/resumo")]
        public async Task<ActionResult<ResumoClienteResponse>> ObterResumo(string cpfCnpj)
        {
            try
            {
                _logger.LogInformation("Obtendo resumo para cliente: {CpfCnpj}", cpfCnpj);
                var resumo = await _clientsApplicationService.GetCustomerSummaryAsync(cpfCnpj);
                return Ok(resumo);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("CPF/CNPJ inválido: {Mensagem}", ex.Message);
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Cliente não encontrado: {Mensagem}", ex.Message);
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
