namespace LojaVirtual.Application.DTO.Display
{
    /// <summary>
    /// DTO para exibição de dados (levar do banco pro front e exibir)
    /// </summary>
    public class ProdutoDisplayDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
        public bool Ativo { get; set; }

        public List<VariacaoProdutoDisplayDTO> Variacoes { get; set; }
    }
}
