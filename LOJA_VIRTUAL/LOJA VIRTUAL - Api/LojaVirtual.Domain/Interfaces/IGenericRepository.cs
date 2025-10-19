using System.Linq.Expressions;

namespace LojaVirtual.Domain.Interfaces
{
    /// <summary>
    /// Repositório genérico para que todos repositórios referentes a classes que herdam de AuditableEntity possam usar este repositório como base de herança.
    /// Lembrando que é apenas uma interface, define o contrato para a implementação em GenerecRepository.cs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : AuditableEntity
    {
        Task CreateAsync(T entity);
        Task CreateRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes); // Permitem parâmetros de buscas, incluindo dados sobre entidades relacionadas
        Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);   
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
    }
}
