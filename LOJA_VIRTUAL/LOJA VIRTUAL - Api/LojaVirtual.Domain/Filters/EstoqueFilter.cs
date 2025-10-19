namespace LojaVirtual.Domain.Filters
{
    public class EstoqueFilter
    {
        public int? VariacaoProdutoId { get; set; }
        public int? QuantidadeMinima { get; set; }
        public int? QuantidadeMaxima { get; set; }
    }
}
