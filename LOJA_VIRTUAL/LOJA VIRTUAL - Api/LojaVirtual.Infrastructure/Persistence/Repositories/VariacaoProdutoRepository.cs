using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Filters;
using LojaVirtual.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Infrastructure.Persistence.Repositories
{
    public class VariacaoProdutoRepository : GenericRepository<VariacaoProduto>, IVariacaoProdutoRepository
    {
        public VariacaoProdutoRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<VariacaoProduto>> GetWithFiltersAsync(VariacaoProdutoFilter filter)
        {
            var queryVariacoes = _context.VariacoesProduto.AsQueryable();

            if (filter.ProdutoId.HasValue)
            {
                queryVariacoes = queryVariacoes.Where(v => v.ProdutoId == filter.ProdutoId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Tamanho))
            {
                queryVariacoes = queryVariacoes.Where(v => v.Tamanho != null && v.Tamanho.Contains(filter.Tamanho));
            }

            if (!string.IsNullOrWhiteSpace(filter.Cor))
            {
                queryVariacoes = queryVariacoes.Where(v => v.Cor != null && v.Cor.Contains(filter.Cor));
            }

            if (filter.PrecoMinimo.HasValue)
            {
                queryVariacoes = queryVariacoes.Where(v => v.Preco >= filter.PrecoMinimo.Value);
            }

            if (filter.PrecoMaximo.HasValue)
            {
                queryVariacoes = queryVariacoes.Where(v => v.Preco <= filter.PrecoMaximo.Value);
            }

            if (filter.Ativo.HasValue)
            {
                queryVariacoes = queryVariacoes.Where(v => v.Ativo == filter.Ativo.Value);
            }

            return await queryVariacoes.ToListAsync();
        }

        public async Task<List<VariacaoProduto>> GetByIdsWithStockAsync(IEnumerable<int> ids)
        {
            return await _context.VariacoesProduto
                                 .Include(vp => vp.Estoque)
                                 .Where(vp => ids.Contains(vp.Id))
                                 .ToListAsync();
        }
    }
}
