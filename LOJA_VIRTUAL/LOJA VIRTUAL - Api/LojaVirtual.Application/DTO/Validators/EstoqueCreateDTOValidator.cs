using FluentValidation;
using LojaVirtual.Application.DTO.Creation;

namespace LojaVirtual.Application.DTO.Validators
{
    public class EstoqueCreateDTOValidator : AbstractValidator<EstoqueCreateDTO>
    {
        public EstoqueCreateDTOValidator()
        {
            RuleFor(x => x.VariacaoProdutoId)
                .GreaterThan(0).WithMessage("O ID do produto é obrigatório.");

            RuleFor(x => x.Quantidade)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade não pode ser negativa.");
        }
    }
}
