namespace LojaVirtual.Domain.Entities
{
    public class VariacaoProduto : AuditableEntity
    {
        public int ProdutoId { get; set; }
        public string? Tamanho { get; set; }
        public string? Cor { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual Estoque Estoque { get; set; }
        public virtual ICollection<ItemVenda> ItensVenda { get; set; } = new List<ItemVenda>();
    }
}
