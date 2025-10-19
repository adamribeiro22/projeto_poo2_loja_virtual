using LojaVirtual.Domain.Enums;

namespace LojaVirtual.Domain.Entities
{
    public class Venda : AuditableEntity
    {
        public int? UsuarioId { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorTotal { get; set; }
        public StatusVenda Status { get; set; }

        public virtual Usuario? Usuario { get; set; }
        public virtual ICollection<ItemVenda> Itens { get; set; } = new List<ItemVenda>();
    }
}
