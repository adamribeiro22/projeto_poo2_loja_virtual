using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Filters;

namespace LojaVirtual.Domain.Interfaces
{
    public interface IEstoqueRepository : IGenericRepository<Estoque>
    {
        Task<IEnumerable<Estoque>> GetWithFiltersAsync(EstoqueFilter query);
    }
}
