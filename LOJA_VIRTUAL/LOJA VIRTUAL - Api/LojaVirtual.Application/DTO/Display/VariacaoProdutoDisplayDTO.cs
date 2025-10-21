namespace LojaVirtual.Application.DTO.Display
{
    /// <summary>
    /// DTO para exibição de dados (levar do banco pro front e exibir)
    /// </summary>
    public class VariacaoProdutoDisplayDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string? Tamanho { get; set; }
        public string? Cor { get; set; }
        public decimal Preco { get; set; }
        public bool Ativo { get; set; }

        public EstoqueDisplayDTO Estoque { get; set; }
    }
}
