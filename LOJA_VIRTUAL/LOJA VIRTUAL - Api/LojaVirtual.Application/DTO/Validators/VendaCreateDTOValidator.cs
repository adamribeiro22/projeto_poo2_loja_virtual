using FluentValidation;
using LojaVirtual.Application.DTO.Creation;

namespace LojaVirtual.Application.DTO.Validators
{
    public class VendaCreateDTOValidator : AbstractValidator<VendaCreateDTO>
    {
        public VendaCreateDTOValidator()
        {
            RuleFor(x => x.UsuarioId)
                .GreaterThan(0).WithMessage("O ID do utilizador é obrigatório.");

            RuleFor(x => x.Itens)
                .NotEmpty().WithMessage("A venda deve conter pelo menos um item.");

            RuleForEach(x => x.Itens)
                .SetValidator(new ItemVendaCreateDTOValidator());
        }
    }
}
