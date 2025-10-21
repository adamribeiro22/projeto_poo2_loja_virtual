namespace LojaVirtual.Application.DTO.Creation
{
    /// <summary>
    /// DTO para criação de um produto.
    /// </summary>
    public class VariacaoProdutoCreateDTO
    {
        public int ProdutoId { get; set; }
        public string? Tamanho { get; set; }
        public string? Cor { get; set; }
        public decimal Preco { get; set; }
        public int QuantidadeEstoqueInicial { get; set; } = 0;
    }

}
