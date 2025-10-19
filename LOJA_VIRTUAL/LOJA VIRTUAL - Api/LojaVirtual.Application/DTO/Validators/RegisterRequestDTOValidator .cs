using FluentValidation;
using LojaVirtual.Application.DTO.Auth;
using LojaVirtual.Domain.Interfaces;

namespace LojaVirtual.Application.DTO.Validators
{
    public class RegisterRequestDTOValidator : AbstractValidator<RegisterRequestDTO>
    {
        public RegisterRequestDTOValidator(IUsuarioRepository usuarioRepository)
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .Length(3, 150).WithMessage("O nome deve ter entre 3 e 150 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("O formato do e-mail é inválido.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.");
        }
    }
}
