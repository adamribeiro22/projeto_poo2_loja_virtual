using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Filters;

namespace LojaVirtual.Domain.Interfaces
{
    public interface IVendaRepository : IGenericRepository<Venda>
    {
        Task<IEnumerable<Venda>> GetWithFiltersAsync(VendaFilter query);
        Task<Venda?> GetByIdWithDetailsAsync(int id);
    }
}
