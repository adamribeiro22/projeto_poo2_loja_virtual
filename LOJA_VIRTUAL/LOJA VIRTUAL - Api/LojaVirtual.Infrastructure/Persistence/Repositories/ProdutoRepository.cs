using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Filters;
using LojaVirtual.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implementação da interface, definida em IProdutoRepository.cs
    /// Aqui basicamente é onde chega os dados do banco, nesse caso, o método GetWithFiltersAsync ta filtrando esses dados
    /// e armazenando apenas aquele que correspondem aos filtros passados em "ProdutoFilter".
    /// </summary>
    public class ProdutoRepository : GenericRepository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Produto>> GetWithFiltersAsync(ProdutoFilter filter)
        {
            var queryProdutos = _context.Produtos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Nome))
            {
                queryProdutos = queryProdutos.Where(p => p.Nome.Contains(filter.Nome));
            }

            if (!string.IsNullOrWhiteSpace(filter.Categoria))
            {
                queryProdutos = queryProdutos.Where(p => p.Categoria.Contains(filter.Categoria));
            }

            if (filter.Ativo.HasValue)
            {
                queryProdutos = queryProdutos.Where(p => p.Ativo == filter.Ativo.Value);
            }

            return await queryProdutos.ToListAsync();
        }

        public async Task<Produto?> GetByIdWithVariationsAsync(int id)
        {
            return await _context.Produtos
                                 .Include(p => p.Variacoes)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> HasAssociatedSalesAsync(int produtoId)
        {
            return await _context.ItensVenda
                         .AnyAsync(item => item.VariacaoProduto.ProdutoId == produtoId);
        }
    }
}
