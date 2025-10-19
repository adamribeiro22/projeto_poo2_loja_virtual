namespace LojaVirtual.Application.DTO.Query
{
    public class EstoqueQueryDTO
    {
        public int? VariacaoProdutoId { get; set; }
        public int? QuantidadeMinima { get; set; }
        public int? QuantidadeMaxima { get; set; }
    }
}
