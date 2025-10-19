using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Filters;
using LojaVirtual.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtual.Infrastructure.Persistence.Repositories
{
    public class EstoqueRepository : GenericRepository<Estoque>, IEstoqueRepository
    {
        public EstoqueRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Estoque>> GetWithFiltersAsync(EstoqueFilter filter)
        {
            var queryEstoque = _context.Estoques.AsQueryable();

            if (filter.VariacaoProdutoId.HasValue)
            {
                queryEstoque = queryEstoque.Where(e => e.VariacaoProdutoId == filter.VariacaoProdutoId.Value);
            }

            if (filter.QuantidadeMinima.HasValue)
            {
                queryEstoque = queryEstoque.Where(e => e.Quantidade >= filter.QuantidadeMinima);
            }

            if (filter.QuantidadeMaxima.HasValue)
            {
                queryEstoque = queryEstoque.Where(e => e.Quantidade <= filter.QuantidadeMaxima);
            }

            return await queryEstoque.ToListAsync();
        }
    }
}
