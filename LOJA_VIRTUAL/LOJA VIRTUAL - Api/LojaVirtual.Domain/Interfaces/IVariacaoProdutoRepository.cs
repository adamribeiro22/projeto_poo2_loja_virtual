using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Filters;

namespace LojaVirtual.Domain.Interfaces
{
    public interface IVariacaoProdutoRepository : IGenericRepository<VariacaoProduto>
    {
        Task<IEnumerable<VariacaoProduto>> GetWithFiltersAsync(VariacaoProdutoFilter query);
        Task<List<VariacaoProduto>> GetByIdsWithStockAsync(IEnumerable<int> ids);
    }
}
