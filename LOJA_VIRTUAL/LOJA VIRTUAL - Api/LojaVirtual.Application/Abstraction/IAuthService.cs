using LojaVirtual.Application.DTO.Auth;

namespace LojaVirtual.Application.Abstraction
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO dto);
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO dto);
    }
}
