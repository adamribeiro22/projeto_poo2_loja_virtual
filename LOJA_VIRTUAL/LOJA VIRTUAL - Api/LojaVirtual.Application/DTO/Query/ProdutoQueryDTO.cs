namespace LojaVirtual.Application.DTO.Query
{
    /// <summary>
    /// DTO que representa a filtragem dos dados de produtos.
    /// </summary>
    public class ProdutoQueryDTO
    {
        public string? Nome { get; set; }
        public string? Categoria { get; set; }
        public bool? Ativo { get; set; }
    }
}
