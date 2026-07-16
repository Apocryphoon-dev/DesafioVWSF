using DesafioVWFS.Application.Shared.Domain.Entities;
using DesafioVWFS.Application.Shared.Domain.Repositories;
using DesafioVWFS.Data;
using Microsoft.EntityFrameworkCore;

namespace DesafioVWFS.Repositories
{
    public class ContratoRepository : IContratoRepository
    {
        private readonly DesafioDbContext _context;

        public ContratoRepository(DesafioDbContext context)
        {
            _context = context;
        }

        public async Task<Contrato?> ObterPorIdAsync(Guid id)
        {
            return await _context.Contratos
                .Include(c => c.Pagamentos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Contrato>> ObterPorClienteAsync(string cpfCnpj)
        {
            return await _context.Contratos
                .Include(c => c.Pagamentos)
                .Where(c => c.ClienteCpfCnpj == cpfCnpj && c.Ativo)
                .ToListAsync();
        }

        public async Task<List<Contrato>> ListarTodosAsync(int page = 1, int pageSize = 10, bool ordenarDescendente = false)
        {
            var query = _context.Contratos
                .Include(c => c.Pagamentos)
                .Where(c => c.Ativo);

            if (ordenarDescendente)
                query = query.OrderByDescending(c => c.DataCriacao);
            else
                query = query.OrderBy(c => c.DataCriacao);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Contrato> CriarAsync(Contrato contrato)
        {
            _context.Contratos.Add(contrato);
            await _context.SaveChangesAsync();
            return contrato;
        }

        public async Task<Contrato> AtualizarAsync(Contrato contrato)
        {
            contrato.DataAtualizacao = DateTime.UtcNow;
            _context.Contratos.Update(contrato);
            await _context.SaveChangesAsync();
            return contrato;
        }

        public async Task<bool> DeletarAsync(Guid id)
        {
            var contrato = await _context.Contratos.FindAsync(id);
            if (contrato == null)
                return false;

            contrato.Ativo = false;
            await AtualizarAsync(contrato);
            return true;
        }

        public async Task<int> ContarTotalAsync()
        {
            return await _context.Contratos.CountAsync(c => c.Ativo);
        }
    }
}
