using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Features.Payments;
using Microsoft.AspNetCore.Mvc;

namespace DesafioVWFS.Controllers
{
    [ApiController]
    [Route("api/contratos/{contratoId}/pagamentos")]
    public class PagamentosController : ControllerBase
    {
        private readonly IPaymentsApplicationService _paymentsApplicationService;
        private readonly ILogger<PagamentosController> _logger;

        public PagamentosController(IPaymentsApplicationService paymentsApplicationService, ILogger<PagamentosController> logger)
        {
            _paymentsApplicationService = paymentsApplicationService;
            _logger = logger;
        }

        /// <summary>
        /// Registrar novo pagamento/parcela
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PagamentoResponse>> RegistrarPagamento(
            Guid contratoId,
            [FromBody] RegistrarPagamentoRequest request)
        {
            try
            {
                _logger.LogInformation("Registrando pagamento para contrato: {ContratoId}", contratoId);
                var pagamento = await _paymentsApplicationService.RegisterPaymentAsync(contratoId, request);
                return CreatedAtAction(nameof(ObterPagamento), new { contratoId, pagamentoId = pagamento.Id }, pagamento);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Erro ao registrar pagamento: {Mensagem}", ex.Message);
                return NotFound(new { mensagem = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao registrar pagamento: {Mensagem}", ex.Message);
                return StatusCode(500, new { mensagem = "Erro ao registrar pagamento" });
            }
        }

        /// <summary>
        /// Listar pagamentos do contrato
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ListaPagamentosResponse>>> ListarPagamentos(Guid contratoId)
        {
            try
            {
                var pagamentos = await _paymentsApplicationService.ListPaymentsAsync(contratoId);
                return Ok(pagamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao listar pagamentos: {Mensagem}", ex.Message);
                return StatusCode(500, new { mensagem = "Erro ao listar pagamentos" });
            }
        }

        /// <summary>
        /// Obter pagamento específico
        /// </summary>
        [HttpGet("{pagamentoId}")]
        public async Task<ActionResult<PagamentoResponse>> ObterPagamento(Guid contratoId, Guid pagamentoId)
        {
            var pagamento = await _paymentsApplicationService.GetPaymentAsync(pagamentoId);
            if (pagamento == null || pagamento.ContratoId != contratoId)
            {
                _logger.LogWarning("Pagamento não encontrado: {PagamentoId}", pagamentoId);
                return NotFound(new { mensagem = "Pagamento não encontrado" });
            }

            return Ok(pagamento);
        }
    }
}
