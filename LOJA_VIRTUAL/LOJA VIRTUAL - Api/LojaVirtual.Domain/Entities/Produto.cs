namespace LojaVirtual.Domain.Entities
{
    public class Produto : AuditableEntity
    {
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<VariacaoProduto> Variacoes { get; set; } = new List<VariacaoProduto>();
    }
}
