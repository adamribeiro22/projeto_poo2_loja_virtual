namespace LojaVirtual.Domain.Filters
{
    /// <summary>
    /// Filtro para produtos.
    /// </summary>
    public class ProdutoFilter
    {
        public string? Nome { get; set; }
        public string? Categoria { get; set; }
        public bool? Ativo { get; set; }
    }
}
