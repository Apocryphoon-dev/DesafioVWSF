using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Domain.Enums;
using DesafioVWFS.Application.Shared.Domain.Repositories;
using DesafioVWFS.Application.Shared.Domain.Services;
using DesafioVWFS.Application.Validators;

namespace DesafioVWFS.Application.Services
{
    public class ContratoService : IContratoService
    {
        private readonly IContratoRepository _contratoRepository;
        private readonly IPagamentoRepository _pagamentoRepository;

        public ContratoService(IContratoRepository contratoRepository, IPagamentoRepository pagamentoRepository)
        {
            _contratoRepository = contratoRepository;
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<ContratoResponse?> ObterPorIdAsync(Guid id)
        {
            var contrato = await _contratoRepository.ObterPorIdAsync(id);
            if (contrato == null)
                return null;

            return MapearParaResponse(contrato);
        }

        public async Task<List<ListaContratoResponse>> ListarTodosAsync(int page = 1, int pageSize = 10, bool ordenarDescendente = false)
        {
            var contratos = await _contratoRepository.ListarTodosAsync(page, pageSize, ordenarDescendente);
            return contratos.Select(c => new ListaContratoResponse
            {
                Id = c.Id,
                ClienteCpfCnpj = c.ClienteCpfCnpj,
                ValorTotal = c.ValorTotal,
                SaldoDevedor = c.Pagamentos.Any() ? c.Pagamentos.OrderByDescending(p => p.DataCriacao).First().SaldoDevedor : c.ValorTotal,
                Ativo = c.Ativo,
                DataCriacao = c.DataCriacao
            }).ToList();
        }

        public async Task<List<ContratoResponse>> ObterPorClienteAsync(string cpfCnpj)
        {
            if (!CpfCnpjValidator.ValidarCpfCnpj(cpfCnpj))
                throw new ArgumentException("CPF/CNPJ inválido");

            var contratos = await _contratoRepository.ObterPorClienteAsync(cpfCnpj);
            return contratos.Select(MapearParaResponse).ToList();
        }

        public async Task<ContratoResponse> CriarAsync(CriarContratoRequest request)
        {
            if (!CpfCnpjValidator.ValidarCpfCnpj(request.ClienteCpfCnpj))
                throw new ArgumentException("CPF/CNPJ inválido");

            if (!Enum.TryParse<TipoVeiculo>(request.TipoVeiculo, out var tipoVeiculo))
                throw new ArgumentException("Tipo de veículo inválido");

            if (!Enum.TryParse<CondicaoVeiculo>(request.CondicaoVeiculo, out var condicaoVeiculo))
                throw new ArgumentException("Condição do veículo inválida");

            var contrato = new Contrato
            {
                Id = Guid.NewGuid(),
                ClienteCpfCnpj = request.ClienteCpfCnpj,
                ValorTotal = request.ValorTotal,
                TaxaMensal = request.TaxaMensal,
                PrazoMeses = request.PrazoMeses,
                DataVencimentoPrimeiraParcela = request.DataVencimentoPrimeiraParcela,
                TipoVeiculo = tipoVeiculo,
                CondicaoVeiculo = condicaoVeiculo,
                DataCriacao = DateTime.UtcNow,
                Ativo = true
            };

            var contratoSalvo = await _contratoRepository.CriarAsync(contrato);
            return MapearParaResponse(contratoSalvo);
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            return await _contratoRepository.DeletarAsync(id);
        }

        private ContratoResponse MapearParaResponse(Contrato contrato)
        {
            var ultimoPagamento = contrato.Pagamentos.OrderByDescending(p => p.DataCriacao).FirstOrDefault();
            var saldoDevedor = ultimoPagamento?.SaldoDevedor ?? contrato.ValorTotal;
            var totalParcelas = contrato.PrazoMeses;
            var parcelasPagas = contrato.Pagamentos.Count(p => p.DataPagamento.HasValue);
            var parcelasAtrasadas = contrato.Pagamentos.Count(p => p.StatusPagamento == StatusPagamento.ATRASADO && !p.DataPagamento.HasValue);
            var parcelasAVencer = totalParcelas - parcelasPagas - parcelasAtrasadas;

            return new ContratoResponse
            {
                Id = contrato.Id,
                ClienteCpfCnpj = contrato.ClienteCpfCnpj,
                ValorTotal = contrato.ValorTotal,
                TaxaMensal = contrato.TaxaMensal,
                PrazoMeses = contrato.PrazoMeses,
                DataVencimentoPrimeiraParcela = contrato.DataVencimentoPrimeiraParcela,
                TipoVeiculo = contrato.TipoVeiculo.ToString(),
                CondicaoVeiculo = contrato.CondicaoVeiculo.ToString(),
                SaldoDevedor = saldoDevedor,
                TotalParcelas = totalParcelas,
                ParcelasPagas = parcelasPagas,
                ParcelasAtrasadas = parcelasAtrasadas,
                ParcelasAVencer = parcelasAVencer,
                Ativo = contrato.Ativo,
                DataCriacao = contrato.DataCriacao
            };
        }
    }
}
