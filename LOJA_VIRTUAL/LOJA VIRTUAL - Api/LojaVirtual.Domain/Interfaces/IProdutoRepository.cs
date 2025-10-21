using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Filters;

namespace LojaVirtual.Domain.Interfaces
{
    /// <summary>
    /// Repositório específico para a entidade Produto, ele detem todos métodos implementados em IGenericRepository e o tipo "T" seria o Produto que passamos em "<>"
    /// Além dos métodos genéricos, podemos definir o contrato de métodos específicos que só terá em IProdutoRepository.
    /// Novamente, aqui é apenas a interface que define o contrato, a implementação deve ser feita em ProdutoRepository.cs
    /// </summary>
    public interface IProdutoRepository : IGenericRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetWithFiltersAsync(ProdutoFilter query);
        Task<Produto?> GetByIdWithVariationsAsync(int id);
        Task<bool> HasAssociatedSalesAsync(int produtoId);
    }
}
