using LojaVirtual.Domain.Enums;

namespace LojaVirtual.Domain.Entities
{
    public class Usuario : AuditableEntity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }
        public TipoUsuario Tipo { get; set; }

        public virtual ICollection<Venda> Vendas { get; set; } = new List<Venda>();
    }
}
