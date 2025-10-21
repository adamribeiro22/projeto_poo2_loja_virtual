using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Display;
using LojaVirtual.Application.DTO.Query;

namespace LojaVirtual.Application.Abstraction
{
    public interface IVariacaoProdutoService
    {
        Task<VariacaoProdutoDisplayDTO?> GetByIdAsync(int id);
        Task<IEnumerable<VariacaoProdutoDisplayDTO>> GetAllAsync(VariacaoProdutoQueryDTO? query);
        Task<VariacaoProdutoDisplayDTO> CreateAsync(VariacaoProdutoCreateDTO criarVariacaoProdutoDto);
        Task UpdateAsync(int id, VariacaoProdutoCreateDTO atualizarVariacaoProdutoDto);
        Task DeleteAsync(int id);
        Task CancelAsync(int id);
    }
}
