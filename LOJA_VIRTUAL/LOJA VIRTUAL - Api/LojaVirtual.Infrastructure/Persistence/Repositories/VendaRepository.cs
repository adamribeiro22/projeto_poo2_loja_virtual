using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Filters;
using LojaVirtual.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Infrastructure.Persistence.Repositories
{
    public class VendaRepository : GenericRepository<Venda>, IVendaRepository
    {
        public VendaRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Venda>> GetWithFiltersAsync(VendaFilter filter)
        {
            var queryVendas = _context.Vendas
                .Include(v => v.Usuario)
                .Include(v => v.Itens)
                    .ThenInclude(i => i.VariacaoProduto)
                        .ThenInclude(vp => vp.Produto)
                .AsQueryable();

            if (filter != null)
            {
                if (filter.UsuarioId.HasValue)
                {
                    queryVendas = queryVendas.Where(v => v.UsuarioId == filter.UsuarioId.Value);
                }

                if (filter.DataVendaDe.HasValue)
                {
                    queryVendas = queryVendas.Where(v => v.DataVenda.Date >= filter.DataVendaDe.Value.Date);
                }

                if (filter.DataVendaAte.HasValue)
                {
                    queryVendas = queryVendas.Where(v => v.DataVenda.Date <= filter.DataVendaAte.Value.Date);
                }

                if (filter.ValorTotalMinimo.HasValue)
                {
                    queryVendas = queryVendas.Where(v => v.ValorTotal >= filter.ValorTotalMinimo.Value);
                }

                if (filter.ValorTotalMaximo.HasValue)
                {
                    queryVendas = queryVendas.Where(v => v.ValorTotal <= filter.ValorTotalMaximo.Value);
                }
            }

            return await queryVendas.ToListAsync();
        }

        public async Task<Venda?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Vendas
                .Include(v => v.Usuario)
                .Include(v => v.Itens)
                    .ThenInclude(i => i.VariacaoProduto)
                        .ThenInclude(vp => vp.Produto)
                .Include(v => v.Itens)
                    .ThenInclude(i => i.VariacaoProduto)
                        .ThenInclude(vp => vp.Estoque)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
