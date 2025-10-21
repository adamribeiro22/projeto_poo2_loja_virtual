using LojaVirtual.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using LojaVirtual.Domain.Interfaces;

namespace LojaVirtual.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context) { }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
