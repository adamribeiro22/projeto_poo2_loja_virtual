namespace LojaVirtual.Domain.Entities
{
    public class Estoque : AuditableEntity
    {
        public int VariacaoProdutoId { get; set; }
        public int Quantidade { get; set; }

        public virtual VariacaoProduto VariacaoProduto { get; set; }
    }
}