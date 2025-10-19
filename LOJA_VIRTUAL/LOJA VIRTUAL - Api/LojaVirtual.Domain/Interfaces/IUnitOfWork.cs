namespace LojaVirtual.Domain.Interfaces
{
    /// <summary>
    /// Entidade geral de trabalho, onde todo e qualquer repositório pode ser acessado por meio dela.
    /// Isso é uma boa prática que algumas empresas usam.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IProdutoRepository ProdutoRepository { get; }
        IVariacaoProdutoRepository VariacaoProdutoRepository { get; }
        IUsuarioRepository UsuarioRepository { get; }
        // Lembrar de adicionar todos repositórios aqui

        Task<int> CommitAsync();
    }
}
