using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioVWFS.Application.Shared.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly DesafioDbContext _context;

        public PagamentoRepository(DesafioDbContext context)
        {
            _context = context;
        }

        public async Task<Pagamento?> ObterPorIdAsync(Guid id)
        {
            return await _context.Pagamentos
                .Include(p => p.Contrato)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Pagamento>> ListarPorContratoAsync(Guid contratoId)
        {
            return await _context.Pagamentos
                .Where(p => p.ContratoId == contratoId)
                .OrderBy(p => p.DataVencimento)
                .ToListAsync();
        }

        public async Task<Pagamento?> ObterUltimoPorContratoAsync(Guid contratoId)
        {
            return await _context.Pagamentos
                .Where(p => p.ContratoId == contratoId)
                .OrderByDescending(p => p.DataCriacao)
                .FirstOrDefaultAsync();
        }

        public async Task<Pagamento> CriarAsync(Pagamento pagamento)
        {
            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();
            return pagamento;
        }

        public async Task<Pagamento> AtualizarAsync(Pagamento pagamento)
        {
            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();
            return pagamento;
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null)
                return false;

            _context.Pagamentos.Remove(pagamento);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
