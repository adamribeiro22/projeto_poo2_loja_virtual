using FluentValidation;
using LojaVirtual.Application.DTO.Update;
using LojaVirtual.Domain.Enums;

namespace LojaVirtual.Application.DTO.Validators
{
    public class AtualizarStatusVendaDTOValidator : AbstractValidator<AtualizarStatusVendaDTO>
    {
        public AtualizarStatusVendaDTOValidator()
        {
            RuleFor(x => x.NovoStatus)
                .NotEmpty().WithMessage("O status fornecido não pode ser nulo.");
        }
    }
}
