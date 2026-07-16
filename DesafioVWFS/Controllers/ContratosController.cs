using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Features.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DesafioVWFS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratosController : ControllerBase
    {
        private readonly IContractsApplicationService _contractsApplicationService;
        private readonly ILogger<ContratosController> _logger;

        public ContratosController(IContractsApplicationService contractsApplicationService, ILogger<ContratosController> logger)
        {
            _contractsApplicationService = contractsApplicationService;
            _logger = logger;
        }

        /// <summary>
        /// Criar novo contrato de financiamento
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ContratoResponse>> CriarContrato([FromBody] CriarContratoRequest request)
        {
            try
            {
                _logger.LogInformation("Criando novo contrato para cliente: {ClienteCpfCnpj}", request.ClienteCpfCnpj);
                var contrato = await _contractsApplicationService.CreateContractAsync(request);
                return CreatedAtAction(nameof(ObterContrato), new { id = contrato.Id }, contrato);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Erro ao validar contrato: {Mensagem}", ex.Message);
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Obter contrato por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ContratoResponse>> ObterContrato(Guid id)
        {
            var contrato = await _contractsApplicationService.GetContractAsync(id);
            if (contrato == null)
            {
                _logger.LogWarning("Contrato não encontrado: {ContratoId}", id);
                return NotFound(new { mensagem = "Contrato não encontrado" });
            }

            return Ok(contrato);
        }

        /// <summary>
        /// Listar todos os contratos com paginação
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ListaContratoResponse>>> ListarContratos(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] bool ordenarDescendente = false)
        {
            var contratos = await _contractsApplicationService.ListContractsAsync(page, pageSize, ordenarDescendente);
            return Ok(contratos);
        }

        /// <summary>
        /// Deletar contrato
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarContrato(Guid id)
        {
            var resultado = await _contractsApplicationService.DeleteContractAsync(id);
            if (!resultado)
            {
                _logger.LogWarning("Tentativa de deletar contrato não encontrado: {ContratoId}", id);
                return NotFound(new { mensagem = "Contrato não encontrado" });
            }

            _logger.LogInformation("Contrato deletado: {ContratoId}", id);
            return NoContent();
        }
    }
}
