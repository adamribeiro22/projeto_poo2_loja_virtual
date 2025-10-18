using LojaVirtual.Domain.Interfaces;

namespace LojaVirtual.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implementação de IUnitOfWork, onde todos os repositórios serão acessados por meio dela.
    /// lembrando que, commitassync ele de fato salva as alterações feitas no banco, verão que nos services iremos usa-lo direto.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IProdutoRepository ProdutoRepository { get; }
        // Lembrar de adicionar todos IRepository exatamente aqui

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            ProdutoRepository = new ProdutoRepository(_context);
            // Lembrar de adicionar todos repositórios aqui
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            // Fecha a conexão com o banco de dados e libera os recursos do Garbage Collector (GC)
            _context.Dispose();
        }
    }
}
