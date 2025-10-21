namespace LojaVirtual.Domain.Entities
{
    public class ItemVenda : AuditableEntity
    {
        public int VendaId { get; set; }
        public int VariacaoProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        public virtual Venda Venda { get; set; }
        public virtual VariacaoProduto VariacaoProduto { get; set; }
    }
}
