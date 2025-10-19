namespace LojaVirtual.Application.DTO.Query
{
    /// <summary>
    /// DTO que representa a filtragem dos dados de produtos.
    /// </summary>
    public class VariacaoProdutoQueryDTO
    {
        public int? ProdutoId { get; set; }
        public string? Tamanho { get; set; }
        public string? Cor { get; set; }
        public decimal? PrecoMinimo { get; set; }
        public decimal? PrecoMaximo { get; set; }
        public bool? Ativo { get; set; }
    }
}
