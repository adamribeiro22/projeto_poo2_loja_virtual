using FluentValidation;
using LojaVirtual.Application.DTO.Creation;

namespace LojaVirtual.Application.DTO.Validators
{
    /// <summary>
    /// Aqui fazemos a validação de "ProdutoCreateDTO" por meio do framework FluentValidator.
    /// Então, quando fazemos uma requisição do controllador "ProdutoController" para criar um produto, o FluentValidator irá validar os dados de entrada
    /// antes mesmo de executar o endpoint de criação, ou qualquer outro que usar "ProdutoCreateDTO" como parâmetro. (Update por exemplo.)
    /// </summary>
    public class VariacaoProdutoCreateDTOValidator : AbstractValidator<VariacaoProdutoCreateDTO>
    {
        public VariacaoProdutoCreateDTOValidator()
        {
            RuleFor(x => x.ProdutoId)
                .GreaterThan(0).WithMessage("O ID do produto é obrigatório.");

            RuleFor(x => x.Preco)
                .GreaterThanOrEqualTo(0).WithMessage("O preço não pode ser negativo.");

            RuleFor(x => x.QuantidadeEstoqueInicial)
            .GreaterThanOrEqualTo(0).WithMessage("A quantidade inicial de estoque não pode ser negativa.");

            RuleFor(x => x.Tamanho)
                .MaximumLength(50).WithMessage("O tamanho não pode exceder 50 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Tamanho));

            RuleFor(x => x.Cor)
                .MaximumLength(50).WithMessage("A cor não pode exceder 50 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Cor));

            RuleFor(x => x.ImagemUrl)
                .MaximumLength(500).WithMessage("A URL da imagem deve ter no máximo 500 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.ImagemUrl));

            RuleFor(x => x)
                .Must(dto => !string.IsNullOrWhiteSpace(dto.Tamanho) || !string.IsNullOrWhiteSpace(dto.Cor))
                .WithMessage("A variação deve ter, no mínimo, um tamanho ou uma cor especificada.");
        }
    }
}
