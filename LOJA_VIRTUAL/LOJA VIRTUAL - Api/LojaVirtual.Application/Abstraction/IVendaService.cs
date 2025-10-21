using LojaVirtual.Application.DTO.Creation;
using LojaVirtual.Application.DTO.Display;
using LojaVirtual.Application.DTO.Query;
using LojaVirtual.Application.DTO.Update;

namespace LojaVirtual.Application.Abstraction
{
    public interface IVendaService
    {

        Task<VendaDisplayDTO?> GetByIdAsync(int id);
        Task<IEnumerable<VendaDisplayDTO>> GetAllAsync(VendaQueryDTO? query);
        Task<VendaDisplayDTO> CreateAsync(VendaCreateDTO dto);
        Task UpdateStatusAsync(int id, AtualizarStatusVendaDTO dto);
        Task CancelAsync(int id);
    }
}
