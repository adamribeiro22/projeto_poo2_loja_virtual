using AutoMapper;
using LojaVirtual.Application.Abstraction;
using LojaVirtual.Application.DTO.Auth;
using LojaVirtual.Domain.Entities;
using LojaVirtual.Domain.Enums;
using LojaVirtual.Domain.Interfaces;
using LojaVirtual.Infrastructure.Helper.Atelie.Core.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LojaVirtual.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO dto)
        {
            var usuarioExistente = await _unitOfWork.UsuarioRepository.GetByEmailAsync(dto.Email);
            if (usuarioExistente != null)
            {
                return new AuthResponseDTO { Sucesso = false, Mensagem = "Este e-mail já está em uso." };
            }

            var novoUsuario = _mapper.Map<Usuario>(dto);

            novoUsuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);
            novoUsuario.Tipo = TipoUsuario.Comum;

            AuditHelper.UpdateAuditFields(novoUsuario);

            await _unitOfWork.UsuarioRepository.CreateAsync(novoUsuario);
            await _unitOfWork.CommitAsync();

            return await LoginAsync(new LoginRequestDTO { Email = dto.Email, Senha = dto.Senha });
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO dto)
        {
            var usuario = await _unitOfWork.UsuarioRepository.GetByEmailAsync(dto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            {
                return new AuthResponseDTO { Sucesso = false, Mensagem = "E-mail ou senha inválidos." };
            }

            var token = GenerateJwtToken(usuario);

            var usuarioDto = _mapper.Map<UsuarioDisplayDTO>(usuario);

            return new AuthResponseDTO { Sucesso = true, Token = token, Usuario = usuarioDto, Mensagem = "Login bem-sucedido." };
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Tipo.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}