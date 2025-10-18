using FluentValidation;
using LojaVirtual.Application.DTO.Creation;

namespace LojaVirtual.Application.DTO.Validators
{
    /// <summary>
    /// Aqui fazemos a validação de "ProdutoCreateDTO" por meio do framework FluentValidator.
    /// Então, quando fazemos uma requisição do controllador "ProdutoController" para criar um produto, o FluentValidator irá validar os dados de entrada
    /// antes mesmo de executar o endpoint de criação, ou qualquer outro que usar "ProdutoCreateDTO" como parâmetro. (Update por exemplo.)
    /// </summary>
    public class ProdutoCreateDTOValidator : AbstractValidator<ProdutoCreateDTO>
    {
        public ProdutoCreateDTOValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(3, 150).WithMessage("O nome deve ter entre 3 e 150 caracteres.");

            RuleFor(x => x.Descricao)
                .MaximumLength(1000).WithMessage("A descrição não pode exceder 1000 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Descricao));

            RuleFor(x => x.Categoria)
                .MaximumLength(100).WithMessage("A categoria não pode exceder 100 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Categoria));
        }
    }
}
