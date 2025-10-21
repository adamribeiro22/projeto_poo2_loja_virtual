using LojaVirtual.Application.DTO.Display;

namespace LojaVirtual.Application.DTO.Creation
{
    /// <summary>
    /// DTO para criação de um produto.
    /// </summary>
    public class ProdutoCreateDTO
    {
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public string? Categoria { get; set; }
        public List<VariacaoProdutoCreateDTO> Variacoes { get; set; }
    }
}
