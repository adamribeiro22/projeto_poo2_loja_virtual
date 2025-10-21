using LojaVirtual.Application.DTO.Query;
using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Display;

namespace LojaVirtual.Application.Abstraction
{
    public interface IEstoqueService
    {
        Task<EstoqueDisplayDTO?> GetByIdAsync(int id);
        Task<IEnumerable<EstoqueDisplayDTO>> GetAllAsync(EstoqueQueryDTO? query);
        Task<EstoqueDisplayDTO> CreateAsync(EstoqueCreateDTO criarEstoqueDto);
        Task UpdateAsync(int id, EstoqueCreateDTO atualizarProdutoDto);
        Task DeleteAsync(int id);
    }
}
