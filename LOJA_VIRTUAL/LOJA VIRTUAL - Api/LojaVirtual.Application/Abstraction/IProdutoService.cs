using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Display;
using LojaVirtual.Application.DTO.Query;
using LojaVirtual.Application.DTO.Update;

namespace LojaVirtual.Application.Abstraction
{
    /// <summary>
    /// Interface para ProdutoService, define os métodos que ele deve implementar OBRIGATORIAMENTE.
    /// </summary>
    public interface IProdutoService
    {
        Task<ProdutoDisplayDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ProdutoDisplayDTO>> GetAllAsync(ProdutoQueryDTO? query);
        Task<IEnumerable<ProdutoDisplayDTO>> GetAllAsyncWithDetails(ProdutoQueryDTO? query);
        Task<ProdutoDisplayDTO> CreateAsync(ProdutoCreateDTO criarProdutoDto);
        Task UpdateAsync(int id, ProdutoUpdateDTO atualizarProdutoDto);
        Task DeleteAsync(int id);
        Task CancelAsync(int id);
    }
}
