using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequestDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.Sucesso)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]     
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.Sucesso)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
    }
}
