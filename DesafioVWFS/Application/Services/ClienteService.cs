using DesafioVWFS.Application.DTOs;
using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Domain.Enums;
using DesafioVWFS.Application.Shared.Domain.Repositories;
using DesafioVWFS.Application.Shared.Domain.Services;
using DesafioVWFS.Application.Validators;

namespace DesafioVWFS.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IContratoRepository _contratoRepository;

        public ClienteService(IContratoRepository contratoRepository)
        {
            _contratoRepository = contratoRepository;
        }

        public async Task<ResumoClienteResponse> ObterResumoAsync(string cpfCnpj)
        {
            if (!CpfCnpjValidator.ValidarCpfCnpj(cpfCnpj))
                throw new ArgumentException("CPF/CNPJ inválido");

            var contratos = await _contratoRepository.ObterPorClienteAsync(cpfCnpj);

            if (!contratos.Any())
                throw new InvalidOperationException("Nenhum contrato encontrado para este cliente");

            var contratosAtivos = contratos.Count(c => c.Ativo);
            var totalParcelas = contratos.Sum(c => c.PrazoMeses);
            var totalPagamentos = contratos.SelectMany(c => c.Pagamentos).ToList();
            var parcelasPagas = totalPagamentos.Count(p => p.DataPagamento.HasValue);
            var parcelasAtrasadas = totalPagamentos.Count(p => p.StatusPagamento == StatusPagamento.ATRASADO && !p.DataPagamento.HasValue);
            var parcelasAVencer = totalParcelas - parcelasPagas - parcelasAtrasadas;
            var parcelasPagasEmDia = totalPagamentos.Count(p => p.DataPagamento.HasValue && p.StatusPagamento == StatusPagamento.EMDIA);
            var percentualParcelasPagasEmDia = totalParcelas > 0 ? (decimal)parcelasPagasEmDia / totalParcelas * 100m : 0m;
            var saldoDevedorConsolidado = contratos.Sum(c =>
            {
                var ultimoPagamento = c.Pagamentos.OrderByDescending(p => p.DataCriacao).FirstOrDefault();
                return ultimoPagamento?.SaldoDevedor ?? c.ValorTotal;
            });

            return new ResumoClienteResponse
            {
                ClienteCpfCnpj = cpfCnpj,
                ContratosAtivos = contratosAtivos,
                TotalParcelas = totalParcelas,
                ParcelasPagas = parcelasPagas,
                ParcelasAtrasadas = parcelasAtrasadas,
                ParcelasAVencer = parcelasAVencer,
                PercentualParcelasPagasEmDia = Math.Round(percentualParcelasPagasEmDia, 2),
                SaldoDevedorConsolidado = Math.Round(saldoDevedorConsolidado, 2)
            };
        }
    }
}
