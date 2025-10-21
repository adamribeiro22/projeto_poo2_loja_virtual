namespace LojaVirtual.Application.DTO.Auth
{
    public class AuthResponseDTO
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string? Token { get; set; }
        public UsuarioDisplayDTO? Usuario { get; set; }
    }
}