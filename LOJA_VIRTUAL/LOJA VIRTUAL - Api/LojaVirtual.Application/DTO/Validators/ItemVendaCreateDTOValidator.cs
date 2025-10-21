using FluentValidation;
using LojaVirtual.Application.DTO.Creation;

namespace LojaVirtual.Application.DTO.Validators
{
    public class ItemVendaCreateDTOValidator : AbstractValidator<ItemVendaCreateDTO>
    {
        public ItemVendaCreateDTOValidator()
        {
            RuleFor(x => x.VariacaoProdutoId)
                .GreaterThan(0).WithMessage("O ID da variação do produto é obrigatório para cada item.");

            RuleFor(x => x.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade de cada item deve ser maior que zero.");
        }
    }
}
