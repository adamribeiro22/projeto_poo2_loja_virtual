namespace LojaVirtual.Domain.Filters
{
    /// <summary>
    /// Filtro para produtos.
    /// </summary>
    public class VariacaoProdutoFilter
    {
        public int? ProdutoId { get; set; }
        public string? Tamanho { get; set; }
        public string? Cor { get; set; }
        public decimal? PrecoMinimo { get; set; }
        public decimal? PrecoMaximo { get; set; }
        public bool? Ativo { get; set; }
    }
}
