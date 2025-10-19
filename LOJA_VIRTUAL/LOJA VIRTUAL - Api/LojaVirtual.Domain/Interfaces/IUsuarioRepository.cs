using LojaVirtual.Domain.Entities;

namespace LojaVirtual.Domain.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
    }
}