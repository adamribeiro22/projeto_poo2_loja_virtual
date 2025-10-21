using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Display;
using LojaVirtual.Application.DTO.Query;

namespace LojaVirtual.Application.Abstraction
{
    /// <summary>
    /// Interface para ProdutoService, define os métodos que ele deve implementar OBRIGATORIAMENTE.
    /// </summary>
    public interface IProdutoService
    {
        Task<ProdutoDisplayDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ProdutoDisplayDTO>> GetAllAsync(ProdutoQueryDTO? query);
        Task<ProdutoDisplayDTO> CreateAsync(ProdutoCreateDTO criarProdutoDto);
        Task UpdateAsync(int id, ProdutoCreateDTO atualizarProdutoDto);
        Task DeleteAsync(int id);
        Task CancelAsync(int id);
    }
}
